angular.module('App').controller('LogListaEMSController', ['$scope', '$rootScope', '$location', 'httpService', '$location', function ($scope, $rootScope, $location, httpService, $location) {

    //====================Inicializa scopes
    $scope.CurrentShow = "Filtro";
    $scope.LogLista = [];

    $scope.gridheaders = [{ 'title': 'Usuario', 'visible': true, 'searchable': false, 'config': false, 'sortable': false },
    { 'title': 'Data Processamento', 'visible': true, 'searchable': false, 'config': false, 'sortable': false },
    { 'title': 'Operação', 'visible': true, 'searchable': true, 'config': true, 'sortable': true },
    { 'title': 'Histórico', 'visible': true, 'searchable': true, 'config': true, 'sortable': true },
    ];
    $scope.MesAnoKeys = { 'Year': new Date().getFullYear(), 'First': '', 'Last': '' }

    //====================Inicializa o Filtro
    $scope.NewFiltro = function () {
        localStorage.removeItem('Filter');
        return {
            'Data_Inicial': '',
            'Data_Final': '',
            'Cod_Cliente': '',
            'CGC_Cliente': '',
            'Cod_Uusario': '',
            'Nome_Usuario': '',
            'chkDesativacao': true,
            'chkReativacao': true,
            'chkCom_Programacao': true,
            'chkSem_Programacao': true
        }
    }
    //===========================Se ja tiver filtro anterior gravado
    var _Filter = JSON.parse(localStorage.getItem('LogListaEMSFilter'));

    if (_Filter) {
        $scope.Filtro = _Filter;
    }
    else {
        $scope.Filtro = $scope.NewFiltro();
    }

    //====================Permissoes
    $scope.PermissaoNew = 'false';
    $scope.PermissaoEditar = 'false';
    httpService.Get("credential/" + "LogListaEMS@New").then(function (response) {
        $scope.PermissaoEdit = response.data;
    });
    httpService.Get("credential/" + "LogListaEMS@New").then(function (response) {
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



    //====================Carrega o Grid
    $scope.CarregarConsultaLogLista = function (pFiltro) {

        if (pFiltro.Data_Inicial == '' && pFiltro.Data_Final == '' && pFiltro.Cod_Cliente=='' && pFiltro.CGC_Cliente=='' && pFiltro.Cod_Usuario=='')
        {
            ShowAlert("É necessário informar pelo menos um parametro");
            return;
        }

        if (pFiltro.Data_Inicial > pFiltro.Data_Final) {
            ShowAlert("Data Inicial é maior que Data Final.");
            return;
        }


        if (pFiltro.chkDesativacao == "" && pFiltro.chkReativacao == "") {
            ShowAlert("Operação  é filtro Obrigatório.");
            return;
        }

        $scope.LogListas = [];
        localStorage.setItem('LogListaEMSFilter', JSON.stringify($scope.Filtro));
        $scope.CurrentShow = '';
        $('#dataTable').dataTable().fnDestroy();
        httpService.Post('CarregarLogListaEMS', pFiltro).then(function (response) {
            if (response) {
                $scope.LogListas = response.data;
                if ($scope.LogListas.length == 0) {
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
                $scope.CarregarConsultaLogLista(_Filter);
            }
        }
    });

    //===========================Evento chamado ao fim do ngrepeat ao carregar grid 
    $scope.$on('ngRepeatFinished', function (ngRepeatFinishedEvent) {
        $scope.RepeatFinished();
    });
}]);


