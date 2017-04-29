(function () {
    $.ajax({
        url: '/Scripts/Config/data.json',
        async: false,
        dataType: 'json',
        success: function (data) {
            window.configuration = data;
        }
    });
    angular.module('app', ['ngMessages', 'ngAnimate', 'ngSanitize', 'angular-growl', "isteven-multi-select", "ui.router",
    "ngImageInputWithPreview", "ngFileUpload", 'ngCsvImport']);
})();