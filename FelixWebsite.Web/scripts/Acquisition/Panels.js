const timeOutNextSlide = 500;
const fileInputs = '.inputfile';

const imgSliderOutside = '#slider-outside';
const imgSliderInside = '#slider-inside';
const imgSliderDamage = '#slider-damage';
const imgSliderDocs = '#slider-docs';

const panelOutside = $('#panel-outside');
const panelInside = $('#panel-inside');
const panelDamage = $('#panel-damage');
const panelDocuments = $('#panel-docs');

const damageImgSliderInside = '#slider-inside-damage';
const damageImgSliderDocs = '#slider-docs-damage';

const damagePanelInside = $('#panel-inside-damage');
const damagePanelDocuments = $('#panel-docs-damage');

const arrStep1 = ["linksvoor", "rechtsvoor", "linksachter", "rechtsachter"];
const arrStep2 = ["voorzetels", "achterzetels", "dashboard", "km"];
const arrStep3 = ["dmgoutside", "dmginside"];
const arrStep4 = ["inschrijvingVoorkant", "inschrijvingAchterkant", "keuringsbewijs"];

const damageArrStep2 = ["km_damages", "chassisnummer"];
const damageArrStep4 = ["inschrijvingVoorkant_damages", "inschrijvingAchterkant_damages", "verzekeringsbewijs", "keuringsbewijs"];

const imagesUploadedFor = [];
let currentPanel = "linksvoor";
let damagePicturesUploaded = 0;

const skipDmgOutside = '.btnSkip-dmgoutside';
const skipDmgInside = '.btnSkip-dmginside';
const skipExamination = '.btnSkip-keuringsbewijs';
const continueDmgOutside = '.btnContinue-dmgoutside';
const continueDmgInside = '.btnContinue-dmginside';
const finishOverview = '#finishOverviewButton';

const goBackArrow = ".go-back-arrow";
const goForwardArrow = ".go-forward-arrow";

var fileInput;
// Change Event on all fileInputs.
$(fileInputs).on('change', function () {
    fileInput = this; //readURL(this);
});

$(goBackArrow).on('click',
    function () {
        selectWhichPreviousSlide();
    });

$(goForwardArrow).on('click',
    function () {
        if (currentPanel === "dmgoutside") {
            continueDamageOutside();
        }
        else if (currentPanel === "dmginside") {
            continueDamageInside();
        } else if (currentPanel === "linksvoor") {
            if (isSchemeScreenView()) {
                goFromSchemeToPhotos();
            } else {
                readURL(false);
            }
        } else {
            readURL(false);
        }
    });

// Hide & show panels
function fadeOutOldFadeInNew(panel1, panel2) {
    panel1.removeClass("fadeInLeft");
    panel2.removeClass("fadeOutRight");
    panel1.addClass("animated fadeOutLeft");
    panel2.addClass("animated fadeInRight");
    setTimeout(function () {
        panel1.css("display", "none");
        panel2.css("display", "block");
    }, timeOutNextSlide);
}

function reverseFadeOutOldFadeInNew(panel1, panel2) {
    panel1.removeClass("fadeInRight");
    panel2.removeClass("fadeInRight");
    panel2.removeClass("fadeOutLeft");
    panel1.addClass("animated fadeOutRight");
    panel2.addClass("animated fadeInLeft");
    setTimeout(function () {
        panel1.css("display", "none");
        panel2.css("display", "block");
    }, timeOutNextSlide);
}

