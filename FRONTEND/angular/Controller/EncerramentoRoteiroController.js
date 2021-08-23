angular.module('App').controller('EncerramentoRoteiroController', ['$scope', '$rootScope', 'httpService', '$location', '$timeout', '$routeParams', function ($scope, $rootScope, httpService, $location, $timeout) {

    //---------------------- Inicializa Scope -------------------------
    

    //---------------------- Novo Filtro ------------------------
    $scope.NewFilter = function () {
        $scope.Filtro = { 'Cod_Veiculo': '', 'Nome_Veiculo': '', 'Data_Exibicao': '', 'Programas': [] };
        $scope.ShowFilter = true;
        $scope.ShowCritica = false
    };
    $scope.NewFilter();
    $scope.newRoteiroConsistencia = function () {
        return { 'Comerciais': [], 'FitaPendente': [], 'Break': [], 'Restricao': [], 'Concorrencia': [] }
    }
    $scope.newRoteiroConsistencia = function () {
        return { 'Comerciais': [], 'FitaPendente': [], 'Break': [], 'Restricao': [], 'Concorrencia': [] }
    }
    $scope.RoteiroConsistencia = $scope.newRoteiroConsistencia();

    //---------------------- Carrega Roteiro -----------------------
    $scope.CarregarRoteiro = function (pFiltro) {
        $scope.Roteiro = "";
        $scope.Comerciais = "";
        $scope.RoteiroConsistencia = $scope.newRoteiroConsistencia();
        var _Url = 'Roteiro/CarregarRoteiro';
        httpService.Post(_Url, pFiltro).then(function (response) {
            $scope.Comerciais = [];
            if (response.data.length == 0) {
                ShowAlert("Não existe Roteiro para esse Veiculo/Data");
            }
            else {
                $scope.Roteiro = response.data;
                $scope.ShowFilter = false;
                $scope.ShowCritica = true;
                $scope.RoteiroConsistencia = $scope.newRoteiroConsistencia();
                httpService.Post("Roteiro/CarregarComerciais", pFiltro).then(function (responseComercial) {
                    if (responseComercial.data) {
                        $scope.Comerciais = responseComercial.data;
                    }
                    $scope.ConsistirOrdenacao($scope.Roteiro);
                    $scope.RenumeraItens($scope.Roteiro);
                });
            };
        });
    };
    //===========================Consistir Ordenacao
    $scope.ConsistirOrdenacao = function (pRoteiro) {
        $scope.CurrentCheck = 'checkBreak';
        $scope.RoteiroConsistencia.Comerciais = $scope.Comerciais.filter(elem => elem.Origem == 'Midia' && !elem.Indica_Titulo_Programa && !elem.Indica_Ordenado);
        $scope.RoteiroConsistencia.FitaPendente = $scope.Roteiro.filter(elem => elem.Indica_Comercial && !elem.Numero_Fita && elem.Permite_Ordenacao);
        $scope.RoteiroConsistencia.Break = $scope.Roteiro.filter(elem => elem.Indica_Titulo_Intervalo || elem.Indica_Titulo_Programa);
        $scope.RoteiroConsistencia.Restricao = $scope.Roteiro.filter(elem => elem.Indica_Comercial && Date.parse(elem.Horario_Restricao) > Date.parse(elem.Hora_Inicio_Programa) && elem.Horario_Restricao != '0001-01-01T00:00:00');
        $scope.RoteiroConsistencia.Concorrencia = [];
        for (var i = 0; i < $scope.Roteiro.length; i++) {
            if ($scope.Roteiro[i].Indica_Comercial) {
                $scope.addMatrizProduto($scope.Roteiro[i]);
            };
        };
    };
    //===========================Adiciona Matriz de Produto para mostrar consistencia do choque de concorrencia
    $scope.addMatrizProduto = function (pItem) {
        var _index = -1
        for (var i = 0; i < $scope.RoteiroConsistencia.Concorrencia.length; i++) {
            if ($scope.RoteiroConsistencia.Concorrencia[i].Cod_Programa == pItem.Cod_Programa && $scope.RoteiroConsistencia.Concorrencia[i].Break == pItem.Break && $scope.RoteiroConsistencia.Concorrencia[i].Cod_Produto_Root == pItem.Cod_Produto_Root) {
                $scope.RoteiroConsistencia.Concorrencia[i].Posicao = $scope.RoteiroConsistencia.Concorrencia[i].Posicao + pItem.Sequencia_Intervalo.toString() + ",";
                $scope.RoteiroConsistencia.Concorrencia[i].Qtd++;
                _index = i;
                break;
            };
        };
        if (_index == -1) {
            $scope.RoteiroConsistencia.Concorrencia.push({
                'Cod_Programa': pItem.Cod_Programa,
                'Titulo_Programa': pItem.Titulo_Programa,
                'Cod_Comercial': pItem.Cod_Comercial,
                'Titulo_Comercial': pItem.Titulo_Comercial,
                'Cod_Produto_Root': pItem.Cod_Produto_Root,
                'Nome_Produto_Root': pItem.Nome_Produto_Root,
                'Break': pItem.Break,
                'Posicao': pItem.Sequencia_Intervalo.toString() + ',',
                'Qtd': 1
            });
        };
    };
    $scope.RenumeraItens = function (pRoteiro) {
        var _sequenciaIntervalo = 0;
        for (var i = 0; i < pRoteiro.length; i++) {
            if (pRoteiro[i].Indica_Titulo_Break) {
                _sequenciaIntervalo = 0;
            }
            if (pRoteiro[i].Indica_Comercial) {
                _sequenciaIntervalo++;
            }
            pRoteiro[i].Id_Item = i;
            pRoteiro[i].Sequencia_Intervalo = _sequenciaIntervalo;
        }
        for (var x = 0; x < $scope.Comerciais.length; x++) {
            $scope.Comerciais[x].Id_Item = x;
        }

        var _totalIntervalo = 0;
        var _totalBreak = 0
        var _totalArtistico = 0
        var _total_Encaixe_Programa = 0;
        var _total_Duracao_Programa = 0
        for (var i = pRoteiro.length - 1; i >= 0; i--) {
            if (pRoteiro[i].Indica_Comercial) {
                if (pRoteiro[i].Origem == 'Artistico') {
                    _totalArtistico += pRoteiro[i].Duracao;
                }
                else {
                    _totalIntervalo += pRoteiro[i].Duracao;
                }
                _totalBreak += pRoteiro[i].Duracao;
                _total_Encaixe_Programa += pRoteiro[i].Duracao;

            };
            if (pRoteiro[i].Indica_Titulo_Intervalo) {
                if (pRoteiro[i].Tipo_Break == '2') {
                    pRoteiro[i].Encaixe = _totalArtistico;
                    _totalArtistico = 0;
                }
                else {
                    pRoteiro[i].Encaixe = _totalIntervalo;
                    _totalIntervalo = 0;
                }
            };
            if (pRoteiro[i].Indica_Titulo_Break) {
                pRoteiro[i].Encaixe = _totalBreak;
                _total_Duracao_Programa += pRoteiro[i].Duracao;
                _totalBreak = 0;
                _totalArtistico = 0;
                _totalArtistico;
                _totalIntervalo
            };
            if (pRoteiro[i].Indica_Titulo_Programa) {
                pRoteiro[i].Encaixe = _total_Encaixe_Programa;
                pRoteiro[i].Duracao = _total_Duracao_Programa;
                _totalBreak = 0;
                _totalArtistico = 0;
                _totalArtistico;
                _totalIntervalo;
                var _total_Encaixe_Programa = 0;
                var _total_Duracao_Programa = 0
            };
        };
    };
    //---------------------- Encerra Roteiro -----------------------
    $scope.Encerrar = function (pFiltro) {
        if (!pFiltro.Cod_Veiculo) {
            ShowAlert("O Veículo é Obrigatório");
            return;
        }
        if (!pFiltro.Data_Exibicao) {
            ShowAlert("A Data é Obrigatória");
            return;
        }

        httpService.Post('Roteiro/EncerrarRoteiro', pFiltro).then(function (response) {
            if (response) {
                if (response.data[0].Status) {
                    ShowAlert(response.data[0].Mensagem, 'success');
                    $scope.NewFilter();
                    $scope.Roteiro = "";
                    $scope.NewFilter();
                    $scope.RoteiroConsistencia = $scope.newRoteiroConsistencia();
                    $scope.ShowCritica = false;
                    $scope.ShowFilter = true;
                }
                else {
                    ShowAlert(response.data[0].Mensagem, 'warning');
                    $scope.NewFilter();
                }
            }
        });
    };


}]);

