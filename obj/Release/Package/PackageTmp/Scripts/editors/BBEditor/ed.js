/*****************************************/
// Name: Javascript Textarea BBCode Markup Editor
// Version: 1.3
// Author: Balakrishnan
// Last Modified Date: 25/jan/2009
// License: Free
// URL: http://www.corpocrat.com
/******************************************/

var textarea;
var content;
var webRoot;

function edToolbar(obj) {
    document.write("<div class=\"toolbar\">");
    document.write("<img title=\"Đậm.\nChú ý: phải tô chọn nội dung trước.\" class=\"button\" src=\"" + webRoot + "editors/BBEditor/images/bold.gif\" name=\"btnBold\" onClick=\"doAddTags('[b]','[/b]','" + obj + "')\">");
    document.write("<img title=\"Nghiêng.\nChú ý: phải tô chọn nội dung trước.\" class=\"button\" src=\"" + webRoot + "editors/BBEditor/images/italic.gif\" name=\"btnItalic\" onClick=\"doAddTags('[i]','[/i]','" + obj + "')\">");
    document.write("<img title=\"Gạch dưới.\nChú ý: phải tô chọn nội dung trước.\" class=\"button\" src=\"" + webRoot + "editors/BBEditor/images/underline.gif\" name=\"btnUnderline\" onClick=\"doAddTags('[u]','[/u]','" + obj + "')\">");
    document.write("<img title=\"Tạo liên kết\" class=\"button\" src=\"" + webRoot + "editors/BBEditor/images/link.gif\" name=\"btnLink\" onClick=\"doURL('" + obj + "')\">");

//    document.write("<img title=\"Chèn hình.\nChú ý: phải tô chọn đường link trước.\" class=\"button\" src=\"" + webRoot + "editors/BBEditor/images/image.gif\" name=\"btnCode\" onClick=\"doAddTags('[img]','[/img]','" + obj + "')\">");
    document.write("<img title=\"Chèn hình.\nChú ý: phải tô chọn đường link trước.\" class=\"button\" src=\"" + webRoot + "editors/BBEditor/images/image.gif\" name=\"btnCode\" onClick=\"doURLImg('" + obj + "')\">");

    document.write("<img title=\"Chèn youtube.\nChú ý: phải tô chọn đường link embed của youtube trước.\" class=\"button\" src=\"" + webRoot + "editors/BBEditor/images/youtube.png\" name=\"btnYoutube\" onClick=\"doYoutube(this)\">");
    //    document.write("<img class=\"button\" src=\"" + webRoot + "editors/BBEditor/images/code.gif\" name=\"btnCode\" onClick=\"doAddTags('[code]','[/code]','" + obj + "')\">");

    document.write("<select title=\"Kích thước chữ.\nChú ý: phải tô chọn nội dung trước.\" style=\"margin-left:2px;\" id=\"size\" onchange=\"changesize(this);\">");
    document.write("<option id=\"size0\" value=\" \" selected=\"selected\">Font size</option>");
    document.write("<option value=\"14\">14</option>");
    document.write("<option value=\"16\">16</option>");
    document.write("<option value=\"18\">18</option>");
    document.write("<option value=\"20\">20</option>");
    document.write("<option value=\"22\">22</option>");
    document.write("<option value=\"24\">24</option>");
    document.write("<option value=\"28\">28</option>");
    document.write("<option value=\"32\">32</option>");
    document.write("<option value=\"36\">36</option></select>");
    document.write("<select title=\"Màu chữ.\nChú ý: phải tô chọn nội dung trước.\" style=\"margin-left:2px;\" id=\"color\" onchange=\"changecolor(this);\">");
    document.write("<option id=\"color0\" value=\" \" selected=\"selected\">Font color</option>");
    document.write("<option value=\"#FF0000\" style=\"background-color: #FF0000;\">Đỏ </option>");
    document.write("<option value=\"#000000\" style=\"background-color: #000000;\">Đen </option>");
    document.write("<option value=\"#00AEEF\" style=\"background-color: #00AEEF;\">Xanh </option>");
    document.write("</select>");


    document.write("<a title=\"Tải hình lên và copy đường link và dán vào nội dung.\" style=\"margin-left:2px;margin-top:-4px;\" class=\"btn2\" target=\"_blank\" href=\"http://www.photobucket.com\">Tải hình lên photobucket.com</a>");

    document.write("<a title=\"Tải hình lên và copy đường link và dán vào nội dung.\" style=\"margin-left:2px;margin-top:-4px;\" class=\"btn2\" target=\"_blank\" href=\"http://imgur.com/\">Tải hình lên imgur.com</a>");


    document.write("</div>");
}

