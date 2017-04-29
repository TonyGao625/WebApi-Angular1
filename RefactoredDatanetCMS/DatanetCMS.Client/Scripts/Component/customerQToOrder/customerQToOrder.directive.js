angular.module('app').directive('customerQtoOrder', [
    function () {
        return {
            restrict: 'E',
            templateUrl: '/Scripts/Component/customerQToOrder/customerQToOrder.html',
            controller: 'customerQToOrderCtrl',
            controllerAs: 'customerQToOrderCtrl'
        }
    }
]);