'use strict';

app.controller('indexController', function ($scope, $http) {
    $scope.curri_choosen = "";

    $scope.curri_me = "";
        
    $http.get("/api/curriculum").success(function (data, status, headers, config) {

        $scope.all_curriculums = data;

    });


     
    $scope.sendCurriAndGetYears =  function (curri){


        //    $http.post('/api/curriculumacademic',  {'Cu_curriculum': curri }).success(function (data, status, headers, config) {
        //     $scope.corresponding_aca_years = data;
        // });
        alert(curri);
        $http.post(
             '/api/curriculumacademic',
             JSON.stringify(curri),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
             $scope.corresponding_aca_years = data;
         });
    }





});
