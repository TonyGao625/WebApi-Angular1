angular.module('app').config([
    'growlProvider', "$httpProvider",
    function (growlProvider, $httpProvider) {
        /*IE CACHE PROBLEM*/
        if (!$httpProvider.defaults.headers.get) {
            $httpProvider.defaults.headers.get = {};
        }

        // Answer edited to include suggestions from comments
        // because previous version of code introduced browser-related errors

        //disable IE ajax request caching
        $httpProvider.defaults.headers.get['If-Modified-Since'] = 'Mon, 26 Jul 1997 05:00:00 GMT';
        // extra
        $httpProvider.defaults.headers.get['Cache-Control'] = 'no-cache';
        $httpProvider.defaults.headers.get['Pragma'] = 'no-cache';
        
        $httpProvider.interceptors.push('BearerAuthInterceptor');
        growlProvider.onlyUniqueMessages(true);
        growlProvider.globalPosition('top-center');
        growlProvider.globalTimeToLive(3000);
        growlProvider.globalReversedOrder(true);
        growlProvider.globalDisableCountDown(true);
    }
]);

angular.module('app').constant('TIME', moment().utcOffset() / 60)