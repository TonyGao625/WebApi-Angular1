angular.module('app').factory('customerQToOrderFactory', ['httpRequest', function (httpRequest) {
    var baseUrl = window.configuration.baseUrl + "order/";
    var factory = {
        getQuotesByCustomerId: getQuotesByCustomerId,
        getOrdersByCustomerId: getOrdersByCustomerId,
        addQuoteToOrder: addQuoteToOrder,
        searchQuotesByCustomerId: searchQuotesByCustomerId,
        searchOrdersByCustomerId: searchOrdersByCustomerId,
        getOrderOrQuoteProduct: getOrderOrQuoteProduct,
        getOrderOrQuoteProductWithImage: getOrderOrQuoteProductWithImage
    }
    function getQuotesByCustomerId(id) {
        return httpRequest.get(baseUrl + "GetQuotesByCustomerId?id=" + id);
    };

    function addQuoteToOrder(id, data) {
        return httpRequest.post(baseUrl + "AddQuoteToOrder?id=" + id, data);
    }
    function getOrdersByCustomerId(id) {
        return httpRequest.get(baseUrl + "GetOrdersByCustomerId?id=" + id);
    };

    function searchQuotesByCustomerId(data) {
        return httpRequest.get(baseUrl + "SearchQuotesByCustomerId", data);
    }

    function searchOrdersByCustomerId(data) {
        return httpRequest.get(baseUrl + "SearchOrdersByCustomerId", data);
    }

    function getOrderOrQuoteProduct(data) {
        return httpRequest.get(baseUrl + "GetOrderOrQuoteProduct", data);
    }

    function getOrderOrQuoteProductWithImage(data) {
        return httpRequest.get(baseUrl + "GetOrderOrQuoteProductWithImage", data);
    }

    return factory;

}])

