$(document).ready(function () {
    my.helpForumVm = function () {
        var vm = {
            Categories: [{ Id: 1, Title: "Bugs" }, { Id: 2, Title: "Ideas" }, { Id: 3, Title: "Help" }]
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
        };
        return {
            vm: vm,
            alerts: alerts,
            selections: selections,
            validateAndAddPost: validateAndAddPost,
            addPostCallback: addPostCallback,
            addPost: addPost,
            newPost: newPost
        };
    }();
    ko.applyBindings(my.helpForumVm);
});