var dmgoutsideCount = 0;
var dmginsideCount = 0;
var deletePictureId;
var errorMessage;
var imageClass;
var overviewForm;
let insurances = [];

let takeoverUserId = 602;
let damagesUserId = 689;
let damageSchemeUserId = 493;

let landscapeModeMessage = "De foto moet in landschap modus genomen zijn";

window.onload = function () {
    loadNotificationMessages();
    getInsurances();
    //setupPhoneInputs();
    //skipToOverview(isTakeOver());
    //skipToTestScheme();
}

function skipToOverview(isTakeOver) {
    $("#mainUserId").val(isTakeOver ? takeoverUserId : damagesUserId);
    goToOverview();
    $("#userdataScreen").css("display", "none");
}

function skipToTestScheme() {
    $("#userdataScreen").css("display", "none");
    $("#mainUserId").val(damageSchemeUserId);
    loadSchemeOverview();
    //deleteOnLoad(damageSchemeUserId);
    showScheme();
    setHiddenClasses(getUserId());
}

$(document).on('change', "form.photos", prepareUpload);
$(document).on('change', "form.scheme", prepareUpload);
function prepareUpload(event) {
    $(event.target).closest("form").submit();
}

$("#generate-pdf").on("click", function () {
    let businessPageId = $("#businessPageId").val();
    let takeOverId = isTakeOver() ? takeoverUserId : damagesUserId;
    //let pdfUrl = `/umbraco/Surface/AcquisitionTool/GeneratePdf?id=${isTakeOver() ? takeoverUserId : damagesUserId}&businessPageId=${businessPageId}`;
    let pdfUrl = "/umbraco/Surface/AcquisitionTool/GeneratePdf?id=" + takeOverId + "&businessPageId=" + businessPageId;
    $.ajax({
        url: pdfUrl,
        type: "GET",
        cache: false,
        success: function (data) {
            notifySuccess("PDF gegenereert");
        }
    })
});

$(document).on("submit", "form", function (e) {
    checkForPostingPhotos(e, this);
});

function checkForPostingPhotos(e, form) {
    e.preventDefault(); // stop the standard form submission
    if (e.currentTarget.id === "formUserInformation") {
        return;
    }
    if (e.currentTarget.id.id === "updateImage") {
        updatePhotoInOverview(form);
    } else if (e.currentTarget.id.id === "addImage") {
        postDamagesInOverview(form);
    } else {
        postPhoto(form);
    }
}

function postPhoto(form) {
    if (fileInput.files.length === 0) return;
    if (isPhotosAmountNotOkayForScheme(fileInput)) {
        return;
    }
    setLoadingIcon(form);
    $.ajax({
        url: "/umbraco/Surface/AcquisitionTool/PostPhotos",
        type: form.method,
        data: new FormData(form),
        contentType: false,
        async: true,
        processData: false,
        cache: false,
        success: function (data) {
            if (isSchemeScreen()) {
                reactToSchemeUpload(data, form);
            } else {
                reactToPhotosUpload(data, form);
            }
            if (!isSchemeScreen() && !isOverviewScreen()) {
                jumpToTop();
            }
            if (isSchemeScreen() || isOverviewScreen() || isADamageScreen()) {
                let label = $(form).find("label");
                label.css("display", "");
            }
        },
        error: function (error) {
            showJsonError(error, "");
            setLoadingIconOff(form);
            setLabelOn(form, false);
        }
    });
}

function isPhotosAmountNotOkayForScheme(fileInput) {
    if (isSchemeScreen()
        && (fileInput.files.length + schemePicturesCurrentAmount) > getAmountOfDamagePicturesValue()) {
        notifyDanger("Het maximum aantal foto's is " + getAmountOfDamagePicturesValue());
        return true;
    }
    return false;
}


