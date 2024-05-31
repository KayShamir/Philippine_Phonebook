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
                    attachButtonHandlers(); // Attach event handlers to the new buttons
                },
                error: function (xhr, status, error) {
                    console.error("Error occurred: " + error);
                }
            });
        }
    });

    function attachButtonHandlers() {
        $('button.btn_update').off('click').on('click', function () {
            var oldMobileNumber = $(this).data('mobile');
            $('#oldMobileNumber').val(oldMobileNumber);
            $('#updateModal').modal('show'); // Ensure modal is initialized with correct method
        });

        $('button.btn_soft_delete').off('click').on('click', function () {
            var phoneNumber = $(this).data('mobile');
            if (confirm("Are you sure you want to mark this phone number as inactive?")) {
                $.ajax({
                    url: '../Home/SoftDeletePhoneNumber',
                    type: 'POST',
                    data: JSON.stringify({ mobileNumber: phoneNumber }),
                    contentType: 'application/json; charset=utf-8',
                    success: function (response) {
                        toastr.success(response);
                        // Optionally refresh the search results
                        $('#btn_search_num').click();
                    },
                    error: function (xhr, status, error) {
                        console.error("Error occurred: " + error);
                    }
                });
            }
        });

        $('button.btn_hard_delete').off('click').on('click', function () {
            var phoneNumber = $(this).data('mobile');
            if (confirm("Are you sure you want to delete this phone number?")) {
                $.ajax({
                    url: '../Home/HardDeletePhoneNumber',
                    type: 'POST',
                    data: JSON.stringify({ mobileNumber: phoneNumber }),
                    contentType: 'application/json; charset=utf-8',
                    success: function (response) {
                        toastr.success(response);
                        // Optionally refresh the search results
                        $('#btn_search_num').click();
                    },
                    error: function (xhr, status, error) {
                        console.error("Error occurred: " + error);
                    }
                });
            }
        });
    }

    $('#btnSubmitUpdate').click(function () {
        var oldMobileNumber = $('#oldMobileNumber').val();
        var newMobileNumber = $('#newMobileNumber').val();
        if (newMobileNumber) {
            $.ajax({
                url: '../Home/UpdatePhoneNumber',
                type: 'POST',
                data: JSON.stringify({ oldMobileNumber: oldMobileNumber, newMobileNumber: newMobileNumber }),
                contentType: 'application/json; charset=utf-8',
                success: function (response) {
                    toastr.success(response);
                    $('#updateModal').modal('hide');
                },
                error: function (xhr, status, error) {
                    console.error("Error occurred: " + error);
                }
            });
        } else {
            toastr.error("Please enter a new mobile number.");
        }
    });
});
