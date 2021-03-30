const progressIncrease = 6; // 80/ hoeveelheid foto's
const userdataButton = "#userdataButton";
const goBackToOverview = "#go-back-to-overview";
const finishOverviewButton = "#finishOverviewButton";
const userdataScreen = "#userdataScreen";
const photosScreen = "#photosScreen";
const overviewScreen = "#overviewScreen";
const successScreen = "#successScreen";
const schemeScreen = "#schemeScreen";
const schemeContinue = ".scheme-continue";
const schemeSkip = ".scheme-skip";
const schemeGoToOverview = ".go-to-overview";
const isInsured = "#is-insured";

const step1 = "information";
const step2 = "photos";
const step3 = "overview";
const step4 = "scheme";
let schemeContinueClicked = false;
let isOverviewEdit = false;

// Hide all the screens except information screen
$(schemeScreen).css('display', 'none');
$(photosScreen).css('display', 'none');
$(overviewScreen).css('display', 'none');
$(successScreen).css('display', 'none');

// Go from photos screen to information screen
function goToInformationOverview() {
    setNextProgressElementActive($("#progressPhotos"), $("#progressInformation"));
    $(userdataScreen).css('display', 'block');
    $(photosScreen).css('display', 'none');
    $(schemeScreen).css('display', 'none');
}

function goToInformationOverviewFromOverview() {
    $(userdataScreen).css('display', 'block');
    $(overviewScreen).css('display', 'none');
    $(goBackToOverview).closest("div").removeClass("hidden");
    $(userdataButton).addClass("hidden");
    if (!isTakeOver()) {
        $(".is-user-insured-checkbox").addClass("hidden");
    }
}

function goToSchemeFromOverview() {
    loadSchemeOverview(getUserId(), true);
}

function goToSchemeOverview() {
    setNextProgressElementActive($("#progressPhotos"), $("#progressInformation"));
    $(schemeScreen).css('display', 'block');
    $(photosScreen).css('display', 'none');
}

function goFromSchemeToPhotos() {
    sendData(currentImageId, getData());
    hideNextSlideArrow();
    skipFromSchemeToPhotos();
    updateProgressBar(step1);
    jumpToTop();
}

function skipFromSchemeToPhotos() {
    $(schemeScreen).css('display', 'none');
    $(photosScreen).css('display', 'block');
    updateProgressBar(step1);
}

$(schemeContinue).on('click', function () {
    //imageClickEvent();
    goFromSchemeToPhotos();
});

$(schemeSkip).on('click', function () {
    skipFromSchemeToPhotos();
})

$(schemeGoToOverview).on('click', function () {
    var data = getData();
    if (!haveAnyFieldsChanged(data)) {
        sendData(currentImageId, data, true);
    } else {
        $(schemeScreen).css('display', 'none');
        getOverViewData();
    }
})

$(isInsured).on('change', function () {
    if ($(isInsured).is(":checked")) {
        $(".makelaar").removeClass("hidden");
        $("#verzekering").removeClass("hidden");
    } else {
        $(".makelaar").addClass("hidden");
        $("#verzekering").addClass("hidden");
    }
})

// Go from information screen to photos screen
$(userdataButton).on('click',
    function () {
        if ($("#formUserInformation").valid()) {
            $(userdataScreen).css('display', 'none');
            if (isTakeOver()) {
                $(photosScreen).css('display', 'block');
                updateProgressBar(step1);
            } else {
                loadSchemeOverview(0);
                $(schemeScreen).css('display', 'block');
                $('#scheme-modal').modal('show');
                updateProgressBar(step4);
            }
        }
    });

//$(goBackToOverview).on("click", function () {
//    if ($("#formUserInformation").valid()) {
//        $(userdataScreen).css('display', 'none');
//        getOverViewData();
//    }
//})

// Go to scheme screen
function showScheme() {
    $(schemeScreen).css('display', 'block');
}

// Go from photos screen to overview screen
function goToOverview() {
    setNextProgressElementActive($("#progressPhotos"), $("#progressOverview"));
    hideUserInformation();
    getOverViewData();
}
// Go from overview screen to success screen

function goToSuccess() {
    $(overviewScreen).css('display', 'none');
    $(successScreen).css('display', 'block');
    updateProgressBar(step3);
    setNextProgressElementActive($("#progressOverview"), $("#progressComplete"));
    $('html,body').scrollTop(0);
    postRemarks();
    if (isTakeOver()) {
        $(".success-damages").remove();
    } else {
        $(".success-takeover").remove();
    }
};

// Change the skip button to the continue button
function changeDamageButton(imageType) {
    let idSkip = "#btnSkip-" + imageType;
    let idContinue = "#btnContinue-" + imageType;
    $(idSkip).addClass("hideOnLoad");
    $(idContinue).removeClass("hideOnLoad");
}

function setHiddenClassesUploaders(data) {
    $(".inputUserLicensePlate").val(data.LicensePlate);
    $(".inputUserId").val(data.Id);
    $(".inputUserName").val(data.Name);
    $("#mainUserId").val(data.Id);
    $("#isInsured").val(data.IsInsured);
}

function isUserInsured() {
    return $("#isInsured").val() === 'true';
}

function setUserInformation(data) {
    $(".userinformation-data").removeClass("hidden");
    let userString = data.Name + " - " + data.Email + " - " + data.LicensePlate;
    $(".userinformation-info").html(userString);
    if (isUserInsured()) {
        let agentName = data.AgentName ? " - " + data.AgentName : "";
        let agentEmail = data.AgentEmail ? " - " + data.AgentEmail : "";
        let insuranceString = data.Insurance + agentName + agentEmail;
        $(".userinformation-insurance").html(insuranceString);
    } else {
        $(".userinformation-insurance-text").addClass("hidden");
        $(".userinformation-insurance").html("Niet verzekerd");
        $(".userinformation-insurance").addClass("bold");
    }
    if (isTakeOver()) {
        $(".userinformation-insurance").addClass("hidden");
    }
}

function hideUserInformation() {
    $(".userinformation-data").addClass("hidden");
    $(".marginTopBreadCrumb").css("margin-top", "190px !important");
}

/* Applies the unobtrusive validation of the "RequiredIf" custom attribute. */
(function ($) {
    $.validator.addMethod("requiredif", function (value, element, params) {
        var isRequiredElement = $("[name='" + params + "']");
        var isRequired = isRequiredElement.val() == "true";

        if (isRequired == true && !value)
            return false;

        return true;
    });
    $.validator.unobtrusive.adapters.addSingleVal("requiredif", "otherproperty");

}(jQuery));


$(document).ready(function () {
    hideUserInfo(isTakeOver());
});

function hideUserInfo(isOvername) {
    $(".makelaar").addClass("hidden");
    $("#verzekering").addClass("hidden");
    if (!isOvername) {
        $("#tweedehandsboekje-status").addClass("hidden");
        $("#bandenstatus").addClass("hidden");
    }
    else {
        //$("#damage-chassis-number").remove();
        $("#chassis-number").remove();
    }
}

function isTakeOver() {
    let path = window.location.pathname;
    return path.indexOf('overname') >= 0;
}

//function setupPhoneInputs() {
//    let phoneInput = $("#mobile-phone");
//    window.intlTelInput(phoneInput, {
//        onlyCountries: ["be","nl"]
//        //utilsScript: "../../build/js/utils.js?1603274336113" // just for formatting/placeholders etc
//    });
//}