function loadSchemeOverviewForOverview() {
    $.ajax({
        url: "/umbraco/surface/AcquisitionScheme/GetSchemeOverviewWithAllDamages?userId=" + getUserId(),
        type: "GET",
        cache: false,
        success: function (data) {
            $("#scheme-in-overview").html(data);

        }
    })
}

function reactToPhotosUpload(data, form) {
    readURL(true, data);
    var imageType = $(form).find("input.imageType");
    setLoadingIconOff(form);
    if (imageType.val() === "dmgoutside" || imageType.val() === "dmginside") {
        if (isIE11()) { $(".img-item").addClass("IE-add-icon"); }
        damagePicturesUploaded += fileInput.files.length;
        setDamageButtons(imageType.val() === "dmgoutside");
        window.setTimeout(function () {
            setImageCaption(data);
            setLoadingIconOff(form);
        }, 100);// timeout so the DOM has time to add the necessary thumbnail
        if (getCurrentAmountOfDamagePictures() === 0) {
            if (isIE11()) { $(".img-item").removeClass("IE-add-icon"); }
        }
    }
}

function reactToSchemeUpload(data, form) {
    if (data.length > 0) {
        if (isIE11()) { $(".img-item").addClass("IE-add-icon"); }
        readUrlForScheme(data, currentImageType);
        schemePicturesCurrentAmount += data.length;
        window.setTimeout(function () {
            setImageCaption(data);
            setLoadingIconOff(form);
        }, 100);// timeout so the DOM has time to add the necessary thumbnail
        setLoadingIconOff(form);
    } else {
        if (getCurrentAmountOfDamagePictures() === 0) {
            if (isIE11()) { $(".img-item").removeClass("IE-add-icon"); }
        }
    }
}

function isSchemeScreen() {
    return $(schemeScreen).css('display') !== 'none';
}

function isOverviewScreen() {
    return $(overviewScreen).css('display') !== 'none';
}

function isADamageScreen() {
    return $("#panel-damage").css('display') !== 'none';
}

function postDamagesInOverview(form) {
    if ($(form).find(".addImageOverview")[0].files.length === 0) return;
    overviewForm = form;
    setLoadingIconOnDamages(form);
    $.ajax({
        url: "/umbraco/Surface/AcquisitionTool/AddOverviewDamagePhoto?isTakeOver=" + isTakeOver(),
        //`/umbraco/Surface/AcquisitionTool/AddOverviewDamagePhoto?isTakeOver=${isTakeOver()}`,
        type: "POST",
        data: new FormData(form),
        contentType: false,
        processData: false,
        cache: false,
        success: function (data) {
            setLoadingIconOff(overviewForm);
            reloadOverviewScreen(data);
        },
        error: function (error) {
            showJsonError(error, landscapeModeMessage);
            setLoadingIconOff(form);
        }
    });
}

function updatePhotoInOverview(form) {
    if ($(form).find(".update-inputfile")[0].files.length === 0) return;
    setLoadingIconOnDamages(form);
    let pictureIndex = $(form).find('input.pictureIndex').val()
    let mediaId = getMediaId(form);
    $.ajax({
        url: "/umbraco/Surface/AcquisitionTool/UpdatePhotos?isTakeOver=" + isTakeOver() + "&isOverview=true&index=" + pictureIndex + "&mediaId=" + mediaId,
        //`/umbraco/Surface/AcquisitionTool/UpdatePhotos?isTakeOver=${isTakeOver()}&isOverview=true&index=${pictureIndex}`,
        type: "PUT",
        data: new FormData(form),
        contentType: false,
        processData: false,
        cache: false,
        success: function (data) {
            setLoadingIconOff(form);
            reloadOverviewScreen(data);
        },
        error: function (error) {
            showJsonError(error, landscapeModeMessage);
            setLoadingIconOff(form);
        }
    });
}

