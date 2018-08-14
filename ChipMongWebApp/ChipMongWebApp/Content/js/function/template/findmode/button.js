$(document).ready(() => {
    $('form input[type="text"]').keypress((e) => {
        const code = e.keyCode || e.which;
        if (code === 13)
            find();
    });
    paging(1);
});


const paging = currentPage => {
    $("#currentPage").val(currentPage);
    ajaxHelper(controller + "/paging/", $("#record").serializeObject(), requestMethod.POST).then((data) => {
        $("#tableData").html(data);
        $("#sortIcon").remove();
        let sortIcon = "";
        const ascIcon = `<i id="sortIcon" class="fas fa-sort-up"></i>`;
        const descIcon = `<i id="sortIcon" class="fas fa-sort-down"></i>`;

        ($("#orderDirection").val() == "ASC") ? sortIcon = ascIcon : sortIcon = descIcon;
        const sortColumnName = $("#orderBy").val();
        $("#sort_" + sortColumnName).append(sortIcon);
    });
};

const cancel = () => location.reload();
const newRecord = () => location.href = controller + "/new";
const find = () => {
    $("#currentPage").val(1);
    paging(1);
}

const sort = (orderBy, prop) => {
    if ($("#orderBy").val() == orderBy)
        ($("#orderDirection").val() == "ASC") ? $("#orderDirection").val("DESC") : $("#orderDirection").val("ASC");
    else
        $("#orderDirection").val("ASC");
    $("#orderBy").val(orderBy);
    paging(1);
}

