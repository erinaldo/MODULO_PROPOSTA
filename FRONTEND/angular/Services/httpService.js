angular.module("App").service("httpService", function ($http, config, $q, $cookies) {

    var _GetConfig = function () {
        return config;
    };

    var _HttpGet = function (pUrl) {
        //-----transformar validar tabela e Listar Tabela em POST        
        if (pUrl.toLowerCase().indexOf('validartabela') >= 0 || pUrl.toLowerCase().indexOf('listartabela') >= 0) {
            var _arrayUrl = pUrl.split('/',2);
            var _xroute = "Post" + _arrayUrl[0];
            var _xtable = _arrayUrl[1]
            var _xvalue  = FindRest(pUrl,"/",2);
            var _json = { 'Table': _xtable, 'Value': _xvalue };
            var deferred = $q.defer();
            $http({
                method: 'POST',
                url: config.baseUrl + "API/" + _xroute ,
                data: _json,
                headers: "{'Content-Type': 'application/x-www-form-urlencoded'}"
            }
            ).then(function (response) {
                deferred.resolve(response);
            });
            return deferred.promise
        }


        var deferred = $q.defer();
        $http({
            method: 'GET',
            url:config.baseUrl + "API/" + pUrl.trim(),
        }
        ).then(function (response) {
            deferred.resolve(response);
        });
        return deferred.promise
    };

    var _httpPost= function (pUrl,pData) {
        var deferred = $q.defer();
        $http({
            method: 'POST',
            url: config.baseUrl + "API/" + pUrl,
            data: pData,
            headers: "{'Content-Type': 'application/x-www-form-urlencoded'}"
        }
        ).then(function (response) {
            deferred.resolve(response);
        });
        return deferred.promise
    };

    var _MobileGet = function (pUrl) {
        var deferred = $q.defer();
        $http({
            method: 'GET',
            url: pUrl
        }
        ).then(function (response) {
            deferred.resolve(response);
        });
        return deferred.promise
    };
    
    var _GetToken = function (pUrl, pData,pToken) {
        var deferred = $q.defer();
        if (!pToken) {
            pToken = "";
        }
        $http({
            method: 'POST',
            url: config.baseUrl + "API/" + pUrl,
            data: pData,
            headers: {'Content-Type': 'application/x-www-form-urlencoded','Cartv-Token':pToken,'CallFrom':'Browser'}
        }
        ).then(function (response) {
            deferred.resolve(response);
        });
        return deferred.promise
    };
    return {
        Get:_HttpGet,
        Post:_httpPost,
        GetConfig: _GetConfig,
        MobileGet: _MobileGet,
        GetToken:_GetToken 
    };

});

function FindRest(pString,pChar,pPos)
{
    var _pos = 0;
    var _ret = ""
    for (var i = 0; i < pString.length; i++) {
        if (pString.substr(i,1)==pChar) {
            _pos ++;
        }
        if (_pos == pPos) {
            _ret = pString.substr(i+1);
            break;
        }
    }
    return _ret;
}

