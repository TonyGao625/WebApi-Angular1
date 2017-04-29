angular.module('app').factory('adminContractFactory', ['httpRequest', function (httpRequest) {
    var baseUrl = window.configuration.baseUrl + "contractnumber/";
    var factory = {
        getAll: getAll,
        getContractsByCustomerId: getContractsByCustomerId,
        addOrUpdate: addOrUpdate,
        deleteById: deleteById,
        search: search
    }
    function getAll() {
        return httpRequest.get(baseUrl + "GetAll");
    };
    function getContractsByCustomerId(id) {
        return httpRequest.get(baseUrl + "getContractsByCustomerId?id=" + id);
    };

    function deleteById(id) {
        return httpRequest.get(baseUrl + "DeleteById?id=" + id);
    }

    function addOrUpdate(data) {
        return httpRequest.post(baseUrl + "AddOrUpdate", data);
    };
    function search(data) {
        return httpRequest.get(baseUrl + "Search", data);
    };
    return factory;

}])