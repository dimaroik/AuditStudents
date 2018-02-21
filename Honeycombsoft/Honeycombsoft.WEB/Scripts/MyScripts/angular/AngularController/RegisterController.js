var app = angular.module('HoneyApp')

app.controller('RegisterController', function ($scope, RegistrationService) {

    $scope.submitText = "Save";
    $scope.submitted = false;
    $scope.message = '';
    $scope.isFormValid = false;

    $scope.RegisterModel = {
        Name: '',
        LastName: '',
        Email: '',
        Password: '',
        ConfirmPassword: '',
        Age: ''
    };

    $scope.$watch('f1.$valid', function (newValid) {
        $scope.isFormValid = newValid;
    });

    $scope.SaveData = function (data) {
        if ($scope.submitText == 'Save') {
            $scope.submitted = true;
            $scope.message = '';

            if ($scope.isFormValid) {
                $scope.submitText = 'Please wait...';
                $scope.RegisterModel = data;
                RegistrationService.SaveFormData($scope.RegisterModel).then(function (d) {
                    alert(d);
                    if (d == 'Success') {
                        ClearForm();
                        window.location.pathname = 'Account/NotConfirm';
                    }
                    $scope.submitText = "Save";
                });
            }
        }
    }

    function ClearForm() {
        $scope.RegisterModel = {};
        $scope.f1.$setPristine();
        $scope.submitted = false;
    }


})
.factory('RegistrationService', function ($http , $q) {

    var fac = {};
    fac.SaveFormData = function (data) {
        var defer = $q.defer();
        $http({
            url: '/Account/RegisterData',
            method: 'POST',
            data: JSON.stringify(data),
            headers: { 'content-type': 'application/json' }
        }).success(function (d) {
            defer.resolve(d);
        }).error(function (e) {
            alert('Error');
            defer.reject(e);
        })
        return defer.promise;
    }
    return fac;
}); 