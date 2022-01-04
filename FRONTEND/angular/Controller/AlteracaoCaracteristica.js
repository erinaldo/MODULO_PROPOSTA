angular.module('App').controller('AlteracaoCaracteristicaController', ['$scope', '$rootScope', '$location', 'httpService', '$location', function ($scope, $rootScope, $location, httpService, $location) {

    //==========================Inicializa scopes
    $scope.Filtros = [];
    //=======================================Adicionar Linha
    $scope.AdicionarLinhas = function (pFiltro) {
        pFiltro.push({ 'Cod_Empresa': '', 'Numero_Mr': '', 'Sequencia_Mr': '', 'Cod_Comercial': '', 'Cod_Veiculo': '', 'Data Exibição': '', 'Cod_Programa': '', 'Chave_Acesso': '', 'Cod_Caracteristica_De': '', 'Cod_Caracteristica_Para': '','Critica':'' });
    }
    //=======================================Novo Filtro
    $scope.NewFiltro = function () {
        $scope.Filtros = [];
        for (var i = 0; i < 5; i++) {
            $scope.AdicionarLinhas($scope.Filtros);
        };
    };
    $scope.NewFiltro();

    //=======================================Salvar Alteracao
    $scope.AlterarCaracteristica = function (pFiltro) {
        httpService.Post("MapaReserva/AlterarCV", pFiltro).then(function (response) {
            if (response.data) {
                $scope.Filtros = response.data;
                for (var i = 0; i < $scope.Filtros.length; i++) {
                    if ($scope.Filtros[i].Numero_Mr == 0) {
                        $scope.Filtros[i].Numero_Mr = "";
                    };
                    if ($scope.Filtros[i].Sequencia_Mr == 0) {
                        $scope.Filtros[i].Sequencia_Mr = "";
                    };
                    if ($scope.Filtros[i].Chave_Acesso == 0) {
                        $scope.Filtros[i].Chave_Acesso = "";
                    };
                }
            };
        });

    };
}]);


