$( ".btn-openingsuren" ).click(function() {
    // Set all buttons inactive
    $( ".btn-openingsuren" ).removeClass('btn-active');
    // If the pressed button hasn't been pressed before => Set class active
    if($($(this).attr('data-target')).attr('aria-expanded') == "false" || $($(this).attr('data-target')).attr('aria-expanded') == undefined) {
        $(this).addClass('btn-active')
    }
    // Hide previously opened collapses
    $('.collapse').collapse('hide');
    // Toggle the collapse related to the button
    $($(this).attr('data-target')).collapse('show');
    // Is view mobile 
    if($( window ).width() <= 991){
        // Scroll to section 
        $('html, body').animate({
            scrollTop: eval($("#openingsuren").offset().top - 70)}, 1000);
    };
});