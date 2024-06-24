function FilterStateDivisionByTownship(lstStateDivisionCtrl, lstTownshipId) {

    var lstTownships = $("#" + lstTownshipId);
    lstTownships.empty();

    var selectedStateDivision = lstStateDivisionCtrl.options[lstStateDivisionCtrl.selectedIndex].value;

    if (selectedStateDivision != null && selectedStateDivision != '') {
        $.getJSON("/Login/GetTownshipByStateDivision", { stateDivisionPkId: selectedStateDivision }, function (townships) {
            if (townships != null && !jQuery.isEmptyObject(townships)) {
                $.each(townships, function (index, township) {
                    lstTownships.append($('<option/>',
                        {
                            value: township.value,
                            text: township.text
                        }));
                });
            };
        });
    }

    return;
}


function FilterNRCTownshipInitial(lstNRCTownshipNumberCtrl, lstNRCTownshipInitialId) {
    console.log("here ............................nrc townshp initial")
    var lstNRCTownshipInitials = $("#" + lstNRCTownshipInitialId);
    lstNRCTownshipInitials.empty();

    var selectedNRCTownshipNumber = lstNRCTownshipNumberCtrl.options[lstNRCTownshipNumberCtrl.selectedIndex].value;
    console.log("township number................" + selectedNRCTownshipNumber);
    if (selectedNRCTownshipNumber != null && selectedNRCTownshipNumber != '') {
        $.getJSON("/Login/GetNRCTownshipInitials", { nrcTownshipNumber: selectedNRCTownshipNumber }, function (townships) {
            if (townships != null && !jQuery.isEmptyObject(townships)) {
                $.each(townships, function (index, township) {
                    lstNRCTownshipInitials.append($('<option/>',
                        {
                            value: township,
                            text: township
                        }));
                });
            };
        });
    }

    return;
}

function FilterNRCTownshipInitial1(lstNRCTownshipNumberCtrl1, lstNRCTownshipInitialId1) {
    var lstNRCTownshipInitials1 = $("#" + lstNRCTownshipInitialId1);
    lstNRCTownshipInitials1.empty();

    var selectedNRCTownshipNumber1 = lstNRCTownshipNumberCtrl1.options[lstNRCTownshipNumberCtrl1.selectedIndex].value;

    if (selectedNRCTownshipNumber1 != null && selectedNRCTownshipNumber1 != '') {
        $.getJSON("/Login/GetNRCTownshipInitials", { nrcTownshipNumber: selectedNRCTownshipNumber1 }, function (townships) {
            if (townships != null && !jQuery.isEmptyObject(townships)) {
                $.each(townships, function (index, township) {
                    lstNRCTownshipInitials1.append($('<option/>',
                        {
                            value: township ,
                            text: township
                        }));
                });
            };
        });
    }

    return;
}

function FilterStateDivision(lstStateDivisionCtrl, lstNRCTownshipId) {

    var lstNRCTownshipInitials = $("#" + lstNRCTownshipId);
    lstNRCTownshipInitials.empty();

    var selectedStateDivision = lstStateDivisionCtrl.options[lstStateDivisionCtrl.selectedIndex].value;

    if (selectedStateDivision != null && selectedStateDivision != '') {
        $.getJSON("/PersonalDetail/GetTownships", { stateDivisionCode: selectedStateDivision }, function (townships) {
            if (townships != null && !jQuery.isEmptyObject(townships)) {
                $.each(townships, function (index, township) {
                    lstNRCTownshipInitials.append($('<option/>',
                        {
                            value:  township.townshipPkid,
                            text: "(" + township.townshipName + ")"
                        }));
                });
            };
        });
    }

    return;
}