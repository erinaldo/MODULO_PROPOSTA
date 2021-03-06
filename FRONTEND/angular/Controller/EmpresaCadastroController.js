angular.module('App').controller('EmpresaCadastroController', ['$scope', '$rootScope', 'httpService', '$location', '$timeout', '$routeParams', function ($scope, $rootScope, httpService, $location, $timeout, $routeParams) {

    //========================Recebe Parametro
    $scope.Parameters = $routeParams;
    $scope.Empresa = "";
    //========================Verifica Permissoes
    $scope.PermissaoDelete= false;
    httpService.Get("credential/Empresa@Destroy").then(function (response) {
        $scope.PermissaoDelete = response.data;
    });

    //========================Verifica Permissoes
    $scope.PermissaoNew = false;
    $scope.PermissaoEdit = false;
    httpService.Get("credential/Empresa@New").then(function (response) {
        $scope.PermissaoNew = response.data;
    });
    httpService.Get("credential/Empresa@Edit").then(function (response) {
        $scope.PermissaoEdit = response.data;
    });
    //==========================Busca dados das Empresa
    var _url = "GetEmpresaData/" + $scope.Parameters.Id;
    httpService.Get(_url).then(function (response) {
        if (response) {
            $scope.Empresa = response.data;
        }
    });

    //==========================Salvar
    $scope.SalvarEmpresa = function (pEmpresa) {
        $scope.Empresa.id_operacao = $scope.Parameters.Action == "New" ? 'I' : 'E';
        httpService.Post("SalvarEmpresa", pEmpresa).then(function (response) {
            if (response) {

                if (response.data[0].Status) {
                    ShowAlert(response.data[0].Mensagem, 'success');
                    $location.path("/Empresa");
                }
                else {
                    ShowAlert(response.data[0].Mensagem, 'warning');
                }
            }
        })
    };

    //======================Excluir
    $scope.ExcluirEmpresa = function (pEmpresa) {

        swal({
            title: "Tem certeza que deseja Excluir esta  Empresa ?",
            //text: "Essa opcação desabilita o acesso ao sistema para esse usuário",
            type: "warning",
            showCancelButton: true,
            confirmButtonClass: "btn-danger",
            confirmButtonText: "Sim, Excluir?",
            cancelButtonText: "Cancelar",
            closeOnConfirm: true
        }, function () {
            httpService.Post("excluirEmpresa", pEmpresa).then(function (response) {
                if (response) {

                    if (response.data[0].Status) {
                        ShowAlert(response.data[0].Mensagem, 'success');
                        $location.path("/Empresa");
                    }
                    else {
                        ShowAlert(response.data[0].Mensagem, 'warning');
                    }
                }
            })
        });

    };

}]);

