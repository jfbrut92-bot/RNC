$(document).ready(function () {

    //On pose un listener sur la saisie du code agrément
    $('.remote_validation').focusout(function () {
        $('input[submit]').attr('disabled', 'disabled');
        var codAgrem = $(this).val();
        if (codAgrem == "" || codAgrem == null) {
            return;
        }
        verifierAgrem(codAgrem);

    });

    inputs = $('.remote_validation');
    codAgrem = inputs[0];
    if ($(codAgrem).val() != null && $(codAgrem).val() != "") {
        verifierAgrem($(codAgrem).val());
    }

});

function verifierAgrem(codAgrem) {
    $('input[submit]').attr('disabled', 'disabled');
    var url = "/Utilisateurs/EstValideCentre";
    var categorie = $.getUrlVar("typeCentre");

    $.getJSON(url, { codAgremCentre: codAgrem, categorieVehicule: categorie, format: "json" }, function (data) {

        if (data.ok) {
            //Le centre est agréé et pas d'utilisateur associé
            $('#Utilisateur_Id').val(codAgrem.toLowerCase());
            $('input[type=submit]').attr('disabled', false);

        }
        else {
            //alert(data.raison);
            alert("Le code agrement saisi n'est pas valide");
            $('.remote_validation').focus();
            $('#Utilisateur_Id').val("");
        }

    });

}
