angular.module('App').controller('DeterminacaoController', ['$scope', '$rootScope', '$location', 'httpService', '$location', function ($scope, $rootScope, $location, httpService, $location) {

    //============================Inicializar Scopes
    $scope.NewFilter = function () {
        return { 'Cod_Empresa': '', 'Numero_Mr': '', 'Sequencia_Mr': '', 'Cod_Veiculo': '' }
    }
    $scope.Filtro = $scope.NewFilter();
    $scope.NewComercial = function () {
        return {
            'Cod_Empresa': '',
            'Numero_Mr': '',
            'Sequencia_Mr': '',
            'Cod_Comercial': '',
            'Titulo_Comercial': '',
            'Duracao': '',
            'Cod_Tipo_Comercial': '',
            'Nome_Tipo_Comercial': '',
            'Cod_Red_Produto': '',
            'Nome_Produto': '',
            'Observacoes': '',
            'Indica_Titulo_Determinar': false
        };
    };
    $scope.NewDeterminacaoVei = function () {
        return {
            'Data_Inicio': '',
            'Data_Fim': '',
            'Cod_Programa': '',
            'Nome_Programa': '',
            'Cod_Comercial': '',
            'Titulo_Comercial': '',
            'Duracao': '',
            'Cod_Tipo_Comercial': '',
            'Nome_Tipo_Comercial': '',
            'ComercialPara': [{ 'Cod_Comercial': '', 'Titulo_Comercial': '', 'Cod_Tipo_Comercial': '', 'Nome_Tipo_Comercial': '', 'Duracao': '' }]
        };
    };
    $scope.CurrentTab = ''
    $scope.Contrato = "";
    $scope.DeterminacaoVei = "";
    $scope.ShowFilter = true;
    $scope.ShowComercial = false;
    $scope.ShowDados = true;
    $scope.ShowAnalise = false;
    $scope.VeiculosAnalise = [];
    $scope.Cod_Veiculo_Analise = "";
    $scope.Analise = "";
    //============================Carregar Dados
    $scope.CarregarComerciais = function (pFiltro) {
        httpService.Post("Determinacao/CarregarDados", pFiltro).then(function (response) {
            if (response.data) {
                if (!response.data.Id_Contrato) {
                    ShowAlert("Não existem dados para esse filtro")
                    return;
                }
                if (response.data.Veiculacoes.length == 0) {
                    ShowAlert("Não existem veiculações a ser determinada para esse Contrato.")
                    return;
                }
                $scope.Contrato = response.data;
                $scope.DeterminacaoVei = $scope.NewDeterminacaoVei();
                $scope.DeterminacaoVei.Data_Inicio = $scope.Contrato.Data_Inicio,
                $scope.DeterminacaoVei.Data_Fim = $scope.Contrato.Data_Fim,
                $scope.CurrentTab = 'Comercial'
                $scope.Filtro.Cod_Veiculo = $scope.Contrato.Veiculacoes[0].Cod_Veiculo;
            };
        });
    };
    //===============Carregar Veiculacao 
    $scope.CarregarVeiculacao = function (pContrato, pFiltro) {
        httpService.Post("Determinacao/CarregarVeiculacao", pFiltro).then(function (response) {
            if (response.data) {
                pContrato.Veiculacoes = response.data
            };
        });
    };
    //===============Cancelar a Determinacao
    $scope.CancelarDeterminacao = function () {
        $scope.Filtro = $scope.NewFilter();
        $scope.CurrentTab = '';
        $scope.Contrato = "";
        $scope.ShowFilter = true;
        $scope.ShowAnalise = false;
        $scope.ShowDados = true;
        $scope.Analise = "";

    };
    //===============Selecao de Programas
    $scope.SelecionarProgramas = function (pDeterminacao) {
        if (!pDeterminacao) {
            return;
        }
        $scope.PesquisaTabelas = NewPesquisaTabela();
        $scope.PesquisaTabelas.Items = pDeterminacao.Programas;
        $scope.PesquisaTabelas.Titulo = "Seleção de Programas"
        $scope.PesquisaTabelas.ClickCallBack = function (value) {
            $scope.DeterminacaoVei.Cod_Programa = value.Codigo;
            $scope.DeterminacaoVei.Nome_Programa = value.Descricao;
        };
        $scope.PesquisaTabelas.MultiSelect = false;
        $("#modalTabela").modal(true);
    };
    //===============Validar programa
    $scope.ValidarPrograma = function (pVeiculacao) {
        if (!pVeiculacao.Cod_Programa) {
            return;
        }
        var _valido = false;
        for (var i = 0; i < $scope.Contrato.Programas.length; i++) {
            if ($scope.Contrato.Programas[i].Codigo.trim().toUpperCase() == pVeiculacao.Cod_Programa.trim().toUpperCase()) {
                _valido = true;
                break;
            };
        };
        if (!_valido) {
            ShowAlert("Não existe esse Programa no Contrato.");
            pVeiculacao.Cod_Programa = "";

        }
    };
    //===============Selecao de Comerciais
    $scope.SelecionarComercial = function (pComercial) {
        if (!$scope.Contrato) {
            return;
        }
        var _ListaComerciais = [];
        for (var i = 0; i < $scope.Contrato.Comerciais.length; i++) {
            _ListaComerciais.push({ 'Codigo': $scope.Contrato.Comerciais[i].Cod_Comercial, 'Descricao': $scope.Contrato.Comerciais[i].Titulo_Comercial })
        }
        $scope.PesquisaTabelas = NewPesquisaTabela();
        $scope.PesquisaTabelas.Items = _ListaComerciais;
        $scope.PesquisaTabelas.Titulo = "Seleção de Comerciais"
        $scope.PesquisaTabelas.ClickCallBack = function (value) {
            pComercial.Cod_Comercial = value.Codigo;
            pComercial.Titulo_Comercial = value.Descricao;
            for (var i = 0; i < $scope.Contrato.Comerciais.length; i++) {
                if ($scope.Contrato.Comerciais[i].Cod_Comercial.trim().toUpperCase() == value.Codigo.trim().toUpperCase()) {
                    pComercial.Cod_Tipo_Comercial = $scope.Contrato.Comerciais[i].Cod_Tipo_Comercial;
                    pComercial.Nome_Tipo_Comercial = $scope.Contrato.Comerciais[i].Nome_Tipo_Comercial;
                    pComercial.Duracao = $scope.Contrato.Comerciais[i].Duracao;
                    break;
                };
            };
        };
        $scope.PesquisaTabelas.MultiSelect = false;
        $("#modalTabela").modal(true);
    };
    //===============Validar Comercial
    $scope.ValidarComercial = function (pVeiculacao) {
        if (!pVeiculacao.Cod_Comercial) {
            return;
        }
        var _valido = false;
        for (var i = 0; i < $scope.Contrato.Comerciais.length; i++) {
            if ($scope.Contrato.Comerciais[i].Cod_Comercial.trim().toUpperCase() == pVeiculacao.Cod_Comercial.trim().toUpperCase()) {
                pVeiculacao.Titulo_Comercial = $scope.Contrato.Comerciais[i].Titulo_Comercial;
                pVeiculacao.Cod_Tipo_Comercial = $scope.Contrato.Comerciais[i].Cod_Tipo_Comercial;
                pVeiculacao.Nome_Tipo_Comercial = $scope.Contrato.Comerciais[i].Nome_Tipo_Comercial;
                pVeiculacao.Duracao = $scope.Contrato.Comerciais[i].Duracao;
                _valido = true;
                break;
            };
        };
        if (!_valido) {
            ShowAlert("Não existe esse Comercial no Contrato.");
            pVeiculacao.Cod_Comercial = "";
            pVeiculacao.Titulo_Comercial = "";
            pVeiculacao.Cod_Tipo_Comercial = "";
            pVeiculacao.Duracao = "";
        };
    };
    //===================AdicionarComercialPara
    $scope.AdicionarComercialVei = function (pDeterminacaoVeiculacao) {
        pDeterminacaoVeiculacao.ComercialPara.push({ 'Cod_Comercial': '', 'Titulo_Comercial': '', 'Cod_Tipo_Comercial': '', 'Nome_Tipo_Comercial': '', 'Duracao': '' });
    }

    //===================Excluir Comercial para
    $scope.ExcluiComercialVei = function (pComercial) {
        for (var i = 0; i < $scope.DeterminacaoVei.ComercialPara.length; i++) {
            if ($scope.DeterminacaoVei.ComercialPara[i].Cod_Comercial.trim().toUpperCase() == pComercial.Cod_Comercial.trim().toUpperCase()) {
                $scope.DeterminacaoVei.ComercialPara.splice(i, 1);
                break;
            };
        };
    }
    //===============Selecao de Veiculos
    $scope.SelecionarVeiculos = function (pContrato) {
        $scope.PesquisaTabelas = NewPesquisaTabela();
        $scope.PesquisaTabelas.Items = pContrato.Veiculos;
        $scope.PesquisaTabelas.Titulo = "Seleção de Veículos"
        $scope.PesquisaTabelas.MultiSelect = true;
        $scope.PesquisaTabelas.ClickCallBack = function () { };
        $("#modalTabela").modal(true);
    };
    //===============Adicionar Comercial
    $scope.AdicionarComercial = function (pContrato) {
        $scope.ShowComercial = true;
        $scope.ShowDados = false;
        $scope.Comercial = $scope.NewComercial();
        $scope.Comercial.Cod_Empresa = pContrato.Cod_Empresa;
        $scope.Comercial.Numero_Mr = pContrato.Numero_Mr;
        $scope.Comercial.Sequencia_Mr = pContrato.Sequencia_Mr;
    };
    //===============Clicou na lupa Produto 
    $scope.PesquisaProduto = function (pDeterminacao, pComercial) {
        $scope.PesquisaTabelas = NewPesquisaTabela();
        $scope.PesquisaTabelas.Items = [];
        $scope.PesquisaTabelas.PreFiltroTexto = "";
        $scope.PesquisaTabelas.PreFilter = true;
        $scope.PesquisaTabelas.Titulo = "Seleção de Produtos";
        $scope.PesquisaTabelas.MultiSelect = false;
        if (pDeterminacao.Cod_Cliente) {
            $scope.PesquisaTabelas.ButtonText = "Mostrar Produtos do Cliente " + pDeterminacao.Cod_Cliente
            $scope.PesquisaTabelas.ButtonCallBack = function () {
                httpService.Get('MapaReserva/ListarProdutoCliente/' + pDeterminacao.Cod_Cliente).then(function (response) {
                    $scope.PesquisaTabelas.Items = response.data;
                });
            };
        };
        $scope.PesquisaTabelas.ClickCallBack = function (value) { pComercial.Cod_Red_Produto = value.Codigo; pComercial.Nome_Produto = value.Descricao };
        $scope.PesquisaTabelas.LoadCallBack = function (pFilter) {
            httpService.Get('ListarTabela/Produto/' + pFilter).then(function (response) {
                $scope.PesquisaTabelas.Items = response.data;
            });
        }
        $("#modalTabela").modal(true);
    };
    ////==========================Validar o Produto
    $scope.ProdutoChange = function (pComercial) {
        if (!pComercial.Cod_Red_Produto) {
            pComercial.Nome_Produto = "";
            return;
        };
        httpService.Get("ValidarTabela/Produto/" + pComercial.Cod_Red_Produto).then(function (response) {
            if (!response.data[0].Status) {
                ShowAlert("Produto Inválido");
                pComercial.Cod_Red_Produto = "";
                pComercial.Nome_Produto = "";
            }
            else {
                pComercial.Nome_Produto = response.data[0].Descricao;
            }
        });
    };
    ////==========================Salvar Comercial
    $scope.SalvarComercial = function (pComercial) {
        pComercial.Cod_Comercial = pComercial.Cod_Comercial.toUpperCase();
        pComercial.Titulo_Comercial = pComercial.Titulo_Comercial.toUpperCase();
        httpService.Post("Determinacao/SalvarComercial", pComercial).then(function (response) {
            if (response.data) {
                if (response.data[0].Status) {
                    $scope.Contrato.Comerciais.push(pComercial);
                    $scope.Comercial = $scope.NewComercial();
                    $scope.Comercial.Cod_Empresa = $scope.Contrato.Cod_Empresa;
                    $scope.Comercial.Numero_Mr = $scope.Contrato.Numero_Mr;
                    $scope.Comercial.Sequencia_Mr = $scope.Contrato.Sequencia_Mr;
                }
                else {
                    ShowAlert(response.data[0].Mensagem)
                };
            };
        });
    };
    ////==========================Marcar veiculacoes
    $scope.MarcarVeiculacao = function (pTipo, pValue) {
        var _first = false;
        var _achou = false
        if (pTipo == 'Dia') {
            for (var x = 0; x < $scope.Contrato.Veiculacoes.length; x++) {
                for (var y = 0; y < $scope.Contrato.Veiculacoes[x].Insercoes.length; y++) {
                    if ($scope.Contrato.Veiculacoes[x].Insercoes[y].Dia == pValue && $scope.Contrato.Veiculacoes[x].Insercoes[y].Qtd) {
                        if (!_achou) {
                            _first = $scope.Contrato.Veiculacoes[x].Insercoes[y].Selected;
                            _achou = true;
                        }
                        $scope.Contrato.Veiculacoes[x].Insercoes[y].Selected = !_first;
                    };
                };
            };
        };
        if (pTipo == 'Ins') {
            if (pValue.Qtd) {
                pValue.Selected = !pValue.Selected;
            };
        };
        if (pTipo == 'Linha') {
            for (var y = 0; y < pValue.Insercoes.length; y++) {
                if (pValue.Insercoes[y].Qtd) {
                    if (!_achou) {
                        _first = pValue.Insercoes[y].Selected;
                        _achou = true;
                    }
                    pValue.Insercoes[y].Selected = !_first;
                };
            };
        };
        if (pTipo == 'Geral') {
            for (var x = 0; x < $scope.Contrato.Veiculacoes.length; x++) {
                for (var y = 0; y < $scope.Contrato.Veiculacoes[x].Insercoes.length; y++) {
                    if ($scope.Contrato.Veiculacoes[x].Insercoes[y].Qtd) {
                        if (!_achou) {
                            _first = $scope.Contrato.Veiculacoes[x].Insercoes[y].Selected;
                            _achou = true;
                        }
                        $scope.Contrato.Veiculacoes[x].Insercoes[y].Selected = !_first;
                    };
                };
            };
        };
        //------------------se nao tem nada selecionar, limpa comerciais para
        var _temSelected = false;
        for (var v = 0; v < $scope.Contrato.Veiculacoes.length; v++) {
            _temSelected = false;
            for (var i = 0; i < $scope.Contrato.Veiculacoes[v].Insercoes.length; i++) {
                if ($scope.Contrato.Veiculacoes[v].Insercoes[i].Selected) {
                    _temSelected = true;
                    break;
                };
            };
            if (!_temSelected) {
                for (var p = 0; p < $scope.Contrato.Veiculacoes[v].ComercialPara.length; p++) {
                    $scope.Contrato.Veiculacoes[v].ComercialPara[p].Cod_Comercial = "";
                };
            };
        };
    };
    //===============Selecao Comercial para
    $scope.SelecionarComercialPara = function (pContrato) {
        var _TemSelected = false;
        if (!$scope.Contrato) {
            return;
        }
        var _Achou = false;
        for (var v = 0; v < $scope.Contrato.Veiculacoes.length; v++) {
            for (var i = 0; i < $scope.Contrato.Veiculacoes[v].Insercoes.length; i++) {
                if ($scope.Contrato.Veiculacoes[v].Insercoes[i].Selected) {
                    _TemSelected = true;
                    break
                };
            };
        };
        if (!_TemSelected) {
            ShowAlert("Nenhuma Inserção está marcada para Substituir.")
            return;
        }
        var _ListaComerciais = [];
        for (var i = 0; i < $scope.Contrato.Comerciais.length; i++) {
            _ListaComerciais.push({ 'Codigo': $scope.Contrato.Comerciais[i].Cod_Comercial, 'Descricao': $scope.Contrato.Comerciais[i].Titulo_Comercial })
        }
        $scope.PesquisaTabelas = NewPesquisaTabela();
        $scope.PesquisaTabelas.Items = _ListaComerciais;
        $scope.PesquisaTabelas.Titulo = "Seleção de Comercial Para"
        $scope.PesquisaTabelas.ClickCallBack = function (value) {
            for (var v = 0; v < $scope.Contrato.Veiculacoes.length; v++) {
                _TemSelected = false;
                for (var i = 0; i < $scope.Contrato.Veiculacoes[v].Insercoes.length; i++) {
                    if ($scope.Contrato.Veiculacoes[v].Insercoes[i].Selected) {
                        _TemSelected = true;
                        _Achou = true;
                        break;
                    };
                };
                if (_TemSelected) {
                    for (var p = 0; p < $scope.Contrato.Veiculacoes[v].ComercialPara.length; p++) {
                        if (!$scope.Contrato.Veiculacoes[v].ComercialPara[p].Cod_Comercial) {
                            $scope.Contrato.Veiculacoes[v].ComercialPara[p].Cod_Comercial = value.Codigo;
                            break;
                        };
                    };
                };
            };
        };
        $scope.PesquisaTabelas.MultiSelect = false;
        $("#modalTabela").modal(true);
    };
    //========================Excluir comercias para da determinacao por mapa
    $scope.ExcluiComercialPara = function (pContrato) {
        for (var v = 0; v < pContrato.Veiculacoes.length; v++) {
            for (var p = 0; p < pContrato.Veiculacoes[v].ComercialPara.length; p++) {
                pContrato.Veiculacoes[v].ComercialPara = [{ 'Cod_Comercial': '', 'Id_Veiculacao': pContrato.Veiculacoes[v].Id_Veiculacao }, { 'Cod_Comercial': '', 'Id_Veiculacao': pContrato.Veiculacoes[v].Id_Veiculacao }];
                var colCount = pContrato.Veiculacoes[0].ComercialPara.length;
                $('.thHeaderPara').attr("colspan", colCount);
            };
        };
    };

    //========================Adicionar o Comercial para
    $scope.AdicionarComercialPara = function (pContrato) {
        for (var v = 0; v < pContrato.Veiculacoes.length; v++) {
            pContrato.Veiculacoes[v].ComercialPara.push({ 'Id_Veiculacao': pContrato.Veiculacoes[v].Id_Veiculacao, 'Cod_Comercial': '' });
        };
        var colCount = pContrato.Veiculacoes[0].ComercialPara.length;
        $('.thHeaderPara').attr("colspan", colCount);
    };

    //========================Limpar Comerciais para da Determinacao por Mapa
    $scope.LimparComercialPara = function (pContrato) {
        for (var v = 0; v < pContrato.Veiculacoes.length; v++) {
            for (var i = 0; i < pContrato.Veiculacoes[v].ComercialPara.length; i++) {
                pContrato.Veiculacoes[v].ComercialPara[i].Cod_Comercial = "";
            };
        };
    };
    //======================Analisar a Determinacao
    $scope.AnalisarDeterminacao = function (pContrato, pOperacao) {

        if ($scope.CurrentTab == 'Mapa') {
            for (var i = 0; i < $scope.Contrato.Veiculos.length; i++) {
                if ($scope.Contrato.Veiculos[i].Codigo == $scope.Filtro.Cod_Veiculo) {
                    $scope.Contrato.Veiculos[i].Selected = true;
                }
                else {
                    $scope.Contrato.Veiculos[i].Selected = false;
                };
            };
        };
        $scope.VeiculosAnalise = [];
        for (var i = 0; i < $scope.Contrato.Veiculos.length; i++) {
            if ($scope.Contrato.Veiculos[i].Selected) {
                $scope.VeiculosAnalise.push($scope.Contrato.Veiculos[i]);
            }
        };
        $scope.Cod_Veiculo_Analise = $scope.VeiculosAnalise[0].Codigo;
        pContrato.De_Para = $scope.DeterminacaoVei;
        pContrato.Operacao = pOperacao;
        httpService.Post('Determinacao/SalvarDeterminacao', pContrato).then(function (response) {
            if (response) {
                if (!response.data[0].Status) {
                    ShowAlert(response.data[0].Mensagem);
                    return;
                }
                else {
                    if (pOperacao == 'A') {
                        $scope.Analise = response.data;
                        $scope.ShowDados = false;
                        $scope.ShowAnalise = true;
                    };
                    if (pOperacao == 'D') {
                        ShowAlert(response.data[0].Mensagem);
                        $scope.CancelarDeterminacao();
                    }
                };
            };
        });
    };
    //========================Excluir Comercial do Contrato
    $scope.ExcluirComercialContrato = function (pComercial) {
        httpService.Post('Determinacao/ExcluirComercialContrato',pComercial).then(function (response) {
            if (response.data) {
                if (!response.data[0].Status) {
                    ShowAlert(response.data[0].Mensagem);
                }
                else {
                    for (var i = 0; i < $scope.Contrato.Comerciais.length; i++) {
                        if ($scope.Contrato.Comerciais[i].Cod_Comercial.trim().toUpperCase() == pComercial.Cod_Comercial.trim().toUpperCase()) {
                            $scope.Contrato.Comerciais.splice(i, 1);
                        };
                    };
                };
            };
        });
    };
    //========================Cancelar a Analise
    $scope.CancelarAnalise = function () {
        $scope.Analise = ""
        $scope.ShowDados = true;
        $scope.ShowAnalise = false;
    }
}]);


