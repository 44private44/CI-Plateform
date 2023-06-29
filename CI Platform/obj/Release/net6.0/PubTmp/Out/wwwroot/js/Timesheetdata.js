//Timesheet hour submit

// time onblur validation
$("#timesheet_mission-select").blur(function () {

    var missionId = $("#timesheet_mission-select option:selected").val();

    if (missionId == "default") {
        $("#missionlabel").addClass("validatefield");
    }
    else {
        $("#missionlabel").removeClass("validatefield");
    }
});
$("#timesheethourdate").blur(function () {

    var missionId = $("#timesheethourdate").val();

    if (missionId == "") {
        $("#timesheethourdatelabel").addClass("validatefield");
    }
    else {
        $("#timesheethourdatelabel").removeClass("validatefield");
    }
});
$("#timesheethour").blur(function () {

    var missionId = $("#timesheethour").val();

    if (missionId == "") {
        $("#timesheethourlabel").addClass("validatefield");
    }
    else {
        $("#timesheethourlabel").removeClass("validatefield");
    }
});
$("#timesheetminute").blur(function () {

    var missionId = $("#timesheetminute").val();

    if (missionId == "") {
        $("#timesheetminutelabel").addClass("validatefield");
    }
    else {
        $("#timesheetminutelabel").removeClass("validatefield");
    }
});
$("#timesheetmessage").blur(function () {

    var missionId = $("#timesheetmessage").val();

    if (missionId == "") {
        $("#timesheetmessagelabel").addClass("validatefield");
    }
    else {
        $("#timesheetmessagelabel").removeClass("validatefield");
    }
});

//onsubmit validation 

$('#timesheetform').submit(function (event) {

    var missionId = $("#timesheet_mission-select option:selected").val();
    var date = $("#timesheethourdate").val();
    var hours = $("#timesheethour").val();
    var minute = $("#timesheetminute").val();
    var message = $("#timesheetmessage").val();

    if (missionId == "default") {

        $("#missionlabel").addClass("validatefield");
        alert("please select mission first !");
        return false;
    }
    else {
        $("#missionlabel").removeClass("validatefield");
    }
    if (!date || !hours || !minute || !message) {

        if (!date) $("#timesheethourdatelabel").addClass("validatefield");
        if (!hours) $("#timesheethourlabel").addClass("validatefield");
        if (!minute) $("#timesheetminutelabel").addClass("validatefield");
        if (!message) $("#timesheetmessagelabel").addClass("validatefield");

        alert("First fill all required data !");
        return false;
    }
    $("#timesheethourdatelabel").removeClass("validatefield");
    $("#timesheethourlabel").removeClass("validatefield");
    $("#timesheetminutelabel").removeClass("validatefield");
    $("#timesheetmessagelabel").removeClass("validatefield");


    $.ajax({
        type: "POST",
        url: "/Timesheet/timemissionsubmit",
        data: { missionId: missionId, date: date, hours: hours, minute: minute, message: message },
        success: function (response) {
            if (response.success) {


            }
        },
        error: function (response) {
            alert("Oops Something went wrong !");
        }

    });
    alert("Great Volunteering Hours Mission added successfully !");
    location.reload();

});


// date validate min and max set 

// Get the current date in yyyy-mm-dd format
const currentDate = new Date().toISOString().split('T')[0];

//Draft data get by the missionid

$('#timesheet_mission-select').change(function () {

    var choosemissionId = $("#timesheet_mission-select option:selected").val();

    $.ajax({
        type: "POST",
        url: "/Timesheet/Drafttimedata",
        data: { missionid: choosemissionId },
        success: function (response) {

            if (response.success) {
                var data = response.data;

                // create the date object with the timezone offset adjustment
                var date = new Date(data.date);
                var timezoneOffset = date.getTimezoneOffset();
                date.setMinutes(date.getMinutes() - timezoneOffset);

                // format the date and set the value of the date input field
                var day = date.getDate();
                var month = (date.getMonth() + 1).toString().padStart(2, '0');
                var year = date.getFullYear();
                var formattedDate = year + "-" + month + "-" + day.toString().padStart(2, '0');


                $("#timesheethourdate").val(formattedDate);
                $("#timesheethour").val(data.hour);
                $("#timesheetminute").val(data.minute);
                $("#timesheetmessage").val(data.message);

                // format the start date and set it to the variable in yyyy-mm-dd format
                var startDateObj = new Date(data.startdate);
                var formattedStartDate = startDateObj.toISOString().split('T')[0];

                document.getElementById('timesheethourdate').min = formattedStartDate;
                document.getElementById('timesheethourdate').max = currentDate;

                return false;

            }
            else {
                $("#timesheethourdate").val("");
                $("#timesheethour").val("");
                $("#timesheetminute").val("");
                $("#timesheetmessage").val("");
                // format the start date and set it to the variable in yyyy-mm-dd format
                var getstartdate = response.data;
                var formattedStartDate = new Date(getstartdate).toISOString().split('T')[0];

                document.getElementById('timesheethourdate').min = formattedStartDate;
                document.getElementById('timesheethourdate').max = currentDate;

                return false;
            }
        },
        error: function (response) {

        }

    });
});


