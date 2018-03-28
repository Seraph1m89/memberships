$(function () {
    var code = $(".register-code-panel input");
    var alertDiv = $(".register-code-panel .alert");

    function displayMessage(success, message) {
        alertDiv.text(message);
        if (success) {
            alertDiv.removeClass("alert-danger").addClass("alert-success");
        } else {
            alertDiv.removeClass("alert-success").addClass("alert-danger");
        }

        alertDiv.removeClass("hidden");
    }

    $(".register-code-panel button").click(function () {
        alertDiv.addClass("hidden");
        if (code.val().length === 0) {
            displayMessage(false, "Enter a code");
            return;
        }

        $.post("/RegisterCode/Register",
            { code: code.val() },
            function (data) {
                displayMessage(true, "The code was successfuly added");
                code.val("");
            }).fail(function () {
                displayMessage(false, "Could not register the code");
            });
    });
});