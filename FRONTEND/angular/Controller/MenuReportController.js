angular.module('App').controller('MenuReportController', ['$scope', '$rootScope', 'httpService', '$location', function ($scope, $rootScope, httpService, $location) {

    //===================Inicializa Scopes
    $scope.Reports = [
        //{ 'Id': 1, 'Key': 'R0063R', 'Title': 'Valores Investidos - Resumido' },
        { 'Id': 1, 'Key': 'R0063', 'Title': 'Valores Investidos' },
        { 'Id': 2, 'Key': 'R0067', 'Title': 'Valores por Contrato' },
        { 'Id': 3, 'Key': 'R0015', 'Title': 'Relatório de Faturas' },
        { 'Id': 4, 'Key': 'R0098', 'Title': 'Listagem de Checking' },
        { 'Id': 5, 'Key': 'R0106', 'Title': 'Desconto por Contato' },
        { 'Id': 6, 'Key': 'R0052', 'Title': 'Demonstrativo Agência Cliente' },
        { 'Id': 7, 'Key': 'R0118', 'Title': 'Receita por Programa' },
        //{ 'Id': 7, 'Key': 'R0054', 'Title': 'Demonstrativo Programa Produto' },
        //{ 'Id': 8, 'Key': 'R0027', 'Title': 'Comissões Extra-Faturamento' },
        //{ 'Id': 9, 'Key': 'R0179', 'Title': 'TRE' },
        //{ 'Id': 10, 'Key': 'R0085', 'Title': 'Vendas por Contato' },
        //{ 'Id': 11, 'Key': 'R0010', 'Title': 'Veiculacoes Sem Faturamento' },
        //{ 'Id': 12, 'Key': 'R0021', 'Title': 'Resumo das Disponibilidades' },
        //{ 'Id': 13, 'Key': 'R0161', 'Title': 'Veiculacoes' },
         { 'Id': 8, 'Key': 'R0050', 'Title': 'Negociações Antecipadas' },
         { 'Id': 14, 'Key': 'R0124', 'Title': 'Preço Médio' },
        { 'Id': 15, 'Key': 'R0125', 'Title': 'Análise de Rotativo' },
        { 'Id': 16, 'Key': 'R0095', 'Title': 'Conta Corrente da Negociação' },
        //{ 'Id': 16, 'Key': 'R0183', 'Title': 'Valor Médio por Programa' },
        { 'Id': 17, 'Key': 'R0076', 'Title': 'Contratos Complementados/A Complementar' },
        { 'Id': 17, 'Key': 'R0183', 'Title': 'Análise dos Breaks' },
    ];
    $scope.gridheaders = [];
    $scope.CurrentShow = 'Menu';
    $scope.PesquisaTabelas = { "Items": [], 'FiltroTexto': '', ClickCallBack: '', 'Titulo': '', 'MultiSelect': false };
    $scope.MesAnoKeys = { 'Year': new Date().getFullYear(), 'First': '', 'Last': '' }
    $scope.ReportData = [];
    $scope.CurrentReport = "";
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
        $scope.PesquisaTabelas = NewPesquisaTabela();
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
        if (!pFilter.Value || !pFilter.Dictionary) {
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
    $scope.PrintReport = function () {
        for (var i = 0; i < $scope.Filter.Filters.length; i++) {
            if ($scope.Filter.Filters[i].ConfigRptName) {
                $scope.Filter.RptName = $scope.Filter.Filters[i].Options[$scope.Filter.Filters[i].Value].RptName;
            };
            if ($scope.Filter.Filters[i].ConfigUrl) {
                console.log($scope.Filter.Filters[i]);
                $scope.Filter.Url = $scope.Filter.Filters[i].Options[$scope.Filter.Filters[i].Value].UrlName;
            };
        };


        var _url = "";
        if ($scope.Filter.Url) {
            _url = $scope.Filter.Url;
        }
        else {
            _url = "Report/Print";
        }
        $scope.Filter.Output = 'PDF';
        httpService.Post(_url, $scope.Filter).then(function (response) {
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
    $scope.ConfiguraGrid = function () {
        param = {};
        param.language = fnDataTableLanguage();
        param.lengthMenu = [[7,10, 15,20, 50, -1], [7,10,15, 20, 50, "Todos"]];
        param.pageLength= 10;

        param.scrollCollapse = true;
        param.paging = true;
        param.dom = "<'row'<'col-sm-6'B><'col-sm-3'l><'col-sm-3'f>>" + "<'row'<'col-sm-12'tr>>" + "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        param.buttons = [
            { text: 'Abrir no Excel<span class="fa fa-file-excel-o margin-left-10" style="color:white"></span>', type: 'excel', className: 'btn btn-primary', extend: 'excel', },
            { text: 'Gerar PDF' + '<span class="fa fa-file-pdf"></span>', className: 'btn btn-primary', action: function (e, dt, button, config) { $('#btnGerarPdf').click(); } },
            { text: 'Voltar' + '<span class="fa fa-arrow-circle-o-left margin-left-10"></span>', className: 'btn btn-warning', action: function (e, dt, button, config) { $('#btnNovoFiltro').click(); } },
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
        pFilter.Output = 'TELA';
        httpService.Post("Report/LoadData", pFilter).then(function (response) {
            if (response.data.length > 0) {
                
                $scope.gridheaders = [];
                angular.forEach(response.data[0], function (value, key) {
                        $scope.gridheaders.push({ 'title': key, 'visible': true, 'searchable': true, 'config': false, 'sortable': true });
                    });

                $scope.ReportData = response.data
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
        $scope.ConfiguraGrid();
        setTimeout(function () {
            $("#dataTable").dataTable().fnAdjustColumnSizing();
        }, 1000)
        setHeader($scope.gridheaders);
    };
    //===========================novo filtro
    $scope.NovoFiltro = function () {
        $scope.ReportData = [];
        $("#dataTable").dataTable().fnClearTable();
        $("#dataTable").dataTable().fnDestroy();
        $scope.CurrentShow = "Filter";
        $scope.gridheaders = [];
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

function setHeader(head) {
    var elements = document.getElementsByClassName("sorting");
    for (var i = 0, len = elements.length; i < len; i++) {
        elements[i].innerText = head[i].title;
    };

}