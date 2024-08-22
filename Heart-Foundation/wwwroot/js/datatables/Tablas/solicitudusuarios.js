window.onload = function () {
    call();
};
function call() {
    HoldOn.open({ theme: 'sk-circle', message: 'Espere un momento...' });
    fetch("/Solicitudusuario/IndexJSON",
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
                            { data: "nombre", name: "nombre", autoWidth: true },
                            { data: "correo", name: "correo", autoWidth: true },
                            { data: "numeroTelefonico", name: "numeroTelefonico", autoWidth: true },
                            {
                                data: null,
                                name: "tipoUsuario",
                                autoWidth: true,
                                render: function (data, type, row) {
                                    switch (data.tipoUsuario) {
                                        case 0:
                                            return 'Representante';
                                        case 1:
                                            return 'Natural';                                       
                                        default:
                                            return 'Desconocido';
                                    }
                                }
                            },
                            {
                                data: null,
                                render: function (data, type, row) {
                                    var id = row.id;
                                    var msj = row.mensaje;

                                    var buttonsHtml = '<div class="btn-group" role="group">';
                                    
                                    buttonsHtml += '<button  class="btn note-button" data-msj="' + msj + '" title="Ver mensaje"><i class="fa-solid fa-comment-dots"></i></button>';
                                    buttonsHtml += '<button  class="btn decline-button" data-id="' + id + '" title="Rechazar"><i class="fa-solid fa-x"></i></button>';
                                    buttonsHtml += '<button  class="btn user-button" data-nombre="' + row.nombre + '" data-correo="' + row.correo + '" data-numero="' + row.numeroTelefonico + '" data-idsu="' + row.id +'" title = "Convertir a usuario" > <i class="fa-solid fa-user-plus"></i></button > ';
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
            $('.dataTables-List').on('click', '.note-button', function () {
                var msj = $(this).data('msj');
                $("#mensaje").val(msj);
                $("#msjModal").modal('show');
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
                                url: "/Solicitudusuario/Rechazar" + "?id=" + id,
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

            $('.dataTables-List').on('click', '.user-button', function () {
                var nombre = $(this).data('nombre');
                var numero = $(this).data('numero');
                var id = $(this).data('idsu');
                var correo = $(this).data('correo'); // Asegúrate de que el correo se pase como atributo en el HTML

                // Redirige a la acción Registro pasando el correo como parámetro
                window.location.href = '/Usuarios/Registro?correo=' + encodeURIComponent(correo) + '&nombre=' + encodeURIComponent(nombre) + '&numero=' + encodeURIComponent(numero) + '&id=' + encodeURIComponent(id);
            });


        }).catch(error => {
            console.error(error);
        });
}