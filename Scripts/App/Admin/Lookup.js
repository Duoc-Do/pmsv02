/// <reference path="../jquery-1.5.1.min.js" />
/// <reference path="../jquery-1.5.1-vsdoc.js" />
//2015-03-24

(function ($) {

    $.fn.lookup2 = function (url) {

        //Tham số là slideshow-container
        var $container = $(this);
        var $containerid = $(this).attr('id');
        var $fieldname = $(this).attr('fieldname');
        var $tablename = $(this).attr('tableautocomplete');

        $container.selectize({
            valueField: 'value',
            labelField: 'value',
            searchField: 'label',
            selectOnTab: true,
            maxOptions: 10,
            persist: false,
            createOnBlur: false,
            options: [],
            create: true,//cho phep tao
            //onChange: function (value) { alert(value);},
            render: {
                option_create: function (data, escape) {
                    var htmladdnew = '<div class="create_add" onclick=Addnew("' + $fieldname.toString() + '")>Thêm mới <strong>"' + escape(data.input) + '"</strong>&hellip;</div>';
                    //alert(htmladdnew);
                    return htmladdnew;
                },
                option: function (data, escape) {
                    return '<div class="option">' + (data['label'] ? escape(data['label']) : '') + '</div>';
                }
            },
            //render: {
            //    option: function (item, escape) {
            //        return '<div>' +
            //            '<span class="title">' +
            //                '<span class="name"><i class="icon ' + (item.value ? 'fork' : 'source') + '"></i>' + escape(item.value) + '</span>' +
            //                '<span class="by">' + escape(item.value) + '</span>' +
            //            '</span>' +
            //            '<span class="description">' + escape(item.value) + '</span>' +
            //            '<ul class="meta">' +
            //                (item.value ? '<li class="language">' + escape(item.value) + '</li>' : '') +
            //                '<li class="watchers"><span>' + escape(item.value) + '</span> watchers</li>' +
            //                '<li class="forks"><span>' + escape(item.value) + '</span> forks</li>' +
            //            '</ul>' +
            //        '</div>';
            //    }
            //},
            //score: function (search) {
            //    var score = this.getScoreFunction(search);
            //    return function (item) {
            //        return score(item) * (1 + Math.min(item.watchers / 100, 1));
            //    };
            //},
            //onItemAdd: function (value, $item) { alert(value); return false; },
            load: function (query, callback) {
                if (!query.length) return callback();
                //string fieldname, string keyword
                $.ajax({
                    url: url,
                    dataType: "json",
                    data: {
                        fieldname: $fieldname,
                        keyword: query,
                        tablename: $tablename,
                        'paging.pagesize': 10,
                        'paging.pagecurrent': 1
                    },
                    type: 'GET',
                    error: function () {
                        callback();
                    },
                    success: function (res) {
                        callback(res.rows.slice(0, 10));
                    }
                });
            }
        });
    };

    $.fn.lookup = function (url) {

        //Tham số là slideshow-container
        var $container = $(this);
        var $containerid = $(this).attr('id');
        var $fieldname = $(this).attr('fieldname');
        var $tablename = $(this).attr('tableautocomplete');

        $container.senvietautocomplete({
            source: function (request, response, paging) {
                $.ajax({
                    type: "GET",
                    url: url,
                    dataType: "json",
                    data: {
                        fieldname: $fieldname,
                        keyword: request.term,
                        tablename: $tablename,
                        'paging.pagesize': paging.pagesize,
                        'paging.pagecurrent': paging.pagecurrent
                    },
                    success: function (data) {
                        response(data.rows, data.paging);
                    }
                });
            },
            minLength: 1,
            select: function (event, ui) {
                //if ($.browser.mozilla==false) {
                $(this).trigger("change");
                //}
            },
            //            change: function (event, ui) {
            //                $(this).trigger("change"); 
            //                //alert("Cuibap");
            //            },
            //            focus: function (event, ui) {
            //                alert("Cuibap");
            //            },
            open: function () {
                $(this).removeClass("ui-corner-all").addClass("ui-corner-top");
            },
            close: function () {
                $(this).removeClass("ui-corner-top").addClass("ui-corner-all");
            }

        });

    };

    //lookup luoi

    $.fn.lookupline = function (urlname, name, id) {
        $(this).autocomplete({
            source: function (request, response) {
                $.ajax({
                    type: "GET",
                    url: urlname,
                    dataType: "json",
                    data: {
                        //	                        featureClass: "P",
                        //	                        style: "full",
                        maxRows: 12,
                        keyword: request.term
                    },
                    success: function (data) {
                        response(data.rows);
                    }
                });
            },
            minLength: 1,
            select: function (event, ui) {


                var $name = $(this).parent().parent().find('input[fieldname = ' + name + ']');
                $name.val(ui.item.name.toString());

                var $id = $(this).parent().parent().find('input[fieldname = ' + id + ']');
                $id.val(ui.item.id.toString());
            },
            open: function () {
                $(this).removeClass("ui-corner-all").addClass("ui-corner-top");
            },
            close: function () {
                $(this).removeClass("ui-corner-top").addClass("ui-corner-all");
            },
            change: function (event, ui) {
                if (ui.item != null) {
                    var $name = $(this).parent().parent().find('input[fieldname = ' + name + ']');
                    $name.val(ui.item.name.toString());

                    var $id = $(this).parent().parent().find('input[fieldname = ' + id + ']');
                    $id.val(ui.item.id.toString());
                }
                else {
                    $(this).val(null)
                    var $namefield = $(this).parent().parent().find('input[fieldname = ' + name + ']');
                    $namefield.val(null);

                    var $idfield = $(this).parent().parent().find('input[fieldname = ' + id + ']');
                    $idfield.val(null);
                }
            }
        });

    }; //kết thúc lookup luoi

})(jQuery);