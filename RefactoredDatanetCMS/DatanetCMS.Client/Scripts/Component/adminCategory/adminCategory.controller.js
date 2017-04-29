angular.module("app").controller('adminCategoryCtrl', ['adminCategoryFactory', '$scope', '$http', 'growl', '$templateRequest', '$compile', function (adminCategoryFactory, $scope, $http, growl, $templateRequest, $compile) {
    var vm = this;
    $scope.search = function (page) {
        //if (page === 0 || page) {
        //    $scope.filterCategory = "";
        //}
        page = page || 0;
        var data = {
            params: {
                page: page,
                pageSize: 10,
                filter: $scope.filterCategory
            }
        };
        adminCategoryFactory.search(data).then(function (res) {
            $scope.categories = res.data.Data.Items;
            $scope.page = res.data.Data.Page;
            $scope.pagesCount = res.data.Data.TotalPages;
            $scope.totalCount = res.data.Data.TotalCount;
            if ($scope.filterCategory && $scope.filterCategory.length) {
                growl.success(res.data.Data.Items.length + (res.data.Data.Items.length > 1 ? ' categories found' : ' category found'));
            }
        });
    };
    $scope.search();
    $scope.cancel = function () {
        $(".child-tr").remove();
        $scope.addDisable = false;
    };
    $scope.showItem = function (category, $event) {
        $scope.addDisable = false;
        $templateRequest('/Scripts/Component/adminCategory/adminCategoryDetail.html').then(function (template) {
            $scope.category = angular.copy(category);
            $(".child-tr").remove();
            var tmp = $compile(template)($scope);
            $($event.currentTarget).parent().parent('tr').after(tmp);
        });
    };
    $scope.add = function ($event) {
        $scope.addDisable = true;
        $templateRequest('/Scripts/Component/adminCategory/adminCategoryDetail.html').then(function (template) {
            $(".child-tr").remove();
            $scope.category= {
                Id: "",
                CategoryName: ""
            }
            var tmp = $compile(template)($scope);
            $($event.currentTarget).parent().parent('tr').after(tmp);
        });
    };
    $scope.createOrUpdate = function (category) {
        if (!category.Id) {
            adminCategoryFactory.addOrUpdate(category).then(function (res) {
                $scope.filterCategory = '';
                $scope.search();
                $scope.addDisable = false;
                growl.success("Item has been added");
                $(".child-tr").remove();
                $scope.category.enabled = false;
            });
        } else {
            adminCategoryFactory.addOrUpdate(category).then(function (res) {
                $scope.filterCategory = '';
                $scope.search();
                growl.success("Item has been updated");
                $(".child-tr").remove();
                $scope.category.enabled = false;
            });
        }
    }
    $scope.delete = function (category) {
        if (!category.Id) {
            $scope.categories.shift();
            $scope.addDisable = false;
            return;
        }
        bootbox.confirm("Are you sure you want to delete this ?", function (res) {
            if (!res) {
                return;
            }
            adminCategoryFactory.deleteById(category.Id).then(function () {
                $scope.categories.splice($scope.categories.indexOf(category), 1);
                growl.success('Delete success');

            });
        });
    }
}]);

