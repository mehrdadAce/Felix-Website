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
var clickCount = 0;
$('#brandLogo').on("click", function () {
    if ($(window).width() < 440) {
        if (clickCount % 2 === 0)
        {
            $('.brandLogo').css('height', '65px');
            $('.brandLogo').css('margin-top', '5px');
        } else
        {
            $('.brandLogo').css('height', '80px');
            $('.brandLogo').css('margin-top', '20px');
        }
        clickCount++;
    }
});