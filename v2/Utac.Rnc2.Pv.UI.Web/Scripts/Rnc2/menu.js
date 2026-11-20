// For mobile nav
var selector = '.navbar-nav li';
$(selector).on('click', function () {
    $(selector).removeClass('active');
    $(this).addClass('active');
});
$(document).ready(function () {
    $('.navbar-nav li').find('a[href="' + location.pathname + '"]')
        .closest('li').addClass('active');

    calculateHeight();
    $(window).on('resize', function () {
        calculateHeight();
    });
});

function calculateHeight() {
    if ($(window).width() < 768) {
        var val = $(window).height() - $('#header-mobile').height() - $('.mentions').height();
        $('.content-height').height(val);
    } else {
        var val = $(window).height() - $('#header').height() - $('#bottom').height();
        $('.content-height').height(val);
    }
}
