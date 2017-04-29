angular.module('app').directive('adminCategory', [
    function () {
        return {
            restrict: 'E',
            templateUrl: '/Scripts/Component/adminCategory/adminCategory.html',
            controller: 'adminCategoryCtrl',
            controllerAs: 'adminCategoryCtrl'
        }
    }
]);