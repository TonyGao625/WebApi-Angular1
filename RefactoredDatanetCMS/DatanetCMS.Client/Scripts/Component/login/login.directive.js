angular.module('app').directive('login', [
    function () {
        return {
            restrict: 'E',
            templateUrl: '/Scripts/Component/login/login.html',
            controller: 'loginCtrl',
            controllerAs: 'loginCtrl'
        }
    }
]);