function goToNextPanel(sliderId) {
    switch (sliderId) {
        case imgSliderDamage:
            currentPanel = arrStep4[0];
            fadeOutOldFadeInNew(panelDamage, panelDocuments);
            break;
        case imgSliderDocs:
            // 🧙‍ Wizard completed! 
            goToOverview();
            break;
        case imgSliderOutside:
            if (isTakeOver()) {
                currentPanel = arrStep2[0];
                fadeOutOldFadeInNew(panelOutside, panelInside);
            } else {
                currentPanel = damageArrStep2[0];
                fadeOutOldFadeInNew(panelOutside, damagePanelInside);
            }
            break;
        case imgSliderInside:
            currentPanel = arrStep3[0];
            fadeOutOldFadeInNew(panelInside, panelDamage);
            setDamageButtons(true);
            break;
        case damageImgSliderInside:
            if (isUserInsured()) {
                currentPanel = damageArrStep4[0];
                fadeOutOldFadeInNew(damagePanelInside, damagePanelDocuments);
            } else {
                goToOverview();
            }
            break;
        case damageImgSliderDocs:
            goToOverview();
            break;
        default:
            // 🧙‍ Wizard completed! 
            goToOverview();
            break;
    }
}
// Go to next slide of silder with, format: '#id'.

function goToPreviousPanel(sliderId) {
    switch (sliderId) {
        case imgSliderDamage:
            currentPanel = arrStep2[arrStep2.length - 1];
            reverseFadeOutOldFadeInNew(panelDamage, panelInside);
            break;
        case imgSliderDocs:
            // 🧙‍ Wizard completed!        
            currentPanel = arrStep3[arrStep3.length - 1];
            reverseFadeOutOldFadeInNew(panelDocuments, panelDamage);
            break;
        case imgSliderOutside:
            if (isTakeOver()) {
                goToInformationOverview();
            } else {
                if (isSchemeScreenView()) {
                    goToInformationOverview();
                } else {
                    goToSchemeOverview();
                }
            }
            //reverseFadeOutOldFadeInNew(panelOutside, panelInside);
            break;
        case imgSliderInside:
            currentPanel = arrStep1[arrStep1.length - 1];
            reverseFadeOutOldFadeInNew(panelInside, panelOutside);
            break;
        case damageImgSliderInside:
            currentPanel = arrStep1[arrStep1.length - 1];
            reverseFadeOutOldFadeInNew(damagePanelInside, panelOutside);
            break;
        case damageImgSliderDocs:
            currentPanel = damageArrStep2[damageArrStep2.length - 1];
            reverseFadeOutOldFadeInNew(damagePanelDocuments, damagePanelInside);
            break;
        default:
            // 🧙‍ Wizard completed! 
            //goToInformationOverview();
            break;
    }
}

function slideToNextSlide(sliderId) {
    const slideIndex = $(sliderId + ' .item.active').index() + 1;
    setTimeout(function () {
        // sliderId check -> otherwise 4 step sliders get stuck on index 2.
        if ((slideIndex === arrStep1.length && sliderId === imgSliderOutside) ||
            (slideIndex === arrStep2.length && sliderId === imgSliderInside) ||
            (slideIndex === arrStep3.length && sliderId === imgSliderDamage) ||
            (slideIndex === arrStep4.length && sliderId === imgSliderDocs) ||
            (slideIndex === damageArrStep2.length && sliderId === damageImgSliderInside) ||
            (slideIndex === damageArrStep4.length && sliderId === damageImgSliderDocs)) {
            goToNextPanel(sliderId);
        } else {
            $(sliderId).carousel("next");
            setCurrentPanel(slideIndex, sliderId);
        }
        updateProgressBar(step2, 1);
        hideNextSlideArrow();
    }, timeOutNextSlide);
}

function slideToPreviousSlide(sliderId) {
    const slideIndex = $(sliderId + ' .item.active').index() - 1;
    setTimeout(function () {
        // sliderId check -> otherwise 4 step sliders get stuck on index 2.
        if (slideIndex === -1) {
            goToPreviousPanel(sliderId);
        } else {
            $(sliderId).carousel("prev");
            setCurrentPanel(slideIndex, sliderId);
        }
        updateProgressBar(step2, -1);
        if (!(slideIndex === -1 && sliderId === imgSliderOutside)) {
            setLabelOn($(fileInput).closest('form'), true);
        }
        showNextSlideArrow();
    }, timeOutNextSlide);
}

