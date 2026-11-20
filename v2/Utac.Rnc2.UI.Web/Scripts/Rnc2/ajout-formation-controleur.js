$(document).ready(function ($) {
    //A l'ouverture on cache la DIV 'bloc_autre_sanction' sauf si le choix 'Type de Sanction' est 'Autre sanction'
    if ($('#Formation_RTypeFormation_Id').val() === '100' || $('#Formation_RTypeFormation_Id').val() === '200') {
        $('#bloc_date_debut').hide();
    }

    //Au changement du 'Type de sanction'
    $('#Formation_RTypeFormation_Id').change(function () {
        if ($('#Formation_RTypeFormation_Id').val() === '100' || $('#Formation_RTypeFormation_Id').val() === '200') {
            $('#bloc_date_debut').hide();
        } else {
            $('#bloc_date_debut').show();
        }
    });
});
