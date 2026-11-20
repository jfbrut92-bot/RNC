$(document).ready(function () {

    $("a.submit").click(function (event) {

        if ($('Email').val() != '') {
            //on récupère le formulaire
            var form = $("form")[0];
            form.submit();

            //le lien est inopérant
            event.preventDefault();
        } else {
            alert('L\'email doit être renseigné !');
        }

    });

});
