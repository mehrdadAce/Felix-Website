let filledInForms = [];
let currentImageId = 0;
let currentImageType = "";
let currentHoverId = 0;
let previousHoverId = 0;
let emptyModel = {
    IsSelected: true,
    Damage: false,
    HeavyDamage: false,
    Marks: false,
    HailDamage: false,
    Dent: false,
    Remarks: "",
};
let currentData = emptyModel;

const schemePictureHasDamage = "#scheme-image-has-damage";
const damageField = "#scheme-damage";
const heavyDamageField = "#scheme-heavy-damage";
const dentField = "#scheme-dent";
const marksField = "#scheme-marks";
const hailDamageField = "#scheme-hail-damage";
const clientCostsField = "#scheme-client-costs";
const schemeRemarksField = "#scheme-remarks";
const schemeCostsField = "#scheme-costs";
const schemeBody = ".scheme-body";
const placeholderSchemeText = "#placeholder-scheme-text";
const schemePicture = "#scheme-image-type";
const schemePicturesMaxAmount = 4;
let schemePicturesCurrentAmount = 0;

function imageClickEvent(e) {
    e.preventDefault();

    let userId = getUserId();

    if (!userId) {
        awaitUserId();
    }

    if ($("#scheme-text").hasClass("setHidden")) {
        $("#scheme-text").removeClass("setHidden");
        $(schemeContinue).removeClass("hidden");
        $(schemeSkip).addClass("hidden");
        $(placeholderSchemeText).addClass("hidden");
        if (userId) { setHiddenClasses(userId); }
    }

    currentImageId = this.id;
    currentImageType = this.alt;

    clearImages();

    $(schemePicture).val(currentImageType);
    if (userId) {
        getPhotoInfoesForScheme(userId, currentImageType);
    }

    fillInLocal(this);

    hideAllSelectedPictures();

    if (wasFilledIn(currentImageId)) {
        getSchemeData(userId, currentImageType, currentImageId);
    }
    else {
        fillInFields(emptyModel);
        currentData = emptyModel;

        var data = getData();
        sendData(currentImageId, data);
    }
}

function hideAllSelectedPictures() {
    $(".selected-img").addClass("opacity-0");
    $(".selected-img").removeClass("opacity-1");
}

function hideSelectedPictureWithId(imageId) {
    var selector = "#selected-img-" + getSchemeImageId(imageId);
    $(selector).addClass("opacity-0");
    $(selector).removeClass("opacity-1");
}

function showSelectedPictureWithId(imageId) {
    let currentSelectedPictureId = "#selected-img-" + getSchemeImageId(imageId);
    $(currentSelectedPictureId).addClass("opacity-1");
    $(currentSelectedPictureId).removeClass("opacity-0");
}

function awaitUserId() {
    window.setTimeout(function () {
        let userId = getUserId();
        if (userId) {
            setHiddenClasses(userId);
            getPhotoInfoesForScheme(userId, currentImageType);
        } else {
            awaitUserId();
        }
    }, 100);
}

function enterHoverEvent() {
    currentHoverId = this.id;

    hideAllSelectedPictures();
    showSelectedPictureWithId(currentHoverId);

    previousHoverId = currentHoverId;
}

function exitHoverEvent() {
    hideSelectedPictureWithId(previousHoverId);
}

function deleteOnLoad(userId) {
    $.ajax({
        url: "/umbraco/surface/AcquisitionScheme/DeleteOnPageLoad?userId=" + userId,
        type: "GET",
        cache: false,
        success: function (data) {
        }
    })
}
function getSchemeData(userId, imageType, imageId) {
    $.ajax({
        url: "/umbraco/surface/AcquisitionScheme/GetSchemeData?userId=" + userId + "&imageEntry=" + imageType,
        type: "GET",
        cache: false,
        success: function (data) {
            data.IsSelected = !data.IsSelected;
            fillInFields(data);
            sendData(imageId, getData());
        }
    })
}

function sendData(imageId, formData, continueToOverview) {
    let pictureId = "#colored-img-" + getSchemeImageId(imageId);
    setOrRemoveMarkedColour(formData, pictureId);
    $.ajax({
        url: "/umbraco/surface/AcquisitionScheme/PostSchemeData",
        type: "POST",
        dataType: 'json',
        data: formData,
        cache: false,
        success: function (data) {
            if (filledInForms.indexOf(imageId) === -1) {
                filledInForms.push(imageId);
            }
            if (isOverviewEdit && continueToOverview) {
                $(schemeScreen).css('display', 'none');
                getOverViewData();
            }
        }
    })
}

