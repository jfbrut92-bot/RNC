$(document).ready(function ($) {

    $('input#continuecheck').click(function () {
        var checked = $('input#continuecheck:checked').length > 0 ? true : false;
        if (checked) {
            $('input#confirmerButton').show();
        } else {
            $('input#confirmerButton').hide();
        }
    });

});
