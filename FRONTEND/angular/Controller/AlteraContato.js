angular.module('App').controller('AlteraContatoController', ['$scope', '$rootScope', '$location', 'httpService', '$location', function ($scope, $rootScope, $location, httpService, $location) {

    
    //===================Inicializa scopes
    $scope.CompetenciaKeys = { 'Year': new Date().getFullYear(), 'First': '', 'Last': '' }
    $scope.Filtro    = "";
    $scope.NewFilter = function () {
        $scope.Filtro={
        'Competencia': '',
        'Cod_Agencia': '',
        'Cod_Contato_De': '',
        'Nome_Contato_De': '',
        'Cod_Contato_Para': '',
        'Nome_Contato_Para': '',
        };
    }
    $scope.NewFilter();
    $scope.ShowFilter = true;
    $scope.ShowGrid = false;
    $scope.DePara = [];
    //============================Processa o De-Para
    $scope.ProcessaDeParaContato = function (pFiltro) {
        pFiltro.Cod_Contato_De = pFiltro.Cod_Contato_De.toUpperCase();
        pFiltro.Cod_Contato_Para = pFiltro.Cod_Contato_Para.toUpperCase();
        httpService.Post("MapaReserva/DeParaContato", pFiltro).then(function (response) {
            if (response.data[0].Critica) {
                ShowAlert(response.data[0].Critica)
            }
            else {
                $scope.DePara = response.data;
                $scope.ShowFilter = false;
                $scope.ShowGrid = true;
            }
        });
    }
    //=======================Limpa Contratos Alterados
    $scope.CancelaStatus = function()
    {
        $scope.DePara = [];
        $scope.Filtro = $scope.NewFilter()
        $scope.ShowFilter = true;
        $scope.ShowGrid = false;
    }
}]);


