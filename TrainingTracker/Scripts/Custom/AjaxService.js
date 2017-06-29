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
                    if (typeof (callback) != 'undefined') {
                        callback(json);
                    }
                   
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
                     if (typeof (callback) != 'undefined') {
                         callback(json);
                     }

                     my.toggleLoader();
                 },
                 error: function ()
                 {
                     my.toggleLoader();
                 }
             });
         },
        ajaxUploadImage = function (method, formData, callback,errorCallback) {
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
                     if (typeof (errorCallback) != 'undefined') {
                         errorCallback();
                     }
                     my.toggleLoader();
                 }
             });
        },
        
        ajaxPostDeffered = function (method, jsonIn)
        {
            return $.ajax({
                url: getServiceUrl(method),
                type: "POST",
                beforeSend: my.toggleLoader(true),
                data: ko.toJSON(jsonIn),
                dataType: "json",
                contentType: "application/json"
            });
        },

        ajaxGetDeffered = function (method, jsonIn) {
            return $.ajax({
                url: getServiceUrl(method),
                type: "GET",
                beforeSend: my.toggleLoader(true),
                data: ko.toJSON(jsonIn),
                dataType: "json",
                contentType: "application/json"
            });
        },
        ajaxPostDefferedCustomLoader = function (method, jsonIn) {
            return $.ajax({
                url: getServiceUrl(method),
                type: "POST",
                data: ko.toJSON(jsonIn),
                dataType: "json",
                contentType: "application/json"
            });
        },

        ajaxGetDefferedCustomLoader = function (method, jsonIn) {
            return $.ajax({
                url: getServiceUrl(method),
                type: "GET",
                data: ko.toJSON(jsonIn),
                dataType: "json",
                contentType: "application/json"
            });
        };
        return {
            ajaxGetJson: ajaxGetJson,
            ajaxPostJson: ajaxPostJson,
            ajaxUploadImage: ajaxUploadImage,
            ajaxPostDeffered: ajaxPostDeffered,
            ajaxGetDeffered: ajaxGetDeffered,
            ajaxGetDefferedCustomLoader: ajaxGetDefferedCustomLoader,
            ajaxPostDefferedCustomLoader:ajaxPostDefferedCustomLoader
        };
    })();
}(my));

