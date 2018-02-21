(function () {

    var app = angular.module('HoneyApp', ['ngRoute']);

    app.controller('HomeController', function ($scope) {
        $scope.Message = "Succsesful! Angular is connection to ASP.NET";
    });



})();