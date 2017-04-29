angular.module('app').directive('adminUom', [
    function () {
        return {
            restrict: 'E',
            templateUrl: '/Scripts/Component/adminUom/adminUom.html',
            controller: 'adminUomCtrl',
            controllerAs: 'adminUomCtrl'
        }
    }
]);