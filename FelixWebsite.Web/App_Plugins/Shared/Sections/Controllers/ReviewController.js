angular.module("umbraco").controller('ReviewController',
    function ($scope, $location) {
        $scope.getParameterQuery = function() {
            var url = $location.absUrl();
            var split1 = url.split(".html%3F")[1];
            console.log(split1);
            return split1;
        };

    });
