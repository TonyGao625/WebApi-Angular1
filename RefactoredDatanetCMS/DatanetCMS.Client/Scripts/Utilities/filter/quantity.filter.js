(function (app) {
    'use strict';

    app.filter('quantityFilter', quantityFilter);

    function quantityFilter() {
        return function (input) {
            var result = /^[0-9]*$/.test(input);
            if (!result) {
                input = 0;
            }
            if (input < 0) {
                input = 0;
            }
            return input;
        }
    }

})(angular.module('app'));