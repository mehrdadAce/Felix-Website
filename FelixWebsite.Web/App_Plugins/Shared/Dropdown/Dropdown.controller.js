angular.module('umbraco.directives').controller("DropdownController", function ($scope, $http) {

    if ($scope.model.value != undefined && $scope.model.value != "") {
        $scope.selected = $scope.model.value; 
    }
  
    $scope.GetData = function (url) {
        $http({
            method: 'GET',
            url: url,
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'X-IBM-Client-Id': '476f077b-67c0-4380-aae2-3f4d21ccbc21'
            }
        })
        .success(function (data, status) {
            $scope.data = data;
        })
        .error(function (data, status) {
            return "Error collecting data from VDAB.";
        })
    }

    $scope.$on("formSubmitting", function (e, params) {
        if (params.action === "save") {
            $scope.model.value = $scope.selected;s
        } else if (params.action === "publish") {
            $scope.model.value = $scope.selected;
        }
    });
});