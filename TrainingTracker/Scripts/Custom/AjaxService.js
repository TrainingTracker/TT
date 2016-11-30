$(document).ready(function (my) {
    "use strict";

    var serviceBase = my.rootUrl,
        getServiceUrl = function (method) {
            if (typeof(serviceBase) == 'undefined') {
                serviceBase = "";
            }
            return serviceBase + method;
        };

    my.ajaxService = (function () {
        var ajaxGetJson = function (method, jsonIn, callback) {
            $.ajax({
                url: getServiceUrl(method),
                type: "GET",
                beforeSend: my.toggleLoader(true),
                data: ko.toJSON(jsonIn),
                dataType: "json",
                contentType: "application/json",
                success: function (json)
                {
                    callback(json);
                    my.toggleLoader();
                },
                error: function() {
                    my.toggleLoader();
                }
                    
            });
        },
         ajaxPostJson = function (method, jsonIn, callback) {
             $.ajax({
                 url: getServiceUrl(method),
                 type: "POST",
                 beforeSend: my.toggleLoader(true),
                 data: ko.toJSON(jsonIn),
                 dataType: "json",
                 contentType: "application/json",
                 success: function (json)
                 {
                     callback(json);
                     my.toggleLoader();
                 },
                 error: function ()
                 {
                     my.toggleLoader();
                 }
             });
         },
        ajaxUploadImage = function (method, formData, callback) {
             $.ajax({
                 url: getServiceUrl(method),
                 type: "POST",
                 beforeSend: my.toggleLoader(true),
                 data: formData,
                 cache: false,
                 contentType: false,
                 processData: false,
                 success: function (json)
                 {
                     callback(json);
                     my.toggleLoader();
                 },
                 error: function ()
                 {
                     my.toggleLoader();
                 }
             });
        },
        
        ajaxPostDeffered = function (method, jsonIn, callback)
        {
            return $.ajax({
                url: getServiceUrl(method),
                type: "POST",
                beforeSend: my.toggleLoader(true),
                data: ko.toJSON(jsonIn),
                dataType: "json",
                contentType: "application/json"
            });
        };
        return {
            ajaxGetJson: ajaxGetJson,
            ajaxPostJson: ajaxPostJson,
            ajaxUploadImage: ajaxUploadImage,
            ajaxPostDeffered: ajaxPostDeffered
        };
    })();
}(my));

