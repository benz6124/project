app.factory('all_curriculums_service', function($http) {
var observerCallbacks = [];

      var my_curriculums = {
	   async: function() {
	 
	      var promise = $http.get('/api/curriculum').then(function (response) {
	       
	        console.log(response);
	       
	        return response.data;
	      });
	  
	      return promise;
	    }
	  };
return my_curriculums;


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