function setOrRemoveMarkedColour(formData, pictureId) {
    $(pictureId).removeClass("opacity-0");
    $(pictureId).addClass("opacity-1");
    if (isFormDataEmpty(formData)) {
        $(pictureId).removeClass("opacity-1");
        $(pictureId).addClass("opacity-0");
    }
}

function loadSchemeOverview(userId, fromOverview) {
    //Make sure we are at the top of the page
    window.scrollTo(0, 0);

    let id = userId === 0 ? userId : getUserId();
    let url = "/umbraco/surface/AcquisitionScheme/GetSchemeOverview";
    if (fromOverview) {
        url = "/umbraco/surface/AcquisitionScheme/GetSchemeOverviewWithAllDamages";
    }
    url += "?userId=" + id;
    $.ajax({
        url: url,
        type: "GET",
        cache: false,
        success: function (data) {
            $(schemeBody).html(data);
            let areas = Array.prototype.slice.call($("map area"));
            areas.map(function (area) {
                $(area).on("click", imageClickEvent);
                $(area).hover(enterHoverEvent, exitHoverEvent);
            });
            window.setTimeout(function () {
                //$('img[usemap]').imageMap();
                ImageMap('img[usemap]')
            }, 1000);
            jumpToTop();
            if (fromOverview) {
                $(schemeScreen).css('display', 'block');
                $(overviewScreen).css('display', 'none');
                $("#scheme-in-overview").first().remove();
                $(schemeGoToOverview).removeClass("hidden");
                $(schemeContinue).addClass("hidden");
                $(schemeSkip).addClass("hidden");
            }
        }
    })
}

function getPhotoInfoesForScheme(userId, imageType) {
    $.ajax({
        url: "/umbraco/surface/AcquisitionTool/GetPhotoInfoesForScheme?userId=" + userId + "&photoIdentifier=" + imageType,
        type: "GET",
        cache: false,
        success: function (data) {
            reactToSchemeUpload(data, null);
        }
    })
}

function getData() {
    let userId = getUserId();
    let imageType = currentImageType;
    let isSelected = $(schemePictureHasDamage).is(":checked");
    let damage = $(damageField).is(":checked");
    let heavyDamage = $(heavyDamageField).is(":checked");
    let dent = $(dentField).is(":checked");
    let marks = $(marksField).is(":checked");
    let hailDamage = $(hailDamageField).is(":checked");
    let remarks = $(schemeRemarksField).val();

    return {
        userId: userId,
        imageType: imageType,
        isSelected: isSelected,
        damage: damage,
        heavyDamage: heavyDamage,
        dent: dent,
        marks: marks,
        hailDamage: hailDamage,
        remarks: remarks
    };
}

function fillInFields(dataModel) {
    $(schemePictureHasDamage).prop('checked', dataModel.IsSelected);
    $(damageField).prop('checked', dataModel.Damage);
    $(heavyDamageField).prop('checked', dataModel.HeavyDamage);
    $(dentField).prop('checked', dataModel.Dent);
    $(marksField).prop('checked', dataModel.Marks);
    $(hailDamageField).prop('checked', dataModel.HailDamage);
    $(schemeRemarksField).val(dataModel.Remarks);

    if ($(schemePictureHasDamage).is(":checked")) {
        $(".input-fields").find("input").removeAttr("disabled");
        $(".input-fields").find("textarea").removeAttr("disabled");
        $("#imageUploader.scheme").slideDown();
    } else {
        $(".input-fields").find("input").attr("disabled", "disabled");
        $(".input-fields").find("textarea").attr("disabled", "disabled");
        $("#imageUploader.scheme").slideUp();
    }
}


function wasFilledIn(imageId) {
    return filledInForms.indexOf(imageId) !== -1;
}

function getUserId() {
    return $("#mainUserId").val();
}

function fillInLocal(currentImage) {
    let number = getSchemeImageId(currentImage.id);
    $("#local-number").html(number);
    $("#local-name").html(currentImage.title)
}

function isFormDataEmpty(formData) {
    return !formData.isSelected
        && !formData.damage
        && !formData.marks
        && !formData.hailDamage
        && !formData.remarks
        && !formData.dent
        && !formData.heavyDamage;
}

function getSchemeImageId(id) {
    if (id === 0) return 0;
    let splitSchemeId = id.split("-");
    return splitSchemeId[splitSchemeId.length - 1];
}

function clearImages() {
    $(".SchemeEntryDamage-uploads > div.setPaddingTo0").remove();
    schemePicturesCurrentAmount = 0;
}