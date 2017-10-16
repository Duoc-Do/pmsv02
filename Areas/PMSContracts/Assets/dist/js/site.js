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
$(document).ready(function () {
    $("#CityName").click({
        source: function (request, response) {
            $.ajax({
                url: "http://localhost:60587/PMSContracts/Contract/AutoCompleteCity?Prefix=c",
                type: "GET",
                dataType: "json",
                data: {},
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.Name, value: item.Name };
                    }))
                }
            })
            alert($.ajax);
        },
        messages: {
            noResults: "", results: ""
        }
    });
});
var getctr = $("#idctr").val();
$("#txtSearch").click(function () {
    document.getElementById('setContractID').innerHTML = getctr;
});

$('#contractList').DataTable({
    'paging': false,
    'lengthChange': false,
    'searching': true,
    'ordering': true,
    'info': true,
    'autoWidth': false
})
function goBack() {
    window.history.back()
}