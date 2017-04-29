angular.module('app').factory('adminDeliveryAddrFactory', ['httpRequest', function (httpRequest) {
    var baseUrl = window.configuration.baseUrl + "DeliveryAddr/";
    var factory = {
        getAll: getAll,
        getAddrsByCustomerId: getAddrsByCustomerId,
        addOrUpdate: addOrUpdate,
        deleteById: deleteById,
        search: search,
        addBulkAddress: addBulkAddress
    }
    function getAll() {
        return httpRequest.get(baseUrl + "GetAll");
    };
    function getAddrsByCustomerId(id) {
        return httpRequest.get(baseUrl + "GetAddrsByCustomerId?id=" + id);
    };

    function deleteById(id) {
        return httpRequest.get(baseUrl + "DeleteById?id=" + id);
    }

    function addOrUpdate(data) {
        return httpRequest.post(baseUrl + "AddOrUpdate", data);
    };
    function addBulkAddress(data){
        return httpRequest.post(baseUrl + "addBulkAddress", data);
    }
    function search(data) {
        return httpRequest.get(baseUrl + "Search", data);
    };
    return factory;

}])