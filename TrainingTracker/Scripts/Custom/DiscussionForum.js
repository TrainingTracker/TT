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
                categoryId: 1,
                searchPostId: ko.observable(my.queryParams["postId"]),
            },
        selections = {
                categoryId: ko.observable(0),
                statusId: ko.observable(0),
                wildcard: ko.observable(""),
                searchPostId: ko.observable(my.queryParams["postId"]),
                postId: ko.observable(0),
                newPostPanelShow: ko.observable(false),
                post: {
                    Title: ko.observable(""),
                    Description: ko.observable(""),
                    AddedByUser: ko.observable(""),
                    AddedByUserImage: ko.observable(""),
                    CategoryId: ko.observable(0),
                    StatusId: ko.observable(0),
                    CreatedOn: ko.observable(""),
                    Threads: ko.observableArray([])
                }
            },
            alerts = {
                postValidation: ko.observable(""),
                postAddedSuccess: ko.observable("")
            },
            newPostThread = {
                Description: ko.observable(""),
                AddedBy: 0,
                PostId: 0
            },
            newPost = {
                Title: ko.observable(""),
                Description: ko.observable(""),
                AddedBy: 0,
                CategoryId: 1,
                StatusId: 1
            },
            getAddSuccessMessage = function () {
                var message = '';
                //if (my.discussionForumVm.newPost.CategoryId() == 1) {
                //    message = "<strong>Thanks for pointing this out. </strong> This bug won't be spared.";
                //}
                //else if (my.discussionForumVm.newPost.CategoryId() == 2) {
                //    message = "<strong>Ideas are root of creation.</strong> Thank you for sharing your valuable idea.";
                //}
                //else if (my.discussionForumVm.newPost.CategoryId() == 3) {
                //    message = "<strong>Help on the way!! </strong> The team will get back to you shortly.";
                //}
                message = "New post is successfully added";
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
            addPostThreadCallback = function (response) {
                my.discussionForumVm.newPostThread.Description("");
                my.discussionForumVm.getPost(my.discussionForumVm.selections.postId());
            },
            addPostThread = function () {
                my.discussionForumVm.newPostThread.PostId = my.discussionForumVm.selections.postId();
                my.discussionForumVm.newPostThread.AddedBy = my.meta.currentUser.UserId;
                my.discussionForumService.addPostThread(my.discussionForumVm.newPostThread, my.discussionForumVm.addPostThreadCallback);
            },
            updatePostStatus = function (statusId) {

                $.confirm({
                    title: 'Change the status!',
                    content: '<span>This will change the Status,  <label>Press OK!</label> to proceed. </span>',
                    columnClass: 'col-md-6 col-md-offset-3 col-sm-8 col-sm-offset-2 col-xs-10',
                    useBootstrap: true,
                    buttons: {
                        confirm:
                        {
                            text: 'OK!',
                            btnClass: 'btn-primary btn-success',
                            action: function () {
                                var message = my.meta.currentUser.FirstName + ' ' + my.meta.currentUser.LastName + ' Updated the status to ';
                                switch (statusId) {
                                    case 1:
                                        message += "New";
                                        break;
                                    case 2:
                                        message += "In Discussion";
                                        break;
                                    case 3:
                                        message += "In Progress";
                                        break;
                                    case 4:
                                        message += "Closed";
                                        break;
                                }
                                my.discussionForumService.updatePostStatus(my.discussionForumVm.selections.postId(), statusId, message,
                                    my.meta.currentUser.UserId, my.discussionForumVm.addPostThreadCallback);
                                return;
                            }
                        },
                        cancel:
                        {
                            text: 'Cancel',
                            btnClass: 'btn-primary btn-warning',
                            action: function () {
                            }
                        }
                    }
                });
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
                    my.discussionForumVm.posts.CurrentPage(response.Posts.CurrentPage);
                    my.discussionForumVm.posts.PageCount(response.Posts.PageCount);
                    my.discussionForumVm.posts.PageSize(response.Posts.PageSize);
                    my.discussionForumVm.posts.RowCount(response.Posts.RowCount);
                    my.discussionForumVm.posts.DisplayPosts([]);
                    $.each(response.Posts.Results, function (arrayId, item) {
                        my.discussionForumVm.posts.DisplayPosts.push(item);
                    });
                }
            },
            getPosts = function () {
                if (!my.discussionForumVm.filters.searchPostId()) {
                    my.discussionForumVm.filters.searchPostId(0);
                }

                my.discussionForumService.getPosts(my.discussionForumVm.filters.wildcard(), my.discussionForumVm.filters.categoryId,
                    my.discussionForumVm.filters.statusId(), my.discussionForumVm.filters.searchPostId(),
                    my.discussionForumVm.posts.CurrentPage(), my.discussionForumVm.getPostsCallback);
            },
            getPostCallback = function (response) {
                if (response) {
                    if (my.discussionForumVm.selections.searchPostId() !== 0) {
                        my.discussionForumVm.selections.categoryId(response.CategoryId);
                        my.discussionForumVm.selections.searchPostId(0);
                    }
                    my.discussionForumVm.selections.postId(response.PostId);
                    my.discussionForumVm.selections.post.Title(response.Title);
                    my.discussionForumVm.selections.post.Description(response.Description);
                    my.discussionForumVm.selections.post.AddedByUser(response.AddedByUser.FirstName + ' ' + response.AddedByUser.LastName);
                    my.discussionForumVm.selections.post.AddedByUserImage(response.AddedByUser.ProfilePictureName);
                    my.discussionForumVm.selections.post.CategoryId(response.CategoryId);
                    my.discussionForumVm.selections.post.StatusId(response.StatusId);
                    my.discussionForumVm.selections.post.CreatedOn(response.CreatedOn);
                    my.discussionForumVm.selections.post.Threads([]);
                    $.each(response.Threads, function (arrayId, item) {
                        my.discussionForumVm.selections.post.Threads.push(item);
                    });
                    my.discussionForumVm.selections.newPostPanelShow(false);
                }
            },
            getPost = function (postId) {
                my.discussionForumService.getPostById(postId, my.discussionForumVm.getPostCallback);
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
            selections: selections,
            validateAndAddPost: validateAndAddPost,
            addPostCallback: addPostCallback,
            addPost: addPost,
            newPost: newPost,
            getPostCallback: getPostCallback,
            getPost: getPost,
            getPostsCallback: getPostsCallback,
            getPosts: getPosts,
            photoUrl: photoUrl,
            getNextPage: getNextPage,
            getPreviousPage: getPreviousPage,
            resetAndGetPosts: resetAndGetPosts,
            newPostThread: newPostThread,
            addPostThreadCallback: addPostThreadCallback,
            addPostThread: addPostThread,
            updatePostStatus: updatePostStatus,
            getAddSuccessMessage: getAddSuccessMessage,
            filters: filters
        };
    }();
    //ko.applyBindings(my.discussionForumVm);
});