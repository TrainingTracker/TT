$(document).ready(function () {
    my.profileVm = function () {
        var userId = my.queryParams["userId"],
            queryStringFeedbackId = my.queryParams["feedbackId"],
            showTimeline = ko.observable(false),
            selectedSkill = ko.observable(),
            selectedProject = ko.observable(),
            validationMessage = ko.observable(),
            tempAllTrainer = ko.observable(), // remove this once temporary feature use end
            recentCodeReviewFeedback = ko.observable(),
            recentWeeklyFeedback = ko.observable(),
            commentFeedbacks = ko.observableArray([]),
            isCommentFeedbackModalVisible = ko.observable(false),
            trainorSynopsis = ko.observable(),
            controls = {
                skillOption: ko.observable("1"),
                assignmentOption: ko.observable(1),
                crOption: ko.observable(1),
            },
            plotFilter =
            {
                StartDate: ko.observable(),
                EndDate: ko.observable(moment(new Date()).format('MM/DD/YYYY')),
                Trainer: ko.observable(),
                FeedbackType: ko.observableArray(['3', '4', '5']),
                TraineeId: 0
            },
            filter = {
                filterFeedback: ko.observable(),
                pageSize: ko.observableArray(['5', '10', '20', '100']),
                selectedPageSize: ko.observable(),
                tempAddedBy: ko.observable() // remove this once temproray feature use end
            },
            feedbackPost = {
                Title: ko.observable(),
                FeedbackText: ko.observable(),
                FeedbackType: ko.observable(),
                Skill: ko.observable(),
                AllSkills: ko.observable(),
                Rating: ko.observable(0),
                AddedFor: '',
                AddedBy: '',
                AddedOn: ko.observable(),
                StartDate: ko.observable(my.calculateLastMonday()),
                EndDate: ko.observable(my.calculateLastFriday()),
                selectedOption : ko.observable()
            },
            surveyQuestion = ko.observableArray([]),
            surveyResponse = [],
            setRating = function(rating) {
                my.profileVm.feedbackPost.Rating(rating);
            },
            toggleTimeline = function() {
                my.profileVm.showTimeline = !my.profileVm.showTimeline;
            },
            userVm = {},
            fullName = function(item) {
                return item.FirstName + " " + item.LastName;
            },
            photoUrl = function(item) {
                return my.rootUrl + "/Uploads/ProfilePicture/" + item.ProfilePictureName;
            },
            getUserCallback = function(jsonData) {
                jsonData.User.FullName = my.profileVm.fullName(jsonData.User);
                jsonData.User.PhotoUrl = my.profileVm.photoUrl(jsonData.User);
                $.each(jsonData.Feedbacks, function(arrayId, feedback) {
                    feedback.AddedBy.UserImageUrl = my.rootUrl + "/Uploads/ProfilePicture/" + feedback.AddedBy.ProfilePictureName;
                });
                jsonData.Feedbacks = ko.observableArray(jsonData.Feedbacks);
                my.profileVm.recentCodeReviewFeedback(jsonData.RecentCrFeedback); 
                my.profileVm.recentWeeklyFeedback(jsonData.RecentWeeklyFeedback);
                my.profileVm.trainorSynopsis(jsonData.TrainorSynopsis);
                my.profileVm.tempAllTrainer(jsonData.AllTrainer); // Temp Feature
                my.profileVm.plotFilter.StartDate(moment(jsonData.User.DateAddedToSystem).format('MM/DD/YYYY'));
                my.profileVm.plotFilter.TraineeId = jsonData.User.UserId;
                my.profileVm.userVm = jsonData;
                my.profileVm.feedbackPost.FeedbackType(my.profileVm.userVm.FeedbackTypes[0]);
                ko.applyBindings(my.profileVm);               
                my.profileVm.feedbackPost.Rating(0);

                if (!my.isNullorEmpty(queryStringFeedbackId)) {
                    loadFeedbackWithThread(queryStringFeedbackId);
                }

            },
            loadPlotData=function() {                   
                if (typeof(my.chartVm) !== 'undefined') {
                    my.chartVm.loadUserPlotData(plotFilter.TraineeId, plotFilter.StartDate(), plotFilter.EndDate(), plotFilter.FeedbackType(), typeof (plotFilter.Trainer()) == 'undefined' ? undefined : plotFilter.Trainer().UserId);
                }
            },
            getUser = function() {
                my.userService.getUserProfileVm(my.profileVm.userId, my.profileVm.getUserCallback);
            },
            validatePost = function() {
                var result = true;
                
                var validationMessageArray = [];

                if (my.profileVm.feedbackPost.FeedbackType().FeedbackTypeId != 5 && ( my.profileVm.feedbackPost.FeedbackText() == undefined ||
                    my.profileVm.feedbackPost.FeedbackText() == "")) {
                    //   my.profileVm.validationMessage("You need to add feedback text.");
                    validationMessageArray.push(" add feedback text");
                    result = false;
                }

                if (my.profileVm.feedbackPost.FeedbackType().FeedbackTypeId != 1) {
                    if (my.profileVm.feedbackPost.Rating() == undefined ||
                        my.profileVm.feedbackPost.Rating() == 0) {
                        //  my.profileVm.validationMessage("You need to select a rating to add feedback.");
                        validationMessageArray.push("select a rating to add feedback");
                        result = false;
                    }

                    if (my.profileVm.feedbackPost.FeedbackType().FeedbackTypeId == 3 &&
                        (typeof(my.profileVm.feedbackPost.Title()) == 'undefined'
                            || my.profileVm.feedbackPost.Title() == "")) {
                        validationMessageArray.push(" add assigment");
                        result = false;
                    }

                    if (my.profileVm.feedbackPost.FeedbackType().FeedbackTypeId == 5) {
                        if (my.isNullorEmpty(my.profileVm.feedbackPost.StartDate()) && my.isNullorEmpty(my.profileVm.feedbackPost.EndDate())) {
                            validationMessageArray.push(" enter start date & end date ");
                            result = false;
                        } else if (my.isNullorEmpty(my.profileVm.feedbackPost.StartDate())) {
                            validationMessageArray.push(" enter start date ");
                            result = false;
                        } else if (my.isNullorEmpty(my.profileVm.feedbackPost.EndDate())) {
                            validationMessageArray.push(" enter end date ");
                            result = false;
                        }

                        if (my.profileVm.feedbackPost.StartDate() > my.profileVm.feedbackPost.EndDate()) {
                            validationMessageArray.push(" end date should be greater than start date ");
                            result = false;
                        }
                    }
                }
                validationMessageArray.length ? my.profileVm.validationMessage("You need to" + validationMessageArray.join(', ') + ".") : my.profileVm.validationMessage("");
                return result;
            },
            addFeedbackCallback = function(jsonData) {
                if (my.profileVm.feedbackPost.FeedbackType() == "Skill Feedback") {
                    {

                    }
                } else if (my.profileVm.feedbackPost.FeedbackType() == "Project Feedback") {

                } else if (my.profileVm.feedbackPost.FeedbackType() == "Comment") {

                }
                
                my.profileVm.selectedSkill(0);
                my.profileVm.feedbackPost.FeedbackType("Comment");
                my.profileVm.feedbackPost.FeedbackText("");
                window.location = $(location).attr('origin') + $(location).attr('pathname') + "?userId=" + userId;
            },
            addFeedback = function() {
                my.profileVm.validationMessage("");

                if (my.profileVm.validatePost()) {
                    my.profileVm.feedbackPost.AddedFor = { UserId: my.profileVm.userId };
                    
                    if (typeof(my.profileVm.filter.tempAddedBy()) == 'undefined') {
                        my.profileVm.feedbackPost.AddedBy = { UserId: my.profileVm.currentUser.UserId };
                    } else {
                        my.profileVm.feedbackPost.AddedBy = { UserId: my.profileVm.filter.tempAddedBy().UserId };
                    }

                    var convertedObject = ko.toJS(my.profileVm.feedbackPost);
                    
                    switch (convertedObject.FeedbackType.FeedbackTypeId)
                    {
                        case 1:
                            convertedObject.Skill = 0;
                            convertedObject.StartDate = null;
                            convertedObject.EndDate = null;
                            convertedObject.Rating = 0;
                            break;
                            
                        case 2:
                            convertedObject.Skill = selectedSkill();
                            convertedObject.StartDate = null;
                            convertedObject.EndDate = null;
                            break;
                            
                        case 3:
                        case 4:
                            convertedObject.Skill = 0;
                            convertedObject.StartDate = null;
                            convertedObject.EndDate = null;
                            break;
                        case 5:
                            convertedObject.Skill = 0;
                            break;
                          
                    }
                    
                    my.userService.addUserFeedback(convertedObject, my.profileVm.addFeedbackCallback);
                }
            },
            currentUser = {},
            getCurrentUserCallback = function(user) {
                my.profileVm.currentUser = user;
                my.profileVm.currentUser.avatarUrl = my.profileVm.photoUrl(user);
            },
            getCurrentUser = function() {
                my.userService.getCurrentUser(my.profileVm.getCurrentUserCallback);
            },        
            applyFilter = function () {
                var filtertype = typeof (my.profileVm.filter.filterFeedback()) == 'undefined' ? 0 : (my.profileVm.filter.filterFeedback().FeedbackTypeId);       
                my.userService.getFeedbackonAppliedFilter(my.profileVm.filter.selectedPageSize(), filtertype, my.profileVm.userId,null,null, my.profileVm.applyFilterCallback);
            },
            applyFilterCallback = function (feedbacks) {
                my.profileVm.userVm.Feedbacks([]);
                $.each(feedbacks, function (key)
                {
                    feedbacks[key].AddedBy.UserImageUrl = my.rootUrl + "/Uploads/ProfilePicture/" + feedbacks[key].AddedBy.ProfilePictureName;
                    my.profileVm.userVm.Feedbacks.push(feedbacks[key]);
                });
            },
            getCountForFeedback=function(type,feedbackList) {
                var feedbackFilteredOnType = ko.utils.arrayFilter(feedbackList, function (item)
                {
                    return item.Rating == type;
                });
                return feedbackFilteredOnType.length;
            },            
        showCommentFeedback = function () {
            closeCommentFeedbackModal();
            my.profileVm.loadcommentFeedbacks();
            isCommentFeedbackModalVisible(true);
        },
        closeCommentFeedbackModal = function () {
            isCommentFeedbackModalVisible(false);
        },
        loadcommentFeedbacks = function () {
            my.userService.getFeedbackonAppliedFilter(100, 1, my.profileVm.userId, my.profileVm.feedbackPost.StartDate(), my.profileVm.feedbackPost.EndDate(), my.profileVm.loadCommentFeedbacksCallback);
        },
        loadCommentFeedbacksCallback = function (feedbacks) {
            my.profileVm.commentFeedbacks([]);
            
            ko.utils.arrayForEach(feedbacks, function (item)
            {
                my.profileVm.commentFeedbacks.push(item);
            });
        },
        isCommentCollapsed = ko.observable(false),
        loadFeedbackWithThread = function(feedbackId) {
            var filteredFeedback = ko.utils.arrayFilter(my.profileVm.userVm.Feedbacks(), function (item)
            {
                return item.FeedbackId == feedbackId;
            });
            
            if (filteredFeedback.length > 0 && my.profileVm.userVm.User.IsTrainee)
            {
                my.feedbackThreadsVm.loadFeedbackDailog(feedbackId, filteredFeedback[0]);
            }
            else
            {
                my.feedbackThreadsVm.loadFeedbackDailog(feedbackId);
            }
        },
        
        initializeSurveyQuestion = function (surveyQuestionJson)
        {
            if (!surveyQuestionJson.IsCodeReviewedForTrainee) {
                $.confirm({
                    title: 'Missing Code Review Feedback!',
                    content: 'Weekly survey automatically captures code review for the week, but no CR added for ' + my.profileVm.userVm.User.FirstName + '  in between ' + moment(my.profileVm.feedbackPost.StartDate()).format("dddd, MMMM Do YYYY") + ' and ' + moment(my.profileVm.feedbackPost.EndDate()).format("dddd, MMMM Do YYYY") +'.' + '</br><label>Do you want to add CR?</label>',
                    columnClass : 'col-md-6 col-md-offset-3 col-sm-8 col-sm-offset-2 col-xs-10 col-xs-offset-1' ,
                    useBootstrap: true,
                    buttons: {
                        confirm:
                        {
                            text: 'Yes, Add CR',
                            btnClass:'btn-primary btn-success',
                            action: function() {
                                my.profileVm.feedbackPost.selectedOption(4);
                                return;
                            }
                        },
                        cancel:
                        {
                            text: 'No, Continue with WF',
                            btnClass: 'btn-primary btn-warning',
                            action: function() {
                                bindSurveyQuestion(surveyQuestionJson);
                            }
                        }
                    }
                });
            } else {
                bindSurveyQuestion(surveyQuestionJson);
            }
        },
            
        bindSurveyQuestion = function (surveyQuestionJson)
        {

            surveyQuestion([]);
            var questionArray = [];

            var questionObject =
            {
                CategoryHeader: '',
                QuestionText: '',
                QuestionId: '',
                Answer: [],
                HelpText: '',
                SelectedAnswer: [],
                ResponseType: 0,
                IsMandatory: false,
                AdditionalNoteRequired: false
            };

            ko.utils.arrayForEach(surveyQuestionJson.Survey.SurveySubSections, function (sub)
            {
                ko.utils.arrayForEach(sub.Questions, function (question)
                {
                    var newObj = Object.create(questionObject);
                    newObj.CategoryHeader = sub.Header;
                    newObj.QuestionText = question.QuestionText.replace("[[[trainee]]]", my.profileVm.userVm.User.FirstName);
                    newObj.QuestionId = question.SurveyQuestionId;
                    newObj.ResponseType = question.ResponseTypeId;
                    newObj.HelpText = question.HelpText;
                    newObj.IsMandatory = question.IsMandatory;
                    newObj.AdditionalNoteRequired = question.AdditionalNoteRequired;

                    var arrayAnswer = [];
                    ko.utils.arrayForEach(question.Answers, function (answer)
                    {
                        arrayAnswer.push({ AnswerId: answer.Id, AnswerText: answer.OptionText });
                    });
                    newObj.Answer = arrayAnswer;
                    questionArray.push(newObj);
                });
            });
            surveyQuestion(questionArray);
        },
            
        wizardOnSubmit = function () {
            var validationMessageArray = [];
            var result = true;
            
              if (my.profileVm.feedbackPost.Rating() == undefined || my.profileVm.feedbackPost.Rating() == 0)
                {
                //  my.profileVm.validationMessage("You need to select a rating to add feedback.");
                validationMessageArray.push("select a rating to add feedback");
                result = false;
                }
               if (my.isNullorEmpty(my.profileVm.feedbackPost.StartDate()) && my.isNullorEmpty(my.profileVm.feedbackPost.EndDate()))
                {
                    validationMessageArray.push(" enter start date & end date ");
                    result = false;
                } else if (my.isNullorEmpty(my.profileVm.feedbackPost.StartDate()))
                {
                    validationMessageArray.push(" enter start date ");
                    result = false;
                } else if (my.isNullorEmpty(my.profileVm.feedbackPost.EndDate()))
                {
                    validationMessageArray.push(" enter end date ");
                    result = false;
                }

                if (my.profileVm.feedbackPost.StartDate() > my.profileVm.feedbackPost.EndDate())
                {
                    validationMessageArray.push(" end date should be greater than start date ");
                    result = false;
                }

              if (!result) return validationMessageArray.join(',');

                my.profileVm.feedbackPost.AddedFor = { UserId: my.profileVm.userId };

                if (typeof (my.profileVm.filter.tempAddedBy()) == 'undefined')
                {
                    my.profileVm.feedbackPost.AddedBy = { UserId: my.profileVm.currentUser.UserId };
                } else
                {
                    my.profileVm.feedbackPost.AddedBy = { UserId: my.profileVm.filter.tempAddedBy().UserId };
                }

                var convertedObject = ko.toJS(my.profileVm.feedbackPost);

                switch (convertedObject.FeedbackType.FeedbackTypeId)
                {
                    
                    case 5:
                        convertedObject.Skill = 0;
                        break;

                }
                
                var objResponse =
                 {
                     AddedBy: my.meta.currentUser,
                     AddedFor: my.profileVm.userVm.User,
                     Response: surveyResponse,
                     Feedback: convertedObject,
                  };
                my.userService.saveWeeklySurveyResponse(objResponse, my.profileVm.addFeedbackCallback);
             return "";
        },
            
        wizardOnStepChanging = function (submittedAnswer, currentIndex)
        {
            try {
                var array = ko.toJS(surveyQuestion());
                var errorMsg = '';
                
                if (array[currentIndex].IsMandatory && array[currentIndex].Answer.length && !submittedAnswer.AnswerId.length)
                {
                    errorMsg += 'Choose some option';
                }
                
                if (array[currentIndex].AdditionalNoteRequired && submittedAnswer.AdditionalNotes.trim().length==0)
                {
                    errorMsg += (errorMsg.length != 0 ? ', ' : '') + 'Add some explanation.';
                }
                
                if (!my.isNullorEmpty(errorMsg.length))
                {
                  //  .

                    var response =
                    {
                        QuestionId: 0,
                        AnswerId: null,
                        AdditionalNotes: ""
                    };

                    var instance = Object.create(response);

                    instance.QuestionId = submittedAnswer.QuestionId;
                    instance.AdditionalNotes = submittedAnswer.AdditionalNotes;
                    
                   
                    ko.utils.arrayForEach(submittedAnswer.AnswerId, function (Id)
                    {
                        instance.AnswerId = Id;
                        
                        if (currentIndex + 1 <= surveyResponse.length)
                        {
                            surveyResponse = surveyResponse.filter(function(element) {
                              return  element.QuestionId != instance.QuestionId;
                            });
                            
                            surveyResponse.splice(currentIndex, 0, instance);
                        }
                        else
                        {
                            surveyResponse.push(instance);
                        }
                       
                    });
                    
                    if (!submittedAnswer.AnswerId.length)
                    {
                        if (currentIndex + 1 <= surveyResponse.length) {
                            surveyResponse.splice(currentIndex, 1);
                            surveyResponse.splice(currentIndex, 0, instance);
                        }
                        else
                        {
                            surveyResponse.push(instance);                            
                        }
                    }
                }
                return errorMsg;
            } 
            catch(ex) {
                
            }
            return "Wizard encounterd some issue ";
        },
        
        wizardOnStepChanged = function (currentIndex)
        {
            if ((currentIndex + 1) == surveyQuestion().length && !isCommentFeedbackModalVisible()) showCommentFeedback();
           // if ((currentIndex + 1) > surveyQuestion().length) loadFeedbackPreview();
        },
            
        loadFeedbackPreview = function (callback)
        {
            var convertedObject = ko.toJS(my.profileVm.feedbackPost);

            switch (convertedObject.FeedbackType.FeedbackTypeId)
            {

                case 5:
                    convertedObject.Skill = 0;
                    break;

            }

            var objResponse =
             {
                 AddedBy: my.meta.currentUser,
                 AddedFor: my.profileVm.userVm.User,
                 Response: surveyResponse,
                 Feedback: convertedObject,
             };

             my.userService.fetchWeeklyFeedbackPreview(objResponse).done(function (response) {
                 callback(response);
            });
             
        },
        
            
        toggleCollapsedPanel = function () {
            my.profileVm.isCommentCollapsed(!my.profileVm.isCommentCollapsed());
        };

        return {
            userId: userId,
            getUserCallback: getUserCallback,
            userVm: userVm,
            getUser: getUser,
            fullName: fullName,
            photoUrl: photoUrl,
            showTimeline: showTimeline,
            toggleTimeline: toggleTimeline,
            feedbackPost: feedbackPost,
            selectedSkill: selectedSkill,
            selectedProject: selectedProject,
            setRating: setRating,
            addFeedback: addFeedback,
            addFeedbackCallback: addFeedbackCallback,
            validatePost: validatePost,
            validationMessage: validationMessage,
            currentUser: currentUser,
            getCurrentUser: getCurrentUser,
            getCurrentUserCallback: getCurrentUserCallback,
            controls: controls,
            filter: filter,
            applyFilter: applyFilter,
            applyFilterCallback: applyFilterCallback,
            recentCodeReviewFeedback: recentCodeReviewFeedback,
            getCountForFeedback: getCountForFeedback,
            recentWeeklyFeedback: recentWeeklyFeedback,
            tempAllTrainer: tempAllTrainer, //temp feature,
            plotFilter: plotFilter,
            loadPlotData: loadPlotData,
            showCommentFeedback: showCommentFeedback,
            isCommentFeedbackModalVisible: isCommentFeedbackModalVisible,
            closeCommentFeedbackModal: closeCommentFeedbackModal,
            commentFeedbacks: commentFeedbacks,
            loadcommentFeedbacks: loadcommentFeedbacks,
            loadCommentFeedbacksCallback: loadCommentFeedbacksCallback,
            isCommentCollapsed: isCommentCollapsed,
            toggleCollapsedPanel: toggleCollapsedPanel,
            loadFeedbackWithThread: loadFeedbackWithThread,
            initializeSurveyQuestion: initializeSurveyQuestion,
            surveyQuestion: surveyQuestion,
            wizardOnStepChanging: wizardOnStepChanging,
            wizardOnStepChanged: wizardOnStepChanged,
            wizardOnSubmit: wizardOnSubmit,
            loadFeedbackPreview: loadFeedbackPreview,
            trainorSynopsis: trainorSynopsis
                
    };
    }();

    my.profileVm.getCurrentUser();
    my.profileVm.getUser();
    
    my.profileVm.feedbackPost.FeedbackType.subscribe(function (selected)
    {
        if (my.profileVm.feedbackPost.FeedbackType().FeedbackTypeId == 5 && !my.profileVm.surveyQuestion().length)
        {
            my.userService.fetchSurveyQuestionForTeam(my.profileVm.userVm.User.UserId, my.profileVm.feedbackPost.StartDate(), my.profileVm.feedbackPost.EndDate(), my.profileVm.initializeSurveyQuestion);
        }
        
    }, null, "change");
    
    my.profileVm.feedbackPost.selectedOption.subscribe(function (selected) {

        var array = ko.utils.arrayFilter(my.profileVm.userVm.FeedbackTypes, function (data) {
            return data.FeedbackTypeId == selected;
        });
        
        my.profileVm.feedbackPost.FeedbackType(array[0]);
    }, null, "change");
});


