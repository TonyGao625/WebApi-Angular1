angular.module("app").controller('masterCtrl',
    ['$scope', 'growl', '$window', 'saveLocalStorage', 'session','auth',
    function (
        $scope,
        growl,
        $window,
        saveLocalStorage,
        session,
        auth) {
        $scope.init = function () {
            var userInfo = session.getUser();
            if (!auth.isLoggedIn() ||
                (userInfo.Role !== 'Admin' &&
                session.getUser().Role !== 'Manager')) {
                $window.location.href = window.configuration.clientBaseUrl + "Login";
                return;
            }
            $scope.userInfo = userInfo;
            if ($scope.userInfo.Role === 'Manager') {
                if ($window.location.href !== window.configuration.clientBaseUrl + "quoteorder") {
                    $window.location.href = window.configuration.clientBaseUrl + "quoteorder";
                }
            }
            return;
        }

        $scope.logout = function () {
            auth.logOut();
            $window.location.href = window.configuration.clientBaseUrl + "Login";
        }
    }]);

