 $(function () {
    //We'll use a JQuery code to implement async Client-side Login ()nality
    // $ is an alias for a jQuery
    var userLoginButton = $("#UserLoginModal button[name='login']").click(onUserLoginClick);
    function onUserLoginClick() {  //EventHandler

        $("#UserLoginModal button[name='login']").prop('disabled', true);

        var antiForgeryToken = $("#UserLoginModal input[name='__RequestVerificationToken']").val();
        //retrieve the value from hidden text in __RequestVerificationToken -- by MVC -- then submit to server-side Login() and Logout()

        var email = $("#UserLoginModal input[name='Email']").val();
        var password = $("#UserLoginModal input[name='Password']").val();
        var rememberMe = $('.form-check input[name="rememberMe"]').prop('checked');

        var url = `Account/Login?RememberMe=${rememberMe}`; //loc set to Login()
        //set as an unanimous ("fully agreed") javascript obj, encapsulating the user's input as well as antiForgeryToken
        var userinput = {
            __RequestVerificationToken: antiForgeryToken, //Confirmed here
            Email: email,
            Password: password,            
        };
        //Using JQuery, we can post to the server
        $.ajax({
            type: "POST",
            url: url, //We set our url to the Action()
            data: userinput,
            success: function (data) { // runs if the post request is successful
                //N: Login()'s return type is PartialView --  the front end encapsulating is passed as a arg to the 'data' to the server, once the login() is complete
                //This ajax allows us to update a portion of the html rendered in the browser asynchronously, rather than needing to update the whole html doc

                var parsed = $.parseHTML(data); //parse the html stored in data, returned from server
                var hasErrors = $(parsed).find("input[name='_ErrorCardFlag']").val() == "true";
                if (hasErrors) {
                    $("#UserLoginModal").html(data); //display Login Dialog Again since it contains the feedback, so go back


                    $("#UserLoginModal button[name='login']").prop('disabled', false);
                    //Wire up the click event for the login button again 
                    var userLoginButton = $("#UserLoginModal button[name='login']").click(onUserLoginClick);
                    $(".progress").hide("fade");

                    var form = $("#UserLoginForm");
                    //Allows client-side unobtrusive validation to word after a login attempt fails!
                    $(form).removeData("validator");
                    $(form).removeData("unobtrusiveValidation");
                    $.validator.unobtrusive.parse(form);
                }
                else {
                    $("#UserLoginModal button[name='login']").prop('disabled', false);
                    location.href = '/Index';
                }
                
            }, //handle post fail
            error: function (xhr, ajaxOptions, thrownError) {
                var errorText = "Status: " + xhr.status + " - " + xhr.statusText;
                PresentClosableBootstrapAlert("#alert_placeholder_login","danger", "Login Error", errorText);
                console.error(thrownError + "\r\n" + xhr.statusText + "\r\n" + xhr.responseText);
                $("#UserLoginModal button[name='login']").prop('disabled', false);
            }                
            });
    }
 });