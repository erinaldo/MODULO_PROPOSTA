﻿angular.module('App').controller('PortalAppController', ['$scope', '$rootScope', 'tokenApi', 'httpService', '$location', function ($scope, $rootScope, tokenApi, httpService, $location) {
    
    var _url = "GetParametroKey?"
    _url += 'Cod_Parametro=16'
    _url += "&";
    $scope.UrlPowerBi = "";
    httpService.Get(_url).then(function (response) {
        if (response.data) {
            $scope.UrlPowerBi = response.data;
        };
    });
 
    
    
    $scope.AppModulos = [
        {
            'Id': 1,
            'Name': 'SIM-Dashboard',
            'Text': 'Informações Gerenciais',
            'bgcolor': 'rgba(10, 153, 99, 0.45)',
            'color': 'black',
            'url': 'indexDash.html'
        },
        {
            'Id': 2,
            'Name': 'SIM-Administração',
            'Text': 'Gestão de Usuários,Tabelas de Apoios, Parâmetros, etc.',
            'bgcolor': 'rgba(85, 113, 83, 0.18)',
            'color': 'black',
            'url': 'indexAdm.html'
        },
        {
            'Id': 2, 'Name': 'SIM-Opec',
            'Text': 'Entrada de Mapas, Determinação de Títulos, etc.',
            'bgcolor': 'antiquewhite',
            'color': 'black',
            'url': 'indexOpec.html'
        },
        {
            'Id': 3,
            'Name': 'SIM-Vendas',
            'Text': 'Simulação de Vendas, Elaboração de Propostas, Negociações, Compensações.',
            'bgcolor': '#9091e1',
            'color': 'white',
            'url': 'indexVendas.html'
        },
        {
            'Id': 4,
            'Name': 'SIM-Programação',
            'Text': 'Gestão da Disponibilidade, Manutenção da Grade.',
            'bgcolor': '#f78595',
            'color': 'black',
            'url': 'indexProg.html'
        },
        {
            'Id': 5,
            'Name': 'SIM-Roteiro',
            'Text': 'Elaboração do Roteiro,Envio e Retorno de Play-Lists',
            'bgcolor': '#d9cd6c',
            'color': 'black',
            'url': 'indexRoteiro.html'
        },
        {
            'Id': 6,
            'Name': 'SIM-Checking',
            'Text': 'Apontamento de Falhas,Horários, Conciliação da Play-List',
            'bgcolor': 'rgb(208, 16, 57)',
            'color': 'white',
            'url': 'indexChecking.html'
        },
        {
            'Id': 7, 'Name': 'SIM-Faturamento',
            'Text': 'Complemento de Contratos, Geração de Pedidos.',
            'bgcolor': 'rgb(199, 197, 213)',
            'color': 'black',
            'url': 'indexFaturamento.html'
        },
        {
            'Id': 8, 'Name': 'SIM-Merchandising',
            'Text': 'Controle de Merchandising',
            'bgcolor': 'rgb(153, 49, 142)',
            'color': 'white',
            'url': 'indexMercha.html'
        },
         {
             'Id': 9, 'Name': 'SIM-Permutas',
             'Text': 'Gestão de Permutas',
             'bgcolor': '#5b42d4',
             'color': 'white',
             'url': 'IndexPermuta.html'
         },
         {
             'Id': 10, 'Name': 'Relatórios',
             'Text': 'Emissão de Relatórios diversos',
             'bgcolor': 'rgba(116, 79, 219, 0.2)',
             'color': '#000',
             'url': 'IndexReport.html'
         },
          {
              'Id': 11, 'Name': 'SIM-Interfaces',
              'Text': 'Parâmetros da Interface, Envio e Retorno de Dados E.M.S',
              'bgcolor': '#98FB98',
              'color': '#000',
              'url': 'IndexInterfacesEMS.html',
              'Register':'Jornal_Comercio'
          },
        {
            'Id': 12, 'Name': 'SIM-Mercado',
            'Text': 'Módulo Mercado Anunciante',
            'bgcolor': 'rgba(136, 10, 10, 0.36)',
            'color': '#000',
            'url': '',
            'directurl':$rootScope.simagenciaUrl,
        },
        
    ];
    $scope.ShortMenus = [
        {
            'Title': 'DashBoard',
            'SubItens': [
                { 'Title': 'Evolução de Vendas', 'Url': $rootScope.pageUrl + 'indexDash.html#/EvolucaoVendas' },
                { 'Title': 'Funil de Vendas', 'Url': $rootScope.pageUrl + 'indexDash.html#/FunilVendas' },
                { 'Title': 'Gráfico de Vendas', 'Url': $rootScope.pageUrl + 'indexDash.html#/GraficoVendas' },
                //{ 'Title': 'Power-Bi', 'Url': $rootScope.UrlPowerBi } --- Nao funciona no short Menu
            ],
        },
        {
            'Title': 'Administração',
            'SubItens': [
                { 'Title': 'Usuários', 'Url': $rootScope.pageUrl + 'IndexAdm.html#/usuario' },
                { 'Title': 'Grupo de Usuários', 'Url': $rootScope.pageUrl + 'IndexAdm.html#/grupousuario' },
                { 'Title': 'Parâmetros Gerais', 'Url': $rootScope.pageUrl + 'IndexAdm.html#/Parametro' },
                { 'Title': 'Parâmetros de Valoração', 'Url': $rootScope.pageUrl + 'IndexAdm.html#/ParametroValoracao' },
                { 'Title': 'Cadastros', 'Url': $rootScope.pageUrl + 'IndexAdm.html#/cadastro' },
                { 'Title': 'Fechamento de Competência', 'Url': $rootScope.pageUrl + 'IndexAdm.html#/numeracao' },
            ],
        },
        {
            'Title': 'Opec',
            'SubItens': [
                { 'Title': 'Manutenção de Mapa Reserva', 'Url': $rootScope.pageUrl + 'IndexOpec.html#/MapaReserva' },
                { 'Title': 'Determinação de Títulos', 'Url': $rootScope.pageUrl + 'IndexOpec.html#/Determinacao' },
                { 'Title': 'Importar Propostas', 'Url': $rootScope.pageUrl + 'IndexOpec.html#/MapaReservaImport' },
                { 'Title': 'Propagação de Mapa Reserva', 'Url': $rootScope.pageUrl + 'IndexOpec.html#/PropagacaoMapa' },
                { 'Title': 'Consulta de Veiculações', 'Url': $rootScope.pageUrl + 'IndexOpec.html#/ConsultaVeiculacoes' },
            ],
        },
        {
            'Title': 'Vendas',
            'SubItens': [
                { 'Title': 'Modelo de Vendas', 'Url': $rootScope.pageUrl + 'IndexVendas.html#/Simulacao' },
                { 'Title': 'Propostas   ', 'Url': $rootScope.pageUrl + 'IndexVendas.html#/Proposta' },
                { 'Title': 'Negociações', 'Url': $rootScope.pageUrl + 'IndexVendas.html#/Negociacao' },
                { 'Title': "Manutenção de Am's", 'Url': $rootScope.pageUrl + 'IndexVendas.html#/ConsultaAM' },
                { 'Title': "Pacote de Descontos", 'Url': $rootScope.pageUrl + 'IndexVendas.html#/pacote' },
                { 'Title': "Regras de Aprovação", 'Url': $rootScope.pageUrl + 'IndexVendas.html#/regraaprovacao' },
                { 'Title': "Previsão de Vendas", 'Url': $rootScope.pageUrl + 'IndexVendas.html#/PrevisaoVendas' },
            ],  
        },
        {
            'Title': 'Programação',
            'SubItens': [
                { 'Title': 'Manutenção da Grade', 'Url': $rootScope.pageUrl + 'IndexProg.html#/Grade' },
                { 'Title': 'Consulta da Disponibilidade', 'Url': $rootScope.pageUrl + 'IndexProg.html#/ConsultaProgramacaoDiaria' },
                { 'Title': 'De-Para da Programação(Período)', 'Url': $rootScope.pageUrl + 'IndexProg.html#/DeParaPeriodo' },
                { 'Title': 'De-Para da Programação(Data)', 'Url': $rootScope.pageUrl + 'IndexProg.html#/DeParaData' },
                { 'Title': 'Confirmação Horário da Programação', 'Url': $rootScope.pageUrl + 'IndexProg.html#/HorarioExibicao' },
            ],
        },
        {
            'Title': 'Roteiro',
            'SubItens': [
                { 'Title': 'Parâmetros do Roteiro', 'Url': $rootScope.pageUrl + 'IndexRoteiro.html#/ParamRoteiro' },
                { 'Title': 'Parâmetros de Retorno da Play-List', 'Url': $rootScope.pageUrl + 'IndexRoteiro.html#/ParRetorPlayList' },
                { 'Title': 'Depositórios', 'Url': $rootScope.pageUrl + 'IndexRoteiro.html#/DepositorioFitas' },
                { 'Title': 'Materiais', 'Url': $rootScope.pageUrl + 'IndexRoteiro.html#/MateriaisFitas' },
                { 'Title': 'Numeração de Fitas', 'Url': $rootScope.pageUrl + 'IndexRoteiro.html#/NumeracaoFitas' },
                { 'Title': 'Numeração de Fitas Patrocínio', 'Url': $rootScope.pageUrl + 'IndexRoteiro.html#/FitaPatrocinio' },
                { 'Title': 'Consulta de Veiculações', 'Url': $rootScope.pageUrl + 'IndexRoteiro.html#/ConsultaVeiculacoes' },
                { 'Title': 'Pré-Ordenação', 'Url': $rootScope.pageUrl + 'IndexRoteiro.html#/PreOrdenacao' },
                { 'Title': 'Ordenação', 'Url': $rootScope.pageUrl + 'IndexRoteiro.html#/Roteiro' },
                { 'Title': 'Consulta do Roteiro', 'Url': $rootScope.pageUrl + 'IndexRoteiro.html#/ConsultaRoteiroOrdenado' },
                { 'Title': 'Composição de Breaks', 'Url': $rootScope.pageUrl + 'IndexRoteiro.html#/Breaks' },
                { 'Title': 'Consulta de Fitas Ordenadas', 'Url': $rootScope.pageUrl + 'IndexRoteiro.html#/ConsultaFitasOrdenadas' },
                { 'Title': 'Envio Play-List', 'Url': $rootScope.pageUrl + 'IndexRoteiro.html#/EnvioPlayList' },
                { 'Title': 'Parâmetros de Numeração de Fitas ', 'Url': $rootScope.pageUrl + 'IndexRoteiro.html#/ParamNumFitas' },
                { 'Title': 'Encerramento do Roteiro', 'Url': $rootScope.pageUrl + 'IndexRoteiro.html#/EncerramentoRoteiro' },
            ],
        },
        {
            'Title': 'Checking',
            'SubItens': [
                { 'Title': 'Baixa por Veiculação', 'Url': $rootScope.pageUrl + 'IndexChecking.html#/BaixaVeiculacao' },
                { 'Title': 'Baixa por Contrato', 'Url': $rootScope.pageUrl + 'IndexChecking.html#/BaixaContrato' },
                { 'Title': 'Baixa por Roteiro', 'Url': $rootScope.pageUrl + 'IndexChecking.html#/BaixaRoteiro' },
                { 'Title': 'Baixa de Veiculações de Site', 'Url': $rootScope.pageUrl + 'IndexChecking.html#/BaixaSite' },
                { 'Title': 'Confirmação do Roteiro', 'Url': $rootScope.pageUrl + 'IndexChecking.html#/ConfirmacaoRoteiro' },
                { 'Title': 'Retorno da Play-List', 'Url': $rootScope.pageUrl + 'IndexChecking.html#/RetornoPlayList' },
                { 'Title': 'Consulta de Veiculações', 'Url': $rootScope.pageUrl + 'IndexChecking.html#/ConsultaVeiculacoes' },
                { 'Title': 'Geração do Comprovante', 'Url': $rootScope.pageUrl + 'IndexChecking.html#/GeracaoCE' },
                { 'Title': 'Impressão do Comprovante', 'Url': $rootScope.pageUrl + 'IndexChecking.html#/ImpressaoCe' },
                { 'Title': 'Reabrir Comprovante', 'Url': $rootScope.pageUrl + 'IndexChecking.html#/ReabreCE' },
            ],
        },
        {
            'Title': 'Faturamento',
            'SubItens': [
                { 'Title': 'Complemento de Contratos', 'Url': $rootScope.pageUrl + 'IndexFaturamento.html#/ComplementoContrato/1' },
                { 'Title': 'Complemento de Antecipados', 'Url': $rootScope.pageUrl + 'IndexFaturamento.html#/ComplementoContrato/0' },
                { 'Title': 'Outras Receitas', 'Url': $rootScope.pageUrl + 'IndexFaturamento.html#/ComplementoOutrasReceitas' },
                { 'Title': 'Pesquisa de Complementos', 'Url': $rootScope.pageUrl + 'IndexFaturamento.html#/ComplementoContratoPesquisa' },
                { 'Title': 'Geração de Faturas', 'Url': $rootScope.pageUrl + 'IndexFaturamento.html#/FaturaGeracao' },
                { 'Title': 'Pesquisa de Faturas', 'Url': $rootScope.pageUrl + 'IndexFaturamento.html#/FaturasPesquisa' },
                { 'Title': 'Valoração de Contratos', 'Url': $rootScope.pageUrl + 'IndexFaturamento.html#/CalculoValoracao' },
                { 'Title': 'Crítica da Valoração', 'Url': $rootScope.pageUrl + 'IndexFaturamento.html#/CriticaValoracao' },

            ],
        },
        {
            'Title': 'Merchandising',
            'SubItens': [
                { 'Title': 'Grade de Merchandising', 'Url': $rootScope.pageUrl + 'IndexMercha.html#/GradeMercha' },
                { 'Title': 'Veiculações de Merchandising', 'Url': $rootScope.pageUrl + 'IndexMercha.html#/ConsultaVeiculacoesMercha' },
                { 'Title': 'Roteiro de Merchandising', 'Url': $rootScope.pageUrl + 'IndexMercha.html#/RoteiroMercha' },

            ],
        },
        {
            'Title': 'Permutas',
            'SubItens': [
                { 'Title': 'Controle de Permutas', 'Url': $rootScope.pageUrl + 'IndexPermuta.html#/Permutas' },
            ],
        },
        {
            'Title': 'Relatórios',
            'SubItens': [
                { 'Title': 'Menu de Relatórios', 'Url': $rootScope.pageUrl + 'IndexReport.html#/report' },
            ],
        },
        {
            'Title': 'Interfaces E.M.S',
            'Register':'Jornal_Comercio',
            'SubItens': [
                { 'Title': 'Associação de Contatos', 'Url': $rootScope.pageUrl + 'IndexInterfacesEms.html#/AssociacaoContatos', },
                { 'Title': 'Associação de Programas', 'Url': $rootScope.pageUrl + 'IndexInterfacesEms.html#/AssociacaoProgramas', },
                { 'Title': 'Cadastro de Portadores', 'Url': $rootScope.pageUrl + 'IndexInterfacesEms.html#/Portador', },
                { 'Title': 'Log da Lista Negra', 'Url': $rootScope.pageUrl + 'IndexInterfacesEms.html#/LogLista', },

            ],
        },
    ]
}]);





