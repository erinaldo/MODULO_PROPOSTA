angular.module('App').controller('AssociacaoContatosCadastroController', ['$scope', '$rootScope', 'httpService', '$location', '$timeout', '$routeParams', function ($scope, $rootScope, httpService, $location, $timeout, $routeParams) {
                                

    //========================Recebe Parametro
    $scope.Parameters = $routeParams;

    if ($scope.Parameters.Action == 'New') {
        $rootScope.routeName = 'Inclusão de Associacao de Contatos'
    }
    if ($scope.Parameters.Action == 'Edit') {
        $rootScope.routeName = 'Edição de Associação de Contatos'
    }

    //$scope.AssociacaoContato = "";
    //==========================Busca dados do Contato
    $scope.CarregarAssociacaoContatos = function () {
        $scope.associacaocontato = [];
        var _url = "GetAssociacaoContatosData/" + $scope.Parameters.Id;
        httpService.Get(_url).then(function (response) {
            if (response) {
                $scope.associacaocontato = response.data;
                if ($scope.associacaocontato.Cod_Representante == 0) {
                    $scope.associacaocontato.Cod_Representante = "";
                };


            }
        });
    }
   
    //==========================Salvar
    $scope.SalvarContato = function (pAssociacaoContato) {
        httpService.Post("SalvarAssociacaoContatos", pAssociacaoContato).then(function (response) {
            if (response) {

                if (response.data[0].Status) {
                    ShowAlert(response.data[0].Mensagem, 'success');
                    $location.path("/AssociacaoContatos")
                }
                else {
                    ShowAlert(response.data[0].Mensagem, 'warning');
                }
            }
        })
    };

    
    //===========================Cancelar Cadastro
    $scope.CancelarAssociacaoContatos = function () {
        $location.path('/AssociacaoContatos');
    };



    //===========================fim do load da pagina
    $scope.$watch('$viewContentLoaded', function () {
        $scope.CarregarAssociacaoContatos($scope.Parameters.Id);
        $rootScope.routeloading = false;
    });



}]);

