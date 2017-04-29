angular.module('app').filter('timezone', ['TIME', function (TIME)
{
    return function (date)
    {
        return date ? moment(date).add(TIME, 'hour').format("YYYY-MM-DD HH:mm:ss") : "";
    }
}])