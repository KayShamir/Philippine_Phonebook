
    $().ready(function () {
        $('#btn_submit').click(function (e) {
            e.preventDefault();

            var phoneNumber = $('#mobile_num').val();

            $.ajax({
                url: '/Home/IsPhoneNumberUnique',
                type: 'POST',
                data: { phoneNumber: phoneNumber },
                success: function (isUnique) {
                    if (isUnique) {
                        var data = new FormData();
                        data.append('name', $('#person_name').val());
                        data.append('area_code', $('#area_code').val());
                        data.append('phone_num', $('#phone_num').val());
                        data.append('mobile_num', $('#mobile_num').val());
                        data.append('email_add', $('#email_add').val());
                        data.append('house_num', $('#house_num').val());
                        data.append('street', $('#street').val());
                        data.append('city', $('#city').val());
                        data.append('province', $('#province').val());
                        data.append('zip_code', $('#zip_code').val());
                        data.append('status', $('#status').val());

                        $.ajax({
                            url: '../Home/PostCreate',
                            type: 'POST',
                            data: data,
                            processData: false,
                            contentType: false,
                            success: function (data) {
                                if (data.success) {
                                    toastr.success(data.message);
                                    $('#addModal').modal('hide');
                                    setTimeout(function () {
                                        location.reload();
                                    }, 1000);
                                } else {
                                    toastr.error(data.message);
                                }
                            },
                            error: function () {
                                toastr.error('Error uploading data.');
                            }
                        });
                    } else {
                        toastr.error('Phone number already exists. Please enter a unique phone number.');
                    }
                },
                error: function () {
                    toastr.error('Error checking phone number uniqueness.');
                }
            });
        });
});
