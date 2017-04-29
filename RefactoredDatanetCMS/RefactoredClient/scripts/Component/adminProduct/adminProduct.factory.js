angular.module('app').factory('adminProductFactory', ['httpRequest', function (httpRequest) {
    var url = window.configuration.baseUrl + 'product/';
    var factory = {
        getAll: getAll,
        deleteOne: deleteOne,
        addOrUpdate: addOrUpdate,
        getById: getById,
        search: search
    }
    function getAll() {
        return httpRequest.get(url + "GetAll");
    };
    function deleteOne(id) {
        return httpRequest.get(url + "DeleteById?id=" + id);
    };
    function addOrUpdate(data) {
        return httpRequest.post(url + "AddOrUpdate", data);
    };

    function getById(id) {
        return httpRequest.get(url + "GetById?id=" + id);
    }
    function search(data) {
        return httpRequest.get(url + "Search", data);
    };

    return factory;

}])