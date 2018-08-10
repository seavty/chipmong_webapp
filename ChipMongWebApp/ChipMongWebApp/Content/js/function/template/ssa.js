const setupSSA = (selector ,ssaURL, ssaPlaceHolder, templateResultCallBack, templateSelectionCallBack) => {
    $(selector).select2({
        ajax: {
            url: ssaURL,
            dataType: "json",
            delay: 50,
            data: (params) => {
                return {
                    q: params.term, // search term
                    page: params.page
                };
            }
        },
        placeholder: ssaPlaceHolder,
        escapeMarkup: (markup) => markup, // let our custom formatter work
        minimumInputLength: 1,
        templateResult: templateResultCallBack,
        templateSelection: templateSelectionCallBack
    });
}


//*** Customer SSA Template ***//
const customerTemplateResult = (data) => {
    if (data.loading)
        return data.text;
    let markup = `<div class="row">
                        <div class="col-4"> ${data.firstName} </div>
                        <div class="col-4"> ${data.lastName}  </div>
                        <div class="col-4"> ${data.code}  </div>
                    </div>`;
    return markup;
}

const customerTemplateSelection = (data) => {
    if (data.text != "") return data.text;
    if (data.id === "") return 'Customer';
    return data.firstName + " " + data.lastName;
}
//*** end Customer SSA Template ***//