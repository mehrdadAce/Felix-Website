angular.module('umbraco.directives').controller("DiplomaController", function ($scope , $http) {

    $scope.selected = $scope.model.value;

    var getData = function() {
        $http({
            method: 'GET', url: 'https://api.vdab.be/services/openservices/v2/diplomas', headers: { 'Content-Type': 'application/json; charset=utf-8', 'X-IBM-Client-Id': '73d362a3-04cd-4ce3-baa9-743b82687346' }})
            .success(function (data, status) {
                $scope.studies = data;
            })
            .error(function (data, status) {
                console.log("Error collecting data from VDAB.");
            })
    }

    getData();

    $scope.$on("formSubmitting", function (e, params) {
        if (params.action === "save") {
            $scope.model.value = $scope.selected;
        } else if (params.action === "publish") {
            $scope.model.value = $scope.selected;
        }
    });

});