angular.module('umbraco').controller('GoogleController',
    function ($scope, smService) {
        $scope.dataIsLoaded = false;
        $scope.data = undefined;
        $scope.dataIsBeingSaved = false;
        $scope.urlStoreId = "";

        $scope.initFunction = function () {
            const splitOn = "GoogleReviews?";
            $scope.urlStoreId = smService.getStoreId(splitOn);
            $scope.urlPageId = smService.getPageId(splitOn);
            $scope.urlAccessToken = smService.getAccessToken(splitOn);
            smService.getReviews("/umbraco/backoffice/GoogleReviews/GetReviews?pageId=" + $scope.urlPageId + "&accessToken=" + $scope.urlAccessToken).
                then(resolveReviews, smService.notifyError);
        };

        function resolveReviews(data) {
            $scope.data = data;
            $scope.dataIsLoaded = true;
        }
        
        $scope.changeSelected = function ($event) {
            $event.stopPropagation();
        };
        $scope.createDateString = function (dateString) {
            return smService.createDateString(dateString);
        };
        $scope.createTimeString = function (dateString) {
            return smService.createTimeString(dateString);
        };
        $scope.sendDataToServer = function ($event) {
            $scope.dataIsBeingSaved = true;
            $.ajax({
                url: "/umbraco/backoffice/GoogleReviews/SubmitSelectedValues",
                cache: false,
                method: "POST",
                data: { formData: $scope.data, storeId: $scope.urlStoreId },
                success: successSend,
                error: errorSend
            });
            $event.stopPropagation();
        };  

        function successSend(firstReviewPath) {
            var pathIsOkay = smService.checkPath(firstReviewPath);
            if (pathIsOkay) {
                window.location.href = firstReviewPath;
                smService.notifySuccess();
            } else {
                smService.notifyError();
            }
            $scope.dataIsBeingSaved = false;
        }
        function errorSend() {
            smService.notifyError();
            $scope.dataIsBeingSaved = false;
        }
    });
