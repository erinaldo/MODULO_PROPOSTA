﻿angular.module('App').controller('ProgramaController', ['$scope', '$rootScope', 'httpService', '$location', '$timeout', function ($scope, $rootScope, httpService, $location, $timeout) {
    $scope.Info = { 'Title': '', 'Text': '' };
    //========================Verifica Permissoes
    $scope.PermissaoNew = false;
    $scope.PermissaoEdit = false;
    httpService.Get("credential/Programa@New").then(function (response) {
        $scope.PermissaoNew = response.data;
    });
    httpService.Get("credential/Programa@Edit").then(function (response) {
        $scope.PermissaoEdit = response.data;
    });
    //====================Inicializa scopes
    $scope.ShowGrid = false;
    $scope.gridheaders = [{ 'title': '', 'visible': true, 'searchable': false, 'sortable': false },
        { 'title': 'Cod_Programa', 'visible': true, 'searchable': true, 'sortable': true },
        { 'title': 'Titulo', 'visible': true, 'searchable': true, 'sortable': true },
        { 'title': 'Descricao', 'visible': true, 'searchable': true, 'sortable': true },
        { 'title': 'Grade Merch', 'visible': true, 'searchable': true, 'sortable': true },
        { 'title': 'Indica_evento', 'visible': true, 'searchable': true, 'sortable': true },
        { 'title': 'Indica_Rotativo', 'visible': true, 'searchable': true, 'sortable': true },
        { 'title': 'Indica_Local', 'visible': true, 'searchable': true, 'sortable': true },
        { 'title': 'Indica_Desativado', 'visible': true, 'searchable': true, 'sortable': true },
        { 'title': 'Codigo_Veiculo', 'visible': true, 'searchable': true, 'sortable': true },
        { 'title': 'Indica_Programet', 'visible': true, 'searchable': true, 'sortable': true },
        { 'title': 'Indica_Boletim', 'visible': true, 'searchable': true, 'sortable': true },
        { 'title': 'NomeRede', 'visible': true, 'searchable': true, 'sortable': true },
        { 'title': 'Horario_Exibicao', 'visible': true, 'searchable': true, 'sortable': true },
    ];
    //======================Verifica se tem filtro anterior
    $scope.Filtro = { 'Codigo_Rede': 0 }
    var _Filter = JSON.parse(localStorage.getItem('ProgramaFilter'));
    if (_Filter) {
        $scope.Filtro = _Filter;
    }
    //====================Carrega Combo de Redes
    $scope.Redes = "";

    httpService.Get("ListarTabela/Rede").then(function (response) {
        $scope.Redes = response.data;
    });

     //====================Quando terminar carga do grid, torna view do grid visible
    $scope.RepeatFinished = function () {
        $rootScope.routeloading = false;
        $scope.ConfiguraGrid();
        $scope.ShowGrid = true;
        setTimeout(function () {
            $("#dataTable").dataTable().fnAdjustColumnSizing();
        }, 10)
    };

    //====================Carrega o Grid
    $scope.CarregarPrograma = function () {
        $rootScope.routeloading = true;
        $scope.Programas = [];
        $scope.ShowGrid = false;
        $('#dataTable').dataTable().fnDestroy();
        var _url = 'ProgramaListar/';
        if ($scope.Filtro.Codigo_Rede) {
            _url += $scope.Filtro.Codigo_Rede
        }
        else {
            _url += "0";
        }

        httpService.Get(_url).then(function (response) {
            if (response) {
                $scope.Programas = response.data;
                localStorage.setItem('ProgramaFilter', JSON.stringify($scope.Filtro));
                if ($scope.Programas.length == 0) {
                    $scope.RepeatFinished();
                }
            }
        });
    };
    //====================Funcao para configurar o Grid
    $scope.ConfiguraGrid = function () {
        param = {};
        param.language = fnDataTableLanguage();
        param.lengthMenu = [[7, 10, 25, 50, -1], [7, 10, 25, 50, "Todos"]];
        param.pageLength = 7;
        param.scrollCollapse = true;
        param.paging = true;

        param.dom = "<'row'<'col-sm-3'l><'col-sm-4'f><'col-sm-5'B>>" + "<'row'<'col-sm-12'tr>>" + "<'row'<'col-sm-5'i><'col-sm-7'p>>",
            param.buttons = [
                {
                    text: 'Exportar<span class="fa fa-file-excel-o margin-left-10" style="color:white"></span>', type: 'excel', className: 'btn btn-warning HideButton', extend: 'excel', exportOptions: {
                        columns: ':visible:not(:first-child)'
                    }
                }
            ];
        param.order = [[0, 'asc']];
        param.autoWidth = false;
        param.columns = [];
        for (var i = 0; i < $scope.gridheaders.length; i++) {
            param.columns.push({ "visible": $scope.gridheaders[i].visible, "searchable": $scope.gridheaders[i].searchable, "sortable": $scope.gridheaders[i].sortable });
        }
        $('#dataTable').DataTable(param);
    };
    //===========================Mostra os veiculos do programa
    $scope.Mostra_Veiculo = function (pCod_Programa) {
        httpService.Get('VeiculosMostrar/' + pCod_Programa).then(function (response) {
            if (response.data) {
                var _text = [];
                for (var i = 0; i < response.data.length; i++) {
                    _text.push(response.data[i].Cod_Veiculo + ' - ' + response.data[i].Nome_Veiculo);
                };
                $scope.Info = {
                    'Title': 'Veiculo',
                    'Text': _text
                }
                $("#modalInfo").modal(true);
            }
        });
    };
    //===========================Evento chamado ao fim do ngrepeat ao carregar grid 
    $scope.$on('ngRepeatFinished', function (ngRepeatFinishedEvent) {
        $scope.RepeatFinished();
    });
    //===========================fim do load da pagina
    $scope.$watch('$viewContentLoaded', function () {
        $scope.ConfiguraGrid();
        $scope.CarregarPrograma();
    });
    $scope.EditarPrograma

}]);

