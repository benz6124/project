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
                                             
                                               AuthService,$cookies,$rootScope) {

   $scope.current_user = {};
  $scope.current_user = $cookies.getObject("mymy");
$scope.fix_mode = false;
$scope.not_choose_curri_and_year_yet = true;
     $rootScope.have_privilege_in_these_curri = {};
  if(!$scope.current_user) {
  $scope.already_login = false;

  }
  else{

    
  	console.log('$scope.current_user')
    console.log($scope.current_user)
  	  $scope.already_login = true;
  	    
  	
  }
    $scope.logout = function(){
        console.log("log out")
        $cookies.remove('mymy');
         $scope.current_user ={};
          $scope.already_login = false;
          $rootScope.clear_choosen();
    }

  $scope.setcurrent_user = function (user) {
      $scope.already_login = true;
    $scope.current_user = user;
     $cookies.putObject("mymy", user);
      	console.log($scope.current_user);
  };


  $scope.watch_only_admin = function(){

    if($scope.current_user.user_type == 'ผู้ดูแลระบบ'){
      return true;
    }
    else{
      return false;
    }
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
  if(title_code=='16' || title_code=='21' || title_code=='22'|| title_code=='23'|| title_code=='25'|| title_code=='27'){
    if($scope.current_user.user_type == 'ผู้ดูแลระบบ'){
      return true;
    }
    return false;
  }

  if(title_code=='18' || title_code=='19' || title_code=='20'|| title_code=='24'|| title_code=='26'){
    if($scope.current_user.user_type == 'ประธานหลักสูตร'){
      return true;
    }
    return false;
  }

  if(title_code=='17' ){
    if($scope.current_user.user_type == 'ผู้ประเมินจากภายนอก'){
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
        for(index=0;index<$scope.current_user.curri_id_in.length;index++){
          if($scope.current_user.privilege[$scope.current_user.curri_id_in[index]][title_code] ==2){
           
            $rootScope.have_privilege_in_these_curri[title_code].push($scope.current_user.curri_id_in[index]);
          
          }

        }
      }

      else if(title_code=='3' || title_code=='5' || title_code=='6'|| title_code=='8'|| title_code=='9'|| title_code=='10' || title_code=='11'|| title_code=='12'|| title_code=='13'){
       
        for(index=0;index<$scope.current_user.curri_id_in.length;index++){
          if($scope.current_user.privilege[$scope.current_user.curri_id_in[index]][title_code] >=2){
           
            $rootScope.have_privilege_in_these_curri[title_code].push($scope.current_user.curri_id_in[index]);
          
          }

        }
      }

      if($rootScope.have_privilege_in_these_curri[title_code].length != 0){
      
        return true;
      }

      return false;
    }

    $scope.watch_only_president = function(){
      if($scope.current_user.user_type == 'ประธานหลักสูตร'){
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