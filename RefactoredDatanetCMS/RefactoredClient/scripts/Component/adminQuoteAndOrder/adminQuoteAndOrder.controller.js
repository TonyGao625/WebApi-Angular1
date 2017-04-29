angular.module("app").controller('adminQuoteAndOrderCtrl', [
    'adminQuoteAndOrderFactory', '$scope', '$http', 'growl', '$templateRequest', '$compile', 'adminCustomerFactory', 'httpRequest', 'session',
    function (adminQuoteAndOrderFactory, $scope, $http, growl, $templateRequest, $compile, adminCustomerFactory, httpRequest, session) {
        $scope.init = function () {
            $scope.userInfo = session.getUser();
            $scope.companyNames = {};
            $scope.contactName = "";

            adminCustomerFactory.getAllName().then(function (res) {
                $scope.customers = angular.copy(res.data.Datas);
                $scope.orderCode = "";
                $scope.selectedTypeId = "1";
                $scope.contactName = "";
                $("#formdate").val("");
                $("#todate").val("");
                $scope.customers.unshift({
                    CompanyName: "All",
                    ticked: true
                });
                $scope.search();
            });
        }

        $scope.searchReset = function () {
            $.each($scope.customers, function (index, item) {
                if (item.CompanyName === "All") {
                    item.ticked = true;
                } else {
                    item.ticked = false;
                }
            })
            $scope.orderCode = "";
            $scope.selectedTypeId = "1";
            $scope.contactName = "";
            $("#formdate").val("");
            $("#todate").val("");
        }

        $scope.search = function (page) {
            page = page || 0;
            var companyName = "", contactName = "";
            if ($scope.companyNames[0] && $scope.companyNames[0].CompanyName !== "All") {
                companyName = $scope.companyNames[0].CompanyName;
            }
            var filterModel = {
                //FromDate: $("#formdate").val(),
                //ToDate: $("#todate").val(),
                FromDate: moment.utc(moment($("#formdate").val())).format("YYYY-MM-DD HH:mm:ss"),
                ToDate: moment.utc(moment($("#todate").val())).format("YYYY-MM-DD HH:mm:ss"),
                Mode: $scope.selectedTypeId,
                CompanyName: companyName,
                ContactName: $scope.contactName,
                OrderCode: $scope.orderCode,
                page: page,
                pageSize: 10,
                //ManagerId: $scope.userInfo.Role === 'Manager' ? $scope.userInfo.Id : 0
                ManagerId: 0
            }
            httpRequest.post(window.configuration.baseUrl + "Order/Search", filterModel).then(function (res) {
                $scope.quoteOrders = res.data.Data.Items;
                $scope.page = res.data.Data.Page;
                $scope.pagesCount = res.data.Data.TotalPages;
                $scope.totalCount = res.data.Data.TotalCount;
                if ($scope.quoteOrders.length === 0) {
                    growl.info("No result has been found ");
                }
                $(".child-tr").remove();
            })
        };
        $scope.searchProducts = function (id, page) {
            page = page || 0;
            var data = {
                params: {
                    page: page,
                    pageSize: 10,
                    id: id
                }
            };
            httpRequest.get(window.configuration.baseUrl + "Order/GetOrderOrQuoteProduct", data).then(function (res) {
                $scope.quoteOrder = res.data.Data.Item;
                if ($scope.quoteOrder.OrderAddressModels && $scope.quoteOrder.OrderAddressModels[0]) {
                    $scope.quoteOrder.Addr1 = $scope.quoteOrder.OrderAddressModels[0].Addr1;
                    $scope.quoteOrder.Addr2 = $scope.quoteOrder.OrderAddressModels[0].Addr2;
                    $scope.quoteOrder.Addr3 = $scope.quoteOrder.OrderAddressModels[0].Addr3;
                    $scope.quoteOrder.State = $scope.quoteOrder.OrderAddressModels[0].State;
                    $scope.quoteOrder.PostCode = $scope.quoteOrder.OrderAddressModels[0].PostCode;
                }
                $scope.pageProduct = res.data.Data.Page;
                $scope.pagesCountProduct = res.data.Data.TotalPages;
                $scope.totalCountProduct = res.data.Data.TotalCount;
                $(".download-file").attr("href", window.configuration.fileUrl + $scope.quoteOrder.CustomerId + "/" + $scope.quoteOrder.PoDocPath);
            })
        };
        $scope.clearScope = function () {
            $scope.quoteOrder = [];
        };
        $scope.showItem = function (quoteOrder, $event) {
            if (!$($event.currentTarget).hasClass("toggle")) {
                $templateRequest('/Scripts/Component/adminQuoteAndOrder/adminQuoteAndOrderDetail.html').then(function (template) {
                    $scope.clearScope();
                    $scope.searchProducts(quoteOrder.Id);
                    $(".child-tr").remove();
                    var tmp = $compile(template)($scope);
                    $($event.currentTarget).addClass("toggle");
                    $($event.currentTarget).parent().parent('tr').after(tmp);
                });
            } else {
                if ($($event.currentTarget).parent().parent('tr').next().is(".child-tr")) {
                    $(".child-tr").remove();
                    $($event.currentTarget).removeClass("toggle");
                } else {
                    $templateRequest('/Scripts/Component/adminQuoteAndOrder/adminQuoteAndOrderDetail.html').then(function (template) {
                        $scope.clearScope();
                        $scope.searchProducts(quoteOrder.Id);
                        $(".child-tr").remove();
                        var tmp = $compile(template)($scope);
                        $($event.currentTarget).addClass("toggle");
                        $($event.currentTarget).parent().parent('tr').after(tmp);
                    });
                }
            }
        }
        $scope.email = function (quoteOrder) {
            bootbox.confirm("Are you sure you want to Send email?", function (res) {
                if (!res) {
                    return;
                }
                httpRequest.get(window.configuration.baseUrl + "Order/SendEmailById?id=" + quoteOrder.Id).then(function (res) {
                    growl.success("Email has been sent successfully.");
                })
            });

        }
    }]);

