angular.module("umbraco").factory('smService',
    function ($location, notificationsService) {

        return {
            notifySuccess: function () {
                notificationsService.success("De geselecteerde reviews zijn opgeslagen in Content");
            },
            notifyWarning: function () {
                notificationsService.warning("Opgepast!", "Er is iets mis gegaan tijdens het opslaan, probeer het later nogmaals.");
            },
            notifyError: function () {
                notificationsService.error("Er is iets mis gegaan, probeer het later opnieuw");
            },
            createDateString: function(dateString) {
                return moment(dateString, "YYYY-MM-DDTHH:mm:ss.sssZ").format("YYYY-MM-DD");
            },
            createTimeString: function(dateString) {
                return moment(dateString, "YYYY-MM-DDTHH:mm:ss.sssZ").format("HH:mm:ss");
            },
            getStoreId: function(firstSplitString) {
                var url = $location.absUrl();
                var parameters = url.split(firstSplitString)[1];
                var firstParam = parameters.split("pageId=")[0];
                var storeIdWithQuestionMark = firstParam.split("storeId=")[1];
                return storeIdWithQuestionMark.split("&")[0];       
            },
            getPageId: function (firstSplitString) {
                var url = $location.absUrl();
                var parameters = url.split(firstSplitString)[1];
                return parameters.split("pageId=")[1];
            },
            getAccessToken: function (firstSplitString) {
                var url = $location.absUrl();   
                var parameters = url.split(firstSplitString)[1];
                return parameters.split("accessToken=")[1];
            },
            getReviews: function (reviewUrl, callback, errCallback) {
                // callback and errcallback are used in the calling function
                return $.ajax({
                    url: reviewUrl,
                    cache: false,
                    method: "GET",
                    dataType: 'json'
                });
            },
            checkPath: function(path) {
                return path !== "/";
            },
           
        };
    });