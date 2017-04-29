angular.module('app').factory('customerOrderFactory', ['httpRequest', 'Upload', '$rootScope', function (httpRequest, Upload,$rootScope) {
    var baseUrl = window.configuration.baseUrl + "order/";
    var factory = {
        getDeliveryAddrsByCustomerId: getDeliveryAddrsByCustomerId,
        uploadFile: uploadFile,
        saveOrder: saveOrder
    }
    function getDeliveryAddrsByCustomerId(id) {
        return httpRequest.get(baseUrl + "GetDeliveryAddrsByCustomerId?id=" + id);
    };
    function uploadFile(file) {
        $rootScope.loading = 30;
        return Upload.upload({
            url: baseUrl + 'UploadPoDoc',
            contentType: "multipart/form-data",
            processData: false,
            cache: false,
            data: { "file": file }
        });
    };

    function saveOrder(data) {
        return httpRequest.post(baseUrl + "AddOrder", data);
    }
    //function deleteById(id) {
    //    return httpRequest.get(baseUrl + "DeleteById?id=" + id);
    //};
    //function addOrUpdate(data) {
    //    return httpRequest.post(baseUrl + "AddOrUpdate", data);
    //};

    //function getById(id) {
    //    return httpRequest.get(baseUrl + "GetById?id=" + id);
    //}
    //function search(data) {
    //    return httpRequest.get(baseUrl + "Search", data);
    //};

    return factory;

}])