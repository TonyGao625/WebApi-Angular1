angular.module('app').service('saveLocalStorage', ['$http', '$q', 'growl', function ($http, $q, growl) {
    this.save = function (name, customer, products) {
        if (!window.localStorage) {
            growl.error("The browser is not support localstorage, please contact administrator");
            return;
        }
        var info = {
            customer: customer,
            products: products
        };

        var localStorageInfo = JSON.stringify(info);
        localStorage[name] = localStorageInfo;
    };
    this.set = function (name, value) {
        if (!window.localStorage) {
            growl.error("The browser is not support localstorage, please contact administrator");
            return;
        }

        var localStorageInfo = JSON.stringify(value);
        localStorage[name] = localStorageInfo;
    }

    this.get = function (name) {
        if (!window.localStorage) {
            growl.error("The browser is not support localstorage, please contact administrator");
            return;
        }

        var info = localStorage[name];
        if (info === undefined || info === null) return;
        return JSON.parse(info);
    }
}])

