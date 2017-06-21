﻿$(document).ready(function () {
    my.profileVm = function () {
        var userId = my.queryParams["userId"],
            queryStringFeedbackId = my.queryParams["feedbackId"],
            queryStringPostId = my.queryParams["postId"],
            showTimeline = ko.observable(false),
            selectedSkill = ko.observable(),
            selectedProject = ko.observable(),
            validationMessage = ko.observable(),
            tempAllTrainer = ko.observable(), // remove this once temporary feature use end
            commentFeedbacks = ko.observableArray([]),
            isCommentFeedbackModalVisible = ko.observable(false),
            trainorSynopsis = ko.observable(),
            feedbackTypes =
            {
                NewFeedback: ko.observableArray([{ FeedbackTypeId: 1, Description: "Comment" },
                                                { FeedbackTypeId: 2, Description: "Skill" },
                                                { FeedbackTypeId: 3, Description: "Assignment" },
                                                { FeedbackTypeId: 4, Description: "Code Review" },
                                                { FeedbackTypeId: 5, Description: "Weekly Feedback" },
                                                { FeedbackTypeId: 7, Description: "Random Review" }]),

                Filter: ko.observableArray([{ FeedbackTypeId: 1, Description: "Comment" },
                                            { FeedbackTypeId: 2, Description: "Skill" },
                                            { FeedbackTypeId: 3, Description: "Assignment" },
                                            { FeedbackTypeId: 4, Description: "Code Review" },
                                            { FeedbackTypeId: 5, Description: "Weekly Feedback" },
                                            { FeedbackTypeId: 6, Description: "Course Feedback" },
                                            { FeedbackTypeId: 7, Description: "Random Review" }])
            },
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
            selectedOption: ko.observable(1)
        },
        surveyQuestion = ko.observableArray([]),
        surveyResponse = [],
        setRating = function (rating) {
            my.profileVm.feedbackPost.Rating(rating);
        },
        toggleTimeline = function () {
            my.profileVm.showTimeline = !my.profileVm.showTimeline;
        },
        userVm = {},
        fullName = function (item) {
            return item.FirstName + " " + item.LastName;
        },
        photoUrl = function (item) {
            return my.rootUrl + "/Uploads/ProfilePicture/" + item.ProfilePictureName;
        },
        getUserCallback = function (jsonData) {
            if (my.profileVm.userId == my.meta.currentUser.UserId) {
                jsonData.User = my.meta.currentUser;
            }

            jsonData.User.FullName = my.profileVm.fullName(jsonData.User);
            jsonData.User.PhotoUrl = my.profileVm.photoUrl(jsonData.User);
            $.each(jsonData.Feedbacks, function (arrayId, feedback) {
                feedback.AddedBy.UserImageUrl = my.rootUrl + "/Uploads/ProfilePicture/" + feedback.AddedBy.ProfilePictureName;
            });
            jsonData.Feedbacks = ko.observableArray(jsonData.Feedbacks);
            my.profileVm.trainorSynopsis(jsonData.TrainorSynopsis);
            //my.profileVm.tempAllTrainer(jsonData.AllTrainer); // Temp Feature
            my.meta.loadedAllActiveUsersPromise().done(function () {
                my.profileVm.tempAllTrainer(my.meta.allMentor());
            });
            my.profileVm.plotFilter.StartDate(moment(jsonData.User.DateAddedToSystem).format('MM/DD/YYYY'));
            my.profileVm.plotFilter.TraineeId = jsonData.User.UserId;
            my.profileVm.userVm = jsonData;
            my.profileVm.userVm.AllSkills = ko.observableArray([]);
            ko.applyBindings([my.profileVm, my.discussionForumVm]);
            my.profileVm.feedbackPost.Rating(0);
            my.profileVm.feedbackPost.selectedOption(1);

            if (!my.isNullorEmpty(queryStringFeedbackId)) {
                loadFeedbackWithThread(queryStringFeedbackId);
            }
            if (!my.isNullorEmpty(queryStringPostId)) {
                my.discussionThreadsVm.loadDiscussionDialog(queryStringPostId);
            }

            if(!my.isNullorEmpty(jsonData.SavedCodeReview))
            {
                savedCodeReviewDataForTrainee(jsonData.SavedCodeReview);
            }

        },

        savedCodeReviewDataForTrainee = function (jsonData) {
            my.profileVm.userVm.SavedCodeReview = jsonData;
            codeReviewPreviewHtml(jsonData.CodeReviewPreviewHtml);
            feedbackPost.selectedOption(4);
          //  codeReviewSelectedTab(1);           

            codeReviewDetails.Id(jsonData.Id);
            codeReviewDetails.Title(jsonData.Title);
            codeReviewDetails.Description(jsonData.Description);

            codeReviewDetails.Skills([]);
            ko.utils.arrayForEach(jsonData.Tags, function (tag) {
                if (tag.Skill.SkillId != 0) {
                    codeReviewDetails.Skills.push(tag.Skill);
                }
            });
        },

        loadPlotData = function () {
            if (typeof (my.chartVm) !== 'undefined') {
                my.chartVm.loadUserPlotData(plotFilter.TraineeId, plotFilter.StartDate(), plotFilter.EndDate(), plotFilter.FeedbackType(), typeof (plotFilter.Trainer()) == 'undefined' ? undefined : plotFilter.Trainer().UserId);
            }
        },
        getUser = function () {
            my.userService.getUserProfileVm(my.profileVm.userId, my.profileVm.getUserCallback);
        },
        validatePost = function () {
            var result = true;

            var validationMessageArray = [];

            if ((my.profileVm.feedbackPost.FeedbackType().FeedbackTypeId != 5 && my.profileVm.feedbackPost.FeedbackType().FeedbackTypeId != 4) && (my.profileVm.feedbackPost.FeedbackText() == undefined ||
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
                    (typeof (my.profileVm.feedbackPost.Title()) == 'undefined'
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
        addFeedbackCallback = function (jsonData) {
            if (jsonData === 'false') {
                $.alert({
                    title: 'Failed to Add Feedback!',
                    columnClass: 'col-md-6 col-md-offset-3 col-sm-8 col-sm-offset-2 col-xs-10',
                    useBootstrap: true,
                    content: 'Unable to Add Feedback, Try again after some time!',
                });
            }
            else
                window.location = $(location).attr('origin') + $(location).attr('pathname') + "?userId=" + userId;
        },
        addFeedback = function () {
            my.profileVm.validationMessage("");

            if (my.profileVm.validatePost()) {
                my.profileVm.feedbackPost.AddedFor = { UserId: my.profileVm.userId };

                if (typeof (my.profileVm.filter.tempAddedBy()) == 'undefined') {
                    my.profileVm.feedbackPost.AddedBy = { UserId: my.profileVm.currentUser.UserId };
                } else {
                    my.profileVm.feedbackPost.AddedBy = { UserId: my.profileVm.filter.tempAddedBy().UserId };
                }

                var convertedObject = ko.toJS(my.profileVm.feedbackPost);

                switch (convertedObject.FeedbackType.FeedbackTypeId) {
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
                    case 7:
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
        getCurrentUserCallback = function (user) {
            my.profileVm.currentUser = user;
            my.profileVm.currentUser.avatarUrl = my.profileVm.photoUrl(user);
        },
        getCurrentUser = function () {
            my.meta.loadedCurrentUserPromise().done(function () {
                my.profileVm.getCurrentUserCallback(my.meta.currentUser);
            });
            //my.userService.getCurrentUser(my.profileVm.getCurrentUserCallback);
        },
        applyFilter = function () {
            var filtertype = typeof (my.profileVm.filter.filterFeedback()) == 'undefined' ? 0 : (my.profileVm.filter.filterFeedback().FeedbackTypeId);
            my.userService.getFeedbackonAppliedFilter(my.profileVm.filter.selectedPageSize(), filtertype, my.profileVm.userId, null, null, my.profileVm.applyFilterCallback);
        },
        applyFilterCallback = function (feedbacks) {
            my.profileVm.userVm.Feedbacks([]);
            $.each(feedbacks, function (key) {
                feedbacks[key].AddedBy.UserImageUrl = my.rootUrl + "/Uploads/ProfilePicture/" + feedbacks[key].AddedBy.ProfilePictureName;
                my.profileVm.userVm.Feedbacks.push(feedbacks[key]);
            });
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

        ko.utils.arrayForEach(feedbacks, function (item) {
            my.profileVm.commentFeedbacks.push(item);
        });
    },
    isCommentCollapsed = ko.observable(false),
    loadFeedbackWithThread = function (feedbackId) {
        var filteredFeedback = ko.utils.arrayFilter(my.profileVm.userVm.Feedbacks(), function (item) {
            return item.FeedbackId == feedbackId;
        });

        if (filteredFeedback.length > 0 && my.profileVm.userVm.User.IsTrainee) {
            my.feedbackThreadsVm.loadFeedbackDailog(feedbackId, filteredFeedback[0]);
        }
        else {
            my.feedbackThreadsVm.loadFeedbackDailog(feedbackId);
        }
    },

    initializeSurveyQuestion = function (surveyQuestionJson) {
        if (!surveyQuestionJson.IsCodeReviewedForTrainee) {
            $.confirm({
                title: 'Missing Code Review Feedback!',
                content: 'Weekly survey automatically captures code review for the week, but no CR added for ' + my.profileVm.userVm.User.FirstName + '  in between ' + moment(my.profileVm.feedbackPost.StartDate()).format("dddd, MMMM Do YYYY") + ' and ' + moment(my.profileVm.feedbackPost.EndDate()).format("dddd, MMMM Do YYYY") + '.' + '</br><label>Do you want to add CR?</label>',
                columnClass: 'col-md-6 col-md-offset-3 col-sm-8 col-sm-offset-2 col-xs-10',
                useBootstrap: true,
                buttons: {
                    confirm:
                    {
                        text: 'Yes, Add CR',
                        btnClass: 'btn-primary btn-success',
                        action: function () {
                            my.profileVm.feedbackPost.selectedOption(4);
                            return;
                        }
                    },
                    cancel:
                    {
                        text: 'No, Continue with WF',
                        btnClass: 'btn-primary btn-warning',
                        action: function () {
                            bindSurveyQuestion(surveyQuestionJson);
                        }
                    }
                }
            });
        } else {
            bindSurveyQuestion(surveyQuestionJson);
        }
    },

    bindSurveyQuestion = function (surveyQuestionJson) {

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

        ko.utils.arrayForEach(surveyQuestionJson.Survey.SurveySubSections, function (sub) {
            ko.utils.arrayForEach(sub.Questions, function (question) {
                var newObj = Object.create(questionObject);
                newObj.CategoryHeader = sub.Header;
                newObj.QuestionText = question.QuestionText.replace("[[[trainee]]]", my.profileVm.userVm.User.FirstName);
                newObj.QuestionId = question.SurveyQuestionId;
                newObj.ResponseType = question.ResponseTypeId;
                newObj.HelpText = question.HelpText;
                newObj.IsMandatory = question.IsMandatory;
                newObj.AdditionalNoteRequired = question.AdditionalNoteRequired;

                var arrayAnswer = [];
                ko.utils.arrayForEach(question.Answers, function (answer) {
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

        if (my.profileVm.feedbackPost.Rating() == undefined || my.profileVm.feedbackPost.Rating() == 0) {
            //  my.profileVm.validationMessage("You need to select a rating to add feedback.");
            validationMessageArray.push("select a rating to add feedback");
            result = false;
        }
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

        if (!result) return validationMessageArray.join(',');

        my.profileVm.feedbackPost.AddedFor = { UserId: my.profileVm.userId };

        if (typeof (my.profileVm.filter.tempAddedBy()) == 'undefined') {
            my.profileVm.feedbackPost.AddedBy = { UserId: my.profileVm.currentUser.UserId };
        } else {
            my.profileVm.feedbackPost.AddedBy = { UserId: my.profileVm.filter.tempAddedBy().UserId };
        }

        var convertedObject = ko.toJS(my.profileVm.feedbackPost);

        switch (convertedObject.FeedbackType.FeedbackTypeId) {

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

    wizardOnStepChanging = function (submittedAnswer, currentIndex) {
        try {
            var array = ko.toJS(surveyQuestion());
            var errorMsg = '';

            if (array[currentIndex].IsMandatory && array[currentIndex].Answer.length && !submittedAnswer.AnswerId.length) {
                errorMsg += 'Choose some option';
            }

            if (array[currentIndex].AdditionalNoteRequired && submittedAnswer.AdditionalNotes.trim().length == 0) {
                errorMsg += (errorMsg.length != 0 ? ', ' : '') + 'Add some explanation.';
            }

            if (!my.isNullorEmpty(errorMsg.length)) {
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


                ko.utils.arrayForEach(submittedAnswer.AnswerId, function (Id) {
                    instance.AnswerId = Id;

                    if (currentIndex + 1 <= surveyResponse.length) {
                        surveyResponse = surveyResponse.filter(function (element) {
                            return element.QuestionId != instance.QuestionId;
                        });

                        surveyResponse.splice(currentIndex, 0, instance);
                    }
                    else {
                        surveyResponse.push(instance);
                    }

                });

                if (!submittedAnswer.AnswerId.length) {
                    if (currentIndex + 1 <= surveyResponse.length) {
                        surveyResponse.splice(currentIndex, 1);
                        surveyResponse.splice(currentIndex, 0, instance);
                    }
                    else {
                        surveyResponse.push(instance);
                    }
                }
            }
            return errorMsg;
        }
        catch (ex) {

        }
        return "Wizard encounterd some issue ";
    },

    wizardOnStepChanged = function (currentIndex) {
        if ((currentIndex + 1) == surveyQuestion().length && !isCommentFeedbackModalVisible()) showCommentFeedback();
        // if ((currentIndex + 1) > surveyQuestion().length) loadFeedbackPreview();
    },

    loadFeedbackPreview = function (callback) {
        var convertedObject = ko.toJS(my.profileVm.feedbackPost);

        switch (convertedObject.FeedbackType.FeedbackTypeId) {

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

        var navigateToCourse = function (courseId) {
            if (my.profileVm.currentUser.UserId == my.profileVm.userVm.User.UserId) {
                window.open(my.rootUrl + '/LearningPath/Course?courseId=' + courseId, '_blank');
            }
            else {
                window.open(my.rootUrl + '/LearningPath/Course?courseId=' + courseId + '&traineeId=' + my.profileVm.userVm.User.UserId, '_blank');
            }
        };

        var codeReviewPointsTypes = [{
            Id: 1,
            Title: "Exceptional",
            Description: "Point better than expected"
        },
            {
                Id: 2,
                Title: "Good",
                Description: "Point matching the expectations"
            },
            {
                Id: 3,
                Title: "Corrected",
                Description: "Point corrected from previous review"
            },
             {
                 Id: 4,
                 Title: "Bad",
                 Description: "Poorly written, Could have been better"
             }, {
                 Id: 5,
                 Title: "Critical",
                 Description: "Need immediate attentions"
             }, {
                 Id: 6,
                 Title: "Suggestion",
                 Description: "Suggestions"
             }
        ];
        var reviewPointsDetails = {
            Id: ko.observable(0),
            Title: ko.observable(""),
            Deleted : ko.observable(false),
            Description: ko.observable(""),
            Rating: ko.observable(0),
            ErrorMessage: ko.observable(""),
            EditMode:ko.observable(false)
        };

        var codeReviewDetails = {
            Id : ko.observable(0),
            Edited: ko.observable(false),
            Deleted: ko.observable(false),
            Title: ko.observable(""),
            Description: ko.observable(""),
            Skills: ko.observableArray([]),
            ErrorMessage: ko.observable(""),
            AddedBy: 0,
            AddedFor: 0,
        };

        var codeReviewSelectedTab = ko.observable(0);
        var codeReviewSelectedTag = ko.observable(0);

        var setReviewPointRating = function (ratingId) {
            reviewPointsDetails.Rating(ratingId);
        };

         var setSelectedTagId =function(tagId)
        {
            if (typeof (tagId) == 'undefined')
            {
                codeReviewSelectedTag(0);
                return;
            }
            codeReviewSelectedTag(tagId);
        }


        var savePointsToCodeReview = function (){           
            var message = validateTagsPoints();

            if (message.length) {
                reviewPointsDetails.ErrorMessage(message.join());
                reviewPointsDetails.Rating(0);
                return;
            }

            var codeReviewPoints = {
                PointId: reviewPointsDetails.Id(),
                CodeReviewTagId: codeReviewSelectedTag(),
                CodeReviewMetadataId : codeReviewDetails.Id(),
                Rating :reviewPointsDetails.Rating(),
                Title : reviewPointsDetails.Title(),
                Description : reviewPointsDetails.Description(),
                IsDeleted: reviewPointsDetails.Deleted()
            }

            PostDataUsingPromise(function () { flushReviewPointsTab(); return my.userService.addUpdateTagPointsWithPromise(codeReviewPoints); },
               saveTagCallback, function () { console.log("Error Adding Points") });
           
        };

        var saveTagCallback = function(data)
        {
            var currentSelection = codeReviewSelectedTag();
            savedCodeReviewDataForTrainee(data);
            codeReviewSelectedTag(currentSelection);
        }

        var validateTagsPoints = function () {
            var message = [];

            if (my.isNullorEmpty(reviewPointsDetails.Title())) {
                message.push("Add Review Points");
            }

            if (reviewPointsDetails.Rating()== 0) {
                message.push("Select Point's Rating");
            }
            return message;
        };

        var validateCodeReviewPoints =function()
        {
            var message = [];

            if(my.isNullorEmpty(codeReviewDetails.Title()))
            {
                message.push("Title");
            }

            if (my.isNullorEmpty(codeReviewDetails.Description())) {
                message.push("Description");
            }

            if (codeReviewDetails.Skills().length == 0) {
                message.push("At least one tag");
            }
            return message;
        }

        var saveCodeReviewData = function ()
        {
            
            var message = validateCodeReviewPoints();

            if (message.length) {
                codeReviewDetails.ErrorMessage('Add ' + message.join(", "));
                return;
            }

            var codeReviewMetaData =
                {
                    Id : codeReviewDetails.Id(),
                    Description: codeReviewDetails.Description(),
                    Title: codeReviewDetails.Title(),
                    IsDeleted: codeReviewDetails.Deleted(),
                    AddedFor : { UserId: my.profileVm.userId },
                    Tags : []
                }

            ko.utils.arrayForEach(codeReviewDetails.Skills() , function (tag) 
            {
                codeReviewMetaData.Tags.push({ Skill: tag });
            });

            updateSelectedCodereviewTag(1);

            if (codeReviewDetails.Id() == 0 || codeReviewDetails.Edited())
            {
                PostDataUsingPromise(function () { return my.userService.addUpdateCodeReviewDetailsWithPromise(codeReviewMetaData); },
                saveCodeReviewCallback, function () { console.log("Error Adding Points") });
            }
           
        };

        var saveCodeReviewCallback = function (data) {
            codeReviewDetails.Id(data.Id);
            codeReviewDetails.Edited(false);
            savedCodeReviewDataForTrainee(data);
            codeReviewSelectedTab(1);
            toggleTab(1);
        };

        var PostDataUsingPromise = function (serviceMethod, callback, failureCallback) {
            var deferredObject = $.Deferred();

            serviceMethod().done(function (data) {
                callback(data);
                deferredObject.resolve(data);
            }).fail(function () {
                failureCallback();
            });
        };

        var updateSelectedCodereviewTag = function (tagId) {
            codeReviewSelectedTab(tagId);
            flushReviewPointsTab();
            if(tagId==2)
            {
                getCodeReviewPreview();
            }
        };

        var getCodeReviewPreview = function (isFeedback) {

            if (codeReviewDetails.Id() <= 0) return;
            my.userService.getCodeReviewPreview(codeReviewDetails.Id(),isFeedback, getCodeReviewPreviewCallback)
        };

        var getCodeReviewPreviewCallback = function (data) {
            codeReviewPreviewHtml(data);
        };

        var codeReviewPreviewHtml = ko.observable("");

        var removeCodeReviewTagAndRefresh = function () {

        };

        var discardCodeReview = function () {
          
            $.confirm({
                title: 'Discard saved Review Details?',
                content: 'Do you want discard this review draft and start over again?',
                columnClass: 'col-md-6 col-md-offset-3 col-sm-8 col-sm-offset-2 col-xs-10',
                useBootstrap: true,
                buttons: {
                    confirm:
                    {
                        text: 'Yes, Discard this Review',
                        btnClass: 'btn-primary btn-danger',
                        action: function () {
                            my.userService.discardCodeReviewFeedback(codeReviewDetails.Id(), discardCodeReviewCallback);
                        }
                    },
                    cancel:
                    {
                        text: 'No, keep editing',
                        btnClass: 'btn-primary btn-warning',
                        action: function () {
                           
                        }
                    }
                }
            });            
        }

        var discardCodeReviewCallback = function () {
            codeReviewSelectedTab(0);
            codeReviewSelectedTag(0);
            codeReviewPreviewHtml("");
            flushCodeReviewDetails();
            flushReviewPointsTab();
            toggleTab(0);
            toggleCodeReviewModal(false);
        }

        var submitCodeReview = function () {
            if(validatePost())
            {
                var codeReview = {
                    Id : codeReviewDetails.Id(),
                    Rating :feedbackPost.Rating(),
                    AddedFor :{ UserId: my.profileVm.userId },
                    AddedBy: { UserId: my.profileVm.currentUser.UserId }              
                };

                my.userService.submitCodeReviewFeedback(codeReview, addFeedbackCallback);
            }
        };

        var flushCodeReviewDetails = function () {
            codeReviewDetails.Id (0),
            codeReviewDetails.Edited(false),
            codeReviewDetails.Deleted(false),
            codeReviewDetails.Title(""),
            codeReviewDetails.Description(""),
            codeReviewDetails.Skills([]),
            codeReviewDetails.ErrorMessage(""),
            codeReviewDetails.AddedBy = 0,
            codeReviewDetails.AddedFor = 0
        };

        var flushReviewPointsTab = function () {          
            reviewPointsDetails.Id(0);
            reviewPointsDetails.Title("");
            reviewPointsDetails.Deleted(false);
            reviewPointsDetails.Description("");
            reviewPointsDetails.Rating(0);
            reviewPointsDetails.ErrorMessage("");
            reviewPointsDetails.EditMode(false);
        };

        var filterKeyWord = ko.observable("");
        var filteredTag = ko.observableArray([]);

        var filterTag = function (data,event) {
            if (my.profileVm.userVm.AllSkills().length == 0 || my.isNullorEmpty(my.profileVm.filterKeyWord().trim())) {
                my.profileVm.filteredTag([]);
                return true;
            }

            var flattenedAllSkillArray = ko.utils.arrayMap(my.profileVm.userVm.AllSkills(), function (item) {
                return item.Name.toUpperCase();
            });
            flattenedAllSkillArray = flattenedAllSkillArray.sort();

            var flattenedSelectedSkillArray = ko.utils.arrayMap(codeReviewDetails.Skills(), function (item) {
                return item.Name.toUpperCase();
            });
            flattenedSelectedSkillArray = flattenedSelectedSkillArray.sort();

            var differences = ko.utils.compareArrays(flattenedAllSkillArray, flattenedSelectedSkillArray);

            var filteredTag = ko.utils.arrayFilter(differences, function (item) {
                return item.status === "deleted" ;
            });

            my.profileVm.filteredTag([]);
            ko.utils.arrayForEach(filteredTag, function (item) {
                var tags = ko.utils.arrayFilter(my.profileVm.userVm.AllSkills(), function (allSkill) {
                    return allSkill.Name.toUpperCase() == item.value && item.value.includes(my.profileVm.filterKeyWord().trim().toUpperCase());
                });
                if (tags.length>0)
                my.profileVm.filteredTag.push(tags[0]);
            });

            // Tabbing Support 
            if (event.keyCode == 9) {
                addTagToCodeReviewDetails(my.profileVm.filteredTag[0].SkillId);
            };
            return true;
        };

        var addTagToCodeReviewDetails = function (skillId) {
            var filteredTag = ko.utils.arrayFilter(my.profileVm.userVm.AllSkills(), function (item) {
                return item.SkillId == skillId ;
            });

            if(filteredTag.length>0)
            {
                codeReviewDetails.Skills.push(filteredTag[0]);
            }
            my.profileVm.filterKeyWord("");
            my.profileVm.filteredTag([]);
            codeReviewDetails.Edited(true);
        };

        var addCategoryCallback = function (data) {
            if(data == null)
            {
                console.log("Failed Adding Skill");
                return;
            }
            my.profileVm.userVm.AllSkills(data);

            var filteredTag = ko.utils.arrayFilter(my.profileVm.userVm.AllSkills(), function (item) {
                return item.Name == filterKeyWord();
            });

            if (filteredTag.length > 0) {
                codeReviewDetails.Skills.push(filteredTag[0]);
            }
            my.profileVm.filterKeyWord("");
            my.profileVm.filteredTag([]);

        };

        var addCategory = function () {
            if (!my.isNullorEmpty(filterKeyWord().trim()))
            {
                my.userService.addCategory({ Name: filterKeyWord().trim(), AddedBy: my.meta.currentUser.UserId },
               addCategoryCallback);
            }          
        };

        var isCodeReviewModalOpen = ko.observable(false);

        var toggleCodeReviewModal = function (openModal) {
            isCodeReviewModalOpen(openModal);

            if (my.profileVm.codeReviewDetails.Id() != 0 )
            {
                if (openModal)
                {
                    $('#divCodeReviewPointsCollapsable').addClass('show');
                    $('#divCodeReviewPointsCollapsableHeader').removeClass('collapsed');

                    $('#divCodeReviewSummaryCollapsable').removeClass('show');
                    $('#divCodeReviewSummaryCollapsableHeader').addClass('collapsed');
                }
                return;
            }
            $('#divCodeReviewSummaryCollapsable').addClass('show');
            $('#divCodeReviewSummaryCollapsableHeader').removeClass('collapsed');
        };

        var toggleTab = function (loadSelection) {
            if (loadSelection == 1)
            {
                $('#divCodeReviewSummaryCollapsable').removeClass('show');
                $('#divCodeReviewSummaryCollapsableHeader').addClass('collapsed');

                $('#divCodeReviewPointsCollapsable').addClass('show');
                $('#divCodeReviewPointsCollapsableHeader').removeClass('collapsed');
               
                return false;
            }
           
            $('#divCodeReviewPointsCollapsable').removeClass('show');
            $('#divCodeReviewPointsCollapsableHeader').addClass('collapsed');

            $('#divCodeReviewSummaryCollapsable').addClass('show');
            $('#divCodeReviewSummaryCollapsableHeader').removeClass('collapsed');
           
          
            return false;
        }

        var removeCodeReviewTagAndRefresh = function (codeReviewTagId, skillId) {

            var tag =  ko.utils.arrayFilter(my.profileVm.userVm.AllSkills(), function (allSkill) {
                return allSkill.SkillId == skillId;
                });


            $.confirm({
                title: 'Delete Tag!!',
                content: 'Do you want to delete  <span class="danger">' + tag[0].Name + ' </span>  tag and all review points?',
                columnClass: 'col-md-6 col-md-offset-3 col-sm-8 col-sm-offset-2 col-xs-10',
                useBootstrap: true,
                buttons: {
                    confirm:
                    {
                        text: 'Yes, Delete this Tag',
                        btnClass: 'btn-primary btn-danger',
                        action: function () {
                            my.userService.discardTagFromCodeReviewFeedback(codeReviewDetails.Id(),codeReviewTagId, discardTagFromCodeReviewFeedbackCallback);
                        }
                    },
                    cancel:
                    {
                        text: 'No, keep editing',
                        btnClass: 'btn-primary btn-warning',
                        action: function () {

                        }
                    }
                }
            });
        }

        var discardTagFromCodeReviewFeedbackCallback = function(data) {
            savedCodeReviewDataForTrainee(data);
        }

        var editCodeReviewPoint = function (codereviewTagId,skillId, codeReviewPointId) {

            var pointData = filterReviewPoint(codereviewTagId, codeReviewPointId);

            codeReviewSelectedTag(skillId);

            reviewPointsDetails.Id(codeReviewPointId);
            reviewPointsDetails.EditMode(true);
            reviewPointsDetails.Title(pointData.Title),
            reviewPointsDetails.Description(pointData.Description),
            reviewPointsDetails.Deleted(false);
            setReviewPointRating(pointData.Rating);

        };

        var removeCodeReviewPoint = function (codereviewTagId, skillId, codeReviewPointId) {

            var pointData = filterReviewPoint(codereviewTagId, codeReviewPointId);

            codeReviewSelectedTag(skillId);

            reviewPointsDetails.Id(codeReviewPointId);
            reviewPointsDetails.EditMode(true);
            reviewPointsDetails.Title(pointData.Title);
            reviewPointsDetails.Description(pointData.Description);
            reviewPointsDetails.Deleted(true);
            setReviewPointRating(pointData.Rating);

            $.confirm({
                title: 'Delete Review Point!!',
                content: 'Do you want to delete  review point',
                columnClass: 'col-md-6 col-md-offset-3 col-sm-8 col-sm-offset-2 col-xs-10',
                useBootstrap: true,
                buttons: {
                    confirm:
                    {
                        text: 'Yes, Delete this Point',
                        btnClass: 'btn-primary btn-danger',
                        action: function () {
                            savePointsToCodeReview();
                        }
                    },
                    cancel:
                    {
                        text: 'No, keep editing',
                        btnClass: 'btn-primary btn-warning',
                        action: function () {

                        }
                    }
                }
            });

        }

        var filterReviewPoint = function (codereviewTagId, codeReviewPointId) {
            var filteredTag = ko.utils.arrayFilter(my.profileVm.userVm.SavedCodeReview.Tags, function (tag) {
                return tag.CodeReviewTagId == codereviewTagId;
            });

            var filteredPoint = ko.utils.arrayFilter(filteredTag[0].ReviewPoints, function (point) {
                return point.PointId == codeReviewPointId;
            });

            return filteredPoint[0];

        };

        var updateReviewPointData = function () {
            $.confirm({
                title: 'Update Review Point!!',
                content: 'Do you want to update review point?, this will update your existing data',
                columnClass: 'col-md-6 col-md-offset-3 col-sm-8 col-sm-offset-2 col-xs-10',
                useBootstrap: true,
                buttons: {
                    confirm:
                    {
                        text: 'Yes, Update ',
                        btnClass: 'btn-primary btn-danger',
                        action: function () {
                            savePointsToCodeReview();
                        }
                    },
                    cancel:
                    {
                        text: 'No',
                        btnClass: 'btn-primary btn-warning',
                        action: function () {

                        }
                    }
                }
            });
        }
      
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
            trainorSynopsis: trainorSynopsis,
            navigateToCourse: navigateToCourse,
            feedbackTypes: feedbackTypes,
            codeReviewDetails: codeReviewDetails,
            reviewPointsDetails: reviewPointsDetails,
            codeReviewPointsTypes: codeReviewPointsTypes,
            setReviewPointRating: setReviewPointRating,
            setSelectedTagId:setSelectedTagId,
            saveCodeReviewData: saveCodeReviewData,
            savePointsToCodeReview:savePointsToCodeReview,
            updateSelectedCodereviewTag: updateSelectedCodereviewTag,
            codeReviewSelectedTab: codeReviewSelectedTab,
            codeReviewSelectedTag: codeReviewSelectedTag,
            codeReviewPreviewHtml: codeReviewPreviewHtml,
            submitCodeReview: submitCodeReview,
            discardCodeReview: discardCodeReview,
            filteredTag:filteredTag,
            filterTag: filterTag,
            filterKeyWord: filterKeyWord,
            addTagToCodeReviewDetails: addTagToCodeReviewDetails,
            addCategory: addCategory,
            isCodeReviewModalOpen: isCodeReviewModalOpen,
            getCodeReviewPreview: getCodeReviewPreview,
            toggleCodeReviewModal: toggleCodeReviewModal,
            removeCodeReviewTagAndRefresh:removeCodeReviewTagAndRefresh,
            //setSelectedTab: setSelectedTab,
            toggleTab: toggleTab,
            removeCodeReviewPoint: removeCodeReviewPoint,
            editCodeReviewPoint: editCodeReviewPoint,
            updateReviewPointData: updateReviewPointData
        };
    }();

    my.profileVm.feedbackPost.FeedbackType(my.profileVm.feedbackTypes.NewFeedback()[0]);
    my.profileVm.getCurrentUser();
    my.profileVm.getUser();

    my.profileVm.feedbackPost.FeedbackType.subscribe(function (selected) {
        if (my.profileVm.feedbackPost.FeedbackType().FeedbackTypeId == 5 && !my.profileVm.surveyQuestion().length) {
            my.userService.fetchSurveyQuestionForTeam(my.profileVm.userVm.User.UserId, my.profileVm.feedbackPost.StartDate(), my.profileVm.feedbackPost.EndDate(), my.profileVm.initializeSurveyQuestion);
        }

        else if ((my.profileVm.feedbackPost.FeedbackType().FeedbackTypeId == 2 || my.profileVm.feedbackPost.FeedbackType().FeedbackTypeId == 4) && my.profileVm.userVm.AllSkills().length == 0) {
            my.userService.getAllSkills().done(function (data) {
                my.profileVm.userVm.AllSkills(data);
               // my.profileVm.filteredTag(data);
                my.toggleLoader();
            });
        }
    }, null, "change");


    my.profileVm.codeReviewDetails.Title.subscribe(function () {
        if( my.profileVm.codeReviewDetails.Id()>0)
        {
            my.profileVm.codeReviewDetails.Edited(true);
        }
    });

    my.profileVm.codeReviewDetails.Description.subscribe(function () {
        if (my.profileVm.codeReviewDetails.Id() > 0) {
            my.profileVm.codeReviewDetails.Edited(true);
        }
    });

    my.profileVm.feedbackPost.selectedOption.subscribe(function (selected) {

        var array = ko.utils.arrayFilter(my.profileVm.feedbackTypes.NewFeedback(), function (data) {
            return data.FeedbackTypeId == selected;
        });

        my.profileVm.feedbackPost.FeedbackType(array[0]);
    }, null, "change");

    my.profileVm.reviewPointsDetails.Rating.subscribe(function (selectedRating) {
        if (selectedRating == 0 || my.profileVm.reviewPointsDetails.EditMode()) return;
        my.profileVm.savePointsToCodeReview();
    });

    my.profileVm.isCodeReviewModalOpen.subscribe(function (isOpen) {
        my.profileVm.getCodeReviewPreview(!isOpen);
    });
});
