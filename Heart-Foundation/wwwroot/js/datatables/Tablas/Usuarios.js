
window.onload = function () {
    call();
};
function call() {
    HoldOn.open({ theme: 'sk-circle', message: 'Espere un momento...' });
    fetch("/Usuarios/IndexJSON",
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
            var table = $('.dataTables-List').DataTable(
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
                            { data: "userName", name: "userName", autoWidth: true },
                            { data: "email", name: "email", autoWidth: true },
                            { data: "fechaCreacion", name: "fechaCreacion", autoWidth: true },

                            {
                                data: null,
                                render: function (data, type, row) {
                                    var id = row.id;

                                    var buttonsHtml = '<div class="btn-group" role="group">';
                                    buttonsHtml += '<button  class="btn update-button" data-id="' + id + '" title="Actualizar contraseña"><i class="fa-solid fa-pencil" style="color:green"></i></button>';
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
            $('.dataTables-List').on('click', '.update-button', function () {
                var id = $(this).data('id');
                onclick = edit(id);
                $('#updateModal').modal('show');
            });

        }).catch(error => {
            console.error(error);
        });
}



//Eliminar
var selectedId;
function edit(id) {
    selectedId = id;
}
function update() {
   
    const baseUrl = "/Usuarios/ActualizarContrasena" + "?id=" + selectedId + "&Contraseña=" + document.getElementById('contra').value;

    fetch("/Usuarios/ActualizarContrasena" + "?id=" + selectedId + "&Contraseña=" + document.getElementById('contra').value, {
        method: 'POST',
        headers:
        {
            'Content-Type': 'application/json'
        }
      
    })
    .then(response => response.json())
    .then(data => {
        console.log(data);
        if (data.success) {
            MostrarSweetAlert(data.message, "success");
        } else {
            MostrarSweetAlert(data.message, "error");
        }
    })
    .catch(error => {
        console.error("Error:", error);
    });
}


