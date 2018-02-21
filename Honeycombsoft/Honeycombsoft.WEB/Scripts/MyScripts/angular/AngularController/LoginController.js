angular.module('HoneyApp' )
.controller('LoginController', function ($scope, LoginService) {

    $scope.IsLoginIn = false;
    $scope.Submitted = false;
    $scope.IsFormValid = false;

    $scope.LoginModel = {
        Email: '',
        Password: '' 
    };

    $scope.$watch('f1.$valid', function (newVal) {
        $scope.IsFormValid = newVal;
    }); 

    $scope.Login = function () {
        $scope.Submitted = true;
        if ($scope.IsFormValid) {
            LoginService.GetUser($scope.LoginModel).then(function (d) {
                if (d.data.Email != null) {
                    $scope.IsLoginIn = true;
                    window.location.pathname = 'Home/Index'; 
                } else {
                    alert("Invalid");
                }
            });
        }
    };
})
.factory('LoginService', function ($http) {
    var fac = {};

    fac.GetUser = function(d){
        return $http({
            url: '/Account/GetLoginModel',
            method: 'POST',
            data: JSON.stringify(d),
            headers: { 'content-type' : 'application/json' }
        });
    };


    return fac;
});