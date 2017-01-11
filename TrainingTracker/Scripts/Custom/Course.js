$(document).ready(function () {

    my.courseVm = function () {
        var courseInfo = {
            CourseId: my.queryParams["courseId"],
            TraineeId: my.queryParams["traineeId"],
            Course: ko.observable(),
            Icon: my.rootUrl + "/Uploads/CourseIcon/DefaultCourse.jpg",
        },
        showSelectedTopic = function (topic) {
            if (!my.isNullorEmpty(topic)) {
                my.courseVm.selectedTopic.Id(topic.Id);
                my.courseVm.selectedTopic.Name(topic.Name);
                my.courseVm.selectedTopic.Description(topic.Description);
                my.courseVm.selectedTopic.SubtopicContents([]);
                my.courseVm.selectedTopic.Assignments([]);
                $.each(topic.SubtopicContents, function (arrayId, item) {
                    my.courseVm.selectedTopic.SubtopicContents.push(item);
                });
                $.each(topic.Assignments, function (arrayId, item) {
                    my.courseVm.selectedTopic.Assignments.push(item);
                });
            }
        },
        selectedTopic = {
            Id: ko.observable(0),
            Name: ko.observable(""),
            Description: ko.observable(""),
            SubtopicContents: ko.observableArray([]),
            Assignments: ko.observableArray([])
        },
        getCourseCallback = function (jsonData) {
            if (jsonData !== null) {
                my.courseVm.courseInfo.Course = jsonData;
                ko.utils.arrayForEach(my.courseVm.courseInfo.Course.CourseSubtopics, function (subtopic) {
                    
                    ko.utils.arrayForEach(subtopic.SubtopicContents, function (item) {
                        item.IsCompleted = ko.observable(item.IsCompleted);
                    });
                    ko.utils.arrayForEach(subtopic.Assignments, function (item) {
                        item.IsCompleted = ko.observable(item.IsCompleted);
                        item.IsApproved = ko.observable(item.IsApproved);
                        item.Rating = ko.observable(0);
                        item.Feedback = ko.observableArray(item.Feedback);
                        
                        if (item.IsApproved() && item.Feedback().length > 0) {
                            ko.utils.arrayForEach(item.Feedback(), function (f) {
                                if (f.FeedbackType.FeedbackTypeId == 3) {
                                    item.Rating(f.Rating);
                                }
                            });
                        }
                        

                        item.NewFeedback = {
                            FeedbackId: ko.observable(0),
                            Title: ko.observable(""),
                            FeedbackText: ko.observable(""),
                            FeedbackType: {
                                FeedbackTypeId : ko.observable("3"),
                                Description:ko.observable("Assignment")
                            },
                            Rating: ko.observable(""),
                            AddedBy: {
                                UserId : ko.observable(""),
                                FullName: ko.observable(""),
                                ProfilePictureName : ko.observable("")
                            },
                            AddedFor: {
                                UserId: ko.observable("")
                            },
                            AddedOn: ko.observable(""),
                            ValidationMsg: ko.observable(""),
                            IsFeedbackCommentValid : ko.observable(true)

                        }
                    });
                });
                
                my.courseVm.courseInfo.Icon = my.rootUrl + "/Uploads/CourseIcon/" + jsonData.Icon;
                if (my.courseVm.courseInfo.Course.CourseSubtopics.length > 0) {
                    my.courseVm.showSelectedTopic(my.courseVm.courseInfo.Course.CourseSubtopics[0]);
                }
                ko.applyBindings(my.courseVm);
            }
        },
        getCourse = function () {
            if (my.courseVm.courseInfo.TraineeId == undefined) {
                my.courseVm.courseInfo.TraineeId = null;
            }
            my.courseService.getCourseWithAllData(my.courseVm.courseInfo.CourseId, my.courseVm.courseInfo.TraineeId, getCourseCallback);
        },

        saveSubtopicContentProgressCallback = function (jsonData) {

        },

        updateAssignmentProgressCallback = function (jsonData) {

        },
        saveProgress = function (data) {
            if (!data.IsCompleted() && my.meta.currentUser.IsTrainee) {
                data.IsCompleted(true);
                my.courseService.saveSubtopicContentProgress(data.Id, saveSubtopicContentProgressCallback);
            }
            
        };

        updateAssignmentProgress = function (data) {
            if (!data.IsCompleted() && my.meta.currentUser.IsTrainee) {
                data.IsCompleted(true);
                my.courseService.updateAssignmentProgress(data, updateAssignmentProgressCallback);
            }
        };

        var feedbackDataToUpdate;
        updateAssignmentProgressWithFeedbackCallback = function (jsonData) {
            if (jsonData > 0) {
                var dateNow = new Date();
                feedbackDataToUpdate.NewFeedback.FeedbackId(jsonData);
                feedbackDataToUpdate.NewFeedback.AddedOn(dateNow.getTime());
                feedbackDataToUpdate.NewFeedback.AddedBy.FullName(my.meta.currentUser.FirstName + " " + my.meta.currentUser.LastName);
                feedbackDataToUpdate.NewFeedback.AddedBy.UserId(my.meta.currentUser.UserId);
                feedbackDataToUpdate.NewFeedback.AddedBy.ProfilePictureName(my.meta.currentUser.ProfilePictureName);
                if (feedbackDataToUpdate.NewFeedback.FeedbackType.FeedbackTypeId() == 3)
                {
                    feedbackDataToUpdate.Rating(feedbackDataToUpdate.NewFeedback.Rating());
                }
                var feedback = ko.mapping.toJS(feedbackDataToUpdate.NewFeedback);
                if (feedbackDataToUpdate.Feedback().length > 0) {
                    feedbackDataToUpdate.Feedback.unshift(feedback);
                }
                else {
                    feedbackDataToUpdate.Feedback.push(feedback);
                }
            }
            else {
                alert("some problem occured while adding feedback");
            }
        }
        updateAssignmentProgressWithFeedback = function (data) {
            if (validateAddedFeedback(data.NewFeedback)) {
                feedbackDataToUpdate = data;
                if (data.IsCompleted() && !data.IsApproved() && data.NewFeedback.FeedbackType.FeedbackTypeId() == 3) {
                    data.IsApproved(true);
                    data.NewFeedback.Title(data.Name);
                    data.NewFeedback.FeedbackType.Description("Assignment");
                }
                else {
                    data.IsApproved(false);
                    data.IsCompleted(false);
                    data.NewFeedback.FeedbackType.FeedbackTypeId(1);
                    data.NewFeedback.Title(data.Name + " Reassigned");
                    data.NewFeedback.FeedbackType.Description("Comment");
                    data.NewFeedback.Rating(0);
                }
                data.NewFeedback.AddedFor.UserId(data.TraineeId);
                var assignmentData = ko.mapping.toJS(data);
                assignmentData.Feedback = [];
                assignmentData.Feedback.push(assignmentData.NewFeedback);
                my.courseService.updateAssignmentProgress(assignmentData, updateAssignmentProgressWithFeedbackCallback);
            }
            
        }

        var validateAddedFeedback = function (data) {
            if (my.isNullorEmpty(data.FeedbackText())) {
                data.ValidationMsg("write some feedback");
                data.IsFeedbackCommentValid(false);
                return false;
            }
            if (data.FeedbackType.FeedbackTypeId() == 3 && data.Rating() < 1 || data.Rating() > 5)
            {
                data.ValidationMsg("Choose a rating");
                return false;
            }
            
            return true;
        }


        var loadFeedbackWithThread = function(feedbackId) {
                my.feedbackThreadsVm.loadFeedbackDailog(feedbackId);
        }
        return {
            loadFeedbackWithThread : loadFeedbackWithThread,
            courseInfo: courseInfo,
            getCourse: getCourse,
            selectedTopic: selectedTopic,
            showSelectedTopic: showSelectedTopic,
            saveProgress: saveProgress,
            updateAssignmentProgress: updateAssignmentProgress,
            updateAssignmentProgressWithFeedback : updateAssignmentProgressWithFeedback
        }
    }();
    my.courseVm.getCourse();
});