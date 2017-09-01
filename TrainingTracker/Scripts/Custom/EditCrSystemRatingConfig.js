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
            for (var i = 1; i < weightConfigs.length-1; i++) {
                var weightConfig = weightConfigs[i];

                if (isNaN(parseFloat(weightConfig.Weight))) {
                    message += 'Invalid weight format of <strong>' + getReviewPointRating(weightConfig.ReviewPointTypeId)+'</strong>';
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

            var rangeConfigs = my.editCrRatingConfigVm.ratingConfig().CrRatingCalcRangeConfigs;

            for (var i = 0; i < rangeConfigs.length-1; i++) {

                var rangeConfig = rangeConfigs[i];

                if (isNaN(parseFloat(rangeConfig.RangeMax))) {
                    message += 'Invalid upper limit format of <strong>' + getFeedbackRating(rangeConfig.FeedbackTypeId) + '</strong>';
                    isValid = false;
                    message += '.<br/>';
                }

                if (rangeConfig.RangeMax <= configMin) {
                    message += ' Upper limit format of <strong>' + getFeedbackRating(rangeConfig.FeedbackTypeId) + '</strong> cannot be less than or equal to ' + configMin;
                    isValid = false;
                    message += '.<br/>';
                }

                if (rangeConfig.RangeMax >= configMax) {
                    message += 'Upper limit format of <strong>' + getFeedbackRating(rangeConfig.FeedbackTypeId) + '</strong> cannot be greater than or equal to' + configMax;
                    isValid = false;
                    message += '.<br/>';
                }

                if (rangeConfig.RangeMax >= rangeConfigs[i+1].RangeMax) {
                    message += 'Upper limit format of <strong>' + getFeedbackRating(rangeConfig.FeedbackTypeId) + '</strong> cannot be greater than or equal to the upperlimit of next Feedback Type';
                    isValid = false;
                    message += '.<br/>';
                }

                if (i!=0 && rangeConfig.RangeMax <= rangeConfigs[i - 1].RangeMax) {
                    message += 'Upper limit format of <strong>' + getFeedbackRating(rangeConfig.FeedbackTypeId) + '</strong> cannot be lesser than or equal to the upperlimit of previous Feedback Type';
                    isValid = false;
                    message += '.<br/>';
                }

            }
            validationMessage = message;
            return isValid;
        }

        var saveConfigSettings = function() {


            if (!validateConfig()) {
                $.alert({
                    title: 'Invalid Format',
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