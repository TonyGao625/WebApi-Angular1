angular.module('app').factory('adminCategoryFactory', ['httpRequest', function (httpRequest) {
    var url = window.configuration.baseUrl + 'category';
    var factory = {
        getAll: getAll,
        search: search,
        deleteById: deleteById,
        addOrUpdate: addOrUpdate,
    }
    function search(data) {
        return httpRequest.get(url + "/Search", data);
    };
    function getAll() {
        return httpRequest.get(url + "/GetAll");
    };
    function deleteById(id) {
        return httpRequest.get(url + "/DeleteById?id=" + id);
    };
    function addOrUpdate(data) {
        return httpRequest.post(url + "/AddOrUpdate", data);
    };
    return factory;
}])