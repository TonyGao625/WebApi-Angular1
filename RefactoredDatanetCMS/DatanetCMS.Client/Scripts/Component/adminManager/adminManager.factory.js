angular.module('app').factory('adminManagerFactory', ['httpRequest', function (httpRequest) {
    var baseUrl = window.configuration.baseUrl + "Manager/";
    var factory = {
        getAll: getAll,
        deleteById: deleteById,
        addOrUpdate: addOrUpdate,
        getById: getById,
        search: search
    }
    function getAll() {
        return httpRequest.get(baseUrl + "GetAll");
    };
    function deleteById(id) {
        return httpRequest.get(baseUrl + "DeleteById?id=" + id);
    };
    function addOrUpdate(data) {
        return httpRequest.post(baseUrl + "AddOrUpdate", data);
    };

    function getById(id) {
        return httpRequest.get(baseUrl + "GetById?id=" + id);
    }
    function search(data) {
        return httpRequest.get(baseUrl + "Search", data);
    };
    return factory;

}])