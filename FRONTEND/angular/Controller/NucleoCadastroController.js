angular.module('App').controller('NucleoCadastroController', ['$scope', '$rootScope', 'httpService', '$location', '$timeout', '$routeParams', function ($scope, $rootScope, httpService, $location, $timeout, $routeParams) {

    //========================Variaveis
    $scope.currentTab = "Dados";
    //========================Verifica Permissoes
    $scope.PermissaoNew = false;
    $scope.PermissaoEdit = false;
    $scope.PermissaoDesativar = false;
    $scope.PermissaoExcluir = false;
    httpService.Get("credential/Nucleo@New").then(function (response) {
        $scope.PermissaoNew = response.data;
    });
    httpService.Get("credential/Nucleo@Edit").then(function (response) {
        $scope.PermissaoEdit = response.data;
    });
    httpService.Get("credential/Nucleo@Destroy").then(function (response) {
        $scope.PermissaoExcluir = response.data;
    });
    httpService.Get("credential/Nucleo@Activate").then(function (response) {
        $scope.PermissaoDesativar = response.data;
    });

    //========================Recebe Parametro
    $scope.Parameters = $routeParams;
    $scope.Nucleo = "";
    console.log($scope.Parameters);

    //====================Inicializa scopes
    $scope.Filtro = {};
    $scope.NewFiltro = function () {
        const newLocal = $scope.Filtro = { 'Cod_Nucleo': '', 'Descricao': '', 'Cod_Empresa': '', 'Cod_Empresa_Fatura': '', 'Cod_Centro_Custo': '' };
        localStorage.removeItem('GetNucleoData');
    }

    //$scope.currentTab = 0;
    //======================Verifica se tem filtro anterior
    //var _Filter = JSON.parse(localStorage.getItem('GetNucleoData'));
    //if (!_Filter) {

    //    $scope.NewFiltro()
    //}
    //==========================Busca dados da Nucleo
    $scope.CarregaDados = function () {
        var _url = "GetNucleoData/" + $scope.Parameters.Id.trim();
        httpService.Get(_url).then(function (response) {
            if (response) {
                $scope.Filtro = response.data;
            }
        });
    }
    $scope.CarregaDados();

    //==========================Salvar
    $scope.SalvarNucleo = function (pFiltro) {
        if (pFiltro.Cod_Nucleo == "") {
            ShowAlert("Código so núcleo é de seleção obrigatorio!")
            return;
        }

        if (pFiltro.Descricao == "") {
            ShowAlert("Descrição do núcleo é de seleção obrigatoria!")
            return;
        }

        if (pFiltro.Cod_Empresa == "") {
            ShowAlert("Codigo da Empresa é de seleção obrigatoria!")
            return;
        }

        //if ($scope.Parameters.Action == "New")
        //{
        //    $scope.Veiculo.id_operacao = 'I';Parameters.Action
        //}
        //$scope.TipoMidia.id_operacao = $scope.sw == "New" ? 'I' : 'E';
        $scope.Filtro.Id_operacao = $scope.Parameters.Action == "New" ? 'I' : 'E';
        httpService.Post("SalvarNucleo", pFiltro).then(function (response) {
            if (response) {

                if (response.data[0].Status) {
                    ShowAlert(response.data[0].Mensagem, 'success');
                    if ($scope.Parameters.Action == 'New') {
                        $scope.CarregaDados();
                    }
                    else {
                        $location.path("/Nucleo")
                    }
                }
                else {
                    ShowAlert(response.data[0].Mensagem, 'warning');
                }
            }
        })
    };
    //======================Excluir
    $scope.ExcluirNucleo = function (pFiltro) {
        swal({
            title: "Tem certeza que deseja Excluir este Nucleo ?",
            //text: "Essa opcação desabilita o acesso ao sistema para esse usuário",
            type: "warning",
            showCancelButton: true,
            confirmButtonClass: "btn-danger",
            confirmButtonText: "Sim, Excluir?",
            cancelButtonText: "Cancelar",
            closeOnConfirm: true
        }, function () {
                httpService.Post("ExcluirNucleo", pFiltro).then(function (response) {
                if (response) {
                    if (response.data[0].Status) {
                        ShowAlert(response.data[0].Mensagem, 'success');
                        $location.path("/Nucleo");
                    }
                    else {
                        ShowAlert(response.data[0].Mensagem, 'warning');
                    }
                }
            })
        });
    };
}]);
