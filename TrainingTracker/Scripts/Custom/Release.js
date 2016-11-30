$(document).ready(function () {
    my.allReleaseVm = function () {
        var releases = ko.observableArray([]),
            showModal = ko.observable(false),
            isVisibleSubmitButton = ko.observable(true),
            releaseCaption = ko.observable("New Release"),
            releaseId = my.queryParams["releaseId"],
            initialLoad = true,
        selectCaption = ko.observable("Select Version:"),
        isViewMode = ko.observable(false),
        isVisibleNewVersion = ko.observable(true),
        isVisiblePublishButton = ko.observable(false),
        existingRelease = {
            Major: 0,
            Minor: 0,
            Patch: 0
        },
        currentVersion = ko.observable("0.0.0"),
        releaseDetails = {
        ReleaseId: 0,
        Major: 0,
        Minor: 0,
        Patch: 0,
        ReleaseTitle: ko.observable(""),
        IsPublished: ko.observable(false),
        Description: ko.observable(""),
        ReleaseDate: ko.observable(new Date()),
        Version: ko.observable(""),
        AddedBy: {FullName : ''}
        },
    openReleaseDialogue = function () {
        selectedReleaseType("Patch");
        resetReleaseDialogue();
        isVisiblePublishButton(true);
        showModal(true);
    },
    closeReleaseDialogue = function () {
        resetReleaseDialogue();
        showModal(false);

    },
    getVersion = function (item) {
        return item.Major + '.' + item.Minor + '.' + item.Patch;
    },
    errorText = ko.observable(""),
    getReleasesCallback = function (releaseList)
    {
        existingRelease.Major = 0;
        existingRelease.Minor = 0;
        existingRelease.Patch = 0;
        
        my.allReleaseVm.releases([]);
        ko.utils.arrayForEach(releaseList, function (item) {
            var version = getVersion(item);
            item.Version = (version == "0.0.0") ? "--" : version;
            item.ReleaseDate = (item.ReleaseDate == null) ? "N/A" : moment(item.ReleaseDate).format('DD/MM/YYYY');
            item.IsPublished = item.IsPublished;
            my.allReleaseVm.releases.push(item);

            if (version !== "0.0.0" && (existingRelease.Major <= item.Major && existingRelease.Minor <= item.Minor && existingRelease.Patch <= item.Patch)) {
                existingRelease.Major = item.Major;
                existingRelease.Minor = item.Minor;
                existingRelease.Patch = item.Patch;
            }
        });

        if (!my.isNullorEmpty(releaseId) && releaseId > 0 && initialLoad) loadReleaseDetails(releaseId);
        releaseId = 0;
        initialLoad = false;
    },
    getReleases = function () {
        my.releaseService.getAllRelease(my.allReleaseVm.getReleasesCallback);
    },
    selectedReleaseType = ko.observable(),
    newVersion = ko.computed(function () {
        if (releases().length == 0 || !releaseDetails.IsPublished()) {
            releaseDetails.Major = 0;
            releaseDetails.Minor = 0;
            releaseDetails.Patch = 1;
            return "0.0.1";
        }
        var item = existingRelease;
        releaseDetails.Major = item.Major;
        releaseDetails.Minor = item.Minor;
        releaseDetails.Patch = item.Patch;
        switch (selectedReleaseType()) {
            case "Major":
                my.allReleaseVm.releaseDetails.Major += 1;
                my.allReleaseVm.releaseDetails.Minor = 0;
                my.allReleaseVm.releaseDetails.Patch = 0;
                break;
            case "Minor":
                if (my.allReleaseVm.releaseDetails.Minor >= 9) {
                    my.allReleaseVm.releaseDetails.Major += 1;
                    my.allReleaseVm.releaseDetails.Minor = 0;
                }
                else {
                    my.allReleaseVm.releaseDetails.Minor += 1;
                }
                my.allReleaseVm.releaseDetails.Patch = 0;
                break;
            case "Patch":
                if (my.allReleaseVm.releaseDetails.Patch >= 9) {
                    my.allReleaseVm.releaseDetails.Minor += 1;
                    if (my.allReleaseVm.releaseDetails.Minor >= 9) {
                        my.allReleaseVm.releaseDetails.Major += 1;
                        my.allReleaseVm.releaseDetails.Minor = 0;
                    }
                    my.allReleaseVm.releaseDetails.Patch = 0;
                }
                else {
                    my.allReleaseVm.releaseDetails.Patch += 1;
                }
                break;
        }
        return getVersion(my.allReleaseVm.releaseDetails);
    }),
    addRelease = function () {
        if (!my.allReleaseVm.validateReleaseData()) return;
        
        if (!releaseDetails.IsPublished()) {
            releaseDetails.ReleaseDate(null);
            releaseDetails.Major = 0;
            releaseDetails.Minor = 0;
            releaseDetails.Patch = 0;
        }
        
        if (my.allReleaseVm.releaseDetails.ReleaseId != 0) {
            my.releaseService.UpdateRelease(my.allReleaseVm.releaseDetails, my.allReleaseVm.updateReleaseCallback);
           // resetReleaseDialogue();
            return;
        }
        my.releaseService.addRelease(my.allReleaseVm.releaseDetails, my.allReleaseVm.addReleaseCallback);
    },
    addReleaseCallback = function (addStatus) {
        if (addStatus)
        {
            my.allReleaseVm.showModal(false);
            getReleases();           
            resetReleaseDialogue();

        }
        else
        {
            alert("Add Release Failure");
        }
    },
    resetReleaseDialogue = function () {
        selectedReleaseType("Patch");
        my.allReleaseVm.releaseDetails.ReleaseTitle("");
        my.allReleaseVm.releaseDetails.Description("");
        my.allReleaseVm.releaseDetails.IsPublished(false);
        my.allReleaseVm.releaseDetails.ReleaseDate(new Date());
        my.allReleaseVm.releaseDetails.ReleaseId = 0;
        my.allReleaseVm.errorText("");
        isViewMode(false);
        isVisibleNewVersion(true);
        isVisiblePublishButton(false);
        my.allReleaseVm.isVisibleSubmitButton(true);
        my.allReleaseVm.releaseCaption("New Release");
        my.allReleaseVm.selectCaption("Select Version:");
    },

    validateReleaseData = function () {
        if (my.isNullorEmpty(releaseDetails.ReleaseTitle()) || my.isNullorEmpty(releaseDetails.Description())) {
            my.allReleaseVm.errorText("All fields are mandatory.");
            return false;
        }
        else {
            my.allReleaseVm.errorText("");
            return true;
        }
    },
    loadReleaseDetails = function (ReleaseId, action) {
        openReleaseDialogue();
       // my.allReleaseVm.releaseDetails.IsPublished(true);
        var filteredRelease = ko.utils.arrayFilter(my.allReleaseVm.releases(), function (item) {
            return item.ReleaseId == ReleaseId;
        });
        
        if (filteredRelease.length == 0) return;
        
        my.allReleaseVm.releaseDetails.ReleaseTitle(filteredRelease[0].ReleaseTitle);
        my.allReleaseVm.releaseDetails.Description(filteredRelease[0].Description);
        my.allReleaseVm.releaseDetails.ReleaseId = filteredRelease[0].ReleaseId;
        my.allReleaseVm.isVisibleSubmitButton(false);
        
        if (typeof(action) == 'undefined') {
            action = filteredRelease[0].IsPublished ? 'VIEW' : 'EDIT';
        }

        if (action.toUpperCase() === 'EDIT') {
            my.allReleaseVm.releaseCaption("Publish Release");
            isVisiblePublishButton(true);
            
        }
        else if (action.toUpperCase() === 'VIEW')
        {
            my.allReleaseVm.releaseCaption("View Release");
            my.allReleaseVm.selectCaption("Selected Version:");
            my.allReleaseVm.isViewMode(true);
            isVisibleNewVersion(false);
            my.allReleaseVm.releaseDetails.ReleaseDate(moment(filteredRelease[0].ReleaseDate,"DD/MM/YYYY"));
            my.allReleaseVm.currentVersion(getVersion(filteredRelease[0]));
        }
        
        if (filteredRelease[0].IsPublished) {
            my.allReleaseVm.releaseDetails.IsPublished(true);
            isVisiblePublishButton(false);
        }
        else
        {
            my.allReleaseVm.releaseDetails.IsPublished(false);
            isVisiblePublishButton(true);
        }
    },
    updateReleaseCallback = function (updateStatus) {
        my.allReleaseVm.showModal(false);
        if (updateStatus) {
            getReleases();
            my.meta.getNotification();
        }
        else {
            alert("Update failure");
        }
    },
    updateRelease = function () {
        if (!my.allReleaseVm.validateReleaseData()) return;

        if (my.allReleaseVm.releaseDetails.ReleaseId == 0) {
            my.releaseService.addRelease(my.allReleaseVm.releaseDetails, my.allReleaseVm.addReleaseCallback);
            //resetReleaseDialogue();
            return;
        }

        my.releaseService.UpdateRelease(my.allReleaseVm.releaseDetails, my.allReleaseVm.updateReleaseCallback);
        resetReleaseDialogue();
        closeReleaseDialogue();
    };

        return {
            releases: releases,
            isViewMode: isViewMode,
            isVisiblePublishButton: isVisiblePublishButton,
            isVisibleNewVersion: isVisibleNewVersion,
            getReleasesCallback: getReleasesCallback,
            getReleases: getReleases,
            addRelease: addRelease,
            addReleaseCallback: addReleaseCallback,
            selectedReleaseType: selectedReleaseType,
            newVersion: newVersion,
            releaseDetails: releaseDetails,
            validateReleaseData: validateReleaseData,
            errorText: errorText,
            showModal: showModal,
            openReleaseDialogue: openReleaseDialogue,
            closeReleaseDialogue: closeReleaseDialogue,
            existingRelease: existingRelease,
            loadReleaseDetails: loadReleaseDetails,
            currentVersion: currentVersion,
            isVisibleSubmitButton: isVisibleSubmitButton,
            updateRelease: updateRelease,
            updateReleaseCallback: updateReleaseCallback,
            releaseCaption: releaseCaption,
            selectCaption: selectCaption
        };
    }();
    my.allReleaseVm.getReleases();
    ko.applyBindings(my.allReleaseVm);
});