//delete timesheet
let timesheetid = 0;
$(".timesheethourid").click(function () {

    timesheetid = $(this).data('timesheetid');

});

$(".timedelete").click(function () {
    $.ajax({
        type: "POST",
        url: "/Timesheet/DeleteTimesheetRecords",
        data: { timesheetid: timesheetid },
        success: function (response) {
            if (response.success) {

                // Show the success message in a pop-up
                swal({
                    title: "Success",
                    text: "Delete records successfully...",
                    icon: "success",
                    button: "OK",
                    timer: 2500,
                    closeOnClickOutside: false,
                    closeOnEsc: false,
                });

                // Delay reload for 2.5 seconds to give time for SweetAlert to be shown
                setTimeout(function () {
                    location.reload();
                }, 2600);
            }
            else {
                alert("Oops Something went wrong !");
            }
        },
        error: function (response) {
            alert("Oops Something went wrong !")
        }
    });
});

//Edit timesheet

$(".timesheethoureditid").click(function () {

    var timesheetid = $(this).data('timesheeteditid');

    $.ajax({
        type: "POST",
        url: "/Timesheet/Drafttimedata",
        data: { missionid: timesheetid },
        success: function (response) {

            if (response.success) {
                var data = response.data;

                // create the date object with the timezone offset adjustment
                var date = new Date(data.date);
                var timezoneOffset = date.getTimezoneOffset();
                date.setMinutes(date.getMinutes() - timezoneOffset);

                // format the date and set the value of the date input field
                var day = date.getDate();
                var month = (date.getMonth() + 1).toString().padStart(2, '0');
                var year = date.getFullYear();
                var formattedDate = year + "-" + month + "-" + day.toString().padStart(2, '0');

                $("#timesheethoureditdate").val(formattedDate);
                $("#timesheethouredit").val(data.hour);
                $("#timesheetminuteedit").val(data.minute);
                $("#timesheetmessageedit").val(data.message);

                // create a new option element with value and text
                var newOption = $("<option></option>");
                newOption.attr("value", data.missionid);
                newOption.text(data.missiontitle);
                newOption.attr("selected", "selected");

                // first empty then append new option
                $("#timesheet_mission-selectedit").empty().append(newOption);

                // format the start date and set it to the variable in yyyy-mm-dd format
                var startDateObj = new Date(data.startdate);
                var formattedStartDate = startDateObj.toISOString().split('T')[0];

                document.getElementById('timesheethoureditdate').min = formattedStartDate;
                document.getElementById('timesheethoureditdate').max = currentDate;


                return false;

            }
            else {
                $("#timesheethoureditdate").val("");
                $("#timesheethouredit").val("");
                $("#timesheetminuteedit").val("");
                $("#timesheetmessageedit").val("");
                // format the start date and set it to the variable in yyyy-mm-dd format
                var getstartdate = response.data;
                var formattedStartDate = new Date(getstartdate).toISOString().split('T')[0];

                document.getElementById('timesheethoureditdate').min = formattedStartDate;
                document.getElementById('timesheethoureditdate').max = currentDate;

                return false;
            }
        },
        error: function (response) {

        }

    });
});

//Edit onsubmit validation 

$('#timesheeteditform').submit(function (event) {

    var missionId = $("#timesheet_mission-selectedit option:selected").val();
    var date = $("#timesheethoureditdate").val();
    var hours = $("#timesheethouredit").val();
    var minute = $("#timesheetminuteedit").val();
    var message = $("#timesheetmessageedit").val();

    if (missionId == "default") {

        $("#missioneditlabel").addClass("validatefield");
        alert("please select mission first !");
        return false;
    }
    else {
        $("#missioneditlabel").removeClass("validatefield");
    }
    if (!date || !hours || !minute || !message) {

        if (!date) $("#timesheethoureditdatelabel").addClass("validatefield");
        if (!hours) $("#timesheethoureditlabel").addClass("validatefield");
        if (!minute) $("#timesheetminuteeditlabel").addClass("validatefield");
        if (!message) $("#timesheetmessageeditlabel").addClass("validatefield");

        alert("First fill all required data !");
        return false;
    }
    $("#timesheethoureditdatelabel").removeClass("validatefield");
    $("#timesheethoureditlabel").removeClass("validatefield");
    $("#timesheetminuteeditlabel").removeClass("validatefield");
    $("#timesheetmessageeditlabel").removeClass("validatefield");


    $.ajax({
        type: "POST",
        url: "/Timesheet/timemissionsubmit",
        data: { missionId: missionId, date: date, hours: hours, minute: minute, message: message },
        success: function (response) {
            if (response.success) {


            }
        },
        error: function (response) {
            alert("Oops Something went wrong !");
        }

    });
    alert("Great Volunteering Hours Mission edited successfully !");
    location.reload();
});


