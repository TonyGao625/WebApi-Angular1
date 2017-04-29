angular.module("app").controller('adminCustomerCtrl', ['adminCustomerFactory', 'adminDeliveryAddrFactory', 'adminManagerFactory', 'adminBuyerGroupFactory', '$scope', '$http', 'growl', '$templateRequest', '$compile', 'adminContractFactory', '$timeout',
    function (adminCustomerFactory, adminDeliveryAddrFactory, adminManagerFactory, adminBuyerGroupFactory, $scope, $http, growl, $templateRequest, $compile, adminContractFactory, $timeout) {
        $scope.managers = [];
        $scope.init = function () {
            $scope.search();
            adminBuyerGroupFactory.getAll().then(function (result) {
                $scope.buyerGroups = result.data.Datas;
            });
            adminManagerFactory.getAll().then(function (result) {
                $.each(result.data.Datas, function (index, item) {
                    if (item.Role === "Manager") {
                        $scope.managers.push(item);
                    }
                });
            });
        }
        $scope.search = function (page) {
            //if (page === 0 || page) {
            //    $scope.filterCustomer = "";
            //}
            page = page || 0;
            var data = {
                params: {
                    page: page,
                    pageSize: 10,
                    filter: $scope.filterCustomer
                }
            };
            adminCustomerFactory.search(data).then(function (res) {
                $scope.customers = res.data.Data.Items;
                $scope.page = res.data.Data.Page;
                $scope.pagesCount = res.data.Data.TotalPages;
                $scope.totalCount = res.data.Data.TotalCount;
                if ($scope.filterCustomer && $scope.filterCustomer.length) {
                    growl.success(res.data.Data.Items.length + (res.data.Data.Items.length > 1 ? ' customers found' : ' customer found'));
                }
            });
        };
        $scope.showItem = function (customer, $event) {
            $scope.addDisable = false;
            $scope.addr = [];
            if ($scope.addrs instanceof Array) {
                $scope.addr = $scope.addrs.filter(function (item) {
                    return item.CustomerId === customer.Id;
                });
            }
            $templateRequest('/Scripts/Component/adminCustomer/adminCustomerDetail.html').then(function (template) {
                adminCustomerFactory.getById(customer.Id).then(function (res) {
                    customer = res.data.Data;
                    $scope.customer = angular.copy(customer);
                    //set buyer groups items
                    angular.forEach($scope.buyerGroups,
                        function (value, key) {
                            if (value.Id === customer.BuyerGroupId) {
                                value.ticked = true;
                            } else {
                                value.ticked = false;
                            }
                        });
                    //set manager items
                    angular.forEach($scope.managers, function (value, key) {
                        if (value.Id === customer.ManagerId) {
                            value.ticked = true;
                        } else {
                            value.ticked = false;
                        }
                    });
                    $(".child-tr").remove();
                    var tmp = $compile(template)($scope);
                    $($event.currentTarget).parent().parent('tr').after(tmp);
                });
            });
        }
        $scope.showAddressItem = function (address, $event) {
            $scope.addAddressDisable = false;
            $templateRequest('/Scripts/Component/adminCustomer/adminAddressDetail.html').then(function (template) {
                $scope.address = angular.copy(address);
                $(".childAddress-tr").remove();
                var tmp = $compile(template)($scope);
                $($event.currentTarget).parent().parent('').after(tmp);
            });
        }
        $scope.showContractItem = function (contract, $event) {
            $scope.addContractDisable = false;
            $templateRequest('/Scripts/Component/adminCustomer/adminContractDetail.html').then(function (template) {
                $scope.contract = angular.copy(contract);
                $(".childContract-tr").remove();
                var tmp = $compile(template)($scope);
                $($event.currentTarget).parent().parent('').after(tmp);
            });
        }
        $scope.getOverViewByCustomerId = function ($event, customer) {
            $scope.addContractDisable = false;
            $scope.addAddressDisable = false;
            $($event.currentTarget).parent().next().remove();
            $templateRequest('/Scripts/Component/adminCustomer/adminCustomerOverView.html').then(function (template) {
                $scope.customer = angular.copy(customer);
                $scope.customer.enabled = false;
                var tmp = $compile(template)($scope);
                $($event.currentTarget).parent().after(tmp);
            });
        }
        $scope.getAddressByCustomerId = function ($event, customer) {
            $scope.addContractDisable = false;
            $scope.addAddressDisable = false;
            $($event.currentTarget).parent().next().remove();
            $templateRequest('/Scripts/Component/adminCustomer/adminAddress.html').then(function (template) {
                $scope.searchAddress(customer.Id);
                var tmp = $compile(template)($scope);
                $($event.currentTarget).parent().after(tmp);
            });
        }
        $scope.getContractNumberByCustomerId = function ($event, customer) {
            $scope.addContractDisable = false;
            $scope.addAddressDisable = false;
            $($event.currentTarget).parent().next().remove();
            $templateRequest('/Scripts/Component/adminCustomer/adminContract.html').then(function (template) {
                $scope.searchContract(customer.Id);
                var tmp = $compile(template)($scope);
                $($event.currentTarget).parent().after(tmp);
            });
        }
        $scope.searchAddress = function (customerId, page) {
            //if (page === 0 || page) {
            //    $scope.filterPostCode = "";
            //}
            page = page || 0;
            var data = {
                params: {
                    page: page,
                    pageSize: 10,
                    filter: $scope.filterPostCode,
                    CustomerId: customerId
                }
            };
            adminDeliveryAddrFactory.search(data).then(function (res) {
                $scope.addrs = res.data.Data.Items;
                $scope.pageAddress = res.data.Data.Page;
                $scope.pagesCountAddress = res.data.Data.TotalPages;
                $scope.totalCountAddress = res.data.Data.TotalCount;
                if ($scope.filterPostCode && $scope.filterPostCode.length) {
                    growl.success(res.data.Data.Items.length + (res.data.Data.Items.length > 1 ? ' addresses found' : ' address found'));
                }
            });
        };
        $scope.searchContract = function (customerId, page) {
            //if (page === 0 || page) {
            //    $scope.filterName = "";
            //}
            page = page || 0;
            var data = {
                params: {
                    page: page,
                    pageSize: 10,
                    filter: $scope.filterName,
                    CustomerId: customerId
                }
            };
            adminContractFactory.search(data).then(function (res) {
                $scope.contracts = res.data.Data.Items;
                $scope.pageContract = res.data.Data.Page;
                $scope.pagesCountContract = res.data.Data.TotalPages;
                $scope.totalCountContract = res.data.Data.TotalCount;
                if ($scope.filterName && $scope.filterName.length) {
                    growl.success(res.data.Data.Items.length + (res.data.Data.Items.length > 1 ? ' contracts found' : ' contract found'));
                }
            });
        };
        $scope.addNewLine = function ($event) {
            $scope.addDisable = true;
            angular.forEach($scope.managers, function (value, key) {
                value.ticked = false;
            });
            angular.forEach($scope.buyerGroups, function (value, key) {
                value.ticked = false;
            });
            $templateRequest('/Scripts/Component/adminCustomer/adminCustomerDetail.html').then(function (template) {
                $(".child-tr").remove();
                $scope.addr = [];
                $scope.customer = {
                    Id: 0,
                    CompanyName: "",
                    LoginId: "",
                    LoginPassword: "",
                    ContactFirstName: "",
                    ContactLastName: "",
                    ContactPhone: "",
                    ContactEmail: "",
                    ManagerId: 0,
                    Manager: null,
                    BuyerGroupId: 0,
                    ContractNo: "",
                    DisplayContractNo: false,
                    PurchaseType: 2,
                    POMandatory: false,
                    PODocMandatory: false,
                    BuyerGroup: null,
                    DeliveryCharge: "",
                    Logo: ""
                }
                var tmp = $compile(template)($scope);
                $($event.currentTarget).parent().parent('tr').after(tmp);
            });
        };
        $scope.addAddress = function ($event, customer) {
            $scope.addAddressDisable = true;
            $templateRequest('/Scripts/Component/adminCustomer/adminAddressDetail.html').then(function (template) {
                $(".childAddress-tr").remove();
                $scope.addr = [];
                $scope.address = {
                    Id: 0,
                    Addr1: "",
                    Addr2: "",
                    Addr3: "",
                    State: "",
                    PostCode: "",
                    Phone: "",
                    CustomerId: customer.Id
                }
                var tmp = $compile(template)($scope);
                $($event.currentTarget).parent().parent('tr').after(tmp);
            });
        }
        $scope.addContract = function ($event, customer) {
            $scope.addContractDisable = true;
            $templateRequest('/Scripts/Component/adminCustomer/adminContractDetail.html').then(function (template) {
                $(".childContract-tr").remove();
                $scope.contract = [];
                $scope.contract = {
                    Id: 0,
                    Name: "",
                    Desc: "",
                    CustomerId: customer.Id
                }
                var tmp = $compile(template)($scope);
                $($event.currentTarget).parent().parent('tr').after(tmp);
            });
        }
        $scope.clearImg = function (e, customer) {
            customer.Logo = '';
            customer.enabled = true;
            $(e.currentTarget).closest(".form-group").find("input[type='file']").val('');
        };
        $scope.cancel = function () {
            $(".child-tr").remove();
            $scope.addDisable = false;
            $scope.search();
        };
        $scope.cancelAddress = function () {
            $(".childAddress-tr").remove();
            $scope.addAddressDisable = false;
            // $scope.search();
        };
        $scope.cancelContract = function () {
            $(".childContract-tr").remove();
            $scope.addContractDisable = false;
            //$scope.search();
        };
        $scope.createOrUpdate = function (customer) {
            if (customer.BuyerGroup[0]) {
                customer.buyerGroupId = customer.BuyerGroup[0].Id;
            }
            if (customer.Manager[0]) {
                customer.ManagerId = customer.Manager[0].Id;
            }
            if (!customer.Id) {
                adminCustomerFactory.addOrUpdate(customer).then(function (res) {
                    $scope.customer = res.data.Data;
                    if (customer.PurchaseType === "1" || customer.DisplayContractNo) {
                        adminContractFactory.getContractsByCustomerId(customer.Id)
                            .then(function (res) {
                                if (res.data.Datas.length === 0) {
                                    bootbox.alert("Please do not forget to set up contract",
                                        function (res) {
                                            $(".contract").parent().next().remove();
                                            $templateRequest('/Scripts/Component/adminCustomer/adminContract.html')
                                                .then(function (template) {
                                                    $scope.contracts = [];
                                                    var tmp = $compile(template)($scope);
                                                    $(".contract").parent().after(tmp);
                                                    $(".contract").addClass("active").siblings().removeClass("active");
                                                });
                                        });
                                } else {
                                    growl.success("Item has been added");
                                    $scope.filterCustomer = '';
                                    $scope.search();
                                    $scope.addDisable = false;
                                    $scope.customer.enabled = false;
                                }
                            });
                    } else {
                        $scope.filterCustomer = '';
                        $scope.search();
                        $scope.addDisable = false;
                        growl.success("Item has been added");
                        $scope.customer.enabled = false;
                    }
                });
            } else {
                adminCustomerFactory.addOrUpdate(customer).then(function (result) {
                    if (customer.PurchaseType === "1" || customer.DisplayContractNo) {
                        adminContractFactory.getContractsByCustomerId(customer.Id)
                            .then(function (res) {
                                if (res.data.Datas.length === 0) {
                                    bootbox.alert("Please do not forget to set up contract",
                                        function (res) {
                                            $(".contract").parent().next().remove();
                                            $templateRequest('/Scripts/Component/adminCustomer/adminContract.html')
                                                .then(function (template) {
                                                    $scope.contracts = [];
                                                    var tmp = $compile(template)($scope);
                                                    $(".contract").parent().after(tmp);
                                                    $(".contract").addClass("active").siblings().removeClass("active");
                                                });
                                        });
                                } else {
                                    growl.success("Item has been updated");
                                    $scope.filterCustomer = '';
                                    $scope.search();
                                    $scope.addDisable = false;
                                    $scope.customer.enabled = false;
                                }
                            });
                    } else {
                        $scope.filterCustomer = '';
                        $scope.search();
                        $scope.addDisable = false;
                        growl.success("Item has been updated");
                        $scope.customer.enabled = false;
                    }
                });
            }
        }
        $scope.createOrUpdateAddress = function (address) {
            if (!address.Id) {
                adminDeliveryAddrFactory.addOrUpdate(address).then(function (res) {
                    $scope.filterPostCode = '';
                    $scope.searchAddress(address.CustomerId);
                    $scope.addAddressDisable = false;
                    growl.success("Item has been added");
                    $(".childAddress-tr").remove();
                    $scope.address.enabled = false;
                });
            } else {
                adminDeliveryAddrFactory.addOrUpdate(address).then(function (res) {
                    $scope.filterPostCode = '';
                    $scope.searchAddress(address.CustomerId);
                    $scope.addAddressDisable = false;
                    growl.success("Item has been updated");
                    $(".childAddress-tr").remove();
                    $scope.address.enabled = false;
                });
            }
        }
        $scope.createOrUpdateContract = function (contract) {
            if (!contract.Id) {
                adminContractFactory.addOrUpdate(contract).then(function (res) {
                    $scope.filterName = '';
                    $scope.searchContract(contract.CustomerId);
                    $scope.addContractDisable = false;
                    growl.success("item has been added");
                    $(".childContract-tr").remove();
                    $scope.contract.enabled = false;
                });
            } else {
                adminContractFactory.addOrUpdate(contract).then(function (res) {
                    $scope.filterName = '';
                    $scope.searchContract(contract.CustomerId);
                    growl.success("Item has been updated");
                    $(".childContract-tr").remove();
                    $scope.contract.enabled = false;
                });
            }
        }
        $scope.deleteAddress = function (address) {
            if (!address.Id) {
                $scope.addrs.shift();
                $scope.addAddressDisable = false;
                return;
            }
            bootbox.confirm("Are you sure you want to delete this ?", function (res) {
                if (!res) {
                    return;
                }
                adminDeliveryAddrFactory.deleteById(address.Id).then(function () {
                    $scope.addrs.splice($scope.addrs.indexOf(address), 1);
                    growl.success('Delete success');
                });
            });
        }
        $scope.deleteContract = function (contract) {
            if (!contract.Id) {
                $scope.contracts.shift();
                $scope.addContractDisable = false;
                return;
            }
            bootbox.confirm("Are you sure you want to delete this ?", function (res) {
                if (!res) {
                    return;
                }
                adminContractFactory.deleteById(contract.Id).then(function () {
                    $scope.contracts.splice($scope.contracts.indexOf(contract), 1);
                    growl.success('Delete success');
                });
            });
        }
        $scope.delete = function (customer) {
            if (!customer.Id) {
                $scope.customers.shift();
                $scope.addDisable = false;
                return;
            }
            bootbox.confirm("Are you sure you want to delete this ?", function (res) {
                if (!res) {
                    return;
                }
                adminCustomerFactory.deleteById(customer.Id).then(function () {
                    $scope.customers.splice($scope.customers.indexOf(customer), 1);
                    growl.success('Delete success');
                });
            });
        }
        $scope.callback = function () {
            var addressList = [];
            $scope.result.shift();
            $scope.result.pop();
            $.each($scope.result, function (index,item) {
                var itemArray = item[0].split(",");
                addressList.push({
                    Addr1: itemArray[0],
                    Addr2: itemArray[1],
                    Addr3: itemArray[2],
                    State: itemArray[3],
                    PostCode: itemArray[4],
                    Phone: itemArray[5],
                    customerId: $scope.customer.Id
                })
            })
            var model = {
                "DeliveryAddrModel": addressList
            }
            adminDeliveryAddrFactory.addBulkAddress(model).then(function (res) {
                growl.success("Addresses imported successfully");
                $scope.searchAddress($scope.customer.Id);
            });
        }
        $scope.triggerUploadFile = function () {
            $("input[type='file']").attr("accept", ".csv");
            $("input[type='file']").click();
        }
    }
]);