$(document).ready(function ()
{
    my.feedbackThreadsVm = function () {
        var feedbackData = { 
            FeedbackId: ko.observable(0),
            User: {
                UserImageUrl: ko.observable(""),
                UserFullName: ko.observable(""),
                UserId: ko.observable(0)
            },
            FeedbackType:
            {
                FeedbackTypeId: ko.observable(0),
                FeedbackTypeDescription: ko.observable(""),
            },
            StartDate:ko.observable(""),
            EndDate: ko.observable(""),
            AddedOn: ko.observable(""),
            Rating: ko.observable(0),
            Title: ko.observable(""),
            FeedbackText: ko.observable(""),
            Threads:ko.observableArray([])
        };

        var threadData =
        {
            FeedbackId: ko.observable(0),
            Comments: ko.observable(""),
            AddedBy:{
                UserId: 0
            }
        };
               
        var viewSettings=
        {
            showDailog: ko.observable(false),
            anyNewThreadAdded:false
        };

        var openFeedbackDailog = function () {
          
            my.feedbackThreadsVm.viewSettings.showDailog(true);
        };
        
        var closeFeedbackDailog = function () {
            resetFeedbackData();
            resetThreadData();
            my.feedbackThreadsVm.viewSettings.showDailog(false);
            
            if (typeof (my.profileVm) !== 'undefined' && viewSettings.anyNewThreadAdded)
            {
                my.profileVm.applyFilter();
            }
            
        };

        var resetFeedbackData = function() {
            my.feedbackThreadsVm.feedbackData.FeedbackId(0),
            my.feedbackThreadsVm.feedbackData.User.UserImageUrl("");
            my.feedbackThreadsVm.feedbackData.User.UserFullName("");
            my.feedbackThreadsVm.feedbackData.User.UserId(0);
            my.feedbackThreadsVm.feedbackData.FeedbackType.FeedbackTypeId(0);
            my.feedbackThreadsVm.feedbackData.FeedbackType.FeedbackTypeDescription("");
            my.feedbackThreadsVm.feedbackData.StartDate("");
            my.feedbackThreadsVm.feedbackData.EndDate("");
            my.feedbackThreadsVm.feedbackData.AddedOn("");
            my.feedbackThreadsVm.feedbackData.Rating(0);
            my.feedbackThreadsVm.feedbackData.Title(0);
            my.feedbackThreadsVm.feedbackData.FeedbackText("");
            my.feedbackThreadsVm.feedbackData.Threads([]);
        };

        var resetThreadData = function() {
            my.feedbackThreadsVm.threadData.FeedbackId(0);
            my.feedbackThreadsVm.threadData.Comments("");
            my.feedbackThreadsVm.threadData.AddedBy.UserId = 0;           
        };

        var loadFeedbackDailog = function (feedbackId, feedbackDetails) {
            
            if (typeof (feedbackDetails) === "undefined")
            {
                my.userService.getFeedbackWithThreads(feedbackId, loadFeedbackDailogCallback);
                return;
            }
            loadFeedbackData(feedbackDetails);
            my.userService.getFeedbackThreads(feedbackId, loadThreadData);
            return;
        };

        var loadFeedbackDailogCallback = function (feedback)
        {
            if (feedback == null || typeof (feedback) == 'undefined') closeFeedbackDailog();

            resetFeedbackData();
            loadFeedbackData(feedback);
            loadThreadData(feedback.Threads);
            
        };

        var loadFeedbackData = function(feedback) {
            my.feedbackThreadsVm.feedbackData.FeedbackId(feedback.FeedbackId),
            my.feedbackThreadsVm.threadData.FeedbackId(feedback.FeedbackId);
            my.feedbackThreadsVm.feedbackData.User.UserImageUrl(feedback.AddedBy.ProfilePictureName);
            my.feedbackThreadsVm.feedbackData.User.UserFullName(feedback.AddedBy.FullName);
            my.feedbackThreadsVm.feedbackData.User.UserId(feedback.AddedBy.UserId);
            my.feedbackThreadsVm.feedbackData.FeedbackType.FeedbackTypeId(feedback.FeedbackType.FeedbackTypeId);
            my.feedbackThreadsVm.feedbackData.FeedbackType.FeedbackTypeDescription(feedback.FeedbackType.Description);
            my.feedbackThreadsVm.feedbackData.StartDate(feedback.StartDate);
            my.feedbackThreadsVm.feedbackData.EndDate(feedback.EndDate);
            my.feedbackThreadsVm.feedbackData.AddedOn(feedback.AddedOn);
            my.feedbackThreadsVm.feedbackData.Rating(feedback.Rating);
            my.feedbackThreadsVm.feedbackData.Title(feedback.Title);
            my.feedbackThreadsVm.feedbackData.FeedbackText(feedback.FeedbackText);

           
        };

        var loadThreadData = function(thread) {
            my.feedbackThreadsVm.feedbackData.Threads(thread);
            openFeedbackDailog();
            $(document).trigger('custom-resize');
        };

        var addNewThread = function() {

            if (!validateThreadDetails()) return;
            my.userService.addNewThread(ko.toJS(my.feedbackThreadsVm.threadData), addNewThreadCallback);
        };

        var addNewThreadCallback = function (response)
        {
            if (response) {                
                var feedback =
                {
                    AddedBy:
                    {
                        UserId: my.meta.currentUser.UserId,
                        FullName: my.meta.currentUser.FirstName + " " + my.meta.currentUser.LastName,
                        ProfilePictureName: my.meta.currentUser.ProfilePictureName,
                       
                     },
                    DateInserted: moment(new Date()),
                    Comments: my.feedbackThreadsVm.threadData.Comments()
                };                
                my.feedbackThreadsVm.feedbackData.Threads.push(feedback);
                my.feedbackThreadsVm.threadData.Comments("");
                viewSettings.anyNewThreadAdded = true;
            }
           
        };

        var validateThreadDetails = function() {

            return ( my.feedbackThreadsVm.threadData.FeedbackId() > 0 && !my.isNullorEmpty(my.feedbackThreadsVm.threadData.Comments()) && my.feedbackThreadsVm.threadData.Comments().length < 500 );
        };
               
        return {            
            feedbackData: feedbackData,
            viewSettings: viewSettings,
            loadFeedbackDailog: loadFeedbackDailog,
            closeFeedbackDailog: closeFeedbackDailog,
            openFeedbackDailog: openFeedbackDailog,
            threadData: threadData,
            addNewThread: addNewThread
        };
    }();   
});