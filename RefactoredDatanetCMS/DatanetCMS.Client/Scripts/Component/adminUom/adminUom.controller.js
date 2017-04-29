angular.module("app").controller('adminUomCtrl', ['adminUomFactory', '$scope', '$http', 'growl', '$templateRequest', '$compile', function (adminUomFactory, $scope, $http, growl, $templateRequest, $compile) {
    var vm = this;
    $scope.search = function (page) {
        //if (page===0||page) {
        //    $scope.filterUom = "";
        //}
        page = page || 0;
        var data = {
            params: {
                page: page,
                pageSize: 10,
                filter: $scope.filterUom
            }
        };
        adminUomFactory.search(data).then(function (res) {
            vm.uoms = res.data.Data.Items;
            $scope.page = res.data.Data.Page;
            $scope.pagesCount = res.data.Data.TotalPages;
            $scope.totalCount = res.data.Data.TotalCount;
            if ($scope.filterUom && $scope.filterUom.length) {
                growl.success(res.data.Data.Items.length + (res.data.Data.Items.length > 1 ? ' UOMs found' : ' UOM found'));
            }
        });
    };
    $scope.search();
    vm.cancel = function () {
        $(".child-tr").remove();
        vm.addDisable = false;
    };
    vm.showItem = function(uom, $event) {
        $templateRequest('/Scripts/Component/adminUom/adminUomDetail.html').then(function (template) {
            $scope.uom = angular.copy(uom);
            $(".child-tr").remove();
            var tmp = $compile(template)($scope);
            $($event.currentTarget).parent().parent('tr').after(tmp);
        });
    };
    vm.add = function ($event) {
        vm.addDisable = true;
        $templateRequest('/Scripts/Component/adminUom/adminUomDetail.html').then(function (template) {
            $(".child-tr").remove();
            $scope.uom= {
                Id: "",
                Name: ""
            }
            var tmp = $compile(template)($scope);
            $($event.currentTarget).parent().parent('tr').after(tmp);
        });
    };
    $scope.createOrUpdate= function(uom) {
        if (!uom.Id) {
            adminUomFactory.addOrUpdate(uom).then(function (res) {
                $scope.filterUom = '';
                $scope.search();
                vm.addDisable = false;
                growl.success("Item has been add");
                $(".child-tr").remove();
                $scope.uom.enabled = false;
            });
        } else {
            adminUomFactory.addOrUpdate(uom).then(function (res) {
                $scope.filterUom = '';
                $scope.search();
                growl.success("Item has been updated");
                $(".child-tr").remove();
                $scope.uom.enabled = false;
            });
        }
    }
    vm.delete = function (uom) {
        if (!uom.Id) {
            vm.uoms.shift();
            vm.addDisable = false;
            return;
        }
        bootbox.confirm("Are you sure you want to delete this ?", function (res) {
            if (!res) {
                return;
            }
            adminUomFactory.deleteOne(uom.Id).then(function () {
                vm.uoms.splice(vm.uoms.indexOf(uom), 1);
                growl.success('Delete success');

            });
        });
    }
}]);

