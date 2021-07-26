angular.module('App').controller('GrupoUsuarioController', ['$scope', '$rootScope', 'httpService', '$location','$timeout', function ($scope, $rootScope, httpService, $location,$timeout) {

    //====================Inicializa scopes
    $scope.CurrentShow = "Grid";
    $scope.CurrentTab = "Perfil";
    $scope.checkBoxPerfil = false;
    $scope.checkBoxAssociar = false;
    $scope.NewGrupo= function () {
        $scope.Ctrl = { 'Id':'','Codigo': '', 'Descricao': '', 'Status':false,'Perfil':[],'Usuarios':[]};
    }
    $scope.Ctrl = $scope.NewGrupo();
    $scope.gridheaders = [{ 'title': '#ID', 'visible': true, 'searchable': false, 'sortable': true },
                            { 'title': 'Código', 'visible': true, 'searchable': true, 'sortable': true },
                            { 'title': 'Descrição', 'visible': true, 'searchable': true, 'sortable': true },
                            { 'title': 'Status', 'visible': true, 'searchable': true, 'sortable': true },
    ];

    //====================Permissoes
    $scope.PermissaoNew = false;
    $scope.PermissaoEdit = false;
    $scope.PermissaoDesativar = false;
    $scope.PermissaoExcluir= false;
    httpService.Get("credential/GrupoUsuario@New").then(function (response) {
        $scope.PermissaoNew = response.data;
    });
    httpService.Get("credential/GrupoUsuario@Edit").then(function (response) {
        $scope.PermissaoEdit = response.data;
    });
    httpService.Get("credential/GrupoUsuario@Destroy").then(function (response) {
        $scope.PermissaoExcluir = response.data;
    });
    httpService.Get("credential/GrupoUsuario@Activate").then(function (response) {
        $scope.PermissaoDesativar = response.data;
    });
    //====================Quando terminar carga do grid, torna view do grid visible
    $scope.RepeatFinished = function () {
        $rootScope.routeloading = false;
        $scope.ConfiguraGrid();
        $scope.CurrentShow = 'Grid';
        setTimeout(function () {
            $("#dataTable").dataTable().fnAdjustColumnSizing();
        }, 10)


    };
    //====================Carrega o Grid
    $scope.CarregarGrupoUsuario = function () {
        $rootScope.routeloading = true;
        $scope.GrupoUsuarios = [];
        $scope.CurrentShow = '';
        $('#dataTable').dataTable().fnDestroy();
        httpService.Get('GrupoUsuarioListar').then(function (response) {
            if (response) {
                $scope.GrupoUsuarios = response.data;
                if ($scope.GrupoUsuarios.length == 0) {
                    $scope.RepeatFinished();
                }
            }
        });
    };
    //====================Novo Grupo/ Editar Grupo
    $scope.EditarGrupoUsuario = function (pIdGrupo) {
        httpService.Get('GetGrupoUsuario/'+pIdGrupo).then(function (response) {
            if (response) {
                $scope.Ctrl = response.data;
            }
            $scope.CurrentShow = 'Dados';
        });
    };
    //====================Desativar Grupo
    $scope.DesativarReativar = function (pIdGrupo, pAction) {

        swal({
            title: "Tem certeza que deseja " + (pAction ? "reativar" : "desativar") + " esse Grupo ?",
            type: "warning",
            showCancelButton: true,
            confirmButtonClass: "btn-danger",
            confirmButtonText: "Sim," + (pAction ? "Reativar" : "Desativar"),
            cancelButtonText: "Cancelar",
            closeOnConfirm: true
        }, function () {
            _Data = { 'Id_Grupo': pIdGrupo, 'Status': pAction }
            httpService.Post('DesativarReativarGrupoUsurio', _Data).then(function (response) {
                $scope.CarregarGrupoUsuario();
                $scope.CurrentShow = 'Dados';
            });
        });
    };
    //====================Excluir Grupo
    $scope.ExcluirGrupoUsuario = function (pIdGrupo) {
        swal({
            title: "Tem certeza que deseja Excluir esse Grupo ?",
            type: "warning",
            showCancelButton: true,
            confirmButtonClass: "btn-danger",
            confirmButtonText: "Sim,Excluir" ,
            cancelButtonText: "Cancelar",
            closeOnConfirm: true
        }, function () {
            _Data = { 'Id_Grupo': pIdGrupo }
            httpService.Post('ExcluirGrupoUsuario', _Data).then(function (response) {
                $scope.CarregarGrupoUsuario();
                $scope.CurrentShow = 'Dados';
            });
        });
    };
    //=====================Marcou uma funcao do Perfil
    $scope.CheckPerfil = function (pPerfil) {
        if (!pPerfil.Selected) {
            for (var i = 0; i < $scope.Ctrl.Perfil.length; i++) {
                if ($scope.Ctrl.Perfil[i].Id_Funcao_Root == pPerfil.Id_Funcao) {
                    $scope.Ctrl.Perfil[i].Selected = pPerfil.Selected;
                };
            };
        };
    };
    //=====================Marcar / Desmarcar todos os perfis
    $scope.MarcarPerfil = function () {
        for (var i = 0; i < $scope.Ctrl.Perfil.length; i++) {
            $scope.Ctrl.Perfil[i].Selected = $scope.checkBoxPerfil;
        };
    };
    //===========================Cancela Edicao
    $scope.CancelaEdicao = function () {
        $scope.CurrentShow = 'Grid';
        $scope.CurrentTab = 'Perfil';
        $scope.Ctrl = $scope.NewGrupo();
        $scope.checkBoxEmpresa = false;
        $scope.checkBoxPerfil = false;
    }
    //===========================Salvar o Grupo
    $scope.SalvarGrupoUsuario = function () {
        httpService.Post('SalvarGrupoUsuario',$scope.Ctrl).then(function (response) {
            if (response.data[0].Status==0) {
                ShowAlert(response.data[0].Mensagem, 'error');
            }
            else {
                ShowAlert('Dados Gravados com Sucesso', 'success');
                $scope.CarregarGrupoUsuario();
                $scope.CancelaEdicao();
            }
        });
    }
    //====================Funcao para configurar o Grid
    $scope.ConfiguraGrid = function () {
        param = {};
        param.language = fnDataTableLanguage();
        param.lengthMenu = [[7, 10, 25, 50, -1], [7, 10, 25, 50, "Todos"]];
        param.scrollCollapse = true;
        param.paging = true;
        param.dom = "<'row'<'col-sm-3'l><'col-sm-4'f><'col-sm-5'B>>" + "<'row'<'col-sm-12'tr>>" + "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        param.buttons = [
            {
                text: 'Exportar<span class="fa fa-file-excel-o margin-left-10" style="color:white"></span>', type: 'excel', className: 'btn btn-warning HideButton', extend: 'excel', exportOptions: {
                    columns: ':visible:not(:first-child)'
                }
            }
        ];
        param.order = [[0, 'asc']];
        param.autoWidth = false;

        param.columns = [];
        for (var i = 0; i < $scope.gridheaders.length; i++) {
            param.columns.push({ "visible": $scope.gridheaders[i].visible, "searchable": $scope.gridheaders[i].searchable, "sortable": $scope.gridheaders[i].sortable });
        }
        $('#dataTable').DataTable(param);

    };
    //===========================Marcar/Desmarcar todos os usuarios
    $scope.MarcarUsuarios = function () {
        for (var i = 0; i < $scope.Ctrl.Usuarios.length; i++) {
            $scope.Ctrl.Usuarios[i].Selected = !$scope.Ctrl.Usuarios[i].Selected;
        };
    };
    //===========================fim do load da pagina
    $scope.$watch('$viewContentLoaded', function () {
        $scope.ConfiguraGrid();
        $scope.CarregarGrupoUsuario();
    });
    //===========================Evento chamado ao fim do ngrepeat ao carregar grid 
    $scope.$on('ngRepeatFinished', function (ngRepeatFinishedEvent) {
        $scope.RepeatFinished();
    });
}]);

