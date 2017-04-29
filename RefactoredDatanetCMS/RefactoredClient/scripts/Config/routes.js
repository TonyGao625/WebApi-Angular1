angular.module('app').config(['$stateProvider', '$urlRouterProvider',
    function ($stateProvider, $urlRouterProvider)
    {
        //$urlRouterProvider.when('', 'home');
        $stateProvider
            .state('uom', {
                url: "/uom",
                templateUrl: "TemplateViews/adminuom.html"
            }).state('category', {
                url: "/category",
                templateUrl: "TemplateViews/admincategory.html"
            }).state('product', {
                url: "/product",
                templateUrl: "TemplateViews/adminproduct.html"
            }).state('EmailTemplate', {
                url: "/EmailTemplate",
                templateUrl: "TemplateViews/adminEmailTemplate.html"
            }).state('manager', {
                url: "/manager",
                templateUrl: "TemplateViews/adminmanager.html"
            }).state('customer', {
                url: "/customer",
                templateUrl: "TemplateViews/admincustomer.html"
            }).state('buyergroup', {
                url: "/buyergroup",
                templateUrl: "TemplateViews/adminbuyergroup.html"
            }).state('quoteandorder', {
                url: "/quoteandorder",
                templateUrl: "TemplateViews/adminquoteandorder.html"
            })
        ;
    }
]);