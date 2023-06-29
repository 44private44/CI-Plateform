// update the time in every second

function updateTime() {
    const now = new Date();
    const formattedTime = now.toLocaleString("en-US", { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric', hour: 'numeric', minute: 'numeric', hour12: false });
    $("#current-time").text(formattedTime);
}

updateTime(); // Call the function once to display the current time
setInterval(updateTime, 1000); // Call the function every second using setInterval

//------------------------------search-------------------------------------------

// search field 

$(".admin_search").on("keyup", function () {
    var query = $(this).val().toLowerCase().trim();
    filterTable(query);
});

// it call when remove all string by X button
$(".admin_search").on("search", function () {
    var query = $(this).val().toLowerCase().trim();
    filterTable(query);
});

// when user click Esc key ASCII value Esc = 27
$(".admin_search").on("keyup", function (event) {
    if (event.keyCode === 27) {
        $(this).val("");
        filterTable("");
    }
});

function filterTable(query) {
    $('tbody tr').each(function () {
        var $row = $(this);
        if ($row.text().toLowerCase().indexOf(query) === -1 && query != "") {
            $row.hide();
        } else {
            $row.show();
        }
    });
}

//--------------------------User--------------------------------------------------

// submit validate data

$("#userAdminprofilename").blur(function () {
    var name = $("#userAdminprofilename").val();

    if (name != "") {
        $("#userAdminvalidname").removeClass("validatefield");
        return false;
    }
    else {
        $("#userAdminvalidname").addClass("validatefield");
        return false;
    }
});
$("#userAdminprofilesurname").blur(function () {
    var sname = $("#userAdminprofilesurname").val();

    if (sname != "") {
        $("#userAdminvalidsname").removeClass("validatefield");
        return false;
    }
    else {
        $("#userAdminvalidsname").addClass("validatefield");
        return false;
    }
});
$("#userAdminprofiletext").blur(function () {
    var profiletext = $("#userAdminprofiletext").val();

    if (profiletext != "") {
        $("#userAdminvalidprofile").removeClass("validatefield");
        return false;
    }
    else {
        $("#userAdminvalidprofile").addClass("validatefield");
        return false;
    }
});
$("#userAdminprofileemail").blur(function () {
    var email = $("#userAdminprofileemail").val();

    if (email != "") {
        $("#userAdminvalidemail").removeClass("validatefield");
        $.ajax({
            url: '/Admin/UserEmailValidate',
            type: 'POST',
            data: { email: email },
            success: function (result) {
                if (result.success) {
                    $("#userAdminValidateEmail").addClass("matchpassworddiv");
                }
                else {
                    $("#userAdminValidateEmail").removeClass("matchpassworddiv");
                }
            },
            error: function (error) {
                return false;
            }
        });
        return false;
    }
    else {
        $("#userAdminvalidemail").addClass("validatefield");
        return false;
    }
});
$("#userAdminprofilepassword").blur(function () {
    var password = $("#userAdminprofilepassword").val();
    if (password != "") {
        $("#userAdminprofilepasswordLabel").removeClass("validatefield");
        // 8 character validate
        if (password.length < 8) {
            $("#usernewpasswordAdminmatchAdmin").removeClass("matchpassworddiv");
            return false;
        }
        $("#usernewpasswordAdminmatchAdmin").addClass("matchpassworddiv");
        return false;
    }
    else {
        $("#userAdminprofilepasswordLabel").addClass("validatefield");
        return false;
    }
});
$("#submituseradmindata").click(function () {

    var name = $("#userAdminprofilename").val();
    var sname = $("#userAdminprofilesurname").val();
    var email = $("#userAdminprofileemail").val();
    var password = $("#userAdminprofilepassword").val();
    var profiletext = $("#userAdminprofiletext").val();
    var Country = $(".addUserAdminCountry").val();

    if (!name || !sname || !profiletext || !email || !password || !Country) {

        if (!name) $("#userAdminvalidname").addClass("validatefield");
        if (!sname) $("#userAdminvalidsname").addClass("validatefield");
        if (!email) $("#userAdminvalidemail").addClass("validatefield");
        if (!password) $("#userAdminprofilepasswordLabel").addClass("validatefield");
        if (!profiletext) $("#userAdminvalidprofile").addClass("validatefield");
        if (!Country) { $("#addUserAdminCountry").addClass("validatefield"); }
        else {
            $("#addUserAdminCountry").removeClass("validatefield");
        }
        alert("please first fill required data !");
        return false;
    }

    // Regular expression for email validation
    var emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

    // Check if email is valid
    if (!emailRegex.test(email)) {
        $("#userAdminvalidemail").addClass("validatefield");
        alert("Please enter a valid email address.");
        return false;
    }

    if (password != "") {
        // 8 character validate
        if (password.length < 8) {
            $("#usernewpasswordmatchAdmin").removeClass("matchpassworddiv");
            alert(" Password must be atleast 8 characters !");
            return false;
        }
        $("#usernewpasswordmatchAdmin").addClass("matchpassworddiv");
    }

    $("#uaevalidpassword").addClass("validatefield");
    $("#userAdminvalidname").removeClass("validatefield");
    $("#userAdminvalidsname").removeClass("validatefield");
    $("#userAdminvalidemail").removeClass("validatefield");
    $("#userAdminprofilepasswordLabel").removeClass("validatefield");
    $("#addUserAdminCountry").removeClass("validatefield");
    $("#userAdminvalidprofile").removeClass("validatefield");
});

//Form add new user submited
$(document).ready(function () {
    $("#addUserAdminForm").submit(function (event) {
        event.preventDefault();
        $.ajax({
            type: "POST",
            url: "/Admin/AddUserAdminProfile",
            data: $(this).serialize(),
            success: function (data) {
                if (data.success) {
                    // Display success message and redirect to page
                    window.location.href = "/Admin/UserAdmin";
                } else {
                    // Display error message
                    $("#userValidateEmail2").removeClass("matchpassworddiv");
                    $("#addUserAdminForm").append("<div class='alert alert-danger'>" + data.message + "</div>");
                    setTimeout(function () {
                        $(".alert-danger").fadeOut("slow", function () {
                            $(this).remove();
                        });
                    }, 5000);
                }
            }
        });
    });
});


//getcities based on countryid
$("#country-select8").change(function () {
    var countryId = $("#country-select8 option:selected").val();

    $.ajax({
        url: "/UserRecords/GetCitiesByCountry",
        type: "GET",
        data: { countryId: countryId },
        success: function (response) {
            var citySelect = $("#city-select8");
            citySelect.empty();
            var newOption = $('<option>', {
                value: '0',
                text: 'Select your city'
            });
            citySelect.append(newOption);
            $.each(response, function (i, city) {
                citySelect.append($('<option>', {
                    value: city.cityId,
                    text: city.cityName
                }));
            });
        },
        error: function (response) {
            console.log(response);
        }
    });
});
$("#country-select5").change(function () {
    var countryId = $("#country-select5 option:selected").val();

    $.ajax({
        url: "/UserRecords/GetCitiesByCountry",
        type: "GET",
        data: { countryId: countryId },
        success: function (response) {
            var citySelect = $("#city-select5");
            citySelect.empty();
            var newOption = $('<option>', {
                value: '0',
                text: 'Select your city'
            });
            citySelect.append(newOption);
            $.each(response, function (i, city) {
                citySelect.append($('<option>', {
                    value: city.cityId,
                    text: city.cityName
                }));
            });
        },
        error: function (response) {
            console.log(response);
        }
    });
});
$("#country-select44").change(function () {
    var countryId = $("#country-select44 option:selected").val();

    $.ajax({
        url: "/UserRecords/GetCitiesByCountry",
        type: "GET",
        data: { countryId: countryId },
        success: function (response) {
            var citySelect = $("#city-select44");
            citySelect.empty();
            var newOption = $('<option>', {
                value: '0',
                text: 'Select your city'
            });
            citySelect.append(newOption);
            $.each(response, function (i, city) {
                citySelect.append($('<option>', {
                    value: city.cityId,
                    text: city.cityName
                }));
            });
        },
        error: function (response) {
            console.log(response);
        }
    });
});
$("#country-select33").change(function () {
    var countryId = $("#country-select33 option:selected").val();

    $.ajax({
        url: "/UserRecords/GetCitiesByCountry",
        type: "GET",
        data: { countryId: countryId },
        success: function (response) {
            var citySelect = $("#city-select33");
            citySelect.empty();
            var newOption = $('<option>', {
                value: '0',
                text: 'Select your city'
            });
            citySelect.append(newOption);
            $.each(response, function (i, city) {
                citySelect.append($('<option>', {
                    value: city.cityId,
                    text: city.cityName
                }));
            });
        },
        error: function (response) {
            console.log(response);
        }
    });
});
//-------------------------------------------------------------------------------

let Userid = 0;
// Edit Admin User Draft data
$(".UserEditAdminButton").click(function () {

    Userid = $(this).data('userid');

    $.ajax({
        url: '/Admin/DraftUserAdminShow',
        type: 'POST',
        data: { Userid: Userid },
        success: function (result) {

            if (result.success) {
                var data = result.userdata;
                $("#userAdminEditName").val(data.firstname);
                $("#userAdminEditSname").val(data.lastname);
                $("#userAdminEditEmail").val(data.email);
                $("#userAdminEditPassword").val(data.password);
                $("#userAdminEditEmpid").val(data.empid);
                $("#userAdminEditTitle").val(data.title);
                $("#userAdminEditManager").val(data.managerdetails);
                $("#userAdminEditDept").val(data.department);
                $("#userAdminEditVolunteer").val(data.whyivol);
                //$("#city-select8").val(data.cityidvalue);
                $("#country-select8").val(data.countryidvalue);

                var cityid = data.cityidvalue;
                var countryId = data.countryidvalue;
                $.ajax({
                    url: "/UserRecords/GetCitiesByCountry",
                    type: "GET",
                    data: { countryId: countryId },
                    success: function (response) {
                        var citySelect = $("#city-select8");
                        if (cityid != 0) {
                            citySelect.empty();
                            $.each(response, function (i, city) {
                                citySelect.append($('<option>', {
                                    value: city.cityId,
                                    text: city.cityName
                                }));
                            });
                            $("#city-select8").val(cityid);
                        }
                        else {
                            citySelect.empty();
                            var newOption = $('<option>', {
                                value: '0',
                                text: 'Select your city'
                            });
                            citySelect.append(newOption);
                            $.each(response, function (i, city) {
                                citySelect.append($('<option>', {
                                    value: city.cityId,
                                    text: city.cityName
                                }));
                            });
                        }
                    },
                    error: function (response) {
                        console.log(response);
                    }
                });

                $("#UserAdminEditAva").val(data.availability);
                $("#userAdminEditProfile").val(data.profiletext);
                $("#userAdminEditLinkedin").val(data.linkedin);
                var status = data.status;

                if (status == 1) {
                    $('#useractive').prop('checked', true);
                }
                else {
                    $('#userinactive').prop('checked', true);
                }
            }
            else {
                alert("Something Went Wrong !");
            }
        },
        error: function (error) {
            console.log("error");
        }
    });
});

// submit validate Edit data
$("#userAdminEditName").blur(function () {
    var name = $("#userAdminEditName").val();

    if (name != "") {
        $("#userAdminEditNameLabel").removeClass("validatefield");
        return false;
    }
    else {
        $("#userAdminEditNameLabel").addClass("validatefield");
        return false;
    }
});
$("#userAdminEditSname").blur(function () {
    var sname = $("#userAdminEditSname").val();

    if (sname != "") {
        $("#userAdminEditSnameLabel").removeClass("validatefield");
        return false;
    }
    else {
        $("#userAdminEditSnameLabel").addClass("validatefield");
        return false;
    }
});
$("#userAdminEditProfile").blur(function () {
    var profile = $("#userAdminEditProfile").val();

    if (profile != "") {
        $("#userAdminEditProfileLabel").removeClass("validatefield");
        return false;
    }
    else {
        $("#userAdminEditProfileLabel").addClass("validatefield");
        return false;
    }
});

$("#SubmitAdminUserEditData").click(function () {

    var name = $("#userAdminEditName").val();
    var sname = $("#userAdminEditSname").val();
    var profiletext = $("#userAdminEditProfile").val();
    var country = $(".editUserAdminCountry").val();

    if (!name || !sname || !profiletext || country == 0) {

        if (!name) $("#userAdminEditNameLabel").addClass("validatefield");
        if (!sname) $("#userAdminEditSnameLabel").addClass("validatefield");
        if (!profiletext) $("#userAdminEditProfileLabel").addClass("validatefield");
        if (country == 0) { $("#editUserAdminCountryLabel").addClass("validatefield"); }
        else {
            $("#editUserAdminCountryLabel").removeClass("validatefield");
        }
        alert("please first fill required data !");
        return false;
    }

    $("#userAdminEditNameLabel").removeClass("validatefield");
    $("#userAdminEditSnameLabel").removeClass("validatefield");
    $("#userAdminEditProfileLabel").removeClass("validatefield");

});


//Form Update user submited
$(document).ready(function () {
    $("#EditUserAdminForm").submit(function (event) {
        event.preventDefault();
        $.ajax({
            type: "POST",
            url: "/Admin/UpdateUserAdminProfile",
            data: $(this).serialize() + "&Userid=" + Userid,
            success: function (data) {
                if (data.success) {
                    // Display success message and redirect to page
                    window.location.href = "/Admin/UserAdmin";
                } else {
                    // Display error message
                    $("#userValidateEmail2").removeClass("matchpassworddiv");
                    $("#EditUserAdminForm").append("<div class='alert alert-danger'>" + data.message + "</div>");
                    setTimeout(function () {
                        $(".alert-danger").fadeOut("slow", function () {
                            $(this).remove();
                        });
                    }, 4000);
                }
            }
        });
    });
});

// Password hide and show
const passwordInput = $("#userAdminEditPassword");
const eyeIcon = $("#eyeIcon");
eyeIcon.on("click", function () {
    if (passwordInput.attr("type") === "password") {
        passwordInput.attr("type", "text");
        eyeIcon.removeClass("fa-eye-slash").addClass("fa-eye");
    } else {
        passwordInput.attr("type", "password");
        eyeIcon.removeClass("fa-eye").addClass("fa-eye-slash");
    }
});
const passwordInput2 = $("#userAdminprofilepassword");
const eyeIcon2 = $("#eyeIcon2");
eyeIcon2.on("click", function () {
    if (passwordInput2.attr("type") === "password") {
        passwordInput2.attr("type", "text");
        eyeIcon2.removeClass("fa-eye-slash").addClass("fa-eye");
    } else {
        passwordInput2.attr("type", "password");
        eyeIcon2.removeClass("fa-eye").addClass("fa-eye-slash");
    }
});

// soft delete user records

let deleteUserid = 0;
$(".UserDeleteAdminButton").click(function () {

    deleteUserid = $(this).data('userid');

});

$(".userDelete").click(function () {
    $.ajax({
        type: "POST",
        url: "/Admin/DeleteUserRecords",
        data: { deleteUserid: deleteUserid },
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


//--------------------------Skills---------------------------------
// Skills Admin
$("#SkillAdminName").blur(function () {
    var skillname = $("#SkillAdminName").val();

    if (skillname != "") {
        $("#SkillAdminNameLabel").removeClass("validatefield");
        return false;
    }
    else {
        $("#SkillAdminNameLabel").addClass("validatefield");
        return false;
    }
});

$("#SkillsAdminSave").click(function () {

    var skillname = $("#SkillAdminName").val();

    if (skillname != "") {
        $("#SkillAdminNameLabel").removeClass("validatefield");
    }
    else {
        $("#SkillAdminNameLabel").addClass("validatefield");

        alert("please first fill required data !");
        return false;
    }
});
//Form add new user submited
$(document).ready(function () {
    $("#AddSkillsAdmin").submit(function (event) {
        event.preventDefault();
        $.ajax({
            type: "POST",
            url: "/Admin/AddSkillAdmin",
            data: $(this).serialize(),
            success: function (data) {
                if (data.success) {
                    // Display success message and redirect to page
                    window.location.href = "/Admin/MissionSkillsAdmin";
                } else {
                    // Display error message
                    $("#skillmesssageshow").append("<div class='alert alert-danger'>" + data.message + "</div>");
                    setTimeout(function () {
                        $(".alert-danger").fadeOut("slow", function () {
                            $(this).remove();
                        });
                    }, 5000);
                }
            }
        });
    });
});

// draft skill data
let Skillid = 0;
// Edit Admin skill Draft data
$(".SkillEditButton").click(function () {

    Skillid = $(this).data('skillid');

    $.ajax({
        url: '/Admin/DraftskillAdminShow',
        type: 'POST',
        data: { Skillid: Skillid },
        success: function (result) {

            if (result.success) {
                var data = result.skilldata;

                $("#SkillAdminNameedit").val(data.skillname);
                var status = data.status;

                if (status == 1) {
                    $('#skillactive').prop('checked', true);
                }
                else {
                    $('#skillinactive').prop('checked', true);
                }
            }
            else {
                alert("Something Went Wrong !");
            }
        },
        error: function (error) {
            console.log("error");
        }
    });
});
// edit skill form 
$("#SkillAdminNameedit").blur(function () {
    var skillname = $("#SkillAdminNameedit").val();

    if (skillname != "") {
        $("#SkillAdminNameeditLabel").removeClass("validatefield");
        return false;
    }
    else {
        $("#SkillAdminNameeditLabel").addClass("validatefield");
        return false;
    }
});

$("#SkillsAdminEditSavebutton").click(function () {

    var skillname = $("#SkillAdminNameedit").val();

    if (skillname != "") {
        $("#SkillAdminNameeditLabel").removeClass("validatefield");
    }
    else {
        $("#SkillAdminNameeditLabel").addClass("validatefield");

        alert("please first fill required data !");
        return false;
    }
});

//Form Update skill submited
$(document).ready(function () {
    $("#SkillsAdminEditSave").submit(function (event) {
        event.preventDefault();
        $.ajax({
            type: "POST",
            url: "/Admin/UpdateSkillAdmin",
            data: $(this).serialize() + "&Skillid=" + Skillid,
            success: function (data) {
                if (data.success) {
                    // Display success message and redirect to page
                    window.location.href = "/Admin/MissionSkillsAdmin";
                } else {
                    // Display error message
                    $("#skillmesssageshow2").append("<div class='alert alert-danger'>" + data.message + "</div>");
                    setTimeout(function () {
                        $(".alert-danger").fadeOut("slow", function () {
                            $(this).remove();
                        });
                    }, 4000);
                }
            }
        });
    });
});

// soft delete skill records

let deleteSkillid = 0;
$(".SkillDeleteAdminButton").click(function () {

    deleteSkillid = $(this).data('skillid');

});

$(".SkillDelete").click(function () {
    $.ajax({
        type: "POST",
        url: "/Admin/DeleteSkillRecords",
        data: { deleteSkillid: deleteSkillid },
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


//--------------------------Theme---------------------------------

// Theme Admin
$("#ThemeAdminName").blur(function () {
    var themename = $("#ThemeAdminName").val();

    if (themename != "") {
        $("#ThemeAdminNameLabel").removeClass("validatefield");
        return false;
    }
    else {
        $("#ThemeAdminNameLabel").addClass("validatefield");
        return false;
    }
});

$("#ThemeAdminSubmitbutton").click(function () {

    var themename = $("#ThemeAdminName").val();

    if (themename != "") {
        $("#ThemeAdminNameLabel").removeClass("validatefield");
    }
    else {
        $("#ThemeAdminNameLabel").addClass("validatefield");

        alert("please first fill required data !");
        return false;
    }
});
//Form add new user submited
$(document).ready(function () {
    $("#AddThemeAdmin").submit(function (event) {
        event.preventDefault();
        $.ajax({
            type: "POST",
            url: "/Admin/AddThemeAdmin",
            data: $(this).serialize(),
            success: function (data) {
                if (data.success) {
                    // Display success message and redirect to page
                    window.location.href = "/Admin/MissionThemeAdmin";
                } else {
                    // Display error message
                    $("#skillmesssageshow").append("<div class='alert alert-danger'>" + data.message + "</div>");
                    setTimeout(function () {
                        $(".alert-danger").fadeOut("slow", function () {
                            $(this).remove();
                        });
                    }, 5000);
                }
            }
        });
    });
});


// draft Theme data
let Themeid = 0;
// Edit Admin theme Draft data
$(".ThemeEditButton").click(function () {

    Themeid = $(this).data('themeid');

    $.ajax({
        url: '/Admin/DraftThemeAdminShow',
        type: 'POST',
        data: { Themeid: Themeid },
        success: function (result) {

            if (result.success) {
                var data = result.themedata;

                $("#ThemeAdminNameedit").val(data.themename);
                var status = data.status;

                if (status == 1) {
                    $('#themeactive').prop('checked', true);
                }
                else {
                    $('#themeinactive').prop('checked', true);
                }
            }
            else {
                alert("Something Went Wrong !");
            }
        },
        error: function (error) {
            console.log("error");
        }
    });
});

// edit theme form 
$("#ThemeAdminNameedit").blur(function () {
    var themename = $("#ThemeAdminNameedit").val();

    if (themename != "") {
        $("#ThemeAdminNameeditLabel").removeClass("validatefield");
        return false;
    }
    else {
        $("#ThemeAdminNameeditLabel").addClass("validatefield");
        return false;
    }
});

$("#ThemeAdminEditSavebutton").click(function () {

    var themename = $("#ThemeAdminNameedit").val();

    if (themename != "") {
        $("#ThemeAdminNameeditLabel").removeClass("validatefield");
    }
    else {
        $("#ThemeAdminNameeditLabel").addClass("validatefield");

        alert("please first fill required data !");
        return false;
    }
});

//Form Update Theme submited
$(document).ready(function () {
    $("#ThemeAdminEditSave").submit(function (event) {
        event.preventDefault();
        $.ajax({
            type: "POST",
            url: "/Admin/UpdateThemeAdmin",
            data: $(this).serialize() + "&Themeid=" + Themeid,
            success: function (data) {
                if (data.success) {
                    // Display success message and redirect to page
                    window.location.href = "/Admin/MissionThemeAdmin";
                } else {
                    // Display error message
                    $("#skillmesssageshow2").append("<div class='alert alert-danger'>" + data.message + "</div>");
                    setTimeout(function () {
                        $(".alert-danger").fadeOut("slow", function () {
                            $(this).remove();
                        });
                    }, 4000);
                }
            }
        });
    });
});

// soft delete user records

let deleteThemeid = 0;
$(".ThemeDeleteAdminButton").click(function () {

    deleteThemeid = $(this).data('themeid');

});

$(".ThemeDelete").click(function () {
    $.ajax({
        type: "POST",
        url: "/Admin/DeleteThemeRecords",
        data: { deleteThemeid: deleteThemeid },
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

//--------------------------Application---------------------------------
// Mission Application Approved data
let Applicationid = 0;
//  Mission Application Approved call
$(".approve_status").click(function () {

    Applicationid = $(this).data('applicationid');

    $.ajax({
        url: '/Admin/ApplicationStatusAdminShow',
        type: 'POST',
        data: { Applicationid: Applicationid },
        success: function (result) {

            if (result.success) {

                // Show the success message in a pop-up
                swal({
                    title: "Success",
                    text: "Mission Approved Successfully...",
                    icon: "success",
                    button: "OK",
                    timer: 2500,
                    closeOnClickOutside: false,
                    closeOnEsc: false,
                });

                // Delay reload for 2.5 seconds to give time for SweetAlert to be shown
                setTimeout(function () {
                    location.reload();
                }, 2500);
            }
            else {
                alert(" Mission Already Approved !");
            }
        },
        error: function (result) {
            if (result.error) {
                alert(" Oops something went wrong !");
            }
        }
    });
});

//  Mission Application Declined call
$(".declined_status").click(function () {

    Applicationid = $(this).data('applicationid');

    $.ajax({
        url: '/Admin/ApplicationStatusdeclinedAdminShow',
        type: 'POST',
        data: { Applicationid: Applicationid },
        success: function (result) {

            if (result.success) {

                // Show the success message in a pop-up
                swal({
                    title: "Success",
                    text: "Mission Declined Successfully...",
                    icon: "success",
                    button: "OK",
                    timer: 2500,
                    closeOnClickOutside: false,
                    closeOnEsc: false,
                });

                // Delay reload for 2.5 seconds to give time for SweetAlert to be shown
                setTimeout(function () {
                    location.reload();
                }, 2500);
            }
            else {
                alert(" Mission Already Declined !");
            }
        },
        error: function (result) {
            if (result.error) {
                alert(" Oops something went wrong !");
            }
        }
    });
});

//--------------------------Story---------------------------------
// Story stsatus data
let Storyid = 0;

//  Story Published call
$(".storyApprove_status").click(function () {

    Storyid = $(this).data('storyid');

    $.ajax({
        url: '/Admin/StoryAdminPublishedStatus',
        type: 'POST',
        data: { Storyid: Storyid },
        success: function (result) {

            if (result.success) {

                // Show the success message in a pop-up
                swal({
                    title: "Success",
                    text: "Story Published Successfully...",
                    icon: "success",
                    button: "OK",
                    timer: 2500,
                    closeOnClickOutside: false,
                    closeOnEsc: false,
                });

                // Delay reload for 2.5 seconds to give time for SweetAlert to be shown
                setTimeout(function () {
                    location.reload();
                }, 2500);
            }
            else {
                alert(" Story Already Published !");
            }
        },
        error: function (result) {
            if (result.error) {
                alert(" Oops something went wrong !");
            }
        }
    });
});

//  Mission Application Declined call
$(".storyDeclined_status").click(function () {

    Storyid = $(this).data('storyid');

    $.ajax({
        url: '/Admin/StoryAdminDeclinedStatus',
        type: 'POST',
        data: { Storyid: Storyid },
        success: function (result) {

            if (result.success) {

                // Show the success message in a pop-up
                swal({
                    title: "Success",
                    text: "Story Declined Successfully...",
                    icon: "success",
                    button: "OK",
                    timer: 2500,
                    closeOnClickOutside: false,
                    closeOnEsc: false,
                });

                // Delay reload for 2.5 seconds to give time for SweetAlert to be shown
                setTimeout(function () {
                    location.reload();
                }, 2500);
            }
            else {
                alert(" Story Already Declined !");
            }
        },
        error: function (result) {
            if (result.error) {
                alert(" Oops something went wrong !");
            }
        }
    });
});

// soft delete Story records

let deleteStoryid = 0;
$(".StoryDeleteAdminButton").click(function () {

    deleteStoryid = $(this).data('storyid');

});

$(".StoryDelete").click(function () {
    $.ajax({
        type: "POST",
        url: "/Admin/DeleteStoryRecords",
        data: { deleteStoryid: deleteStoryid },
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


//--------------------------Cms Admin---------------------------------

// add new cms blur
$("#cmsaddtitle").blur(function () {
    var title = $("#cmsaddtitle").val();

    if (title != "") {
        $("#cmsaddtitlelabel").removeClass("validatefield");
        return false;
    }
    else {
        $("#cmsaddtitlelabel").addClass("validatefield");
        return false;
    }
});
$("#cmsaddslug").blur(function () {
    var slug = $("#cmsaddslug").val();

    if (slug != "") {
        $("#cmssluglelabel").removeClass("validatefield");
        return false;
    }
    else {
        $("#cmssluglelabel").addClass("validatefield");
        return false;
    }
});

$("#cmsAddSubmitButton").click(function () {

    var title = $("#cmsaddtitle").val();
    var slug = $("#cmsaddslug").val();
    var details = tinymce.get('cmsadddetails').getContent();

    if (!title || !slug || !details) {

        if (!title) $("#cmsaddtitlelabel").addClass("validatefield");
        if (!slug) $("#cmssluglelabel").addClass("validatefield");
        if (!details) $("#cmsadddetailslabel").addClass("validatefield");

        alert("please first fill required data !");
        return false;
    }

    $("#cmsaddtitlelabel").removeClass("validatefield");
    $("#cmssluglelabel").removeClass("validatefield");
    $("#cmsadddetailslabel").removeClass("validatefield");

});

//Form add new Cms 
$(document).ready(function () {
    $("#cmsadminaddform").submit(function (event) {
        event.preventDefault();
        $.ajax({
            type: "POST",
            url: "/Admin/AddCmsAdmin",
            data: $(this).serialize(),
            success: function (data) {
                if (data.success) {
                    // Display success message and redirect to page
                    window.location.href = "/Admin/CmsAdmin";
                } else {
                    // Display error message
                    $("#cmsadminaddform").append("<div class='alert alert-danger'>" + data.message + "</div>");
                    setTimeout(function () {
                        $(".alert-danger").fadeOut("slow", function () {
                            $(this).remove();
                        });
                    }, 5000);
                }
            }
        });
    });
});

//Draft cms data show
let Cmsid = 0;
// Edit Admin User Draft data
$(".CmsEditAdminButton").click(function () {

    Cmsid = $(this).data('cmsid');

    $.ajax({
        url: '/Admin/DraftCmsAdminShow',
        type: 'POST',
        data: { Cmsid: Cmsid },
        success: function (result) {

            if (result.success) {
                var data = result.cmsdata;

                $("#cmsaddtitleedit").val(data.title);
                tinymce.get('cmsadddetailsedit').setContent(data.details);
                $("#cmsaddslugedit").val(data.slug);
                $("#cmseditselect").val(data.status);
            }
            else {
                alert("Something Went Wrong !");
            }
        },
        error: function (error) {
            console.log("error");
        }
    });
});

// edit validate
$("#cmsaddtitleedit").blur(function () {
    var title = $("#cmsaddtitleedit").val();

    if (title != "") {
        $("#cmsaddtitleeditlabel").removeClass("validatefield");
        return false;
    }
    else {
        $("#cmsaddtitleeditlabel").addClass("validatefield");
        return false;
    }
});
$("#cmsaddslugedit").blur(function () {
    var slug = $("#cmsaddslugedit").val();

    if (slug != "") {
        $("#cmsslugleditlabel").removeClass("validatefield");
        return false;
    }
    else {
        $("#cmsslugleditlabel").addClass("validatefield");
        return false;
    }
});

$("#cmsEditSubmitButton").click(function () {

    var title = $("#cmsaddtitleedit").val();
    var slug = $("#cmsaddslugedit").val();
    var details = tinymce.get('cmsadddetailsedit').getContent();

    if (!title || !slug || !details) {

        if (!title) $("#cmsaddtitleeditlabel").addClass("validatefield");
        if (!slug) $("#cmsslugleditlabel").addClass("validatefield");
        if (!details) $("#cmsadddetailseditlabel").addClass("validatefield");

        alert("please first fill required data !");
        return false;
    }

    $("#cmsaddtitleeditlabel").removeClass("validatefield");
    $("#cmsslugleditlabel").removeClass("validatefield");
    $("#cmsadddetailseditlabel").removeClass("validatefield");

});

//Form Update Cms submited
$(document).ready(function () {
    $("#cmsUpdateForm").submit(function (event) {
        event.preventDefault();
        $.ajax({
            type: "POST",
            url: "/Admin/UpdateCmsAdmin",
            data: $(this).serialize() + "&Cmsid=" + Cmsid,
            success: function (data) {
                if (data.success) {
                    // Display success message and redirect to page
                    window.location.href = "/Admin/CmsAdmin";
                } else {
                    // Display error message
                    $("#cmsedittitleerror").append("<div class='alert alert-danger'>" + data.message + "</div>");
                    setTimeout(function () {
                        $(".alert-danger").fadeOut("slow", function () {
                            $(this).remove();
                        });
                    }, 4000);
                }
            }
        });
    });
});

// soft delete user records

$(".CmsDeleteAdminButton").click(function () {

    Cmsid = $(this).data('cmsid');

});

$(".cmsDelete").click(function () {
    $.ajax({
        type: "POST",
        url: "/Admin/DeleteCmsRecords",
        data: { Cmsid: Cmsid },
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

//--------------------------Mission Admin---------------------------------

// time and goal based type
$(document).ready(function () {
    $(".mission_goal").hide();

    document.getElementById('addMissionDeadline').min = currentDate;
    document.getElementById('addMissionStartdate').min = currentDate;
    document.getElementById('addMissionEnddate').min = currentDate;

    $("input[name='missiontype']").click(function () {
        if ($(this).val() == "Time") {
            // If the "Time" option is selected
            $(".mission_time").show();
            $(".mission_goal").hide();
        } else {
            // If the "Goal" option is selected
            $(".mission_goal").show();
            $(".mission_time").hide();
        }
    });
});

//Add Mision Validate 
$("#addMissionTitle").blur(function () {
    var title = $("#addMissionTitle").val();

    if (title != "") {
        $("#addMissionTitleLabel").removeClass("validatefield");
        return false;
    }
    else {
        $("#addMissionTitleLabel").addClass("validatefield");
        return false;
    }
});
$("#addMissionShortDesc").blur(function () {
    var shortdesc = $("#addMissionShortDesc").val();

    if (shortdesc != "") {
        $("#addMissionShortDescLabel").removeClass("validatefield");
        return false;
    }
    else {
        $("#addMissionShortDescLabel").addClass("validatefield");
        return false;
    }
});
$("#addMissionOrgName").blur(function () {
    var orgName = $("#addMissionOrgName").val();

    if (orgName != "") {
        $("#addMissionOrgLabel").removeClass("validatefield");
        return false;
    }
    else {
        $("#addMissionOrgLabel").addClass("validatefield");
        return false;
    }
});
$("#addMissionGoalName").blur(function () {
    var goalName = $("#addMissionGoalName").val();

    if (goalName != "") {
        $("#addMissionGoalNameLabel").removeClass("validatefield");
        return false;
    }
    else {
        $("#addMissionGoalNameLabel").addClass("validatefield");
        return false;
    }
});
$("#addMissionGoalValue").blur(function () {
    var goalValue = $("#addMissionGoalValue").val();

    if (goalValue != "") {
        $("#addMissionGoalValueLabel").removeClass("validatefield");
        return false;
    }
    else {
        $("#addMissionGoalValueLabel").addClass("validatefield");
        return false;
    }
});

$("#addMissionSubmitBtn").click(function () {

    var title = $("#addMissionTitle").val();
    var shortdesc = $("#addMissionShortDesc").val();
    var details = tinymce.get('addMissionDesDetails').getContent();
    var orgDetails = tinymce.get('addMissionOrgDetails').getContent();
    var orgName = $("#addMissionOrgName").val();
    var themeName = $("#addMissionTheme").val();
    var availability = $("#addMissionAvailability").val();
    var Country = $(".addMissionCountry").val();
    var City = $(".addMissionCity").val();
    var goalName = $("#addMissionGoalName").val();
    var goalValue = $("#addMissionGoalValue").val();

    if (!title || !shortdesc || !details || !orgDetails || !orgName || !themeName || !availability || !Country || City == 0) {

        if (!title) $("#addMissionTitleLabel").addClass("validatefield");
        if (!shortdesc) $("#addMissionShortDescLabel").addClass("validatefield");
        if (!details) { $("#addMissionDesDetailsLabel").addClass("validatefield"); }
        else {
            $("#addMissionDesDetailsLabel").removeClass("validatefield");
        }
        if (!orgDetails) { $("#addMissionOrgDetailsLabel").addClass("validatefield"); }
        else {
            $("#addMissionOrgDetailsLabel").removeClass("validatefield");
        }

        if (!orgName) $("#addMissionOrgLabel").addClass("validatefield");
        if (!themeName) $("#addMissionThemeLabel").addClass("validatefield");
        else {
            $("#addMissionThemeLabel").removeClass("validatefield");
        }
        if (!availability) $("#addMissionAvailabilityLabel").addClass("validatefield");
        else {
            $("#addMissionAvailabilityLabel").removeClass("validatefield");
        }
        if (!Country) { $("#addMissionCountryLabel").addClass("validatefield"); }
        else {
            $("#addMissionCountryLabel").removeClass("validatefield");
        }
        if (City == 0) { $("#addMissionCityLabel").addClass("validatefield"); }
        else {
            $("#addMissionCityLabel").removeClass("validatefield");
        }

        alert("please first fill required data !");
        return false;
    }

    var goalselect = $("input[name='missiontype']:checked").val();
    if (goalselect == "Goal" && (!goalName || !goalValue)) {
        if (!goalName) $("#addMissionGoalNameLabel").addClass("validatefield");
        if (!goalValue) $("#addMissionGoalValueLabel").addClass("validatefield");
        alert("please first fill required data !");
        return false;
    }

    $("#addMissionTitleLabel").removeClass("validatefield");
    $("#addMissionShortDescLabel").removeClass("validatefield");
    $("#addMissionOrgLabel").removeClass("validatefield");
    $("#addMissionAvailabilityLabel").removeClass("validatefield");
    $("#addMissionCountryLabel").removeClass("validatefield");
    $("#addMissionThemeLabel").removeClass("validatefield");
    $("#addMissionGoalNameLabel").removeClass("validatefield");
    $("#addMissionGoalValueLabel").removeClass("validatefield");

});

//Form add new Mission 
$(document).ready(function () {
    $("#addMissionForm").submit(function (event) {
        event.preventDefault();

        var selectedSkills = [];
        $('.dropdown-menu.addmissiondropdown input[type=checkbox]:checked').each(function () {
            selectedSkills.push($(this).val());
        });

        //image data submit
        //Retrieve the base64 encoded image data from the hidden input fields
        var imageData = [];
        $('input[name="imageData[]"]').each(function () {
            imageData.push(JSON.parse($(this).val()));
        });
        // Extract an array of image names from the imageData array
        var imageNames = imageData.map(function (data) {
            return data.name;
        });

        if (imageNames.length === 0) {
            alert("upload atleast one image")
            $("#uploadvalidate").addClass("validatefield");
            return false;
        }
        else {
            $("#uploadvalidate").removeClass("validatefield");
        }

        $.ajax({
            type: "POST",
            url: "/Admin/AddMissionAdmin",
            data: $(this).serialize() + "&allSkills=" + selectedSkills + "&imageNames=" + imageNames,
            success: function (data) {
                if (data.success) {
                    // Display success message and redirect to page
                    window.location.href = "/Admin/MissionAdmin";
                } else {
                    // Display error message
                    //$("#cmsadminaddform").append("<div class='alert alert-danger'>" + data.message + "</div>");
                    setTimeout(function () {
                        $(".alert-danger").fadeOut("slow", function () {
                            $(this).remove();
                        });
                    }, 5000);
                }
            }
        });
    });
});
// ---------------------------------------------------------------------------



// ---------------------------------------------------------------------------





//Draft Mission data show
let Missionid = 0;

//edit Mision Validate 
$("#editMissionTitle").blur(function () {
    var title = $("#editMissionTitle").val();

    if (title != "") {
        $("#editMissionTitleLabel").removeClass("validatefield");
        return false;
    }
    else {
        $("#editMissionTitleLabel").addClass("validatefield");
        return false;
    }
});
$("#editMissionShortDesc").blur(function () {
    var shortdesc = $("#editMissionShortDesc").val();

    if (shortdesc != "") {
        $("#editMissionShortDescLabel").removeClass("validatefield");
        return false;
    }
    else {
        $("#editMissionShortDescLabel").addClass("validatefield");
        return false;
    }
});
$("#editMissionOrgName").blur(function () {
    var orgName = $("#editMissionOrgName").val();

    if (orgName != "") {
        $("#editMissionOrgLabel").removeClass("validatefield");
        return false;
    }
    else {
        $("#editMissionOrgLabel").addClass("validatefield");
        return false;
    }
});
$("#editMissionGoalName").blur(function () {
    var goalName = $("#editMissionGoalName").val();

    if (goalName != "") {
        $("#editMissionGoalNameLabel").removeClass("validatefield");
        return false;
    }
    else {
        $("#editMissionGoalNameLabel").addClass("validatefield");
        return false;
    }
});
$("#editMissionGoalValue").blur(function () {
    var goalValue = $("#editMissionGoalValue").val();

    if (goalValue != "") {
        $("#editMissionGoalValueLabel").removeClass("validatefield");
        return false;
    }
    else {
        $("#editMissionGoalValueLabel").addClass("validatefield");
        return false;
    }
});

$("#editMissionSubmitBtn").click(function () {

    var title = $("#editMissionTitle").val();
    var shortdesc = $("#editMissionShortDesc").val();
    var details = tinymce.get('editMissionDesDetails').getContent();
    var orgDetails = tinymce.get('editMissionOrgDetails').getContent();
    var orgName = $("#editMissionOrgName").val();
    var themeName = $("#editMissionTheme").val();
    var availability = $("#editMissionAvailability").val();
    var Country = $(".editMissionCountry").val();
    var City = $(".editMissionCity").val();
    var goalName = $("#editMissionGoalName").val();
    var goalValue = $("#editMissionGoalValue").val();

    if (!title || !shortdesc || !details || !orgDetails || !orgName || !themeName || !availability || Country == 0 || City == 0) {

        if (!title) $("#editMissionTitleLabel").addClass("validatefield");
        if (!shortdesc) $("#editMissionShortDescLabel").addClass("validatefield");
        if (!details) { $("#editMissionDesDetailsLabel").addClass("validatefield"); }
        else {
            $("#editMissionDesDetailsLabel").removeClass("validatefield");
        }
        if (!orgDetails) { $("#editMissionOrgDetailsLabel").addClass("validatefield"); }
        else {
            $("#editMissionOrgDetailsLabel").removeClass("validatefield");
        }

        if (!orgName) $("#editMissionOrgLabel").addClass("validatefield");
        if (!themeName) $("#editMissionThemeLabel").addClass("validatefield");
        else {
            $("#editMissionThemeLabel").removeClass("validatefield");
        }
        if (!availability) $("#editMissionAvailabilityLabel").addClass("validatefield");
        else {
            $("#editMissionAvailabilityLabel").removeClass("validatefield");
        }
        if (Country == 0) { $("#editMissionCountryLabel").addClass("validatefield"); }
        else {
            $("#editMissionCountryLabel").removeClass("validatefield");
        }
        if (City == 0) { $("#editMissionCityLabel").addClass("validatefield"); }
        else {
            $("#editMissionCityLabel").removeClass("validatefield");
        }

        alert("please first fill required data !");
        return false;
    }

    var goalselect = $("input[name='missiontype']:checked").val();
    if (goalselect == "Goal" && (!goalName || !goalValue)) {
        if (!goalName) $("#editMissionGoalNameLabel").addClass("validatefield");
        if (!goalValue) $("#editMissionGoalValueLabel").addClass("validatefield");
        alert("please first fill required data !");
        return false;
    }

    $("#editMissionTitleLabel").removeClass("validatefield");
    $("#editMissionShortDescLabel").removeClass("validatefield");
    $("#editMissionDesDetailsLabel").removeClass("validatefield");
    $("#editMissionOrgDetailsLabel").removeClass("validatefield");
    $("#editMissionOrgLabel").removeClass("validatefield");
    $("#editMissionAvailabilityLabel").removeClass("validatefield");
    $("#editMissionCountryLabel").removeClass("validatefield");
    $("#editMissionCityLabel").removeClass("validatefield");
    $("#editMissionThemeLabel").removeClass("validatefield");
    $("#editMissionGoalNameLabel").removeClass("validatefield");
    $("#editMissionGoalValueLabel").removeClass("validatefield");

});

// Edit Mission Draft data
$(".addMissionEditIcon").click(function () {

    Missionid = $(this).data('missionid');

    $.ajax({
        url: '/Admin/DraftMissionAdminShow',
        type: 'POST',
        data: { Missionid: Missionid },
        success: function (result) {

            if (result.success) {
                var data = result.missiondata;

                $("#editMissionTitle").val(data.title);
                $("#editMissionShortDesc").val(data.shortdes);
                tinymce.get('editMissionDesDetails').setContent(data.description);
                $("#editMissionOrgName").val(data.orgname);
                tinymce.get('editMissionOrgDetails').setContent(data.orgdetails);

                $("#country-select33").val(data.countryidvalue);
                //$("#GetMissionAdminCity").text(data.cityname);
                //$("#GetMissionAdminCountry").text(data.countryname);

                //$("#editMissionThemeDefault").text(data.themename);
                $("#editMissionTheme").val(data.themeidvalue);

                $("#editMissionAvailabilityDefault").text(data.availability);
                $("#editMissionSeats").val(data.seats);


                var cityid = data.cityidvalue;
                var countryId = data.countryidvalue;

                $.ajax({
                    url: "/UserRecords/GetCitiesByCountry",
                    type: "GET",
                    data: { countryId: countryId },
                    success: function (response) {
                        var citySelect = $("#city-select33");
                        if (cityid != 0) {
                            citySelect.empty();
                            $.each(response, function (i, city) {
                                citySelect.append($('<option>', {
                                    value: city.cityId,
                                    text: city.cityName
                                }));
                            });
                            $("#city-select33").val(cityid);
                        }
                        else {
                            citySelect.empty();
                            var newOption = $('<option>', {
                                value: '0',
                                text: 'Select your city'
                            });
                            citySelect.append(newOption);
                            $.each(response, function (i, city) {
                                citySelect.append($('<option>', {
                                    value: city.cityId,
                                    text: city.cityName
                                }));
                            });
                        }
                    },
                    error: function (response) {
                        console.log(response);
                    }
                });

                var missiontype = data.type;
                var missionstatus = data.status;

                if (missiontype == "Time") {
                    $('#editMissionTime').prop('checked', true);
                }
                else {
                    $('#editMissionGoal').prop('checked', true);
                }
                if (missionstatus == 1) {
                    $('#editMissionStatusActive').prop('checked', true);
                }
                else {
                    $('#editMissionStatusInactive').prop('checked', true);
                }
                // create the date object with the timezone offset adjustment
                var startdate = new Date(data.stardate);

                var timezoneOffset = startdate.getTimezoneOffset();
                startdate.setMinutes(startdate.getMinutes() - timezoneOffset);

                // format the date and set the value of the date input field
                var day = startdate.getDate();
                var month = (startdate.getMonth() + 1).toString().padStart(2, '0');
                var year = startdate.getFullYear();
                var formattedDate = year + "-" + month + "-" + day.toString().padStart(2, '0');

                $("#editMissionStartdate").val(formattedDate);

                var enddate = new Date(data.enddate);
                var timezoneOffset = enddate.getTimezoneOffset();
                enddate.setMinutes(enddate.getMinutes() - timezoneOffset);
                var day = enddate.getDate();
                var month = (enddate.getMonth() + 1).toString().padStart(2, '0');
                var year = enddate.getFullYear();
                var formattedendDate = year + "-" + month + "-" + day.toString().padStart(2, '0');

                $("#editMissionEndtdate").val(formattedendDate);

                var deadline = new Date(data.deadline);
                var timezoneOffset = deadline.getTimezoneOffset();
                deadline.setMinutes(deadline.getMinutes() - timezoneOffset);
                var day = deadline.getDate();
                var month = (deadline.getMonth() + 1).toString().padStart(2, '0');
                var year = deadline.getFullYear();
                var deadlineDate = year + "-" + month + "-" + day.toString().padStart(2, '0');

                $("#editMissionDeadline").val(deadlineDate);

                // Check the checkboxes for selected skills
                var Skills = data.missionskills;
                for (var i = 0; i < Skills.length; i++) {
                    var skillId = Skills[i];
                    $(".updatemissiondropdown").find(":checkbox[value='" + skillId + "']").prop("checked", true);
                }

                var missiontype = data.type;
                if (missiontype == "Goal") {
                    $(".mission_goal").show();
                    $(".mission_time").hide();
                    $("#editMissionGoalName").val(data.goaltext);
                    $("#editMissionGoalValue").val(data.goalvalue);
                } else {
                    $(".mission_goal").hide();
                    $(".mission_time").show();
                }
            }
            else {
                alert("Something Went Wrong !");
            }
        },
        error: function (error) {
            console.log("error");
        }
    });
});

//Form Update Mission submited
$(document).ready(function () {
    $("#editMissionForm").submit(function (event) {
        event.preventDefault();

        var selectedUpdateSkills = [];
        $('.dropdown-menu.updatemissiondropdown input[type=checkbox]:checked').each(function () {
            selectedUpdateSkills.push($(this).val());
        });

        $.ajax({
            type: "POST",
            url: "/Admin/UpdateMissionAdmin",
            data: $(this).serialize() + "&Missionid=" + Missionid + "&allSkills=" + selectedUpdateSkills,
            success: function (data) {
                if (data.success) {
                    // Display success message and redirect to page
                    window.location.href = "/Admin/MissionAdmin";
                } else {
                    // Display error message
                    $("#cmsedittitleerror").append("<div class='alert alert-danger'>" + data.message + "</div>");
                    setTimeout(function () {
                        $(".alert-danger").fadeOut("slow", function () {
                            $(this).remove();
                        });
                    }, 4000);
                }
            }
        });
    });
});

// soft delete user records

$(".MissionDeleteIcon").click(function () {

    Missionid = $(this).data('missionid');

});

$(".MissionDelete").click(function () {
    $.ajax({
        type: "POST",
        url: "/Admin/DeleteMissionRecords",
        data: { Missionid: Missionid },
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


//--------------------------Banner Admin---------------------------------

//Add banner Validate 
$("#addBannerText").blur(function () {
    var text = $("#addBannerText").val();

    if (text != "") {
        $("#addBannerTextLabel").removeClass("validatefield");
        return false;
    }
    else {
        $("#addBannerTextLabel").addClass("validatefield");
        return false;
    }
});
$("#addBannerOrder").blur(function () {
    var order = $("#addBannerOrder").val();

    if (order != "") {
        $("#addBannerOrderLabel").removeClass("validatefield");
        return false;
    }
    else {
        $("#addBannerOrderLabel").addClass("validatefield");
        return false;
    }
});


$("#addBannerSubmitBtn").click(function () {

    var text = $("#addBannerText").val();
    var order = $("#addBannerOrder").val();

    var imageData = [];
    $('input[name="imageData[]"]').each(function () {
        imageData.push(JSON.parse($(this).val()));
    });
    // Extract an array of image names from the imageData array
    var imageNames = imageData.map(function (data) {
        return data.name;
    });

    if (imageNames.length === 0) {
        alert("upload atleast one image")
        $("#bannerimagevalidate").addClass("validatefield");
        return false;
    }
    else if (imageNames.length > 1) {
        alert("upload only one image")
        $("#bannerimagevalidate").addClass("validatefield");
        return false;
    }
    else {
        $("#bannerimagevalidate").removeClass("validatefield");
    }

    if (!text || !order) {

        if (!text) $("#addBannerTextLabel").addClass("validatefield");
        if (!order) $("#addBannerOrderLabel").addClass("validatefield");
        alert("please first fill required data !");
        return false;
    }


    $("#addBannerTextLabel").removeClass("validatefield");
    $("#addBannerOrderLabel").removeClass("validatefield");

});

//Form add new banner 
$(document).ready(function () {
    $("#addBannerForm").submit(function (event) {
        event.preventDefault();

        // image data submit
        var imageData = [];
        $('input[name="imageData[]"]').each(function () {
            imageData.push(JSON.parse($(this).val()));
        });

        // Extract an array of image names from the imageData array
        var imageNames = imageData.map(function (data) {
            return data.name;
        });

        var imagename = imageNames[0];

        $.ajax({
            type: "POST",
            url: "/Admin/AddBannerAdmin",
            data: $(this).serialize() + "&imageNames=" + imagename,
            success: function (data) {
                if (data.success) {
                    // Display success message and redirect to page
                    window.location.href = "/Admin/BannerAdmin";
                } else {
                    // Display error message
                    alert("Oops Something went wrong !");
                }
            }
        });
    });
});

//Edit banner Validate 
$("#editBannerText").blur(function () {
    var text = $("#editBannerText").val();

    if (text != "") {
        $("#editBannerTextLabel").removeClass("validatefield");
        return false;
    }
    else {
        $("#editBannerTextLabel").addClass("validatefield");
        return false;
    }
});
$("#editBannerOrder").blur(function () {
    var order = $("#editBannerOrder").val();

    if (order != "") {
        $("#editBannerOrderLabel").removeClass("validatefield");
        return false;
    }
    else {
        $("#editBannerOrderLabel").addClass("validatefield");
        return false;
    }
});

$("#editBannerSubmitBtn").click(function () {

    var text = $("#editBannerText").val();
    var order = $("#editBannerOrder").val();

    if (!text || !order) {

        if (!text) $("#editBannerTextLabel").addClass("validatefield");
        if (!order) $("#editBannerOrderLabel").addClass("validatefield");
        alert("please first fill required data !");
        return false;
    }

    $("#editBannerTextLabel").removeClass("validatefield");
    $("#editBannerOrderLabel").removeClass("validatefield");

});
// draft banner data
let Bannerid = 0;
// Edit Admin banner Draft data
$(".BannerEditButton").click(function () {

    Bannerid = $(this).data('bannerid');

    $.ajax({
        url: '/Admin/DrafBannerAdminShow',
        type: 'POST',
        data: { Bannerid: Bannerid },
        success: function (result) {

            if (result.success) {
                var data = result.bannerdata;

                $("#editBannerText").val(data.description);
                $("#editBannerOrder").val(data.order);
            }
            else {
                alert("Something Went Wrong !");
            }
        },
        error: function (error) {
            console.log("error");
        }
    });
});

//Form Update banner submited
$(document).ready(function () {
    $("#editBannerForm").submit(function (event) {
        event.preventDefault();

        var imageData = [];
        $('input[name="imageData2[]"]').each(function () {
            imageData.push(JSON.parse($(this).val()));
        });
        // Extract an array of image names from the imageData array
        var imageNames = imageData.map(function (data) {
            return data.name;
        });

        if (imageNames.length === 0) {
            alert("upload atleast one image")
            $("#bannereditimagevalidate").addClass("validatefield");
            return false;
        }
        else if (imageNames.length > 1) {
            alert("upload only one image")
            $("#bannereditimagevalidate").addClass("validatefield");
            return false;
        }
        else {
            $("#bannereditimagevalidate").removeClass("validatefield");
        }

        var imagename = imageNames[0];

        $.ajax({
            type: "POST",
            url: "/Admin/UpdateBannerAdmin",
            data: $(this).serialize() + "&Bannerid=" + Bannerid + "&imagename=" + imageNames,
            success: function (data) {
                if (data.success) {
                    // Display success message and redirect to page
                    window.location.href = "/Admin/BannerAdmin";
                } else {
                    // Display error message
                    $("#bannermesssageshow2").append("<div class='alert alert-danger'>" + data.message + "</div>");
                    setTimeout(function () {
                        $(".alert-danger").fadeOut("slow", function () {
                            $(this).remove();
                        });
                    }, 4000);
                }
            }
        });
    });
});

// soft delete banner records

let deleteBannerId = 0;
$(".BannerDeleteAdminButton").click(function () {

    deleteBannerId = $(this).data('bannerid');

});

$(".BannerDelete").click(function () {
    $.ajax({
        type: "POST",
        url: "/Admin/DeleteBannerRecords",
        data: { deleteBannerId: deleteBannerId },
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

//-------------------------Registration-----------------------------

$("#RegisterFname").blur(function () {
    var title = $("#RegisterFname").val();

    if (title != "") {
        $("#RegisterFnameLabel").removeClass("validatefield");
        return false;
    }
    else {
        $("#RegisterFnameLabel").addClass("validatefield");
        return false;
    }
});
$("#RegisterLname").blur(function () {
    var title = $("#RegisterLname").val();

    if (title != "") {
        $("#RegisterLnameLabel").removeClass("validatefield");
        return false;
    }
    else {
        $("#RegisterLnameLabel").addClass("validatefield");
        return false;
    }
});
$("#Registerphoneno").blur(function () {
    var title = $("#Registerphoneno").val();

    if (title != "") {
        $("#RegisterphonenoLabel").removeClass("validatefield");
        return false;
    }
    else {
        $("#RegisterphonenoLabel").addClass("validatefield");
        return false;
    }
});
$("#UserRegisterEmail").blur(function () {
    var title = $("#UserRegisterEmail").val();

    if (title != "") {

        $("#UserRegisterEmailLabel").removeClass("validatefield");
        return false;
    }
    else {
        $("#UserRegisterEmailLabel").addClass("validatefield");
        return false;
    }
});
$("#UserRegisterPassword").blur(function () {
    var title = $("#UserRegisterPassword").val();

    if (title != "") {
        if (title.length < 8) {
            $("#UserRegisterPasswordLabel").removeClass("validatefield");
            return false;
        }
    }
    else {
        $("#UserRegisterPasswordLabel").addClass("validatefield");
        return false;
    }
});
$("#UserRegisterConPassword").blur(function () {
    var title = $("#UserRegisterConPassword").val();
    var title1 = $("#UserRegisterPassword").val();
    if (title != "") {
        if (title.length < 8) {
            $("#UserRegisterConPasswordLabel").removeClass("validatefield");
            return false;
        }
        if (title1 != conpassword) {
            alert("Both password must be same !");
            $("#UserRegisterConPasswordLabel").addClass("validatefield");
            return false;
        }
    }
    else {
        $("#UserRegisterConPasswordLabel").addClass("validatefield");
        return false;
    }
});

$("#registervalidate").click(function () {

    var fname = $("#RegisterFname").val();
    var lname = $("#RegisterLname").val();
    var no = $("#Registerphoneno").val();
    var email = $("#UserRegisterEmail").val();
    var password = $("#UserRegisterPassword").val();
    var conpassword = $("#UserRegisterConPassword").val();

    if (!fname || !lname || !no || !email || !password || !conpassword) {

        if (!fname) $("#RegisterFnameLabel").addClass("validatefield");
        if (!order) $("#RegisterLnameLabel").addClass("validatefield");
        if (!no) $("#RegisterphonenoLabel").addClass("validatefield");
        if (!email) $("#UserRegisterEmailLabel").addClass("validatefield");
        if (!password) $("#UserRegisterPasswordLabel").addClass("validatefield");
        if (!conpassword) $("#UserRegisterConPasswordLabel").addClass("validatefield");

        alert("please first fill required data !");
        return false;
    }

    if (password.length < 8) {
        alert("password must be atleast 8 character long!");
        $("#UserRegisterPasswordLabel").addClass("validatefield");
        return false;
    }
    if (password != conpassword) {
        alert("Both password must be same !");
        $("#UserRegisterConPasswordLabel").addClass("validatefield");
        return false;
    }
    $("#RegisterFnameLabel").removeClass("validatefield");
    $("#RegisterLnameLabel").removeClass("validatefield");
    $("#RegisterphonenoLabel").removeClass("validatefield");
    $("#UserRegisterEmailLabel").removeClass("validatefield");
    $("#UserRegisterPasswordLabel").removeClass("validatefield");
    $("#UserRegisterConPasswordLabel").removeClass("validatefield");

});