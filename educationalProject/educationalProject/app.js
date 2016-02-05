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

app.factory('AuthService', function ($http, Session) {
  var authService = {};
 console.log('in authserv')
  authService.login = function (credentials) {
    // return $http
    //   .post('/login', credentials)
    //   .then(function (res) {
    //     Session.create(res.data.id, res.data.user.id,
    //                    res.data.user.role);
    //     return res.data.user;
    //   });

return {
'username': 'meme',
'user_type':'admin',
'information':{'t_name':'fafa', 'email':'blahblah'},
'privilege':{'21':{'สร้างหลักสูตร':'อนุญาต' , 'อัลบั้ม':'ดูเท่านั้น'} }
}
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
                                               AuthService) {
  $scope.currentUser = null;
  $scope.userRoles = USER_ROLES;
  $scope.isAuthorized = AuthService.isAuthorized;
 
  $scope.setCurrentUser = function (user) {

    $scope.currentUser = user;
      	console.log($scope.currentUser);
  };
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