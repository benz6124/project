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



 app.config(function($stateProvider){

$stateProvider.state('protected-route', {
  url: '/',
  resolve: {

    auth: function resolveAuthentication(AuthResolver) { 

      return AuthResolver.resolve();
    }
  }
});
	
});



app.constant('AUTH_EVENTS', {
  loginSuccess: 'auth-login-success',
  loginFailed: 'auth-login-failed',
  logoutSuccess: 'auth-logout-success',
  sessionTimeout: 'auth-session-timeout',
  notAuthenticated: 'auth-not-authenticated',
  notAuthorized: 'auth-not-authorized'
})

app.constant('USER_ROLES', {
  all: '*',
  admin: 'admin',
  editor: 'editor',
  guest: 'guest'
})

app.factory('AuthService', function ($http, Session,$cookies) {
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

// return {"user_id":"admin1","username":"admin1","user_type":"ผู้ดูแลระบบ","information":{"t_prename":"","t_name":"ชื่อจริง นามสกุลจริง","e_prename":"","e_name":"","citizen_id":"","gender":" ","email":"b@hotmail.com","tel":"","addr":"","file_name_pic":"myimages/profile_pic/nopic.jpg","timestamp":"7/1/2559 0:05:15"},"privilege":[]};




//  $cookies.putObject("mymy", credentials);
   	

// return {
// 'username': credentials.username,
// 'user_type':'admin',
// 'information':{'t_name':'fafa', 'email':'blahblah'},
// 'privilege':{'21':{'สร้างหลักสูตร':'อนุญาต' , 'อัลบั้ม':'ดูเท่านั้น'} }
// }



// return $http
//       .post('/api/users/login', credentials)
//       .then(function (res) {
//         console.log('')
//         console.log(res.data)
//         // Session.create(res.data.id, res.data.user.id,
//         //                res.data.user.role);
//         // // $cookies.putObject("mymy", credentials);
//         // return res.data.user;
//       });



  };
 
  authService.isAuthenticated = function () {
    return !!Session.user_id;
  };
 

 
  return authService;
});

app.service('Session', function () {
  this.create = function (sessionId, userId) {
    this.id = sessionId;
    this.userId = userId;

  };
  this.destroy = function () {
    this.id = null;
    this.userId = null;

  };
})

app.controller('main_controller', function ($scope,
                                               USER_ROLES,
                                               AuthService,$cookies) {

   $scope.current_user = {};
  $scope.current_user = $cookies.getObject("mymy");
$scope.fix_mode = true;


  if(!$scope.current_user) {
  $scope.already_login = false;

  }
  else{
  	console.log('$scope.already_login = false;')
  	  $scope.already_login = true;
  	    	console.log('$scope.already_login = true;')
  	
  }
    $scope.logout = function(){
        console.log("log out")
        $cookies.remove('mymy');
         $scope.current_user ={};
          $scope.already_login = false;
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


    $scope.watch_in_curri = function(){
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



  


app.factory('AuthResolver', function ($q, $rootScope, $state) {
  return {
    resolve: function () {
    	console.log('im resolve')
      var deferred = $q.defer();
      var unwatch = $rootScope.$watch('current_user', function (current_user) {
        if (angular.isDefined(current_user)) {
          if (current_user) {
            deferred.resolve(current_user);
          } else {
            deferred.reject();
            $state.go('user-login');
          }
          unwatch();
        }
      });
      return deferred.promise;
    }
  };
})