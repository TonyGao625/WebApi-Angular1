angular.module('app').factory('adminBuyerGroupFactory', ['httpRequest', function (httpRequest) {
    var baseUrl = window.configuration.baseUrl + "buyergroup/";
    var factory = {
        getAll: getAll,
        deleteOne: deleteOne,
        addOrUpdate: addOrUpdate,
        getById: getById,
        search: search
    }
    function search(data) {
        return httpRequest.get(baseUrl + "Search", data);
    };
    function getAll() {
        return httpRequest.get(baseUrl+"GetAll");
    };
    function deleteOne(id) {
        return httpRequest.get(baseUrl+"DeleteById?id=" + id);
    };
    function addOrUpdate(data) {
        return httpRequest.post(baseUrl+"AddOrUpdate", data);
    };

    function getById(id) {
        return httpRequest.get(baseUrl+"GetById?id=" + id);
    }

    return factory;

}])