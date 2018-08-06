const save = () => { if (isValid()) ajaxHelper(controller + "/create", $("#record").serializeObject(), requestMethod.POST).then((data) => location.href = controller + "/view/" + data.id) }
const edit = () => location.href = controller + "/edit/" + id;
const back = () => location.href = controller + "/find/";
const deleteRecord = () => { if (confirm("Do you want to delete this record?")) ajaxHelper(controller + "/delete/" + id, null, requestMethod.POST).then((data) => location.href = controller + "/find/") }

const removePlaceHolder = () => $(":input").removeAttr("placeholder");


