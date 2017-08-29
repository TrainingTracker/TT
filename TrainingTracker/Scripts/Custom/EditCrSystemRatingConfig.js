$(document).ready(function() {

    my.editCrSystemRatingConfigVm = function() {

        var ratingConfig = ko.observable({});

        var getRatingConfig = function() {
            my.userService.getCrRatingConfig(null, function(data) {
                ratingConfig(data);
                my.editCrSystemRatingConfigVm.isVisible(true);
            });
        }
        var saveConfigSettings = function() {
            my.userService.updateCrRatingConfig(ratingConfig(), function (data) {
                console.dir(data);
                ratingConfig(data);
            });
        };

        return {
            getRatingConfig: getRatingConfig,
            ratingConfig: ratingConfig,

            isVisible: ko.observable(false),
            saveConfigSettings: saveConfigSettings,
            updateConfigModel: function () {
                my.editCrSystemRatingConfigVm.ratingConfig.valueHasMutated();
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
        return my.editCrSystemRatingConfigVm.ratingConfig().CrRatingCalcRangeConfigs[index - 1].RangeMax;
}