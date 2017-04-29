angular.module('app').factory('adminCustomerFactory', ['httpRequest', function (httpRequest) {
    var baseUrl = window.configuration.baseUrl + "customer/";
    var factory = {
        getAll: getAll,
        getAllName: getAllName,
        deleteById: deleteById,
        addOrUpdate: addOrUpdate,
        getById: getById,
        search: search,
        getByManagerId: getByManagerId
    }
    function getAll() {
        return httpRequest.get(baseUrl + "GetAll");
    };
    function getAllName() {
        return httpRequest.get(baseUrl + "GetAllName");
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
    function getByManagerId(id) {
        return httpRequest.get(baseUrl + "GetByManagerId?id=" + id);
    };

    return factory;

}])