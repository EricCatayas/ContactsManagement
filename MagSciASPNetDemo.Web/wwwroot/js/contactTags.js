
$(document).ready(function () {
    $('#removeTagIcon').click(function () {
        var icon = $(this);
        var personId = $(this).find('input[type=hidden]').val();
        $.ajax({
            type: 'POST',
            url: 'Tags/RemoveContactTagFromPerson?personId=' + personId,
            success: function (data) {
                if (data.success) {
                    icon.remove();
                }
            },
            error: function (xhr, status, error) {
                // Handle error
            }
        });
    });
});