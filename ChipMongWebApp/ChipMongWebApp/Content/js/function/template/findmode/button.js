let paging = currentPage => { $("#currentPage").val(currentPage); ajaxHelper(controller + "/paging/", $("#record").serializeObject(), requestMethod.POST).then((data) => $("#tableData").html(data)) };
const cancel = () => location.reload();
const newRecord = () => location.href = controller + "/new";
const find = () => { $("#currentPage").val(1); paging(1) }