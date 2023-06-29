//image upload
const avatarUpload = $('#avatar-upload');
const avatarImage = $('.useravatarimage img');

avatarUpload.on('change', (event) => {
    const file = event.target.files[0];
    if (file) {
        // Display the selected image in the avatar
        const reader = new FileReader();
        reader.addEventListener('load', () => {
            avatarImage.attr('src', reader.result);
        });

        const formData = new FormData();
        formData.append('file', file);

        $.ajax({
            type: 'POST',
            url: '/UserRecords/Updateuerimage',
            data: formData,
            processData: false,
            contentType: false,
            success: function (result) {
                if (result.success) {
                    console.log("success stored image");
                    // Show a success message to the user
                    alert('Image uploaded successfully!');
                }
            },
            error: function (error) {
                console.log("error image not upload");
                // Show an error message to the user
                alert('Error uploading image!');
            }
        });
        reader.readAsDataURL(file);
    }
});

//---------------------------------------------------------

// Draft data show
$(document).ready(function () {

    $.ajax({
        url: '/UserRecords/Draftdatashow',
        type: 'POST',
        data: {},
        success: function (result) {

            if (result.success) {
                var data = result.userdata;
                $("#userprofilename").val(data.firstname);
                $("#userprofilesurname").val(data.lastname);
                $("#userprofileempid").val(data.empid);
                $("#userprofiletitle").val(data.title);
                $("#userprofilemanager").val(data.managerdetails);
                $("#userprofiledept").val(data.department);
                $("#userprofilewhyivol").val(data.whyivol);

                //$("#getusercity").text(data.cityname);
                //$("#getusercountry").text(data.countryname);
                $("#country-select20").val(data.countryidvalue);

                var cityid = data.cityidvalue;
                var countryId = data.countryidvalue;

                $.ajax({
                    url: "/UserRecords/GetCitiesByCountry",
                    type: "GET",
                    data: { countryId: countryId },
                    success: function (response) {
                        var citySelect = $("#city-select20");
                        if (cityid != 0) {
                            citySelect.empty();
                            $.each(response, function (i, city) {
                                citySelect.append($('<option>', {
                                    value: city.cityId,
                                    text: city.cityName
                                }));
                            });
                            $("#city-select20").val(cityid);
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

                $("#getuseravailability").text(data.availability);
                $("#userprofiletext").val(data.profiletext);
                $("#userprofilelinkedin").val(data.linkedin);
            }
        },
        error: function (error) {
            console.log("error");
        }
    });
});

//change password

//obblur validate
$("#userprofileoldpassword").blur(function () {
    var name = $("#userprofileoldpassword").val();

    if (name != "") {
        $("#useroldpassword").removeClass("validatefield");
        /*  return false;*/
    }
    else {
        $("#useroldpassword").addClass("validatefield");
        //return false;
    }
});
$("#userprofilepassword").blur(function () {
    var password = $("#userprofilepassword").val();

    if (password != "") {
        $("#usernewpassword").removeClass("validatefield");
        // 8 character validate
        if (password.length < 8) {
            $("#usernewpasswordmatch").removeClass("matchpassworddiv");
            return false;
        }
        $("#usernewpasswordmatch").addClass("matchpassworddiv");
        return false;
    }
    else {
        $("#usernewpassword").addClass("validatefield");
        return false;
    }
});
$("#userprofileconpassword").blur(function () {
    var name = $("#userprofileconpassword").val();

    if (name != "") {
        $("#usernewconpassword").removeClass("validatefield");
        return false;
    }
    else {
        $("#usernewconpassword").addClass("validatefield");
        return false;
    }
});

$(".usereditpassword").click(function () {

    event.preventDefault();

    var newpassword = $("#userprofilepassword").val();
    var newconpassword = $("#userprofileconpassword").val();
    var currentpassword = $("#userprofileoldpassword").val();

    // onclick validate

    if (!currentpassword || !newconpassword || !newpassword) {

        if (!currentpassword) $("#useroldpassword").addClass("validatefield");
        if (!newconpassword) $("#usernewconpassword").addClass("validatefield");
        if (!newpassword) $("#usernewpassword").addClass("validatefield");

        $("#validatebothpdiv").addClass("matchpassworddiv");
        alert("First fill all required data !");
        return false;
    }

    $("#useroldpassword").removeClass("validatefield");
    $("#usernewconpassword").removeClass("validatefield");
    $("#usernewpassword").removeClass("validatefield");
    $("#validatebothpdiv").addClass("matchpassworddiv");

    //Password matching
    if (newpassword != newconpassword) {
        $("#validatebothpassword").addClass("validatefield");
        $("#validatebothpdiv").removeClass("matchpassworddiv");
        return false;
    }
    else {
        $("#validatebothpdiv").addClass("matchpassworddiv");
        $("#validatebothpassword").removeClass("validatefield");
    }

    $.ajax({
        url: '/UserRecords/changepassword',
        type: 'POST',
        data: { currentpassword: currentpassword, newpassword: newpassword },
        success: function (result) {
            if (result.success) {
                alert("Great password successfully changed...");
                location.reload();
            }
            else {
                alert(result.message);
                return false;
            }
        },
        error: function (result) {
            if (result.error) {
                alert("Oops something went wrong!");
            }
        }
    });

});


//submit data validate
$("#userprofilename").blur(function () {
    var name = $("#userprofilename").val();

    if (name != "") {
        $("#uservalidname").removeClass("validatefield");
        return false;
    }
    else {
        $("#uservalidname").addClass("validatefield");
        return false;
    }
});
$("#userprofilesurname").blur(function () {
    var sname = $("#userprofilesurname").val();

    if (sname != "") {
        $("#uaevalidsname").removeClass("validatefield");
        return false;
    }
    else {
        $("#uaevalidsname").addClass("validatefield");
        return false;
    }
});
$("#userprofiletext").blur(function () {
    var profiletext = $("#userprofiletext").val();

    if (profiletext != "") {
        $("#uservalidprofile").removeClass("validatefield");
        return false;
    }
    else {
        $("#uservalidprofile").addClass("validatefield");
        return false;
    }
});

$("#submitusereditdata").click(function () {

    var name = $("#userprofilename").val();
    var sname = $("#userprofilesurname").val();
    var profiletext = $("#userprofiletext").val();
    var country = $(".editUserprofileCountry").val();

    if (!name || !sname || !profiletext || country == 0) {

        if (!name) $("#uservalidname").addClass("validatefield");
        if (!sname) $("#uaevalidsname").addClass("validatefield");
        if (!profiletext) $("#uservalidprofile").addClass("validatefield");
        if (country == 0) { $("#editUserprofileCountryLabel").addClass("validatefield"); }
        else {
            $("#editUserprofileCountryLabel").removeClass("validatefield");
        }
        alert("please first fill required data !");
        return false;
    }

    $("#uservalidname").removeClass("validatefield");
    $("#uaevalidsname").removeClass("validatefield");
    $("#uservalidprofile").removeClass("validatefield");

});

//getcities based on countryid
$("#country-select20").change(function () {
    var countryId = $("#country-select20 option:selected").val();

    $.ajax({
        url: "/UserRecords/GetCitiesByCountry",
        type: "GET",
        data: { countryId: countryId },
        success: function (response) {
            var citySelect = $("#city-select20");
            citySelect.empty();
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

//add skills
$(".allskillchoose").click(function () {

    var skillid = $(this).data("id");

    $.ajax({
        type: "POST",
        url: "/UserRecords/addskills",
        data: { skillid: skillid },
        success: function (response) {
            if (response.success) {
                var skillid = response.dataid;
                var skillname = response.dataname;

                var newSkillElement = $("<p>")
                    .attr("id", skillid)
                    .text(skillname)
                    .append($("<i>").addClass("fa-solid fa-trash-can"));
                $(".addnewskill").append(newSkillElement);

                // Update the skill list
                updateSkillList();
            }
            else {
                alert("This skill has already been added!");
            }
        },
        error: function (response) {
            alert("Oops something went wrong !");

        }
    });
});

//remove skills
$(".selectedskillchoose").click(function () {

    var userskillid = $(this).data("id");

    $.ajax({
        type: "post",
        url: "/userrecords/removeskills",
        data: { userskillid: userskillid },
        success: function (response) {
            if (response.success) {

                //remove the element with match userid
                $(".selectedskillchoose").filter(function () {
                    return $(this).data("id") == userskillid;
                }).remove();

                // Update the skill list
                updateSkillList();
            }
            else {
                alert("oops something went wrong !");
            }
        },
        error: function (response) {
            alert("oops something went wrong !");

        }
    });
});

// Update the skill list
function updateSkillList() {
    $.ajax({
        type: "GET",
        url: "/UserRecords/GetUserSkills",
        success: function (response) {
            var html = "";
            response.forEach(function (skill) {
                html += "<p>" + skill + "</p>";
            });
            $(".alluserskillsscroll").html(html);
        },
        error: function (response) {
            alert("Oops something went wrong !");
        }
    });
}

//save skills
$("#saveskills").click(function () {
    // Update the skill list
    updateSkillList();
});



//contact us
$("#contactussubject").blur(function () {
    var name = $("#contactussubject").val();

    if (name != "") {
        $("#contactussubjectlabel").removeClass("validatefield");
    }
    else {
        $("#contactussubjectlabel").addClass("validatefield");
    }
});
$("#contactusmessage").blur(function () {
    var name = $("#contactusmessage").val();

    if (name != "") {
        $("#contactusmessagelabel").removeClass("validatefield");
    }
    else {
        $("#contactusmessagelabel   ").addClass("validatefield");
    }
});
$(".contactussend").click(function () {

    event.preventDefault();

    var subject = $("#contactussubject").val();
    var message = $("#contactusmessage").val();

    // onclick validate

    if (!subject || !message) {

        if (!subject) $("#contactussubjectlabel").addClass("validatefield");
        if (!message) $("#contactusmessagelabel").addClass("validatefield");

        alert("First fill all required data !");
        return false;
    }

    $("#contactussubjectlabel").removeClass("validatefield");
    $("#contactusmessagelabel").removeClass("validatefield");

    $.ajax({
        type: "POST",
        url: "/UserRecords/Contactusdata",
        data: { subject: subject, message: message },
        success: function () {
            // Show the success message in a pop-up
            swal({
                title: "Success",
                text: "Your message has beeen sent.",
                icon: "success",
                button: "OK",
                timer: 2000,
                closeOnClickOutside: false,
                closeOnEsc: false,
            });

            // Delay reload for 2.5 seconds to give time for SweetAlert to be shown
            setTimeout(function () {
                location.reload();
            }, 2500);
        },
        error: function () {
            alert("Oops something went wrong !");
        }
    });
});