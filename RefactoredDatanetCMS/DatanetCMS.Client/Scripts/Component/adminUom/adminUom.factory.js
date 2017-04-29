angular.module('app').factory('adminUomFactory', ['httpRequest', function (httpRequest) {
    var url = window.configuration.baseUrl + "uom/";
    var factory = {
        getAll: getAll,
        deleteOne: deleteOne,
        addOrUpdate: addOrUpdate,
        getById: getById,
        search:search
    }

    function search(data) {
        return httpRequest.get(url + "Search", data);
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

    function getById(id) {
        return httpRequest.get(url + "GetById?id=" + id);
    }

    return factory;

}])