angular.module('app').factory('BearerAuthInterceptor', ['$window', '$q','session', function ($window, $q, session) {
    return {
        request: function (config) {

            if (config.url === window.configuration.clientBaseUrl + "/home/login")
                return config;
            config.headers = config.headers || {};
            var accessToken = session.getAccessToken();
            if (accessToken) {
                // may also use sessionStorage
                config.headers.Authorization = 'Bearer ' + accessToken;
            }
            return config || $q.when(config);
        },
        response: function (response) {
            if (response.status === 401) {
                $window.location.href = window.configuration.clientBaseUrl + 'login';
            }
            return response || $q.when(response);
        },
        responseError: function (response) {
            if (response.status === 401) {
                $window.location.href = window.configuration.clientBaseUrl + 'login';
            }
            return response || $q.when(response);
        }
    };
}]);