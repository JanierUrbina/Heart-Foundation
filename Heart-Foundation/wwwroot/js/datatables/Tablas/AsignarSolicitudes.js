window.onload = function () {
    call();
    callSA();
};
function call() {
    HoldOn.open({ theme: 'sk-circle', message: 'Espere un momento...' });
    fetch("/Solicitudes/AsignarSolicitudesJSON",
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
                                    buttonsHtml += '<button  class="btn note-button" data-id="' + id + '" data-action="VerNotas" title="Ver Notas"><i class="fa-solid fa-comment-dots"></i></button>';
                                    buttonsHtml += '<button  class="btn ar-button" data-id="' + id + '" title="Asignar Representante"><i class="fa-solid fa-user-check"></i></button>';
                                    buttonsHtml += '<button  class="btn decline-button" data-id="' + id + '" title="Rechazar Solicitud"><i class="fa-solid fa-x"></i></button>';
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
           
            $('.dataTables-List').on('click', '.note-button', function () {
                var id = $(this).data('id');
                var action = $(this).data('action');
                openmodal(id, action);
            });
            $('.dataTables-List').on('click', '.ar-button', function () {
                var id = $(this).data('id');
                $("#modalGrl").modal('show');
                $.ajax({
                    type: "GET",
                    url: "/Representante/CargarRepresentantes?idsol=" + id,
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
            });
            $('.dataTables-List').on('click', '.decline-button', function () {
                var id = $(this).data('id');
                swal({
                    title: '¿Seguro que quieres rechazar esta solicitud?',
                    showCancelButton: true,
                    confirm: false,
                    cancel: true,
                    confirmButtonText: 'Aceptar'
                },
                    function (isConfirm) {
                        if (isConfirm) {
                            // Enviar los datos de los detalles al servidor mediante AJAX
                            $.ajax({
                                type: "POST",
                                processData: false,
                                contentType: false,
                                url: "/Solicitudes/Rechazar" + "?id=" + id,
                                dataType: "JSON",
                                success: function (response) {
                                    if (response.exito) {
                                        MostrarSweetAlert(response.message, "success");
                                    } else {
                                        MostrarSweetAlert(response.message, "warning");
                                    }
                                },
                                error: function (xhr, status, error) {
                                    // Manejar cualquier error que ocurra durante la solicitud AJAX
                                    console.error("Error al enviar los datos:", error);
                                    MostrarSweetAlert(error.message, "error");
                                }
                            });
                        }

                    });

            });

        }).catch(error => {
            console.error(error);
        });


}


function callSA() {
    HoldOn.open({ theme: 'sk-circle', message: 'Espere un momento...' });
    fetch("/Solicitudes/SolicitudesAsignadasJSON",
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
            var table = $('#miTablaSA').DataTable(
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
                               data: "representante", name: "representante", autoWidth: true
                            },

                        ],
                    createdRow: function (row, data, dataIndex) {
                        $(row).addClass('row-with-buttons');
                    },
                    language: {

                        "url": "../lib/spanish.json"
                    },
                });


        }).catch(error => {
            console.error(error);
        });


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