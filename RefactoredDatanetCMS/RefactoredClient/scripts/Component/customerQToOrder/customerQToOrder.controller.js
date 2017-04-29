angular.module("app").controller('customerQToOrderCtrl', [
    'customerQToOrderFactory', '$scope', '$http', 'growl', '$templateRequest', '$compile', 'session', 'saveLocalStorage', '$window',
    function (
        customerQToOrderFactory,
        $scope,
        $http,
        growl,
        $templateRequest,
        $compile,
        session,
        saveLocalStorage,
        $window) {
        $scope.init = function () {
            $scope.userInfo = session.getUser();
            var data = {
                params: {
                    page: -1,
                    pageSize: -1,
                    id: $scope.userInfo.Id,
                    code: ""
                }
            };
            //if not follow the procedure, then back to welcome page
            if (!$window.localStorage.getItem("state")) {
                $window.location.href = window.configuration.clientBaseUrl + "welcome";
            }
            customerQToOrderFactory.searchQuotesByCustomerId(data)
                //customerQToOrderFactory.getQuotesByCustomerId($scope.userInfo.Id)
                .then(function (result) {
                    $scope.quotes = result.data.Data.Items;
                    if ($scope.quotes.length < 1) {
                        growl.info("There is no quote");
                    }
                });

        }

        $scope.showDetail = function (quote, event) {
            var data = {
                params: {
                    page: -1,
                    pageSize: -1,
                    id: quote.Id
                }
            };
            customerQToOrderFactory.getOrderOrQuoteProduct(data).then(function (result) {
                $scope.quoteDetail = result.data.Data.Item;
                $templateRequest('/Scripts/Component/customerQToOrder/customerQuoteDetail.html')
                    .then(function (temp) {
                        temp = $compile(temp)($scope);
                        box = bootbox.dialog({
                            title: 'Product Detail',
                            onEscape: function () { },
                            backdrop: true,
                            message: temp
                        });

                    });
            });
        }

        $scope.quoteToOrder = function (info) {
            var data = {
                params: {
                    page: -1,
                    pageSize: -1,
                    id: info.Id
                }
            };
            customerQToOrderFactory.getOrderOrQuoteProductWithImage(data).then(function (result) {
                var productsModel = result.data.Data.Items;
                if (!productsModel || productsModel.length < 1) {
                    growl.info("There is no product");
                    return;
                }
                var products = productsModel.map(function (item) {
                    return {
                        Id: item.ProductModel.Id,
                        Code: item.ProductModel.Code,
                        ShortDesc: item.ProductModel.ShortDesc,
                        LongDesc: item.ProductModel.LongDesc,
                        Vendor: item.ProductModel.Vendor,
                        Image: item.ProductModel.Image,
                        Quantity: item.Quantity,
                        Price: item.Price
                    };
                });
                saveLocalStorage.save("cart",
                    {
                        "CustomerId": $scope.userInfo.Id,
                        "FromQuoteToOrder": true,
                        "QuoteId": info.Id
                    },
                    products);
                $window.location.href = window.configuration.clientBaseUrl + 'order';
            });
            //customerQToOrderFactory.getOrderOrQuoteProduct(data)
            //    .then(function (result) {
            //        var quote = result.data.Data.Item;
            //        if (!quote || !quote.OrderProductModels || quote.OrderProductModels.length < 1) {
            //            growl.info("There is no product");
            //            return;
            //        }
            //        var products = quote.OrderProductModels.map(function (item) {
            //            return {
            //                Id: item.ProductModel.Id,
            //                Code: item.ProductModel.Code,
            //                ShortDesc: item.ProductModel.ShortDesc,
            //                LongDesc: item.ProductModel.LongDesc,
            //                Vendor: item.ProductModel.Vendor,
            //                Image: item.ProductModel.Image,
            //                Quantity: item.Quantity,
            //                Price: item.Price
            //            };
            //        });
            //        saveLocalStorage.save("cart",
            //            {
            //                "CustomerId": $scope.userInfo.Id,
            //                "FromQuoteToOrder": true,
            //                "QuoteId": quote.Id
            //            },
            //            products);
            //        $window.location.href = window.configuration.clientBaseUrl + 'order';
            //    });
        }
    }]);

