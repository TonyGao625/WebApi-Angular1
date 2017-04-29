angular.module("app").controller("loginCtrl", 
['session', 'auth', '$scope', '$http', 'growl', '$templateRequest', '$compile', '$window', 'loginFactory',
    function (
        session, auth, $scope, $http, growl, $templateRequest, $compile, $window, loginFactory) {

        var vm = this;
        vm.login_data = {};
        //var userInfo = session.getUser();
        //if (userInfo != null) {
        //    if (userInfo.Role == "Admin") {
        //        $window.location.href = window.configuration.clientBaseUrl + "uom";
        //    }
        //    if (userInfo.Role == "Manager") {
        //        $window.location.href = window.configuration.clientBaseUrl + "quoteorder";
        //    }
        //    if (userInfo.Role == "Customer") {
        //        $window.location.href = window.configuration.clientBaseUrl + "welcome"
        //    }
        //}
    vm.login = function () {
        auth.logIn(vm.login_data).then(function (isLogin) {
            if (isLogin)
            {
                 var userInfo = session.getUser();

                if (userInfo.Role == "Admin") {
                    $window.location.href = window.configuration.clientBaseUrl + "uom";
                }
                if (userInfo.Role == "Manager") {
                    $window.location.href = window.configuration.clientBaseUrl + "quoteorder";
                }
                if (userInfo.Role == "Customer") {
                    $window.location.href = window.configuration.clientBaseUrl + "welcome"
                }
            }
        });
    }
}])


