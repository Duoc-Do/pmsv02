/// <reference path="../jquery-1.5.1.min.js" />
/// <reference path="../jquery-1.5.1-vsdoc.js" />


(function ($) {

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