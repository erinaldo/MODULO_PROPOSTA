angular.module('App').controller('loginController', ['$scope', '$rootScope', 'tokenApi', 'httpService', '$location', function ($scope, $rootScope, tokenApi, httpService, $location) {

    $scope.CookieEnabled = navigator.cookieEnabled;
    $scope.app_CheckLogin = { 'Indica_Token': false, 'Mensagem': '' }
    $scope.showLogin = true;
    $scope.showToken = false;
    $scope.Msg_Token = "";
    $scope.app_Logout = function (event) {
        $rootScope.Islogged = false;
        tokenApi.removeAll();
        $rootScope.Mensagens = [];
        $rootScope.UserData = {};
        for (key in localStorage) {
            delete localStorage[key];
        }
        window.location.href = $rootScope.pageUrl;
    };
    $scope.setLogin = function (user) {
        {
            $scope.CookieEnabled = navigator.cookieEnabled;
            $scope.Msg_Token = "";
            if (!$scope.CookieEnabled) return
            var cuser = Salt(user.login);
            var cpassword = Salt(user.password);
            //var cuser = user.login;
            //var cpassword = user.password;
            $rootScope.App_Erro = "";
            var _data = "username=" + cuser + "&password=" + cpassword + "&grant_type=password";
            httpService.GetToken('security/token', _data, user.Token).then(function (response) {
                var _valido = true
                if ($rootScope.App_Erro) {
                    ShowAlert($rootScope.App_Erro, 'warning', 2000, 'topRight');
                    _valido = false;
                    $rootScope.loading = 0;
                }
                if (_valido == true) {
                    tokenApi.setToken('oAuth_token', response.data['access_token']);
                    $rootScope.Islogged = true;
                    delete $scope.app_user;
                    httpService.Get('GetUserData').then(function (response) {
                        $rootScope.UserData = response.data[0];
                    });

                    $rootScope.Mensagens = [];
                    httpService.Post('GetMensagem').then(function (response) {
                        $rootScope.Mensagens = response.data;
                        $rootScope.ShowMensagem = $rootScope.Mensagens.length;
                    });
                    $location.path("/PortalApp")
                }
                else {
                    $rootScope.Islogged = false;
                    tokenApi.removeAll();
                    //delete $scope.app_user
                }
            });
            for (key in localStorage) {
                delete localStorage[key];
            }
        }
    }
    $scope.CheckCookie = function () {
        $scope.CookieEnabled = navigator.cookieEnabled;
    }
    $scope.CheckLogin = function (user) {
        $scope.Msg_Token = "";
        var cuser = Salt(user.login);
        var cpassword = Salt(user.password);
        var _data = {'login':cuser,'password':cpassword}
        httpService.Post("credential/checklogin", _data).then(function (response) {
            if (response.data) {
                $scope.app_CheckLogin = response.data[0];
                if (response.data[0]._valido==0) {
                    ShowAlert(response.data[0].Mensagem);
                    return;
                }
                
                if ($scope.app_CheckLogin.Indica_Token) {
                    $scope.showLogin = false;
                    $scope.showToken = true;
                }
                else {
                    $scope.setLogin(user);
                };
            };
        });
    };

    $scope.GetToken = function (user) {
        $scope.Msg_Token = "";
        var _data = { 'login': user.login}
        httpService.Post("credential/GetToken", _data).then(function (response) {
            if (response.data) {
                $scope.Msg_Token = response.data[0].Msg_Token;
            };
        });
    };
}]);





