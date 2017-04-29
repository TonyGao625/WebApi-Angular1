angular.module('app').factory('customerProductFactory', ['httpRequest', function (httpRequest) {
    var url = window.configuration.baseUrl + 'product';
    var factory = {
        getByProductId: getByProductId,
        getAllCategoryByCustomerId: getAllCategoryByCustomerId,
        getAllByCustomerId: getAllByCustomerId,
        search: search
    }

    function getAllByCustomerId(id) {
        return httpRequest.get(url + "/GetAllByCustomerId?customerId=" + id);
    }
    function getByProductId(id) {
        return httpRequest.get(url + "/GetById?id=" + id);
    }
    function getAllCategoryByCustomerId(customerId) {
        return httpRequest.get(url + "/getAllCategoryByCustomerId?customerId="+customerId);
    };    
    function search(data) {
        return httpRequest.get(url + "/SearchByCustomerId", data);
    };
    return factory;
}])