// Read uploaded files.
function readURL(isPhotoUploaded, mediaValuesList) {
    if (!fileInput.files && !fileInput.files[0]) { return; }

    function readAndPreview(file, currentFileIndex) {
        let uploadField = (isTakeOver() ? "#panel-damage ." : "#panel-damage-damage .") + currentPanel + "-uploads";

        if (currentPanel === "dmgoutside"
            || currentPanel === "dmginside") {// Add new element to container
            SetImage(uploadField, currentPanel, mediaValuesList[currentFileIndex].MediaUrl, mediaValuesList[currentFileIndex].MediaId);
        }
        else {// Show image and continue
            if (isPhotoUploaded) {
                const img = $('.img-' + currentPanel);
                img.attr('src', mediaValuesList[currentFileIndex].MediaUrl);
                img.addClass("img-uploaded");
            }
            selectWhichSlide();
        }
    }

    [].forEach.call(fileInput.files, readAndPreview);
}

function readUrlForScheme(mediaValuesList, schemeType) {
    if (!mediaValuesList && !mediaValuesList[0]) { return; }

    function readAndPreview(file, currentFileIndex) {
        SetImage(".SchemeEntryDamage-uploads", schemeType, mediaValuesList[currentFileIndex].MediaUrl, mediaValuesList[currentFileIndex].MediaId);
    }

    [].forEach.call(mediaValuesList, readAndPreview);
}

function selectWhichSlide() {
    pushImagesUploadedFor(currentPanel);
    let isTakeOverVariable = isTakeOver();

    if (arrStep1.indexOf(currentPanel) !== -1) {
        slideToNextSlide(imgSliderOutside);
    } else if (arrStep2.indexOf(currentPanel) !== -1 && isTakeOverVariable) {
        slideToNextSlide(imgSliderInside);
    } else if (arrStep3.indexOf(currentPanel) !== -1 && isTakeOverVariable) {
        slideToNextSlide(imgSliderDamage);
    } else if (arrStep4.indexOf(currentPanel) !== -1 && isTakeOverVariable) {
        slideToNextSlide(imgSliderDocs);
    } else if (damageArrStep2.indexOf(currentPanel) !== -1) {
        slideToNextSlide(damageImgSliderInside);
    } else if (damageArrStep4.indexOf(currentPanel) !== -1) {
        slideToNextSlide(damageImgSliderDocs);
    }
}

function selectWhichPreviousSlide() {
    let isTakeOverVariable = isTakeOver();

    if (arrStep1.indexOf(currentPanel) !== -1) {
        slideToPreviousSlide(imgSliderOutside);
    } else if (arrStep2.indexOf(currentPanel) !== -1 && isTakeOverVariable) {
        slideToPreviousSlide(imgSliderInside);
    } else if (arrStep3.indexOf(currentPanel) !== -1 && isTakeOverVariable) {
        slideToPreviousSlide(imgSliderDamage);
    } else if (arrStep4.indexOf(currentPanel) !== -1 && isTakeOverVariable) {
        slideToPreviousSlide(imgSliderDocs);
    } else if (damageArrStep2.indexOf(currentPanel) !== -1) {
        slideToPreviousSlide(damageImgSliderInside);
    } else if (damageArrStep4.indexOf(currentPanel) !== -1) {
        slideToPreviousSlide(damageImgSliderDocs);
    }
}

$("#next-slide").click(function () {
    slideToNextSlide(imgSliderDamage);
});

function continueDamageInside() {
    if (damagePicturesUploaded >= getAmountOfDamagePicturesValue()) {
        pushImagesUploadedFor("dmginside");
        goToNextPanel(imgSliderDamage);
        hideNextSlideArrow();
        updateProgressBar(step2, 1);
    } else {
        notifyWarning(getErrorMessageDamagePictures(getAmountOfDamagePicturesValue()));
    }
}

function continueDamageOutside() {
    if (isTakeOver()) {
        pushImagesUploadedFor("dmgoutside");
        currentPanel = "dmginside";
        $(imgSliderDamage).carousel("next");
        setDamageButtons(true);
        updateProgressBar(step2, 1);
    } else {
        if (damagePicturesUploaded >= getAmountOfDamagePicturesValue()) {
            selectWhichSlide();
        } else {
            notifyWarning(getErrorMessageDamagePictures(getAmountOfDamagePicturesValue()));
        }
    }
    hideNextSlideArrow();
}

