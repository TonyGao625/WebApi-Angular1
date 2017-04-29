angular.module("app").controller('customerQAndOCtrl', [
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
            $scope.searchQuoteCode = "";
            $scope.searchOrderCode = "";
            $scope.dateFilter = "1";
            $scope.dateFilterForOrder = "1";

            $scope.isQuoteTab = true;
            $scope.userInfo = session.getUser();
            $scope.getQuotes();
        }

        $scope.getQuotes = function (page) {

            page = page || 0;
            var data = {
                params: {
                    page: page,
                    pageSize: 10,
                    id: $scope.userInfo.Id,
                    code: $scope.searchQuoteCode,
                    dateFilter: $scope.searchQuoteCode===""?$scope.dateFilter:"0"
                }
            };

            customerQToOrderFactory.searchQuotesByCustomerId(data)
                .then(function (result) {
                    $scope.quotes = result.data.Data.Items;
                    $scope.page = result.data.Data.Page;
                    $scope.pagesCount = result.data.Data.TotalPages;
                    $scope.totalCount = result.data.Data.TotalCount;
                    if ($scope.quotes.length < 1) {
                        growl.info("There is no quote");
                    }
                });
        }

        $scope.getOrders = function (page) {

            page = page || 0;
            var data = {
                params: {
                    page: page,
                    pageSize: 10,
                    id: $scope.userInfo.Id,
                    code: $scope.searchOrderCode,
                    dateFilter: $scope.searchOrderCode === "" ? $scope.dateFilterForOrder : "0"
                }
            };

            customerQToOrderFactory.searchOrdersByCustomerId(data)
                .then(function (result) {
                    $scope.orders = result.data.Data.Items;
                    $scope.pageOrder = result.data.Data.Page;
                    $scope.pagesCountOrder = result.data.Data.TotalPages;
                    $scope.totalCountOrder = result.data.Data.TotalCount;
                    if ($scope.orders.length < 1) {
                        growl.info("There is no order");
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
                $templateRequest('/Scripts/Component/customerQuoteAndOrder/customerOrderDetail.html')
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
        }

        $scope.toQuotes = function () {
            $scope.isQuoteTab = true;
            if (!$scope.quotes) {
                $scope.getQuotes();
            }
        }

        $scope.toOrders = function () {
            $scope.isQuoteTab = false;
            if (!$scope.orders) {
                $scope.getOrders();
            }
        }
    }]);

