angular.module("app").controller('adminEmailTemplateCtrl',
    ['adminEmailTemplateFactory', '$scope', '$http', 'growl',
        function (adminEmailTemplateFactory, $scope, $http, growl) {

            $scope.init = function () {
                adminEmailTemplateFactory.getEmailTemplates()
                    .then(function (result) {
                        if (result.data.Datas.length < 1) {
                            $scope.emailSubject = "";
                            $scope.emailBody = "";
                            $scope.emailTemplateId = 0;
                        }
                        else {
                            $scope.emailBody = result.data.Datas[0].Body;
                            $scope.emailSubject = result.data.Datas[0].Subject;
                            $scope.emailTemplateId = result.data.Datas[0].Id;
                        }
                    });
            }

            $scope.saveTemplate = function () {
                adminEmailTemplateFactory.saveEmailTemplate({ "Id": $scope.emailTemplateId, "Body": $scope.emailBody, "Subject": $scope.emailSubject })
                    .then(function (result) {
                        growl.success("Email template has been add");
                    });
            }

            $scope.cancel = function () {
                $scope.init();
            }
        }]);

