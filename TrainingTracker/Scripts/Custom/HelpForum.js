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
            postId: ko.observable(0)
        },
            alerts = {
                postValidation: ko.observable(""),
                postAddedSuccess: ko.observable("")
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
        validateAndAddPost = function () {
            var message = '';
            my.helpForumVm.alerts.postValidation('');
            if (my.helpForumVm.newPost.Title() === '') {
                message += "Title is mandatory. ";
            }
            if (my.helpForumVm.newPost.Description() === '') {
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
            getNextPage = function () {
                my.helpForumVm.posts.CurrentPage(my.helpForumVm.posts.CurrentPage() + 1);
                my.helpForumVm.getPosts();
            },
            getPreviousPage = function() {
                my.helpForumVm.posts.CurrentPage(my.helpForumVm.posts.CurrentPage() - 1);
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
            getPostsCallback: getPostsCallback,
            getPosts: getPosts,
            photoUrl: photoUrl,
            getNextPage: getNextPage,
            getPreviousPage: getPreviousPage
        };
    }();
    ko.applyBindings(my.helpForumVm);
    my.helpForumVm.getPosts();
});