$('#formUserInformation').submit(function (e) {
    e.preventDefault ? e.preventDefault() : event.returnValue = false; // stop the standard form submission (also in IE)

    if (!$("#formUserInformation").valid()) {
        showJsonError(error, "Niet alle velden zijn juist ingevuld");
        return;
    }
    if ($("#hasBeenSubmitted").val().toLowerCase() === "true") {
        hideNextSlideArrow();
    }

    let businessPageId = $("#businessPageId").val();

    let url = isTakeOver() ? "/umbraco/Surface/AcquisitionTool/PostTakeOverInformation" : "/umbraco/Surface/AcquisitionTool/PostDamageInformation";
    let method = "POST";
    url += "?businessPageId=" + businessPageId;
    if (isOverviewEdit) {
        url = isTakeOver() ? "/umbraco/Surface/AcquisitionTool/UpdateTakeOverInformation" : "/umbraco/Surface/AcquisitionTool/UpdateDamageInformation";
        url += "?businessPageId=" + businessPageId;
        url += "&userId=" + getUserId();
        method = "PUT";
    }

    $.ajax({
        url: url,
        type: method,
        data: new FormData(this),
        cache: false,
        contentType: false,
        processData: false,
        success: function (data) {
            if (isOverviewEdit) {
                $(userdataScreen).css('display', 'none');
                getOverViewData();
            } else {
                setHiddenClassesUploaders(data);
                setUserInformation(data);
                $("#hasBeenSubmitted").val("true");
                jumpToTop();
            }
        },
        error: function (error) {
            showJsonError(error, "Niet alle velden zijn juist ingevuld");
        }
    });
});

function getOverViewData() {
    var id = $("#mainUserId").val();
    if (id === "") {
        id = -1;
    }

    let overviewDataUrl = "GetOverviewData";
    let overviewDataDamagesUrl = "GetOverviewDataDamages";

    let action = isTakeOver() ? overviewDataUrl : overviewDataDamagesUrl;
    let overviewUrl = "/umbraco/Surface/AcquisitionTool/" + action + "?id=" + id;
    //`/umbraco/Surface/AcquisitionTool/${isTakeOver() ? overviewDataUrl : overviewDataDamagesUrl}?id=${id}`;

    $.ajax({
        url: overviewUrl,
        type: 'GET',
        dataType: 'html',
        cache: false,
        success: function (data) {
            reloadOverviewScreen(data);
            $(overviewScreen).css('display', 'block');
            $(photosScreen).css('display', 'none');
            jumpToTop();
            isOverviewEdit = true;
        },
        error: function (error) {
            showJsonError(error);
        }
    });

}
function setHiddenClasses(id) {
    $.ajax({
        url: '/umbraco/Surface/AcquisitionTool/GetUserInformation?id=' + id,
        type: 'GET',
        dataType: 'json',
        cache: false,
        success: function (data) {
            setHiddenClassesUploaders(data);
        },
        error: function (error) {
            showJsonError(error);
        }
    });
}
function postRemarks() {
    var id = $("#mainUserId").val();
    var businessPageId = $("#businessPageId").val();
    if (id === "") {
        id = -1;
    }
    var remarks = $("#userRemarks").val();
    $.ajax({
        url: '/umbraco/Surface/AcquisitionTool/FinishOverview',
        type: 'POST',
        dataType: 'json',
        cache: false,
        data: { message: remarks, id: id, businessPageId: businessPageId },
        success: function (data) {
            var userEmail = data.Email;
            $("#successUserEmail").html(userEmail);
            jumpToTop();
        },
        error: function (error) {
            showJsonError(error);
        }
    });
}

function loadNotificationMessages() {
    $.ajax({
        url: "/umbraco/Surface/GlobalResources/GetAcquisitionNotificationMessages",
        type: "GET",
        dataType: "json",
        cache: false,
        success: function (data) {
            errorMessage = data.ErrorMessage;
        }
    });
}

function getInsurances() {
    $.ajax({
        url: "/umbraco/Surface/InsuranceApi/GetInsurances",
        type: "GET",
        dataType: "json",
        cache: false,
        success: function (data) {
            insurances = data;
            populateInsuranceDropdown(insurances);
        }
    });
}

