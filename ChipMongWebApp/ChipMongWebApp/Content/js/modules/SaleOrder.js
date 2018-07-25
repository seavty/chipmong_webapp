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
    if (data.text != "") return data.text;
    if (data.id === "") return 'Customer';
    return data.firstName + " " + data.lastName;
}


//-> deleteLineItem
function deleteLineItem(prop) {
    if (confirm("Do you to delete this line item?")) {
        var rowIndex = prop.closest("tr").attr("rowIndex");
        var deleteLineItemID = $("#deleteLineItemID").val();
        var lineItemID = $("#lineItemID" + rowIndex).val();
        deleteLineItemID += "," + lineItemID;
        $("#deleteLineItemID").val(deleteLineItemID);
        prop.closest("tr").remove();
        headerCalculation();
    }
}

//-> headerCalculation
function headerCalculation() {
    var sum = 0;
    $(".total").each(function () {
        var value = toFloat($(this).val());
        sum += parseFloat(value);
    });
    $("#total").val(toFloatWithTwoPrecision(sum));
}

//-> saveLineItem
function saveLineItem(prop) {
    if (prop.attr('class') == "btn btn-success") {
        if (isValidLineItem()) {
            prop.removeClass("btn-success")
                .addClass("btn-danger")
                .attr('rowIndex', rowIndex)
                .html('<i class="fas fa-trash-alt"></i> ');
            rowIndex++;
            prop.closest("tr").attr("rowIndex", rowIndex)
            $("#quantity0").attr('id', "quantity" + (rowIndex));
            $("#price0").attr('id', "price" + (rowIndex));
            $("#total0").attr('id', "total" + (rowIndex));
            $("#tblLineItem").append(tableRow);
            headerCalculation();
        }
    }
    else {
        if (confirm("Do you to delete this line item?")) {
            prop.closest("tr").remove();
            headerCalculation();
        }
    }
}

//-> calculation
function calculation(prop) {
    var index = prop.closest("tr").attr("rowIndex")
    var quantity = toFloat($("#quantity" + index).val());
    var price = toFloat($("#price" + index).val());
    var total = quantity * price;
    $("#total" + index).val(toFloatWithTwoPrecision(total));
    if (index > 0)
        headerCalculation();
}

//-> getItem
function getItem(prop, endPoint) {
    var index = prop.closest("tr").attr("rowIndex")
    var itemID = prop.val();
    simpleAjax(endPoint + "/record/" + itemID, null, requestMethod.GET).then(function (data) {
        $("#price" + index).val(toFloatWithTwoPrecision(data.price));
        calculation(prop);
    });
}

//--saveRecord
function saveRecord(endPoint, action) {
    $(".select2-selection").css("border-color", "#ced4da")
    $("#sourceOfSupplyID").css("border-color", "#ced4da");
    if ($("#sourceOfSupplyID").val() == "")
        $("#sourceOfSupplyID").css("border-color", "red");

    if ($("#customerID").val() == null) return $(".select2-selection").css("border-color", "red");

    

    if (isValid()) {
        $('#tblLineItem tr:last').remove();
        ajaxHelper(endPoint + "/" + action, $('#record').serializeObject(), requestMethod.POST).then(function (data) {
            window.location.href = endPoint + "/view/" + data.id;
        });
    }
}

//-> setupEvents
function setupEvents() {
    $("#btnSave").click(function () { save(); });
    $("#btnCancel").click(function () { cancel(); });
    setupDatePicker("#date");
    setupDatePicker("#requiredDate");
    customerSSA(ssaURL, select2PlaceHolder);
    $('.numeric').numeric();
}
