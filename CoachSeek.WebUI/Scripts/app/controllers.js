
'use strict';

/* Controllers */

(function(){

var coachSeekControllers = angular.module('coachSeekControllers', []);

coachSeekControllers.controller('BusinessRegCtrl', ['$scope', '$http', '$location', function ($scope, $http, $location) {
  $scope.businessReg = {};
  $scope.business = {};

  $scope.registerBusiness = function () {
      $scope.business = {};
      $scope.error = {};
      $http.post('/api/BusinessRegistration', $scope.businessReg)
           .success(function (data) {
               $scope.business = data;
               $location.path("/business/locations");
          })
           .error(function (data) {
               $scope.error = data;
               if (data.field === "Email")
                   $scope.businessRegForm.email.$setValidity("email", false);
           });
  };
  
}]);

coachSeekControllers.controller('LocationCtrl', ['$scope', '$http', function($scope, $http) {

}]);

})();