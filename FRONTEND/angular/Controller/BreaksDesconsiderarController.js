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
        $scope.Breaks.Composicao = [];
        $scope.ShowGrid = false;
        var _url = 'Roteiro/ListarBreak';
        _url += '?Cod_Veiculo=' + pFiltro.Cod_Veiculo;
        _url += '&Data_Exibicao=' + pFiltro.Data_Exibicao;
        _url += '&Cod_Programa=' + pFiltro.Cod_Programa;
        _url += '&';
        httpService.Get(_url).then(function (response) {
            if (response.data.Cod_Programa) {
                $scope.ShowFilter = false;
                $scope.ShowGrid = true;
                $scope.Breaks = response.data;
                $scope.Breaks.Data_Inicio_Propagacao = pFiltro.Data_Exibicao;
                $scope.Breaks.Data_Fim_Propagacao = pFiltro.Data_Exibicao;
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
        httpService.Post("Roteiro/GravarBreakDesconsiserado", pBreak).then(function (response) {
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
    //===========================Atualiza data da propagacao
    $scope.RefreshData = function (pBreak) {
        pBreak.Data_Inicio_Propagacao = $scope.Filtro.Data_Exibicao;
        pBreak.Data_Fim_Propagacao = pBreak.Ultimo_Dia_Break;
    };
    //===========================fim do load da pagina
    $scope.$watch('$viewContentLoaded', function () {
        
    });
}]);


