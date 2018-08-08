const initializeComponents = () => {
    setupSSA("#customerID", ssaURL, ssaPlaceHolder, customerTemplateResult, customerTemplateSelection);
}


//-> deleteLineItem
const deleteLineItem = (prop) => {
    if (confirm("Do you to delete this line item?")) {
        let rowIndex = prop.closest("tr").attr("rowIndex");
        let deleteLineItemID = $("#deleteLineItemID").val();
        let lineItemID = $("#lineItemID" + rowIndex).val();
        deleteLineItemID += "," + lineItemID;
        $("#deleteLineItemID").val(deleteLineItemID);
        prop.closest("tr").remove();
        headerCalculation();
    }
}

//-> saveLineItem
const saveLineItem = (prop) => {
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

const headerCalculation = () => {
    let sum = 0;
    $(".total").each(function () {
        var value = toFloat($(this).val());
        sum += parseFloat(value);
    });
    $("#total").val(toFloatWithTwoPrecision(sum));
}


const calculation = (prop) => {
    let index = prop.closest("tr").attr("rowIndex")
    let quantity = toFloat($("#quantity" + index).val());
    let price = toFloat($("#price" + index).val());
    let total = quantity * price;
    $("#total" + index).val(toFloatWithTwoPrecision(total));
    if (index > 0)
        headerCalculation();
}

const getItem = (prop, endPoint) => {
    let index = prop.closest("tr").attr("rowIndex")
    let itemID = prop.val();
    simpleAjax(endPoint + "/record/" + itemID, null, requestMethod.GET).then(function (data) {
        $("#price" + index).val(toFloatWithTwoPrecision(data.price));
        calculation(prop);
    });
}


