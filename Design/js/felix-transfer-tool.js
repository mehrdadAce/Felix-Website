$( window ).on( "load", initScript );
function initScript() {

    const progresIncrease  = 6;
    const timeOutNextSlide = 500;
    const fileInputs       = '.inputfile';
    const imgSliderOutside = '#slider-outside';
    const imgSliderInside  = '#slider-inside';
    const imgSliderDamage  = '#slider-damage';
    const imgSliderDocs    = '#slider-docs';
    const panelOutside     = $('#panel-outside');
    const panelInside      = $('#panel-inside');
    const panelDamage      = $('#panel-damage');
    const panelDocuments   = $('#panel-docs');
    const arrStep1 = ["linksvoor", "rechtsvoor", "linksachter", "rechtsachter"];
    const arrStep2 = ["voorzetels", "achterzetels", "dashboard", "km"];
    const arrStep3 = ["dmgoutside","dmginside"]
    const arrStep4 = ["inschrijving", "keuringsbewijs"];

    // Change Event on all fileInputs.
    $(fileInputs).on('change', function() {
        readURL(this);
    });

    // Hide & show panels
    function fadeOutOldFadeInNew(panel1, panel2) {
        panel1.addClass("animated fadeOutLeft");
        panel2.addClass("animated fadeInRight");
        setTimeout(function() { 
            panel1.css("display", "none");
            panel2.css("display", "block");
        }, timeOutNextSlide);
    }

    function goToNextPanel(sliderId, index) {
        // Last index of 2 step sliders.
        if (index === 2 && (sliderId === imgSliderDocs || sliderId === imgSliderDamage)){
            switch (sliderId) {
                case imgSliderDamage:
                    fadeOutOldFadeInNew(panelDamage, panelDocuments);
                    break;
                case imgSliderDocs:
                    // 🧙‍ Wizard completed! 
                    window.location.href = 'second-hand-transfer-step-3.html';
                    break;
            }
        }
        // Last index of 4 step sliders.
        else if (index === 4) {
            switch (sliderId) {
                case imgSliderOutside:
                    fadeOutOldFadeInNew(panelOutside, panelInside);
                    break;
                case imgSliderInside:
                    fadeOutOldFadeInNew(panelInside, panelDamage);
                    break;
            }
        }
    }
    function uploadNextPicture(slideId, index) {

    }

    function updateProgressBar() {
        const currentProgress = parseInt($(".progress-bar").attr("aria-valuenow")) + progresIncrease;
        $(".progress-bar").css("width", currentProgress + "%").attr("aria-valuenow", currentProgress); 
    }

    // Go to next slide of silder with, format: '#id'.
    function slideToNextSlide(sliderId) {
        const slideIndex = $(sliderId + ' .item.active').index() + 1;
        setTimeout(function() {
            // sliderId check -> otherwise 4 step sliders get stuck on index 2.
            if((slideIndex === 2 && (sliderId === imgSliderDocs || sliderId === imgSliderDamage)) || slideIndex == 4 ) {
                goToNextPanel(sliderId, slideIndex);
            }
            else {
                $(sliderId).carousel("next"); 
            }
            updateProgressBar();
        }, timeOutNextSlide);            
    }

    // Show uploaded image in right <img>.
    function showImage(input, event, imgId) {
        const img = $(imgId);
        img.attr('src', event.target.result);
        img.addClass( "img-uploaded" );
        $(input).parent().parent().css( "display", "none" );   
    }

    // Read uploaded files.
    function readURL(input) {
        if (!input.files && !input.files[0]) { return; }
        const reader = new FileReader();
        reader.onload = function (event) {
            const id = input.id
            if(id != "dmginside" && id != "dmgoutside"){
                showImage(input, event, '#img-' + id);
                if(arrStep1.includes(id)) {
                    slideToNextSlide(imgSliderOutside);
                } else if (arrStep2.includes(id)) {
                    slideToNextSlide(imgSliderInside);
                } else if (arrStep3.includes(id)) {
                    slideToNextSlide(imgSliderDamage);
                } else if (arrStep4.includes(id)) {
                    slideToNextSlide(imgSliderDocs);
                }
            }
            else {  
                // Add new element to container. 
                if(id === "dmgoutside") {
                    const id = $("#damage-outside-uploads").children().length; 
                    $( "<div id='img-outside" + id + "' class='img-item wow animated fadeInUp' data-wow-delay='0.2s'></div>").appendTo( "#damage-outside-uploads" );                
                    var bg = $("#img-outside" + id);
                    bg.css("background-image", "url('" + event.target.result + "')");
                }
                else if( id== "dmginside") {
                    const id = $("#damage-inside-uploads").children().length; 
                    $( "<div id='img-inside" + id + "' class='img-item wow animated fadeInUp' data-wow-delay='0.2s'></div>").appendTo( "#damage-inside-uploads" );                
                    var bg = $("#img-inside" + id);
                    bg.css("background-image", "url('" + event.target.result + "')");
                }
            }
        }
        reader.readAsDataURL(input.files[0]);
    }

    $("#next-slide").click(function() {
        slideToNextSlide(imgSliderDamage);
    });
}