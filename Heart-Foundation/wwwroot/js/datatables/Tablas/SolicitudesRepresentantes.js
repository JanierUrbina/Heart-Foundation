window.onload = function () {
    call();
};
function call() {
    HoldOn.open({ theme: 'sk-circle', message: 'Espere un momento...' });
    fetch("/Representante/MisSolicitudesJSON",
        {
            method: 'GET',
            headers:
            {
                'Content-Type': 'application/json'
            }
        }).then(response => response.json())
        .then(data => {
            console.log(data);
            data.forEach(item => {
                const fecha = new Date(item.fechaCreacion);
                item.fechaCreacion = fecha.toLocaleString('es-ES');
            });
            HoldOn.close();
            var table = $('#miTablaS').DataTable(
                {
                    "deferRender": true,
                    select: true,
                    responsive: true,
                    autoWidth: false,
                    bPaginate: true,
                    "ordering": true,
                    "colReorder": true,
                    dom: 'Blfrtip',
                    data: data,
                    columns:
                        [
                            { data: "nombreCompleto", name: "nombreCompleto", autoWidth: true },
                            {
                                data: null,
                                name: "estadoSolicitud",
                                autoWidth: true,
                                render: function (data, type, row) {
                                    switch (data.estadoSolicitud) {
                                        case 0:
                                            return 'Solicitado';
                                        case 1:
                                            return 'Rechazado';
                                        case 2:
                                            return 'Proceso';
                                        case 3:
                                            return 'Finalizado';
                                        default:
                                            return 'Desconocido';
                                    }
                                }
                            },
                            { data: "fechaCreacion", name: "fechaCreacion", autoWidth: true },
                            {
                                data: null,
                                render: function (data, type, row) {
                                    var id = row.id;

                                    var buttonsHtml = '<div class="btn-group" role="group">';
                                    buttonsHtml += '<button  class="btn note-button" data-id="' + id + '" data-action="AgregarNota" title="Notificar"><i class="fa-solid fa-envelope"></i></button>';
                                    buttonsHtml += '<button  class="btn watchexpediente-button" data-id="' + id + '" title="Ver Expediente"><i class="fas fa-file-alt"></i></button>';
                                    buttonsHtml += '<button  class="btn note-button" data-id="' + id + '" data-action="VerNotas" title="Ver Notas"><i class="fa-solid fa-comment-dots"></i></button>';
                                    buttonsHtml += '<button  class="btn sol-button" data-id="' + id + '" title="Solicitud"><i class="fa-solid fa-clipboard-check"></i></button>';
                                    buttonsHtml += '</div>';

                                    return buttonsHtml;
                                }
                            },

                        ],
                    createdRow: function (row, data, dataIndex) {
                        $(row).addClass('row-with-buttons');
                    },
                    language: {

                        "url": "../lib/spanish.json"
                    },
                });
            $('.dataTables-List').on('click', '.watchexpediente-button', function () {
                var id = $(this).data('id');
                openimages(id);
            });

            $('.dataTables-List').on('click', '.note-button', function () {
                var id = $(this).data('id');
                var action = $(this).data('action');
                openmodal(id, action);
            });

            $('.dataTables-List').on('click', '.sol-button', function () {
                var id = $(this).data('id');
                $('.collapse').collapse();
                openmodal(id, "ModalSolicitud");
            });


        }).catch(error => {
            console.error(error);
        });
}
const listbase64 = [];
function openimages(id) {
    HoldOn.open({ theme: 'sk-circle', message: 'Cargando imágenes...' });
    listbase64.length = 0; 
    $.ajax({
        type: "POST",
        processData: false,
        contentType: false,
        url: "/Solicitudes/CargarImagenes?id=" + id,
        dataType: "JSON",
        success: function (response) {
            if (response.exito) {
                let img = document.getElementById("miImagen");
                console.log(response.imagenes);
                img.src = "data:image/png;base64," + response.imagenes[0].imagenBase64;
                document.getElementById("nota").textContent = response.imagenes[0].nota;
                HoldOn.close();
                
                response.imagenes.forEach(function (i) {
                    listbase64.push({
                        imagenBase64: i.imagenBase64,
                        nota: i.nota
                        });
                });
                var max = (listbase64.length - 1);
                if (max > 0) {
                    $('#changerid').removeAttr('disabled', "disabled");
                    changerid.style.display = "";
                }
                else {
                    changerid.style.display = "none";
                }

                $('#modalimages').modal('show');

            } else {
                HoldOn.close();
                MostrarSweetAlert(response.message, "warning");
            }
           
        },
        error: function (xhr, status, error) {
            // Manejar cualquier error que ocurra durante la solicitud AJAX
            console.error("Error al enviar los datos:", error);
            HoldOn.close();
            MostrarSweetAlert(error.message, "error");
        }
    });

}

function changeref(move) {
    var currentpos = $("#prev").data("indexp");
    var max = (listbase64.length - 1);
    var newpos = currentpos + move;
    var valpos = 0;
    if (newpos <= 0) {
        $("#prev").data("indexp", 0);
        $("#prev").prop("disabled", "disabled");
        $("#next").removeAttr("disabled");
        valpos = 0;
    }
    else if (newpos >= max) {
        $("#prev").data("indexp", max);
        $("#next").prop("disabled", "disabled");
        $("#prev").removeAttr("disabled");
        valpos = max;
    }
    else {
        $("#prev").removeAttr("disabled");
        $("#next").removeAttr("disabled");
        $("#prev").data("indexp", newpos);
        valpos = newpos;
    }
    HoldOn.open({ theme: 'sk-circle', message: 'Cargando imagen...' });
    let img = document.getElementById("miImagen");
    img.src = "data:image/png;base64," + listbase64[valpos].imagenBase64;
    document.getElementById("nota").textContent = listbase64[valpos].nota;
    HoldOn.close();
}

function openmodal(id, action) {
    $("#modalGrl").modal('show');
    $.ajax({
        type: "GET",
        url: "/Solicitudes/" + action + "?IdSolicitud=" + id,
        contentType: false,
        processData: false,
        cache: false,
        async: false,
        dataType: "html",
        success: function (data) {
            $("#modalcontentGrl").html(data);
            HoldOn.close();
        },
        error: function (data) {
            console.log("Error al llamar el modal del requisito: " + data);
        }
    });
}