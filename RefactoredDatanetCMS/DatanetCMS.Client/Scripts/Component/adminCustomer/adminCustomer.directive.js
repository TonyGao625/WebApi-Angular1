angular.module('app').directive('adminCustomer', [
    function () {
        return {
            restrict: 'E',
            templateUrl: '/Scripts/Component/adminCustomer/adminCustomer.html',
            controller: 'adminCustomerCtrl',
            controllerAs: 'adminCustomerCtrl'
        }
    }
]);