$(document).ready(function () {
    my.allReleaseVm = function () {

        var release = {
            CurrentPage: ko.observable(1),
            PageCount: ko.observable(0),
            PageSize: ko.observable(0),
            RowCount: ko.observable(0),
            DisplayReleases: ko.observableArray([])
        };

        var currentRelease =
        {
            Id: ko.observable(0),
            Title: ko.observable(""),
            Description: ko.observable(""),
            Date: ko.observable(""),
            AddedBy: ko.observable(null)
        };

        var newRelease =
        {
            Title: ko.observable(""),
            Description: ko.observable(""),
            Type: ko.observable("Patch"),

        }

        var settings =
        {
            searchReleaseId: ko.observable(my.queryParams["releaseId"]),
            wildcard: ko.observable(""),
            addReleaseModal: ko.observable(false)
        };

        var photoUrl = function (pictureName) {
            return my.rootUrl + "/Uploads/ProfilePicture/" + (pictureName || "dummy.jpg");
        };

        var getReleases = function () {
            if (!settings.searchReleaseId()) {
                settings.searchReleaseId(0);
            }
            my.releaseService.GetReleaseOnFilter(settings.wildcard(), settings.searchReleaseId(), release.CurrentPage(), getReleaseCallback);
        };

        var getReleaseCallback = function (response) {
            if (response) {
                release.CurrentPage(response.CurrentPage),
                    release.PageCount(response.PageCount),
                    release.PageSize(response.PageSize),
                    release.RowCount(response.RowCount),
                    release.DisplayReleases([]);

                $.each(response.Results, function (arrayId, item) {
                    release.DisplayReleases.push(item);
                });

                closeReleaseDialogue();
                loadDefaultRelease(release.DisplayReleases()[0]);
            }
        };

        var loadDefaultRelease = function (releaseObject) {
            if (releaseObject == null) return;

            currentRelease.Id(releaseObject.ReleaseId),
            currentRelease.Title(releaseObject.ReleaseTitle),
            currentRelease.Description(releaseObject.Description),
            currentRelease.Date(releaseObject.ReleaseDate),
            currentRelease.AddedBy(releaseObject.AddedBy);
        };

        var loadReleaseData = function (releaseId) {

            var activeAttendies = ko.utils.arrayFilter(release.DisplayReleases(), function (releaseObj) {
                return releaseObj.ReleaseId == releaseId;
            });

            if (activeAttendies.length == 0) {
                settings.searchReleaseId(releaseId);
                getReleases();
                return;
            }
            loadDefaultRelease(activeAttendies[0]);
        };

        var resetFilterAndGetData = function () {
            release.CurrentPage(1);
            settings.searchReleaseId(0);
            getReleases();
        };

        var getNextPage = function () {
            release.CurrentPage(release.CurrentPage() + 1);
            getReleases();
        };

        var getPreviousPage = function () {
            release.CurrentPage(release.CurrentPage() - 1);
            getReleases();
        };

        var closeReleaseDialogue = function () {
            settings.addReleaseModal(false);
            clearNewRelease();
        }

        var addNewRelease = function () {
            clearNewRelease();
            settings.addReleaseModal(true);

        };

        var clearNewRelease = function () {
            newRelease.Title("");
            newRelease.Description("");
            newRelease.Type("Patch");
        }

        var publishRelease = function () {
            if (validateReleaseData()) {

                var type = 1;
                if (newRelease.Type() == 'Major') {
                    type = 3;
                }
                else if (newRelease.Type() == 'Minor') {
                    type = 2;
                } else {
                    type = 1;
                }

                var releaseDetails =
                {
                    ReleaseType: type,
                    ReleaseTitle: newRelease.Title(),
                    Description: newRelease.Description()
                }

                my.releaseService.addRelease(releaseDetails, getReleaseCallback);
            }
            else {
                alert("All fields are mandatory.");
            }
        }

        var validateReleaseData = function () {
            if (my.isNullorEmpty(newRelease.Title()) || my.isNullorEmpty(newRelease.Description())) {
                return false;
            }
            return true;
        }

        return {
            release: release,
            currentRelease: currentRelease,
            getReleases: getReleases,
            settings: settings,
            AllRelease: release.DisplayReleases,
            loadReleaseData: loadReleaseData,
            photoUrl: photoUrl,
            resetFilterAndGetData: resetFilterAndGetData,
            getNextPage: getNextPage,
            getPreviousPage: getPreviousPage,
            newRelease: newRelease,
            closeReleaseDialogue: closeReleaseDialogue,
            addNewRelease: addNewRelease,
            publishRelease: publishRelease
        };

    }();
    my.allReleaseVm.getReleases();
    ko.applyBindings(my.allReleaseVm);
});