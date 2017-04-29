angular.module('app').directive('customerOrder', [
    function () {
        return {
            restrict: 'E',
            templateUrl: '/Scripts/Component/customerOrder/customerOrder.html',
            controller: 'customerOrderCtrl',
            controllerAs: 'customerOrderCtrl'
        }
    }
]);
