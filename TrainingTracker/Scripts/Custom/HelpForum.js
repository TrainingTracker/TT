$(document).ready(function () {
    my.helpForumVm = function () {
        var vm = {
            Categories: [{ Id: 1, Title: "Bugs" }, { Id: 2, Title: "Ideas" }, { Id: 3, Title: "Help" }]
            },
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
            selections = {
                categoryId: ko.observable(0),
                statusId: ko.observable(0),
                wildcard: ko.observable(""),
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
                CategoryId: ko.observable(),
                StatusId: 1
            },
            addPostCallback = function () {
                my.helpForumVm.alerts.postAddedSuccess("Question added.");
                my.helpForumVm.newPost.Title("");
                my.helpForumVm.newPost.Description("");
                my.helpForumVm.getPosts();
            },
            addPost = function () {
                my.helpForumVm.alerts.postAddedSuccess('');
                my.helpForumVm.newPost.AddedBy = my.meta.currentUser.UserId;
                my.helpForumService.addPost(my.helpForumVm.newPost, my.helpForumVm.addPostCallback);
            },
            addPostThreadCallback = function (response) {
                my.helpForumVm.newPostThread.Description("");
                my.helpForumVm.getPost(my.helpForumVm.selections.postId());
            },
            addPostThread = function() {
                my.helpForumVm.newPostThread.PostId = my.helpForumVm.selections.postId();
                my.helpForumVm.newPostThread.AddedBy = my.meta.currentUser.UserId;
                my.helpForumService.addPostThread(my.helpForumVm.newPostThread, my.helpForumVm.addPostThreadCallback);
            },
            updatePostStatus = function (statusId) {
                var message = my.meta.currentUser.FirstName + ' ' + my.meta.currentUser.LastName + ' Updated the status to ';
                switch(statusId) {
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
                my.helpForumService.updatePostStatus(my.helpForumVm.selections.postId(), statusId,message,
                    my.meta.currentUser.UserId, my.helpForumVm.addPostThreadCallback);
            },
            validateAndAddPost = function () {
            var message = '';
            my.helpForumVm.alerts.postValidation('');
            if (my.helpForumVm.newPost.Title().trim() === '') {
                message += "Title is mandatory. ";
            }
            if (my.helpForumVm.newPost.Description().trim() === '') {
                message += "Description is mandatory. ";
            }

                if (message === '') {
                my.helpForumVm.addPost();
                } else {
                my.helpForumVm.alerts.postValidation(message);
                }
            },
            getPostsCallback = function (response) {
                my.helpForumVm.posts.CurrentPage(response.CurrentPage);
                my.helpForumVm.posts.PageCount(response.PageCount);
                my.helpForumVm.posts.PageSize(response.PageSize);
                my.helpForumVm.posts.RowCount(response.RowCount);
                my.helpForumVm.posts.DisplayPosts([]);
                $.each(response.Results, function (arrayId, item) {
                    my.helpForumVm.posts.DisplayPosts.push(item);
                });
            },
            getPosts = function () {
                my.helpForumService.getPosts(my.helpForumVm.selections.wildcard(), my.helpForumVm.selections.categoryId(),
                    my.helpForumVm.selections.statusId(), my.helpForumVm.posts.CurrentPage(), my.helpForumVm.getPostsCallback);
            },
            getPostCallback = function (response) {
                my.helpForumVm.selections.post.Title(response.Title);
                my.helpForumVm.selections.post.Description(response.Description);
                my.helpForumVm.selections.post.AddedByUser(response.AddedByUser.FirstName + ' ' + response.AddedByUser.LastName);
                my.helpForumVm.selections.post.AddedByUserImage(response.AddedByUser.ProfilePictureName);
                my.helpForumVm.selections.post.CategoryId(response.CategoryId);
                my.helpForumVm.selections.post.StatusId(response.StatusId);
                my.helpForumVm.selections.post.CreatedOn(response.CreatedOn);
                my.helpForumVm.selections.post.Threads([]);
                $.each(response.Threads, function (arrayId, item) {
                    my.helpForumVm.selections.post.Threads.push(item);
                });
                my.helpForumVm.selections.newPostPanelShow(false);
            },
            getPost = function (postId) {
                my.helpForumVm.selections.postId(postId);
                my.helpForumService.getPostById(postId, my.helpForumVm.getPostCallback);
            },
            getNextPage = function () {
                my.helpForumVm.posts.CurrentPage(my.helpForumVm.posts.CurrentPage() + 1);
                my.helpForumVm.getPosts();
            },
            getPreviousPage = function () {
                my.helpForumVm.posts.CurrentPage(my.helpForumVm.posts.CurrentPage() - 1);
                my.helpForumVm.getPosts();
            },
            resetAndGetPosts= function() {
                my.helpForumVm.posts.CurrentPage(1);
                my.helpForumVm.posts.PageCount(0);
                my.helpForumVm.posts.PageSize(0);
                my.helpForumVm.posts.RowCount(0);
                my.helpForumVm.getPosts();
            };
        return {
            vm: vm,
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
            updatePostStatus: updatePostStatus
        };
    }();
    ko.applyBindings(my.helpForumVm);
    my.helpForumVm.getPosts();
});