angular.module("app").controller('customerProductCtrl', [
    'session', 'customerProductFactory', '$scope', '$http', 'growl', "$window", "adminCategoryFactory", "saveLocalStorage", "$rootScope",
    function (session, customerProductFactory, $scope, $http, growl, $window, adminCategoryFactory, saveLocalStorage, $rootScope) {
        var products = [];
        var CategoryId = 0;
        $scope.init = function () {
            $scope.userInfo = session.getUser();
            if (!$scope.userInfo) {
                growl.error("Get user information failed");
                return;
            }
            $scope.search();
            $scope.getCart();
            adminCategoryFactory.getAll().then(function (res) {
                $scope.categories = res.data.Datas;
            });
            //if not follow the procedure, then back to welcome page
            if (!$window.localStorage.getItem("state")) {
                $window.location.href = window.configuration.clientBaseUrl + "welcome";
            }

        }
        $scope.getCart = function () {
            if (!window.localStorage) {
                growl.error("The browser does not support local storage, please contact administrator");
                return;
            }
            if (localStorage['cart']) {
                var cartInfo = localStorage['cart'];
                $scope.cartInfo = JSON.parse(cartInfo);
                if ($scope.cartInfo) {
                    products = $scope.cartInfo.products;
                }
            }
        }
        $scope.search = function (page, categoryId) {
            //if (page === 0 || page) {
            //    $rootScope.filterProductCode = "";
            //}
            page = page || 0;
            if (CategoryId) {
                categoryId = CategoryId;
            } else {
                categoryId = categoryId || 0;
            }
            var data = {
                params: {
                    page: page,
                    pageSize: 6,
                    filter: $rootScope.filterProductCode,
                    customerId: $scope.userInfo.Id,
                    categoryId: categoryId
                }
            };
            customerProductFactory.search(data).then(function (res) {
                $scope.products = res.data.Data.Items;
                $scope.page = res.data.Data.Page;
                $scope.pagesCount = res.data.Data.TotalPages;
                $scope.totalCount = res.data.Data.TotalCount;
                if ($rootScope.filterProductCode && $rootScope.filterProductCode.length) {
                    growl.success(res.data.Data.Items.length + ' products found');
                }
                if (!$scope.products.length) {
                    growl.info('No products have been found');
                }
                $.each($scope.products, function (index, item) {
                    $http.get(window.configuration.baseUrl + 'product' + "/GetById?id=" + item.Id)
                        .then(function (res) {
                            if (res.data.Status === 0) {
                                item.Image = res.data.Data.Image;
                                item.Quantity = 0;
                                if (localStorage['cart']) {
                                    var cartInfo = localStorage['cart'];
                                    $scope.cartInfo = JSON.parse(cartInfo);
                                    if ($scope.cartInfo) {
                                        if (!products) {
                                            products = $scope.cartInfo.products;
                                        }
                                        $.each(products, function (selectedProductIndex, selectedProductItem) {
                                            if (item.Id === selectedProductItem.Id) {
                                                item.Quantity = selectedProductItem.Quantity;
                                            }
                                        })
                                    }
                                }
                            }
                        });
                });
            });
        };
        $scope.continue = function () {
            $.each(products,
                function (index, item) {
                    if (!item.Quantity || item.Quantity === 0) {
                        products.splice(index, 1);
                    };
                });
            if (products.length === 0) {
                growl.error("Please specify a product at least!");
                return;
            }
            if ($window.localStorage.getItem("state")) {
                saveLocalStorage.save("cart",
               {
                   "CustomerId": $scope.userInfo.Id
               },
               products);
                $window.location.href = window.configuration.clientBaseUrl + $window.localStorage.getItem("state");
            }

        }
        $scope.selectProduct = function (product, quantity) {
            var productIdList = [];
            $.each(products, function (index, item) {
                productIdList.push(item.Id);
            });
            if (productIdList.indexOf(product.Id) >= 0) {
                if (!quantity) {
                    products.splice(productIdList.indexOf(product.Id), 1);
                } else {
                    products[productIdList.indexOf(product.Id)].Quantity = quantity;
                }
            } else {
                products.push({
                    Id: product.Id,
                    Code: product.Code,
                    ShortDesc: product.ShortDesc,
                    LongDesc: product.LongDesc,
                    Vendor: product.Vendor,
                    Uom: product.Uom,
                    Image: product.Image,
                    Quantity: quantity,
                    Price: product.Price
                });
                saveLocalStorage.save("cart",
              {
                  "CustomerId": $scope.userInfo.Id
              },
              products);
            }
        };
        $scope.decrease = function (product) {
            product.Quantity = parseFloat(product.Quantity);
            if (isNaN(product.Quantity)) {
                product.Quantity = 1;
                return;
            }
            if (product.Quantity <= 0) {
                product.Quantity = 0;
            }
            else {
                product.Quantity -= 1;
            }
            $scope.selectProduct(product, product.Quantity);

        }
        $scope.increase = function (product) {
            product.Quantity = parseFloat(product.Quantity);
            if (isNaN(product.Quantity)) {
                product.Quantity = 1;
                return;
            }
            product.Quantity += 1;
            if (product.Quantity > 9999) {
                product.Quantity = 9999;
                return;
            }
            $scope.selectProduct(product, product.Quantity);
        }
        $scope.changeQuantity = function (product) {
            var result = /^[0-9]*$/.test(product.Quantity);
            if (!result) {
                product.Quantity = 1;
            }
            if (product.Quantity <= 0) {
                product.Quantity = 0;
            }
            if (product.Quantity > 9999) {
                product.Quantity = 9999;
                return;
            }
        }
        $scope.showPopup = function ($index) {
            if ($(window).width() <= 1023) {
                $("#myModal" + $index).css('top', '20%');
                $("#myModal" + $index).modal({
                    keyboard: true
                })
            }
        }
        $scope.showtimes = false;
        $('.product-categopy .list-group').on('click', '.list-group-item', function () {
            $(this).css({ "background": "#4688be" }).siblings().css({
                "background": "rgb(238,238,238)"
            });
        });
        $scope.setCategory = function (categoryId) {
            CategoryId = categoryId
        }
    }]);
