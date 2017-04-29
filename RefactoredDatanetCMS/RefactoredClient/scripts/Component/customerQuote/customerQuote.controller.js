angular.module("app").controller('customerQuoteCtrl', [
    'session', '$window', '$timeout', 'adminDeliveryAddrFactory', 'customerQuoteFactory', 'adminCustomerFactory', '$scope', '$http', 'growl', '$templateRequest', '$compile', 'saveLocalStorage', '$rootScope','TIME',
    function (
        session,
        $window,
        $timeout,
        adminDeliveryAddrFactory,
        customerQuoteFactory,
        adminCustomerFactory,
        $scope,
        $http,
        growl,
        $templateRequest,
        $compile, saveLocalStorage, $rootScope, TIME) {

        $scope.products = [];
        $scope.deliveryAddrs = [];
        $scope.deliveryAddr = {};
        $scope.cartInfo = {};
        //$scope.customer = {};
        $scope.filePath = "";
        $scope.purchaseCode = "";

        $scope.saving = false;
        $scope.deliveryCharge = 0;

        $scope.init = function () {

            //get user information
            $scope.userInfo = session.getUser();
            if (!$scope.userInfo) {
                growl.error("Get user information failed");
                return;
            }
            $scope.contactName = $scope.userInfo.ContactName;
            $scope.contactEmail = $scope.userInfo.ContactEmail;
            $scope.contactPhone = $scope.userInfo.ContactPhone;

            //adminCustomerFactory.getById($scope.userInfo.Id).then(function (result) {
            //    $scope.customer = result.data.Data;

            //    $scope.contactName = $scope.customer.ContactName;
            //    $scope.contactEmail = $scope.customer.ContactEmail;
            //    $scope.contactPhone = $scope.customer.ContactPhone;
            //});

            $scope.getAddrs();
            $scope.getQuoteCart();
            //if not follow the procedure, then back to welcome page
            if (!$window.localStorage.getItem("state")) {
                $scope.backToWelcome();
            }
        }
        $scope.backToWelcome = function () {
            $rootScope.backToProduct = false;
            $window.location.href = window.configuration.clientBaseUrl + "welcome";
        }
        $scope.getQuoteCart = function () {
            if (!window.localStorage) {
                growl.error("The browser does not support localstorage, please contact administrator");
                return;
            }
            var cartInfo = localStorage['cart'];

            if (cartInfo === 'undefined' || cartInfo == null) {
                growl.error("There is no product in carts");
                $scope.backToWelcome();
                return;
            }

            $scope.cartInfo = JSON.parse(cartInfo);
            if (!$scope.cartInfo.hasOwnProperty("products")) {
                growl.error("There is no product in carts");
                $scope.backToWelcome();
                return;
            }
            $scope.products = $scope.cartInfo.products;
        }

        $scope.removeProduct = function (product) {
            bootbox.confirm("Are you sure you want to remove this product?", function (res) {
                if (!res) {
                    return;
                } else {
                    var index = $scope.products.indexOf(product);
                    if (index < 0 || index > $scope.products.length) {
                        return;
                    }
                    $scope.products.splice(index, 1);
                    $scope.$apply();
                    saveLocalStorage.save('cart', $scope.userInfo, $scope.products); //this is for go back product page to reselect
                    growl.success('Remove success');
                }
            });
        }

        $scope.getAddrs = function () {
            customerQuoteFactory.getDeliveryAddrsByCustomerId($scope.userInfo.Id).then(function (result) {
                var deliveryAddrs = result.data.Datas;
                deliveryAddrs.map(function (item) {
                    item.ticked = false;
                });
                $scope.deliveryAddrs = deliveryAddrs;
            });
        }

        $scope.saveQuote = function () {
            if ($scope.saving) {
                growl.info("Saving, please wait a second");
                return;
            }

            var quantityFlag = $scope.products.some(function (item) {
                return item.Quantity <= 0 || isNaN(item.Quantity);
            });

            if (quantityFlag) {
                growl.error("Quantity must be greater than 0");
                return;
            }

            $scope.saving = true;

            var orderProudcts = [];

            $scope.products.map(function (item) {
                var orderProduct = {};
                orderProduct.Id = 0;
                orderProduct.OrderId = 0;
                orderProduct.ProductId = item.Id;
                orderProduct.Quantity = item.Quantity;
                orderProduct.Price = item.Price;
                orderProudcts.push(orderProduct);
            });
            var orderAddrModel = angular.copy($scope.deliveryAddr);

            var quote = {};
            quote.CustomerId = $scope.userInfo.Id;

            quote.ContactName = $scope.contactName;
            quote.ContactEmail = $scope.contactEmail;
            quote.ContactPhone = $scope.contactPhone;
            quote.GSTRate = 0.1;
            quote.GST = $scope.getGST();
            //quote.DeliveryCharge = $scope.deliveryCharge;
            quote.DeliveryCharge = $scope.userInfo.DeliveryCharge;
            quote.Amount = $scope.getTotal();

            quote.OrderAddres = orderAddrModel;
            quote.OrderProducts = orderProudcts;
            quote.TimeZone = TIME;
            customerQuoteFactory.saveQuote(JSON.stringify(quote)).then(function (result) {
                //set quote cart to null
                localStorage['quoteCart'] = "";

                bootbox.dialog({
                    onEscape: false,
                    backdrop: true,
                    closeButton: false,
                    message: "The quote has been built successfully, Thank you!"
                });

                $timeout(function () {
                    $window.location.href = window.configuration.clientBaseUrl + "welcome";
                }, 3000);
            }, function () {
                $scope.saving = false;
            });
        }

        $scope.addAddress = function () {
            $scope.address = {
                Addr1: "",
                Addr2: "",
                Addr3: "",
                State: "",
                PostCode: "",
                Phone: "",
                CustomerId: $scope.userInfo.Id
            }
            $templateRequest('/Scripts/Component/customerQuote/addressForm.html')
                .then(function (temp) {
                    temp = $compile(temp)($scope);
                    box = bootbox.dialog({
                        title: 'Add new address',
                        onEscape: function () { },
                        backdrop: true,
                        message: temp
                    });
                });
        }

        $scope.createAddress = function () {
            adminDeliveryAddrFactory.addOrUpdate($scope.address).then(function (result) {

                box.modal('hide');
                growl.success('Created success');

                customerQuoteFactory.getDeliveryAddrsByCustomerId($scope.userInfo.Id).then(function (results) {
                    var deliveryAddrs = results.data.Datas;
                    deliveryAddrs.map(function (item) {
                        if (result.data.Data.Id === item.Id) {
                            item.ticked = true;
                        }
                        else {
                            item.ticked = false;
                        }
                    });
                    $scope.deliveryAddrs = deliveryAddrs;
                });
            });
        }

        $scope.getSubtotal = function () {
            if (!$scope.userInfo) {
                growl.error("Get user information failed");
                return;
            }
            var total = 0;
            angular.forEach($scope.products, function (product, key) {
                //total += product.Price * product.Quantity + $scope.deliveryCharge;
                total += product.Price * product.Quantity;
            });
            total += $scope.userInfo.DeliveryCharge;
            return total;
        }

        $scope.decrease = function (product) {
            product.Quantity = parseFloat(product.Quantity);
            if (isNaN(product.Quantity)) {
                product.Quantity = 1;
                return;
            }
            if (product.Quantity <= 1) {
                product.Quantity = 1;
            }
            else {
                product.Quantity -= 1;
            }
            $scope.changeQuantity(product);
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
            }
            $scope.changeQuantity(product);
        }

        $scope.changeQuantity = function (product) {
            var result = /^[0-9]*$/.test(product.Quantity);
            if (!result) {
                product.Quantity = 1;
            }
            if (product.Quantity <= 1) {
                product.Quantity = 1;
            }
            if (product.Quantity > 9999) {
                product.Quantity = 9999;
            }
            $.each($scope.products, function (index, item) {
                if (product.Id === item.Id) {
                    item.Quantity = product.Quantity;
                }
            })
            saveLocalStorage.save('cart', $scope.userInfo, $scope.products); //this is for go back product page to reselect
        }

        $scope.calcDeliveryCharge = function (addr) {
            if (!$scope.userInfo) {
                growl.error("Get user information failed");
                return;
            }
            $scope.deliveryCharge = $scope.userInfo.DeliveryCharge;
        }

        $scope.selectReset = function () {
            $scope.deliveryAddrs.map(function (item) {
                if (item.ticked === true) {
                    item.ticked = false;
                }
            });
            $scope.deliveryCharge = 0;
        }

        $scope.getGST = function () {
            var subTotal = $scope.getSubtotal();
            return subTotal * 0.1;
        }

        $scope.getTotal = function () {
            return $scope.getSubtotal() + $scope.getGST();
        }
    }
]);