angular.module('app').directive('initDatetime', [function ()
{
    return {
        link: function (scope, element, attr)
        {
            element.datetimepicker({
                format: 'YYYY-MM-DD',
                useCurrent: false,
                focusOnShow:false
            });
            
        }
    }
}])