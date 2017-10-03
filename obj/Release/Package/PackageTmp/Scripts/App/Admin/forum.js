var urlsearch = $('#linkpageid').val();

$(document).ready(function () {
    document.getElementById("keywordsid").addEventListener("keydown", function (e) {
        if (!e) { var e = window.event; }

        // Enter is pressed
        if (e.keyCode == 13) {
            e.preventDefault(); // sometimes useful
            submitpage();
        }
    }, false);
})
function submitpage() {
    if ($('#keywordsid').val() == '') {
        return;
    }
    urlsearch = urlsearch + $('#keywordsid').val(); //  + '&googletab=true'
    document.location = urlsearch;
}


