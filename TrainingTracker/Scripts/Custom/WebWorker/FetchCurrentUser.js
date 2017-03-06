importScripts('Ajax.js');

self.addEventListener("message", function(e) {
    MakeAjaxRequest(e.data + '/Login/GetCurrentUser', function (xhr) {
        postMessage(JSON.parse(xhr.responseText));
    });
}, false);