// Skip the current uploader
$(skipDmgInside).on("click", function () {
    continueDamageInside();
});
$(continueDmgInside).on("click", function () {
    continueDamageInside();
});

$(skipDmgOutside).on("click", function () {
    continueDamageOutside();
});

$(continueDmgOutside).on("click", function () {
    continueDamageOutside();
});

$(skipExamination).on("click", function () {
    goToNextPanel(imgSliderDocs);
    updateProgressBar(step2, 1);
});

// Set image in damages
function SetImage(dmgId, dmgWhere, imgSource, mediaId) {
    const id = !isSchemeScreen() ? $(dmgId).children().length : $(dmgId).children().length - 1;

    var thumbnail =
        '<div id="' + dmgWhere + id + '" class="img-item wow animated fadeInUp customDamagesStyle setHidden setPaddingTo0" data-wow-delay="0.2s">' +
        '<div class="thumbnail setMarginBottomTo0">' +
        '<img id="img-someImage" src="' + imgSource + '" alt="Source" class="setDamageImgHeight">' +
        '<input type="hidden" class="mediaId" value="' + mediaId + '"/>' +
        '</div>' +
        '</div>';
    $(dmgId).prepend(thumbnail);
}

// Progress bar functions
function updateProgressBar(currentStep, direction) {
    //Check if current screen is overviewscreen
    var overviewScreenDisplay = $(overviewScreen).css("display");
    if (overviewScreenDisplay === "block") return;
    if (currentStep === step1) {
        $(".progress-bar").css("width", 10 + "%").attr("aria-valuenow", 10);
        setNextProgressElementActive($("#progressInformation"), $("#progressPhotos"));
        setNextProgressElementActive($("#ssProgressInformation"), $("#ssProgressPhotos"));
        setNextProgressShortActive("#ssProgressPhotosShort", "#ssProgressPhotos", "#ssProgressInformationShort", "#ssProgressInformation");
    } else if (currentStep === step2) {
        let progressIncreaseUsed = isUserInsured() || isTakeOver() ? progressIncrease : 10;
        const currentProgress = parseInt($(".progress-bar").attr("aria-valuenow")) + progressIncreaseUsed * direction;
        $(".progress-bar").css("width", currentProgress + "%").attr("aria-valuenow", currentProgress);
        if (currentProgress >= 88) {// 88 is when the last picture has been uploaded which is examination
            setNextProgressElementActive($("#ssProgressPhotos"), $("#ssProgressOverview"));
            setNextProgressShortActive("#ssProgressOverviewShort", "#ssProgressOverview", "#ssProgressPhotosShort", "#ssProgressPhotos");
        }
    } else if (currentStep === step3) {
        $(".progress-bar").css("width", 100 + "%").attr("aria-valuenow", 100);
        setNextProgressElementActive($("#ssProgressOverview"), $("#ssProgressComplete"));
        setNextProgressShortActive("#ssProgressCompleteShort", "#ssProgressComplete", "#ssProgressOverviewShort", "#ssProgressOverview");
        $("#smallScreenProgress").children("div").css("width", "20%");
    }
    else if (currentStep === step4) {
        $(".progress-bar").css("width", 5 + "%").attr("aria-valuenow", 5);
    }
}
function setNextProgressElementActive(element1, element2) {
    element1.removeClass("active");
    element2.addClass("active");
}

function setNextProgressShortActive(elementshort, element, previousElementShort, previousElement) {
    $(elementshort).addClass("progressPartHide");
    $(element).removeClass("progressPartHide");

    $(previousElement).addClass("progressPartHide");
    $(previousElementShort).removeClass("progressPartHide");
}

function showNextSlideArrow() {
    $(goForwardArrow).removeClass('hidden');
}

