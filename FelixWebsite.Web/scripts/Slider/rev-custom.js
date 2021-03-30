/* 
 * piple
 * Created by codepiple
 * V1.0
 */


//full width revolution
var revapi;

jQuery(document).ready(function() {

    revapi = jQuery('.full-width-banner').revolution(
            {
                delay: 9000,
                startwidth: 1170,
                startheight: 500,
                hideThumbs: 10,
                fullWidth: "on",            
                touchenabled: "on",
                forceFullWidth: "on"

            });

});	//ready


