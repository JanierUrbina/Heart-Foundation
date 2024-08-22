console.log("MIMI")
$("#savethis").click(function () {
    var estadoactual = $("#checkval").prop("checked");
    var estadoactualN = $("#checknoval").prop("checked");
    if (estadoactual || estadoactualN) {
        var estadoactual = estadoactual ? true : false;
        var nota = document.getElementById("divnota").style.display == "none" ? "" : $("#Nota").val();
        if (estadoactualN && nota == "") {
            MostrarSweetAlert("Debe escribir una nota", "warning");
        }
        else {
            if (estadoactualN && nota.length < 25) {
                MostrarSweetAlert("Indique de forma clara la razón por la cual rechaza el documento", "warning");
            }
            else {
                //0 = responsable / 1 = director
                HoldOn.open({ message: 'Espere un momento...' });
                var id = $("#savethis").data("id");;

                $.ajax({
                    url: '/Solicitudes/ValidarSolicitud?idsol=' + id + '&Nota=' + nota + '&Estado=' + estadoactual,
                    type: 'POST',
                    dataType: 'JSON',
                    success: function (data) {
                        if (data.bien) {
                            HoldOn.close();
                            $("#savethis").prop("disabled", "disabled");
                            MostrarSweetAlert("Bien", "success");
                        }
                        else {
                            HoldOn.close();
                            MostrarSweetAlert("Ocurrió un problema al momento de guardar", "error");
                        }
                    }
                });
            }
        }

    }
    else {
        MostrarSweetAlert("Debe serleccionar un estado primero", "warning");
    }
});

function Valido() {
    var estadoactual = $("#checkval").prop("checked");
    if (estadoactual == true) {
        $("#checknoval").prop("checked", false);
        HabilitarNota(false);
    }
}

function Invalido() {
    var estadoactual = $("#checknoval").prop("checked");
    if (estadoactual == true) {
        $("#checkval").prop("checked", false);
    }
   
    HabilitarNota(estadoactual);
}

function HabilitarNota(estado) {
    if (estado) {
        document.getElementById("divnota").style.display = "";
    }
    else {
        document.getElementById("divnota").style.display = "none";
    }
}