angular.module('app').config(['$stateProvider', '$urlRouterProvider',
    function ($stateProvider, $urlRouterProvider)
    {
        //$urlRouterProvider.when('', 'home');
        $stateProvider
            .state('uom', {
                url: "/uom",
                templateUrl: "TemplateViews/adminuom.html"
            })
        ;
    }
]);