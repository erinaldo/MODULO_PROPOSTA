angular.module('App').controller('DepositorioFitasController', ['$scope', '$rootScope', '$location', 'httpService', '$location', function ($scope, $rootScope, $location, httpService, $location) {

    //========================Verifica Permissoes
    $scope.PermissaoNew = false;
    $scope.PermissaoEdit = false;
    $scope.PermissaoDelete = false;
    $scope.PermissaoProgramaAvulso = false;

    httpService.Get("credential/DepositorioFitas@New").then(function (response) {
        $scope.PermissaoNew = response.data;
    });
    httpService.Get("credential/DepositorioFitas@Edit").then(function (response) {
        $scope.PermissaoEdit = response.data;
    });
    httpService.Get("credential/DepositorioFitas@Destroy").then(function (response) {
        $scope.PermissaoDelete = response.data;
    });


    //====================Inicializa scopes

    $scope.PosSit = [
        { 'id': 1, 'nome': 'Avulso' },
        { 'id': 2, 'nome': 'Artistico' },
        { 'id': 3, 'nome': 'Ambos' }
    ]


    $scope.CurrentShow = "Filtro";

    $scope.CompetenciaKeys = { 'Year': new Date().getFullYear(), 'First': '', 'Last': '' }

    $scope.gridheaders = [{ 'title': '', 'visible': true, 'searchable': false, 'config': false, 'sortable': false },
    { 'title': 'Tipo', 'visible': true, 'searchable': true, 'config': true, 'sortable': true },
    { 'title': 'Tit.Comercial', 'visible': true, 'searchable': true, 'config': true, 'sortable': true },
    { 'title': 'Produto', 'visible': true, 'searchable': true, 'config': true, 'sortable': true },
    { 'title': 'Dur.', 'visible': true, 'searchable': true, 'config': true, 'sortable': true },
    { 'title': 'Status', 'visible': true, 'searchable': true, 'config': true, 'sortable': true },
    { 'title': 'N.Fita', 'visible': true, 'searchable': true, 'config': true, 'sortable': true },
    { 'title': 'Inicio.Prog', 'visible': true, 'searchable': true, 'config': true, 'sortable': true },
    { 'title': 'Térm.Prog', 'visible': true, 'searchable': true, 'config': true, 'sortable': true },
    { 'title': '', 'visible': true, 'searchable': false, 'config': false, 'sortable': false }
    ];
    //====================Inicializa o Filtro
    $scope.Filtro = {};
    $scope.NewFiltro = function () {
        localStorage.removeItem('DepositorioFitas');
        return {
            'Cod_Veiculo': '',
            'Situacao': 1,
            'Numero_Fita_Inicio': '',
            'Numero_Fita_Fim': '',
            'Data_Inicio': '',
            'Data_Final': ''
        }
    }
    //===========================Se ja tiver filtro anterior gravado
    var _Filter = JSON.parse(localStorage.getItem('DepositorioFitas'));

    if (_Filter) {
        $scope.Filtro = _Filter;
    }
    else {
        $scope.Filtro = $scope.NewFiltro();
    }


    //====================Quando terminar carga do grid, torna view do grid visible
    $scope.RepeatFinished = function () {
        $rootScope.routeloading = false;
        $scope.ConfiguraGrid();
        $scope.CurrentShow = 'Grid';
        setTimeout(function () {
            $("#dataTable").dataTable().fnAdjustColumnSizing();
        }, 1000)
    };


    //====================Formata o Numero da Fita Inicio
    $scope.FormataFitaInicio = function (pNumeroFita) {
        var _letra = '';
        if ($scope.Filtro.Situacao==1) {
            _letra = 'CO'
        }
        else {
            _letra = 'AR'
        }
        if (pNumeroFita) {
            pNumeroFita = pNumeroFita.replace(/[^0-9]/g, '')
            pNumeroFita = '000000' + pNumeroFita;
            pNumeroFita = pNumeroFita.slice(pNumeroFita.length - 6);
            pNumeroFita = _letra + pNumeroFita;
            $scope.Filtro.Numero_Fita_Inicio = pNumeroFita;
        };
    };
    //====================Formata o Numero da Fita Inicio
    $scope.FormataFitaFim = function (pNumeroFita) {
        var _letra = '';
        if ($scope.Filtro.Situacao == 2) {
            _letra = 'AR'
        }
        else {
            _letra = 'CO'
        }
        if (pNumeroFita) {
            pNumeroFita = pNumeroFita.replace(/[^0-9]/g, '')
            pNumeroFita = '000000' + pNumeroFita;
            pNumeroFita = pNumeroFita.slice(pNumeroFita.length - 6);
            pNumeroFita = _letra + pNumeroFita;
            $scope.Filtro.Numero_Fita_Fim = pNumeroFita;
        };
    };
    //====================Carrega o Grid

    $scope.CarregarDeposFitasAvulsoArtistico = function (pFiltro) {
        $scope.DepositorioFitasS = [];
        if (!pFiltro.Cod_Veiculo) {
            ShowAlert("Codigo veículo é um filtro obrigatório");
            return;
        }
        localStorage.setItem('DepositorioFitas', JSON.stringify($scope.Filtro));
        $scope.CurrentShow = '';
        $('#dataTable').dataTable().fnDestroy();
        var _url = 'DepositoFitasListar';
        _url += '?Cod_Veiculo=' + pFiltro.Cod_Veiculo;
        _url += '&Situacao=' + pFiltro.Situacao;
        _url += '&Numero_Fita_Inicio=' + pFiltro.Numero_Fita_Inicio;
        _url += '&Numero_Fita_Fim=' + pFiltro.Numero_Fita_Fim;
        _url += '&Data_Inicio=' + pFiltro.Data_Inicio;
        _url += '&Data_Final=' + pFiltro.Data_Final;
        _url += '&';
        httpService.Get(_url).then(function (response) {
            if (response) {
                $scope.DepositorioFitasS = response.data;
                if ($scope.DepositorioFitasS.length == 0) {
                    $scope.RepeatFinished();
                }
            }
        });
    };
    //==================== Nova Fita Avulso ou Artistico 
    $scope.NovaFitaAvulsoArtistico = function () {
        $location.path("/DeposFitasAvulsoArtisticoCadastro/New/0")
    }
    //==================== Edicao da fita avulso ou artisitico
    $scope.NovaMapaReservaCompensacao = function (pIdFitaAvulsoArtistico) {
        $location.path("/DeposFitasAvulsoArtisticoCadastro/Edit/" + pIdFitaAvulsoArtistico)
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
                //{ text: 'Novo Mapa Reserva', className: 'btn btn-primary btnNew', action: function (e, dt, button, config) { $('#btnNovoMapaReservaCompensacao').click(); } },
                {
                    text: 'Abrir no Excel<span class="fa fa-file-excel-o margin-left-10" style="color:white"></span>', type: 'excel', className: 'btn btn-warning', extend: 'excel', exportOptions: {
                        columns: ':visible:not(:first-child)'
                    }
                },
            { text: 'Novo Filtro' + '<span class="fa fa-filter margin-left-10"></span>', className: 'btn btn-info', action: function (e, dt, button, config) { $('#btnNovoFiltro').click(); } },
            { text: 'Nova Fita', className: 'btn  btn-primary',  action: function (e, dt, button, config) { $('#btnNovoDepositorioFitasCadastro').click(); } },
            ];
        param.order = [[1, 'desc']];
        ////param.autoWidth = false;
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
    //======================Excluir
    $scope.ExcluirDepositorioFitas = function (pDepositorioFitas) {
        swal({
            title: "Tem certeza que deseja Excluir esta  Fita ?",
            type: "warning",
            showCancelButton: true,
            confirmButtonClass: "btn-danger",
            confirmButtonText: "Sim, Excluir?",
            cancelButtonText: "Cancelar",
            closeOnConfirm: true
        }, function () {
            var _data = {"Cod_Veiculo":pDepositorioFitas.Cod_Veiculo,
                "Numero_Fita":pDepositorioFitas.Numero_Fita,
                "Tipo_Fita":pDepositorioFitas.Numero_Fita.substr(0,2)
            }
            httpService.Post("ExcluirDepositorioFitas", _data).then(function (response) {
                if (response) {

                    if (response.data[0].Status) {
                        ShowAlert(response.data[0].Mensagem, 'success');
                        for (var i = 0; i < $scope.DepositorioFitasS.length; i++) {
                            if ($scope.DepositorioFitasS[i].Cod_Veiculo==pDepositorioFitas.Cod_Veiculo && $scope.DepositorioFitasS[i].Numero_Fita==pDepositorioFitas.Numero_Fita) {
                                $scope.DepositorioFitasS.splice(i,1);
                                break;
                            }
                        };
                    }
                    else {
                        ShowAlert(response.data[0].Mensagem, 'warning');
                    }
                };
            });
        });

    };

    //===========================Evento chamado ao fim do ngrepeat ao carregar grid 
    $scope.$on('ngRepeatFinished', function (ngRepeatFinishedEvent) {
        $scope.RepeatFinished();
    });

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
                $scope.CarregarDeposFitasAvulsoArtistico(_Filter);
            }
        }
    });

}]);


