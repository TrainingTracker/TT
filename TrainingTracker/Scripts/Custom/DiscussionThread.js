$(document).ready(function ()
{
    my.discussionThreadsVm = function () {
        var discussionPostData = { 
            PostId: ko.observable(0),
            User: {
                UserImageUrl: ko.observable(""),
                UserFullName: ko.observable(""),
                UserId: ko.observable(0)
            },
            StatusId: ko.observable(),
            Status: {
                StatusId:0,
                Title:ko.observable("")
            },
            AddedBy : ko.observable(),
            AddedOn: ko.observable(""),
            Title: ko.observable(""),
            Description: ko.observable(""),
            Threads:ko.observableArray([])
        };

        var newThreadData =
        {
            PostId: ko.observable(0),
            Description: ko.observable(""),
            AddedByUser:{
                UserId: 0
            },
            AddedBy : 0,
            AddedFor: 0
        };
               
        var viewSettings=
        {
            showDialog: ko.observable(false),
            anyNewThreadAdded:false
        };

        var openDiscussionDialog = function () {
            my.discussionThreadsVm.viewSettings.showDialog(true);
        };
        
        var closeDiscussionDialog = function () {
            resetDiscussionData();
            resetThreadData();
            my.discussionThreadsVm.viewSettings.showDialog(false);
            
            if (typeof (my.profileVm) !== 'undefined' && viewSettings.anyNewThreadAdded)
            {
                my.profileVm.applyFilter();
            }
            
        };

        var resetDiscussionData = function () {
            my.discussionThreadsVm.discussionPostData.PostId(0),
            my.discussionThreadsVm.discussionPostData.User.UserImageUrl("");
            my.discussionThreadsVm.discussionPostData.User.UserFullName("");
            my.discussionThreadsVm.discussionPostData.User.UserId(0);
            my.discussionThreadsVm.discussionPostData.StatusId(0);
            my.discussionThreadsVm.discussionPostData.Status.Title("");
            my.discussionThreadsVm.discussionPostData.AddedOn("");
            my.discussionThreadsVm.discussionPostData.Title("");
            my.discussionThreadsVm.discussionPostData.Description("");
            my.discussionThreadsVm.discussionPostData.Threads([]);
        };

        var resetThreadData = function() {
            my.discussionThreadsVm.newThreadData.PostId(0);
            my.discussionThreadsVm.newThreadData.Description("");
            my.discussionThreadsVm.newThreadData.AddedByUser.UserId = 0;           
        };

        var loadDiscussionDialog = function (postId, discussionDetails) {
            
            //if (typeof (discussionDetails) === "undefined")
            //{
            //    my.discussionForumService.getPostById(postId, loadDiscussionDialogCallback);
            //    return;
            //}
            my.discussionForumService.getPostById(postId, loadDiscussionDialogCallback);
            return;
        };

        var loadDiscussionDialogCallback = function (data)
        {
            if (data == null || typeof (data) == 'undefined') {
                closeDiscussionDialog();
            } else {
                resetDiscussionData();
                loadDiscussionData(data);
                loadThreadData(data.Threads);
            }
            
            
        };

        var loadDiscussionData = function(data) {
            my.discussionThreadsVm.discussionPostData.PostId(data.PostId),
            my.discussionThreadsVm.newThreadData.PostId(data.PostId);
            my.discussionThreadsVm.discussionPostData.User.UserImageUrl(data.AddedByUser.ProfilePictureName);
            my.discussionThreadsVm.discussionPostData.User.UserFullName(data.AddedByUser.FirstName + ' ' + data.AddedByUser.LastName);
            my.discussionThreadsVm.discussionPostData.User.UserId(data.AddedBy.UserId);
            my.discussionThreadsVm.discussionPostData.StatusId(data.StatusId);
            my.discussionThreadsVm.discussionPostData.Status.Title(data.Status.Title);
            my.discussionThreadsVm.discussionPostData.AddedOn(data.CreatedOn);
            my.discussionThreadsVm.discussionPostData.Title(data.Title);
            my.discussionThreadsVm.discussionPostData.AddedBy(data.AddedBy);
            my.discussionThreadsVm.discussionPostData.Description(data.Description);

           
        };

        var loadThreadData = function(thread) {
            my.discussionThreadsVm.discussionPostData.Threads(thread);
            openDiscussionDialog();
            $(document).trigger('custom-resize');
        };

        var addNewThread = function() {

            if (!validateThreadDetails()) return;
            newThreadData.AddedFor = discussionPostData.AddedBy();
            newThreadData.PostId = discussionPostData.PostId();
            newThreadData.AddedBy = my.meta.currentUser.UserId;
            my.discussionForumService.addPostThread(ko.toJS(my.discussionThreadsVm.newThreadData), addNewThreadCallback);
        };

        var addNewThreadCallback = function (response)
        {
            if (response) {                
                var newThread =
                {
                    AddedByUser:
                    {
                        UserId: my.meta.currentUser.UserId,
                        FirstName : my.meta.currentUser.FirstName,
                        LastName : my.meta.currentUser.LastName,
                        ProfilePictureName: my.meta.currentUser.ProfilePictureName,
                       
                     },
                    CreatedOn: moment(new Date()),
                    Description: my.discussionThreadsVm.newThreadData.Description()
                };                
                my.discussionThreadsVm.discussionPostData.Threads.push(newThread);
                my.discussionThreadsVm.newThreadData.Description("");
                viewSettings.anyNewThreadAdded = true;
            }
           
        };

        var validateThreadDetails = function() {

            return ( my.discussionThreadsVm.newThreadData.PostId() > 0 && !my.isNullorEmpty(my.discussionThreadsVm.newThreadData.Description()) && my.discussionThreadsVm.newThreadData.Description().length < 500 );
        };

        var updatePostStatus = function(statusId) {

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
                            var statusTitle = "";
                            var message = my.meta.currentUser.FirstName + ' ' + my.meta.currentUser.LastName + ' Updated the status to ';
                            switch (statusId) {
                            case 1:
                                message += "New";
                                statusTitle = "New";
                                break;
                            case 2:
                                message += "In Discussion";
                                statusTitle = "In Discussion";
                                break;
                            case 3:
                                message += "Closed";
                                statusTitle = "Closed";
                                break;
                            }
                            my.discussionThreadsVm.newThreadData.Description(message);
                            my.discussionThreadsVm.discussionPostData.StatusId(statusId);
                            my.discussionThreadsVm.discussionPostData.Status.Title(statusTitle);
                            my.discussionForumService.updatePostStatus(my.discussionThreadsVm.discussionPostData.PostId(), statusId, message,
                                discussionPostData.AddedBy(), addNewThreadCallback);
                            return;
                        }
                    },
                    cancel:
                    {
                        text: 'Cancel',
                        btnClass: 'btn-primary btn-warning',
                        action: function() {
                        }
                    }
                }
            });
        };
               
        return {            
            discussionPostData: discussionPostData,
            viewSettings: viewSettings,
            loadDiscussionDialog: loadDiscussionDialog,
            closeDiscussionDialog: closeDiscussionDialog,
            openDiscussionDialog: openDiscussionDialog,
            newThreadData: newThreadData,
            addNewThread: addNewThread,
            updatePostStatus: updatePostStatus
        };
    }();   
});