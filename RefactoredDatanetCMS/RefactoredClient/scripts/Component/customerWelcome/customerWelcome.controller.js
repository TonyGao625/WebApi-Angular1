angular.module("app").controller('customerWelcomeCtrl', [
    '$scope', '$http', 'growl', '$templateRequest',
    '$compile', '$window', 'session', 'adminCustomerFactory',
    'adminContractFactory', 'customerQToOrderFactory',
    function ($scope, $http, growl, $templateRequest,
        $compile, $window, session, adminCustomerFactory,
        adminContractFactory, customerQToOrderFactory) {
        $scope.contractNumbers = [];
        $scope.contractNumber = {};

        $scope.init = function () {
            $scope.userInfo = session.getUser();
            if (!$scope.userInfo) {
                growl.error("Get user inforamtion failed");
                return;
            }
            //adminCustomerFactory
            //    .getById($scope.userInfo.Id)
            //    .then(function (result) {
            //        $scope.user = result.data.Data;
            //        if ($scope.user.DisplayContractNo) {
            //            adminContractFactory.getContractsByCustomerId($scope.userInfo.Id).then(function (result) {
            //                var contractNos = angular.copy(result.data.Datas);
            //                var selectedContractNumber = $window.localStorage["selectedContractNumber"];
            //                if (!contractNos || contractNos.length === 0) {
            //                    growl.info("There is no contract number, please contact manager!");
            //                    return;
            //                }
            //                $scope.contractNumbers = contractNos.map(function (item, index, array) {
            //                    if (array.length === 1) {
            //                        item.ticked = true;
            //                        $scope.contractNumber = array;
            //                        $scope.selectContractNumber();
            //                    }
            //                    else {
            //                        if (item.Name === selectedContractNumber) {
            //                            item.ticked = true;
            //                        }
            //                        else {
            //                            item.ticked = false;
            //                        }
            //                    }
            //                    return item;
            //                });
            //            });
            //        }
            //    });

            if ($scope.userInfo.DisplayContractNo) {
                adminContractFactory.getContractsByCustomerId($scope.userInfo.Id).then(function (result) {
                    var contractNos = angular.copy(result.data.Datas);
                    var selectedContractNumber = $window.localStorage["selectedContractNumber"];
                    if (!contractNos || contractNos.length === 0) {
                        growl.info("There is no contract number, please contact manager!");
                        return;
                    }

                    $scope.contractNumbers = contractNos.map(function (item, index, array) {
                        if (array.length === 1) {
                            item.ticked = true;
                            $scope.contractNumber = array;
                            $scope.selectContractNumber();
                        }
                        else {
                            if (item.Name === selectedContractNumber) {
                                item.ticked = true;
                            }
                            else {
                                item.ticked = false;
                            }
                        }
                        return item;
                    });
                });
            }
        }

        $scope.selectContractNumber = function () {
            $window.localStorage.setItem("selectedContractNumber", $scope.contractNumber[0].Name);
        }

        $scope.checkContractNumber = function () {
            if ($scope.userInfo.DisplayContractNo && !$scope.contractNumber[0]) {
                growl.error("Please select a contract number to proceed");
                return false;
            }
            return true;
        }

        $window.localStorage.removeItem("state");
        $window.localStorage.removeItem("cart");

        $scope.toQuote = function () {
            if (!$scope.checkContractNumber()) return;
            $window.localStorage.removeItem("state");
            $window.localStorage.setItem("state", "quote");
            $window.location.href = window.configuration.clientBaseUrl + "products";
        }

        $scope.QtoOrder = function () {
            if (!$scope.checkContractNumber()) return;
            //check quote record
            //var data = {
            //    params: {
            //        page: -1,
            //        pageSize: -1,
            //        id: $scope.userInfo.Id,
            //        code: ""
            //    }
            //};

            //customerQToOrderFactory.searchQuotesByCustomerId(data)
            //    .then(function (result) {
            //        $scope.quotes = result.data.Data.Items;
            //        if (!$scope.quotes || $scope.quotes.length < 1) {
            //            growl.info("There is no quote record");
            //            return;
            //        }
            //        else {
            //            $window.localStorage.removeItem("state");
            //            $window.localStorage.setItem("state", "qtoorder");
            //            $window.location.href = window.configuration.clientBaseUrl + "qtoorder";
            //        }
            //    });

            $window.localStorage.removeItem("state");
            $window.localStorage.setItem("state", "qtoorder");
            $window.location.href = window.configuration.clientBaseUrl + "qtoorder";
        }
        $scope.toOrder = function () {
            if (!$scope.checkContractNumber()) return;
            $window.localStorage.removeItem("state");
            $window.localStorage.setItem("state", "order");
            $window.location.href = window.configuration.clientBaseUrl + "products";
        }

        $scope.toQuoteAndOrder = function () {
            if (!$scope.checkContractNumber()) return;
            //check quote and order
            //var data = {
            //    params: {
            //        page: 0,
            //        pageSize: 1,
            //        id: $scope.userInfo.Id,
            //        code: ""
            //    }
            //};

            //customerQToOrderFactory.searchQuotesByCustomerId(data)
            //    .then(function (result) {
            //        $scope.quotes = result.data.Data.Items;
            //        $scope.page = result.data.Data.Page;
            //        $scope.pagesCount = result.data.Data.TotalPages;
            //        $scope.totalCount = result.data.Data.TotalCount;

            //        var data1 = {
            //            params: {
            //                page: 0,
            //                pageSize: 1,
            //                id: $scope.userInfo.Id,
            //                code: ""
            //            }
            //        };

            //        customerQToOrderFactory.searchOrdersByCustomerId(data1)
            //            .then(function (result) {
            //                $scope.orders = result.data.Data.Items;
            //                if ((!$scope.quotes || $scope.quotes.length < 1)
            //                    && (!$scope.orders || $scope.orders.length < 1)) {
            //                    growl.info("There is no quote and order!");
            //                } else {
            //                    $window.localStorage.removeItem("state");
            //                    $window.localStorage.setItem("state", "qandorder");
            //                    $window.location.href = window.configuration.clientBaseUrl + "qandorder";
            //                }
            //            });
            //    });

            $window.localStorage.removeItem("state");
            $window.localStorage.setItem("state", "qandorder");
            $window.location.href = window.configuration.clientBaseUrl + "qandorder";
        }
    }]);

