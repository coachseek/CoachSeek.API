'use strict';

/* Controllers */

(function(){

var coachSeekControllers = angular.module('coachSeekControllers', []);

coachSeekControllers.controller('BusinessRegCtrl', ['$scope', '$http', function ($scope, $http) {
  $scope.businessReg = {};
  $scope.business = {};

  $scope.registerBusiness = function() {
      $http.post('/api/BusinessRegistration', $scope.businessReg)
           .success(function (data) {
               $scope.business = data;
           })
           .error(function (data) {
               $scope.error = data;
           });
  };
  
} ]);

})();