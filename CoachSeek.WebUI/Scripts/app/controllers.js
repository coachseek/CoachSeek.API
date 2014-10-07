
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


coachSeekControllers.controller('LocationCtrl', ['$scope', '$filter', '$http', '$rootScope', function ($scope, $filter, $http, $rootScope) {

    $scope.locations = [];

    $scope.checkLocation = function (name, id) {
        if (isNewLocation(id))
            return checkNewLocation(name);
        
        return checkExistingLocation(name, id);
    };


    function isNewLocation(id) {
        return id === undefined;
    }

    function checkNewLocation(name) {
        name = name || "";
        if (name == "")
            return "Location name is required.";
        if (containsLocationName(name))
            return "Location '" + name + "' already exists.";
        
        return true;
    }

    function checkExistingLocation(name, id) {
        name = name || "";
        if (name == "")
            return "Location name is required.";
        if (containsLocationName(name, id)) {
            return "Location '" + name + "' already exists.";
        }
        return true;
    }

    function containsLocationName(name, excludeId) {
        excludeId = excludeId || "";
        for (var i = 0; i < $scope.locations.length; i++) {
            if ($scope.locations[i].id == excludeId)
                continue;
            if ($scope.locations[i].name.toLowerCase() == name.toLowerCase())
                return true;
        }
        return false;
    }


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


coachSeekControllers.controller('CoachCtrl', ['$scope', '$filter', '$http', '$rootScope', function ($scope, $filter, $http, $rootScope) {

    $scope.coaches = [];

    $scope.saveCoach = function (data, id) {
        var coach = {};
        coach.businessId = $rootScope.business.id;
        coach.id = id;
        coach.firstname = data.firstname;
        coach.lastname = data.lastname;
        coach.email = data.email;
        coach.phone = data.phone;

        return $http.post('/api/Coaches', coach)
           .success(function (business) {
               $rootScope.business = business;
               $scope.coaches = business.coaches;
           })
           .error(function (error) {
               $scope.error = error;
           });
    };

    $scope.addCoach = function () {
        $scope.inserted = { firstname: '', lastname: '' };
        $scope.coaches.push($scope.inserted);
    };
}]);

})();