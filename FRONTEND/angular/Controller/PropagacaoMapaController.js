angular.module('App').controller('PropagacaoMapaController', ['$scope', '$rootScope', 'httpService', '$location', '$timeout', function ($scope, $rootScope, httpService, $location, $timeout) {
                                  
    //============== Inicializa Scopes
    $scope.ShowGrid = false;
    $scope.Propagacao_Mapa = [];

    $scope.CurrentShow = "Filtro";

    $scope.CompetenciaKeys = { 'Year': new Date().getFullYear(), 'First': '', 'Last': '' }
    $scope.PeriodoInicioKeys = { 'Year': new Date().getFullYear(), 'First': '', 'Last': '' };
    $scope.PeriodoFimKeys = { 'Year': new Date().getFullYear(), 'First': '', 'Last': '' };

    //====================Inicializa o Filtro
    $scope.Filtro = {};
    $scope.ShowGrid = false;
    $scope.NewFiltro = function () {

        return {
            'Empresa': '',
            'Nome_Empresa': '',
            'Numero_Mr': '',
            'Sequencia_Mr': '',
            'Competencia': '',
            'Competencia_Inicial': '',
            'Competencia_Final': '',
            'Indica_Tqp': false
        }
    }
    $scope.Filtro = $scope.NewFiltro();
     
    //===================Carregar Mapa
    $scope.CarregarPropagacaoMapa = function (pFiltro) {

        if (pFiltro.Cod_Empresa == "" ) {
            ShowAlert("Codigo Empresa é  filtro Obrigatório.");
            return;
        }

        if (pFiltro.Numero_Mr == "") {
            ShowAlert("Numero do contrato é  filtro Obrigatório.");
            return;
        }

        if (pFiltro.Sequencia_Mr == "") {
            ShowAlert("Sequencia do contrato é  filtro Obrigatório.");
            return;
        }

        if (pFiltro.Competencia == "") {
            ShowAlert("Competência de origem é filtro Obrigatório.");
            return;
        }

        if (pFiltro.Competencia_Inicial == "" ) {
            ShowAlert("Competência Inicial  é filtro Obrigatório.");
            return;
        }

        if (pFiltro.Competencia_Final == "") {
            ShowAlert("Competência Final  é filtro Obrigatório.");
            return;
        }
        $rootScope.routeloading = true;
        $scope.Propagacao_Mapa = [];
        

        httpService.Post("CarregarPropagacaoMapa", pFiltro).then(function (response) {
            if (response) {
                $scope.Propagacao_Mapa = response.data;
                for (var i = 0; i < response.data.length; i++) {
                    $scope.Propagacao_Mapa[i].Competencia = response.data[i].Competencia;
                    $scope.Propagacao_Mapa[i].Mensagem_Status = response.data[i].Mensagem_Status;

                }
                $scope.ShowGrid = true;
                if (response.data[0].Indica_Erro != 1) {
                    $scope.Filtro = $scope.NewFiltro();
                };
            };
        });
        
    };
    //===========================fim do load da pagina
    $scope.$watch('$viewContentLoaded', function () {
        $rootScope.routeloading = false;
    });
}]);

