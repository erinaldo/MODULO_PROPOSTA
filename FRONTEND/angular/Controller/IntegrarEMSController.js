angular.module('App').controller('IntegrarEMSController', ['$scope', '$rootScope', '$location', 'httpService', '$location', function ($scope, $rootScope, $location, httpService, $location) {

    //====================Inicializa scopes
    $scope.ShowCritica1 = false;
    $scope.ShowCritica2 = false;
    $scope.ShowGrid = false;
    $scope.ShowFiltro = true;
    $scope.Filtro = {};
    $scope.FilCrit = {};
    $scope.CompetenciaKeys = { 'Year': new Date().getFullYear(), 'First': '', 'Last': '' };
    $scope.GuardaOrigem = "";
    $scope.GuardaSEQ_WT_DOCTO = 0;
    $scope.IntegrarFBs = [];
    $scope.gridheaders = [
        { 'title': '', 'visible': true, 'searchable': false, 'config': false, 'sortable': false },
        { 'title': 'Número Fatura', 'visible': true, 'searchable': false, 'config': false, 'sortable': false },
        { 'title': 'Fatura Anterior', 'visible': true, 'searchable': false, 'config': false, 'sortable': false },
        { 'title': 'Negociação', 'visible': true, 'searchable': true, 'config': true, 'sortable': true },
        { 'title': 'Qtde.Parc', 'visible': true, 'searchable': true, 'config': true, 'sortable': true },
        { 'title': 'Qtde.Veic', 'visible': true, 'searchable': false, 'config': false, 'sortable': false },
        { 'title': 'Cliente', 'visible': true, 'searchable': false, 'config': false, 'sortable': false },
        { 'title': 'Produto', 'visible': true, 'searchable': true, 'config': true, 'sortable': true },
        { 'title': 'Agência', 'visible': true, 'searchable': true, 'config': true, 'sortable': true },
        { 'title': 'Contato', 'visible': true, 'searchable': false, 'config': false, 'sortable': false },
        { 'title': 'Tipo', 'visible': true, 'searchable': false, 'config': false, 'sortable': false },
        { 'title': 'Valor', 'visible': true, 'searchable': true, 'config': true, 'sortable': true },
        { 'title': 'Integrada', 'visible': true, 'searchable': true, 'config': true, 'sortable': true },
    ];

    //====================Inicializa o Filtro
    $scope.NovoFiltro = function () {
        $scope.ShowCritica1 = false;
        $scope.ShowCritica2 = false;
        $scope.ShowGrid = false;
        $scope.ShowFiltro = true;
        $scope.chkMarcar = false;
        var _dtCompet = CurrentMMYYYY();

        $scope.Filtro = {
            'Indica_Faturas': true,
            'Indica_Boletos': false,
            'Indica_Diaria': true,
            'Indica_Mensal': false,
            'Cod_Empresa_Faturamento': '',
            'Competencia': _dtCompet,
            'Numero_Negociacao': '',
            'Nota_Fiscal': '',
            'Indica_Pendentes': true,
            'Indica_Todos': false,
        };
    }
    $scope.NovoFiltro()

    //====================Filtro da Critica
    $scope.NovoFilCrit = function () {
        $scope.ShowFiltro = false;
        $scope.ShowGrid = false;
        $scope.ShowCritica2 = false;
        $scope.ShowCritica1 = true;
        $scope.FilCrit = {};
    }

    //--------------------- Controle das Flags -------------------
    $scope.SetaFlag = function (pTipo) {
        //-----Faturas e Boletos
        if (pTipo == 'F') {
            if ($scope.Filtro.Indica_Faturas) {
                $scope.Filtro.Indica_Boletos = false;
            };
        };
        if (pTipo == 'B') {
            if ($scope.Filtro.Indica_Boletos) {
                $scope.Filtro.Indica_Faturas = false;
            };
        };
        //-----Diária e Mensal
        if (pTipo == 'D') {
            if ($scope.Filtro.Indica_Diaria) {
                $scope.Filtro.Indica_Mensal = false;
            };
        };
        if (pTipo == 'M') {
            if ($scope.Filtro.Indica_Mensal) {
                $scope.Filtro.Indica_Diaria = false;
            };
        };
        //-----Pendentes e Todos
        if (pTipo == 'P') {
            if ($scope.Filtro.Indica_Pendentes) {
                $scope.Filtro.Indica_Todos = false;
            };
        };
        if (pTipo == 'T') {
            if ($scope.Filtro.Indica_Todos) {
                $scope.Filtro.Indica_Pendentes = false;
            };
        };

    };


    //----------------------Marca/Desmarca Todos
    $scope.MarcarTodos = function (pFiltro, pValue) {
        for (var i = 0; i < pFiltro.length; i++) {
            pFiltro[i].Indica_Marcado = pValue;
        }
    };


    //====================Quando terminar carga do grid, torna view do grid visible
    $scope.RepeatFinished = function () {
        $rootScope.routeloading = false;
        $scope.ConfiguraGrid();
        $scope.ShowGrid = true;
        setTimeout(function () {
            $("#dataTable").dataTable().fnAdjustColumnSizing();
        }, 1000)
    };

    //====================Carrega o Grid
    $scope.CarregarFaturasBoletos = function (pFiltro) {
        if (pFiltro.Indica_Faturas && pFiltro.Indica_Boletos) {
            ShowAlert("Escolha se é integração de Faturas ou Boletos");
            return;
        }
        if (!pFiltro.Indica_Faturas && !pFiltro.Indica_Boletos) {
            ShowAlert("Escolha se é integração de Faturas ou Boletos");
            return;
        }
        if (pFiltro.Indica_Boletos) {
            if (pFiltro.Indica_Diaria && pFiltro.Indica_Mensal) {
                ShowAlert("Escolha se a integração de Boletos é Diária ou Mensal");
                return;
            }
            if (!pFiltro.Indica_Diaria && !pFiltro.Indica_Mensal) {
                ShowAlert("Escolha se a integração de Boletos é Diária ou Mensal");
                return;
            }
        }
        if (pFiltro.Indica_Pendentes && pFiltro.Indica_Todos) {
            ShowAlert("Escolha se quer filtrar só os Pendentes ou Todos");
            return;
        }
        if (!pFiltro.Indica_Pendentes && !pFiltro.Indica_Todos) {
            ShowAlert("Escolha se quer filtrar só os Pendentes ou Todos");
            return;
        }
        if (!pFiltro.Cod_Empresa_Faturamento) {
            ShowAlert("A Empresa de Faturamento não pode ficar em branco");
            return;
        }
        if (!pFiltro.Competencia) {
            ShowAlert("A Competência não pode ficar em branco");
            return;
        }
        //--
        var _strmes1 = pFiltro.Competencia.substr(0, 2);
        var _strano1 = pFiltro.Competencia.substr(3, 4);
        var _strcompet = _strano1 + _strmes1;
        var _intcompet = parseInt(_strcompet);
        //--
        var _dthoje = new Date();
        var _intmes2 = _dthoje.getMonth();
        var _intano2 = _dthoje.getFullYear();
        var _inthoje = parseInt(_intano2.toString() + LeftZero(_intmes2, 2));
        //--
        if (pFiltro.Indica_Boletos && pFiltro.Indica_Mensal && _intcompet >= _inthoje) {
            ShowAlert("A integração de Boletos Mensal só permite competências anteriores");
            return;
        }
        if (pFiltro.Indica_Boletos && pFiltro.Indica_Diaria && _intcompet > _inthoje) {
            ShowAlert("A integração de Boletos Diária só permite a competência atual ou anteriores");
            return;
        }

        $rootScope.routeloading = true;
        $scope.IntegrarFBs = [];
        $scope.ShowCritica1 = false;
        $scope.ShowCritica2 = false;
        $scope.ShowFiltro = false;
        $scope.ShowGrid = false;
        $('#dataTable').dataTable().fnDestroy();
        httpService.Post('CarregarFaturasBoletos', pFiltro).then(function (response) {
            if (response) {
                $scope.IntegrarFBs = response.data;
                if ($scope.IntegrarFBs.length == 0) {
                    $scope.RepeatFinished();
                }
            }
        });
    };



    //====================Processa Integração de Envio para o EMS
    $scope.ProcessarEnvioEMS = function (pFiltro, pFatBols) {
        var _valido = false;
        for (var i = 0; i < pFatBols.length; i++) {
            if (pFatBols[i].Indica_Marcado) {
                _valido = true
            };
        };
        if (!_valido) {
            ShowAlert("Nenhum Item foi Marcado.");
            return;
        };
        //--Verifica se emp.fat parametrizada
        var _url = "VerificarEmpresaParametrizada/" + pFiltro.Cod_Empresa_Faturamento;
        httpService.Get(_url).then(function (response) {
            if (!response) {
                //--Não encontrou o parâmetro
                ShowAlert("A Empresa não está parametrizada para Interface EMS");
                return;
            }
            else {
                swal({
                    title: "Confirma a integração para o EMS ?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonClass: "btn-danger",
                    confirmButtonText: "Sim, Confirmar",
                    cancelButtonText: "Cancelar",
                    closeOnConfirm: true
                }, function () {

                    //--------------------------- INTEGRAÇÃO DE FATURAS --------------------------------
                    if (pFiltro.Indica_Faturas) {
                        httpService.Post("ProcessarFaturas", pFatBols).then(function (response) {
                            if (response.data) {
                                if (response.data.Indica_Critica) {
                                    if (response.data.IsSuccessful) {
                                        $scope.FilCrit.Cod_Emp_Fat_Crit = pFiltro.Cod_Empresa_Faturamento;
                                        $scope.FilCrit.Usuario = $rootScope.UserData.Login;
                                        $scope.CarregarCriticasEMS($scope.FilCrit.Cod_Emp_Fat_Crit, $scope.FilCrit.Usuario);
                                    }
                                    else {
                                        ShowAlert(response.data.Message);
                                        return;
                                    };
                                }
                                else {
                                    if (response.data.IsSuccessful) {
                                        ShowAlert(response.data.Message);
                                        var _url = "GravarLogControle";
                                        _url += "?Cod_Empresa_Faturamento=" + pFiltro.Cod_Empresa_Faturamento;
                                        _url += "&Competencia=" + pFiltro.Competencia;
                                        _url += "&LogFatBoleto=F";
                                        _url += "&LogTipo=X";
                                        _url += "&=";
                                        httpService.Get(_url).then(function (response) {
                                            if (response.data) {
                                                //--
                                            }
                                            else {
                                                ShowAlert("Erro na gravação do Log de Controle de Integrações. Falar com a CARTV");
                                                return;
                                            }
                                        });
                                        return;

                                    }
                                    else {
                                        ShowAlert(response.data.Message);
                                        return;
                                    };
                                }

                            };
                        });
                    }
                    //--------------------------- INTEGRAÇÃO DE BOLETOS --------------------------------
                    else {
                        httpService.Post("ProcessarBoletos", pFatBols).then(function (response) {
                            if (response.data) {
                                if (response.data.Indica_Critica) {
                                    if (response.data.IsSuccessful) {
                                        $scope.FilCrit.Cod_Emp_Fat_Crit = pFiltro.Cod_Empresa_Faturamento;
                                        $scope.FilCrit.Usuario = $rootScope.UserData.Login;
                                        $scope.CarregarCriticasEMS($scope.FilCrit.Cod_Emp_Fat_Crit, $scope.FilCrit.Usuario);
                                    }
                                    else {
                                        ShowAlert(response.data.Message);
                                        return;
                                    };
                                }
                                else {
                                    if (response.data.IsSuccessful) {
                                        ShowAlert(response.data.Message);
                                        //--Atualiza o Log de Controle de Integrações
                                        var LogTipo = "";
                                        if (pFiltro.Indica_Diaria) {
                                            LogTipo = "D";
                                        }
                                        else {
                                            LogTipo = "M";
                                        }
                                        var _url = "GravarLogControle";
                                        _url += "?Cod_Empresa_Faturamento=" + pFiltro.Cod_Empresa_Faturamento;
                                        _url += "&Competencia=" + pFiltro.Competencia;
                                        _url += "&LogFatBoleto=B";
                                        _url += "&LogTipo=" + LogTipo;
                                        _url += "&=";
                                        httpService.Get(_url).then(function (response) {
                                            if (response.data) {
                                                //--
                                            }
                                            else {
                                                ShowAlert("Erro na gravação do Log de Controle de Integrações. Falar com a CARTV");
                                                return;
                                            }
                                        });
                                        return;
                                    }
                                    else {
                                        ShowAlert(response.data.Message);
                                        return;
                                    };
                                }
                            };
                        });
                    } 
                });     //------fim do swal
            };
        });
    };


    //====================Carrega Críticas do Envio para EMS
    $scope.CarregarCriticasEMS = function (pFil_CodEmp) {
        if (!pFil_CodEmp) {
            ShowAlert("Empresa de Faturamento não pode ficar vazio");
            return;
        }
        $scope.ShowCritica1 = true;
        $scope.ShowCritica2 = true;
        $scope.ShowFiltro = false;
        $scope.ShowGrid = false;
        $scope.Criticas = [];
        var _url = "CriticasEMSCarregar";
        _url += "?Cod_Emp_Fat_Crit=" + pFil_CodEmp;
        _url += "&=";
        httpService.Get(_url).then(function (response) {
            if (response) {
                $scope.Criticas = response.data;
                $scope.ShowCritica1 = true;
                $scope.ShowCritica2 = true;
            }
        });
    }


    //====================Configuracao do Grid
    $scope.ConfiguraGrid = function () {
        param = {};
        param.language = fnDataTableLanguage();
        param.lengthMenu = [[7, 10, 25, 50, -1], [7, 10, 25, 50, "Todos"]];
        param.paging = true;
        param.dom = "<'row'<'col-sm-6'B><'col-sm-3'l><'col-sm-3'f>>" + "<'row'<'col-sm-12'tr>>" + "<'row'<'col-sm-5'i><'col-sm-7'p>>",
            param.buttons = [
                {
                    text: 'Exportar<span class="fa fa-file-excel-o margin-left-10" style="color:white"></span>', type: 'excel', className: 'btn btn-warning HideButton', extend: 'excel', exportOptions: {
                        columns: ':visible:not(:first-child)'
                    }
                }
            ];
        param.order = [[1, 'asc']];
        param.autoWidth = false;
        param.columns = [];
        for (var i = 0; i < $scope.gridheaders.length; i++) {
            param.columns.push({ "visible": $scope.gridheaders[i].visible, "searchable": $scope.gridheaders[i].searchable, "sortable": $scope.gridheaders[i].sortable });
        }
        $('#dataTable').DataTable(param);
    };


    //===========================Evento chamado ao fim do ngrepeat ao carregar grid 
    $scope.$on('ngRepeatFinished', function (ngRepeatFinishedEvent) {
        $scope.RepeatFinished();
    });

    //===========================fim do load da pagina
    $scope.$watch('$viewContentLoaded', function () {
        $scope.ConfiguraGrid();
    });

}]);


