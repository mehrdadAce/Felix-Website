//full width revolution
var revapi;

jQuery(document).ready(function() {
    revapi = jQuery('.full-width-banner').revolution(
    {
        delay: 9000,
        startwidth: 1170,
        startheight: 550,
        hideThumbs: 10,
        fullWidth: "on",            
        touchenabled: "on",
        forceFullWidth: "on"
    });
});	//ready


