$(document).ready(function (my) {
    "use strict";
    my.helpForumService = {
        getPosts: function (wildcard, categoryId, statusId, pageNumber, callback) {
            my.ajaxService.ajaxGetJson(my.rootUrl + "/Help/GetPosts?wildcard=" + wildcard +
            "&categoryId=" + categoryId + "&statusId=" + statusId + "&pageNumber=" + pageNumber, null, callback);
        },
        getPostById: function (postId, callback) {
            my.ajaxService.ajaxGetJson(my.rootUrl + "/Help/GetPostById?postId="
                + postId, null, callback);
        },
        addPost: function (post, callback) {
            my.ajaxService.ajaxPostJson(my.rootUrl + "/Help/AddPost", post, callback);
        },
        addPostThread: function (postThread, callback) {
            my.ajaxService.ajaxPostJson(my.rootUrl + "/Help/AddPostThread", postThread, callback);
        },
        updatePostStatus: function (postId, statusId, message, userId, callback) {
            my.ajaxService.ajaxPostJson(my.rootUrl + "/Help/UpdatePostStatus?postId=" + postId +
            "&statusId=" + statusId + "&message=" + message + "&userId=" + userId, null, callback);
        }
    };
}(my));