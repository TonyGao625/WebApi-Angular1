(function (angular) {

    function sessionService(localStorage) {

        // Instantiate data when service
        // is loaded

        this._userInfo = JSON.parse(localStorage.getItem('session.userInfo'));
        this._user = JSON.parse(localStorage.getItem('session.user'));
        this._accessToken = localStorage.getItem('session.accessToken');

        this.getUser = function () {
            return this._user;
        };

        this.getUserInfo = function () {
            return this._userinfo;
        }

        this.setUser = function (user) {
            this._user = user;
            localStorage.setItem('session.user', JSON.stringify(user));
            return this;
        };

        this.setUserInfo = function (userInfo) {
            this._userInfo = userInfo;
            localStorage.setItem('session.userInfo', JSON.stringify(userInfo));
            return this;
        }

        this.getAccessToken = function () {
            return this._accessToken;
        };

        this.setAccessToken = function (token) {
            this._accessToken = token;
            localStorage.setItem('session.accessToken', token);
            return this;
        };

        /**
         * Destroy session
         */
        this.destroy = function destroy() {
            this.setUser(null);
            this.setAccessToken(null);
            localStorage.clear();
        };

    }

    // Inject dependencies
    sessionService.$inject = ['localStorage'];

    // Export
    angular
        .module('app')
        .service('session', sessionService);

})(angular);