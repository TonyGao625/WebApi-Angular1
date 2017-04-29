angular.module('app').directive('adminBuyerGroup', [
    function () {
        return {
            restrict: 'E',
            templateUrl: '/Scripts/Component/adminBuyerGroup/adminBuyerGroup.html',
            controller: 'adminBuyerGroupCtrl',
            controllerAs: 'adminBuyerGroupCtrl'
        }
    }
]);