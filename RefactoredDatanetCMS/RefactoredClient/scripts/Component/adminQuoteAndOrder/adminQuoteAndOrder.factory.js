angular.module('app').factory('adminQuoteAndOrderFactory', ['httpRequest', function (httpRequest) {
    var url = window.configuration.baseUrl + "Order/";
    var factory = {
        getAll: getAll,
        deleteOne: deleteOne,
        addOrUpdate: addOrUpdate,
        sendEmailById: sendEmailById,
        search:search
    }

    function search(data) {
        return httpRequest.post(url + "Search", data);
    };

    function getAll() {
        return httpRequest.get(url + "GetAll");
    };
    function deleteOne(id) {
        return httpRequest.get(url + "DeleteById?id=" + id);
    };
    function addOrUpdate(data) {
        return httpRequest.post(url + "AddOrUpdate", data);
    };

    function sendEmailById(id) {
        return httpRequest.get(url + "SendEmailById?id=" + id);
    }

    return factory;

}])