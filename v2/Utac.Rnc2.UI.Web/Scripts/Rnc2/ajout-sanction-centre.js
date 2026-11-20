$(document).ready(function ($) {
    //A l'ouverture on cache la DIV 'bloc_autre_sanction' sauf si le choix 'Type de Sanction' est 'Autre sanction'
    if ($('#Sanction_RTypeSanctionCentre_Id').val() != '5') {
        $('#bloc_autre_sanction').hide();
    }

    //Au changement du 'Type de sanction'
    $('#Sanction_RTypeSanctionCentre_Id').change(function () {
        if ($('#Sanction_RTypeSanctionCentre_Id').val() != '5') {
            $('#bloc_autre_sanction').hide();
        } else {
            $('#bloc_autre_sanction').show();
        }
    });
});
