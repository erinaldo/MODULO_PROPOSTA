angular.module('App').controller('GradeMerchaCadastroController', ['$scope', '$rootScope', 'httpService', '$location', '$timeout', '$routeParams', function ($scope, $rootScope, httpService, $location, $timeout, $routeParams) {

    //================================Recebe Parametros
    $scope.Parameters = $routeParams;

    //======================================Inicializa Scopes
    $scope.Grade = "";
    $scope.PesquisaTabelas = { "Items": [], 'FiltroTexto': '', ClickCallBack: '', 'Titulo': '', 'MultiSelect': false };
    $scope.TituloVeiculo = ""
    if ($scope.Parameters.Action == 'New') {
        $scope.TituloVeiculo = "Criar Grade para os Veículos"
    }
    else {
        $scope.TituloVeiculo = "Alterar grade nos Veículos"
    }
    //========================Verifica Permissoes
    $scope.PermissaoNew = false;
    $scope.PermissaoEdit = false;
    $scope.PermissaoDelete = false;
    $scope.PermissaoActivate = false;
    

    httpService.Get("credential/GradeMercha@New").then(function (response) {
        $scope.PermissaoNew = response.data;
    });
    httpService.Get("credential/GradeMercha@Edit").then(function (response) {
        $scope.PermissaoEdit = response.data;
    });
    httpService.Get("credential/GradeMercha@Destroy").then(function (response) {
        $scope.PermissaoDelete = response.data;
    });
    httpService.Get("credential/GradeMercha@Activate").then(function (response) {
        $scope.PermissaoActivate = response.data;
    });
    
    //======================================Carrega Lista de Redes
    $scope.Redes = ""
    httpService.Get("ListarTabela/Rede/''").then(function (response) {
        $scope.Redes = response.data;
    });
    //======================================Carrega Lista de Faixa Horaira
    $scope.Faixas = ""
    httpService.Get("ListarTabela/Faixa_Horaria/''").then(function (response) {
        $scope.Faixas = response.data;
    });

    //======================================Carrega Dados da Grade
    $scope.CarregaGrade = function () {
        var _url = "GradeMercha/GetData/"
        _url += "?Action=" + $scope.Parameters.Action;
        _url += "&Cod_Veiculo=" + $scope.Parameters.Veiculo;
        _url += "&Data_Exibicao=" + $scope.Parameters.Data;
        _url += "&Cod_Programa=" + $scope.Parameters.Programa;
        _url += "&"
        httpService.Get(_url).then(function (response) {
            $scope.Grade = response.data;
        });
    };
    $scope.CarregaGrade();
    //======================================Pesquisa Programas da Rede
    $scope.PesquisaPrograma = function (pRedeId) {
        $scope.PesquisaTabelas = NewPesquisaTabela();
        httpService.Get('GradeMercha/GetProgramas?RedeId=' + pRedeId + '&').then(function (response) {
            if (response.data) {
                $scope.PesquisaTabelas.Items = response.data
                $scope.PesquisaTabelas.FiltroTexto = ""
                $scope.PesquisaTabelas.Titulo = "Seleção de Programas"
                $scope.PesquisaTabelas.MultiSelect = false;
                $scope.PesquisaTabelas.ClickCallBack = function (value) { $scope.Grade.Cod_Programa = value.Codigo; $scope.Grade.Nome_Programa = value.Descricao }
                $("#modalTabela").modal(true);
            }
        });
    }
    //===================================Validar o Programa
    $scope.fnChangePrograma = function (pRedeId, pCodPrograma) {
        $scope.Grade.Nome_Programa = "";
        $scope.Grade.Horario_Inicial = "";
        $scope.Grade.Horario_Final = "";
        $scope.Grade.Faixa_Horaria = -1

        if (!pCodPrograma) {
            return;
        };
        var _url = 'GradeMercha/GetProgramas?RedeId=' + pRedeId + '&Cod_Programa=' + pCodPrograma + '&'
        httpService.Get(_url).then(function (response) {
            if (response.data.length == 0) {
                ShowAlert("Programa Inválido", 'warning');
                $scope.Grade.Cod_Programa = ""
                $scope.Grade.Nome_Programa = ""
            }
            else {
                $scope.Grade.Nome_Programa = response.data[0].Descricao;
                $scope.Grade.Horario_Inicial = response.data[0].Horario_Inicial;
                $scope.Grade.Horario_Final = response.data[0].Horario_Final;
                $scope.Grade.Faixa_Horaria = response.data[0].Faixa_Horaria;
            }
        });
    }
    //==========================Marcar/Desmarcar todos os veiculos
    $scope.MarcarVeiculos = function (value) {
        for (var i = 0; i < $scope.Grade.Veiculos.length; i++) {
            $scope.Grade.Veiculos[i].Selected = value;
        }
    }
    //========================================Obtem o Ultimo Dia da Grade
    $scope.GetUltimoDiaGrade = function (pCodPrograma) {
        var _url = 'GradeMercha/GetUltimoDiaGrade?Cod_Programa=' + pCodPrograma + '&'
        httpService.Get(_url).then(function (response) {
            if (response.data) {
                $scope.Grade.Termino_Validade = response.data;
            }
        });
    }
    //========================================Mudou a Rede
    $scope.RedeChange = function (pRedeId) {
        $scope.Grade.Cod_Programa = "";
        $scope.Grade.Nome_Programa = "";
        var _url = 'GradeMercha/GetVeiculosRede/' + pRedeId
        httpService.Get(_url).then(function (response) {
            if (response.data) {
                $scope.Grade.Veiculos = response.data;
            }
        });
    };
    //========================================Salvar a Grade
    $scope.SalvarGrade = function (pGrade) {
        httpService.Post('GradeMercha/Salvar', pGrade).then(function (response) {
            if (response.data[0].Status == 1) {
                ShowAlert("Dados Gravados com Sucesso !", 'success')
                if ($scope.Parameters.Action == 'Edit') {
                    $location.path("/GradeMercha")
                }
                else {
                    $scope.CarregaGrade(); //permanece na tela para cadastrar mais grade
                }
            }
            else {
                ShowAlert(response.data[0].Mensagem, 'warning')

            }
        });
    };
    //========================================Excluir a Grade
    $scope.ExcluirGrade= function (pGrade) {
        swal({
            title: "Tem certeza que deseja Excluir essa Programação da Grade?",
            text: "Somente serão excluidas grades que não existam programação.",
            type: "warning",
            showCancelButton: true,
            confirmButtonClass: "btn-danger",
            confirmButtonText: "Sim, Excluir?",
            cancelButtonText: "Cancelar",
            closeOnConfirm: true
        }, function () {
            httpService.Post("GradeMercha/Excluir", pGrade).then(function (response) {
                if (response) {
                    if (response.data[0].Status) {
                        ShowAlert(response.data[0].Mensagem, 'success');
                        $location.path("/GradeMercha");
                    }
                    else {
                        ShowAlert(response.data[0].Mensagem, 'warning');
                    }
                }
            })
        });
    };
    //========================================Desativar/Reativar a Grade
    $scope.DesativarGrade = function (pGrade, pAction) {
        var _url = (pAction) ? "GradeMercha/Reativar" : "GradeMercha/Desativar"
        var _title = "Tem certeza que deseja "
        _title += (pAction ? " Reativar" : " Desativar")  + ' essa Grade ?'
        swal({
            title:_title,
            type: "warning",
            showCancelButton: true,
            confirmButtonClass: "btn-danger",
            confirmButtonText: "Sim",
            cancelButtonText: "Cancelar",
            closeOnConfirm: true
        }, function () {
            httpService.Post(_url, pGrade).then(function (response) {
                if (response) {
                    if (response.data[0].Status) {
                        ShowAlert(response.data[0].Mensagem, 'success');
                        $location.path("/GradeMercha");
                    }
                }
            })
        });
    };
}]);

