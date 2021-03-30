"use strict";
function getBusinessInfoFromUmbraco(mailadress, carModel) {

    mailadress = validateMailAdress(mailadress);

    const settings = {
        "async": true,
        "crossDomain": true,
        "url": "/umbraco/surface/UmbracoBusiness/GetUmbracoBusinessByMailAdress?mailAddress=" + mailadress + "&carModel="+ carModel, 
        "method": "GET"
    };

    $.ajax(settings).done(function (response) {
        $("#businessPicture").attr("src", response.ImageUrl);
        $("#businessLink").attr("href", response.Link);
        $("#businessMail").html("Mail: <a href='mailto:" + response.Mail + "'>" + response.Mail + "</a>");
        $("#BusinessMailModal").attr("value", response.Mail);
        $("#businessPhone").text("Tel: " + response.Phone);
        $("#businessAdress").text(response.Adress);  
        $("#businessName").text(response.Brand + " " + response.Name + ", " + response.Location);
        // set testdrive and contact modal hidden fields
        $(".location-hidden").val(response.Id);
        $(".mail-to-hidden").val(response.Mail);
        $(".business-hidden").val(response.Brand);
        $(".car-model-hidden").val(response.CarModel);
        $(".url-hidden").val(window.location);
    });
}

function validateMailAdress(mailadress) {
    // console.log("Mailadress from carflow: " + mailadress);

    if (mailadress === undefined || mailadress === "") {
        console.log("Mailadress undefined, using tim@felix.be");
        mailadress = 'tim@felix.be';
    }

    return mailadress;
}