
'use strict';

/* Controllers */

(function(){

var coachSeekControllers = angular.module('coachSeekControllers', []);


coachSeekControllers.controller('BusinessRegCtrl', ['$scope', '$http', '$location', '$rootScope', function ($scope, $http, $location, $rootScope) {
  $scope.businessReg = {};
  $scope.business = {};

  $scope.registerBusiness = function () {
      $scope.business = {};
      $scope.error = {};
      $http.post('/api/BusinessRegistration', $scope.businessReg)
           .success(function (data) {
               $rootScope.business = data;
               $location.path("/business/locations");
          })
           .error(function (data) {
               $scope.error = data;
               if (data.field === "Email")
                   $scope.businessRegForm.email.$setValidity("email", false);
           });
  };
  
}]);


coachSeekControllers.controller('LocationCtrl',  ['$scope', '$filter', '$http', '$rootScope', function ($scope, $filter, $http, $rootScope) {

    $scope.locations = [
      //{ id: 1, name: 'Browns Bay' },
      //{ id: 2, name: 'Orakei' },
      //{ id: 3, name: 'Mt Roskill' }
    ];

    //$scope.groups = [];
    //$scope.loadGroups = function () {
    //    return $scope.groups.length ? null : $http.get('/groups').success(function (data) {
    //        $scope.groups = data;
    //    });
    //};

    //$scope.showGroup = function (user) {
    //    if (user.group && $scope.groups.length) {
    //        var selected = $filter('filter')($scope.groups, { id: user.group });
    //        return selected.length ? selected[0].text : 'Not set';
    //    } else {
    //        return user.groupName || 'Not set';
    //    }
    //};

    //$scope.showStatus = function (user) {
    //    var selected = [];
    //    if (user.status) {
    //        selected = $filter('filter')($scope.statuses, { value: user.status });
    //    }
    //    return selected.length ? selected[0].text : 'Not set';
    //};

    $scope.checkName = function (data, id) {
        if (id === 2 && data !== 'awesome') {
            return "Username 2 should be `awesome`";
        }
    };

    $scope.saveLocation = function (data, id) {
        var location = {};
        location.businessId = $rootScope.business.id;
        location.id = id;
        location.name = data.name;

        return $http.post('/api/Locations', location)
           .success(function (business) {
               $rootScope.business = business;
               $scope.locations = business.locations;
            })
           .error(function (error) {
               $scope.error = error;
           });
    };

    $scope.addLocation = function () {
        $scope.inserted = { name: '' };
        $scope.locations.push($scope.inserted);
    };
}]);

})();