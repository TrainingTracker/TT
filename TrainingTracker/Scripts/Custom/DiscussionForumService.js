$(document).ready(function (my) {
    "use strict";
    my.discussionForumService = {
        getPosts: function (wildcard, traineeId, statusId, searchPostId, pageNumber, callback) {
            my.ajaxService.ajaxGetJson(my.rootUrl + "/Profile/GetPosts?wildcard=" + wildcard +
            "&traineeId=" + traineeId + "&statusId=" + statusId + "&searchPostId=" + searchPostId
            + "&pageNumber=" + pageNumber, null, callback);
        },
        getPostById: function (postId, callback) {
            my.ajaxService.ajaxGetJson(my.rootUrl + "/Profile/GetPostById?postId="
                + postId, null, callback);
        },
        addPost: function (post, callback) {
            my.ajaxService.ajaxPostJson(my.rootUrl + "/Profile/AddPost", post, callback);
        },
        addPostThread: function (postThread, callback) {
            my.ajaxService.ajaxPostJson(my.rootUrl + "/Profile/AddPostThread", postThread, callback);
        },
        updatePostStatus: function (postId, statusId, message, userId, callback) {
            my.ajaxService.ajaxPostJson(my.rootUrl + "/Profile/UpdatePostStatus?postId=" + postId +
            "&statusId=" + statusId + "&message=" + message + "&addedFor=" + userId, null, callback);
        }
    };
}(my));