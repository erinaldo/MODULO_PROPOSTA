angular.module('App').controller('RetornarEMSController', ['$scope', '$rootScope', '$location', 'httpService', '$location', function ($scope, $rootScope, $location, httpService, $location) {

    //====================Inicializa scopes
    $scope.Filtro = {};

    //====================Inicializa o Filtro
    $scope.NovoFiltro = function () {
        $scope.Filtro = {
            'Cod_Empresa_Faturamento': '',
            'Nome_Empresa_Faturamento': '',
            'Indica_Lista_Negra': false,
            'Indica_Baixa_Duplicata': false,
            'Indica_Numeracao_Fatura': false,
            'Data_Inicial': '',
            'Data_Final': ''
        };
    }

    $scope.NovoFiltro()



    //====================Processa Integrações de Retorno do EMS
    $scope.ProcessarRetornoEMS = function (pFiltro) {
        if (!pFiltro.Cod_Empresa_Faturamento) {
            ShowAlert("A Empresa de Faturamento não pode ficar em branco");
            return;
        }
        if (!pFiltro.Indica_Lista_Negra && !pFiltro.Indica_Baixa_Duplicata && !pFiltro.Indica_Numeracao_Fatura) {
            ShowAlert("Escolha uma opção de integração");
            return;
        }

        if (pFiltro.Indica_Numeracao_Fatura) {
            if (!pFiltro.Data_Inicial) {
                ShowAlert("Data Inicial não pode ficar em branco");
                return;
            }
            if (!pFiltro.Data_Final) {
                ShowAlert("Data Final não pode ficar em branco");
                return;
            }
            var _dia = parseInt(pFiltro.Data_Inicial.substr(0, 2));
            var _mes = parseInt(pFiltro.Data_Inicial.substr(3, 2));
            var _ano = parseInt(pFiltro.Data_Inicial.substr(6, 4));
            var _diaInicio = new Date(_ano, _mes - 1, _dia)
            _dia = parseInt(pFiltro.Data_Final.substr(0, 2));
            _mes = parseInt(pFiltro.Data_Final.substr(3, 2));
            _ano = parseInt(pFiltro.Data_Final.substr(6, 4));
            var _diaFim = new Date(_ano, _mes - 1, _dia)
            if (_diaFim < _diaInicio) {
                ShowAlert("Data Final não pode ser menor que a Data Inicial");
                return;
            }
        }

        swal({
            title: "Confirma a(s) integração(ões) de Retorno do EMS ?",
            type: "warning",
            showCancelButton: true,
            confirmButtonClass: "btn-danger",
            confirmButtonText: "Sim, Confirmar",
            cancelButtonText: "Cancelar",
            closeOnConfirm: true
        }, function () {
            httpService.Post("ProcessarRetorno", pFiltro).then(function (response) {
                
                if (response.data) {
                    //nsole.log(response);
                    ShowAlert(response.data.Message);
                    return;
                }
            });
        });     //------fim do swal

    }





}]);


