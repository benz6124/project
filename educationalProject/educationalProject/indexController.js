﻿'use strict';

app.controller('choice_index_controller', function($scope, $http) {


    $scope.curri_me = "";
     $scope.curri_choosen={};
    $http.get("/api/curriculum").success(function (data, status, headers, config) {

        $scope.all_curriculums = data;

    });


    $scope.sendCurriAndGetYears = function () {

        console.log($scope.curri_choosen);
        //    $http.post('/api/curriculumacademic',  {'Cu_curriculum': curri }).success(function (data, status, headers, config) {
        //     $scope.corresponding_aca_years = data;
        // });
      
        // $http.post(
        //      '/api/curriculumacademic',
        //      JSON.stringify(curri),
        //      {
        //          headers: {
        //              'Content-Type': 'application/json'
        //          }
        //      }
        //  ).success(function (data) {
        //      $scope.corresponding_aca_years = data;
        //  });
    }




});