function deletePhoto() {
    var id = getUserId()
    let mediaId = getMediaId(this);
    let form = this;

    $.ajax({
        url: '/umbraco/Surface/AcquisitionTool/DeletePhoto',
        type: 'DELETE',
        data: { userId: id, mediaId: mediaId },
        cache: false,
        success: function (data) {
            if (data === true) {
                if (isSchemeScreen()) {
                    deletePhotoForScheme(form);
                }
                else if (isOverviewScreen()) {
                    deletePhotoForOverview(form);
                } else {
                    deletePhotoForDamage(form);
                }
            }
        },
        error: function (error) {
            showJsonError(error, "Het verwijderen van de foto is niet gelukt. Probeer het opnieuw voor een andere foto.");
        }
    });
}

function getMediaId(input) {
    if (isOverviewScreen()) {
        return $(input).closest("form").find("input.mediaId").val();
    }
    return $(input).closest(".thumbnail.setMarginBottomTo0").find("input.mediaId").val();
}

function deletePhotoForScheme(form) {
    let deletePictureId = getDeletePictureId(form);
    $("#" + deletePictureId).remove();
    schemePicturesCurrentAmount -= 1;
    if (schemePicturesCurrentAmount === 0) {
        if (isIE11()) { $(".img-item").removeClass("IE-add-icon"); }
    }
}

function deletePhotoForDamage(form) {
    setDamageButtons($(form).hasClass("dmgoutside"));

    $(form).parents(".img-item").remove();
    damagePicturesUploaded = getCurrentAmountOfDamagePictures();
}

function deletePhotoForOverview(form) {
    if ($(form).closest("div.col-md-3").hasClass("scheme-picture")) {
        if ($(form).closest(".scheme-picture-row").children().length === 1) {
            getOverViewData();
        } else {
            $(form).closest(".scheme-picture").remove();
        }
    } else {
        $(form).closest(".damage-modifier").remove();
        damagePicturesUploaded = getCurrentAmountOfDamagePictures();
        checkFinishOverviewButton();
    }
}

function getDeletePictureId(current) {
    if (isSchemeScreen()) {
        return $(current).closest("div.img-item")[0].id;
    }
    return current.id;
}

function setLoadingIcon(form) {
    if (isIE11()) {
        return;
    }
    let inputElement = $(form).find('input:hidden')[0];
    if (isSchemeScreen()) {
        setLoadingIconOnDamages(form);
        return;
    }
    if (inputElement.value === "dmgoutside") {
        if (dmgoutsideCount === 0) {
            changeDamageButton(inputElement.value);
        }
        dmgoutsideCount++;
    }
    else if (inputElement.value === "dmginside") {
        if (dmginsideCount === 0) {
            changeDamageButton(inputElement.value);
        }
        dmginsideCount++;
    } else {
        var label = $(form).find("label");
        label.css("display", "none");
        $(form).find("img.loadingIcon").css("display", "block");
        return;
    }
    setLoadingIconOnDamages(form);
}

function reloadOverviewScreen(data) {
    var userId = $("#mainUserId").val();
    $(overviewScreen).html(data);
    setHiddenClasses(userId);
    $(finishOverview).on("click", goToSuccess);
    var listOfDamageUploaders = Array.prototype.slice.call($("input.dmgDeleteInput"));
    listOfDamageUploaders.map(function (dmgUploader) {
        $(dmgUploader).on("click", deletePhoto);
    });
    let timeOutTime = isIE11() ? 5000 : 100;
    setTimeout(function () {
        var thumbnails = $('.loadingOverviewThumbnail');
        thumbnails.removeClass('loadingOverviewThumbnail');
        var images = thumbnails.find('img.loadingOverview');
        images.css('display', 'none');
    }, timeOutTime);
    damagePicturesUploaded = $("#amountOfDamagePicturesOverview").val();
    checkFinishOverviewButton();
    loadSchemeOverviewForOverview();

    $("#edit-userinformation").on('click', goToInformationOverviewFromOverview);
    $("#edit-scheme-overview").on('click', goToSchemeFromOverview);
    if (isIE11()) {
        $(".addDamage-label").addClass("IE-Settings");
        $(".addDamage-icon").addClass("IE-Settings");
    }
}

