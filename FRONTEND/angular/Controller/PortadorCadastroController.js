angular.module('App').controller('PortadorCadastroController', ['$scope', '$rootScope', 'httpService', '$location', '$timeout', '$routeParams', function ($scope, $rootScope, httpService, $location, $timeout, $routeParams) {

    //========================Recebe Parametro
    $scope.Parameters = $routeParams;

    //========================Verifica Permissoes
    $scope.PermissaoDelete = false;
    httpService.Get("credential/Portador@Destroy").then(function (response) {
        $scope.PermissaoDelete = response.data;
    });
    //==========================Busca dados do Portador
    $scope.CarregaDados = function () {
        var _url = "GetPortadorData/" + $scope.Parameters.Id;
        httpService.Get(_url.trim()).then(function (response) {
            if (response) {
                $scope.portador = response.data;
            }
        });
    }
    $scope.CarregaDados();
    //==========================Salvar
    $scope.SalvarPortador = function (pPortador) {

        $scope.portador.Id_Operacao = $scope.Parameters.Action == "New" ? 'I' : 'E';
        httpService.Post("SalvarPortador", pPortador).then(function (response) {
            if (response) {

                if (response.data[0].Status) {

                    ShowAlert(response.data[0].Mensagem, 'success');
                    $location.path("/Portador")
                }
                else {
                    ShowAlert(response.data[0].Mensagem, 'warning');
                }
            }
        })
    };

    //======================Excluir
    $scope.excluirPortador = function (pPortador) {

        swal({
            title: "Tem certeza que deseja Excluir este Portador ?",
            //text: "Essa opcação desabilita o acesso ao sistema para esse usuário",
            type: "warning",
            showCancelButton: true,
            confirmButtonClass: "btn-danger",
            confirmButtonText: "Sim, Excluir?",
            cancelButtonText: "Cancelar",
            closeOnConfirm: true
        }, function () {
                httpService.Post("excluirPortador", pPortador).then(function (response) {
                if (response) {

                    if (response.data[0].Status) {
                        ShowAlert(response.data[0].Mensagem, 'success');
                        $location.path("/Portador");
                    }
                    else {
                        ShowAlert(response.data[0].Mensagem, 'warning');
                    }
                }
            })
        });

    };

}]);

