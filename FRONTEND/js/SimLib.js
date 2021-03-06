"use strict";
function redirect(url) {
    window.location = url;
}
//function fnDatepickerIcon() {
//    var a = {
//        time: "icon-clock",
//        date: "icon-calendar-full",
//        up: "icon-chevron-up",
//        down: "icon-chevron-down",
//        previous: 'icon-chevron-left',
//        next: 'icon-chevron-right',
//        today: 'icon-calendar-insert',
//        clear: 'icon-trash',
//        close: 'icon-cross'
//    }
//    return a;
//}
function fnDataTableLanguage() {
    return {
        "sEmptyTable": "Nenhum registro encontrado",
        "sInfo": "Mostrando de _START_ até _END_ de _TOTAL_ registros",
        "sInfoEmpty": "Mostrando 0 até 0 de 0 registros",
        "sInfoFiltered": "(Filtrados de _MAX_ registros)",
        "sInfoPostFix": "",
        "sInfoThousands": ".",
        "sLengthMenu": "_MENU_ resultados por página",
        "sLoadingRecords": "Carregando...",
        "sProcessing": "Processando...",
        "sZeroRecords": "Nenhum registro encontrado",
        "sSearch": "Pesquisar",
        "oPaginate": {
            "sNext": "Próximo",
            "sPrevious": "Anterior",
            "sFirst": "Primeiro",
            "sLast": "Último"
        },
        "oAria": {
            "sSortAscending": ": Ordenar colunas de forma ascendente",
            "sSortDescending": ": Ordenar colunas de forma descendente"
        }
    };
};

function ShowAlert(pMensagem, pType, pTimeout, pLayoyt) {
    if (!pType) {
        pType="warning"
    }
    if (pType=='warning' || pType=='error') {
        pType   = ""
    }
    //setTimeout(function () {
    //    swal('', pMensagem, pType);
    //}, 100)
    setTimeout(function () {
        swal({
            title: '',
            text:pMensagem,
        }, function () {
            var close = window.swal.close;
            window.swal.close = function () {
                close();
                window.onkeydown = null;
            };
        });
    }, 100)
}



function CompetenciaToInt(pCompetencia) {
    if (pCompetencia) {
        return parseInt(pCompetencia.substr(3, 4) + pCompetencia.substr(0, 2));
    }
    else {
        return null;
    }
}

//function MesExtenso(pParam, pFormat) {
//    var _dt = new Date(pParam);
//    var _mes = _dt.getMonth();
//    var _ano = _dt.getFullYear();
//    var _aMes = ['Janeiro', 'Fevereiro', 'Marco', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'];
//    var _ext = _aMes[_mes];
//    switch (pFormat) {
//        case 'short-yyyy':
//            var _ret = _ext.substring(0, 3); + '/' + _ano;
//            break;
//        case 'long-yyyy':
//            var _ret = _ext + '/' + _ano;
//            break;
//        case 'short':
//            var _ret = _ext.substring(0, 3);
//            break;
//        case 'long':
//            var _ret = _ext;
//            break;
//    }
//    return _ret;
//}

function CurrentMMYYYY(pIncrement) {

    var x = new Date();
    if (pIncrement) {
        x = addMonths(x, pIncrement);
    }
    var _mes = '0000' + (x.getMonth() + 1)
    var _ano = x.getFullYear();
    return _mes.substring(_mes.length - 2, _mes.length) + '/' + _ano
}
function addMonths(pDate, value) {
    //var _date = pDate;
    var _date = new Date(pDate.valueOf())
    var _dia = pDate.getDate();
    var _mes = pDate.getMonth();
    var _ano = pDate.getFullYear();
    var _new_mes = _mes + value;
    var _last_day = new Date(_ano, _new_mes + 1, 0);
    var _result = pDate;
    _result.setMonth(_result.getMonth() + value);
    if (_result > _last_day) {
        _result = _last_day
    }
    return _result;
};
function Left(str, n) {
    if (n <= 0)
        return "";
    else if (n > String(str).length)
        return str;
    else
        return String(str).substring(0, n);
}
function Right(str, n) {
    if (n <= 0)
        return "";
    else if (n > String(str).length)
        return str;
    else {
        var iLen = String(str).length;
        return String(str).substring(iLen, iLen - n);
    }
}
function LeftZero(pNumber, pLen) {
    return Right("0".repeat(pLen) + pNumber.toString(), pLen);
}

