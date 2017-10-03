/// <reference path="../jquery-1.5.1.min.js" />


$(document).ready(function () {
    $('.sv-gv-header,.header.headersort').click(function (even) {
        var sortname = $(this).attr("id");
        var sorttype = "asc";
        if (!even.shiftKey) {
        }
        else {
            var sortshiftkey = "<input name='" + sortname + ".shiftkey' type='hidden'  value ='true' />";
            $(this).append(sortshiftkey);
        }

        var sortname = $(this).attr("id");
        var sorttype = "asc";
        if ($(this).attr("orderexpression") != null) {
            if ($(this).attr("orderexpression").toString() == sorttype) {
                sorttype = "desc";
            };

        };
        if (sortname != "") {
            var tagsort = "<input name='" + sortname + "' type='hidden'  value ='" + sorttype + "' />";
            $(this).append(tagsort);
            //$('form').not('form[name|="searchform"]').submit();

            $(this).closest('form').submit();
        }

    });
});

function PagingCick2(trang, a) {
    $('#paging_page').val(trang);
    $(a).closest('form').submit();
};

function PagingCick(loai, a) {
    var pagecur;
    switch (loai) {
        case "dau":
            $('#paging_page').val(1);
            break;
        case "truoc":
            pagecur = parseInt($('#paging_page').val()) - 1;
            $('#paging_page').val(pagecur);
            break;
        case "sau":
            pagecur = parseInt($('#paging_page').val()) + 1;
            $('#paging_page').val(pagecur);
            break;
        case "cuoi":
            pagecur = parseInt($('#paging_pagecount').val());
            $('#paging_page').val(pagecur);
            break;
        default:

    }
    //$('form').not('form[name|="searchform"]').submit();
    $(a).closest('form').submit();

};

function SortClick(colSort, a) {
    var curcolSort = $('#paging_currentsort').val();
    if (curcolSort == colSort) {
        colSort += " DESC";
    }
    $('#paging_sortorder').val(colSort)
    //$('form').not('form[name|="searchform"]').submit();
    $(a).closest('form').submit();
};

function PageSizeChange(a) {
    //$('form').not('form[name|="searchform"]').submit();
    $(a).closest('form').submit();
};


///// <reference path="../jquery-1.5.1.min.js" />


//$(document).ready(function () {
//    $('.sv-gv-header').click(function (even) {
//        var sortname = $(this).attr("id");
//        var sorttype = "asc";
//        if (!even.shiftKey) {
//        }
//        else {
//            var sortshiftkey = "<input name='" + sortname + ".shiftkey' type='hidden'  value ='true' />";
//            $(this).append(sortshiftkey);
//        }

//        var sortname = $(this).attr("id");
//        var sorttype = "asc";
//        if ($(this).attr("orderexpression") != null) {
//            if ($(this).attr("orderexpression").toString() == sorttype) {
//                sorttype = "desc";
//            };

//        };
//        if (sortname != "") {
//            var tagsort = "<input name='" + sortname + "' type='hidden'  value ='" + sorttype + "' />";
//            $(this).append(tagsort);
//            $('form').not('form[name|="searchform"]').submit();
//        }

//    });
//});



//function PagingCick(loai) {
//    var pagecur;
//    switch (loai) {
//        case "dau":
//            $('#paging_page').val(1);
//            break;
//        case "truoc":
//            pagecur = parseInt($('#paging_page').val()) - 1;
//            $('#paging_page').val(pagecur);
//            break;
//        case "sau":
//            pagecur = parseInt($('#paging_page').val()) + 1;
//            $('#paging_page').val(pagecur);
//            break;
//        case "cuoi":
//            pagecur = parseInt($('#paging_pagecount').val());
//            $('#paging_page').val(pagecur);
//            break;
//        default:

//    }
//    $('form').not('form[name|="searchform"]').submit();
//};

//function SortClick(colSort) {
//    var curcolSort = $('#paging_currentsort').val();
//    if (curcolSort == colSort) {
//        colSort += " DESC";
//    }
//    $('#paging_sortorder').val(colSort)
//    $('form').not('form[name|="searchform"]').submit();
//};

//function PageSizeChange() {
//    $('form').not('form[name|="searchform"]').submit();
//};
