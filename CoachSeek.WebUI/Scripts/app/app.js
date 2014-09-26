'use strict';

/* App Module */

(function(){

var app = angular.module('coachSeekApp', ['coachSeekControllers', 'coachSeekDirectives']);



//var app = angular.module('coachSeekApp', ['ngRoute', 'coachSeekControllers', 'coachSeekDirectives']);

//app.config(['$routeProvider', '$locationProvider',
//  function($routeProvider, $locationProvider) {
//    $routeProvider.
//      when('/', {
//        templateUrl: 'partials/business-registration.html',
//        controller: 'BusinessRegCtrl'
//      }).
//      when('/business', {
//        templateUrl: 'partials/business-registration.html',
//        controller: 'BusinessRegCtrl'
//      }).
//      when('/admin', {
//        templateUrl: 'partials/admin-registration.html',
//        controller: 'AdminCtrl'
//      }).
//      otherwise({
//        redirectTo: '/business'
//      });
      
//    // use the HTML5 History API
//		$locationProvider.html5Mode(true);
//  }]);

})();