importScripts('Ajax.js');
MakeAjaxRequest('/Profile/GetActiveUsers', function (xhr) {
    postMessage(JSON.parse(xhr.responseText));
});