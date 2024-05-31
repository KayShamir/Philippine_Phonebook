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
            var mobileNumber = $(this).data('mobile');
            if (confirm("Are you sure you want to mark this phone number as inactive?")) {
                $.ajax({
                    url: '../Home/SoftDeletePhoneNumber',
                    type: 'POST',
                    data: JSON.stringify({ mobileNumber: mobileNumber }),
                    contentType: 'application/json; charset=utf-8',
                    success: function (response) {
                        toastr.success(response);
                        $('#btn_search_num').click();
                    },
                    error: function (xhr, status, error) {
                        console.error("Error occurred: " + error);
                    }
                });
            }
        });

        $('button.btn_hard_delete').off('click').on('click', function () {
            var mobileNumber = $(this).data('mobile');
            if (confirm("Are you sure you want to delete this phone number?")) {
                $.ajax({
                    url: '../Home/HardDeletePhoneNumber',
                    type: 'POST',
                    data: JSON.stringify({ mobileNumber: mobileNumber }),
                    contentType: 'application/json; charset=utf-8',
                    success: function (response) {
                        toastr.success(response);
                        $('#btn_search_num').click();
                        setTimeout(function () {
                            location.reload();
                        }, 1000);
                    },
                    error: function (xhr, status, error) {
                        console.error("Error occurred: " + error);
                    }
                });
            }
        });
    }

    $(document).on('click', '.btn_update', function () {
        var name = $(this).data('name');
        var areaCode = $(this).data('areacode');
        var phoneNumber = $(this).data('phonenumber');
        var mobileNumber = $(this).data('mobilenumber');
        var email = $(this).data('email');
        var status = $(this).data('status');
        var house = $(this).data('house');
        var street = $(this).data('street');
        var city = $(this).data('city');
        var province = $(this).data('province');
        var zipCode = $(this).data('zipcode');

        $('#update_person_name').val(name);
        $('#update_area_code').val(areaCode);
        $('#update_phone_num').val(phoneNumber);
        $('#update_mobile_num').val(mobileNumber);
        $('#update_email_add').val(email);
        $('#update_status').val(status);
        $('#update_house_num').val(house);
        $('#update_street').val(street);
        $('#update_city').val(city);
        $('#update_province').val(province);
        $('#update_zip_code').val(zipCode);
        $('#oldMobileNumber').val(mobileNumber);

        $('#updateModal').modal('show');
    });

    $('#btnSubmitUpdate').click(function () {
        var oldMobileNumber = $('#oldMobileNumber').val();
        var newMobileNumber = $('#update_mobile_num').val();
        if (newMobileNumber) {
            $.ajax({
                url: '../Home/UpdatePhoneNumber',
                type: 'POST',
                data: JSON.stringify({ oldMobileNumber: oldMobileNumber, newMobileNumber: newMobileNumber }),
                contentType: 'application/json; charset=utf-8',
                success: function (response) {
                    toastr.success(response);
                    $('#updateModal').modal('hide');
                    setTimeout(function () {
                        location.reload();
                    }, 1000);
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
