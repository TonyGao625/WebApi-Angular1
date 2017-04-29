angular.module('app').directive('customerQuote', [
    function () {
        return {
            restrict: 'E',
            templateUrl: '/Scripts/Component/customerQuote/customerQuote.html',
            controller: 'customerQuoteCtrl',
            controllerAs: 'customerQuoteCtrl'
        }
    }
]);
