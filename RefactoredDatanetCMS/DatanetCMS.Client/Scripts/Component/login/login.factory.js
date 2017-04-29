angular.module('app').factory('loginFactory', ['httpRequest', function (httpRequest) {
    var url = window.configuration.baseUrl + "account/";
    var factory = {
        login: login,
        logout: logout
    }
    function login(data) {
            return httpRequest.post(url + "Login", data);
    };

    function logout() {
        return httpRequest.get(url + "Logout");
    }
    
    return factory;

}])