angular.module("app")
    .controller('adminManagerCtrl',
        ['adminManagerFactory', '$scope',
        '$http', 'growl',
        '$templateRequest', '$compile',
        function(
            adminManagerFactory,
            $scope,
            $http,
            growl,
            $templateRequest,
            $compile) {

            $scope.init = function () {
                $scope.search();
                $scope.roles = [
                    { Name: "Admin", ticked: false },
                    {Name: "Manager", ticked:true}
                ];
            }

            $scope.search = function (page) {
                //if (page === 0 || page) {
                //    $scope.filterManager = "";
                //}
                page = page || 0;
                var data = {
                    params: {
                        page: page,
                        pageSize: 10,
                        filter: $scope.filterManager
                    }
                };
                adminManagerFactory.search(data).then(function (res) {
                    $scope.managers = res.data.Data.Items;
                    $scope.page = res.data.Data.Page;
                    $scope.pagesCount = res.data.Data.TotalPages;
                    $scope.totalCount = res.data.Data.TotalCount;
                    if ($scope.filterManager && $scope.filterManager.length) {
                        growl.success(res.data.Data.Items.length + (res.data.Data.Items.length > 1 ? ' managers found' : ' manager found'));
                    }
                });
            };

            $scope.showItem = function (manager, $event) {
                $scope.addDisable = false;
                $templateRequest('/Scripts/Component/adminManager/adminManagerDetail.html').then(function (template) {
                    $scope.manager = angular.copy(manager);
                    if (manager.Role === "Admin") {
                        $scope.roles[0].ticked = true;
                        $scope.roles[1].ticked = false;
                    }
                    $(".child-tr").remove();
                    var tmp = $compile(template)($scope);
                    $($event.currentTarget).parent().parent('tr').after(tmp);
                });
            }

            $scope.addNewLine = function ($event) {
                $scope.addDisable = true;
                $templateRequest('/Scripts/Component/adminManager/adminManagerDetail.html').then(function (template) {
                    $(".child-tr").remove();
                    $scope.manager = {
                        Id: 0,
                        Name: "",
                        LoginName:"",
                        Password: "",
                        Role: "Manager",
                        Email: "",
                        Phone:""
                    }
                    var tmp = $compile(template)($scope);
                    $($event.currentTarget).parent().parent('tr').after(tmp);
                });
            };

            $scope.cancel = function () {
                $(".child-tr").remove();
                $scope.addDisable = false;
            };

            $scope.createOrUpdate = function (manager) {
                if ($scope.manager.Role === "") {
                    growl.error('Please select role');
                    return;
                }

                if (!manager.Id) {
                    adminManagerFactory.addOrUpdate(manager).then(function (result) {
                        $scope.filterManager = '';
                        $scope.search();
                        $scope.addDisable = false;
                        growl.success("Item has been add");
                        $(".child-tr").remove();
                        $scope.manager.enabled = false;
                    });
                } else {
                    adminManagerFactory.addOrUpdate(manager).then(function (result) {
                        $scope.filterManager = '';
                        $scope.search();
                        growl.success("Item has been updated");
                        $(".child-tr").remove();
                        $scope.manager.enabled = false;
                    });
                }
            }

            $scope.delete = function (manager) {
                if (!manager.Id) {
                    $scope.managers.shift();
                    $scope.addDisable = false;
                    return;
                }
                bootbox.confirm("Are you sure you want to delete this ?", function (res) {
                    if (!res) {
                        return;
                    }
                    adminManagerFactory.deleteById(manager.Id).then(function () {
                        $scope.managers.splice($scope.managers.indexOf(manager), 1);
                        growl.success('Delete success');
                    });
                });
            }

        }]);
