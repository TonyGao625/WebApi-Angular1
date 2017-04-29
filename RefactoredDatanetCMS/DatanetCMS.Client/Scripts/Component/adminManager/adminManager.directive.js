angular.module('app').directive('adminManager', [
    function () {
        return {
            restrict: 'E',
            templateUrl: '/Scripts/Component/adminManager/adminManager.html',
            controller: 'adminManagerCtrl',
            controllerAs: 'adminManagerCtrl'
        }
    }
]);