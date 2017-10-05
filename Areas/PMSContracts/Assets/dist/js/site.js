$('#ContractDetailTab a').click(function (e) {
    e.preventDefault();

    var url = $(this).attr("data-url");
    var href = this.hash;
    var pane = $(this);

    // ajax load from data-url
    $(href).load(url, function (result) {
        pane.tab('show');
    });
});
function loadDoc() {
    var xhttp = new XMLHttpRequest();
    var gaj = "General/Index";
    alert(gaj);
    xhttp.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            document.getElementById("listdata").innerHTML = this.responseText;
            alert("dddddddd")
        }
    };
    xhttp.open("POST",gaj, false);
    xhttp.send();
}
//// load first tab content
//$('#ContractTab').load($('.active a').attr("data-url"), function (result) {
//    $('.active a').tab('show');
//});
