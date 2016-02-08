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
    // return $http
    //   .post('/login', credentials)
    //   .then(function (res) {
    //     Session.create(res.data.id, res.data.user.id,
    //                    res.data.user.role);
    //     return res.data.user;
    //   });


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
return {

  "user_id":"00001","username":"wiboon","user_type":"อาจารย์","information":{"t_prename":"นาย","t_name":"วิบูลย์ พร้อมพานิชย์","e_prename":"Mr.","e_name":"Wiboon Prompanich","citizen_id":"1234567890123","gender":"M","email":"kpwiboon@kmitl.ac.th","tel":"0818685657","addr":"กรุงเทพ","file_name_pic":"myimages/profile_pic/วิบูลย์.jpg","timestamp":"8/9/2558 23:50:01"},
"privilege":[{"curri_id":"20","privilege_list":[{
"title_code":1,
"title_privilege_code":2,
"name":"สร้างบัญชีรายชื่อ",
"privilege":"อนุญาต"},
{
"title_code":2,
"title_privilege_code":1,
"name":"เพิ่มปีการศึกษาในแต่ละหลักสูตร",
"privilege":"ไม่อนุญาต"}]},
{"curri_id":"21","privilege_list":[{
"title_code":1,
"title_privilege_code":1,
"name":"สร้างบัญชีรายชื่อ",
"privilege":"ไม่อนุญาต"},
{
"title_code":2,
"title_privilege_code":2,
"name":"เพิ่มปีการศึกษาในแต่ละหลักสูตร",
"privilege":"อนุญาต"}]}]}



  };
 
  authService.isAuthenticated = function () {
    return !!Session.userId;
  };
 
  authService.isAuthorized = function (authorizedRoles) {
    if (!angular.isArray(authorizedRoles)) {
      authorizedRoles = [authorizedRoles];
    }
    return (authService.isAuthenticated() &&
      authorizedRoles.indexOf(Session.userRole) !== -1);
  };
 
  return authService;
});

app.service('Session', function () {
  this.create = function (sessionId, userId, userRole) {
    this.id = sessionId;
    this.userId = userId;
    this.userRole = userRole;
  };
  this.destroy = function () {
    this.id = null;
    this.userId = null;
    this.userRole = null;
  };
})

app.controller('main_controller', function ($scope,
                                               USER_ROLES,
                                               AuthService,$cookies) {


  $scope.currentUser = $cookies.getObject("mymy");
  $scope.userRoles = USER_ROLES;
  $scope.isAuthorized = AuthService.isAuthorized;
  if(!$scope.currentUser){
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
         $scope.currentUser ={};
          $scope.already_login = false;
    }

  $scope.setCurrentUser = function (user) {

    $scope.currentUser = user;
     $cookies.putObject("mymy", user);
      	console.log($scope.currentUser);
  };


  $scope.can_watch_this_modal = function(){

  }
});





app.factory('AuthResolver', function ($q, $rootScope, $state) {
  return {
    resolve: function () {
    	console.log('im resolve')
      var deferred = $q.defer();
      var unwatch = $rootScope.$watch('currentUser', function (currentUser) {
        if (angular.isDefined(currentUser)) {
          if (currentUser) {
            deferred.resolve(currentUser);
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