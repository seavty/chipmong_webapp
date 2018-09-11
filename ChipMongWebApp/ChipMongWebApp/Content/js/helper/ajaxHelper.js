const requestMethod = {
    GET: "GET",
    POST: "POST",
    DELETE: "DELETE",
    PUT: "PUT"
};

//-> ajaxHelper
const ajaxHelper = (url, data, method) => {
    $('#loadingIndicator').modal({
        keyboard: false,
        backdrop: "static"
    });
    let promise = new Promise((resolve, reject) => {
        window.setTimeout(() => {
            $.ajax({
                url: url,
                data: data,
                type: method,
                async: false,
                error: (jqXHR, textStatus, errorThrown) => {
                    if (jqXHR.status == 401) 
                        window.location.href = $("#baseURL").val() + "/auth/login/-1";
                    else if (jqXHR.status == 400)
                        alert(jqXHR.responseText);
                    else 
                        alert("Error code: " + jqXHR.status + "; Message: error occured while processing your request.");
                    $('#loadingIndicator').modal("hide");
                    return;
                },
                beforeSend: () => {},
                success: (data) => {
                    $('#loadingIndicator').modal("hide");
                    resolve(data);
                }
            });
        }, 100);
    });
    return promise;
}


//-> Simple ajax
let simpleAjax = (url, data, method) => {
    var promise = new Promise((resolve, reject) => {
        window.setTimeout(() => {
            $.ajax({
                url: url,
                data: data,
                type: method,
                async: false,
                error: (jqXHR, textStatus, errorThrown) =>  {
                    if (jqXHR.status == 401) 
                        window.location.href = $("#baseURL").val() + "/auth/login/-1";
                    else if (jqXHR.status == 400)
                        alert(jqXHR.responseText);
                    else
                        alert("Error code: " + jqXHR.status + "; Message: error occured while processing your request.");
                    return;
                },
                beforeSend: () => {},
                success: (data) => {
                    resolve(data);
                }
            });
        }, 100);
    });
    return promise;
}
