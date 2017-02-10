$(document).ready(function () {
    my.discussionForumVm = function () {

        var 
            photoUrl = function (pictureName) {
                return my.rootUrl + "/Uploads/ProfilePicture/" + pictureName;
            },
            posts = {
                CurrentPage: ko.observable(1),
                PageCount: ko.observable(0),
                PageSize: ko.observable(0),
                RowCount: ko.observable(0),
                DisplayPosts: ko.observableArray([])
            },
            filters = {
                statusId: ko.observable(0),
                wildcard: ko.observable(""),
                searchPostId: ko.observable(my.queryParams["postId"]),
            },
            alerts = {
                postValidation: ko.observable(""),
                postAddedSuccess: ko.observable("")
            },
            newPost = {
                Title: ko.observable(""),
                Description: ko.observable(""),
                AddedBy: 0,
                StatusId: 1
            },
            getAddSuccessMessage = function () {
                var message = "New post is successfully added";
                return message;
            },
            addPostCallback = function () {
                my.discussionForumVm.alerts.postAddedSuccess(my.discussionForumVm.getAddSuccessMessage());
                my.discussionForumVm.newPost.Title("");
                my.discussionForumVm.newPost.Description("");
                my.discussionForumVm.getPosts();
            },
            addPost = function () {
                my.discussionForumVm.alerts.postAddedSuccess('');
                my.discussionForumVm.newPost.AddedBy = my.meta.currentUser.UserId;
                my.discussionForumService.addPost(my.discussionForumVm.newPost, my.discussionForumVm.addPostCallback);
            },
           
            validateAndAddPost = function () {
                var message = '';
                my.discussionForumVm.alerts.postValidation('');
                if (my.discussionForumVm.newPost.Title().trim() === '') {
                    message += "Title is mandatory. ";
                }
                if (my.discussionForumVm.newPost.Description().trim() === '') {
                    message += "Description is mandatory. ";
                }
                if (message === '') {
                    my.discussionForumVm.addPost();
                } else {
                    message = "<strong>Something is missing!! </strong>" + message;
                    my.discussionForumVm.alerts.postValidation(message);
                }
            },
            getPostsCallback = function (response) {
                if (response) {
                    my.discussionForumVm.posts.CurrentPage(response.CurrentPage);
                    my.discussionForumVm.posts.PageCount(response.PageCount);
                    my.discussionForumVm.posts.PageSize(response.PageSize);
                    my.discussionForumVm.posts.RowCount(response.RowCount);
                    my.discussionForumVm.posts.DisplayPosts([]);
                    $.each(response.Results, function (arrayId, item) {
                        my.discussionForumVm.posts.DisplayPosts.push(item);
                    });
                }
            },
            getPosts = function () {
                if (!my.discussionForumVm.filters.searchPostId()) {
                    my.discussionForumVm.filters.searchPostId(0);
                }
               
                my.discussionForumService.getPosts(my.discussionForumVm.filters.wildcard(), my.profileVm.userVm.User.UserId,
                    my.discussionForumVm.filters.statusId(), my.discussionForumVm.filters.searchPostId(),
                    my.discussionForumVm.posts.CurrentPage(), my.discussionForumVm.getPostsCallback);
            },
           
            getNextPage = function () {
                my.discussionForumVm.posts.CurrentPage(my.discussionForumVm.posts.CurrentPage() + 1);
                my.discussionForumVm.getPosts();
            },
            getPreviousPage = function () {
                my.discussionForumVm.posts.CurrentPage(my.discussionForumVm.posts.CurrentPage() - 1);
                my.discussionForumVm.getPosts();
            },
            resetAndGetPosts = function () {
                my.discussionForumVm.posts.CurrentPage(1);
                my.discussionForumVm.posts.PageCount(0);
                my.discussionForumVm.posts.PageSize(0);
                my.discussionForumVm.posts.RowCount(0);
                my.discussionForumVm.getPosts();
            };
        return {
            posts: posts,
            alerts: alerts,
            validateAndAddPost: validateAndAddPost,
            addPostCallback: addPostCallback,
            addPost: addPost,
            newPost: newPost,
            getPostsCallback: getPostsCallback,
            getPosts: getPosts,
            photoUrl: photoUrl,
            getNextPage: getNextPage,
            getPreviousPage: getPreviousPage,
            resetAndGetPosts: resetAndGetPosts,
            getAddSuccessMessage: getAddSuccessMessage,
            filters: filters
        };
    }();
    //ko.applyBindings(my.discussionForumVm);
});