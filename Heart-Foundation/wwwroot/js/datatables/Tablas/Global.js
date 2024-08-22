

function MostrarSweetAlert(msj, tp) {
    switch (tp) {
        case "success":
            swal("Éxito", msj, tp);
            $("button.confirm").text("Aceptar").click(function () { window.location.href = ''; });
            break;
        case "error":
            swal("Error", msj, tp);
            break;
        case "warning":
            swal("Advertencia", msj, tp);
            break;

    }
}

function closemodal(button) {
    // Encuentra el modal padre del botón que se hizo clic
    var modal = button.closest('.modal');

    // Si se encuentra el modal, ciérralo
    if (modal) {
        $(modal).modal('hide');
    }
}

function handleFormSubmit() {
    // Desactivar los botones al enviar el formulario
    document.getElementsByClassName("submit").disabled = true;

    // Opcional: Mostrar el mensaje de procesamiento si lo necesitas
    HoldOn.open({
        message: 'Procesando..',
        textColor: 'white'
    });

    // Asegurarse de que el formulario se envíe
    return true;
}


