$(document).ready(function() {

    my.editCrSystemRatingConfigVm = function() {

        var ratingConfig ={
            rangeConfig: ko.observableArray([]),
            weightConfig: ko.observableArray([])
        };
        var teams = ko.observableArray([]);
        var selectedTeam = ko.observable({});

        var getRatingCallback = function(data) {
            ratingConfig.rangeConfig(data.CrRatingCalcRangeConfigs);
            ratingConfig.weightConfig(data.CrRatingCalcWeightConfigs);
        };

        var getRatingConfigForTeam = function(team) {
            my.userService.getCrRatingConfig(team, getRatingCallback);
        };


        var getRatingConfig = function () {
            my.editCrSystemRatingConfigVm.isVisible(true);
            if (my.meta.isAdministrator()) {
                my.userService.getAllTeams(function(data) {
                    my.editCrSystemRatingConfigVm.teams (data);
                    my.editCrSystemRatingConfigVm.selectedTeam (my.editCrSystemRatingConfigVm.teams()[0]);
                    getRatingConfigForTeam(my.editCrSystemRatingConfigVm.selectedTeam());
                });
                return;
            }
            my.userService.getCrRatingConfig(null, getRatingCallback);
        }
        return {
            getRatingConfig: getRatingConfig,
            getRatingConfigForTeam: getRatingConfigForTeam,
            selectedTeam: selectedTeam,
            ratingConfig: ratingConfig,
            teams: teams,
            isVisible: ko.observable(false),
            saveConfigSettings: function() {
                alert("Saved");
            }
        };

    }();
    my.editCrSystemRatingConfigVm.selectedTeam.subscribe(my.editCrSystemRatingConfigVm.getRatingConfigForTeam);
    //ko.applyBindings(my.editCrSystemRatingConfigVm);
});