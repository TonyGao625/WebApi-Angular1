angular.module('app').directive('adminProduct', [
    function () {
        return {
            restrict: 'E',
            templateUrl: '/Scripts/Component/adminProduct/adminProduct.html',
            controller: 'adminProductCtrl',
            controllerAs: 'adminProductCtrl'
        }
    }
]);