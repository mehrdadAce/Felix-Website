$(document).ready(function () {
    $("#submit-news").click(function () {
        setConfirmMessage();
    });

    function setConfirmMessage() {
        $.ajax({
            type: "GET",
            url: "/Umbraco/Surface/Newsletter/ShowSuccesMessage",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: successFunc,
            error: errorFunc
        });

        function successFunc(data, status) {}
        function errorFunc() {}
    }

});