function DateToString(pDate) {
    var _dia = pDate.getDate();
    var _mes = pDate.getMonth() + 1;
    var _ano = pDate.getFullYear();
    return LeftZero(_dia, 2) + '/' + LeftZero(_mes, 2) + '/' + _ano.toString()
}
function DateToMonthYear(pDate) {
    var _dia = pDate.getDate();
    var _mes = pDate.getMonth() + 1;
    var _ano = pDate.getFullYear();
    return LeftZero(_mes, 2) + '/' + _ano.toString()
}
function StringToDate(pData, pFormat) {
    if (!pFormat) {
        pFormat = "dd/mm/yyyy";
    }
    if (pData) {
        var _formato = pFormat.replace("/", "-");
        var _formato = _formato.replace("/", "-");
        var _formato = _formato.replace("/", "-");
        switch (_formato) {
            case 'yyyy-mm-dd':
                var dia = pData.substring(8, 10);
                var mes = pData.substring(5, 7);
                var ano = pData.substring(0, 4);
                break;
            case 'dd-mm-yyyy':
                var dia = pData.substring(0, 2)
                var mes = pData.substring(3, 5)
                var ano = pData.substring(6, 10)
                break;
            case 'mm-dd-yyyy':
                var dia = pData.substring(3, 5)
                var mes = pData.substring(0, 2)
                var ano = pData.substring(6, 10)
                break;
        }
        return new Date(ano, mes - 1, dia, 0, 0,0);
    } else {
        return undefined;
    }
}

function NumericOnly(e) {
    return (e.charCode >= 48 && e.charCode <= 57) || (e.keyCode == 8) || (e.keyCode == 9)
}
function DecimalOnly(e) {
    return (e.charCode >= 48 && e.charCode <= 57) || (e.keyCode == 8) || (e.keyCode == 9) || (e.charCode == 44)
}
//Procura um valor em um objeto array e retorno o index ou -1 quando nao encontrar
function SearchInObject(pArray, pKey, pValue) {

    var _ret = -1;
    for (var i = 0; i < pArray.length; i++) {
        for (var key in pArray[i]) {
            if (key == pKey) {
                if (pArray[i][key] == pValue) {
                    _ret = i;
                }
            }
        }
    }
    return _ret;
}

function tabOnly(e) {
    return (e.keyCode == 9)
}

function DeleteFromObeject(pArray, pKey, pValue) {
    var _ret = -1;
    for (var i = 0; i < pArray.length; i++) {
        for (key in pArray[i]) {
            if (key == pKey) {
                if (pArray[i][key] == pValue) {
                    pArray.splice(i, 1);
                }
            }
        }
    }
}

function Salt(string) {
    var _byte1 = 0;
    var _byte2 = 0;
    var _byte3 = 0;
    var _byte4 = "";
    var _byte5 = "";
    for (var i = 0; i < string.length; i++) {
        _byte1 = parseInt(string.substr(i, 1).charCodeAt(0).toString())
        _byte2 = Math.floor((Math.random() * 700) + 1);
        _byte3 = _byte1 + _byte2;
        _byte4 += ('0000' + _byte3.toString()).slice(-3) + ('0000' + _byte2.toString()).slice(-3)
    }
    for (var x = 0; x < _byte4.length; x += 2) {
        _byte5 += _byte4.substr(x + 1, 1) + _byte4.substr(x, 1);
    }
    return _byte5;
}
function ValidaData(element) //Valida e formata uma pData7
{
    var pData = element.value
    if (!pData) {
        return;
    }
    pData = pData.replace(/[^0-9]+/g, "");
    //=========================================Consiste o tamanho 
    if (pData.length != 6 && pData.length != 8) {
        ShowAlert('Data Inválida', 'error', 2000, 'topRight');
        element.value = '';
        return;
    };
    var _day = parseInt(pData.substr(0, 2));
    var _month = parseInt(pData.substr(2, 2));
    _month -= 1
    if (pData.length == 6) {
        var _year = pData.substr(4, 2);
        _year = parseInt('20' + _year);
    }
    else {
        var _year = parseInt(pData.substr(4, 4));
    }
    var newDate = new Date(_year, _month, _day, 0, 0, 0, 0);
    if (newDate.getDate() != _day || newDate.getMonth() != _month || newDate.getFullYear() != _year) {
        ShowAlert('Data Inválida', 'error', 2000, 'topRight');
        element.value = '';
        return;
    }
    _month += 1;
    element.value = (100 + _day).toString().substr(1, 2) + "/" +
                    (100 + _month).toString().substr(1, 2) + "/" +
                    _year.toString();
}
function ValidaCompetencia(element) //Valida e formata uma Competencia
{
    var pData = element.value
    if (!pData) {
        return;
    }
    pData = pData.replace(/[^0-9]+/g, "");
    //=========================================Consiste o tamanho 
    if (pData.length != 4 && pData.length != 6) {
        ShowAlert('Competência Inválida', 'error', 2000, 'topRight');
        element.value = '';
        return;
    };
    var _month = parseInt(pData.substr(0, 2));
    if (pData.length == 4) {
        var _year = pData.substr(2, 2);
        _year = parseInt('20' + _year);

    }
    else {
        var _year = parseInt(pData.substr(2, 4));
    }
    if (_month < 1 || _month > 12) {
        ShowAlert('Competência Inválida', 'error', 2000, 'topRight');
        element.value = '';
        return;
    }
    element.value = (100 + _month).toString().substr(1, 2) + "/" +
                    _year.toString();
}


