
window.onload = function () {
    call();
};
function call() {
    HoldOn.open({ theme: 'sk-circle', message: 'Espere un momento...' });
    fetch("/Solicitudes/IndexJSON",
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
                                    if (row.estadoSolicitud == 0) {
                                        buttonsHtml += '<button  class="btn addexpediente-button" data-id="' + id + '" title="Adjuntar Expediente"><i class="fas fa-folder-plus"></i></button>';
                                        buttonsHtml += '<button  class="btn edit-button" data-id="' + id + '" title="Editar"><i class="fa fa-edit"></i></button>';
                                    }
                                    buttonsHtml += '<button  class="btn watchexpediente-button" data-id="' + id + '" title="Ver Expediente"><i class="fas fa-file-alt"></i></button>';
                                    buttonsHtml += '<button  class="btn note-button" data-id="' + id + '" title="Ver Notas"><i class="fa-solid fa-comment-dots"></i></button>';
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
            // Manejar el evento de clic en el botón de deshabilitar
            $('.dataTables-List').on('click', '.addexpediente-button', function () {
                var id = $(this).data('id');
                putid(id);
            });

            $('.dataTables-List').on('click', '.watchexpediente-button', function () {
                var id = $(this).data('id');
                openimages(id);
            });

            $('.dataTables-List').on('click', '.edit-button', function () {
                var id = $(this).data('id');
                window.location.href = '/Solicitudes/Solicitud?id=' + id;
            });

            $('.dataTables-List').on('click', '.note-button', function () {
                var id = $(this).data('id');
                $("#modalnotas").modal('show');
                $.ajax({
                    type: "GET",
                    url: "/Solicitudes/VerNotas?IdSolicitud=" + id,
                    contentType: false,
                    processData: false,
                    cache: false,
                    async: false,
                    dataType: "html",
                    success: function (data) {
                        $("#modalcontentnotas").html(data);
                        HoldOn.close();
                    },
                    error: function (data) {
                        console.log("Error al llamar el modal del requisito: " + data);
                    }
                });
            });

        }).catch(error => {
            console.error(error);
        });
}

var selectedId;


function putid(id) {
    selectedId = id;
    $('#adjutar').modal('show');
}

//--------------------------------- AÑADIR EXPEDIENTE -------------------------------------------

var tbody = document.getElementById('tbodyadjuntos');
function addform() {

    var newRow = document.createElement('tr');
    var cells = '';

    cells += '<td><div class="input-field col s12">';
    cells += '<label>Archivo de Expediente</label></div></td>'; 
    cells += '<input class="form-control validate file" type="file", multiple = "multiple" onchange="addfile(this)" accept="image/*" />';
    cells += '<label>Nota</label></div></td>';   
    cells += '<input class="form-control validate nota" type="text" />';
    cells += '<td><button onclick="remover(this)" type="button" class="btn btn-danger right btnQuitar">Quitar</button></td>';

    newRow.innerHTML = cells;
    tbody.appendChild(newRow);  
    // Asignar data-index a la nueva fila
    var index = allfiles.length;
    newRow.setAttribute('data-index', index);
}
const allfiles = [];
const allnotas = [];

function remover(button) {
    var row = button.parentNode.parentNode;
    var fileInput = row.querySelector('input[type="file"]');
    var indexToRemove = row.getAttribute('data-index');

    // Verificar si hay un archivo asociado antes de eliminarlo de allfiles
    if (fileInput.files.length > 0 && allfiles[indexToRemove]) {
        allfiles.splice(indexToRemove, 1); // Elimina el archivo de allfiles
    }

    row.parentNode.removeChild(row);
}


function addfile(file) {
    console.log('Este es mi file: ' + file);
    if (file.files != null) {
        allfiles.push(file.files[0]); //añade el archivo actual
    }
}

function add() {
    var formData = new FormData();
    
    if (contarFilasConCamposLlenos()) {
        allfiles.forEach(function (file) {
            formData.append('files', file);
        });

        formData.append('notas', JSON.stringify(allnotas));

        // Enviar los datos de los detalles al servidor mediante AJAX
        $.ajax({
            type: "POST",
            processData: false,
            contentType: false,
            data: formData,
            url: "/Solicitudes/GuardarAdjuntos",
            dataType: "JSON",
            success: function (response) {
                if (response.message != null) {
                    if (response.exito) {
                        MostrarSweetAlert(response.message, "success");
                    } else {
                        MostrarSweetAlert(response.message, "warning");
                    }
                }
            },
            error: function (xhr, status, error) {
                // Manejar cualquier error que ocurra durante la solicitud AJAX
                console.error("Error al enviar los datos:", error);
                MostrarSweetAlert(error.message, "error");
            }
        });
    }
    else {
        MostrarSweetAlert("Campos Incompletos", "warning");
    }
   

}

// Función para verificar si todos los campos de una fila están llenos
function contarFilasConCamposLlenos() {
    var TodasLasFilas = document.querySelectorAll('#tbodyadjuntos tr');   
    var TodoBien = true;
    TodasLasFilas.forEach(function (fila) {
        var CamposLlenos = verificarCamposLlenos(fila);
        if (!CamposLlenos) {
            MostrarSweetAlert("Campos incompletos en la fila " + fila.rowIndex, "warning", function () { $("#modalestacionmovil").modal("open"); });
            TodoBien = false;
        }        
    });
    return TodoBien;
}

function verificarCamposLlenos(row) {
    var fila = row.querySelectorAll('.validate'); // Obtener todos los elementos de validación en la fila

    var camposLlenos = true;
    for (var i = 0; i < fila.length; i++) {
        if (fila[i].value.trim() === '') { // Verificar si algún campo está vacío
            camposLlenos = false;
            break;
        }
        var obj =
        {
            nota: fila[1].value,
            idSolicitud: selectedId           
        }; //nuevo objeto

    }
    allnotas.push(obj);
    return camposLlenos;
}



//--------------------------------- IMAGENES -------------------------------------------
function openimages(id) {
    process(id);
}
const listbase64 = [];
function process(id) {
    HoldOn.open({ theme: 'sk-circle', message: 'Cargando imágenes...' });
    listbase64.length = 0; 

    $.ajax({
        type: "POST",
        processData: false,
        contentType: false,
        url: "/Solicitudes/CargarImagenes?id="+id,
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




