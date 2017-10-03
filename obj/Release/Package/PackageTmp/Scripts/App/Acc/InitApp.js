//2015-04-07


$(document).ready(function (event) {

    /// <reference path="Lookup.js" />

    //set lookup

    //$('input[isautocomplete = "isautocomplete"]').each(function (i) {
    //    var pathautocomplete = '/Accounting/Services/autocomplete';
    //    //var pathautocomplete = '../autocomplete';
    //    //var pathautocomplete = '@Url.Action("AutoComplete")';
    //    $(this).lookup(pathautocomplete);
    //});

    $('.senviet-grid-btn-addrow').focus(function (event) {
        senvietgridaddrow(this);
    });

    $('select[isautocomplete = "isautocomplete"]:visible').each(function (i) {
        var pathautocomplete = '/Accounting/Services/autocomplete';
        $(this).lookup2(pathautocomplete);
    });


    //$('select[isautocomplete = "isautocomplete"]').not(":hidden").each(function (i) {
    //    var pathautocomplete = '/Accounting/Services/autocomplete';
    //    $(this).lookup2(pathautocomplete);
    //});


    //kết thúc set lookup

    //Set định dạng numeric
    $('.numeric').each(function (i) {
        var decimaldigits = $(this).attr("decimaldigits");
        var vmax = $(this).attr("senviet-vmax");
        var vmin = $(this).attr("senviet-vmin");
        var asep = $(this).attr("senviet-asep");

        if (vmax == null) { vmax: '999999999999999999'; }
        if (vmin == null) { vmin: '-999999999999999999'; }
        if (asep == null) { asep: '.'; }

        $(this).autoNumeric({ mDec: decimaldigits, vMax: vmax, vMin: vmin, aSep: asep });
    });

    //kết thúc set định dạng số

    //    //Set định dạng ngày
    //    $.datepicker.setDefaults($.datepicker.regional["vi"]);
    //    $(".datetime").datepicker($.datepicker.regional["vi"]);
    //    //Kết thúc set định dạng ngày

    //    //Set định dạng ngày giờ
    //    $.datepicker.setDefaults($.datepicker.regional["vi"]);

    //    $.timepicker.regional['vi'] = {
    //        timeOnlyTitle: 'Chọn thời gian',
    //        timeText: 'Thời gian',
    //        hourText: 'Giờ',
    //        minuteText: 'Phút',
    //        secondText: 'Giây',
    //        millisecText: 'Mili giây',
    //        currentText: 'Hiện tại',
    //        closeText: 'Đóng',
    //        ampm: false
    //    };

    //    $(".datetime-g").datetimepicker($.timepicker.regional['vi']);
    //    //Kết thúc set định dạng ngày giờ



    // set định dạng ngày tháng kiểu mới không show popup
    //$.datetimeEntry.setDefaults($.datetimeEntry.regionalOptions['']);
    //$(".datetime").datetimeEntry({ datetimeFormat: 'D/O/Y', spinnerImage: '' });
    //$(".datetime-g").datetimeEntry({ datetimeFormat: 'D/O/Y H:M', spinnerImage: '' });

    // kết thúc set định dạng ngày tháng kiểu mới không show popup

    //phần ngày tháng mới
    $.extend($.inputmask.defaults, {
        'clearIncomplete': true,
        'showMaskOnFocus': false,
        'showMaskOnHover': false
    });

    //$(".datetime").datepicker({
    //    language: "vi",
    //    autoclose: true
    //});
    //$('.datetime').parent().append("<span class='input-group-addon'><span class='lyphicon glyphicon-calendar'></span></span>");


    //$.fn.datepicker.defaults.showOnFocus = false;
    $('.date').datepicker({
        format: "dd/mm/yyyy",
        todayBtn: true,
        language: "vi",
        autoclose: true,
        todayHighlight: true,
        showOnFocus : false
    });




    $(".datetime").inputmask("dd/mm/yyyy");
    $(".datetime-g").inputmask("datetime");



});



