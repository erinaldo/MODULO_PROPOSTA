angular.module('App').controller('PermutasEntregaCadastroController', ['$scope', '$rootScope', '$location', 'httpService', '$location', '$routeParams', function ($scope, $rootScope, $location, httpService, $location, $routeParams) {

    //========================Recebe Parametro
    $scope.Parameters = $routeParams;

    if ($scope.Parameters.Action == 'New') {
        $rootScope.routeName = 'Inclusão de Entrega Permutas'
    }
    if ($scope.Parameters.Action == 'Edit') {
        $rootScope.routeName = 'Edição de Entrega Permutas'
    }

    $scope.Competencia_Inicio = "";
    $scope.Competencia_Fim = "";


    $scope.NewItensEntregaPermuta = function () {
        return {
            'Id_Permuta': '',
            'Numero_Nota_Fiscal': '',
            'Serie': '',
            'Descricao': '',
            'Quantidade': '',
            'Valor_Liq_Unitario': '',
            'Vlr_Liq_Total': '',
            'Data_Emissao': '',

        }
    }


    //===========================Carrega Dados do Contrato e Entrega para New ou Edit
           
    $scope.CarregaEntregaPermuta = function (pIdPermuta) {
        //$scope.permuta = [];
        httpService.Get('Permutas/GetEntregaPermuta/' + pIdPermuta).then(function (response) {
            if (response) {
                $scope.permuta = response.data;
               
                //for (var i = 0; i < $scope.permuta.ItensEntregaPermuta.length; i++) {
                //    $scope.permuta.ItensEntregaPermuta[i].Numero_Nota = response.data.ItensEntregaPermuta[i].Numero_Nota_Fiscal;

                //}
                $scope.permuta.Operacao = $scope.Parameters.Action;

                $scope.permuta.Editar_Numero_Contrato   = false;
                $scope.permuta.Editar_Numero_Negociacao = false;
                $scope.permuta.Editar_Vlr_Liq_Total     = false;
                $scope.permuta.Editar_Data_Emissao = false;
                $scope.permuta.Editar_Data_Autorizacao = false;
                $scope.PermissaoDescricao = false;

                if ($scope.Parameters.Action == 'New') {
                    $scope.permuta.Cod_Empresa_Venda       = $scope.FnSetEmpresaDefault('Codigo');
                    $scope.permuta.Nome_Empresa_Venda      = $scope.FnSetEmpresaDefault('Nome');
                    $scope.permuta.Cod_Empresa_Faturamento = $scope.FnSetEmpresaDefault('Codigo');
                    $scope.permuta.Nome_Empresa_Faturamento= $scope.FnSetEmpresaDefault('Nome');
                }
                if ($scope.permuta.Numero_Contrato == 0) {
                    $scope.permuta.Numero_Contrato = "";
                };
                if ($scope.permuta.Numero_Negociacao == 0) {
                    $scope.permuta.Numero_Negociacao = "";
                };
                if ($scope.permuta.Valor_Verba == 0) {
                    $scope.permuta.Valor_Verba = "";
                };

            };
        });
    };

    //===========================Mudou o Numero da Negociacao
    $scope.NegociacaoChange = function (pNegociacao) {
        if (!pNegociacao) {
            $scope.permuta.Cod_Tipo_Midia = "";
            return;
        }
        httpService.Post('Permutas/ValidarNegociacao', $scope.permuta).then(function (response) {
            if (response) {
                if (response.data[0].Status == false) {
                    ShowAlert(response.data[0].Mensagem)
                    $scope.permuta.Numero_Negociacao = "";
                    $scope.permuta.Numero_Contrato = "";
                    $scope.permuta.Cod_Tipo_Midia = "";
                    $scope.permuta.Cod_Empresa_Venda = "";
                    $scope.permuta.Nome_Empresa_Venda = "";
                    $scope.permuta.Cod_Empresa_Faturamento = "";
                    $scope.permuta.Nome_Empresa_Faturamento = "";
                    $scope.permuta.Cod_Cliente = "";
                    $scope.permuta.Nome_Cliente = "";
                    $scope.permuta.Cod_Agencia = "";
                    $scope.permuta.Nome_Agencia = "";
                    $scope.permuta.Cod_Nucleo = "";
                    $scope.permuta.Nome_Nucleo = "";
                    $scope.permuta.Cod_Contato = "";
                    $scope.permuta.Nome_Contato = "";

                }
                else {

                    httpService.Get('Negociacao/Get/?Numero_Negociacao=' + pNegociacao + '&').then(function (responseNegociacao) {
                        if (responseNegociacao) {
                            $scope.Negociacao = responseNegociacao.data;
                            $scope.permuta.Cod_Tipo_Midia = $scope.Negociacao.Cod_Tipo_Midia;
                            $scope.permuta.Editar_Tipo_Midia = false;

                            if ($scope.Negociacao.Nucleos.length == 1) {
                                $scope.permuta.Cod_Nucleo = $scope.Negociacao.Nucleos[0].Cod_Nucleo;
                                $scope.permuta.Nome_Nucleo = $scope.Negociacao.Nucleos[0].Nome_Nucleo;
                                if ($scope.Parameters.Action == 'New') {
                                    $scope.permuta.Editar_Nucleo = false;
                                }
                            };


                            $scope.permuta.Competencia_Inicial = $scope.Negociacao.Competencia_Inicial;
                            $scope.permuta.Competencia_Final = $scope.Negociacao.Competencia_Final;
                            $scope.permuta.Editar_Valor_Verba = false;
                            $scope.permuta.Editar_Competencia_Inicial = false;
                            $scope.permuta.Editar_Competencia_Final = false;
                            $scope.permuta.Editar_Id_Item_Permuta = false;

                            if ($scope.Negociacao.Empresas_Faturamento.length == 1) {
                                $scope.permuta.Cod_Empresa_Faturamento = $scope.Negociacao.Empresas_Faturamento[0].Cod_Empresa;
                                $scope.permuta.Nome_Empresa_Faturamento = $scope.Negociacao.Empresas_Faturamento[0].Nome_Empresa;
                                if ($scope.Parameters.Action == 'New') {
                                    $scope.permuta.Editar_Empresa_Faturamento = false;
                                }
                            };
                            if ($scope.Negociacao.Empresas_Venda.length == 1) {
                                $scope.permuta.Cod_Empresa_Venda = $scope.Negociacao.Empresas_Venda[0].Cod_Empresa;
                                $scope.permuta.Nome_Empresa_Venda = $scope.Negociacao.Empresas_Venda[0].Nome_Empresa;
                                if ($scope.Parameters.Action == 'New') {
                                    $scope.permuta.Editar_Empresa_Venda = false;
                                }
                            };
                            if ($scope.Negociacao.Clientes.length == 1) {
                                $scope.permuta.Cod_Cliente = $scope.Negociacao.Clientes[0].Cod_Cliente;
                                $scope.permuta.Nome_Cliente = $scope.Negociacao.Clientes[0].Nome_Cliente;
                                if ($scope.Parameters.Action == 'New') {
                                    $scope.permuta.Editar_Cliente = false;
                                }
                            };
                            if ($scope.Negociacao.Agencias.length == 1) {
                                $scope.permuta.Cod_Agencia = $scope.Negociacao.Agencias[0].Cod_Agencia;
                                $scope.permuta.Nome_Agencia = $scope.Negociacao.Agencias[0].Nome_Agencia;
                                if ($scope.Parameters.Action == 'New') {
                                    $scope.permuta.Editar_Agencia = false;
                                }
                            };
                            if ($scope.Negociacao.Contatos.length == 1) {
                                $scope.permuta.Cod_Contato = $scope.Negociacao.Contatos[0].Cod_Contato;
                                $scope.permuta.Nome_Contato = $scope.Negociacao.Contatos[0].Nome_Contato;
                                if ($scope.Parameters.Action == 'New') {
                                    $scope.permuta.Editar_Contato = false;
                                }
                            };

                        }
                    });
                }
            }
        });
    }

    //===========================Adicionar Linhas de Entrega Permutas

    $scope.AdicionarEntregaPermutas = function (pPermuta) {
        $scope.permuta.ItensEntregaPermuta.push({});
    }

    //==============================Remover Entrega Permuta
    $scope.RemoverItemEntregaPermuta = function (pPermuta, pItem) {
        if (pItem.Descricao != undefined) {

            swal({
                title: "Tem certeza que deseja Excluir esse Itens de Permuta?",
                type: "warning",
                showCancelButton: true,
                confirmButtonClass: "btn-danger",
                confirmButtonText: "Sim,Excluir",
                cancelButtonText: "Cancelar",
                closeOnConfirm: true


            }, function () {
                httpService.Post('ExcluirItensEntregaPermuta', pItem).then(function (response) {
                    if (response) {

                        if (response.data[0].Status) {

                            ShowAlert(response.data[0].Mensagem, 'success');
                            for (var i = 0; i < pPermuta.ItensEntregaPermuta.length; i++) {
                                if (pPermuta.ItensEntregaPermuta[i].Numero_Nota_Fiscal == pItem.Numero_Nota_Fiscal) {
                                    pPermuta.ItensEntregaPermuta.splice(i, 1);
                                    break;
                                }
                            }
                        }
                        else {

                            ShowAlert(response.data[0].Mensagem, 'warning');
                        }
                    }
                });
            });
        }

        for (var i = 0; i < pPermuta.ItensEntregaPermuta.length; i++) {
            if (pPermuta.ItensEntregaPermuta[i].Numero_Nota_Fiscal == undefined) {
                pPermuta.ItensEntregaPermuta.splice(i, 1);
                break;
            }
        };


    };


    //===========================Cancelar Cadastro
    $scope.CancelarEntregaPermuta = function () {
        $location.path('/Permutas');
    };


    //===========================Salvar Contrato
    $scope.SalvarEntregaPermuta = function (pPermuta) {



        for (var i = 0; i < pPermuta.ItensEntregaPermuta.length; i++) {

            if (pPermuta.ItensEntregaPermuta[i].Id_Item_Permuta == undefined || pPermuta.ItensEntregaPermuta[i].Id_Item_Permuta == "" ) {
                ShowAlert("Código do item não Informado");
                return;
                break;
            }

            if (pPermuta.ItensEntregaPermuta[i].Data_Emissao == undefined) {
                ShowAlert("Data Emissao em branco");
                return;
                break;
            }


            if (pPermuta.ItensEntregaPermuta[i].Numero_Nota_Fiscal == "") {
                ShowAlert("Numero Nota fiscal  em branco!"); 
                return;
                break;
            }

            if (pPermuta.ItensEntregaPermuta[i].Serie == "") {
                ShowAlert("Série em branco!");
                return;
                break;
            }


            if (pPermuta.ItensEntregaPermuta[i].Quantidade == "") {
                ShowAlert("Quantidade em branco!");
                return;
                break;
            }

            if (pPermuta.ItensEntregaPermuta[i].Valor_Liq_Unitario == "") {
                ShowAlert("Valor Liquido Unitario em branco!");
                return;
                break;
            }

            if (pPermuta.ItensEntregaPermuta[i].Data_Emissao == "") {
                ShowAlert("Data Emissão em branco!");
                return;
                break;
            }

           

        };


        httpService.Post('Permutas/SalvarEntrega', pPermuta).then(function (response) {
                                 
            if (response.data[0].Status == 1) {
                ShowAlert(response.data[0].Mensagem, 'success');
                $location.path('/Permutas');
            }
            else {
                ShowAlert(response.data[0].Mensagem), 'warning';
            }
        });
    };

    //=====================================Calcular Valor da parcela a partir do percentual por item no modo de insercao
    $scope.CalcularEntrega = function (pItem) {
        var quantidade = DoubleVal(pItem.Quantidade);
        var Valor_Liq_Unitario = DoubleVal(pItem.Valor_Liq_Unitario);
        pItem.Vlr_Liq_Total = MoneyFormat(Valor_Liq_Unitario * quantidade );
    }



    //======================Validar Item Permuta
    $scope.ValidarItemPermuta = function (pPermuta, pItensPermuta) {
        pPermuta.Id_Item_Permuta = pItensPermuta.Id_Item_Permuta;
    httpService.Post('Permutas/ValidarItemPermuta', pPermuta).then(function (response) {
            if (response.data[0].Status == 0) {
                ShowAlert(response.data[0].Mensagem);
                pItensPermuta.Id_Item_Permuta = "";
                return;
            }
            else {

                pItensPermuta.Descricao = response.data[0].Descricao;
                pItensPermuta.Quantidade = response.data[0].Quantidade;
                pItensPermuta.Valor_Liq_Unitario = MoneyFormat(response.data[0].Valor_Unitario);
                pItensPermuta.Vlr_Liq_Total = response.data[0].Quantidade * response.data[0].Valor_Unitario;
                pItensPermuta.Vlr_Liq_Total = MoneyFormat(pItensPermuta.Vlr_Liq_Total);

            }
        });

    };

    //=======================Selecao de Itens de Permuta
    $scope.PesquisaItemPermuta = function (pPermuta,pItensPermuta) {
        $scope.PesquisaTabelas = NewPesquisaTabela();
        $scope.listaTipoComercial = ""
        httpService.Post('Permutas/PesquisarItemPermuta', pPermuta).then(function (response) {
            $scope.PesquisaTabelas.Items = response.data;
            $scope.PesquisaTabelas.FiltroTexto = ""
            $scope.PesquisaTabelas.PreFilter = false;
            $scope.PesquisaTabelas.Titulo = "Seleção de Itens de Permuta"
            $scope.PesquisaTabelas.MultiSelect = false;
            $scope.PesquisaTabelas.ClickCallBack = function (value)
            {
                pItensPermuta.Id_Item_Permuta = value.Codigo;

                for (var i = 0; i < pPermuta.ItensEntregaPermuta.length; i++) {
                    if (pPermuta.ItensEntregaPermuta[i].Id_Item_Permuta == pItensPermuta.Id_Item_Permuta && pPermuta.ItensEntregaPermuta[i].Numero_Nota_Fiscal != pPermuta.Numero_Nota_Fiscal) {

                        ShowAlert("Já existe código adicionado!");
                        return;
                    }
                };

                pItensPermuta.Descricao = value.Descricao;
                pItensPermuta.Quantidade = value.Quantidade;
                pItensPermuta.Valor_Liq_Unitario = MoneyFormat(value.Valor_Unitario);
                pItensPermuta.Vlr_Liq_Total = value.Quantidade * value.Valor_Unitario;
                pItensPermuta.Vlr_Liq_Total = MoneyFormat(pItensPermuta.Vlr_Liq_Total);

            };
            $scope.PesquisaTabelas.LoadCallBack = function (pFilter) {
                httpService.Get('ListarTabela/Tb_Proposta_Item_Permuta/' + pFilter).then(function (response) {
                    $scope.PesquisaTabelas.Items = response.data;
                });
            }
                $("#modalTabela").modal(true);
        });
    };


    //===========================fim do load da pagina
    $scope.$watch('$viewContentLoaded', function () {

        $scope.CarregaEntregaPermuta($scope.Parameters.Id);
        $rootScope.routeloading = false;
    });
}]);


