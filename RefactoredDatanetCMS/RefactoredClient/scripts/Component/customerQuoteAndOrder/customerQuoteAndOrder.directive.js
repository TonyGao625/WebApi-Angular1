angular.module('app').directive('customerQandOrder', [
    function () {
        return {
            restrict: 'E',
            templateUrl: '/Scripts/Component/customerQuoteAndOrder/customerQuoteAndOrder.html',
            controller: 'customerQAndOCtrl',
            controllerAs: 'customerQAndOCtrl'
        }
    }
]);