var SenVietGlobal = {
    ExObjectChange: function ExObjectChange(a,url) {
        var $thisobject = $(a);
        var val = $(a).val();
        var fieldname = $(a).attr("fieldname");
        //var url = '@Url.Action("FieldChange")';
        //var url = 'FieldChange';
        switch (fieldname) {
            case "ExObject01Code":
                var $tr = $thisobject.closest("tr");
                var ExObject01ID = $tr.find("input[fieldname = 'ExObject01ID']").first();
                $(ExObject01ID).val("");
                if (val != "") {
                    $.ajax({
                        type: "GET", url: url, dataType: "json",
                        data: { type: 1, fieldname: fieldname, keyword: val },
                        success: function (data) {
                            if (data.rows != null)
                            { $(ExObject01ID).val(data.rows.ExObject01ID); }
                            else {
                                $thisobject[0].selectize.setValue('');
                                $thisobject[0].selectize.clearOptions();
                            }
                        }
                    });
                }
                break;
            case "ExObject02Code":
                var $tr = $thisobject.closest("tr");
                var ExObject02ID = $tr.find("input[fieldname = 'ExObject02ID']").first();
                $(ExObject02ID).val("");
                if (val != "") {
                    $.ajax({
                        type: "GET", url: url, dataType: "json",
                        data: { type: 1, fieldname: fieldname, keyword: val },
                        success: function (data) {
                            if (data.rows != null)
                            { $(ExObject02ID).val(data.rows.ExObject02ID); }
                            else {
                                $thisobject[0].selectize.setValue('');
                                $thisobject[0].selectize.clearOptions();
                            }
                        }
                    });
                }
                break;
            case "ExObject03Code":
                var $tr = $thisobject.closest("tr");
                var ExObject03ID = $tr.find("input[fieldname = 'ExObject03ID']").first();
                $(ExObject03ID).val("");
                if (val != "") {
                    $.ajax({
                        type: "GET", url: url, dataType: "json",
                        data: { type: 1, fieldname: fieldname, keyword: val },
                        success: function (data) {
                            if (data.rows != null)
                            { $(ExObject03ID).val(data.rows.ExObject03ID); }
                            else {
                                $thisobject[0].selectize.setValue('');
                                $thisobject[0].selectize.clearOptions();
                            }
                        }
                    });
                }
                break;
            case "ExObject04Code":
                var $tr = $thisobject.closest("tr");
                var ExObject04ID = $tr.find("input[fieldname = 'ExObject04ID']").first();
                $(ExObject04ID).val("");
                if (val != "") {
                    $.ajax({
                        type: "GET", url: url, dataType: "json",
                        data: { type: 1, fieldname: fieldname, keyword: val },
                        success: function (data) {
                            if (data.rows != null)
                            { $(ExObject04ID).val(data.rows.ExObject04ID); }
                            else {
                                $thisobject[0].selectize.setValue('');
                                $thisobject[0].selectize.clearOptions();
                            }
                        }
                    });
                }
                break;
            case "ExObject05Code":
                var $tr = $thisobject.closest("tr");
                var ExObject05ID = $tr.find("input[fieldname = 'ExObject05ID']").first();
                $(ExObject05ID).val("");
                if (val != "") {
                    $.ajax({
                        type: "GET", url: url, dataType: "json",
                        data: { type: 1, fieldname: fieldname, keyword: val },
                        success: function (data) {
                            if (data.rows != null)
                            { $(ExObject05ID).val(data.rows.ExObject05ID); }
                            else {
                                $thisobject[0].selectize.setValue('');
                                $thisobject[0].selectize.clearOptions();
                            }
                        }
                    });
                }
                break;
            case "ExObject06Code":
                var $tr = $thisobject.closest("tr");
                var ExObject06ID = $tr.find("input[fieldname = 'ExObject06ID']").first();
                $(ExObject06ID).val("");
                if (val != "") {
                    $.ajax({
                        type: "GET", url: url, dataType: "json",
                        data: { type: 1, fieldname: fieldname, keyword: val },
                        success: function (data) {
                            if (data.rows != null)
                            { $(ExObject06ID).val(data.rows.ExObject06ID); }
                            else {
                                $thisobject[0].selectize.setValue('');
                                $thisobject[0].selectize.clearOptions();
                            }
                        }
                    });
                }
                break;
            case "ExObject07Code":
                var $tr = $thisobject.closest("tr");
                var ExObject07ID = $tr.find("input[fieldname = 'ExObject07ID']").first();
                $(ExObject07ID).val("");
                if (val != "") {
                    $.ajax({
                        type: "GET", url: url, dataType: "json",
                        data: { type: 1, fieldname: fieldname, keyword: val },
                        success: function (data) {
                            if (data.rows != null)
                            { $(ExObject07ID).val(data.rows.ExObject07ID); }
                            else {
                                $thisobject[0].selectize.setValue('');
                                $thisobject[0].selectize.clearOptions();
                            }
                        }
                    });
                }
                break;
            case "ExObject08Code":
                var $tr = $thisobject.closest("tr");
                var ExObject08ID = $tr.find("input[fieldname = 'ExObject08ID']").first();
                $(ExObject08ID).val("");
                if (val != "") {
                    $.ajax({
                        type: "GET", url: url, dataType: "json",
                        data: { type: 1, fieldname: fieldname, keyword: val },
                        success: function (data) {
                            if (data.rows != null)
                            { $(ExObject08ID).val(data.rows.ExObject08ID); }
                            else {
                                $thisobject[0].selectize.setValue('');
                                $thisobject[0].selectize.clearOptions();
                            }
                        }
                    });
                }
                break;
            case "ExObject09Code":
                var $tr = $thisobject.closest("tr");
                var ExObject09ID = $tr.find("input[fieldname = 'ExObject09ID']").first();
                $(ExObject09ID).val("");
                if (val != "") {
                    $.ajax({
                        type: "GET", url: url, dataType: "json",
                        data: { type: 1, fieldname: fieldname, keyword: val },
                        success: function (data) {
                            if (data.rows != null)
                            { $(ExObject09ID).val(data.rows.ExObject09ID); }
                            else {
                                $thisobject[0].selectize.setValue('');
                                $thisobject[0].selectize.clearOptions();
                            }
                        }
                    });
                }
                break;
            case "ExObject10Code":
                var $tr = $thisobject.closest("tr");
                var ExObject10ID = $tr.find("input[fieldname = 'ExObject10ID']").first();
                $(ExObject10ID).val("");
                if (val != "") {
                    $.ajax({
                        type: "GET", url: url, dataType: "json",
                        data: { type: 1, fieldname: fieldname, keyword: val },
                        success: function (data) {
                            if (data.rows != null)
                            { $(ExObject10ID).val(data.rows.ExObject10ID); }
                            else {
                                $thisobject[0].selectize.setValue('');
                                $thisobject[0].selectize.clearOptions();
                            }
                        }
                    });
                }
                break;
            case "ExObject11Code":
                var $tr = $thisobject.closest("tr");
                var ExObject11ID = $tr.find("input[fieldname = 'ExObject11ID']").first();
                $(ExObject11ID).val("");
                if (val != "") {
                    $.ajax({
                        type: "GET", url: url, dataType: "json",
                        data: { type: 1, fieldname: fieldname, keyword: val },
                        success: function (data) {
                            if (data.rows != null)
                            { $(ExObject11ID).val(data.rows.ExObject11ID); }
                            else {
                                $thisobject[0].selectize.setValue('');
                                $thisobject[0].selectize.clearOptions();
                            }
                        }
                    });
                }
                break;
            case "ExObject12Code":
                var $tr = $thisobject.closest("tr");
                var ExObject12ID = $tr.find("input[fieldname = 'ExObject12ID']").first();
                $(ExObject12ID).val("");
                if (val != "") {
                    $.ajax({
                        type: "GET", url: url, dataType: "json",
                        data: { type: 1, fieldname: fieldname, keyword: val },
                        success: function (data) {
                            if (data.rows != null)
                            { $(ExObject12ID).val(data.rows.ExObject12ID); }
                            else {
                                $thisobject[0].selectize.setValue('');
                                $thisobject[0].selectize.clearOptions();
                            }
                        }
                    });
                }
                break;
            case "ExObject13Code":
                var $tr = $thisobject.closest("tr");
                var ExObject13ID = $tr.find("input[fieldname = 'ExObject13ID']").first();
                $(ExObject13ID).val("");
                if (val != "") {
                    $.ajax({
                        type: "GET", url: url, dataType: "json",
                        data: { type: 1, fieldname: fieldname, keyword: val },
                        success: function (data) {
                            if (data.rows != null)
                            { $(ExObject13ID).val(data.rows.ExObject13ID); }
                            else {
                                $thisobject[0].selectize.setValue('');
                                $thisobject[0].selectize.clearOptions();
                            }
                        }
                    });
                }
                break;
            case "ExObject14Code":
                var $tr = $thisobject.closest("tr");
                var ExObject14ID = $tr.find("input[fieldname = 'ExObject14ID']").first();
                $(ExObject14ID).val("");
                if (val != "") {
                    $.ajax({
                        type: "GET", url: url, dataType: "json",
                        data: { type: 1, fieldname: fieldname, keyword: val },
                        success: function (data) {
                            if (data.rows != null)
                            { $(ExObject14ID).val(data.rows.ExObject14ID); }
                            else {
                                $thisobject[0].selectize.setValue('');
                                $thisobject[0].selectize.clearOptions();
                            }
                        }
                    });
                }
                break;
            case "ExObject15Code":
                var $tr = $thisobject.closest("tr");
                var ExObject15ID = $tr.find("input[fieldname = 'ExObject15ID']").first();
                $(ExObject15ID).val("");
                if (val != "") {
                    $.ajax({
                        type: "GET", url: url, dataType: "json",
                        data: { type: 1, fieldname: fieldname, keyword: val },
                        success: function (data) {
                            if (data.rows != null)
                            { $(ExObject15ID).val(data.rows.ExObject15ID); }
                            else {
                                $thisobject[0].selectize.setValue('');
                                $thisobject[0].selectize.clearOptions();
                            }
                        }
                    });
                }
                break;
            case "ExObject16Code":
                var $tr = $thisobject.closest("tr");
                var ExObject16ID = $tr.find("input[fieldname = 'ExObject16ID']").first();
                $(ExObject16ID).val("");
                if (val != "") {
                    $.ajax({
                        type: "GET", url: url, dataType: "json",
                        data: { type: 1, fieldname: fieldname, keyword: val },
                        success: function (data) {
                            if (data.rows != null)
                            { $(ExObject16ID).val(data.rows.ExObject16ID); }
                            else {
                                $thisobject[0].selectize.setValue('');
                                $thisobject[0].selectize.clearOptions();
                            }
                        }
                    });
                }
                break;
            case "ExObject17Code":
                var $tr = $thisobject.closest("tr");
                var ExObject17ID = $tr.find("input[fieldname = 'ExObject17ID']").first();
                $(ExObject17ID).val("");
                if (val != "") {
                    $.ajax({
                        type: "GET", url: url, dataType: "json",
                        data: { type: 1, fieldname: fieldname, keyword: val },
                        success: function (data) {
                            if (data.rows != null)
                            { $(ExObject17ID).val(data.rows.ExObject17ID); }
                            else {
                                $thisobject[0].selectize.setValue('');
                                $thisobject[0].selectize.clearOptions();
                            }
                        }
                    });
                }
                break;
            case "ExObject18Code":
                var $tr = $thisobject.closest("tr");
                var ExObject18ID = $tr.find("input[fieldname = 'ExObject18ID']").first();
                $(ExObject18ID).val("");
                if (val != "") {
                    $.ajax({
                        type: "GET", url: url, dataType: "json",
                        data: { type: 1, fieldname: fieldname, keyword: val },
                        success: function (data) {
                            if (data.rows != null)
                            { $(ExObject18ID).val(data.rows.ExObject18ID); }
                            else {
                                $thisobject[0].selectize.setValue('');
                                $thisobject[0].selectize.clearOptions();
                            }
                        }
                    });
                }
                break;
            case "ExObject19Code":
                var $tr = $thisobject.closest("tr");
                var ExObject19ID = $tr.find("input[fieldname = 'ExObject19ID']").first();
                $(ExObject19ID).val("");
                if (val != "") {
                    $.ajax({
                        type: "GET", url: url, dataType: "json",
                        data: { type: 1, fieldname: fieldname, keyword: val },
                        success: function (data) {
                            if (data.rows != null)
                            { $(ExObject19ID).val(data.rows.ExObject19ID); }
                            else {
                                $thisobject[0].selectize.setValue('');
                                $thisobject[0].selectize.clearOptions();
                            }
                        }
                    });
                }
                break;
            case "ExObject20Code":
                var $tr = $thisobject.closest("tr");
                var ExObject20ID = $tr.find("input[fieldname = 'ExObject20ID']").first();
                $(ExObject20ID).val("");
                if (val != "") {
                    $.ajax({
                        type: "GET", url: url, dataType: "json",
                        data: { type: 1, fieldname: fieldname, keyword: val },
                        success: function (data) {
                            if (data.rows != null)
                            { $(ExObject20ID).val(data.rows.ExObject20ID); }
                            else {
                                $thisobject[0].selectize.setValue('');
                                $thisobject[0].selectize.clearOptions();
                            }
                        }
                    });
                }
                break;

            default:

        }

    }
};

//Hàm tiện ích

//thêm dấu phân cách hàng nghìn và hàng đơn vị theo vi
function addCommas(nStr) {
    nStr += '';
    x = nStr.split('.');
    x1 = x[0];
    x2 = x.length > 1 ? ',' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + '.' + '$2');
    }
    return x1 + x2;
}

function addCommasEn(nStr) {
    nStr += '';
    x = nStr.split('.');
    x1 = x[0];
    x2 = x.length > 1 ? '.' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + '.' + '$2');
    }
    return x1 + x2;
}

//định dạng ngày tháng của json
function formatJSONDate(jsonDate) {

    // parse JSON formatted date to javascript date object
    var date = new Date(parseInt(jsonDate.substr(6)));
    
    // format display date (e.g. 04/10/2012)
    var displayDate = date.getDate().toString() + "/" + (date.getMonth() + 1).toString() + "/" + date.getFullYear().toString();
        //$.datepicker.formatDate("dd/mm/yy", date);
    return displayDate;

}