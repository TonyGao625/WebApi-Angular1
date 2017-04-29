angular.module("app").controller('customerOrderCtrl', ['session', '$window', '$timeout', 'adminDeliveryAddrFactory',
    '$rootScope', 'adminContractFactory', 'customerOrderFactory', 'adminCustomerFactory', 'Upload', '$scope', '$http',
    'growl', '$templateRequest', '$compile', 'saveLocalStorage','customerQToOrderFactory','TIME',
    function (
        session,
        $window,
        $timeout,
        adminDeliveryAddrFactory,
        $rootScope,
        adminContractFactory,
        customerOrderFactory,
        adminCustomerFactory,
        Upload,
        $scope,
        $http,
        growl,
        $templateRequest,
        $compile, saveLocalStorage,
        customerQToOrderFactory, TIME) {

        var vm = $scope.vm = {};
        vm.purchaseCode = "";
        vm.contractNumber = {};

        $scope.saving = false;
        $scope.deliveryCharge = 0;
        $scope.isUploading = false;

        $scope.init = function () {

            $scope.userInfo = session.getUser();
            if (!$scope.userInfo) {
                growl.error("Get user information failed");
                return;
            }
            $scope.contactName = $scope.userInfo.ContactName;
            $scope.contactEmail = $scope.userInfo.ContactEmail;
            $scope.contactPhone = $scope.userInfo.ContactPhone;

            $scope.products = [];
            $scope.deliveryAddrs = [];
            $scope.deliveryAddr = {};
            $scope.cartInfo = {};
            //$scope.customer = {};
            $scope.filePath = "";
            $scope.fileName = "";

            var selectedContractNumber = $window.localStorage["selectedContractNumber"];
            $scope.selectedContractNumber = selectedContractNumber;

            $scope.getAddrs();

            //get customer information
            //adminCustomerFactory.getById($scope.userInfo.Id).then(function (result) {
            //    $scope.customer = result.data.Data;
            //    $scope.contactName = $scope.customer.ContactName;
            //    $scope.contactEmail = $scope.customer.ContactEmail;
            //    $scope.contactPhone = $scope.customer.ContactPhone;
            //});
            if ($scope.userInfo.PurchaseType === "1") {
                $scope.getContractNos();
            }
            
            $scope.getOrderCart();
            //if not follow the procedure, then back to welcome page
            if (!$window.localStorage.getItem("state")) {
                $scope.backToWelcome();
            }
        }
        $scope.backToWelcome = function () {
            $rootScope.backToProduct = false;
            $window.location.href = window.configuration.clientBaseUrl + "welcome";
        }
        //get contract number by customer id
        $scope.getContractNos = function () {
            adminContractFactory.getContractsByCustomerId($scope.userInfo.Id).then(function (result) {
                var contractNos = result.data.Datas;
                if (Array.isArray(contractNos)) {
                    if (contractNos.length <= 0) {
                        growl.error("There is no contract number, please contact your manager!");
                        return;
                    }
                    contractNos.map(function (item) {
                        //update for select contract number 11/4/2017 sh
                        if (item.Name === $scope.selectedContractNumber) {
                            item.ticked = true;
                        }
                        else {
                            item.ticked = false;
                        }
                    });
                }
                $scope.contractNumbers = contractNos;
            });
        }

        $scope.getOrderCart = function () {
            if (!window.localStorage) {
                growl.error("The browser is not support local storage, please contact administrator");
                return;
            }
            var cartInfo = localStorage['cart'];

            if ((cartInfo === undefined || cartInfo === null) && $window.localStorage.getItem("state") !== "qandorder") {
                growl.error("There is no product in carts");
                $scope.backToWelcome();
                return;
            }

            $scope.cartInfo = JSON.parse(cartInfo);
            if ((!$scope.cartInfo.hasOwnProperty("products")) && $window.localStorage.getItem("state") !== "qandorder") {
                growl.error("There is no product in carts");
                $scope.backToWelcome();
                return;
            }
            $scope.fromQuoteToOrder = false;
            if ($scope.cartInfo.customer && $scope.cartInfo.customer.hasOwnProperty("FromQuoteToOrder")) {
                $scope.fromQuoteToOrder = true;
                $scope.quoteId = $scope.cartInfo.customer.QuoteId;
            }
            $scope.customerInfo = $scope.cartInfo.customer;
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
                   // saveLocalStorage.save('orderCart', $scope.customer, $scope.products);
                    //saveLocalStorage.save('cart', $scope.customer, $scope.products); //this is for go back product page to reselect
                    saveLocalStorage.save('cart', $scope.userInfo, $scope.products);
                    growl.success('Remove success');
                }
            });
        }

        $scope.getAddrs = function () {
            customerOrderFactory.getDeliveryAddrsByCustomerId($scope.userInfo.Id).then(function (result) {
                var deliveryAddrs = result.data.Datas;
                if (deliveryAddrs.length <= 0) {
                    growl.error("No delivery address");
                    return;
                }
                deliveryAddrs.map(function (item) {
                    item.ticked = false;
                });
                $scope.deliveryAddrs = deliveryAddrs;
            });
        }

        $scope.saveOrder = function () {
            if ($scope.saving) {
                growl.info("Saving, please wait a second");
                return;
            }

            if ($scope.products.length <= 0) {
                growl.error("There is no product");
                return;
            }
            //change for customer info saved in localstorage
            //if ($scope.userInfo.PurchaseType === "1" && !$scope.userInfo.DisplayContractNo) {
            if ($scope.userInfo.PurchaseType === "1") {
                if (!vm.contractNumber[0]) {
                    growl.error("Contract number is required");
                    return;
                }
            }
            //change for customer info saved in localstorage
            if ($scope.userInfo.PoDocMandatory && $scope.fileName === "") {
                growl.error("Purchase Document is required");
                return;
            }

            if (!$scope.deliveryAddr[0]) {
                growl.error("Delivery address is required");
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

            //purchase code setting
            //purchase by contract number
            var purchaseCode;
            //change for customer info saved in localstorage
            if ($scope.userInfo.PurchaseType === "1") {
                //if (!$scope.userInfo.DisplayContractNo) { //is selected in welcome page
                //    purchaseCode = vm.contractNumber[0].Name;
                //} else {
                //    if (!$scope.selectedContractNumber) {
                //        growl.error("Please go to welcome page to select a contract number");
                //        return;
                //    }
                //    purchaseCode = $scope.selectedContractNumber;
                //}
                purchaseCode = vm.contractNumber[0].Name;
            } else {
                purchaseCode = vm.purchaseCode;
            }

            //products configuration
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

            var order = {};
            order.CustomerId = $scope.userInfo.Id;

            order.PurchaseCode = purchaseCode;
            order.OrderDocs = [{ "Id": 0, "OrderId": 0, "PoDocPath": $scope.fileName }];
            order.OrderAddres = orderAddrModel;
            order.OrderProducts = orderProudcts;

            order.ContactName = $scope.contactName;
            order.ContactPhone = $scope.contactPhone;
            order.ContactEmail = $scope.contactEmail;
            order.GSTRate = 0.1;
            order.GST = $scope.getGST();
            //order.DeliveryCharge = $scope.deliveryCharge;
            //change for customer info saved in localstorage
            order.DeliveryCharge = $scope.userInfo.DeliveryCharge;
            order.Amount = $scope.getTotal();
            order.TimeZone = TIME;
            if ($scope.fromQuoteToOrder) {//add quote to order
                customerQToOrderFactory.addQuoteToOrder($scope.quoteId, JSON.stringify(order)).then(function (result) {
                    //set order cart to null
                    localStorage['cart'] = "";

                    bootbox.dialog({
                        onEscape: false,
                        backdrop: true,
                        closeButton:false,
                        message: "The quote has been converted to order successfully. Thank you!"
                    });

                    $timeout(function () {
                        $window.location.href = window.configuration.clientBaseUrl + "welcome";
                    }, 3000);

                }, function () {
                    $scope.saving = true;
                });
            } else {
                customerOrderFactory.saveOrder(JSON.stringify(order)).then(function (result) {
                    //set order cart to null
                    localStorage['cart'] = "";

                    bootbox.dialog({
                        onEscape: false,
                        backdrop: true,
                        closeButton: false,
                        message: "The order has been placed successfully. Thank you!"
                    });

                    $timeout(function () {
                        $window.location.href = window.configuration.clientBaseUrl + "welcome";
                    }, 3000);

                }, function () {
                    $scope.saving = true;
                });
            }
        }

        $scope.uploadFiles = function (file, errFiles) {
            $scope.isUploading = true;
            $scope.f = file;
            $scope.errFile = errFiles && errFiles[0];
            if (file) {
                customerOrderFactory.uploadFile(file).then(function(result) {
                    if (result.data.Status != 0) {
                        $scope.isUploading = false;
                        $rootScope.loading = 100;
                        growl.error('Upload failed');
                        return;
                    } else {
                        $scope.isUploading = false;
                        $rootScope.loading = 100;
                        $scope.filePath = result.data.Data.Path;
                        $scope.fileName = result.data.Data.FileName;
                        growl.success('Upload success');
                    }
                });
            } else {
                $scope.isUploading = false;
            }
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

                customerOrderFactory.getDeliveryAddrsByCustomerId($scope.userInfo.Id).then(function (results) {
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
                    $scope.calcDeliveryCharge();
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
            //change for customer info saved in localstorage
            total += $scope.userInfo.DeliveryCharge;
            return total;
        }

        $scope.decrease = function (product) {
            if ($scope.fromQuoteToOrder) {
                growl.info("Quote to order can't modify quantity");
                return;
            }
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
            if ($scope.fromQuoteToOrder) {
                growl.info("Quote to order can't modify quantity");
                return;
            }
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
            $.each($scope.products,
                function(index, item) {
                    if (product.Id === item.Id) {
                        item.Quantity = product.Quantity;
                    }
                });
            //change for customer info saved in localstorage
            saveLocalStorage.save('cart', $scope.userInfo, $scope.products); //this is for go back product page to reselect
        }

        $scope.calcDeliveryCharge = function (addr) {
            if (!$scope.userInfo) {
                growl.error("Get user information failed");
                return;
            }
            //change for customer info saved in localstorage
            $scope.deliveryCharge = $scope.userInfo.DeliveryCharge;
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