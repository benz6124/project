'use strict';

app.controller('choice_index_controller', function($scope, $http) {

    // 
     console.log("it's me na");
    // console.log(select_nothing);

     $scope.init_var = function(){
    $scope.not_select_curri_and_year = true;
    $scope.not_select_sub_indicator = true;
    $scope.year_choosen = {};
     $scope.curri_choosen={};
     $scope.indicator_choosen = {};
     $scope.sub_indicator_choosen = {};
     $scope.select_overall = true;
     }

    $http.get("/api/curriculum").success(function (data, status, headers, config) {

        $scope.all_curriculums = data;

    });


    $scope.sendCurriAndGetYears = function (curri) {

        console.log("it's me");
      
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
        $event = $scope.sendYearAndGetIndicators($scope.year_choosen);
    }
     $scope.sendYearAndGetIndicators = function (year) {
   $scope.not_select_curri_and_year = false;

    $scope.not_select_sub_indicator = true;
        console.log(year);
        console.log(year.aca_year);

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
    $scope.send_sub_indicator = function(sub_indicator){
        $scope.sub_indicator_choosen = sub_indicator;
        $scope.not_select_sub_indicator = false;
    }
     $scope.sendIndicatorAndGetSubIndicators = function (indicator) {
        $scope.not_select_sub_indicator = true;
          $scope.indicator_choosen = indicator;
          $scope.select_overall = false;
        console.log("sendIndicatorAndGetSubIndicators")
        console.log(indicator);

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