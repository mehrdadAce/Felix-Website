$(document).ready(function () {
    InitSelectSectionToggle();
    InitTestDriveDateTime();
});
function InitSelectSectionToggle() {
    const IdSelectbox = "storeId";
    const ModalLoaded = "shown.bs.modal";
    const IdQuotationModal = "#quotationModal";
    const IdTestDriveModal = "#testDriveModal";

    function ToggleSectionWithSelectbox(modal, sectionId) {
        // Disable & hide previous dropdown
        $(".model-section").css("display", "none");
        $(".model-selector").attr("disabled", true);
        // Enable & show current dropdown
        $(modal + " #" + sectionId).css("display", "block");
        $(modal + " #" + sectionId + " select").attr("disabled", false);
    }

    // ------------------------------------------
    // Show the right dropdown when modal loads
    // ------------------------------------------

    $(IdQuotationModal).on(ModalLoaded, function () {
        const selectedValue = $(IdQuotationModal + " #" + IdSelectbox).children(":selected").attr("value");
        if (selectedValue != undefined) {
            ToggleSectionWithSelectbox(IdQuotationModal, ("modelSection-" + selectedValue));
        }
    });

    $(IdTestDriveModal).on(ModalLoaded, function () {
        const selectedValue = $(IdTestDriveModal + " #" + IdSelectbox).children(":selected").attr("value");
        if (selectedValue != undefined) {
            ToggleSectionWithSelectbox(IdTestDriveModal, ("modelSection-" + selectedValue));
        }
    });

    // ------------------------------------------
    // Show the right dropdown when store changes
    // ------------------------------------------

    $(IdQuotationModal + " #" + IdSelectbox).change(function () {
        const selectedValue = $(this).children(":selected").attr("value");
        ToggleSectionWithSelectbox(IdQuotationModal, ("modelSection-" + selectedValue));
    });

    $(IdTestDriveModal + " #" + IdSelectbox).change(function () {
        const selectedValue = $(this).children(":selected").attr("value");
        ToggleSectionWithSelectbox(IdTestDriveModal, ("modelSection-" + selectedValue));
    });
}

function InitTestDriveDateTime() {
    documnt.
}

//window.onload = function () {
//    console.log("on page load - mehrdad");
//};