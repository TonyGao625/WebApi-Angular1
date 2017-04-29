//angular.module('app').directive('popOver', [
//    function () {
//        return {
//            link: function (scope, ele, attr) {
//                ele.attr('title', scope.product.Code);
//                ele.attr('data-html', true);
//                ele.attr('data-content', '<h4>Product information</h4><p>' + scope.product.LongDesc +
//                    '</p><p><b>Vendor</b>:' + scope.product.Vendor + '</p><p>' +
//                    '<b>UOM</b>: <span>' + scope.product.Uom.Name + '</span></p>' + '<p><b>Price</b>: <span>$' + scope.product.Price + '</span></p>');
//                ele.popover();
//            }
//        }
//    }
//]);