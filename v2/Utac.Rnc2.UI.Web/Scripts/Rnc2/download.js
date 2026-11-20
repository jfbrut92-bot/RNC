$(document).ready(function () {

    $("a.download").click(function (event) {

        alert("Après le téléchargement, pensez à actualiser la page avec F5");
        /*
        event.preventDefault();
        alert("Lancement téléchargement de : " + $(this).attr('href'));
        $.ajax({
        url: $(this).attr('href'),
        context: document.body,
        success: function () {
        alert('Téchargement ok');
        document.location.reload(true);
        },
        error: function () {
        alert('Téchargement en échec');
        document.location.reload(true);
        },
        async: false
        });
        */
        /*
        form = $("#formDwn");
        if (!form) {
        $('<form id="formDwn" method="get" action="' + $(this).attr('href') + '">').css("display", "inline").appendTo("body");
        }

        $('#formDwn').submit();
        */
    });



});
