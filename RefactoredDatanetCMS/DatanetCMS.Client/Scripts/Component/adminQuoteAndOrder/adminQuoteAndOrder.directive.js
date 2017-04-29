angular.module('app').directive('quoteAndOrder', [
    function () {
        return {
            restrict: 'E',
            templateUrl: '/Scripts/Component/adminQuoteAndOrder/adminQuoteAndOrder.html',
            controller: 'adminQuoteAndOrderCtrl',
            controllerAs: 'adminQuoteAndOrderCtrl'
        }
    }
]);