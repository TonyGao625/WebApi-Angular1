angular.module("app").controller('adminProductCtrl', ['adminProductFactory', '$scope', '$http', 'growl', "adminUomFactory", '$templateRequest', '$compile', 'adminCategoryFactory',
    function (adminProductFactory, $scope, $http, growl, adminUomFactory, $templateRequest, $compile, adminCategoryFactory) {
        var vm = this;
        var uomNameList = [];
        var categoryNameList = [];
        adminUomFactory.getAll().then(function (res) {
            $scope.uoms = res.data.Datas;
            $.each($scope.uoms, function (index, item) {
                uomNameList.push(item.Name);
            });
        });
        adminCategoryFactory.getAll().then(function (res) {
            $scope.categories = res.data.Datas;
            $.each($scope.categories, function (index, item) {
                categoryNameList.push(item.CategoryName);
            });
        });

        $scope.search = function (page) {
            //if (page === 0 || page) {
            //    $scope.filterProduct = "";
            //}
            page = page || 0;
            var data = {
                params: {
                    page: page,
                    pageSize: 10,
                    filter: $scope.filterProduct
                }
            };
            adminProductFactory.search(data).then(function (res) {
                vm.products = res.data.Data.Items;
                $.each(vm.products, function (index, item) {
                    if (uomNameList.indexOf(item.Uom.Name) < 0) {
                        item.Uom.Name = "";
                    }
                });
                $scope.page = res.data.Data.Page;
                $scope.pagesCount = res.data.Data.TotalPages;
                $scope.totalCount = res.data.Data.TotalCount;
                if ($scope.filterProduct && $scope.filterProduct.length) {
                    growl.success(res.data.Data.Items.length + (res.data.Data.Items.length > 1 ? ' products found' : ' product found'));
                }
            });
        };
        $scope.search();

        vm.cancel = function () {
            $(".child-tr").remove();
            vm.addDisable = false;
        };
        vm.showItem = function (product, $event) {
            $templateRequest('/Scripts/Component/adminProduct/adminProductDetail.html').then(function (template) {
                adminProductFactory.getById(product.Id).then(function (res) {
                    if (res.data.Data.Image) {
                        product.Image = res.data.Data.Image;
                    } else {
                        product.Image = "";
                    }
                    $scope.product = angular.copy(product);
                    $.each($scope.uoms, function (index, item) {
                        if (product.UomId === item.Id) {
                            item.ticked = true;
                        } else {
                            item.ticked = false;
                        }
                    });
                    $.each($scope.categories, function (index, item) {
                        if (product.CategoryId === item.Id) {
                            item.ticked = true;
                        } else {
                            item.ticked = false;
                        }
                    });
                    $(".child-tr").remove();
                    var tmp = $compile(template)($scope);
                    $($event.currentTarget).parent().parent('tr').after(tmp);
                });

            });
        }
        vm.clearImg = function (e, product) {
            product.Image = '';
            product.enabled = true;
            $(e.currentTarget).closest(".form-group").find("input[type='file']").val('');
        }
        vm.add = function ($event) {
            vm.addDisable = true;
            $.each($scope.uoms, function (index, item) {
                item.ticked = false;
            });
            $.each($scope.categories, function (index, item) {
                item.ticked = false;
            });
            $templateRequest('/Scripts/Component/adminProduct/adminProductDetail.html').then(function (template) {
                $(".child-tr").remove();
                $scope.product = {
                    Id: "",
                    Code: "",
                    ShortDesc: "",
                    LongDesc: "",
                    Vendor: "",
                    Image: "",
                    Uom: {
                        Id: "",
                        Name: ""
                    },
                    Category: {
                        Id: "",
                        CategoryName: ""
                    }
                }
                var tmp = $compile(template)($scope);
                $($event.currentTarget).parent().parent('tr').after(tmp);
            });
        };
        vm.createOrUpdate = function (product) {
            if (!product.Uom[0]) {
                growl.error("UOM is required");
                return;
            }
            if (!product.Category[0]) {
                growl.error("Category is required");
                return;
            }
            product.UomId = product.Uom[0].Id;
            product.CategoryId = product.Category[0].Id;
            if (!product.Id) {
                adminProductFactory.addOrUpdate(product).then(function (res) {
                    $scope.filterProduct = '';
                    $scope.search();
                    vm.addDisable = false;
                    growl.success("Item has been add");
                    $(".child-tr").remove();
                    $scope.product.enabled = false;
                });
            } else {
                adminProductFactory.addOrUpdate(product).then(function (res) {
                    $scope.filterProduct = '';
                    $scope.search();
                    growl.success("Item has been updated");
                    $(".child-tr").remove();
                    $scope.product.enabled = false;
                });
            }
        }
        vm.delete = function (product) {
            if (!product.Id) {
                vm.products.shift();
                vm.addDisable = false;
                return;
            }
            bootbox.confirm("Are you sure you want to delete this ?", function (res) {
                if (!res) {
                    return;
                }
                adminProductFactory.deleteOne(product.Id).then(function () {
                    vm.products.splice(vm.products.indexOf(product), 1);
                    growl.success('Delete success');
                });
            });
        }
       
    }]);

