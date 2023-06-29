
//get cities
function getCitiesByCountry(CountryId) {
    $.ajax({
        url: '/Mission/GetCitiesByCountry',
        type: 'GET',
        data: { CountryId: CountryId },

        success: function (data) {
            var cities = $('#cities');
            cities.empty();
            var items = "";
            $.each(data, function (i, item) {
                items += `<li class="ms-2 filtercheckboxes filterlist" >
                        <a class="dropdown-item form-check">
                        <input name="cityCheckboxes" type="checkbox"
                    onclick="CheckedId('`+ item.name + `')"
class="form-check-input me-3" id="`+ item.name + `" value=` + item.name + `><label class="form-check-label" for="exampleCheck1" >` + item.name + `</label>
                        </a></li>`

            });
            cities.html(items);
        }
    });
}

let selectedCountry = null;
// country filter
$(".countryDropdown li").click(function () {
    var countryId = $(this).val();
    getCitiesByCountry(countryId);

    selectedCountry = $(this).find('a').text();

    $('.quote1').each(function () {
        var cardCountry = $(this).find('.mission-country').val();

        if (countryId == cardCountry) {
            $(this).show();

        } else {
            $(this).hide();
        }
    });

    if ($('.card').is(':visible')) {
        document.getElementById("mission_check").style.display = "none";
        document.getElementById("mission_check2").style.display = "block";
    }
    else {
        document.getElementById("mission_check").style.display = "block";
        document.getElementById("mission_check2").style.display = "none";
    }
});


// city filter
$(".cityDropdown li").click(function () {

    var themeId = $(this).val();
    selectedTheme = $(this).find('a').text();

    $('.quote1').each(function () {
        var cardTheme = $(this).find('.mission-city').val();

        if (themeId == cardTheme) {
            $(this).show();

        } else {
            $(this).hide();
        }
    });

    if ($('.card').is(':visible')) {
        document.getElementById("mission_check").style.display = "none";
        document.getElementById("mission_check2").style.display = "block";
    }
    else {
        document.getElementById("mission_check").style.display = "block";
        document.getElementById("mission_check2").style.display = "none";
    }
});

// theme filter
$(".themeDropdown li").click(function () {

    var themeId = $(this).val();
    selectedTheme = $(this).find('a').text();

    $('.quote1').each(function () {
        var cardTheme = $(this).find('.mission-theme').val();

        if (themeId == cardTheme) {
            $(this).show();

        } else {
            $(this).hide();
        }
    });
    if ($('.card').is(':visible')) {
        document.getElementById("mission_check").style.display = "none";
        document.getElementById("mission_check2").style.display = "block";
    }
    else {
        document.getElementById("mission_check").style.display = "block";
        document.getElementById("mission_check2").style.display = "none";
    }
});


// logout 
$("#userLogout").click(function () {
    $.ajax({
        type: "POST",
        url: "/Admin/userLogout",
        data: {},
        success: function (response) {
            window.location.href = "/UserAuth/Login";
        },
        error: function (response) {
            alert("Oops Something went wrong !")
        }
    });

});

// logout 
$("#adminLogout").click(function () {
    $.ajax({
        type: "POST",
        url: "/Admin/adminLogout",
        data: {},
        success: function (response) {
            window.location.href = "/UserAuth/Login";
        },
        error: function (response) {
            alert("Oops Something went wrong !")
        }
    });

});


// email blur
$("#UserRegisterEmail").blur(function () {
    var email = $("#UserRegisterEmail").val();

    if (email != "") {
        $("#UserRegisterEmailLabel").removeClass("validatefield");
        $.ajax({
            url: '/UserAuth/UserEmailValidateRegister',
            type: 'POST',
            data: { email: email },
            success: function (result) {
                if (result.success) {
                    $("#userAdminValidateEmail8").removeClass("d-none");
                }
                else {
                    $("#userAdminValidateEmail8").addClass("d-none");
                    return false;
                }
            },
            error: function (error) {
                return false;
            }
        });
        return false;
    }
    else {
        $("#UserRegisterEmailLabel").addClass("validatefield");
        return false;
    }
});


//login

$("#loginemailmain").blur(function () {
    var name = $("#loginemailmain").val();

    if (name != "") {
        $("#loginemailmainlabel").removeClass("validatefield");
        return false;
    }
    else {
        $("#loginemailmainlabel").addClass("validatefield");
        return false;
    }
});

$("#loginpasswordmain").blur(function () {
    var name = $("#loginpasswordmain").val();

    if (name != "") {
        $("#loginpasswordmainlabels").removeClass("validatefield");
        return false;
    }
    else {
        $("#loginpasswordmainlabels").addClass("validatefield");
        return false;
    }
});

// email blur
$("#loginemailmain").blur(function () {
    var email = $("#loginemailmain").val();

    if (email != "") {
        $("#loginemailmainlabel").removeClass("validatefield");
        $.ajax({
            url: '/UserAuth/UserEmailValidateLogin',
            type: 'POST',
            data: { email: email },
            success: function (result) {
                if (result.success) {
                    $("#userAdminValidateEmail9").addClass("d-none");
                }
                else {
                    $("#userAdminValidateEmail9").removeClass("d-none");
                    return false;
                }
            },
            error: function (error) {
                return false;
            }
        });
    }
    else {
        $("#loginemailmainlabel").addClass("validatefield");
        return false;
    }
});

