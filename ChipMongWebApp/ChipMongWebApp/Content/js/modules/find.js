//-> setupEvents
function setupEvents() {
    paging(1);
    $("#btnFind").click(function () { find(); });
    $("#btnCancel").click(function () { cancel(); });
    $("#btnNew").click(function () { newRecord(); });
}


//-> getPage
function paging(currentPage) {
    $("#currentPage").val(currentPage);
    ajaxHelper(url + "/paging/", $("#record").serializeObject(), requestMethod.POST).then(function (data) {
        $("#tableData").html(data);
    });
}

//-> find
function find() {
    $("#currentPage").val(1);
    paging(1);
}

//-> cancel
function cancel() {
    window.location.reload();
}

//-> newRecord
function newRecordFromFindScreen(endPoint) {
    window.location.href = endPoint + "/new/";
}