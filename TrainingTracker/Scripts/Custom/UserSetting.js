$(document).ready(function () {

    my.userSettingVm = function () {

        var selectedSetting = ko.observable("");
        var markAsSelected = function(selectedSettingName) {
            selectedSetting(selectedSettingName);
        }
        return {
            selectedSetting: selectedSetting,
            markAsSelected: markAsSelected
        }
    }();
    ko.applyBindings(my.userSettingVm);
});