angular.module('App').controller('RetornoPlayListController', ['$scope', 'orderByFilter', '$rootScope', 'httpService', '$location', '$timeout', function ($scope, orderByFilter, $rootScope, httpService, $location, $timeout) {

    //------------------- Inicializa Scopes --------------------
    $scope.ShowFilter = true;
    $scope.ShowGrid = false;
    $scope.ShowAviso = false;
    $scope.currentTab = 0;
    $scope.Parametros = "";
    $scope.Veiculacoes = [];
    $scope.DownloadUrl = $rootScope.baseUrl + 'ANEXOS\\RETORNO_PLAYLIST\\';
    $scope.gridheaders = [{ 'title': '', 'visible': true, 'searchable': false, 'sortable': false, 'currentSort': 0, 'fieldName': '' },
        { 'title': '', 'visible': true, 'searchable': false, 'sortable': false,'currentSort':0 ,'fieldName':''},
   { 'title': 'Veículo', 'visible': true, 'searchable': true, 'sortable': true, 'currentSort': 0, 'fieldName': 'Cod_Veiculo' },
   { 'title': 'Data Exib', 'visible': true, 'searchable': true, 'sortable': true, 'currentSort': 0, 'fieldName': 'Data_Exibicao' },
   { 'title': 'Programa', 'visible': true, 'searchable': true, 'sortable': true, 'currentSort': 0, 'fieldName': 'Cod_Programa' },
   { 'title': 'Ch.Acesso', 'visible': true, 'searchable': true, 'sortable': true, 'currentSort': 0, 'fieldName': 'Chave_Acesso' },
   { 'title': 'Fita', 'visible': true, 'searchable': true, 'sortable': true, 'currentSort': 0, 'fieldName': 'Numero_Fita' },
   { 'title': 'Título', 'visible': true, 'searchable': true, 'sortable': true, 'currentSort': 0, 'fieldName': 'Titulo_Comercial' },
   { 'title': 'Qual', 'visible': true, 'searchable': true, 'sortable': true, 'currentSort': 0, 'fieldName': 'Cod_Qualidade' },
   { 'title': 'Horário', 'visible': true, 'searchable': true, 'sortable': true, 'currentSort': 0, 'fieldName': 'Horario_Exibicao' },
   { 'title': 'C.E', 'visible': true, 'searchable': true, 'sortable': true, 'currentSort': 0, 'fieldName': 'Numero_Ce' },
   { 'title': 'Baixado', 'visible': true, 'searchable': true, 'sortable': true, 'currentSort': 0, 'fieldName': '' },
    { 'title': 'Dur.Sctv', 'visible': true, 'searchable': true, 'sortable': true, 'currentSort': 0, 'fieldName': 'Duracao' },
    { 'title': 'Dur.Exibidor', 'visible': true, 'searchable': true, 'sortable': true, 'currentSort': 0, 'fieldName': 'Duracao_Exibidor' },
    { 'title': 'Dif', 'visible': true, 'searchable': true, 'sortable': true, 'currentSort': 0, 'fieldName': 'Diferenca' },
    { 'title': 'Perc', 'visible': true, 'searchable': true, 'sortable': true, 'currentSort': 0, 'fieldName': 'Pct_Exibido' },
    ];
    $scope.orderBy = "Horario_Exibicao";
    $scope.pdfFieldSort = "Horario_Exibicao";
    $scope.pdfTypeSort = "asc";

    $scope.sorticon = ['fa fa-sort','fa fa-sort-desc','fa fa-sort-asc']
    //------------------- Novo Filtro ------------------------
    $scope.NewFilter = function () {
        $scope.Parametros = {
            'Cod_Veiculo': '',
            'Nome_Veiculo': '',
            'Data_Exibicao': '',
            'Arquivo': "",
            'Indica_Fitas_Avulsas': true,
            'Indica_Fitas_Artisticas': true,
            'Data_Inicio': '',
            'Hora_Inicio': '',
            'Data_Fim': '',
            'Hora_Fim': '',
            'Horario_Emissora': '',
            'Sistema_Exibicao': '',
            'Tipo_Arquivo': '',
            'Nome_Tabela': '',
            'Anexos': [],
        };
    };
    $scope.NewFilter();

    //====================Quando terminar carga do grid, torna view do grid visible
    //$scope.RepeatFinished = function () {
    //    $rootScope.routeloading = false;
    //    $scope.ConfiguraGrid();
    //    setTimeout(function () {
    //        $("#dataTable").dataTable().fnAdjustColumnSizing();
    //    }, 10)
    //};
    //------------------- Carrega os Dados ------------------------
    $scope.RetornoPlayListCarregaDados = function (pParam) {
        //-----Só carrega os dados após digitação de Veiculo e Data Exibição
        if (!pParam.Cod_Veiculo || !pParam.Data_Exibicao) {
            return;
        }
        //-----Faz Consulta
        $scope.Parametros.Horario_Emissora = "";
        $scope.Parametros.Sistema_Exibicao = "";
        var _url = "RetornoPlayListDados";
        _url += "?Cod_Veiculo=" + pParam.Cod_Veiculo;
        _url += "&Nome_Veiculo=" + pParam.Nome_Veiculo;
        _url += "&Data_Exibicao=" + pParam.Data_Exibicao;
        _url += "&=";
        httpService.Get(_url).then(function (response) {
            if (response) {
                $scope.Parametros = response.data;
                $scope.SortClick($scope.gridheaders[9]);
            }
        });
    };


    //-------Quando mudar veiculo ou data exibição, carrega os dados---------------- 
    $scope.$watch('[Parametros.Cod_Veiculo,Parametros.Data_Exibicao]', function (newValue, oldValue) {
        if (newValue != oldValue) {
            $scope.RetornoPlayListCarregaDados($scope.Parametros)
        }
    });

    //-------------Importar Arquivo------------------------------
    $scope.ImportarArquivo = function (pParam) {
        $scope.ShowAviso = false;
        $scope.currentTab = 0;

        for (var i = 0; i < pParam.Anexos.length; i++) {
            for (var z = 1; z < pParam.Anexos.length; z++) {
                if (pParam.Anexos[i].AnexoName == pParam.Anexos[z].AnexoName && i != z) {
                    ShowAlert('O Arquivo ' + pParam.Anexos[i].AnexoName + ' foi selecionado mais de uma vez.');
                    return;
                };
            };
        };

        httpService.Post("RetornoPlayListConsiste", pParam).then(function (response) {
            if (response) {
                if (response.data[0].Status == 0) {
                    ShowAlert(response.data[0].Mensagem);
                }
                else {
                    $('#dataTable').dataTable().fnDestroy();
                    httpService.Post("RetornoPlayListProcessa", pParam).then(function (response) {
                        $scope.Veiculacoes = response.data;
                        if ($scope.Veiculacoes.length == 0) {
                            $scope.RepeatFinished();
                        }
                        $scope.Parametros.Anexos = [];
                        if ($scope.Veiculacoes.length == 0) {
                            $scope.ShowAviso = true;
                        }
                        else {
                            $scope.ShowGrid = true;
                            $scope.ShowFilter = false;
                        };
                    });
                };
            };
        });
    };
    //====================Funcao para configurar o Grid
    //$scope.ConfiguraGrid = function () {
    //    param = {};
    //    param.language = fnDataTableLanguage();
    //    param.lengthMenu = [[7, 10, 25, 50, -1], [7, 10, 25, 50, "Todos"]];
    //    param.pageLength = 7;
    //    param.scrollCollapse = true;
    //    param.paging = true;

    //    param.dom = "<'row'<'col-sm-3'l><'col-sm-5'B>>" + "<'row'<'col-sm-12'tr>>" + "<'row'<'col-sm-5'i><'col-sm-7'p>>",
    //        param.buttons = [
    //            {
    //                text: 'Exportar<span class="fa fa-file-excel-o margin-left-10" style="color:white"></span>', type: 'excel', className: 'btn btn-warning HideButton', extend: 'excel', exportOptions: {
    //                    columns: ':visible:not(:first-child)'
    //                }
    //            }
    //        ];
    //    param.order = [[0, 'asc']];
    //    param.autoWidth = false;
    //    param.columns = [];
    //    for (var i = 0; i < $scope.gridheaders.length; i++) {
    //        param.columns.push({ "visible": $scope.gridheaders[i].visible, "searchable": $scope.gridheaders[i].searchable, "sortable": $scope.gridheaders[i].sortable });
    //    }
    //    $('#dataTable').DataTable(param);
    //};
    ////-------------Processar Baixar------------------------------
    $scope.ProcessarBaixa = function (pVeiculacoes) {
        swal({
            title: "Essa Operação irá baixar todas as Veiculações em todas as Pastas. Confirma ? ",
            //type: "warning",
            showCancelButton: true,
            confirmButtonClass: "btn-danger",
            confirmButtonText: "Sim,Baixar",
            cancelButtonText: "Não,Cancelar",
            closeOnConfirm: true
        }, function () {
            httpService.Post('RetornoPlayListBaixa', pVeiculacoes).then(function (response) {
                $scope.Veiculacoes = response.data;
            });
        });
    };
    //====================Add Anexos
    $scope.AddAnexo = function (value) {
        $scope.Parametros.Anexos.push({ 'Url': $scope.DownloadUrl, 'AnexoName': value });
        $("#modalUpload").modal('hide');
    };
    //====================Remove Anexos
    $scope.RemoveAnexo = function (file) {
        swal({
            title: "Tem certeza que deseja remover  esse Arquivo ?",
            //type: "warning",
            showCancelButton: true,
            confirmButtonClass: "btn-danger",
            confirmButtonText: "Sim,Excluir",
            cancelButtonText: "Não,Cancelar",
            closeOnConfirm: true
        }, function () {
            for (var i = 0; i < $scope.Parametros.Anexos.length; i++) {
                if ($scope.Parametros.Anexos[i] === file) {
                    $scope.Parametros.Anexos.splice(i, 1);
                    httpService.Post("RetornoPlayListRemoveAnexo", file).then(function (response) {
                    });
                }
            }
        });
    }
    //===============Clicou na lupa de qualidade
    $scope.PesquisaQualidade = function (pQualidade) {

        $scope.PesquisaTabelas = NewPesquisaTabela();
        httpService.Get('ListarTabela/Qualidade').then(function (response) {
            if (response.data) {
                $scope.PesquisaTabelas.Items = response.data;
                $scope.PesquisaTabelas.FiltroTexto = "";
                $scope.PesquisaTabelas.Titulo = "Seleção de Qualidade";
                $scope.PesquisaTabelas.MultiSelect = false;
                $scope.PesquisaTabelas.ClickCallBack = function (value) { pQualidade.Cod_Qualidade = value.Codigo; pQualidade.Descricao = value.Descricao };
                $("#modalTabela").modal(true);
            }
        });
    };
    //===========================Validar Código de Qualidade
    $scope.ValidarQualidade = function (pCodQualidade) {
        if (pCodQualidade.Cod_Qualidade == "") {
            return;
        };
        if (pCodQualidade.Cod_Qualidade.toUpperCase() == 'VEI') {
            return;
        }
        if (pCodQualidade.Cod_Qualidade == pCodQualidade.Cod_Qualidade_Ant) {
            return;
        }
        httpService.Post('BaixaVeiculacoes/ValidarQualidade', pCodQualidade).then(function (response) {
            if (response.data) {
                if (response.data[0].Status == 0) {
                    ShowAlert(response.data[0].Mensagem)
                    pCodQualidade.Cod_Qualidade = pCodQualidade.Cod_Qualidade_Ant;
                    pCodQualidade.Horario_Exibicao = "";
                    return;
                }
                if (response.data[0].Critica == 1) {
                    pCodQualidade.Cod_Qualidade = pCodQualidade.Cod_Qualidade_Ant
                    return;
                };
            };
        });
    };
    //======================Validar Horario de exibicao
    $scope.ValidarHorario = function (pVeiculacao) {
        if (pVeiculacao.Horario_Exibicao == "") {
            return;
        };
        var hora = parseInt(Left(pVeiculacao.Horario_Exibicao, 2));
        var minuto = parseInt(Right(pVeiculacao.Horario_Exibicao, 2))
        if (hora < 0 || hora > 23 || minuto < 0 || minuto > 59) {
            ShowAlert("Horário Inválido")
            pVeiculacao.Horario_Exibicao = ""
        }
        else {
            pVeiculacao.Horario_Exibicao = Left(pVeiculacao.Horario_Exibicao, 2) + ':' + Right(pVeiculacao.Horario_Exibicao, 2);
        }
    };

    //===========================Imprimir retorno pdf
    $scope.GerarPdf = function (pVeiculacao, pStatus) {
        var _data = [];
        for (var i = 0; i < pVeiculacao.length; i++) {
            if (pVeiculacao[i].Status == pStatus) {
                _data.push(pVeiculacao[i]);
            };
        };
        if (_data.length == 0) {
            ShowAlert("Não ha dados a ser Impresso.")
            return;
        }
        _data[0].SortOrder = $scope.pdfFieldSort;
        _data[0].SortType = $scope.pdfTypeSort;
        
        httpService.Post("RetornoPlayGerarPdf/", _data).then(function (response) {
            if (response.data) {
                url = $rootScope.baseUrl + "PDFFILES/RETORNOPLAYLIST/" + $rootScope.UserData.Login.trim() + "/" + response.data;
                var win = window.open(url, '_blank');
                win.focus();
            }
            else {
                ShowAlert("Não há dados a ser impresso.")
            }
        });
    };

    //===========================Fecha a Conciliacao
    $scope.FechaConciliacao = function()
    {
        $scope.Veiculacoes = [];
        $('#dataTable').dataTable().fnDestroy();
        $scope.ShowGrid = false;
        $scope.ShowFilter = true;
    }

    //===========================Sort Column
    $scope.SortClick = function (pHeader) {
        for (var i = 0; i < $scope.gridheaders.length; i++) {
            if (pHeader.title != $scope.gridheaders[i].title) {
                $scope.gridheaders[i].currentSort = 0;
            }     
        };
        pHeader.currentSort++
        if (pHeader.currentSort > 2) {
            pHeader.currentSort = 1
        };
        //$scope.orderBy = (pHeader.currentSort == 1 ? '' : '-') + pHeader.fieldName;
        $scope.pdfFieldSort = pHeader.fieldName;
        $scope.pdfTypeSort = (pHeader.currentSort == 1 ? 'ASC' : 'DESC')

        var _reverse = false;
        if ($scope.pdfTypeSort.toUpperCase() == 'DESC') {
            _reverse = true;
        };
        angular.forEach(orderByFilter($scope.Veiculacoes, $scope.pdfFieldSort, _reverse), function (value, key) {
            value["Chave"] = value[$scope.pdfFieldSort];
        });
        $scope.orderBy = (pHeader.currentSort == 1 ? '' : '-') + 'Chave';
    }
    //===========================Procurar Fita no Roteiro
    $scope.ProcurarRoteiro = function (pFiltro) {
        var _url = 'ConsultaFitasOrdenadasListar';
        _url += '?Cod_Veiculo=' + $scope.Parametros.Cod_Veiculo;
        _url += '&Data_Inicio=' + $scope.Parametros.Data_Exibicao;
        _url += '&Data_Fim=' + $scope.Parametros.Data_Exibicao;
        _url += '&Numero_Fita_Inicio=' + pFiltro.Numero_Fita;
        _url += '&Numero_Fita_Fim=' + pFiltro.Numero_Fita;
        _url += '&';
        httpService.Get(_url).then(function (response) {
            if (response) {
                if (response.data.length > 0) {
                    $scope.ConsultaFitasOrdenadaS = response.data;
                    $("#modaFitaOrdenada").modal(true);
                }
                else {
                    ShowAlert("Fita não encontrada no Roteiro.")
                }
            };
        });
    }
    //===========================Evento chamado ao fim do ngrepeat ao carregar grid 
    //$scope.$on('ngRepeatFinished', function (ngRepeatFinishedEvent) {
    //    $scope.RepeatFinished();
    //});
    //===========================fim do load da pagina
    //$scope.$watch('$viewContentLoaded', function () {
    //    $scope.ConfiguraGrid();
    //});
}]);



