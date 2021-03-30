angular.module('umbraco.directives').controller("Driverslicense", function ($scope , $http) {

    if ($scope.model.value != undefined && $scope.model.value != "" && $scope.model.value != " ") {
        $scope.selected = $scope.model.value;
    }

    var getData = function() {
        $http({
            method: 'GET', url: 'https://api.vdab.be/services/openservices/v1/rijbewijzen', headers: { 'Content-Type': 'application/json; charset=utf-8', 'X-IBM-Client-Id': '73d362a3-04cd-4ce3-baa9-743b82687346' }})
            .success(function (data, status) {
                $scope.licenses = data.filter(isNotVAK);
            })
            .error(function (data, status) {
                console.log("Error collecting data from VDAB.");
            })
    }

    function isNotVAK(value) {
        if (value.code.includes("_VAK")) {
            return false;
        } else {
            return true;
        }
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