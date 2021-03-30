angular.module("umbraco").controller('FacebookNewsController',
    function ($scope, smService) {
        $scope.dataIsLoaded = false;
        $scope.data = undefined;
        $scope.dataIsBeingSaved = false;

        $scope.initFunction = function () {
            const splitOn = "FacebookNews?";
            $scope.urlStoreId = smService.getStoreId(splitOn);
            $scope.urlPageId = smService.getPageId(splitOn);
            $scope.urlAccesstoken = smService.getAccessToken(splitOn);
            smService.getReviews("/umbraco/backoffice/FacebookPosts/GetArticles?pageId=" + $scope.urlPageId + "&accessToken=" + $scope.urlAccesstoken).
                then(resolveReviews, smService.notifyError);
        };
        
        $scope.sendDataToServer = function ($event) {
            $scope.dataIsBeingSaved = true;
            $.ajax({
                url: "/umbraco/backoffice/FacebookPosts/SubmitSelectedValues",
                cache: false,
                method: "POST",
                data: {
                    formData: $scope.data,
                    storeId: $scope.urlStoreId
                },
                success: successfulSend,
                error: errorSend
            });
            $event.stopPropagation();
        };

        function resolveReviews(data) {
            $scope.data = data;
            $scope.dataIsLoaded = true;
            $scope.$apply();
        }
        
        function successfulSend(firstReviewPath) {
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

        $scope.changeSelected = function ($event) {
            $event.stopPropagation();
        };
        $scope.createDateString = function (dateString) {
            return smService.createDateString(dateString);
        };
        $scope.createTimeString = function (dateString) {
            return smService.createTimeString(dateString);
        };
    });