function MoneyFormat(pValue, return_zero) {

    var formatter = new Intl.NumberFormat('pt-BR', {
        style: 'currency',
        currency: 'BRL',
        minimumFractionDigits: 2,
    });
    if (pValue) {
        return formatter.format(pValue.toString().replace(',', '.'));
    }
    else {
        if (return_zero) {
            return formatter.format(pValue.toString().replace(',', '.'));
        }
        else {
            return "";
        }

    }

}
function DecimalUnformat(pValue) {
    if (pValue) {
        return pValue.toString().replace(/[^0-9,]+/g, "");
    }
    else {
        return "";
    }
}
function cnpjUnformat(pValue) {
    if (pValue) {
        return pValue.toString().replace(/[^0-9,]+/g, "");
    }
    else {
        return "";
    }
}
function cnpjFormat(pValue) {

    if (pValue) {
        if (pValue.length == 14) {
            return pValue.replace(/(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})/g, "\$1.\$2.\$3\/\$4\-\$5");
        }
        else {
            return pValue.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/g, "\$1.\$2.\$3\-\$4");
        }

    }
    else {
        return "";
    }
}

function PercentFormat(pValue) {
    var formatter = new Intl.NumberFormat('pt-BR', {
        style: 'decimal',
        minimumFractionDigits: 4,
    });
    if (pValue) {
        return formatter.format(pValue.toString().replace(',', '.'));
    }
    else {
        return "";
    }
}
function DecimalFormat(pValue) {
    var formatter = new Intl.NumberFormat('pt-BR', {
        style: 'decimal',
        minimumFractionDigits: 2,
    });
    if (pValue) {
        return formatter.format(pValue.toString().replace(',', '.'));
    }
    else {
        return "";
    }
}
function TimeUnformat(elm) {

    if (elm.value) {
        return elm.value.replace(':', '');
    }
    else {
        return "";
    }
}
//function TimeFormat(pValue) {

//    if (pValue) {
//        pValue = pValue.trim();
//        var x = LeftZero(pValue, 4)
//        return x.substr(0, 2) + ':' + x.substr(2, 2);
//    }
//    else {
//        return "";
//    }
//}

function TimeFormat(elm) {
    var x = elm.value;
    if (x) {
        x= x.trim();
        x = LeftZero(x, 4)
        x= x.substr(0, 2) + ':' + x.substr(2, 2);
    }
    else {
        x = "";
    }
    var ctrl = angular.element(elm).data('$ngModelController');
    ctrl.$setViewValue(x);
    ctrl.$commitViewValue();
    return x;
    
}

function DoubleVal(pValue) {
    var _ret = 0;
    if (pValue) {
        pValue = pValue.toString().replace(/\./g, '')
        pValue = pValue.toString().replace(/\,/g, '.')
        pValue = pValue.toString().replace(/[^0-9.,-]+/g, "");
        _ret = parseFloat(pValue);
    }
    return _ret;
}

function NullToString(pValue) {
    var _ret = "";
    if (pValue) {
        _ret = pValue.trim();
    }
    return _ret;
}

