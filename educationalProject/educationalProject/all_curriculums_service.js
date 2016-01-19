app.factory('request_all_curriculums_service_server', function($http,$rootScope) {
var observerCallbacks = [];

      var my_curriculums = {
	   async: function() {
	 
	      var promise = $http.get('/api/curriculum').then(function (response) {
	       	
	        console.log(response);
	        $rootScope.all_curriculums = response.data;
	       console.log($rootScope.all_curriculums);
	        return response.data;
	      });
	  
	      return promise;
	    }
	  };
return my_curriculums;


});


app.factory('current_all_curriculums_service', function ($rootScope) {
    'use strict';

    var my_current_all_curriculums = [];

    var broadcast = function (new_all_curri) {
    console.log("new_all_curri_broadcast");
    console.log(new_all_curri);
      $rootScope.$broadcast('current_all_curriculums_service.update', new_all_curri);
      console.log("broadcast");
    };

    var update = function (new_all_curri) {
      my_current_all_curriculums = new_all_curri;
     console.log("new_all_curri_update");
    console.log(new_all_curri);

         console.log("my_current_all_curriculums");
    console.log(my_current_all_curriculums);

      $rootScope.$broadcast('current_all_curriculums_service.update', my_current_all_curriculums);
    };
    
    return {
      update: update,
      current_all_curriculums: my_current_all_curriculums,
    };
   });

// app.factory('update_curriculums_service', function($http) {

// 	 var observerCallbacks = [];
// 	 var current_all_curriculums;


//  var notifyObservers = function(){
//     angular.forEach(observerCallbacks, function(callback){
//       callback();
//     });
//   }
// 	// this.register_observers = function(callback){
//  //    observerCallbacks.push(callback);
//  //  };
//  var update_all_curriculums = function(new_all_curriculums){
//  	current_all_curriculums = new_all_curriculums ;
//  	notifyObservers();
//  }

//  return "meo";
// });