// --------------Goal-----------------

// Goal blur validation
$("#timesheet_goalmission-select").blur(function () {

    var missionId = $("#timesheet_goalmission-select option:selected").val();

    if (missionId == "default") {
        $("#goalmissionlabel").addClass("validatefield");
    }
    else {
        $("#goalmissionlabel").removeClass("validatefield");
    }
});
$("#timesheetaction").blur(function () {

    var ation = $("#timesheetaction").val();

    if (ation == "") {
        $("#timesheetactionlabel").addClass("validatefield");
    }
    else {
        $("#timesheetactionlabel").removeClass("validatefield");
    }
});
$("#timesheetgoaldate").blur(function () {

    var goaldate = $("#timesheetgoaldate").val();

    if (goaldate == "") {
        $("#timesheetgoaldatelabel").addClass("validatefield");
    }
    else {
        $("#timesheetgoaldatelabel").removeClass("validatefield");
    }
});
$("#timesheetgoalmessage").blur(function () {

    var goalmessage = $("#timesheetgoalmessage").val();

    if (goalmessage == "") {
        $("#timesheetgoalmessagelabel").addClass("validatefield");
    }
    else {
        $("#timesheetgoalmessagelabel").removeClass("validatefield");
    }
});

//onsubmit goal validation 

$('#timesheetgoalform').submit(function (event) {

    var missionId = $("#timesheet_goalmission-select option:selected").val();
    var action = $("#timesheetaction").val();
    var date = $("#timesheetgoaldate").val();
    var message = $("#timesheetgoalmessage").val();

    if (missionId == "default") {

        $("#goalmissionlabel").addClass("validatefield");
        alert("please select mission first !");
        return false;
    }
    else {
        $("#goalmissionlabel").removeClass("validatefield");
    }
    if (!date || !action || !message) {

        if (!date) $("#timesheetgoaldatelabel").addClass("validatefield");
        if (!action) $("#timesheetactionlabel").addClass("validatefield");
        if (!message) $("#timesheetgoalmessagelabel").addClass("validatefield");

        alert("First fill all required data !");
        return false;
    }
    $("#timesheetgoaldatelabel").removeClass("validatefield");
    $("#timesheetactionlabel").removeClass("validatefield");
    $("#timesheetgoalmessagelabel").removeClass("validatefield");


    $.ajax({
        type: "POST",
        url: "/Timesheet/goalmissionsubmit",
        data: { missionId: missionId, date: date, action: action, message: message },
        success: function (response) {
            if (response.success) {


            }
        },
        error: function (response) {
            if (response.error) {

                alert("Oops Something went wrong !");
            }
        }

    });
    alert("Great Volunteering Goals Mission added successfully !");
    location.reload();
});

//Draft data get by the missionid in goal based

$('#timesheet_goalmission-select').change(function () {

    var choosemissionId = $("#timesheet_goalmission-select option:selected").val();

    $.ajax({
        type: "POST",
        url: "/Timesheet/Drafttimedata",
        data: { missionid: choosemissionId },
        success: function (response) {

            if (response.success) {
                var data = response.data;
                // create the date object with the timezone offset adjustment
                var date = new Date(data.date);
                var timezoneOffset = date.getTimezoneOffset();
                date.setMinutes(date.getMinutes() - timezoneOffset);

                // format the date and set the value of the date input field
                var day = date.getDate();
                var month = (date.getMonth() + 1).toString().padStart(2, '0');
                var year = date.getFullYear();
                var formattedDate = year + "-" + month + "-" + day.toString().padStart(2, '0');

                $("#timesheetgoaldate").val(formattedDate);
                $("#timesheetgoalmessage").val(data.message);
                $("#timesheetaction").val(data.action);

                // format the start date and set it to the variable in yyyy-mm-dd format
                var startDateObj = new Date(data.startdate);
                var formattedStartDate = startDateObj.toISOString().split('T')[0];

                document.getElementById('timesheetgoaldate').min = formattedStartDate;
                document.getElementById('timesheetgoaldate').max = currentDate;


                return false;

            }
            else {
                $("#timesheethourdate").val("");
                $("#timesheethour").val("");
                $("#timesheetminute").val("");
                $("#timesheetmessage").val("");
                // format the start date and set it to the variable in yyyy-mm-dd format
                var getstartdate = response.data;
                var formattedStartDate = new Date(getstartdate).toISOString().split('T')[0];

                document.getElementById('timesheetgoaldate').min = formattedStartDate;
                document.getElementById('timesheetgoaldate').max = currentDate;

                return false;
            }
        },
        error: function (response) {
            $("#timesheetgoaldate").val("");
            $("#timesheetgoalmessage").val("");
            // format the start date and set it to the variable in yyyy-mm-dd format
            var getstartdate = response.data;
            var formattedStartDate = new Date(getstartdate).toISOString().split('T')[0];

            document.getElementById('timesheetgoaldate').min = formattedStartDate;
            document.getElementById('timesheetgoaldate').max = currentDate;

            return false;
        }

    });
});

