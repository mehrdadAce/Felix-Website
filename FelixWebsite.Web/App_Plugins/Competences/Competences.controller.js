angular.module('umbraco.directives').controller('CompetenceController', function ($scope, $http, $window, $location) {

    $scope.competencepatterns = undefined;
    $scope.competencepattern = undefined;
    $scope.selectedcompetences = { 'id': undefined, 'keyword': undefined, 'patternName': undefined, 'competences': undefined };

    if ($scope.model.value.id != undefined && $scope.model.value.keyword != undefined) {

        $scope.selectedcompetences.id = $scope.model.value.id;
        $scope.selectedcompetences.keyword = $scope.model.value.keyword;
        $scope.selectedcompetences.patternName = $scope.model.value.patternName;

        GetCompetencePatterns($scope.model.value.keyword)

        if ($scope.model.value.competences != undefined) {
            $scope.selected = $scope.model.value.competences;
            $scope.selectedcompetences.competences = $scope.model.value.competences; 
        }
    }

    console.log($scope.model.value);

    // 1. Get competence patterns by keyword.
    function GetCompetencePatterns(keyword) {
        console.log("Getting all patterns for keyword '" + keyword + "'...");

        $.ajax({
            url: "/umbraco/backoffice/Competence/GetCompetences?keyword=" + keyword,
            dataType: "json",
            type: "GET",
            contentType: false,
            processData: false,
            success: function (data) {
                
                $scope.selectedcompetences.keyword = keyword;
                $scope.competencepatterns = JSON.parse(data);
                $scope.$apply();

                console.log($scope.competencepatterns);

            }, error: function (data) {
                console.log("Error:" + data);
            }
        });
    }

    // 2. Get competences by competence pattern code.
    function GetCompetencePattern(code,title) {
        console.log("Getting competences for pattern with code " + code +  " title " + title + "...");
        $.ajax({
            url: "/umbraco/backoffice/Competence/GetCompetencePattern?code=" + code,
            dataType: "json",
            type: "GET",
            contentType: false,
            processData: false,
            success: function (data) {

                $scope.competencepattern = JSON.parse(data);
                $scope.$apply();

                console.log($scope.competencepattern);
            }, error: function (data) {
                console.log("Error:" + data);
            }
        });
    }

    $scope.GetCompetences = function () {
        $scope.competencepatterns = undefined;
        GetCompetencePatterns($scope.keywords);
        $scope.keywords = '';
    }

    $scope.GetPattern = function (code, titel) {
        $scope.competencepattern = undefined;
        $scope.selectedcompetences.id = code;
        $scope.selectedcompetences.patternName = titel;
        GetCompetencePattern(code, titel);
    }

    $scope.localSelectChange = function () {
        $scope.selectedcompetences.competences = this.selected;
    }

    $scope.$on("formSubmitting", function (e, params) {
        if (params.action === "save") {
            if ($scope.selectedcompetences.competences != undefined) {
                $scope.model.value = $scope.selectedcompetences;
            }
        } else if (params.action === "publish") {
            if ($scope.selectedcompetences.competences != undefined) {
                $scope.model.value = $scope.selectedcompetences;
            }
        }
    });
});