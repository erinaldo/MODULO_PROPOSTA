angular.module('App').controller('PermutasController', ['$scope', '$rootScope', '$location', 'httpService', '$location', function ($scope, $rootScope, $location, httpService, $location) {

    //====================Inicializa scopes
    $scope.CurrentShow = "Filtro";
    $scope.permuta = [];

    $scope.gridheaders = [{ 'title': 'Edit', 'visible': true, 'searchable': false, 'config': false, 'sortable': false },
    { 'title': 'Botao2', 'visible': true, 'searchable': false, 'config': false, 'sortable': false },
    { 'title': 'Contrato', 'visible': true, 'searchable': true, 'config': true, 'sortable': true },
    { 'title': 'Negociação', 'visible': true, 'searchable': true, 'config': true, 'sortable': true },
    { 'title': 'Período Campanha', 'visible': true, 'searchable': true, 'config': true, 'sortable': true },
    { 'title': 'Agencia', 'visible': true, 'searchable': true, 'config': true, 'sortable': true },
    { 'title': 'Cliente', 'visible': true, 'searchable': true, 'config': true, 'sortable': true },
    { 'title': 'Contato', 'visible': true, 'searchable': true, 'config': true, 'sortable': true },
    ];
    $scope.MesAnoKeys = { 'Year': new Date().getFullYear(), 'First': '', 'Last': '' }

    //====================Inicializa o Filtro
    $scope.NewFiltro = function () {
        localStorage.removeItem('Filter');
        return {
            'Numero_Negociacao': '',
            'Numero_Mr': '',
            'Competencia_Inicio': '',
            'Competencia_Fim': '',
            'Numero_Contrato': '',
            'Cod_Empresa_Venda': '',
            'Cod_Empresa_Faturamento': '',
            'Agencia': '',
            'Cliente': '',
            'Contato': ''
        }
    }
    //===========================Se ja tiver filtro anterior gravado
    var _Filter = JSON.parse(localStorage.getItem('PermutaFilter'));

    if (_Filter) {
        $scope.Filtro = _Filter;
    }
    else {
        $scope.Filtro = $scope.NewFiltro();
    }

    //====================Permissoes
    $scope.PermissaoNew = 'false';
    $scope.PermissaoEditar = 'false';
    httpService.Get("credential/" + "Permuta@New").then(function (response) {
        $scope.PermissaoEdit = response.data;
    });
    httpService.Get("credential/" + "Permuta@New").then(function (response) {
        $scope.PermissaoNew = response.data;
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

    //==================== Novo Permuta
    $scope.NovaPermuta = function () {
        $location.path("/PermutasCadastro/New/0")
    }

    //====================Carrega o Grid
    $scope.CarregarPermuta = function (pFiltro) {

        if (pFiltro.Competencia_Inicio == "" || pFiltro.Competencia_Fim == "") {
            ShowAlert("Competências são filtros Obrigatórios.");
            return;
        }


        $scope.Permutas = [];
        localStorage.setItem('PermutaFilter', JSON.stringify($scope.Filtro));
        $scope.CurrentShow = '';
        $('#dataTable').dataTable().fnDestroy();
        httpService.Post('CarregarPermutas', pFiltro).then(function (response) {
            if (response) {
                $scope.Permutas = response.data;
                if ($scope.Permutas.length == 0) {
                    $scope.RepeatFinished();
                }
            }
        });
    };

    //==================== Nova Permuta
    $scope.NovaPermutas = function () {
        $location.path("/PermutaEntregaCadastro/New/0")
    }
    //==================== Edicao da Permuta
    $scope.NovaPermutas = function (pIdPermuta) {
        $location.path("/PermutaEntregaCadastro/Edit/" + pIdPermuta)
    }
    //====================Configuracao do Grid
    $scope.ConfiguraGrid = function () {
        param = {};
        param.language = fnDataTableLanguage();
        param.lengthMenu = [[7, 10, 25, 50, -1], [7, 10, 25, 50, "Todos"]];

        //param.scrollCollapse = true;
        param.paging = true;
        param.dom = "<'row'<'col-sm-6'B><'col-sm-3'l><'col-sm-3'f>>" + "<'row'<'col-sm-12'tr>>" + "<'row'<'col-sm-5'i><'col-sm-7'p>>",
            param.buttons = [
                //{ text: 'Nova Entrega', className: 'btn btn-primary btnNew', action: function (e, dt, button, config) { $('#btnNovoPermuta').click(); } },
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
        //if (table.rows({ selected: true }).indexes().length === 0) {
        //    buttons.disable();
        //}
        //else {
        //    buttons.enable();
        //}
        //var buttonsNew = table.buttons([0]);
        //if (!$scope.PermissaoNew) {
        //    buttonsNew.disable();
        //}
        //else {
        //    buttonsNew.enable();
        //}
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
                $scope.CarregarPermuta(_Filter);
            }
        }
    });

    $scope.NovaPermutas = function () {
        $location.path("/PermutaEntregaCadastro/New/0/");
    };

    //===========================Evento chamado ao fim do ngrepeat ao carregar grid 
    $scope.$on('ngRepeatFinished', function (ngRepeatFinishedEvent) {
        $scope.RepeatFinished();
    });
}]);


