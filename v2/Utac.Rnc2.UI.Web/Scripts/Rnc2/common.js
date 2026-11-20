$(document).ready(function () {
    // On donne la possibilité de fermer les div de class="static-msg"
    $('a.static-msg-btn').click(function () {
        var divMessage = $(this).parents("div.static-msg").length > 0 ? $(this).parents("div.static-msg").get(0) : null;
        if (divMessage) {
            $(divMessage).fadeOut();
            $(divMessage).removeClass("d-flex");
        }
    });
});
