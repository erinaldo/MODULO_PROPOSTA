﻿angular.module('App').controller('GradeMerchaController', ['$scope', '$rootScope', 'httpService', '$location', '$timeout', function ($scope, $rootScope, httpService, $location, $timeout) {

    //========================Verifica Permissoes
    $scope.PermissaoNew = false;
    $scope.PermissaoEdit = false;
    $scope.PermissaoDelete = false;
    $scope.PermissaoActivate = false;
    $scope.PermissaoReplicar = false;
    $scope.chkMostrarDispo = true;
    httpService.Get("credential/GradeMercha@New").then(function (response) {
        $scope.PermissaoNew = response.data;
    });
    httpService.Get("credential/GradeMercha@Edit").then(function (response) {
        $scope.PermissaoEdit = response.data;
    });
    httpService.Get("credential/GradeMercha@Destroy").then(function (response) {
        $scope.PermissaoDelete = response.data;
    });
    httpService.Get("credential/GradeMercha@Activate").then(function (response) {
        $scope.PermissaoActivate = response.data;
    });
    httpService.Get("credential/GradeMercha@Replicar").then(function (response) {
        $scope.PermissaoReplicar = response.data;
    });

    //===================Declarar scopes
    $scope.CompetenciaKeys = { 'Year': new Date().getFullYear(), 'First': '', 'Last': '' }
    $scope.ShowGrid = false;
    $scope.ShowFilter = true;
    $scope.ShowAviso = false
    $scope.Filtro = "";
    $scope.NewFilter = function (fromButtom) {
        $scope.Grades = [];
        $scope.ShowFilter = true;
        $scope.ShowGrid = false;
        $scope.ShowAviso = false;
        $scope.Filtro = { 'Cod_Veiculo': '', 'Competencia': '', 'Nome_Veiculo': '', 'Cod_Programa': '' };
        if (fromButtom) {
            delete localStorage['GradeFilter'];
        }
        
    };
    $scope.NewFilter();
    //===================Carregar o grid
    $scope.CarregarGrade = function (pFiltro) {
        $rootScope.routeloading = true;
        $scope.Grades = [];
        $scope.ShowGrid = '';
        $scope.ShowAviso = false;
        var _url = "GradeMercha/List";
        _url += "?Cod_Veiculo=" + pFiltro.Cod_Veiculo;
        _url += "&Competencia=" + pFiltro.Competencia;
        _url += "&Cod_Programa=" + pFiltro.Cod_Programa;
        _url += "&";
        httpService.Get(_url).then(function (response) {
            if (response.data.Dias.length > 0) {
                $scope.Grades = response.data;
                $scope.ShowGrid = true;
                $scope.ShowFilter = false;
                localStorage.setItem('GradeMerchaFilter', JSON.stringify(pFiltro));
            }
            else {
                $scope.ShowAviso = true 
            }
        });
    };
    //============================Edit Grade 
    $scope.GradeCadastro = function (pAction, pVeiculo, pData, pPrograma) {
        $location.path("/GradeMerchaCadastro/" + pAction + "/" + pVeiculo + "/" + pData + "/" + pPrograma)
    };
    //===========================fim do load da pagina
    $scope.$watch('$viewContentLoaded', function () {
        var _filter = JSON.parse(localStorage.getItem('GradeMerchaFilter'));
        if (_filter) {
            $scope.Filtro=_filter
            $scope.CarregarGrade($scope.Filtro);
        }
    });
}]);

