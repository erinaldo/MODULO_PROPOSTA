﻿angular.module('App').config(function ($routeProvider) {
    $routeProvider
        .when('/portal', {
            templateUrl: 'view/portal.html',
            authorize: false,
            routeName: 'Portal',
            RouteId: 0
        })
        .when('/cadastro', {
            templateUrl: 'view/UnderConstrution.html',
            authorize: false,
            routeName: 'Cadastros',
            RouteId: 0
        })
        .when('/blank', {
            templateUrl: 'view/blank.html',
            authorize: false,
            routeName: 'SIM - Módulo Propostas',
            RouteId: 0
        })
        .when('/login', {
            templateUrl: 'view/login.html',
            authorize: false,
            routeName: 'Autenticação',
            RouteId: 0
        })
        .when('/newpassword', {
            templateUrl: 'view/newpassword.html',
            controller: 'newPasswordController',
            authorize: false,
            routeName: 'Solicitação de Alteração de Senha',
            RouteId: 0
        })
        .when('/error', {
            templateUrl: 'view/error.html',
            authorize: false,
            routeName: 'Mensagens de Erro',
            RouteId: 0
        })
        .when('/unlogged', {
            templateUrl: 'view/unlogged.html',
            authorize: false,
            routeName: 'Login',
            RouteId: 0
        })
        .when('/unauthorized', {
            templateUrl: 'view/unauthorized.html',
            authorize: false,
            routeName: 'Acesso não autorizado',
            RouteId: 0
        })
        .when('/dashboard', {
            //templateUrl: 'view/UnderConstrution.html',
            templateUrl: 'view/dashboard.html',
            authorize: true,
            routeName: 'Dashboard',
            RouteId: 0
        })
        .when('/usuario', {
            templateUrl: 'view/Usuario.html',
            authorize: true,
            controller: 'UsuarioController',
            routeName: 'Cadastro de Usuários',
            RouteId: 'Usuario@Index'
        })

        .when('/veiculo', {
            templateUrl: 'view/Veiculo.html',
            authorize: true,
            controller: 'VeiculoController',
            routeName: 'Cadastro de Veículos',
            RouteId: 'Veiculo@Index'
        })

        .when('/mercado', {
            templateUrl: 'view/Mercado.html',
            authorize: true,
            controller: 'MercadoController',
            routeName: 'Cadastro de Mercado',
            RouteId: 'Mercado@Index'
        })
        .when('/VeiculoCadastro/:Action/:Id', {
            templateUrl: 'view/VeiculoCadastro.html',
            authorize: true,
            controller: 'VeiculoCadastroController',
            routeName: 'Inclusao de Veículos',
            RouteId: 'Veiculo@New'
        })

        .when('/MercadoCadastroEdit/:Action/:Id', {
            templateUrl: 'view/MercadoCadastro.html',
            authorize: true,
            controller: 'MercadoCadastroController',
            routeName: 'Edição de Mercados',
            RouteId: 'Mercado@Edit'
        })

        .when('/MercadoCadastroNew/:Action/:Id', {
            templateUrl: 'view/MercadoCadastro.html',
            authorize: true,
            controller: 'MercadoCadastroController',
            routeName: 'Inclusao de Mercados',
            RouteId: 'Mercado@New'
        })


        .when('/TipoComercial', {
            templateUrl: 'view/TipoComercial.html',
            authorize: true,
            controller: 'TipoComercialController',
            routeName: 'Cadastro de Tipo Comercial',
            RouteId: 'TipoComercial@Index'
        })

        .when('/TipoComercialCadastro/:Action/:Id', {
            templateUrl: 'view/TipoComercialCadastro.html',
            authorize: true,
            controller: 'TipoComercialCadastroController',
            routeName: 'Inclusao de Tipo Comercial',
            RouteId: 'TipoComercial@New'
        })
        
        .when('/Simulacao', {
            templateUrl: 'view/simulacao_List.html',
            controller: 'Simulacao_List_Controller',
            authorize: true,
            routeName: 'Modelo de Vendas',
            RouteId: 'Simulacao@Index'
        })
        .when('/SimulacaoCadastro/:Action/:Id/:Processo', {
            templateUrl: 'view/simulacao.html',
            controller: 'SimulacaoController',
            authorize: true,
            routeName: 'Modelo de Vendas',
            RouteId: 'Simulacao@Edit'
        })

        .when('/Proposta', {
            templateUrl: 'view/simulacao_List.html',
            controller: 'Simulacao_List_Controller',
            authorize: true,
            routeName: 'Proposta',
            RouteId: 'Proposta@Index'
        })
    .when('/pacote', {
        templateUrl: 'view/PacoteDesconto_List.html',
        controller: 'PacoteDesconto_List_Controller',
        authorize: true,
        routeName: 'Pacote de Descontos',
        RouteId: 'Pacote@Index'
    })
    .when('/PacoteNew/:Action/:Id', {
        templateUrl: 'view/PacoteCadastro.html',
        controller: 'PacoteCadastroController',
        authorize: true,
        routeName: 'Novo Pacote de Descontos',
        RouteId: 'Pacote@New'
    })
    .when('/PacoteEdit/:Action/:Id', {
        templateUrl: 'view/PacoteCadastro.html',
        controller: 'PacoteCadastroController',
        authorize: true,
        routeName: 'Edição de Pacotes',
        RouteId: 'Pacote@New'
    })
        .when('/PacoteShow/:Action/:Id', {
            templateUrl: 'view/PacoteCadastro.html',
            controller: 'PacoteCadastroController',
            routeName: 'Visualização do Pacote ',
        })
    .when('/regraaprovacao', {
        templateUrl: 'view/RegraAprovacao.html',
        controller: 'RegraAprovacao_Controller',
        authorize: true,
        routeName: 'Regras de Aprovação de Descontos',
        RouteId: 'Aprovacao@Index'
    })
        .when('/PendenciaAprovacao', {
            templateUrl: 'view/PendenciaAprovacao.html',
            controller: 'PendenciaAprovacaoController',
            authorize: true,
            routeName: 'Pendencias de Aprovação',
            RouteId: 'Proposta@Approve'
        })
        .when('/PropostaAprovacao/:Id/:From', {
            templateUrl: 'view/PropostaPendenteAprovacao.html',
            controller: 'AprovarPendentes_Controller',
            authorize: true,
            routeName: 'Aprovação da Proposta',
            RouteId: 'Proposta@Approve'
        })
        .when('/regracadastro/:Action/:Id', {
            templateUrl: 'view/RegraAprovacao_Cadastro.html',
            controller: 'RegraAprovacaoCadastro_Controller',
            authorize: true,
            routeName: 'Regra de Aprovação de Desconto',
            RouteId: 'Pacote@New'
        })
    .otherwise({ redirectTo: "/blank" })

});

angular.module('App')
    .run(['$rootScope', '$location', 'httpService', 'tokenApi', function ($rootScope, $location, httpService, tokenApi) {
        $rootScope.$on('$routeChangeStart', function (route, next) {
            $rootScope.routeloading = true;
            if (next.authorize == true) {
                httpService.Get('Credential/' + next.RouteId)
                    .then(function (response) {
                        if (response) {
                            if (response.data == false) {
                                $location.path("/unauthorized")
                            }
                        }
                    })
                .catch(function (ex) {
                    console.log(ex);
                });
            }
        });

        $rootScope.$on('$routeChangeSuccess', function (route, current) {
            $rootScope.routeloading = false;
            $rootScope.routeName = current.$$route.routeName;
            $rootScope.routeId = current.$$route.RouteId;
        });
    }]);

angular.module('App').config(['$locationProvider', function ($locationProvider) {
    $locationProvider.hashPrefix("");
}]);


