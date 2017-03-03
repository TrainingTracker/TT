importScripts('Ajax.js');

self.addEventListener("message", function (e) {
    MakeAjaxRequest(e.data + '/Profile/GetActiveUsers', function (xhr) {
        postMessage(JSON.parse(xhr.responseText));
    });

}, false);