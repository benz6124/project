'use strict';
app.controller('indexController', function ($scope, $http) {

    $http.get("/api/curriculum").success(function (data, status, headers, config) {
        $scope.curriculums = data;

    });

    $http.get("/api/curriculumAcademic").success(function (data, status, headers, config) {
        $scope.aca_yrs = data;

    });
});