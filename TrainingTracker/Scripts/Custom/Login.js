$(document).ready(function () {

    my.loginVm = function () {
        var userName = ko.observable(),
            password = ko.observable(),
            validationMessage = ko.observable(),
            validate = function () {
                var result = true;
                if (my.loginVm.userName() == undefined ||
                                    my.loginVm.userName() == "") {
                    my.loginVm.validationMessage("Enter username.");
                    result = false;
                }
                else if (my.loginVm.password() == undefined ||
                                    my.loginVm.password() == "") {
                    my.loginVm.validationMessage("Enter password.");
                    result = false;
                }
                return result;
            },
            authenticateUserCallback = function (response) {
                if (response.IsValid) {
                    window.location = my.rootUrl+'/Login/Valid';
                }
                else {
                    my.loginVm.validationMessage("Invalid credentials.");
                }
            },
            authenticateUser = function () {
                if (my.loginVm.validate()) {
                    my.userService.authenticateUser({
                        UserName: my.loginVm.userName(),
                        Password: my.loginVm.password()
                    }, my.loginVm.authenticateUserCallback);
                }
            };

        return {
            userName: userName,
            password: password,
            validationMessage: validationMessage,
            validate: validate,
            authenticateUserCallback: authenticateUserCallback,
            authenticateUser: authenticateUser
        };
    }();

    ko.applyBindings(my.loginVm);
});