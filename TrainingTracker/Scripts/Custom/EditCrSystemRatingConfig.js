$(document).ready(function() {

    my.editCrRatingConfigVm = function() {

        var ratingConfig = ko.observable({});
        var validationMessage = "";
        var getRatingConfig = function() {
            my.userService.getCrRatingConfig(null, function(data) {
                ratingConfig(data);
                my.editCrRatingConfigVm.isVisible(true);
            });
        }

        var validateConfig = function() {
            var message = "";
            var isValid = true;

            var configMax = 10;
            var configMin = 0;
            var weightConfigs = my.editCrRatingConfigVm.ratingConfig().CrRatingCalcWeightConfigs;
            for (var i = 1; i < weightConfigs.length - 1; i++) {
                var weightConfig = weightConfigs[i];

                if (isNaN(parseFloat(weightConfig.Weight))) {
                    message += 'Invalid weight format of <strong>' + getReviewPointRating(weightConfig.ReviewPointTypeId) + '</strong>';
                    isValid = false;
                    message += '.<br/>';
                }

                if (weightConfig.Weight <= configMin) {
                    message += 'Weight for <strong>' + getReviewPointRating(weightConfig.ReviewPointTypeId) + '</strong> cannot be less than or equal to ' + configMin;
                    isValid = false;
                    message += '.<br/>';
                }

                if (weightConfig.Weight >= configMax) {
                    message += 'Weight for <strong>' + getReviewPointRating(weightConfig.ReviewPointTypeId) + '</strong> cannot be greater than or equal to' + configMax;
                    isValid = false;
                    message += '.<br/>';
                }


            }

            if (!isValid) {
                message = "<h4>Validation Errors for Review Points: </h4>" + message;
            }

            var rangeConfigs = my.editCrRatingConfigVm.ratingConfig().CrRatingCalcRangeConfigs;

            var isRangeValid = true;

            for (var i = 0; i < rangeConfigs.length - 1; i++) {

                var rangeConfig = rangeConfigs[i];
                var rangeInvalidMessage = "<h4>Validation Errors for Feedback Rating: </h4>";

                if (isNaN(parseFloat(rangeConfig.RangeMax))) {
                    message += (isRangeValid ? rangeInvalidMessage : '') + 'Invalid Maximum score format of <strong>'
                        + getFeedbackRating(rangeConfig.FeedbackTypeId) + '</strong>';
                    isRangeValid = false;
                    message += '.<br/>';
                }

                if (rangeConfig.RangeMax <= configMin) {
                    message += (isRangeValid ? rangeInvalidMessage : '') + ' Maximum score of <strong>'
                        + getFeedbackRating(rangeConfig.FeedbackTypeId) + '</strong> cannot be less than or equal to ' + configMin;
                    isRangeValid = false;
                    message += '.<br/>';
                }

                if (rangeConfig.RangeMax >= configMax) {
                    message += (isRangeValid ? rangeInvalidMessage : '') + 'Maximum score of <strong>'
                        + getFeedbackRating(rangeConfig.FeedbackTypeId) + '</strong> cannot be greater than or equal to' + configMax;
                    isRangeValid = false;
                    message += '.<br/>';
                }

                if (rangeConfig.RangeMax >= rangeConfigs[i + 1].RangeMax) {
                    message += (isRangeValid ? rangeInvalidMessage : '') + 'Maximum score of <strong>'
                        + getFeedbackRating(rangeConfig.FeedbackTypeId)
                        + '</strong> should be less than maximum score of <strong>'
                        + getFeedbackRating(rangeConfigs[i + 1].FeedbackTypeId) + '</strong>';
                    isRangeValid = false;
                    message += '.<br/>';
                }
            }

            isValid = isRangeValid && isValid;
            validationMessage = message;
            return isValid;
        }

        var saveConfigSettings = function() {


            if (!validateConfig()) {
                $.alert({
                    title: 'Invalid Input',
                    columnClass: 'col-md-6 col-md-offset-3 col-sm-8 col-sm-offset-2 col-xs-10',
                    useBootstrap: true,
                    content: validationMessage,
                    buttons: {
                        ok: {
                            btnClass: 'btn-primary btn-danger'
                        }
                    }
                });
                return;
            }


            my.userService.updateCrRatingConfig(ratingConfig(), function(data) {
                console.dir(data);
                ratingConfig(data);
            });
        };

        return {
            getRatingConfig: getRatingConfig,
            ratingConfig: ratingConfig,

            isVisible: ko.observable(false),
            saveConfigSettings: saveConfigSettings,
            updateConfigModel: function() {
                my.editCrRatingConfigVm.ratingConfig.valueHasMutated();
            }
        };
    }();
});

function getReviewPointRating(ratingId) {
    switch (ratingId) {
        case 1:
            return "Exceptional";
        case 2:
            return "Good";
        case 3:
            return "Corrected";
        case 4:
            return "Poor";
        case 5:
            return "Critical";
        case 6:
            return "Suggestion";
    }
};

function getFeedbackRating(ratingId) {
    switch (ratingId) {
        case 1:
            return "Slow";
        case 2:
            return "Average";
        case 3:
            return "Fast";
        case 4:
            return "Exceptional";
    }
}

function getPrevMax(index) {
    if (index > 0)
        return my.editCrRatingConfigVm.ratingConfig().CrRatingCalcRangeConfigs[index - 1].RangeMax;
}