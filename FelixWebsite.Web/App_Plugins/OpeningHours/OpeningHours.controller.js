var app = angular.module("umbraco");

/* Directive */
angular.module("umbraco.directives").directive('myTimePicker', function (assetsService) {
    return {
        restrict: 'EA',
        replace: true,
        template: '<input class="datepickerinput" type="text" />',
        require: "ngModel",
        link: function (scope, element, attrs, ngModelCtrl) {
            // load css file for the date picker
            assetsService.loadCss('lib/datetimepicker/bootstrap-datetimepicker.min.css', scope);
            // load the js file for the date picker
            assetsService.loadJs('lib/datetimepicker/bootstrap-datetimepicker.js', scope).then(function () {
                // init date picker
                // initDatePicker();         
                element.datetimepicker({
                    pickDate: false,
                    pickTime: true,
                    useSeconds: false,
                    format: "HH:mm",
                    icons: {
                        time: "icon-time",
                        date: "icon-calendar",
                        up: "icon-chevron-up",
                        down: "icon-chevron-down"
                    }
                }).on('dp.change',
                    function (e) {
                        ngModelCtrl.$setViewValue(e.date.format("HH:mm"));
                        scope.$apply();
                    });
            });
        }
    };
});

angular.module('umbraco.directives').controller("OpeningHoursController",
    function ($scope) {
        var ohC = this;
        var dayArray = ['Maandag', "Dinsdag", "Woensdag", "Donderdag", "Vrijdag", "Zaterdag", "Zondag"];
        $scope.databaseWeek = $scope.model.value;
        ohC.week = initializeWeek();
       
        function initializeWeek() {
            if ($scope.databaseWeek === undefined || $scope.databaseWeek === "" || $scope.databaseWeek === typeof(new Week())) {
                var week = new Week();
                week.days = [];
                dayArray.forEach(day => {
                    var newDay = createStandardDay(day);
                    week.days.push(newDay);
                });
                $scope.databaseWeek = week;
                $scope.model.value = week;
                return week;
            } 
            return $scope.databaseWeek;
        }

        function createStandardDay(day) {
            return {
                openings: [
                    {
                        start: "08:00",
                        end: "12:00",
                        isOpen: true
                    }, {
                        start: "13:00",
                        end: "17:00",
                        isOpen: true
                    }
                ],
                dayName: day
            };
        }
     
        $scope.$on("formSubmitting", function(e, params) {
            if (params.action === "save") {
                $scope.model.value = JSON.stringify($scope.databaseWeek);
            } else if (params.action === "publish") {
                console.log($scope.databaseWeek);
                $scope.model.value = JSON.stringify($scope.databaseWeek);
            }
        });
    }
);
//http://umbraco.github.io/Belle/#/tutorials/CreatingAPropertyEditor