// edit goal mission

$(".timesheetgoaleditid").click(function () {

    var timesheetid = $(this).data('timesheeteditid');

    $.ajax({
        type: "POST",
        url: "/Timesheet/Drafttimedata",
        data: { missionid: timesheetid },
        success: function (response) {

            if (response.success) {
                var data = response.data;

                // create the date object with the timezone offset adjustment
                var date = new Date(data.date);
                var timezoneOffset = date.getTimezoneOffset();
                date.setMinutes(date.getMinutes() - timezoneOffset);

                // format the date and set the value of the date input field
                var day = date.getDate();
                var month = (date.getMonth() + 1).toString().padStart(2, '0');
                var year = date.getFullYear();
                var formattedDate = year + "-" + month + "-" + day.toString().padStart(2, '0');

                $("#timesheetgoaldateedit").val(formattedDate);
                $("#timesheetgoalmessageedit").val(data.message);
                $("#timesheetactionedit").val(data.action);

                // create a new option element with value and text
                var newOption = $("<option></option>");
                newOption.attr("value", data.missionid);
                newOption.text(data.missiontitle);
                newOption.attr("selected", "selected");

                // first empty then append new option
                $("#timesheet_goaleditmission-select").empty().append(newOption);

                // format the start date and set it to the variable in yyyy-mm-dd format
                var startDateObj = new Date(data.startdate);
                var formattedStartDate = startDateObj.toISOString().split('T')[0];

                document.getElementById('timesheetgoaldateedit').min = formattedStartDate;
                document.getElementById('timesheetgoaldateedit').max = currentDate;


                return false;

            }
            else {
                $("#timesheetgoaldateedit").val("");
                $("#timesheetgoalmessageedit").val("");
                // format the start date and set it to the variable in yyyy-mm-dd format
                var getstartdate = response.data;
                var formattedStartDate = new Date(getstartdate).toISOString().split('T')[0];

                document.getElementById('timesheetgoaldateedit').min = formattedStartDate;
                document.getElementById('timesheetgoaldateedit').max = currentDate;

                return false;
            }
        },
        error: function (response) {

        }

    });
});
$('#timesheetgoaleditform').submit(function (event) {

    var missionId = $("#timesheet_goaleditmission-select option:selected").val();
    var date = $("#timesheetgoaldateedit").val();
    var action = $("#timesheetactionedit").val();
    var message = $("#timesheetgoalmessageedit").val();

    if (missionId == "default") {

        $("#goalmissionlabel").addClass("validatefield");
        alert("please select mission first !");
        return false;
    }
    else {
        $("#goalmissionlabel").removeClass("validatefield");
    }
    if (!date || !action || !message) {

        if (!date) $("#timesheetgoaldateeditlabel").addClass("validatefield");
        if (!action) $("#timesheetactioneditlabel").addClass("validatefield");
        if (!message) $("#timesheetgoalmessageeditlabel").addClass("validatefield");

        alert("First fill all required data !");
        return false;
    }
    $("#timesheetgoaldateeditlabel").removeClass("validatefield");
    $("#timesheetactioneditlabel").removeClass("validatefield");
    $("#timesheetgoalmessageeditlabel").removeClass("validatefield");


    $.ajax({
        type: "POST",
        url: "/Timesheet/goalmissionsubmit",
        data: { missionId: missionId, date: date, action: action, message: message },
        success: function (response) {
            if (response.success) {


            }
        },
        error: function (response) {
            if (response.error) {

                alert("Oops Something went wrong !");
            }
        }

    });
    alert("Great Volunteering Goals Mission edited successfully !");
    location.reload();
});
