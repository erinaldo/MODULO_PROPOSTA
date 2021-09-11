angular.module('App').controller('AssociacaoProgramasCadastroController', ['$scope', '$rootScope', 'httpService', '$location', '$timeout', '$routeParams', function ($scope, $rootScope, httpService, $location, $timeout, $routeParams) {


    //========================Recebe Parametro
    $scope.Parameters = $routeParams;

    $scope.associacaoprograma = "";

      if ($scope.Parameters.Action == 'New') {
        $rootScope.routeName = 'Inclusão de Associacao de Programas'
    }
    if ($scope.Parameters.Action == 'Edit') {
        $rootScope.routeName = 'Edição de Associação de Programas'
    }

    //==========================Busca dados do Associação dos Programas
    $scope.CarregarAssociacaoProgramas = function () {

        var _url = 'GetAssociacaoProgramas'
        _url += '?Cod_Programa=' + $scope.Parameters.Cod_Programa;
        _url += '&Cod_Empresa_Faturamento=' + $scope.Parameters.Cod_Empresa_Faturamento;
        _url += '&';

        httpService.Get(_url).then(function (response) {
            if (response) {

                $scope.associacaoprograma = response.data;
               

            }
        });
    }

    //==========================Salvar
          
    $scope.SalvarAssociacaoProgramas = function (pAssociacaoProgramas) {
        httpService.Post("SalvarAssociacaoProgramas", pAssociacaoProgramas).then(function (response) {
            if (response) {

                if (response.data[0].Status) {
                    ShowAlert(response.data[0].Mensagem, 'success');
                    $location.path("/AssociacaoProgramas")
                }
                else {
                    ShowAlert(response.data[0].Mensagem, 'warning');
                }
            }
        })
    };


    //===========================Cancelar Cadastro
    $scope.CancelarAssociacaoProgramas = function () {
        $location.path('/AssociacaoProgramas');
    };



    //===========================fim do load da pagina
    $scope.$watch('$viewContentLoaded', function () {
        $rootScope.routeloading = false;
        $scope.CarregarAssociacaoProgramas();
    });



}]);

