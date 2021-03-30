angular.module('umbraco.directives').controller("LanguageController", function ($scope , $http) {

    $scope.languages = [
        {
            "code": "NL",
            "name": "Nederlands",
            "selected": true,
            "score": {
                "value": 5,
                "alias": "Uitstekend"
            }
        },
        {
            "code": "FR",
            "name": "Frans",
            "selected": false,
            "score": {
                "value": 1,
                "alias": "Basis"
            }
        },
        {
            "code": "DE",
            "name": "Duits",
            "selected": false,
            "score": {
                "value": 1,
                "alias": "Basis"
            }
        },
        {
            "code": "EN",
            "name": "Engels",
            "selected": false,
            "score": {
                "value": 1,
                "alias": "Basis"
            }
        }
    ];

    if ($scope.model.value != undefined && $scope.model.value != "") {
        $scope.languages = $scope.model.value;
    }

    $scope.scores = [
        {
            "value": 1,
            "alias": "basis"
        },
        {
            "value": 2,
            "alias": "goed"
        },
        {
            "value": 3,
            "alias": "zeer goed"
        },
        {
            "value": 4,
            "alias": "uitstekend"
        }
    ];

    $scope.$on("formSubmitting", function (e, params) {
        if (params.action === "save") {
            $scope.model.value = $scope.languages;
        } else if (params.action === "publish") {
            $scope.model.value = $scope.languages;
        }
    });

});