function setImageCaption(imageNames) {
    var thumbnail = $(".setHidden > div");
    function setCaptionPerImage(innerThumbnail, index) {
        let inner = $(innerThumbnail).html();
        let currentImageName = imageNames[index % imageNames.length].MediaName;
        let iconClass = "";
        let labelClass = "";
        let IEClass = isIE11() ? "IE-settings" : "";
        if (isSchemeScreen()) {
            iconClass = "scheme-icon";
            labelClass = "scheme-label";
        } else if (isOverviewScreen()) {
            iconClass = "overview-icon";
            labelClass = "overview-label";
        } else {
            iconClass = "damage-icon";
            labelClass = "damage-label";
        }

        let caption =
            '<div class="caption">' +
            '<input id="' + currentImageName + '" name="file" type="button" class="inputfile ' + currentImageName + '">' +
            '<h4 class="marginTop">' +
            '<label for="' + currentImageName + '" class="caption-label ' + labelClass + ' ' + currentImageName + ' ' + IEClass + ' ">' +
            '<i class="fas fa-trash updateIcon ' + iconClass + ' ' + IEClass + '"></i>' +
            '</label>' +
            '</h4>' +
            '</div>' +
            inner;
        $(innerThumbnail).html(caption);
        $(innerThumbnail).parent().removeClass("setHidden");
        if (isSchemeScreen()) {
            $("#schemeScreen ." + currentImageName).on("click", deletePhoto);
        } else {
            $(".carousel-inner ." + currentPanel + "-uploads ." + currentImageName).on("click", deletePhoto);
        }
    }
    [].forEach.call(thumbnail, setCaptionPerImage)
}

function setLoadingIconOnDamages(form) {
    $(form).find("input.inputfile").css("display", "none");
    $(form).find("img.loadingIcon").css("display", "block");
}
function setLoadingIconOff(form) {
    window.setTimeout(function () {
        $(form).find("input.inputfile").css("display", "block");
    }, 500);
    $(form).find("img.loadingIcon").css("display", "none");
}

function setLabelOn(form, setEditIcon) {
    let label = $("label[for=" + currentPanel + "]");
    label.css("display", "block");
    if (setEditIcon) {
        setIconToEdit(label);
    }
}

function setIconToEdit(label) {
    let icon = $(label).find("i");
    icon.removeClass("fa-plus");
    icon.addClass("fa-edit")
}

function showJsonError(error, errorMessage) {
    if (window.location.href.indexOf("localtest") >= 0) {
        notifyDanger(error.responseText.replace('"', '').replace('"', ''));
    } else {
        if (error.responseText === null || error.responseText === "" || error.responseText === undefined
            || errorMessage === null || errorMessage === "" || errorMessage === undefined) {
            notifyDanger("Er is iets misgegaan. Contacteer de beheerders.");
        } else {
            //notifyDanger(error.responseText.replace('"', '').replace('"', ''));
            notifyDanger(errorMessage);
        }
    }
}

function populateInsuranceDropdown(insurancesJson) {
    let dropdown = $("#Insurance");
    dropdown.empty();

    dropdown.append('<option selected="true" disabled>Kies een verzekeraar</option>');
    dropdown.prop('selectedIndex', 0);

    for (let i = 0; i < insurancesJson.length; i++) {
        dropdown.append($('<option></option>').attr('value', insurancesJson[i]).text(insurancesJson[i]));
    }
}

function isIE11() {
    return !!window.MSInputMethodContext && !!document.documentMode;
}

function jumpToTop() {
    window.scrollTo(0, 0);
}