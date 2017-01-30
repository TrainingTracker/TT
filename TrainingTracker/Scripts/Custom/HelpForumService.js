(document).ready(function (my) {
    "use strict";
    my.helpForumService = {
        getPosts: function (callback) {
            my.ajaxService.ajaxGetJson(my.rootUrl + "/Help/GetPosts", null, callback);
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
        }
    };
}(my));