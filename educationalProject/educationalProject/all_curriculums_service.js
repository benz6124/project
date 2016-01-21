app.factory('request_all_curriculums_service_server', function($http,$rootScope) {


      var my_curriculums = {
	   async: function() {
	 
	      var promise = $http.get('/api/curriculum').then(function (response) {
	       	
	
	        $rootScope.all_curriculums = response.data;
	      
	        return response.data;
	      });
	  
	      return promise;
	    }
	  };
return my_curriculums;


});


app.factory('request_years_from_curri_choosen_service', function($http) {


      var my_years = {
     async: function(curri) {
   
        var promise = $http.post(
             '/api/curriculumacademic/getbycurriculum',
             JSON.stringify(curri),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).then(function (response) {
          
      
          // $rootScope.all_curriculums = response.data;
        
          return response.data;
        });
    
        return promise;
      }
    };
return my_years;


});



// app.factory('current_all_curriculums_service', function ($rootScope) {
//     'use strict';

//     var my_current_all_curriculums = [];

//     var broadcast = function (new_all_curri) {
//     console.log("new_all_curri_broadcast");
//     console.log(new_all_curri);
//       $rootScope.$broadcast('current_all_curriculums_service.update', new_all_curri);
//       console.log("broadcast");
//     };

//     var update = function (new_all_curri) {
//       my_current_all_curriculums = new_all_curri;
//      console.log("new_all_curri_update");
//     console.log(new_all_curri);

//          console.log("my_current_all_curriculums");
//     console.log(my_current_all_curriculums);

//       $rootScope.$broadcast('current_all_curriculums_service.update', my_current_all_curriculums);
//     };
    
//     return {
//       update: update,
//       current_all_curriculums: my_current_all_curriculums,
//     };
//    });

// // app.factory('update_curriculums_service', function($http) {

// // 	 var observerCallbacks = [];
// // 	 var current_all_curriculums;


// //  var notifyObservers = function(){
// //     angular.forEach(observerCallbacks, function(callback){
// //       callback();
// //     });
// //   }
// // 	// this.register_observers = function(callback){
// //  //    observerCallbacks.push(callback);
// //  //  };
// //  var update_all_curriculums = function(new_all_curriculums){
// //  	current_all_curriculums = new_all_curriculums ;
// //  	notifyObservers();
// //  }

// //  return "meo";
// // });
