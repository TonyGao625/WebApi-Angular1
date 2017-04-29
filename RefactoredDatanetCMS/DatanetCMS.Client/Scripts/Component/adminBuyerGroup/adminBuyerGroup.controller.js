angular.module("app").controller('adminBuyerGroupCtrl', ['adminBuyerGroupFactory', '$scope', '$http', 'growl', '$templateRequest', '$compile', "adminProductFactory", "adminCustomerFactory",
    function (adminBuyerGroupFactory, $scope, $http, growl, $templateRequest, $compile, adminProductFactory, adminCustomerFactory) {
        var vm = this;
        $scope.search = function (page) {
            //if (page === 0 || page) {
            //    $scope.filterBuyerGroup = "";
            //}
            page = page || 0;
            var data = {
                params: {
                    page: page,
                    pageSize: 10,
                    filter: $scope.filterBuyerGroup
                }
            };
            adminBuyerGroupFactory.search(data).then(function (res) {
                vm.buyerGroups = res.data.Data.Items;
                $scope.page = res.data.Data.Page;
                $scope.pagesCount = res.data.Data.TotalPages;
                $scope.totalCount = res.data.Data.TotalCount;
                if ($scope.filterBuyerGroup && $scope.filterBuyerGroup.length) {
                    growl.success(res.data.Data.Items.length + (res.data.Data.Items.length > 1 ? ' Buyer Groups found' : ' Buyer Group found'));
                }
            });
        };
        $scope.search();
        $scope.selectedCustomers = [];
        $scope.selectedProducts = [];
        adminProductFactory.getAll().then(function (res) {
            vm.productList = res.data.Datas;
        });
        adminCustomerFactory.getAll().then(function (res) {
            vm.customerList = res.data.Datas;
        });
        $scope.init = function () {
            $scope.price = "";
            $.each(vm.productList, function (index, item) {
                item.ticked = false;
            });
            $.each(vm.customerList, function (index, item) {
                item.ticked = false;
            });
        }
        vm.add = function ($event) {
            $scope.init();
            vm.addDisable = true;
            $scope.selectedCustomers = [];
            $scope.selectedProducts = [];
            if ($scope.groupProduct) {
                $scope.groupProduct.Code = "";
            }
            $scope.groupProduct = {
                Code:""
            };
            $templateRequest('/Scripts/Component/adminBuyerGroup/adminBuyerGroupDetail.html').then(function (template) {
                $(".child-tr").remove();
                var tmp = $compile(template)($scope);
                $($event.currentTarget).parent().parent('tr').after(tmp);
            });
        };
        vm.showItem = function (id, $event) {
            adminBuyerGroupFactory.getById(id).then(function(res) {
                $scope.groupProduct = res.data.Data;
                $scope.selectedCustomers = res.data.Data.CustomerTempModelList;
                $scope.selectedProducts = res.data.Data.ProductTempModelList;
                $.each(vm.productList, function (index, item) {
                        item.ticked = false;
                });
                $.each(vm.customerList, function (index, item) {
                    item.ticked = false;
                });
                $scope.price = "";
            });
            $templateRequest('/Scripts/Component/adminBuyerGroup/adminBuyerGroupDetail.html').then(function (template) {
                $(".child-tr").remove();
                var tmp = $compile(template)($scope);
                $($event.currentTarget).parent().parent('tr').after(tmp);
            });
        }
        vm.addCustomer = function () {
            if (!$scope.selectedCustomer || !$scope.selectedCustomer.length) {
                growl.error("Please select one customer");
                return;
            }
            var selectedCustomersId = [];
            $.each($scope.selectedCustomers, function (index, item) {
                selectedCustomersId.push(item.Id);
            });
            if (selectedCustomersId.indexOf($scope.selectedCustomer[0].Id) >= 0) {
                growl.error("This customer is in the list");
                return;
            }
            $scope.selectedCustomers.unshift({
                Id: $scope.selectedCustomer[0].Id,
                Name: $scope.selectedCustomer[0].CompanyName
                }
            );
            $.each(vm.customerList, function (index, item) {
                item.ticked = false;
            })
        };
        vm.deleteCustomer = function (customer) {
            $scope.selectedCustomers.splice($scope.selectedCustomers.indexOf(customer), 1);
        };
        vm.deleteBuyerGroup = function (buyerGroup) {
            if (!buyerGroup.Id) {
                vm.buyerGroups.shift();
                vm.addDisable = false;
                return;
            }
            bootbox.confirm("Are you sure you want to delete this ?", function (res) {
                if (!res) {
                    return;
                }
                adminBuyerGroupFactory.deleteOne(buyerGroup.Id).then(function () {
                    vm.buyerGroups.splice(vm.buyerGroups.indexOf(buyerGroup), 1);
                    growl.success('Delete success');
                });
            });
        }
        vm.addProduct = function (priceValid) {
            if (!$scope.selectedProduct || !$scope.selectedProduct.length) {
                growl.error("Product is required");
                return;
            }
            if (!priceValid) {
                growl.error("Price should be between 1 to 999,999");
                return;
            }
            if (!$scope.price) {
                growl.error("Price is required");
                return;
            }
            var selectedProductsId = [];
            $.each($scope.selectedProducts, function (index, item) {
                selectedProductsId.push(item.ProductId);
            });
            if (selectedProductsId.indexOf($scope.selectedProduct[0].Id) >= 0) {
                growl.error("This product is in the list ");
                return;
            }
            $scope.selectedProducts.unshift({
                ProductId: $scope.selectedProduct[0].Id,
                Code: $scope.selectedProduct[0].Code,
                Price: $scope.price
            });
            $scope.price = undefined;
            $.each(vm.productList, function(index, item) {
                item.ticked = false;
            })
        };
        vm.deleteProduct = function (product) {
            $scope.selectedProducts.splice($scope.selectedProducts.indexOf(product), 1);
        };
        vm.cancel = function () {
            $(".child-tr").remove();
            vm.addDisable = false;
            $scope.groupProduct.enabled =false;
        };
        vm.createOrUpdate = function (groupProduct) {
            groupProduct.CustomerTempModelList = $scope.selectedCustomers;
            groupProduct.ProductTempModelList = $scope.selectedProducts;
            if (!groupProduct.Id) {
                adminBuyerGroupFactory.addOrUpdate(groupProduct).then(function (res) {
                    $(".child-tr").remove();
                    $scope.filterBuyerGroup = "";
                    $scope.search();
                    vm.addDisable = false;
                    $scope.groupProduct.enabled = false;
                    growl.success("Item has been added");
                });
            } else {
                adminBuyerGroupFactory.addOrUpdate(groupProduct).then(function (res) {
                    $(".child-tr").remove();
                    $scope.filterBuyerGroup = "";
                    $scope.search();
                    $scope.groupProduct.enabled = false;
                    growl.success("Item has been updated");
                });
            }
        };

    }
]);