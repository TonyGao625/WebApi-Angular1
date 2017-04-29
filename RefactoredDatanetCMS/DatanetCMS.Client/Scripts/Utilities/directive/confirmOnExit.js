//angular.module("app").directive('confirmOnExit', function () {
//    return {
//        link: function ($scope, elem, attrs) {
//            window.onbeforeunload = function () {
//                    return "Are you sure want to leave?";
//            }
//            $scope.$on('$locationChangeStart', function (event, next, current) {
//                if (!$(".btn-info:disabled").length) {
//                    if (!confirm("Are you sure want to leave?")) {
//                        event.defaultPrevented();
//                    }
//                }
//            });
//        }
//    };
//});

angular.module("app").directive('confirmOnExit', ["$location", "$rootScope", function ($location, $rootScope) {
    return {
        scope: {
            confirmOnExit: '&',
            confirmMessageWindow: '@',
            confirmMessageRoute: '@',
            confirmMessage: '@'
        },
        link: function ($scope, elem, attrs) {
            window.onbeforeunload = function () {
                if ($scope.confirmOnExit() && $rootScope.backToProduct) {
                    return $scope.confirmMessageWindow || $scope.confirmMessage;
                }
                
            }
            //var $locationChangeStartUnbind = $scope.$on('$locationChangeStart', function (event, toState, toParams, fromState, fromParams) {
            //    if ($scope.confirmOnExit()) {
            //        if (!confirm($scope.confirmMessageRoute || $scope.confirmMessage)) {
            //            event.defaultPrevented();
            //        }
            //    }
            //});
            //$scope.$on('$destroy', function () {
            //    window.onbeforeunload = null;
            //    $locationChangeStartUnbind();
            //});
        }
    };
}]);