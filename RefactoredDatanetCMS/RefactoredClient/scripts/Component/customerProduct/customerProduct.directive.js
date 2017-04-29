angular.module('app').directive('customerProduct', [
    function () {
        return {
            restrict: 'E',
            templateUrl: '/Scripts/Component/customerProduct/customerProduct.html',
            controller: 'customerProductCtrl',
            controllerAs: 'customerProductCtrl'
        }
    }
]);