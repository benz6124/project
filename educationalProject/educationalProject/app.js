var app = angular.module('myProject', [
	
        'ngSanitize',
        'ngAnimate',
        'ngQuantum',
        'ngDialog'
        // ,'ngRoute'
        ,'ui.bootstrap'
        ,'bootstrapLightbox'
        ,'flow'
        ,'ui.router'
        ,'ngCookies'
       
   
]);



//  app.config(function($stateProvider){

// $stateProvider.state('protected-route', {
//   url: '/',
//   resolve: {

//     auth: function resolveAuthentication(AuthResolver) { 

//       return AuthResolver.resolve();
//     }
//   }
// });
	
// });



app.constant('AUTH_EVENTS', {
  loginSuccess: 'auth-login-success',
  loginFailed: 'auth-login-failed',
  // logoutSuccess: 'auth-logout-success',
  // sessionTimeout: 'auth-session-timeout',
  // notAuthenticated: 'auth-not-authenticated',
  // notAuthorized: 'auth-not-authorized'
})



app.factory('AuthService', function ($http,$cookies) {
  var authService = {};

  authService.login = function (credentials) {
    return $http
      .post('/api/users/login', credentials)
      .then(function (res) {
        // Session.create(res.data.user_id, res.data.user.username);
      console.log('login leaw')
      console.log(res.data);
        return res.data;
      });

  };
 
  // authService.isAuthenticated = function () {
  //   return !!Session.user_id;
  // };
 

 
  return authService;
});

// app.service('Session', function () {
//   this.create = function (sessionId, userId) {
//     this.id = sessionId;
//     this.userId = userId;

//   };
//   this.destroy = function () {
//     this.id = null;
//     this.userId = null;

//   };
// })

