angular.module('app').service('httpRequest', ['$http', '$q', 'growl', '$rootScope','session',
    function ($http, $q, growl, $rootScope, session) {
        this.get = function (url, data) {
        var accessToken = session.getAccessToken();
        var deferred = $q.defer();
        $rootScope.loading = 30;
        $http.get(url, data)
            //.get(url, data, { headers: { "Authorization": 'Bearer ' + accessToken}})
            .then(function (res) {
            $rootScope.loading = 100;
            if (res.data && res.data.Status ===0) {
                deferred.resolve(res);
            } else {
                if (!res.data) {
                    growl.error("Error happened inside");
                }
                else {
                    growl.error(res.data.Message);
                }
                deferred.reject(res);
            }
        }, function (res) {
            $rootScope.loading = 100;
            growl.error("Error happened inside");
            deferred.reject(res);
        });
        return deferred.promise;
    };
    this.post = function (url, data) {
        var accessToken = session.getAccessToken();
        var deferred = $q.defer();
        $rootScope.loading = 30;
        $http.post(url, data)
            //.post(url, data, { headers: { "Authorization": 'Bearer ' + accessToken } })
            .then(function (res) {
            $rootScope.loading = 100;
            if (res.data.Status === 0) {
                deferred.resolve(res);
            } else {
                growl.error(res.data.Message);
                deferred.reject(res);
            }
        }, function (res) {
            $rootScope.loading = 100;
            growl.error("Error happened inside");
            deferred.reject(res);
        });
        return deferred.promise;
    }

}])