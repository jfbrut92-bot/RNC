//positionner le focus sur le premier champ de saisie du formulaire
//
//appel: <script src="/Scripts/rnc2/setfocus.js" type="text/javascript"></script>

$(function () {
    $("input[type=text]").first().focus();
});
