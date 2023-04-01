$(function () { //Ref: UserLogin.js

    var RegistrationModal = new bootstrap.Modal(document.getElementById('UserRegistrationModal'), {}) //Bootstrap 5

    /* TODO: Registration prompt -- outside of index view
    $('.RegisterLink').click(onCourseCardClick); // When the course card is clicked by a public user R: _DisplayCardRowPartial.cshtml  -- " " doesn't work       

    function onCourseCardClick() {
        RegistrationModal.show();
        $("#UserRegistrationModal input[name='CategoryId']").val($(this).attr('data-categoryId'));
    }
    */

    $("#UserRegistrationModal button[name='register']").on("click", onUserClick);

    function onUserClick() {

        var url = "Account/Register";

        var antiForgeryToken = $("#UserRegistrationModal input[name='__RequestVerificationToken']").val();
        var personName = $("#personName").val();
        var email = $("#UserRegistrationModal input[name='Email']").val();
        var password = $("#UserRegistrationModal input[name='Password']").val();
        var confirmPassword = $("#UserRegistrationModal input[name='ConfirmPassword']").val();
        var phonenumber = $("#UserRegistrationModal input[name='Phone']").val();

        var registerDTO = {
            // __RequestVerificationToken: antiForgeryToken,
            PersonName: personName,
            Email: email,
            Password: password,
            ConfirmPassword: confirmPassword,
            Phone: phonenumber,
        };

        $.ajax({
            type: "POST",
            url: url,
            data: registerDTO,
            success: function (data) {
                var parsed = $.parseHTML(data)
                var hasErrors = $(parsed).find("input[name='_ErrorCardFlag']").val() == "true";

                if (hasErrors) {
                    $("#UserRegistrationModal").html(data);

                    var userLoginButton = $("#UserRegistrationModal button[name='register']").click(onUserClick);
                    var form = $("#UserRegistrationForm");
                    $(form).removeData("validator");
                    $(form).removeData("unobtrusiveValidation");
                    $.validator.unobtrusive.parse(form);
                }
                else {
                    location.href = '/Home/Index';
                }

            },
            error: function (xhr, ajaxOptions, thrownError) {
                var errorText = "Status: " + xhr.status + " - " + xhr.statusText;

                PresentClosableBootstrapAlert("#alert_placeholder_registration", "danger", "Error!", errorText);

                console.error(thrownError + "\r\n" + xhr.statusText + "\r\n" + xhr.responseText);
            }
        });
    };
})