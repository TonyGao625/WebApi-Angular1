angular.module('app').factory('adminEmailTemplateFactory',
    ['httpRequest', function (httpRequest) {
    var url = window.configuration.baseUrl + 'Setting';
    var factory = {
        getEmailTemplates: getEmailTemplates,
        saveEmailTemplate: saveEmailTemplate
    }
    function getEmailTemplates() {
        return httpRequest.get(url + "/GetEmailTemplates");
    };

    function saveEmailTemplate(data) {
        return httpRequest.post(url + "/SaveEmailTemplate", data);
    };
    return factory;
}])