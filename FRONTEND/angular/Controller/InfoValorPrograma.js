angular.module('App').controller('InfoValorProgramaController', ['$scope', '$rootScope', '$location', 'httpService', '$location', function ($scope, $rootScope, $location, httpService, $location) {
    
    //===================Inicializa scopes
    $scope.Filtro = { 'Cod_Empresa': '', 'Numero_Mr': '', 'Sequencia_Mr': '' }
    $scope.ShowFilter = true;
    $scope.ShowGrid = false;
    $scope.InfoValor =  [];
    //===================Carrega Informacao de Valores
    $scope.CarregarInfoValor = function (pFiltro) {
        if (!pFiltro.Cod_Empresa || !pFiltro.Numero_Mr || !pFiltro.Sequencia_Mr) {
            ShowAlert("Preencha Empresa/Contrato/Sequencia");
            return;
        }
        httpService.Post('ValorInformadoProgramaGet', pFiltro).then(function (response) {
            if (response.data) {
                if (response.data.length > 0) {
                    $scope.InfoValor = response.data;
                    $scope.ShowGrid = true;
                    $scope.ShowFilter = false;
                }
                else {
                    ShowAlert("Não existem dados para esse contrato.")
                };
            };
        });
    };
    //===================Salvar Info
    $scope.SalvarInfo = function (pInfo) {
        httpService.Post("ValorInformadoProgramaSalvar", pInfo).then(function (response) {
            if (response.data) {
                $scope.CarregarInfoValor($scope.Filtro)
            };
        });
    }
    //===================Cancelar Info
    $scope.CancelarInfo = function () {
        $scope.InfoValor = [];
        $scope.ShowGrid = false;
        $scope.ShowFilter = true;
        $scope.Filtro = { 'Cod_Empresa': '', 'Numero_Mr': '', 'Sequencia_Mr': '' }
    };
}]);

