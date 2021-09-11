angular.module('App').controller('PermutasCadastroController', ['$scope', '$rootScope', '$location', 'httpService', '$location', '$routeParams', function ($scope, $rootScope, $location, httpService, $location, $routeParams) {

    //========================Recebe Parametro
    $scope.Parameters = $routeParams;

    if ($scope.Parameters.Action == 'New') {
        $rootScope.routeName = 'Inclusão de Permutas'
    }
    if ($scope.Parameters.Action == 'Edit') {
        $rootScope.routeName = 'Edição de Permutas'
    }

    $scope.Competencia_Inicio = "";
    $scope.Competencia_Fim = "";


    $scope.NewItensPermuta = function () {
        return {
            'Id_Item_Permuta': '',
            'Descricao': '',
            'Quantidade': '',
            'Valor_Unitario': '',
            'Desconto': '',
            'Vlr_Liq_Unit': '',
            'Valor_Tabela': '',
            'Valor_Liquido': ''
        }
    }


    //===========================Carrega Dados do Contrato para New ou Edit
    $scope.CarregaPermuta = function (pIdPermuta) {
        $scope.permuta = [];
        httpService.Get('Permutas/GetPermuta/' + pIdPermuta).then(function (response) {
            if (response) {
                $scope.permuta = response.data;
                $scope.permuta.Operacao = $scope.Parameters.Action;

                $scope.permuta.Editar_Vlr_Liq_Unit = false;
                $scope.permuta.Editar_Valor_Tabela = false;
                $scope.permuta.Editar_Valor_Liquido = false;


                if ($scope.Parameters.Action == 'New') {
                    $scope.permuta.Cod_Empresa_Venda = $scope.FnSetEmpresaDefault('Codigo');
                    $scope.permuta.Nome_Empresa_Venda = $scope.FnSetEmpresaDefault('Nome');
                    $scope.permuta.Cod_Empresa_Faturamento = $scope.FnSetEmpresaDefault('Codigo');
                    $scope.permuta.Nome_Empresa_Faturamento = $scope.FnSetEmpresaDefault('Nome');
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
                            $scope.permuta.Valor_Verba = $scope.Negociacao.Verba_Negociada_String;
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

    //===========================Pesquisa  de Empresas/Cliente/Agencia/Nucleo/Contato da Negociacao
    $scope.PesquisaNegociacaoTerceiro = function (pPermuta, pTabela) {
        $scope.PesquisaTabelas = NewPesquisaTabela();
        if (pPermuta.Numero_Negociacao) {
            var _url = 'Permutas/GetTerceirosNegociacao'
            _url += '?Numero_Negociacao=' + pPermuta.Numero_Negociacao;
            _url += '&Tabela=' + pTabela;
            _url += '&Codigo=';
            _url += '&';
            httpService.Get(_url).then(function (response) {
                $scope.PesquisaTabelas.Items = response.data;
            });
        };
        switch (pTabela.toLowerCase()) {
            case 'cliente':
                $scope.PesquisaTabelas.Titulo = "Pesquisa de Clientes";
                $scope.PesquisaTabelas.ClickCallBack = function (value) { pPermuta.Cod_Cliente = value.Codigo; pPermuta.Nome_Cliente = value.Descricao };
                if (!pPermuta.Numero_Negociacao) {
                    $scope.PesquisaTabelas.PreFilter = true;
                    $scope.PesquisaTabelas.LoadCallBack = function (pFilter) {
                        httpService.Get('ListarTabela/Cliente/' + pFilter).then(function (response) {
                            $scope.PesquisaTabelas.Items = response.data;
                        });
                    };
                };
                break;
            case 'agencia':
                $scope.PesquisaTabelas.Titulo = "Pesquisa de Agências";
                $scope.PesquisaTabelas.ClickCallBack = function (value) { pPermuta.Cod_Agencia = value.Codigo; pPermuta.Nome_Agencia = value.Descricao };
                if (!pPermuta.Numero_Negociacao) {
                    $scope.PesquisaTabelas.PreFilter = true;
                    $scope.PesquisaTabelas.LoadCallBack = function (pFilter) {
                        httpService.Get('ListarTabela/Agencia/' + pFilter).then(function (response) {
                            $scope.PesquisaTabelas.Items = response.data;
                        });
                    };
                };
                break;
            case 'contato':
                $scope.PesquisaTabelas.ClickCallBack = function (value) { pPermuta.Cod_Contato = value.Codigo; pPermuta.Nome_Contato = value.Descricao };
                if (!pPermuta.Numero_Negociacao) {
                    httpService.Get('ListarTabela/Contato').then(function (response) {
                        $scope.PesquisaTabelas.Items = response.data;
                    });
                };
                break;
            case 'nucleo':
                $scope.PesquisaTabelas.ClickCallBack = function (value) { pPermuta.Cod_Nucleo = value.Codigo; pPermuta.Nome_Nucleo = value.Descricao };
                $scope.PesquisaTabelas.Titulo = "Pesquisa de Nucleos";
                if (!pPermuta.Numero_Negociacao) {
                    httpService.Get('ListarTabela/Nucleo').then(function (response) {
                        $scope.PesquisaTabelas.Items = response.data;
                    });
                };
                break;
            case 'empresa_venda':
                $scope.PesquisaTabelas.Titulo = "Pesquisa de Empresas";
                if (!pPermuta.Numero_Negociacao) {
                    httpService.Get('ListarTabela/Empresa_Usuario').then(function (response) {
                        $scope.PesquisaTabelas.Items = response.data;
                    });
                }
                $scope.PesquisaTabelas.ClickCallBack = function (value) { pPermuta.Cod_Empresa_Venda = value.Codigo; pPermuta.Nome_Empresa_Venda = value.Descricao };
                break;
            case 'empresa_faturamento':
                $scope.PesquisaTabelas.Titulo = "Pesquisa de Empresas";
                if (!pPermuta.Numero_Negociacao) {
                    httpService.Get('ListarTabela/Empresa_Usuario').then(function (response) {
                        $scope.PesquisaTabelas.Items = response.data;
                    });

                };
                $scope.PesquisaTabelas.ClickCallBack = function (value) { pPermuta.Cod_Empresa_Faturamento = value.Codigo; pPermuta.Nome_Empresa_Faturamento = value.Descricao };
                break;
            default:
        };
        $("#modalTabela").modal(true);
    };
    //==============================Validar Terceiros da Negociacao - empresas/cliente/agencia/contato e nucleo
    $scope.ValidarNegociacaoTerceiro = function (pPermuta, pTabela, pField_Codigo, pField_Descricao) {
        var _Codigo = $scope.permuta[pField_Codigo].trim();
        if (_Codigo == '') {
            $scope.permuta[pField_Descricao] = "";
            return;
        }
        var _url = ""

        if (pPermuta.Numero_Negociacao) {
            var _url = 'Permutas/GetTerceirosNegociacao'
            _url += '?Numero_Negociacao=' + pPermuta.Numero_Negociacao;
            _url += '&Tabela=' + pTabela;
            _url += '&Codigo=' + _Codigo;
            _url += '&';
            httpService.Get(_url).then(function (response) {
                if (response.data.length == 0) {
                    ShowAlert("Código Inválido para essa Negociação");
                    $scope.permuta[pField_Codigo] = "";
                    $scope.permuta[pField_Descricao] = "";
                }
                else {
                    $scope.pPermuta[pField_Descricao] = response.data[0].Descricao;
                };
            });
        }
        else {
            switch (pTabela.toLowerCase()) {
                case 'cliente':
                    _url = 'ValidarTabela/Cliente/' + _Codigo;
                    break
                case 'agencia':
                    _url = 'ValidarTabela/Agencia/' + _Codigo;
                    break
                case 'contato':
                    _url = 'ValidarTabela/Contato/' + _Codigo;
                    break
                case 'nucleo':
                    _url = 'ValidarTabela/Nucleo/' + _Codigo;
                    break
                case 'empresa_venda':
                    _url = 'ValidarTabela/Empresa_Usuario/' + _Codigo;
                    break
                case 'empresa_faturamento':
                    _url = 'ValidarTabela/Empresa_Usuario/' + _Codigo;
                    break
            };

            httpService.Get(_url).then(function (response) {
                if (response.data[0].Status == 0) {
                    ShowAlert(response.data[0].Mensagem);
                    $scope.permuta[pField_Descricao] = ""
                    $scope.permuta[pField_Codigo] = ""
                }
                else {
                    $scope.permuta[pField_Descricao] = response.data[0].Descricao;
                };
            });
        }
    };
    //=====================================Calcular Valor da parcela a partir do percentual por item no modo de insercao
    $scope.CalcularItensValor_Unitario = function (pPermuta, pItem) {
        var quantidade = DoubleVal(pItem.Quantidade);
        var Valor_Unitario = DoubleVal(pItem.Valor_Unitario);
        var pct = DoubleVal(pItem.Desconto);
        pItem.Vlr_Liq_Unit = MoneyFormat(Valor_Unitario * (1-(pct / 100)));
        pItem.Valor_Tabela = MoneyFormat(Valor_Unitario * quantidade);
        pItem.Valor_Liquido = MoneyFormat((Valor_Unitario * quantidade) * (1-(pct / 100)))

    }

    //===========================Salvar Contrato
    $scope.SalvarPermuta = function (pPermuta) {

        if (pPermuta.Numero_Contrato == "") {
            ShowAlert("Numero de Contrato não Informado");
            return;
        }

        if (pPermuta.Numero_Negociacao == "") {
            ShowAlert("Numero de Negociação não Informado");
            return;
        }

        if (pPermuta.Data_Autorizacao == "") {
            ShowAlert("Data de Autorização em branco. ");
            return;
        }

        if (pPermuta.Data_Autorizacao == undefined) {
            ShowAlert("Data de Autorização não Informado");
            return;
        }

        for (var i = 0; i < pPermuta.ItensPermuta.length; i++) {
            if (pPermuta.ItensPermuta[i].Descricao == undefined) {
                ShowAlert("Descrição não Informado");
                return;
            }

            if (pPermuta.ItensPermuta[i].Quantidade == undefined) {
                ShowAlert("Quantidade não Informado");
                return;
            }

            if (pPermuta.ItensPermuta[i].Valor_Unitario == undefined) {
                ShowAlert("Valor Unitário não Informado");
                return;
            }

        };


        httpService.Post('Permutas/Salvar', pPermuta).then(function (response) {
            if (response.data[0].Status == 1) {
                ShowAlert(response.data[0].Mensagem, 'success');
                $location.path('/Permutas');
            }
            else {
                ShowAlert(response.data[0].Mensagem), 'warning';
            }
        });
    };


    //==============================Remover Permuta
    $scope.RemoverItemPermuta = function (pPermuta, pItem) {
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
                httpService.Post('ExcluirItensPermuta', pItem).then(function (response) {
                    if (response) {

                        if (response.data[0].Status) {

                            ShowAlert(response.data[0].Mensagem, 'success');
                            for (var i = 0; i < pPermuta.ItensPermuta.length; i++) {
                                if (pPermuta.ItensPermuta[i].Descricao == pItem.Descricao) {
                                    pPermuta.ItensPermuta.splice(i, 1);
                                    break;
                                }
                            }
                        }
                        else {

                            if (response.data[0].Mensagem.match(/FK/)) {
                                ShowAlert('Não é possivel excluir este registro por que o registro está  relacionado com outra tabela de dados!', 'warning');
                                return;
                            }

                            ShowAlert(response.data[0].Mensagem, 'warning');
                        }
                    }
                });
            });
        }

        for (var i = 0; i < pPermuta.ItensPermuta.length; i++) {
            if (pPermuta.ItensPermuta[i].Descricao == undefined) {
                pPermuta.ItensPermuta.splice(i, 1);
                break;
            }
        };


    };



    //===========================Adicionar Linhas de Permutas

    $scope.AdicionarPermutas = function (pPermuta) {
        $scope.permuta.ItensPermuta.push({});
    }


    //===========================Cancelar Cadastro
    $scope.CancelarPermuta = function () {
        $location.path('/Permutas');
    };



    //===========================fim do load da pagina
    $scope.$watch('$viewContentLoaded', function () {

        $scope.CarregaPermuta($scope.Parameters.Id);
        $rootScope.routeloading = false;
    });
}]);


