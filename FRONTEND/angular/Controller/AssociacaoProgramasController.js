angular.module('App').controller('AssociacaoProgramasController', ['$scope', '$rootScope', '$location', 'httpService', '$location', function ($scope, $rootScope, $location, httpService, $location) {

    //====================Inicializa scopes
    $scope.CurrentShow = "Filtro";
    $scope.AssociacaoProgramas = [];

    $scope.gridheaders = [{ 'title': 'Edit', 'visible': true, 'searchable': false, 'config': false, 'sortable': false },
    { 'title': 'Código Programa', 'visible': true, 'searchable': true, 'config': true, 'sortable': true },
    { 'title': 'Titulo', 'visible': true, 'searchable': true, 'config': true, 'sortable': true },
    { 'title': 'Item', 'visible': true, 'searchable': true, 'config': true, 'sortable': true },
    { 'title': 'Conta Contábil', 'visible': true, 'searchable': true, 'config': true, 'sortable': true },
    ];


    //====================Inicializa o Filtro
    $scope.NewFiltro = function () {
        localStorage.removeItem('Filter');
        return {
            'Cod_Empresa_Faturamento': ''
        }
    }
    //===========================Se ja tiver filtro anterior gravado
    var _Filter = JSON.parse(localStorage.getItem('AssociacaoProgramasFilter'));

    if (_Filter) {
        $scope.Filtro = _Filter;
    }
    else {
        $scope.Filtro = $scope.NewFiltro();
    }

    //====================Permissoes
    //========================Verifica Permissoes
    $scope.PermissaoNew = false;
    $scope.PermissaoEdit = false;
    httpService.Get("credential/AssociacaoProgramas@New").then(function (response) {
        $scope.PermissaoNew = response.data;
    });
    httpService.Get("credential/AssociacaoProgramas@Edit").then(function (response) {
        $scope.PermissaoEdit = response.data;
    });

    //====================Quando terminar carga do grid, torna view do grid visible
    $scope.RepeatFinished = function () {
        $rootScope.routeloading = false;
        $scope.ConfiguraGrid();
        $scope.CurrentShow = 'Grid';
        setTimeout(function () {
            $("#dataTable").dataTable().fnAdjustColumnSizing();
        }, 1000)
    };


    //====================Carrega o Grid

           
    $scope.CarregarAssociacaoProgramas = function (pFiltro) {

        if (pFiltro.Cod_Empresa_Faturamento == "") {
            ShowAlert("Competência são é filtro Obrigatório.");
            return;
        }
        $scope.AssociacaoProgramas = [];
        localStorage.setItem('AssociacaoProgramasFilter', JSON.stringify($scope.Filtro));
        $scope.CurrentShow = '';
        $('#dataTable').dataTable().fnDestroy();
        httpService.Post('AssociacaoProgramasListar', pFiltro).then(function (response) {
            if (response) {
                $scope.AssociacaoProgramas = response.data;
                if ($scope.AssociacaoProgramas.length == 0) {
                    $scope.RepeatFinished();
                }
            }
        });
    };



    //====================Configuracao do Grid
    $scope.ConfiguraGrid = function () {
        param = {};
        param.language = fnDataTableLanguage();
        param.lengthMenu = [[7, 10, 25, 50, -1], [7, 10, 25, 50, "Todos"]];

        //param.scrollCollapse = true;
        param.paging = true;
        param.dom = "<'row'<'col-sm-6'B><'col-sm-3'l><'col-sm-3'f>>" + "<'row'<'col-sm-12'tr>>" + "<'row'<'col-sm-5'i><'col-sm-7'p>>",
            param.buttons = [
               
                {
                    text: 'Abrir no Excel<span class="fa fa-file-excel-o margin-left-10" style="color:white"></span>', type: 'excel', className: 'btn btn-warning', extend: 'excel', exportOptions: {
                        columns: ':visible:not(:first-child)'
                    }
                },
                { text: 'Novo Filtro' + '<span class="fa fa-filter margin-left-10"></span>', className: 'btn btn-info', action: function (e, dt, button, config) { $('#btnNovoFiltro').click(); } },
            ];
        param.order = [[1, 'desc']];
        param.autoWidth = false;

        param.columns = [];
        for (var i = 0; i < $scope.gridheaders.length; i++) {
            param.columns.push({ "visible": $scope.gridheaders[i].visible, "searchable": $scope.gridheaders[i].searchable, "sortable": $scope.gridheaders[i].sortable });
        }
        $('#dataTable').DataTable(param);
        var table = $('#dataTable').DataTable();
        var buttons = table.buttons([1]);
    };
    //===========================fim do load da pagina
    $scope.$watch('$viewContentLoaded', function () {
        $rootScope.routeloading = false;
        $scope.ConfiguraGrid();

        if (_Filter) {
            var _Carregar = false
            angular.forEach(_Filter, function (value, key) {
                if (value) {
                    _Carregar = true;
                }
            });
            if (_Carregar) {
                       
                $scope.CarregarAssociacaoProgramas(_Filter);
            }
        }
    });

    //===========================Evento chamado ao fim do ngrepeat ao carregar grid 
    $scope.$on('ngRepeatFinished', function (ngRepeatFinishedEvent) {
        $scope.RepeatFinished();
    });
}]);


