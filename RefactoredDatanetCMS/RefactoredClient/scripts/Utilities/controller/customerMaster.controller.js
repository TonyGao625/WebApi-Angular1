angular.module("app").controller('customerMasterCtrl', ['$scope', 'growl', '$window',
    'saveLocalStorage',
    'loginFactory',
    'session',
    'auth',
    function (
        $scope,
        growl,
        $window,
        saveLocalStorage,
        loginFactory,
        session,
    auth) {
        $scope.init = function () {
            var userInfo = session.getUser();
            if (!auth.isLoggedIn() || userInfo.Role !== 'Customer') {
                $window.location.href = window.configuration.clientBaseUrl + "Login";
            }
            $scope.userInfo = userInfo;
            $scope.Logo = userInfo.Logo;
        }

        $scope.logout = function () {
            bootbox.confirm("Are you sure you want to log out ?", function (res) {
                if (!res) {
                    return;
                }
                auth.logOut();
                $window.location.href = window.configuration.clientBaseUrl + "Login";
            });
        }
    }]);

