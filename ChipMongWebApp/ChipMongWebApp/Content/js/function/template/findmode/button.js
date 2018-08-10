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
    ajaxHelper(controller + "/paging/", $("#record").serializeObject(), requestMethod.POST).then((data) => $("#tableData").html(data))
};
const cancel = () => location.reload();
const newRecord = () => location.href = controller + "/new";
const find = () => { $("#currentPage").val(1); paging(1) }