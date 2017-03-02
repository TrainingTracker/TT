importScripts('Ajax.js');
MakeAjaxRequest('/Login/GetCurrentUser', function (xhr) {
    postMessage(JSON.parse(xhr.responseText));
});