function doURLImg(obj) {
    textarea = document.getElementById(obj);
    var url = prompt('Nhập URL:', 'http://');
    var scrollTop = textarea.scrollTop;
    var scrollLeft = textarea.scrollLeft;
    if (url != '' && url != null) {
        if (document.selection) {
            textarea.focus();
            var sel = document.selection.createRange();
            if (sel.text == "") {
                sel.text = '[img]' + url + '[/img]';
            }
            else {
                sel.text = '[img]' + url + '[/img]';
            }
        }
        else {
            var len = textarea.value.length;
            var start = textarea.selectionStart;
            var end = textarea.selectionEnd;

            var sel = textarea.value.substring(start, end);

            if (sel == "") {
                var rep = '[img]' + url + '[/img]';
            }
            else {
                var rep = '[img]' + url + '[/img]';
            }

            textarea.value = textarea.value.substring(0, start) + rep + textarea.value.substring(end, len);
            textarea.scrollTop = scrollTop;
            textarea.scrollLeft = scrollLeft;
        }
    }
}


function doURL(obj) {
    textarea = document.getElementById(obj);
    var url = prompt('Nhập URL:', 'http://');
    var scrollTop = textarea.scrollTop;
    var scrollLeft = textarea.scrollLeft;
    if (url != '' && url != null) {
        if (document.selection) {
            textarea.focus();
            var sel = document.selection.createRange();
            if (sel.text == "") {
                sel.text = '[url]' + url + '[/url]';
            }
            else {
                sel.text = '[url=' + url + ']' + sel.text + '[/url]';
            }
        }
        else {
            var len = textarea.value.length;
            var start = textarea.selectionStart;
            var end = textarea.selectionEnd;

            var sel = textarea.value.substring(start, end);

            if (sel == "") {
                var rep = '[url]' + url + '[/url]';
            }
            else {
                var rep = '[url=' + url + ']' + sel + '[/url]';
            }

            textarea.value = textarea.value.substring(0, start) + rep + textarea.value.substring(end, len);
            textarea.scrollTop = scrollTop;
            textarea.scrollLeft = scrollLeft;
        }
    }
}

function doAddTags(tag1, tag2, obj) {
    textarea = document.getElementById(obj);
    // Code for IE
    if (document.selection) {
        textarea.focus();
        var sel = document.selection.createRange();
        if (sel.text == "") {
            return;
        }
        sel.text = tag1 + sel.text + tag2;
    }
    else {  // Code for Mozilla Firefox
        var len = textarea.value.length;
        var start = textarea.selectionStart;
        var end = textarea.selectionEnd;
        var scrollTop = textarea.scrollTop;
        var scrollLeft = textarea.scrollLeft;
        var sel = textarea.value.substring(start, end);
        if (sel == "") {
            return;
        }
        var rep = tag1 + sel + tag2;
        textarea.value = textarea.value.substring(0, start) + rep + textarea.value.substring(end, len);
        textarea.scrollTop = scrollTop;
        textarea.scrollLeft = scrollLeft;
    }
}


function changesize(obj) {
    var val = obj.value;
    if (val == "") {
        return;
    }
    var tag1 = "[size=" + val + "]";
    var tag2 = "[/size]"
    doAddTags(tag1, tag2, "Text");

    obj.selectedIndex = 0;

}

function changecolor(obj) {
    var val = obj.value;
    if (val == "") {
        return;
    }
    var tag1 = "[color=" + val + "]";
    var tag2 = "[/color]"

    doAddTags(tag1, tag2, "Text");

    obj.selectedIndex = 0;
}

function doYoutube(a) {

    textarea = document.getElementById("Text");

    var regiframe = new RegExp("\<iframe (.+?)\>\</iframe\>");
    // Code for IE
    if (document.selection) {
        textarea.focus();
        var sel = document.selection.createRange();
        if (sel.text == "") {
            return;
        }
        sel.text = sel.text.replace(regiframe, "[iframe $1][/iframe]");
    }
    else {  // Code for Mozilla Firefox
        var len = textarea.value.length;
        var start = textarea.selectionStart;
        var end = textarea.selectionEnd;
        var scrollTop = textarea.scrollTop;
        var scrollLeft = textarea.scrollLeft;
        var sel = textarea.value.substring(start, end);
        if (sel == "") {
            return;
        }
        var rep = sel.replace(regiframe, "[iframe $1][/iframe]");
        textarea.value = textarea.value.substring(0, start) + rep + textarea.value.substring(end, len);
        textarea.scrollTop = scrollTop;
        textarea.scrollLeft = scrollLeft;
    }
}