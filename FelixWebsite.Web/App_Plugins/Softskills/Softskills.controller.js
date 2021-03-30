angular.module('umbraco.directives').controller("SoftskillController", function ($scope , $http) {

    $scope.selected = $scope.model.value;

    var getData = function() {
        $http({
            method: 'GET', url: 'https://api-cbt.vdab.be/services/openservices/v1/softskills', headers: { 'Content-Type': 'application/json; charset=utf-8', 'X-IBM-Client-Id': '476f077b-67c0-4380-aae2-3f4d21ccbc21' }})
            .success(function (data, status) {
                $scope.softskills = data;
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