$(document).ready(function () {
    $('#btn_search_num').click(function () {
        var search_num = $('#search_num').val();

        if (search_num) {
            $.ajax({
                url: '../Home/SearchPhonebook',
                type: 'POST',
                data: JSON.stringify({ phoneNumber: search_num }),
                contentType: 'application/json; charset=utf-8',
                success: function (response) {
                    $('#search_result').html(response);
                },
                error: function (xhr, status, error) {
                    console.error("Error occurred: " + error);
                }
            });
        }
    });
});