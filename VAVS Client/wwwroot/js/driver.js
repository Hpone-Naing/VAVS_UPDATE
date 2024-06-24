function FillDriverLicenseByDriverId(lstDriverName, driverLicenseId) {

    var driverLicenseTestBox = $("#" + driverLicenseId);
    driverLicenseTestBox.empty();
    var selectedDriverId = lstDriverName.options[lstDriverName.selectedIndex].value;

    if (selectedDriverId != null && selectedDriverId != '') {
        $.getJSON("/IRD_VAVS_Client/YBORecord/GetDriverLicenseByDriverId", { driverPkId: selectedDriverId }, function (driverLicense) {
            driverLicenseTestBox.val(driverLicense);
        });
    }

    return;
}

function FillDriverInfoByDriverId(lstDriverName) {

    var driverLicenseTestBox = $("#driverLicense");
    var driverAgeTestBox = $("#age");
    var driverAddressTestBox = $("#address");
    var driverFatherNameTestBox = $("#fatherName");
    var driverPhoneTestBox = $("#phone");
    var driverEducationLevelTestBox = $("#educationLevel");
    driverLicenseTestBox.val("");
    driverAgeTestBox.val("");
    driverAddressTestBox.val("");
    driverFatherNameTestBox.val("");
    driverPhoneTestBox.val("");
    driverEducationLevelTestBox.val("");
    var selectedDriverId = lstDriverName.options[lstDriverName.selectedIndex].value;

    if (selectedDriverId != null && selectedDriverId != '') {
        $.getJSON("/YBSDriverCourseDelivery/GetDriverInfoByDriverId", { driverPkId: selectedDriverId }, function (data) {
            var license = data.license;
            var trainedDriverInfo = data.trainedDriverInfo;

            driverLicenseTestBox.val(license);

            if (trainedDriverInfo != null) {
                driverAgeTestBox.val(trainedDriverInfo.age);
                driverAddressTestBox.val(trainedDriverInfo.address);
                driverFatherNameTestBox.val(trainedDriverInfo.fatherName);
                driverPhoneTestBox.val(trainedDriverInfo.phone);
                driverEducationLevelTestBox.val(trainedDriverInfo.educationLevel);
            }
        });
    }

    return;
}

$(document).ready(function () {
    $('#add').click(function () {
        $('#lstDriverName').prop('selectedIndex', 0);
        $('#lstDriverNameDiv').hide();
        $('#add').hide();
        $('#driverLicenseLbl').show();
        $('#driverNameLbl').show();
        $('#ageLbl').show();
        $('#addressLbl').show();
        $('#fatherNameLbl').show();
        $('#educationLevelLbl').show();
        $('#phoneLbl').show();

        $('#newDriverDiv').show();
        $('#remove').show();
        $('#driverLicense').prop('readonly', false);
        $('#driverLicense').val('');
        $('#newDriverName').val('');
        $('#address').val('');
        $('#age').val('');
        $('#phone').val('');
        $('#educationLevel').val('');
        $('#fatherName').val('');
    });

    $('#remove').click(function () {
        $('#lstDriverNameDiv').show();
        $('#lstDriverName').prop('selectedIndex', 0);
        $('#add').show();
        $('#newDriverName').val('');
        $('#newDriverDiv').hide();
        $('#remove').hide();
        $('#driverLicenseLbl').hide();
        $('#driverNameLbl').hide();
        $('#ageLbl').hide();
        $('#addressLbl').hide();
        $('#fatherNameLbl').hide();
        $('#educationLevelLbl').hide();
        $('#phoneLbl').hide();
        $('#driverLicense').prop('readonly', true);
    });
});
