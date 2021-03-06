
var app = angular.module('myProject', [
	
        'ngSanitize',
        'ngAnimate',
        'ngQuantum',
        // ,'ngRoute'
        'ui.bootstrap'
        ,'bootstrapLightbox'
        ,'flow'
        ,'ngCookies'

]);


//         window.loading_screen = pleaseWait({


//   backgroundColor: '#6F002B',
//   loadingHtml: " <div style='color:white' class='sk-chasing-dots'><div class='sk-child sk-dot1'></div><div class='sk-child sk-dot2'></div></div><p style='font-size:30px;color:#E0E0E0;font-family:FreesiaUPC' class='loading-message'>กำลังนำท่านเข้าสู่ระบบ...</p>"
// });


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

app.controller('main_controller', function ($scope,AuthService,$cookies,$rootScope,$http,$alert,$modal) {
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
           if(!!$rootScope.current_user.not_send_primary){
              $scope.open_modal_primary_not_send();
         }
          $scope.already_login = true;
         });
  }

    $rootScope.open_modal_primary_not_send = function () {
       var i;
       $scope.content_pop = "<b>ท่านมีหลักฐานค้างการอัพโหลด ในหลักสูตร ปีการศึกษาเหล่านี้</b> <ul>";
              for(i=0;i<$rootScope.current_user.not_send_primary.length;i++){
                $scope.content_pop =  $scope.content_pop+"<li>"+$rootScope.current_user.not_send_primary[i].evidence_name+" <b>หลักสูตร</b> "+$rootScope.current_user.not_send_primary[i].curr_tname+" <b>ปีการศึกษา</b> "+$rootScope.current_user.not_send_primary[i].aca_year+"</li>";
               
              }
               $scope.content_pop  =  $scope.content_pop  +"</ul>"
                var modalOptions = {
                title:  'แจ้งเตือน',
                content: $scope.content_pop,
                theme: 'danger',
                size: 'lg',
                effect: 'bounce-in',
                speed: 'fast',
                backdrop:'static',
               typeClass: 'just_alert',
               closeText :'ปิดหน้าต่าง'              
            }
            $modal(modalOptions);
        }
    
    $scope.logout = function(){
        console.log("log out")
        $cookies.remove('mymy');
         $rootScope.current_user ={};
         $rootScope.have_privilege_in_these_curri = {};
        $scope.already_login = false;
        $rootScope.clear_choosen();
        $rootScope.current_curriculum = "";
        $rootScope.current_aca_year = "";
        $rootScope.current_indicator = "";
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
     for(index =0;index<$rootScope.current_user.president_in[curri_id].length;index++){     
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
            obj_privilege_curri.push($rootScope.all_curriculums[index]);break;
        }
    }
}
  }

    $scope.have_privilege_to_open_modal = function(title_code){
if( $scope.already_login == false){
  return false;
}

   var ever_been_committee = !!$rootScope.current_user.committee_in;
   var ever_been_president = !!$rootScope.current_user.president_in;
 
  if(title_code=='18' || title_code=='16' || title_code=='21' || title_code=='22'|| title_code=='23'|| title_code=='25'|| title_code=='27' || title_code=='29' || title_code=='28' || title_code=='30'){
    if($rootScope.current_user.user_type == 'ผู้ดูแลระบบ'){
      return true;
    }
  }

  if(title_code=='18' || title_code=='19' || title_code=='20'|| title_code=='24'|| title_code=='26'){
    if(angular.isUndefined($rootScope.current_user.president_in)==true){
      return false;
    }
    else{          
        return true;
    }
  }

  if(title_code=='17'){
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

      var index;
     $rootScope.have_privilege_in_these_curri[title_code] = [];

//Handle privilege
      if(title_code=='2' || title_code=='14' || title_code=='1' || title_code=='4' || title_code=='7' || title_code=='28' || title_code=='30'  || title_code=='32' ||  /*privilege 2 option*/
      title_code=='3' || title_code=='5' || title_code=='6'|| title_code=='8'|| title_code=='9'|| title_code=='10' || title_code=='11'|| title_code=='12'|| title_code=='13') /*privilege 3-5 option*/{
        for(index=0;index<$rootScope.current_user.curri_id_in.length;index++){
          //If normal privilege in that curri is more than 1 => Add curri_id to the $rootScope.have_privilege_in_these_curri[title_code]
          if($rootScope.current_user.privilege[$rootScope.current_user.curri_id_in[index]][title_code] >1){
            $rootScope.have_privilege_in_these_curri[title_code].push($rootScope.current_user.curri_id_in[index]);continue;
          }
//If user is committee in any curriculum ,the priviledge to open modal will overridden
           if(ever_been_committee == true){
            if(!!$rootScope.current_user.committee_in[$rootScope.current_user.curri_id_in[index]]){
             if($rootScope.current_user.committee_privilege[$rootScope.current_user.curri_id_in[index]][title_code] >1){
                 $rootScope.have_privilege_in_these_curri[title_code].push($rootScope.current_user.curri_id_in[index]);continue;
              }
            }
          }
//If user is president in any curriculum ,the priviledge to open modal will overridden
          if (ever_been_president == true){
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

  $rootScope.is_admin = function(){
    if($rootScope.current_user.user_type == 'ผู้ดูแลระบบ'){
      return true;
    }
    else{
      return false;
    }
  }

  $rootScope.is_president = function(){
    if(angular.isUndefined($rootScope.current_user.president_in)==true){
        return false;
    }
    return true;
  }

  $rootScope.have_privilege = function(title_code,privilege_level,curri_id,aca_year){
    if (angular.isUndefined($rootScope.current_user.privilege)) {
      return false;
    }
    if(angular.isUndefined(curri_id) || angular.isUndefined(aca_year)){ /*prevent case curriculum or aca_year is not selected.*/
      return false;
    }
    if($rootScope.current_user.privilege[curri_id][title_code] >= privilege_level){
      return true;
    }
    
    if(!aca_year){ //Check don't care aca_year
      if($rootScope.president_in_this_curri(curri_id)){
        return true;
      }
      if($rootScope.right_from_committee_just_curri(curri_id,title_code,privilege_level)){
        return true;
      }
    }
    else{
      if($rootScope.president_in_this_curri_and_year(curri_id,aca_year)){
        return true;
      }
      if($rootScope.right_from_committee(curri_id,aca_year,title_code,privilege_level)){
        return true;
      }
    }
    return false;
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