function ValidaEmail(pfield, pRequired) {
    var _ret = true
    if (!pfield) {
        if (pRequired) {
            _ret=false
        }
    }
    else {
        var _usuario = pfield.substring(0, pfield.indexOf("@"));
        var _dominio = pfield.substring(pfield.indexOf("@") + 1, pfield.length);
        if ((_usuario.length >= 1) &&
            (_dominio.length >= 3) &&
            (_usuario.search("@") == -1) &&
            (_dominio.search("@") == -1) &&
            (_usuario.search(" ") == -1) &&
            (_dominio.search(" ") == -1) &&
            (_dominio.search(".") != -1) &&
            (_dominio.indexOf(".") >= 1) &&
            (_dominio.lastIndexOf(".") < _dominio.length - 1)) {
            _ret = true
        }
        else {
            _ret = false
        }
    }
    return _ret;
}

function LastDay(Year,Month) {
    var _ret = new Date(Year, Month, 1, 0, 0, 0, 0)
    _ret.setDate(_ret.getDate()-1)
    return _ret;
}

function MesExtenso(MesAno)
{
    if (MesAno>0) {
        return ['', 'Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'][parseInt(MesAno.toString().substr(4, 2))] + '/' + MesAno.toString().substr(0, 4);
    }
    else {
        return "";
    }
    
}

function FormatChart(pGraph,ptype)
{
    if (ptype.toUpperCase() == "MONEY") {
        pGraph.options.scales.yAxes[0].ticks = {
            callback: function (value, index, values) {
                return value.toLocaleString('pt-br', { style: 'currency', currency: 'BRL' });
            }
        };
        pGraph.options.tooltips = {
            callbacks: {
                label: function (tooltipItem, data) {
                    let label = data.labels[tooltipItem.index];
                    let value = data.datasets[tooltipItem.datasetIndex].data[tooltipItem.index];
                    return ' ' + label + ': ' + value.toLocaleString('pt-br', { style: 'currency', currency: 'BRL' });
                }
            }
        }
    }
    if (ptype.toUpperCase() == "NUMBER") {
        //pGraph.options.scales.yAxes[0].ticks = {
        //    callback: function (value, index, values) {
        //        return value.toFixed(0);
        //    }
        //};
        pGraph.options.tooltips = {
            callbacks: {
                label: function (tooltipItem, data) {
                    let label = data.labels[tooltipItem.index];
                    let value = data.datasets[tooltipItem.datasetIndex].data[tooltipItem.index];
                    return ' ' + label + ': ' + value;
                }
            }
        }
    };
    if (ptype.toUpperCase() == "PERCENT") {
        pGraph.options.scales.yAxes[0].ticks = {
            callback: function (value, index, values) {
                return value.toString() + " %";
            }
        };
        pGraph.options.tooltips = {
            callbacks: {
                label: function (tooltipItem, data) {
                    let label = data.labels[tooltipItem.index];
                    let value = data.datasets[tooltipItem.datasetIndex].data[tooltipItem.index];
                    return ' ' + label + ': ' + value.toString() + "%";
                }
            }
        }
    }
    
}

function NewPesquisaTabela()
{
    return {
        "Items": [],
        'FiltroTexto': '',
        'ClickCallBack': '',
        'Titulo': '',
        'MultiSelect': false,
        'PreFiltroTexto': "",
        'PreFilter' : false,
        'ButtonText': '',
        'LoadCallBack': '',
        'ButtonCallBack': '',
        'MarcarTodos':false
    };
}
function MergeCommonRows(table, cols) {
    return;
    //var firstColumnBrakes = [];
    //for (var i = 0; i < cols.length; i++) {
    //    var previous = null, cellToExtend = null, rowspan = 1;
    //    table.find("td:nth-child(" + cols[i] + ")").each(function (index, e) {
    //        var jthis = $(this), content = jthis.text();
    //        if (previous == content && content !== "" && $.inArray(index, firstColumnBrakes) === -1) {
    //            jthis.addClass('hidden');
    //            cellToExtend.attr("rowspan", (rowspan = rowspan + 1));
    //        } else {
    //            if (cols[i] === 1) firstColumnBrakes.push(index);
    //            rowspan = 1;
    //            previous = content;
    //            cellToExtend = jthis;
    //        }
    //    });
    //}
}










