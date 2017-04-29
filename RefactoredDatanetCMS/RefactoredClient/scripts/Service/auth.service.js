(function (angular) {

    function AuthService($http, session, $rootScope, growl) {
        var baseUrl = window.configuration.baseUrl;

        /**
        * Check whether the user is logged in
        * @returns boolean
        */
        this.isLoggedIn = function isLoggedIn() {
            return session.getUser() !== null;
        };

        /**
        * Log in
        *
        * @param credentials
        * @returns {*|Promise}
        */
        this.logIn = function (credentials) {
            $rootScope.loading = 30;
            return $http
                .post(baseUrl + 'account/login', credentials)
                .then(function (response) {
                    $rootScope.loading = 100;
                    if (response.data && response.data.Status === 0) {
                        var data = response.data;
                        session.setUser(data.UserInfo);
                        //session.setUserInfo(data.UserInfo.User);
                        session.setAccessToken(data.AccessToken);
                        return true;
                    }
                    else {
                        if (!response.data) {
                            growl.error("Error happened inside");
                        }
                        else {
                            growl.error(response.data.Message);
                        }
                        return false;
                    }
                },
                function (res) {
                    $rootScope.loading = 100;
                    growl.error("Error happened inside");
                    return false;
                })
        };

        /**
        * Log out
        *
        * @returns {*|Promise}
        */
        this.logOut = function () {
            session.destroy();
        };

    }

    // Inject dependencies
    AuthService.$inject = ['$http', 'session', '$rootScope', 'growl'];

    // Export
    angular
        .module('app')
        .service('auth', AuthService);

})(angular);