'use strict';
app.controller('indexController', function ($scope, $http) {
	$scope.curri_choosen = "aa";


    $http.get("/api/curriculum").success(function (data, status, headers, config) {
        $scope.all_curriculums = data;

    });

    $http.get("/api/curriculumAcademic").success(function (data, status, headers, config) {
        $scope.aca_yrs = data;

    });
});