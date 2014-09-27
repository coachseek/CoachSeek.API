'use strict';

/* Controllers */

(function(){

var coachSeekControllers = angular.module('coachSeekControllers', []);

coachSeekControllers.controller('BusinessRegCtrl', ['$scope', '$http', function ($scope, $http) {
  $scope.businessReg = {};
  $scope.business = {};

  $scope.registerBusiness = function () {
      $scope.business = {};
      $scope.error = {};
      $http.post('/api/BusinessRegistration', $scope.businessReg)
           .success(function (data) {
               $scope.business = data;
           })
           .error(function (data) {
               $scope.error = data;
               if (data.field === "Email")
                   $scope.businessRegForm.email.$setValidity("email", false);
           });
  };
  
} ]);

})();