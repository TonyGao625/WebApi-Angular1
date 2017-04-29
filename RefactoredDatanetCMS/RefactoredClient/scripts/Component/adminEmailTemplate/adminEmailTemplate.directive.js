angular.module('app').directive('adminEmailTemplate', [
    function () {
        return {
            restrict: 'E',
            templateUrl: '/Scripts/Component/adminEmailTemplate/adminEmailTemplate.html',
            controller: 'adminEmailTemplateCtrl',
            controllerAs: 'adminEmailTemplateCtrl'
        }
    }
]);