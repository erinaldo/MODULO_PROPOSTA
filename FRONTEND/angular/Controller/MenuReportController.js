angular.module('App').controller('MenuReportController', ['$scope', '$rootScope', 'httpService', '$location', function ($scope, $rootScope, httpService, $location) {

    //===================Inicializa Scopes
    $scope.Reports = [
        //{ 'Id': 1, 'Key': 'R0063R', 'Title': 'Valores Investidos - Resumido' },
        { 'Id': 1, 'Key': 'R0063D', 'Title': 'Valores Investidos' },
        { 'Id': 2, 'Key': 'R0067', 'Title': 'Valores por Contrato' },
        { 'Id': 3, 'Key': 'R0015', 'Title': 'Relatório de Faturas' },
        { 'Id': 4, 'Key': 'R0098', 'Title': 'Listagem de Checking' },
        { 'Id': 5, 'Key': 'R0106', 'Title': 'Desconto por Contato' },


    ];
    $scope.gridheaders = [];
    $scope.CurrentShow = 'Menu';
    $scope.PesquisaTabelas = { "Items": [], 'FiltroTexto': '', ClickCallBack: '', 'Titulo': '', 'MultiSelect': false };
    $scope.MesAnoKeys = { 'Year': new Date().getFullYear(), 'First': '', 'Last': '' }
    $scope.ReportData = [];
    $scope.CurrentReport = "";
    $scope.QtdLoad = 0;
    //===========================Apresenta Filtro do Relatorio
    $scope.ReportFilter = function (pReport) {
        $scope.CurrentReport = pReport;
        httpService.Get("Report/GetFilter/" + pReport.Key).then(function (response) {
            if (response.data) {
                $scope.Filter = response.data;
                $scope.CurrentShow = 'Filter'
                for (var i = 0; i < $scope.Filter.Filters.length; i++) {
                    if ($scope.Filter.Filters[i].Type == 'List') {
                        $scope.CarregaItem($scope.Filter.Filters[i]);
                    };
                };
            };
        });
    };

    //===========================Lupa de Pesquisa - Simples Item
    $scope.PesquisaItems = function (pFilter) {
        if (pFilter.BigQuery) {
            $scope.PesquisaItemsBigQuery(pFilter);
        }
        else {
            $scope.PesquisaItemsSmallQuery(pFilter);
        }
    }
    //===========================Lupa de Pesquisa - Simples Item - Small Query
    $scope.PesquisaItemsSmallQuery = function (pFilter) {
        $scope.PesquisaTabelas = NewPesquisaTabela();
        var _url = "ListarTabela/" + pFilter.Dictionary;
        httpService.Get(_url).then(function (response) {
            if (response.data) {
                $scope.PesquisaTabelas.Items = response.data
                $scope.PesquisaTabelas.FiltroTexto = ""
                $scope.PesquisaTabelas.Titulo = "Pesquisa"
                $scope.PesquisaTabelas.MultiSelect = pFilter.Multiple;
                $scope.PesquisaTabelas.PreFilter = pFilter.BigQuery;
                $scope.PesquisaTabelas.ClickCallBack = function (value) {
                    pFilter.Value = value.Codigo;
                };
            };
            $("#modalTabela").modal(true);

        });
    };
    //===========================Lupa de Pesquisa - Simples Item - Big Query
    $scope.PesquisaItemsBigQuery = function (pFilter) {
        $scope.PesquisaTabelas = NewPesquisaTabela();
        $scope.PesquisaTabelas.FiltroTexto = ""
        $scope.PesquisaTabelas.Titulo = "Pesquisa"
        $scope.PesquisaTabelas.MultiSelect = pFilter.Multiple;
        $scope.PesquisaTabelas.PreFilter = pFilter.BigQuery;
        $scope.PesquisaTabelas.ClickCallBack = function (value) {
            pFilter.Value = value.Codigo;
        };
        $scope.PesquisaTabelas.LoadCallBack = function (pValue) {
            var _url = 'ListarTabela/' + pFilter.Dictionary + '/' + pValue;
            httpService.Get(_url).then(function (response) {
                $scope.PesquisaTabelas.Items = response.data;
            });
        }
        $("#modalTabela").modal(true);
    };
    //===========================Lupa de Pesquisa - Multipla escolha
    $scope.PesquisaArrayItems = function (pFilter) {
        $scope.PesquisaTabelas.Items = pFilter.ArrayValue;
        $scope.PesquisaTabelas.FiltroTexto = ""
        $scope.PesquisaTabelas.Titulo = "Pesquisa"
        $scope.PesquisaTabelas.MultiSelect = pFilter.Multiple;
        $scope.PesquisaTabelas.ClickCallBack = function (value) {
            pFilter.SelectedCount = 0;
            for (var i = 0; i < pFilter.ArrayValue.length; i++) {
                if (pFilter.ArrayValue[i].Selected) {
                    pFilter.SelectedCount++;
                };
            };
        };
        $("#modalTabela").modal(true);
    };
    //===========================Valida Item
    $scope.ValidarItem = function (pFilter) {
        if (!pFilter.Value) {
            return;
        }
        var _url = "ValidarTabela/" + pFilter.Dictionary.trim() + '/' + pFilter.Value.trim();
        httpService.Get(_url).then(function (response) {
            if (response.data) {
                if (response.data[0].Status == 0) {
                    ShowAlert(response.data[0].Mensagem);
                    pFilter.Value = "";
                };

            };
        });
    };
    //===========================Carrega Itens para Multipla Escolha
    $scope.CarregaItem = function (pFilter) {
        httpService.Get("ListarTabela/" + pFilter.Dictionary.trim()).then(function (response) {
            pFilter.ArrayValue = response.data;
            for (var i = 0; i < pFilter.ArrayValue.length; i++) {
                pFilter.ArrayValue[i].Selected = false;
            };
        });
    };
    //===========================Gera o Relatorio
    $scope.PrintReport = function (pFilter) {
        httpService.Post("Report/Print", pFilter).then(function (response) {
            if (response.data) {
                url = $rootScope.baseUrl + "PDFFILES/REPORT/" + $rootScope.UserData.Login.trim() + "/" + response.data;
                var win = window.open(url, '_blank');
                win.focus();
            }
            else {
                ShowAlert("Não existe Dados a ser Impresso")
            }
        });
    };
    //====================Configuracao do Grid
    $scope.ConfiguraGrid = function (pFilter) {
        param = {};
        param.language = fnDataTableLanguage();
        param.lengthMenu = [[7,10, 15,20, 50, -1], [7,10,15, 20, 50, "Todos"]];

        param.scrollCollapse = true;
        param.paging = true;
        param.dom = "<'row'<'col-sm-6'B><'col-sm-3'l><'col-sm-3'f>>" + "<'row'<'col-sm-12'tr>>" + "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        param.buttons = [
            { text: 'Abrir no Excel<span class="fa fa-file-excel-o margin-left-10" style="color:white"></span>', type: 'excel', className: 'btn btn-warning', extend: 'excel', },
            { text: 'Voltar' + '<span class="fa fa-arrow-circle-o-left margin-left-10"></span>', className: 'btn btn-info', action: function (e, dt, button, config) { $('#btnNovoFiltro').click(); } },
        ];
        param.order = [];
        param.autoWidth = false;
        
        param.columns = [];
        for (var i = 0; i < $scope.gridheaders.length; i++) {
            param.columns.push({ "visible": $scope.gridheaders[i].visible, "searchable": $scope.gridheaders[i].searchable, "sortable": $scope.gridheaders[i].sortable });
        }
        $('#dataTable').DataTable(param);
        var table = $('#dataTable').DataTable();
        var buttons = table.buttons([0]);
        if (table.rows({ selected: true }).indexes().length === 0) {
            buttons.disable();
        }
        else {
            buttons.enable();
        }
    };
    //===========================Carrega os dados para exibicao na tela
    $scope.CarregaGrid = function (pFilter) {
        
        httpService.Post("Report/LoadData", pFilter).then(function (response) {
            if (response.data.length > 0) {
                $scope.QtdLoad++;
                $scope.ReportData = response.data;
                if ($scope.QtdLoad == 1) {
                    angular.forEach($scope.ReportData[0], function (value, key) {
                        $scope.gridheaders.push({ 'title': key, 'visible': true, 'searchable': true, 'config': false, 'sortable': false });
                    });
                }
                $scope.CurrentShow = 'Grid';
            }
            else {
                ShowAlert("Não ha dados a ser mostrado com esse filtro.")
            }
        });
    };
    //===========================Repeat finished
    $scope.RepeatFinished = function () {
        $rootScope.routeloading = false;
        $scope.ConfiguraGrid($scope.ReportData);
        setTimeout(function () {
            $("#dataTable").dataTable().fnAdjustColumnSizing();
        }, 1000)
    };
    //===========================novo filtro
    $scope.NovoFiltro = function () {
        $scope.ReportData = [];
        $("#dataTable").dataTable().fnDestroy();
        $scope.CurrentShow = "Filter";
        //$scope.gridheaders = [];
    };
    $scope.CancelarReport = function () {
        location.reload();
    };
    //===========================Evento chamado ao fim do ngrepeat ao carregar grid 
    $scope.$on('ngRepeatFinished', function (ngRepeatFinishedEvent) {
        $scope.RepeatFinished($scope.ReportData);
    });
    //===========================fim do load da pagina
    $scope.$watch('$viewContentLoaded', function (pReport) {
    });
}]);

