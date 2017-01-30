$(document).ready(function() {
    my.helpForumVm = function() {
        var vm = {
                Categories: [{ Id: 1, Title: "Bugs" }, { Id: 2, Title: "Ideas" }, { Id: 3, Title: "Help" }]
            },
            newPost = {
                Title: ko.observable(""),
                Description: ko.observable(""),
                AddedBy: 0,
                CategoryId: ko.observable(),
                StatusId:1
            },
            validateAndAddPost = function() {
                
            };
        return {
            vm: vm,
            newPost: newPost
        };
    }();
    ko.applyBindings(my.helpForumVm);
});