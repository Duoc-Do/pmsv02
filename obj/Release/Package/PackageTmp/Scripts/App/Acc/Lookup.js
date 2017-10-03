/// <reference path="../jquery-1.5.1.min.js" />
/// <reference path="../jquery-1.5.1-vsdoc.js" />
//2015-04-09



(function ($) {

    $.fn.lookup3 = function (url) {
        var container = $(this);
        var containerid = $(this).attr('id');
        var fieldname = $(this).attr('fieldname');
        var tablename = $(this).attr('tableautocomplete');

        $(this).typeahead({
            source: function (query, process, paging) {
                // This is going to make an HTTP post request to the controller
                return $.get(url, {
                    fieldname: fieldname,
                    keyword: query,
                    tablename: tablename, 'paging.pagesize': paging.pagesize, 'paging.pagecurrent': paging.pagecurrent
                }, function (data) {
                    var lists = [];
                    map = {};
                    // Loop through and push to the array
                    $.each(data.rows, function (i, row) {
                        map[row.name] = row;
                        lists.push(row.name);
                    });

                    //alert(lists.length);
                    // Process the details
                    process(lists, data.paging);
                });
            }
        });
    };

    $.fn.lookup2 = function (url) {

        //Tham số là slideshow-container
        var container = $(this);
        var containerid = $(this).attr('id');
        var fieldname = $(this).attr('fieldname');
        var tablename = $(this).attr('tableautocomplete');
        var selectontab = true;
        if (typeof $(this).attr('data-selectontab') != 'undefined') {
            selectontab = $(this).attr('data-selectontab');
        }

        $(container).selectize(
            {
            valueField: 'value',
            labelField: 'value',
            searchField: 'label',
            openOnFocus:false,
            selectOnTab: selectontab,
            maxOptions: 10,
            persist: false,
            createOnBlur:false,
            options: [],
            create: true,//cho phep tao
            render: {
                option_create: function (data, escape) {
                    var htmladdnew = '<div class="create_add" onclick=Addnew("' + fieldname.toString() + '")>Thêm mới <strong>"' + escape(data.input) + '"</strong>&hellip;</div>';
                    return htmladdnew;
                },
                option: function (data, escape) {
                    return '<div class="option">' + (data['label'] ? escape(data['label']) : '') + '</div>';
                }
            },
            load: function (query, callback) {
                if (!query.length) return callback();
                this.clearOptions();
                //string fieldname, string keyword
                $.ajax({
                    url:url,
                    dataType: "json",
                    data: {
                        fieldname: fieldname,
                        keyword: query,
                        tablename: tablename,
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
            }
        );

    };

    $.fn.lookup = function (url) {

        //Tham số là slideshow-container
        var $container = $(this);
        var $containerid = $(this).attr('id');
        var $fieldname = $(this).attr('fieldname');
        var $tablename = $(this).attr('tableautocomplete');
        ////alert($containerid);
        //$($containerid).typeahead({
        //    source: function (query, process, paging) {
        //        // This is going to make an HTTP post request to the controller
        //        return $.post(url, { fieldname: $fieldname, keyword: query, tablename: $tablename, 'paging.pagesize': paging.pagesize, 'paging.pagecurrent': paging.pagecurrent }, function (data) {
        //            var lists = [];
        //            map = {};
        //            // Loop through and push to the array
        //            $.each(data.rows, function (i, row) {
        //                map[row.name] = row;
        //                lists.push(row.name);
        //            });
        //            // Process the details
        //            process(lists, data.paging);
        //        });
        //    },
        //    updater: function (item) {
        //        var selectedId = map[item].id;
        //        //Set the text to our selected id
        //        //$("#UserId").val(selectedId);
        //        alert(selectedId);
        //        $(this).trigger("change");
        //        return item;
        //    }
        //});


        //return;

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
                //event.preventDefault();
                //if ($.browser.mozilla==false) {
                $(this).trigger("change");
                //}
            },
            change: function (event, ui) {
                //event.preventDefault();
                //$(this).val(ui.item.value);
                //$(this).trigger("change"); 
                //alert("Cuibap");
            },
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

    $.fn.selectizesetvalue = function (val) {
        if (val=="") {
            $(this)[0].selectize.setValue('');
        }
        else {
            $(this)[0].selectize.addOption({ value: val, label: val });
            $(this)[0].selectize.setValue(val);

        }
    };
    $.fn.selectizegetvalue = function () {

        $(this)[0].selectize.getValue();

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