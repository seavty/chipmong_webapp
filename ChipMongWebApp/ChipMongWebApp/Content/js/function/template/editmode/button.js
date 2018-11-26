$(document).ready(() => {
    $('form input[type="text"]').keypress((e) => {
        const code = e.keyCode || e.which;
        if (code === 13)
            save();
    });
});

const save = () => {
    if (isValid()) {
        $('#tblLineItem tr:last').remove();
        ajaxHelper(controller + "/edit", $("#record").serializeObject(), requestMethod.POST).then((data) => {
            if (data.id != null)
                location.href = controller + "/view/" + data.id
            else
                alert(data);
        })
    }
}
const cancel = () => location.href = controller + "/view/" + id;