function hideNextSlideArrow() {
    if (imagesUploadedFor.indexOf(currentPanel) === -1) {
        $(goForwardArrow).addClass('hidden');
    }
    if (isTakeOver() && currentPanel === "dmginside" && !imagesUploadedFor.indexOf("inschrijvingVoorkant") >= 0) {
        $(goForwardArrow).addClass('hidden');
    }
    if (!isTakeOver() && currentPanel === "dmgoutside" && !imagesUploadedFor.indexOf("inschrijvingVoorkant_damages") >= 0) {
        $(goForwardArrow).addClass('hidden');
    }
}

function pushImagesUploadedFor() {
    if (imagesUploadedFor.indexOf(currentPanel) === -1) {
        imagesUploadedFor.push(currentPanel);
    }
}

function setCurrentPanel(slideIndex, sliderId) {
    if (isTakeOver()) {
        switch (sliderId) {
            case imgSliderOutside:
                currentPanel = arrStep1[slideIndex];
                break;
            case imgSliderInside:
                currentPanel = arrStep2[slideIndex];
                break;
            case imgSliderDamage:
                currentPanel = arrStep3[slideIndex];
                break;
            case imgSliderDocs:
                currentPanel = arrStep4[slideIndex];
                break;
        }
    } else {
        switch (sliderId) {
            case imgSliderOutside:
                currentPanel = arrStep1[slideIndex];
                break;
            case damageImgSliderInside:
                currentPanel = damageArrStep2[slideIndex];
                break;
            case damageImgSliderDocs:
                currentPanel = damageArrStep4[slideIndex];
                break;
        }
    }
}

function setDamageButtons(isDamageOutside) {
    let continueDamage = isDamageOutside ? continueDmgOutside : continueDmgInside;
    let skipDamage = isDamageOutside ? skipDmgOutside : skipDmgInside;
    let tooltipText = $(".tooltiptext");

    if (isTakeOver()) {
        if (damagePicturesUploaded < getAmountOfDamagePicturesValue()) {
            $(continueDmgInside).addClass("disabled");
            $(skipDmgInside).addClass("disabled");
            tooltipText.removeClasss("hidden");
        } else {
            $(continueDmgInside).removeClass("disabled");
            $(skipDmgInside).removeClass("disabled");
            tooltipText.addClass("hidden");
        }
        if (damagePicturesUploaded === 0) {
            $(continueDamage).addClass("hideOnLoad");
            $(skipDamage).removeClass("hideOnLoad");
        } else {
            $(continueDamage).removeClass("hideOnLoad");
            $(skipDamage).addClass("hideOnLoad");
        }
    } else {
        if (damagePicturesUploaded < getAmountOfDamagePicturesValue()) {
            $(continueDamage).addClass("disabled");
            tooltipText.removeClass("hidden");
        } else {
            $(continueDamage).removeClass("disabled");
            tooltipText.addClass("hidden");
        }
        $(continueDamage).removeClass("hidden");
        $(skipDamage).addClass("hideOnLoad");
    }
}

function getAmountOfDamagePicturesValue() {
    return $("#amountOfDamagePictures").val();
}

function getErrorMessageDamagePictures(amountOfDamagePictures) {
    return $("#errormessageAmountOfDamagePictures").val() + amountOfDamagePictures;
}

function checkFinishOverviewButton() {
    if (!isTakeOver()) {
        $(finishOverview).removeClass("disabled");
        return;
    }
    if (damagePicturesUploaded < getAmountOfDamagePicturesValue()) {
        $(finishOverview).addClass("disabled");
    } else {
        $(finishOverview).removeClass("disabled");
    }
}

function getCurrentAmountOfDamagePictures() {
    if (isOverviewScreen()) {
        return $(".overview-damage").find(".damage-modifier").length;
    } else {
        if (isTakeOver()) {
            return $("panel-damage .dmgoutside-uploads > .fadeInUp").length
                + $("panel-damage .dmginside-uploads > .fadeInUp").length;
        } else {
            return $("#panel-damage-damage .dmgoutside-uploads > .fadeInUp").length;
        }
    }
}

function isSchemeScreenView() {
    return $(schemeScreen).css("display") === "block";
}