angular.module('App').controller('MapaReservaDetalheController', ['$scope', '$rootScope', '$location', 'httpService', '$location', '$routeParams', function ($scope, $rootScope, $location, httpService, $location, $routeParams) {
    //========================Recebe Parametro
    $scope.Parameters = $routeParams;
    $scope.FiltroMidia = { 'Competencia': '', 'Cod_Veiculo': '', 'Indica_Demanda': true, 'Display': 1 };
    $scope.Display = [{ 'Id': 1, 'Nome': 'Midia Demandada' }, { 'Id': 2, 'Nome': 'Midia Exibivel' }, { 'Id': 3, 'Nome': 'Lista de Veiculações ' }]
    $scope.Contrato = {};
    $scope.Comerciais = [];
    $scope.Competencias = [];
    $scope.Veiculos = [];
    $scope.Midias = [];
    $scope.Resumos = [];
    $scope.Veiculacoes = [];
    $scope.Competencia_Text = ""
    $scope.ShowBaixa = false;
    $scope.ShowMidia = true;
    $scope.DadosBaixa = {};
    $scope.yPosition = 0
    $scope.AvisoBaixa = "";
    //==========================Verifica se Tem Permissao para baixa
    $scope.PermissaoBaixa = false;
    httpService.Get("credential/baixa@veiculacao").then(function (response) {
        $scope.PermissaoBaixa = response.data;
    });


    //==========================Carrega Dados do Contrato
    $scope.CarregaContrato = function () {
        var _url = "MapaReserva/DetalheContrato/" + $scope.Parameters.Id;
        httpService.Get(_url).then(function (response) {
            if (response) {
                $scope.Contrato = response.data[0];
                $scope.CarregaComerciais();
            }
        });
    };
    //==========================Carrega Dados dos  Comerciais
    $scope.CarregaComerciais = function () {
        var _url = "MapaReserva/DetalheComercial/" + $scope.Parameters.Id;
        httpService.Get(_url).then(function (response) {
            if (response) {
                $scope.Comerciais = response.data;
                $scope.CarregaCompetencias();
            }
        });
    };
    //==========================Carrega Competencias
    $scope.CarregaCompetencias = function () {
        var _url = "MapaReserva/DetalheCompetencia/" + $scope.Parameters.Id;
        httpService.Get(_url).then(function (response) {
            if (response.data.length > 0) {
                $scope.Competencias = response.data;
                $scope.FiltroMidia.Competencia = response.data[0].Competencia_Int;
                var _year = parseInt($scope.FiltroMidia.Competencia.substr(0, 4));
                var _mes = parseInt($scope.FiltroMidia.Competencia.substr(4, 2));
                $scope.Competencia_Text = MesExtenso($scope.FiltroMidia.Competencia)
                $scope.CarregaVeiculos();
            }
        });
    };
    //==========================Carrega Veiculos
    $scope.CarregaVeiculos = function () {
        var _url = "MapaReserva/DetalheVeiculo/" + $scope.Parameters.Id;
        httpService.Get(_url).then(function (response) {
            if (response) {
                $scope.Veiculos = response.data;
                $scope.FiltroMidia.Cod_Veiculo = response.data[0].Cod_Veiculo
                $scope.CarregaMidia();
                $scope.CarregaResumo();
            }
        });
    };
    //==========================Carrega Midia
    $scope.CarregaMidia = function () {
        if ($scope.FiltroMidia.Display == 3) {
            var _url = "MapaReserva/DetalheVeiculacao"
            _url += "?Id_Contrato=" + $scope.Parameters.Id;
            _url += "&Competencia=" + $scope.FiltroMidia.Competencia;
            _url += "&Cod_Veiculo=" + $scope.FiltroMidia.Cod_Veiculo;
            _url += "&Display=" + $scope.FiltroMidia.Display;
            _url += "&"
            httpService.Get(_url).then(function (response) {
                if (response) {
                    $scope.Veiculacoes = response.data;
                };
            });
        }
        else {
            var _url = "MapaReserva/DetalheMidia"
            _url += "?Id_Contrato=" + $scope.Parameters.Id;
            _url += "&Competencia=" + $scope.FiltroMidia.Competencia;
            _url += "&Cod_Veiculo=" + $scope.FiltroMidia.Cod_Veiculo;
            _url += "&Display=" + $scope.FiltroMidia.Display;
            _url += "&"
            httpService.Get(_url).then(function (response) {
                if (response) {
                    $scope.Midias = response.data;
                };
            });
        }
        
        $scope.$digest;
    };
    //==========================Carrega Midia
    $scope.CarregaResumo = function () {

        var _url = "MapaReserva/DetalheResumo"
        _url += "?Id_Contrato=" + $scope.Parameters.Id;
        _url += "&Competencia=" + $scope.FiltroMidia.Competencia;
        _url += "&Cod_Veiculo=" + $scope.FiltroMidia.Cod_Veiculo;
        _url += "&Display=" + $scope.FiltroMidia.Display;
        _url += "&"
        httpService.Get(_url).then(function (response) {
            if (response) {
                $scope.Resumos = response.data;
            };
        });

    };
    //==========================Baixar Veiculacao
    $scope.BaixarVeiculacao = function (pVeiculacao) {
        $scope.yPosition = (window.scrollY);
        var _Titulo_Comercial = '';
        var _Duracao = '';
        for (var i = 0; i < $scope.Comerciais.length; i++) {
            if ($scope.Comerciais[i].Cod_Comercial.trim() == pVeiculacao.Cod_Comercial.trim()) {
                _Titulo_Comercial = $scope.Comerciais[i].Titulo_Comercial;
                _Duracao = $scope.Comerciais[i].Duracao;
            };
        };

        $scope.DadosBaixa = {
            'Cod_Veiculo': pVeiculacao.Cod_Veiculo,
            'Data_Exibicao': pVeiculacao.Data_Exibicao,
            'Cod_Programa': pVeiculacao.Cod_Programa,
            'Chave_Acesso': pVeiculacao.Chave_Acesso,
            'Cod_Comercial': pVeiculacao.Cod_Comercial,
            'Titulo_Comercial': _Titulo_Comercial,
            'Duracao': _Duracao,
            'Cod_Qualidade': pVeiculacao.Cod_Qualidade,
            'Horario_Exibicao': pVeiculacao.Horario_Exibicao,
            'Cod_Empresa': $scope.Contrato.Cod_Empresa,
            'Numero_Mr': $scope.Contrato.Numero_Mr,
            'Sequencia_Mr': $scope.Contrato.Sequencia_Mr,
            'Documento_De': pVeiculacao.Documento_De,
            'Documento_Para': pVeiculacao.Documento_Para
        };
        $scope.ShowBaixa = true;
        $scope.ShowMidia = false;
    }
    //==========================Cancelar Baixar Veiculacao
    $scope.CancelarBaixar = function () {
        $scope.ShowBaixa = false;
        $scope.AvisoBaixa = "";
        $scope.ShowMidia = true;
        setTimeout(function () {
            window.scroll(0, $scope.yPosition);
        }, 100)
    };

    //=============BaixarVeiculacao
    $scope.SalvarBaixarVeiculacao = function (pDadosBaixa) {
        httpService.Post('MapaReserva/BaixarVeiculacao', pDadosBaixa).then(function (response) {
            if (response.data) {
                if (response.data.Status == 0) {
                    $scope.AvisoBaixa = response.data.Critica;
                    return;
                }
                else {
                    for (var i = 0; i < $scope.Veiculacoes.length; i++) {
                        if ($scope.Veiculacoes[i].Cod_Veiculo == pDadosBaixa.Cod_Veiculo &&
                            $scope.Veiculacoes[i].Data_Exibicao == pDadosBaixa.Data_Exibicao
                            && $scope.Veiculacoes[i].Cod_Programa == pDadosBaixa.Cod_Programa
                            && $scope.Veiculacoes[i].Chave_Acesso == pDadosBaixa.Chave_Acesso)
                        {
                            $scope.Veiculacoes[i].Cod_Qualidade = response.data.Cod_Qualidade;
                            $scope.Veiculacoes[i].Horario_Exibicao= response.data.Horario_Exibicao;
                            $scope.Veiculacoes[i].Documento_De = response.data.Documento_De;
                            $scope.Veiculacoes[i].Documento_Para = response.data.Documento_Para;
                            break;
                        }
                    };
                    $scope.CancelarBaixar();
                };
            };
        });
    }
    //===========================Mudou o Horario de exibicao na baix
    $scope.ChangeHorario = function (pData) {
        if (pData.Horario_Exibicao) {
            pData.Cod_Qualidade = 'VEI';
        }
        else {
            pData.Cod_Qualidade = '';
        }
    }
    //===========================fim do load da pagina
    $scope.$watch('$viewContentLoaded', function () {
        $scope.CarregaContrato();
    });

}]);


