function customerSSA(ssaURL, select2PlaceHolder) {
    $("#customerID").select2({
        ajax: {
            url: ssaURL,
            dataType: "json",
            delay: 50,
            data: function (params) {
                return {
                    q: params.term, // search term
                    page: params.page
                };
            },
        },
        placeholder: select2PlaceHolder,
        escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
        minimumInputLength: 1,
        templateResult: templateResult,
        templateSelection: templateSelection
    });
}

//-> templateResult
function templateResult(data) {
    if (data.loading)
        return data.text;

    var markup = `<div class="row">
                            <div class="col-4"> ${data.firstName}  </div>
                            <div class="col-4"> ${data.lastName}  </div>
                            <div class="col-4"> ${data.code}  </div>
                        </div>`;
    return markup;
}

//-> templateSelection
function templateSelection(data) {
    if (data.id === "")
        return 'Customer';
    return data.firstName + " " + data.lastName;
}