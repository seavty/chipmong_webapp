const save = () => {
    if (isValid()) {
        $('#tblLineItem tr:last').remove();
        ajaxHelper(controller + "/new", $("#record").serializeObject(), requestMethod.POST).then((data) => {
            location.href = controller + "/view/" + data.id
        });
    }
};
const cancel = () => location.href = controller + "/find";