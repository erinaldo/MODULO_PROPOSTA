angular.module('App').controller('DepositorioFitasCadastroController', ['$scope', '$rootScope', 'httpService', '$location', '$timeout', '$routeParams', function ($scope, $rootScope, httpService, $location, $timeout, $routeParams) {

    //========================Recebe Parametro
    $scope.PesquisaTabelas = { "Items": [], 'FiltroTexto': '', ClickCallBack: '', 'Titulo': '', 'MultiSelect': false };
    $scope.Parameters = $routeParams;
    $scope.ListadeVeiculos = [];
    $scope.DepositorioFitas = "";
    $scope.DepositorioFitas.Veiculos = [];
    $scope.currentVeiculos = 0;
    $scope.PosTipoFita = [
        { 'id': 1, 'nome': 'Avulso' },
        { 'id': 2, 'nome': 'Artistico' }
    ]

    //========================Verifica Permissoes

    $scope.PermissaoDelete = false;
    httpService.Get("credential/DepositorioFitas@Destroy").then(function (response) {
        $scope.PermissaoDelete = response.data;
    });

    //====================Formata o Numero da Fita
    $scope.FormataNumeroFita = function (pNumeroFita) {
        if (pNumeroFita.Numero_Fita) {
            var _letra = pNumeroFita.Tipo_Fita == 1 ? "CO" : "AR";
            pNumeroFita.Numero_Fita = pNumeroFita.Numero_Fita.replace(/[^0-9]/g, '')
            pNumeroFita.Numero_Fita = '000000' + pNumeroFita.Numero_Fita;
            pNumeroFita.Numero_Fita = pNumeroFita.Numero_Fita.slice(pNumeroFita.Numero_Fita.length - 6);
            pNumeroFita.Numero_Fita = _letra + pNumeroFita.Numero_Fita;
        };
    };
    //==========================Busca dados dos Depositorio Fitas
    var _url = "GetDepositorioFitasData/" + $scope.Parameters.Id;
    httpService.Get(_url).then(function (response) {

        if (response) {
            $scope.DepositorioFitas = response.data;
            if ($scope.Parameters.Action == "New") {
                $scope.DepositorioFitas.Tipo_Fita = "";
                $scope.DepositorioFitas.Quantidade = "";
                $scope.DepositorioFitas.Duracao = "";
                $scope.DepositorioFitas.Cod_Red_Produto = "";

            }

            if ($scope.Parameters.Action == "Edit") {
                if (response.data.Tipo_Fita == "Avulso") {
                    $scope.DepositorioFitas.Tipo_Fita = 1;
                }
                else {
                    $scope.DepositorioFitas.Tipo_Fita = 2;
                }

                if ($scope.DepositorioFitas.Cod_Red_Produto == 0)
                {
                    $scope.DepositorioFitas.Cod_Red_Produto = ""
                }


            }


        }
    });

    ////==========================Salvar
    $scope.SalvarDepositorioFitas = function (pDepositorioFitas) {
        $scope.DepositorioFitas.id_operacao = $scope.Parameters.Action == "New" ? 'I' : 'E';

        if (pDepositorioFitas.Data_Inicio > pDepositorioFitas.Data_Final) {

            ShowAlert("Data Inicio da Fita não pode ser maior que a Data Final!");
            return;
        }

        if (pDepositorioFitas.Data_Inicio == null && pDepositorioFitas.Data_Inicio == undefined) {

            ShowAlert("Data Inicio da Fita não pode ficar em branco");
            return;
        }


        if (pDepositorioFitas.Data_Final == null && pDepositorioFitas.Data_Final == undefined) {

            ShowAlert("Data Final da Fita não pode ficar em branco");
            return;
        }


        if (pDepositorioFitas.Numero_Fita == null && pDepositorioFitas.Numero_Fita == undefined) {

            ShowAlert("Número de Fita não pode ficar em branco");
            return;
        }


        if (pDepositorioFitas.Titulo_Comercial == null && pDepositorioFitas.Titulo_Comercial == undefined) {

            ShowAlert("Titulo  de Fita não pode ficar em branco");
            return;
        }

        if (pDepositorioFitas.Quantidade=="") {

            ShowAlert("Quantidade  de Fita não pode ficar em branco");
            return;
        }

        if (pDepositorioFitas.Duracao=="") {

            ShowAlert("Duração  de Fita não pode ficar em branco");
            return;
        }

        if (pDepositorioFitas.Cod_Tipo_Comercial == null && pDepositorioFitas.Cod_Tipo_Comercial == undefined) {

            ShowAlert("Código de tipo Comercial  de Fita não pode ficar em branco");
            return;
        }

        if (pDepositorioFitas.Cod_Veiculo == null && pDepositorioFitas.Cod_Veiculo == undefined) {
            ShowAlert("Código de Veículo  de Fita não pode ficar em branco");
            return;
        }

        var ct_Arquivo_Midia = document.getElementById('txtArquivoMidia').value;
        pDepositorioFitas.Arquivo_Midia = ct_Arquivo_Midia;

        if (pDepositorioFitas.Tipo_Fita == 1) {
            pDepositorioFitas.Letra= 'CO';
        }
        else {
            pDepositorioFitas.Letra = 'AR'
            
        }

        httpService.Post("SalvarDepositorioFitas", pDepositorioFitas).then(function (response) {
            if (response) {

                if (response.data[0].Status) {
                    ShowAlert(response.data[0].Mensagem, 'success');
                    $location.path("/DepositorioFitas");
                }
                else {
                    ShowAlert(response.data[0].Mensagem, 'warning');
                }
            }
        })
    };


    //======================Excluir
    $scope.ExcluirDepositorioFitas = function (pDepositorioFitas) {

        swal({
            title: "Tem certeza que deseja Excluir esta  Fita ?",
            //text: "Essa opcação desabilita o acesso ao sistema para esse usuário",
            type: "warning",
            showCancelButton: true,
            confirmButtonClass: "btn-danger",
            confirmButtonText: "Sim, Excluir?",
            cancelButtonText: "Cancelar",
            closeOnConfirm: true
        }, function () {
           
            var _data = {
                "Cod_Veiculo": pDepositorioFitas.Cod_Veiculo,
                "Numero_Fita": pDepositorioFitas.Numero_Fita,
                "Tipo_Fita": pDepositorioFitas.Numero_Fita.substr(0, 2)
            };

            httpService.Post("ExcluirDepositorioFitas", _data).then(function (response) {
                if (response) {

                    if (response.data[0].Status) {
                        ShowAlert(response.data[0].Mensagem, 'success');
                        $location.path("/DepositorioFitas");
                    }
                    else {
                        ShowAlert(response.data[0].Mensagem, 'warning');
                    }
                }
            })
        });

    };
  
 
    //=====================Carregar Numero de Fita 
    $scope.CarregarNumeroFita = function (pNumero_Fita) {
        var _Tipo = "";

        if ($scope.DepositorioFitas.Tipo_Fita == "") {
            ShowAlert("Para utilizar numeração automática, primeiro selecione Tipo de Fita");
            return;
        }

        if ($scope.DepositorioFitas.Cod_Veiculo == null && $scope.DepositorioFitas.Cod_Veiculo == undefined) {

            ShowAlert("Para utilizar numeração automática, primeiro selecione um veiculo");
            return;
        }

        if (pNumero_Fita.Tipo_Fita == 1) {
            _Tipo= 'CO'
        }
        else {
            _Tipo= 'AR'
            
        }
        var _data = {
            'Cod_Veiculo': pNumero_Fita.Cod_Veiculo,
            'Tipo_Fita': _Tipo,
            'Tipo_Midia': '',
            'Cod_Tipo_Comercial': pNumero_Fita.Cod_Tipo_Comercial
        };

        httpService.Post("RangeFita", _data).then(function (response) {
            if (response) {
                if (response.data[0].Status == 0) {
                    ShowAlert('Veículo não esta parametrizado corretamente em Paramêtros de Numeração de Fitas');
                }
                else {
                    $scope.DepositorioFitas.Numero_Fita = response.data[0].Numero_Fita;
                }
            }
        })    
    }


    //=====================Marcar / Desmarcar todos os Reencaixe
    $scope.MarcaDepositorioFitas = function () {
        if ($scope.DepositorioFitas.MarcarDesmarcar == true) {
            $scope.DepositorioFitas.Indica_DiaDom = true;
            $scope.DepositorioFitas.Indica_DiaSeg = true;
            $scope.DepositorioFitas.Indica_DiaTer = true;
            $scope.DepositorioFitas.Indica_DiaQua = true;
            $scope.DepositorioFitas.Indica_DiaQui = true;
            $scope.DepositorioFitas.Indica_DiaSex = true;
            $scope.DepositorioFitas.Indica_DiaSab = true;
        }
        else {
            $scope.DepositorioFitas.Indica_DiaDom = false;
            $scope.DepositorioFitas.Indica_DiaSeg = false;
            $scope.DepositorioFitas.Indica_DiaTer = false;
            $scope.DepositorioFitas.Indica_DiaQua = false;
            $scope.DepositorioFitas.Indica_DiaQui = false;
            $scope.DepositorioFitas.Indica_DiaSex = false;
            $scope.DepositorioFitas.Indica_DiaSab = false;
        }

    }



}]);

