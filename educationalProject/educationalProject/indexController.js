'use strict';

app.controller('choice_index_controller', function($scope, $http) {


    $scope.year_choosen = {};
     $scope.curri_choosen={};
     $scope.indicator_choosen = {};
    $http.get("/api/curriculum").success(function (data, status, headers, config) {

        $scope.all_curriculums = data;

    });


    $scope.sendCurriAndGetYears = function (curri) {

        console.log(curri);
        //    $http.post('/api/curriculumacademic',  {'Cu_curriculum': curri }).success(function (data, status, headers, config) {
        //     $scope.corresponding_aca_years = data;
        // });
      
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
    $scope.loadingIndexPage = function(){
        $event = sendYearAndGetIndicators($scope.year_choosen);
    }
     $scope.sendYearAndGetIndicators = function (year) {
 
        console.log(year);
        console.log(year.aca_year);
        //    $http.post('/api/curriculumacademic',  {'Cu_curriculum': curri }).success(function (data, status, headers, config) {
        //     $scope.corresponding_aca_years = data;
        // });
  // console.log(" ");
        // year = new Object();
        // year.curri_id = "20";
        // year.aca_year = 2554;
          // console.log(year);
        $http.post(
             '/api/indicator',
             JSON.stringify(year),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
             $scope.corresponding_indicators = data;
         });
    }

     $scope.sendIndicatorAndGetSubIndicators = function (indicator) {
          $scope.indicator_choosen = indicator;
        console.log("sendIndicatorAndGetSubIndicators")
        console.log(indicator);

        //    $http.post('/api/curriculumacademic',  {'Cu_curriculum': curri }).success(function (data, status, headers, config) {
        //     $scope.corresponding_aca_years = data;
        // });
  // console.log(" ");
        // year = new Object();
        // year.curri_id = "20";
        // year.aca_year = 2554;
          // console.log(year);
        $http.post(
             '/api/subindicator',
             JSON.stringify(indicator),
             {
                 headers: {
                     'Content-Type': 'application/json'
                 }
             }
         ).success(function (data) {
             $scope.corresponding_sub_indicators = data;
         });
    }

});