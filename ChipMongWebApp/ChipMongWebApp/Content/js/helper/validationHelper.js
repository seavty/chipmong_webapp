let isValid = () => {
    let isOk = true;
    $(".required").each(function (i, v) {
        $(this).removeClass("is-invalid file-error error");

        if ($(this).hasClass("select2-hidden-accessible"))
            $("[aria-labelledby='select2-" + $(this).attr("id") + "-container']").closest("span").css("border-color", ""); //for valid select 2
                
        if ($(this).val() == "") {
            if ($(this).attr('type') == "file")
                $(this).addClass("file-error");
            else
                $(this).addClass("is-invalid");
            isOk = false;
        }
        else if ($(this).val() == null) {
            //== for valid select 2
            $("[aria-labelledby='select2-" + $(this).attr("id") + "-container']").closest("span").css("border-color", "red");
            isOk = false; 
        }
    });

    if (!isOk) {
        $.toast({
            heading: 'System',
            text: 'Please key in all required field(s).',
            showHideTransition: 'slide',
            position: 'top-right',
            icon: 'error'
        });
    }
    return isOk;
}


//-- line item ---//
let isValidLineItem = () => {
    let isOk = true;
    $(".line-item-required").each(function (i, v) {
        $(this).removeClass("is-invalid");
        if ($(this).val() == "") {
            $(this).addClass("is-invalid");
            isOk = false;
        }
    });

    if (!isOk) {
        $.toast({
            heading: 'System',
            text: 'Please key in all required field(s).',
            showHideTransition: 'slide',
            position: 'top-right',
            icon: 'error'
        });
    }
    return isOk;
}