app.controller('main_controller', function ($scope,
                                             
                                               AuthService,$cookies,$rootScope,$http) {

   $rootScope.current_user = {};
  $scope.cookies_user_id = $cookies.getObject("mymy");

  console.log('$scope.cookies_user_id',$scope.cookies_user_id)
$scope.fix_mode = false;
$scope.not_choose_curri_and_year_yet = true;
     $rootScope.have_privilege_in_these_curri = {};
  if(!$scope.cookies_user_id) {
  $scope.already_login = false;

  }
  else{


        $http.post(
             'api/users/getuserdata',
             JSON.stringify($scope.cookies_user_id),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
          $rootScope.current_user =data;
          console.log('$rootScope.current_user')
          console.log($rootScope.current_user)
          $scope.already_login = true;
    
         });
    

  	    
  	
  }
    $scope.logout = function(){
        console.log("log out")
        $cookies.remove('mymy');
         $rootScope.current_user ={};
          $scope.already_login = false;
          $rootScope.clear_choosen();
    }

  $scope.setcurrent_user = function (user) {
      $scope.already_login = true;
    $rootScope.current_user = user;
     $cookies.putObject("mymy", user.user_id);
     
  };


  $scope.watch_only_admin = function(){

    if($rootScope.current_user.user_type == 'ผู้ดูแลระบบ'){
      return true;
    }
    else{
      return false;
    }
  }


$rootScope.curri_that_be_president_in = function(obj){
  if(!$rootScope.current_user){
      return false;
    }

     if(angular.isUndefined($rootScope.current_user.president_in)==true){
         
        return false;
    }

    var index;

    for(index =0;index<$rootScope.all_curriculums.length;index++){
      if(!!$rootScope.current_user.president_in[$rootScope.all_curriculums[index].curri_id]){
        obj.push($rootScope.all_curriculums[index]);
      }
    }
}

$rootScope.president_in_this_curri = function(curri_id){

    if(!$rootScope.current_user){
      return false;
    }
    if(angular.isUndefined($rootScope.current_user.president_in)==true){
         
        return false;
    }
     if(angular.isUndefined($rootScope.current_user.president_in[curri_id])==true){
        
        return false;
    }

     // var index;
     // for(index =0;index<$rootScope.current_user.president_in[curri_id].length;index++ ){
     
     //    if($rootScope.current_user.president_in[curri_id][index] == aca_year){
     //        return true;
     //    }
     // }
     return true;
}

$rootScope.right_from_committee_just_curri = function(curri_id,topic_num,from){

    if(!$rootScope.current_user){
      return false;
    }
    if(angular.isUndefined($rootScope.current_user.committee_in)==true){
         
        return false;
    }
     if(angular.isUndefined($rootScope.current_user.committee_in[curri_id])==true){
        
        return false;
    }

    if($rootScope.current_user.committee_privilege[curri_id][topic_num] >= from){
      return true;
    }

    return false;
}


$rootScope.president_in_this_curri_and_year = function(curri_id,aca_year){

    if(!$rootScope.current_user){
      return false;
    }
    if(angular.isUndefined($rootScope.current_user.president_in)==true){
         
        return false;
    }
     if(angular.isUndefined($rootScope.current_user.president_in[curri_id])==true){
        
        return false;
    }

     var index;
     for(index =0;index<$rootScope.current_user.president_in[curri_id].length;index++ ){
     
        if($rootScope.current_user.president_in[curri_id][index] == aca_year){
            return true;
        }
     }
     return false;
}

$rootScope.right_from_committee = function(curri_id,aca_year,topic_num,from){

    if(!$rootScope.current_user){
      return false;
    }
    if(angular.isUndefined($rootScope.current_user.committee_in)==true){
         
        return false;
    }
     if(angular.isUndefined($rootScope.current_user.committee_in[curri_id])==true){
        
        return false;
    }

     var index;
     for(index =0;index<$rootScope.current_user.committee_in[curri_id].length;index++ ){
     
        if($rootScope.current_user.committee_in[curri_id][index] == aca_year){
            if($rootScope.current_user.committee_privilege[curri_id][topic_num] >= from){
              return true;
            }
            else{
              return false;
            }
            
        }
     }
     return false;
}

  $scope.scan_only_privilege_curri = function(title_code,obj_privilege_curri){

    var index;
    var index2;

for(index=0;index<$rootScope.all_curriculums.length;index++){
    for(index2 = 0;index2<$rootScope.have_privilege_in_these_curri[title_code].length ;index2++){
      
        if($rootScope.all_curriculums[index].curri_id ==$rootScope.have_privilege_in_these_curri[title_code][index2] ){
            obj_privilege_curri.push($rootScope.all_curriculums[index]);
        }
    }
}
  }

    $scope.have_privilege_to_open_modal = function(title_code){

          var ever_been_committee = !!$rootScope.current_user.committee_in;
   var ever_been_president = !!$rootScope.current_user.president_in;
 
  if(title_code=='16' || title_code=='21' || title_code=='22'|| title_code=='23'|| title_code=='25'|| title_code=='27'){
    if($rootScope.current_user.user_type == 'ผู้ดูแลระบบ'){
      return true;
    }
    return false;
  }

  if(title_code=='18' || title_code=='19' || title_code=='20'|| title_code=='24'|| title_code=='26'){


    if(angular.isUndefined($rootScope.current_user.president_in)==true){
      
      return false;
    }
    else{
          
        return true;
    }

  }

  if(title_code=='17' ){
    if($rootScope.current_user.user_type == 'ผู้ประเมินจากภายนอก'){
      return true;
    }
    return false;
  }


      if($rootScope.have_privilege_in_these_curri[title_code]){
         if($rootScope.have_privilege_in_these_curri[title_code].length != 0){
       return true;
    }
      }
   
      var should_see = false;
      var index;
      var index2;
var first = true;
     $rootScope.have_privilege_in_these_curri[title_code] = [];


      if(title_code=='2' || title_code=='14' || title_code=='1' || title_code=='4' || title_code=='7'){
        for(index=0;index<$rootScope.current_user.curri_id_in.length;index++){
          if($rootScope.current_user.privilege[$rootScope.current_user.curri_id_in[index]][title_code] ==2){
           
            $rootScope.have_privilege_in_these_curri[title_code].push($rootScope.current_user.curri_id_in[index]);
            
          }

          else if (ever_been_committee == true){
            if(!!$rootScope.current_user.committee_in[$rootScope.current_user.curri_id_in[index]]){

             if($rootScope.current_user.committee_privilege[$rootScope.current_user.curri_id_in[index]][title_code] ==2){
          
                $rootScope.have_privilege_in_these_curri[title_code].push($rootScope.current_user.curri_id_in[index]);
            
              }
            }
          }

               else if (ever_been_president == true){
            if(!!$rootScope.current_user.president_in[$rootScope.current_user.curri_id_in[index]]){

           
                $rootScope.have_privilege_in_these_curri[title_code].push($rootScope.current_user.curri_id_in[index]);
            
            
            }
          }

        }
      }

      else if(title_code=='3' || title_code=='5' || title_code=='6'|| title_code=='8'|| title_code=='9'|| title_code=='10' || title_code=='11'|| title_code=='12'|| title_code=='13'){
       
        for(index=0;index<$rootScope.current_user.curri_id_in.length;index++){
          if($rootScope.current_user.privilege[$rootScope.current_user.curri_id_in[index]][title_code] >=2){
           
            $rootScope.have_privilege_in_these_curri[title_code].push($rootScope.current_user.curri_id_in[index]);
          
          }
      else if (ever_been_committee == true){
            if(!!$rootScope.current_user.committee_in[$rootScope.current_user.curri_id_in[index]]){

             if($rootScope.current_user.committee_privilege[$rootScope.current_user.curri_id_in[index]][title_code] >=2){
           
                $rootScope.have_privilege_in_these_curri[title_code].push($rootScope.current_user.curri_id_in[index]);
            
              }
            }
          }

              else if (ever_been_president == true){
            if(!!$rootScope.current_user.president_in[$rootScope.current_user.curri_id_in[index]]){

           
                $rootScope.have_privilege_in_these_curri[title_code].push($rootScope.current_user.curri_id_in[index]);
            
            
            }
          }
        }
      }

      if($rootScope.have_privilege_in_these_curri[title_code].length != 0){
      
        return true;
      }

      return false;
    }

    $scope.watch_only_president = function(){
      if($rootScope.current_user.user_type == 'ประธานหลักสูตร'){
      return true;
    }
    else{
      return false;
    }
  }
});



  


// app.factory('AuthResolver', function ($q, $rootScope, $state) {
//   return {
//     resolve: function () {
//     	console.log('im resolve')
//       var deferred = $q.defer();
//       var unwatch = $rootScope.$watch('current_user', function (current_user) {
//         if (angular.isDefined(current_user)) {
//           if (current_user) {
//             deferred.resolve(current_user);
//           } else {
//             deferred.reject();
//             $state.go('user-login');
//           }
//           unwatch();
//         }
//       });
//       return deferred.promise;
//     }
//   };
// })