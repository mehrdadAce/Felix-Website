$(document).on('click','.navbar-collapse.in',function(e) {
    if( $(e.target).is('a') ) {
        $(this).collapse('hide');
    }
});
$(document).on('click', '.navbar-nav li', function() {
    if($(this).attr('id') != "contact") {
        $(".navbar-nav li").removeClass("active");
        $(this).addClass("active");
    }
});