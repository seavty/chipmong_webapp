function setupDatePicker(selector) {
    $(selector).datepicker({
        uiLibrary: "bootstrap4",
        iconsLibrary: "fontawesome",
        format: "dd/mm/yyyy",
    });
}