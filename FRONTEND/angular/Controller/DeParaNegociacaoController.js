angular.module('App').controller('DeParaNegociacaoController', ['$scope', '$rootScope', '$location', 'httpService', '$location', function ($scope, $rootScope, $location, httpService, $location) {

    
    //===================Inicializa scopes
    $scope.CompetenciaKeys = { 'Year': new Date().getFullYear(), 'First': '', 'Last': '' }
    $scope.NewFilter = function () {
        $scope.Filtro={
        'Cod_Empresa': '',
        'Numero_Mr': '',
        'Sequencia_Mr': '',
        'Agencia': '',
        'Cliente': '',
        'Negociacao_De': '',
        'Negociacao_Para': '',
        };
    }
    $scope.NewFilter();
    //=======================Carrega Dados
    $scope.CarregaDados = function () {
        httpService.Post("MapaReserva/GetContratoDeParaNegociacao", $scope.Filtro).then(function (response) {
            if (response.data) {
                if (!response.data.Numero_Mr) {
                    ShowAlert('Contrato não Cadastrado');
                }
                $scope.Filtro.Cod_Agencia = response.data.Nome_Agencia;
                $scope.Filtro.Cod_Cliente = response.data.Nome_Cliente;
                $scope.Filtro.Nome_Agencia = response.data.Nome_Agencia;
                $scope.Filtro.Nome_Cliente = response.data.Nome_Cliente;
                $scope.Filtro.Negociacao_De = response.data.Negociacao_De;
                $scope.Filtro.Negociacao_Para = "";
            };
        })
    };
    //=======================Processa o De-Para
    $scope.ProcessaDeParaNegociacao = function(pFiltro)
    {
        httpService.Post("MapaReserva/ProcessaDeParaNegociacao", pFiltro).then(function (response) {
            if (response.data[0].Retorno) {
                ShowAlert(response.data[0].Retorno);
            }
            else {
                ShowAlert("Processado com Sucesso.");
                $scope.NewFilter();
            };
        });
    }
    //=======================Limpa Dados
    $scope.CancelaStatus = function()
    {
        $scope.Filtro = $scope.NewFilter()
    }
    //=======================Se Mudou Empresa.Contrato,Seqquencia, carrega dados 
    $scope.$watch('[Filtro.Cod_Empresa,Filtro.Numero_Mr,Filtro.Sequencia_Mr]', function (newValue, oldValue) {
        if (newValue != oldValue) {
            if (newValue[0] && newValue[1] && newValue[2] ) {
                $scope.CarregaDados();
            }
        };
    });
}]);


