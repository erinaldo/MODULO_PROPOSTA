angular.module('App').controller('BreaksDesconsiderarController', ['$scope', '$rootScope', '$location', 'httpService', '$filter', function ($scope, $rootScope, $location, httpService, $filter, $routeParams) {


    //====================Inicializa scopes
    $scope.ShowStatus = false;
    $scope.Gravacao = false;
    $scope.ShowFilter = true;
    $scope.ShowGrid = false;
    $scope.Breaks = [];
    $scope.Critica= [];
    $scope.Intervalo = [{ 'Codigo': 0, 'Descricao': 'Local' }, { 'Codigo': 1, 'Descricao': 'Net' }, { 'Codigo': 2, 'Descricao': 'Artistico' }, { 'Codigo': 3, 'Descricao': 'Local PE' }];

    $scope.NewFiltro = function(){
        return { 'Cod_Veiculo': '', 'Nome_Veiculo': '', 'Data_Exibicao': '', 'Cod_Programa': '', 'Nome_Programa': '', 'Data_Inicio_Propagacao': '', 'Data_Fim_Propagacao': '' }
    };
    $scope.Filtro = $scope.NewFiltro();
    //====================Carrega Table de Breaks
    $scope.CarregarListaBreaks = function (pFiltro) {
        $rootScope.routeloading = true;
        $scope.Breaks= [];
        $scope.ShowGrid = false;
        var _url = 'Roteiro/ListarBreakDesconsiserado';
        _url += '?Cod_Veiculo=' + pFiltro.Cod_Veiculo;
        _url += '&Data_Exibicao=' + pFiltro.Data_Exibicao;
        _url += '&Cod_Programa=' + pFiltro.Cod_Programa;
        _url += '&';
        httpService.Get(_url).then(function (response) {
            if (response.data.length>0) {
                $scope.ShowFilter = false;
                $scope.ShowGrid = true;
                $scope.Breaks = response.data;
                $scope.Breaks[0].Data_Inicio_Propagacao = pFiltro.Data_Exibicao;
                $scope.Breaks[0].Data_Fim_Propagacao = pFiltro.Data_Exibicao;
            }
            else {
                ShowAlert("Não existem Breaks para esse filtro.")
            }
        });
    };
   
    //===========================Solicita data da Propagacao
    $scope.MostraData = function()
    {
        $scope.ShowFilter = false;
        $scope.ShowGrid = false;
        $scope.ShowGravacao = true;
    }
    $scope.CriticaOk = function()
    {
        $scope.ShowFilter = false
        $scope.ShowGrid = true;
        $scope.ShowGravacao = false;
        $scope.ShowStatus = false;
    }
    //===========================Bota Cancela
    $scope.CancelaBreak = function () {
        $scope.ShowFilter = true;
        $scope.ShowGrid = false;
        $scope.ShowGravacao = false;
        $scope.ShowStatus= false;
        $scope.Breaks = [];
        $scope.Critica = [];
        $scope.Filtro = $scope.NewFiltro();
    };
    $scope.GravarBreak = function (pBreak) {
        httpService.Post("Roteiro/GravarBreakDesconsiderado", pBreak).then(function (response) {
            if (!pBreak[0].Grade_Domingo && !pBreak[0].Grade_Segunda && !pBreak[0].Grade_Terca && !pBreak[0].Grade_Quarta && !pBreak[0].Grade_Quinta && !pBreak[0].Grade_Sexta && !pBreak[0].Grade_Sabado) {
                ShowAlert("Marque pelo menos um dia da semana.")
            }
            
            
            if (response.data) {
                if (response.data.length > 0) {
                    $scope.Critica = response.data;
                    $scope.ShowStatus = true;
                    $scope.ShowGravacao = false;
                    $scope.ShowFilter = false;
                    $scope.ShowGrid = false;
                }
                else {
                    ShowAlert("Dados Gravados com Sucesso.");
                    $scope.CriticaOk();
                };
            };
        });
    };
    //===============================Selecionar programa
    $scope.SelecionarPrograma = function (pFiltro) {
        if (!pFiltro.Cod_Veiculo || !pFiltro.Data_Exibicao) {
            ShowAlert("Informe o Veiculo e Data");
            pFiltro.Cod_Programa = "";
            pFiltro.Nome_Programa = ""
            return
        }
        $scope.PesquisaTabelas = NewPesquisaTabela();
        var _url = 'Roteiro/ProgramasBreak'
        _url += '?Cod_Veiculo=' + pFiltro.Cod_Veiculo;
        _url += '&Data_Exibicao=' + pFiltro.Data_Exibicao;
        _url += '&';

        httpService.Get(_url).then(function (response) {
            $scope.PesquisaTabelas.Titulo = "Selecionar Programas"
            if (response.data) {
                for (var i = 0; i < response.data.length; i++) {
                    if (response.data[i].Indica_Rotativo == 0) {
                        $scope.PesquisaTabelas.Items.push({ 'Codigo': response.data[i].Cod_Programa, 'Descricao': response.data[i].Nome_Programa });
                    };
                };

                $scope.PesquisaTabelas.FiltroTexto = ""
                $scope.PesquisaTabelas.MultiSelect = false;
                $scope.PesquisaTabelas.ClickCallBack = function (value) {
                    pFiltro.Cod_Programa = value.Codigo;
                    pFiltro.Nome_Programa = value.Descricao;
                };
            }
            $("#modalTabela").modal(true);
        });
    };

    //===============================Validar programa
    $scope.ProgramaChange = function (pFiltro) {
        if (!pFiltro.Cod_Veiculo || !pFiltro.Data_Exibicao) {
            ShowAlert("Informe o Veiculo e Data");
            pFiltro.Cod_Programa = "";
            pFiltro.Nome_Programa = ""
            return
        }
        var _url = 'Roteiro/ProgramasBreak'
        _url += '?Cod_Veiculo=' + pFiltro.Cod_Veiculo;
        _url += '&Data_Exibicao=' + pFiltro.Data_Exibicao;
        _url += '&Cod_Programa=' + pFiltro.Cod_Programa;
        _url += '&';

        httpService.Get(_url).then(function (response) {
            if (response.data.length == 0) {
                ShowAlert("Não existe grade para esse Programa");
                pFiltro.Cod_Programa = "";
                pFiltro.Nome_Programa = ""
            }
            else {
                pFiltro.Nome_Programa = response.data[0].Nome_Programa;
            }
        });
    };
    //===========================Atualiza data da propagacao
    $scope.RefreshData = function (pBreak) {
        $scope.Breaks[0].Data_Inicio_Propagacao = $scope.Filtro.Data_Exibicao;
        $scope.Breaks[0].Data_Fim_Propagacao = pBreak[0].Ultimo_Dia_Break;
    };
    //===========================fim do load da pagina
    $scope.$watch('$viewContentLoaded', function () {
        
    });
}]);


