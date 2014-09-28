'use strict';

/* App Module */

(function(){

var app = angular.module('coachSeekApp', ['ngRoute', 'coachSeekControllers', 'coachSeekDirectives']);



app.config(['$routeProvider', '$locationProvider',
  function($routeProvider, $locationProvider) {
    $routeProvider.
      when('/', {
        templateUrl: 'partials/business-registration.html',
        controller: 'BusinessRegCtrl'
      }).
      when('/business/registration', {
        templateUrl: 'partials/business-registration.html',
        controller: 'BusinessRegCtrl'
      }).
      when('/business/locations', {
          templateUrl: 'partials/business-locations.html',
        controller: 'LocationCtrl'
      }).
      otherwise({
        redirectTo: '/business'
      });
      
    // use the HTML5 History API
		//$locationProvider.html5Mode(true);
  }]);

})();