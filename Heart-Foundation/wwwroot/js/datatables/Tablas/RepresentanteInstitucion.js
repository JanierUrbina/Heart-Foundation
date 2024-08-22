function validate() {
    var j = $("input:radio[name=AreaEmpresa]:checked").val();
    if (j === undefined) {
        MostrarSweetAlert("Debe seleccionar un tipo de persona interna", "warning");
        return false;
    }
    else {
        $("#AreaEmpresa").val(j);
        return true;
    }
}
window.onload = function () {
    call();
};
function call() {
    HoldOn.open({ theme: 'sk-circle', message: 'Espere un momento...' });
    fetch("/Representante/IndexJSON",
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
            var table = $('#miTablaR').DataTable(
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
                            { data: "cargo", name: "cargo", autoWidth: true },
                            { data: "institucion", name: "institucion", autoWidth: true },                            
                            {
                                data: null,
                                render: function (data, type, row) {
                                    var id = row.id;

                                    var buttonsHtml = '<div class="btn-group" role="group">';
                                    buttonsHtml += '<button  class="btn watchLogo-button" data-id="' + id + '" title="Ver Logo"><i class="fas fa-file-alt"></i></button>';
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
            $('.dataTables-List').on('click', '.watchLogo-button', function () {
                var id = $(this).data('id');
                process(id);
            });


        }).catch(error => {
            console.error(error);
        });
}

function process(id) {
    HoldOn.open({ theme: 'sk-circle', message: 'Cargando Logo de la Institución...' });
    $.ajax({
        type: "POST",
        processData: false,
        contentType: false,
        url: "/Representante/CargarLogo?id=" + id,
        dataType: "JSON",
        success: function (response) {
            if (response.exito) {
                let img = document.getElementById("miImagen");
                img.src = "data:image/png;base64," + response.imagen;
                HoldOn.close();
                $('#modalimage').modal('show');
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