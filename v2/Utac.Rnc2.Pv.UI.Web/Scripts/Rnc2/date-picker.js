$(document).ready(function () {

    //parametrage DatePicker
    $.datepicker.setDefaults($.datepicker.regional["fr"]);
    $(".datepicker").datepicker({
        showOn: "button",
        buttonImage: "/Content/Datepicker/calendar.gif",
        buttonImageOnly: true,
        showButtonPanel: true,
        changeMonth: true,
        changeYear: true
    });

    //option d'ouverture
    $(".datepicker").datepicker("option", "showAnim", "slideDown");

    //gestion de la verification des dates
    $("form.verif_date").submit(function () {
        var inputDate = $("input.verif_date").get(0);
        var date1 = new Date();
        var sdate1 = $(inputDate).val();
        date1.setFullYear(sdate1.substr(6, 4));
        date1.setMonth(parseInt(sdate1.substr(3, 2)) - 1);
        date1.setDate(sdate1.substr(0, 2));
        date1.setHours(0);
        date1.setMinutes(0);
        date1.setSeconds(0);
        date1.setMilliseconds(0);

        var currentTime = new Date();

        var milli_d1 = date1.getTime();
        var milli_d2 = currentTime.getTime();
        var diff = milli_d1 - milli_d2;
        if (diff <= 0) {
            if (confirm("Vous avez saisi une date égale ou antérieure à la date du jour. Consultez le message d’alerte affiché à l’écran. Pour confirmer la date saisie, cliquez sur OK, pour la corriger, cliquez sur ANNULER"))
                return true;
            else
                return false;
        }
        return true;

    });
});
