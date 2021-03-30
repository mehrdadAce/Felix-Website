$( document ).ready(function() {
    $('.scroll').click(function() {
        $('html, body').animate({
            scrollTop: eval($('#' + $(this).attr('target')).offset().top - 70)
        }, 1000);
    });
});
