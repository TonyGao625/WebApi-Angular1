angular.module('app').directive('customerWelcome', [
    function () {
        return {
            restrict: 'E',
            templateUrl: '/Scripts/Component/customerWelcome/customerWelcome.html',
            controller: 'customerWelcomeCtrl',
            controllerAs: 'customerWelcomeCtrl'
        }
    }
]);