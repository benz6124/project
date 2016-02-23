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


app.factory('request_years_from_curri_choosen_service', function($rootScope,$http) {


      var my_years = {
     async: function(curri,title_code,to_see_from) {
   
        var promise = $http.post(
             '/api/curriculumacademic/getbycurriculum',
             JSON.stringify(curri),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).then(function (response) {
          
     

          if($rootScope.current_user.privilege[curri.curri_id][title_code] < to_see_from){
       var valid_year = [];
       var i;
   var i2;
            if(!!$rootScope.current_user.president_in){
              if(!!$rootScope.current_user.president_in[curri.curri_id]){
                
                      for(i2 =0;i2<response.data.length;i2++){
                  for(i =0;i<$rootScope.current_user.president_in[curri.curri_id].length;i++){
                  
                      if(response.data[i2].aca_year == $rootScope.current_user.president_in[curri.curri_id][i]){
                       valid_year.push(response.data[i2]);
                       break;
                      }
                    }
                   
                  }
              }
            }

            if(!!$rootScope.current_user.committee_in){
              if(!!$rootScope.current_user.committee_in[curri.curri_id]){
                if($rootScope.current_user.committee_privilege[curri.curri_id][title_code]>= to_see_from ){
                    for(i2 =0;i2<response.data.length;i2++){
                  for(i =0;i<$rootScope.current_user.committee_in[curri.curri_id].length;i++){
                  
                      if(response.data[i2].aca_year == $rootScope.current_user.committee_in[curri.curri_id][i]){
                       valid_year.push(response.data[i2]);
                       break;
                      }
                    }
                   
                  }
                }
              
              }
            }
            console.log('caz dada');
            console.log(valid_year);
               return valid_year;
               console.log('heyy')
          }
          
          else{
             console.log('normal dada');
        return response.data;
          }
      

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
