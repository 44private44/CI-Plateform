// JavaScript code
function search_mission() {
    let input = document.getElementById('searchbar').value
    input = input.toLowerCase();
    let x = document.getElementsByClassName('Mission_Title');  //mission message title
    let y = document.getElementsByClassName('quote1');

    var errorMessage = document.getElementById('error-message');

    var matchingElements = document.querySelectorAll(`[data-search*="${y}"]`);
    for (i = 0; i < x.length; i++) {
        if (!x[i].innerHTML.toLowerCase().includes(input)) {
            x[i].style.display = "none";
            y[i].style.display = "none";
        }
        else {
            x[i].style.display = "block";
            y[i].style.display = "block";
        }
        if ($('.card').is(':visible')) {
            document.getElementById("mission_check").style.display = "none";
            document.getElementById("mission_check2").style.display = "block";
            document.getElementById("mission_check3").style.display = "block";
        }
        else {
            document.getElementById("mission_check").style.display = "block";
            document.getElementById("mission_check2").style.display = "none";
            document.getElementById("mission_check3").style.display = "none";

        }
    }
}


// show checkbox selected items
arr = new Set([]);

function CheckedId(cid) {

    checkbox = document.getElementById(cid);
    var k = document.getElementById("filter-bar-row");
    if (event.target.checked) {
        arr.add(event.target.id);
    }
    else {
        arr.delete(event.target.value);
    }
    var temp = "";
    for (const item of arr) {
        temp += `<div class="col-auto filter-bar" id="${item}"><span> ${item} </span><button type="button" value= "${item}" class="btn btn-close-search"></button ></div >`;
    }

    // Add "Clear All" button if there are items in the set
    if (arr.size > 0) {
        temp += `<div class="col-auto filter-bar clearallfilter"><button type="button" class="btn btn-clear-all ">Clear All</button></div>`;
    }

    k.innerHTML = temp;
    $(".filter-bar-row").on("click", "button", function () {
        var uniID = $(this).val();
        $(".filter-bar-row #" + uniID).remove();
        $(".filterlist .form-check #" + uniID).prop('checked', false);
        arr.delete(uniID);
        if (arr.size === 0) {
            $(".clearallfilter").remove();
            location.reload();
        }
    });

    // Add click listener for "Clear All" button
    $(".filter-bar-row .btn-clear-all").on("click", function () {
        arr.clear();
        k.innerHTML = "";
        // Uncheck all checkboxes
        $(".filtercheckboxes .form-check input[type='checkbox']").prop('checked', false);
        location.reload();
    });

    if (arr.size === 0) {
        $(".clearallfilter").remove();
        location.reload();
    }
}

// password check

$("#resetpasswordsubmit").click(function () {

    var password = $("#resetpassword").val();
    var conpassword = $("#resetconpassword").val();

    if (!password || !conpassword) {

        if (!password) $("#resetpasswordlabel").addClass("validatefield");
        if (!conpassword) $("#resetconpasswordlabel").addClass("validatefield");
        //alert("please first fill required data !");
        return false;
    }

    if (password != "") {
        // 8 character validate
        if (password.length < 8) {
            $("#resetpasswordalert").removeClass("d-none");
            //alert(" Password must be atleast 8 characters !");
            return false;
        }
        $("#resetpasswordalert").addClass("d-none");
    }
    console.log(password);
    console.log(conpassword);
    if (password != conpassword) {
        $("#resetcomparepasswordalert").removeClass("d-none");
        //alert("both password must be same !");
        return false;
    }

    $("#resetpasswordlabel").removeClass("validatefield");
    $("#resetconpasswordlabel").removeClass("validatefield");

});

// Sorting
let gridContainer = $('.quote1').parent();

let tempArray = [];

$('#sort-box li').on('click', function () {
    let optionSelected = $(this).find('a').text();

    switch (optionSelected) {
        case 'Newest': sortByNewest()
            break;
        case 'Oldest': sortByOldest();
            break;
        case 'Lowest available seats': sortByLowestSeats();
            break;
        case 'Highest available seats': sortByHighestSeats();
            break;
        case 'Sort by my favourite': sortByFavourite();
            break;
        case 'Sort by deadline': sortByDeadline();
            break;
    }
})

//Newest sorting
function sortByNewest() {
    let dateCreated = $('.quote1').find('.dateCreated');
    dateCreated.each(function () {
        tempArray.push($(this).text());
    })
    tempArray = $.unique(tempArray);

    tempArray.sort(function (d1, d2) {
        var Dated1 = new Date(parseInt(d1.substring(6)),
            parseInt(d1.substring(3, 5)) - 1, parseInt(d1.substring(0, 2)));
        var Dated2 = new Date(parseInt(d2.substring(6)),
            parseInt(d2.substring(3, 5)) - 1, parseInt(d2.substring(0, 2)));
        return Dated2 - Dated1; // newest
    })

    tempArray = $.unique(tempArray);
    for (var i = 0; i < tempArray.length; i++) {
        $('.card').each(function () {
            if ($(this).find('.dateCreated').text() == tempArray[i]) {
                $(this).parent().appendTo($(gridContainer));
            }
        });

    }

    /*filter();*/
}

// Oldest Sorting
function sortByOldest() {
    let dateCreated = $('.quote1').find('.dateCreated');
    dateCreated.each(function () {
        tempArray.push($(this).text());
    })

    tempArray = $.unique(tempArray);

    tempArray.sort(function (d1, d2) {
        var Dated1 = new Date(parseInt(d1.substring(6)),
            parseInt(d1.substring(3, 5)) - 1, parseInt(d1.substring(0, 2)));
        var Dated2 = new Date(parseInt(d2.substring(6)),
            parseInt(d2.substring(3, 5)) - 1, parseInt(d2.substring(0, 2)));
        return Dated1 - Dated2; // oldest
    })

    tempArray = $.unique(tempArray);
    for (var i = 0; i < tempArray.length; i++) {
        $('.card').each(function () {
            if ($(this).find('.dateCreated').text() == tempArray[i]) {
                $(this).parent().appendTo($(gridContainer));
            }
        });


    }
}

// Seat left sorting asc
function sortByLowestSeats() {
    let dateCreated = $('.quote1').find('.seatsLeft');
    dateCreated.each(function () {
        tempArray.push($(this).text());
    })

    tempArray = $.unique(tempArray);

    tempArray.sort(function (s1, s2) {

        return s1 - s2; // lowest seats
    })

    tempArray = $.unique(tempArray);
    for (var i = 0; i < tempArray.length; i++) {
        $('.card').each(function () {
            if ($(this).find('.seatsLeft').text() == tempArray[i]) {
                $(this).parent().appendTo($(gridContainer));
            }
        });
    }

    //filter();
}

// Seat left sorting des
function sortByHighestSeats() {
    let dateCreated = $('.quote1').find('.seatsLeft');
    dateCreated.each(function () {
        tempArray.push($(this).text());
    })

    tempArray = $.unique(tempArray);

    tempArray.sort(function (s1, s2) {

        return s2 - s1; // highest seats
    })

    tempArray = $.unique(tempArray);
    for (var i = 0; i < tempArray.length; i++) {
        $('.card').each(function () {
            if ($(this).find('.seatsLeft').text() == tempArray[i]) {
                $(this).parent().appendTo($(gridContainer));
            }
        });
    }

    //filter();
}

// favourite rating sorting
function sortByFavourite() {
    let dateCreated = $('.quote1').find('.favouriterating');
    dateCreated.each(function () {
        tempArray.push($(this).text());
    })

    tempArray = $.unique(tempArray);

    tempArray.sort(function (s1, s2) {

        return s2 - s1; // highest seats
    })

    tempArray = $.unique(tempArray);
    for (var i = 0; i < tempArray.length; i++) {
        $('.card').each(function () {
            if ($(this).find('.favouriterating').text() == tempArray[i]) {
                $(this).parent().appendTo($(gridContainer));
            }
        });
    }
}

// deadline sorting
function sortByDeadline() {
    let deadlineDate = $('.quote1').find('.deadlineDate');
    deadlineDate.each(function () {
        tempArray.push($(this).text());
    })
    tempArray = $.unique(tempArray);

    tempArray.sort(function (d1, d2) {
        var Dated1 = new Date(parseInt(d1.substring(6)), parseInt(d1.substring(3, 5) - 1), parseInt(d1.substring(0, 2)));
        var Dated2 = new Date(parseInt(d2.substring(6)), parseInt(d2.substring(3, 5) - 1), parseInt(d2.substring(0, 2)));

        return Dated1 - Dated2;
    })

    tempArray = $.unique(tempArray);

    for (var i = 0; i < tempArray.length; i++) {
        $('.card').each(function () {
            if ($(this).find('.deadlineDate').text() == tempArray[i]) {
                $(this).parent().appendTo($(gridContainer));
            }
        })
    }
}


// favourite mission 
$('.favorite-button').click(function () {
    var button = $(this)
    var missionId = $(this).val();
    $.ajax({
        url: '/Mission/AddToFavorites',
        type: 'POST',
        data: { missionId: missionId },
        // Show a success message or update the UI
        success: function (result) {
            var allMissionId = $('.favorite-button')
            allMissionId.each(function () {
                if ($(this).val() === missionId) {
                    if ($(this).hasClass('heart-remove')) {
                        $(this).removeClass('heart-remove')
                        $(this).addClass('heart')
                    }
                    else {
                        $(this).removeClass('heart')
                        $(this).addClass('heart-remove')
                    }
                }
            })
        },
        error: function (error) {
            console.log("error")

        }
    });
});

// comment
$('#comment_click').click(function () {
    var missionId = $(this).val();
    var commenttext = $('#exampleFormControlTextarea1').val();
    var commenuname = $('#commentusername').text();
    $.ajax({
        url: '/Mission/postcomment',
        type: 'POST',
        data: { missionId: missionId, commenttext: commenttext, commenuname: commenuname },
        // Show a success message or update the UI
        success: function (result) {
            console.log("success");

        },
        error: function (error) {
            // Show an error message or handle the error
            console.log("error");

        }
    });
});

//recommended co-worker
$('.recommendedonclick').click(function () {
    var recoemail = $(this).val();
    var recommissionid = $('.favorite-button').val();
    $.ajax({
        url: '/Mission/recommendedcoworkers',
        type: 'POST',
        data: { recoemail: recoemail, recommissionid: recommissionid },
        // Show a success message or update the UI
        success: function (result) {
            var successMsg = "Email successfully sent!";
            if (result.success) {
                // Show the success message in a pop-up
                swal({
                    title: "Success",
                    text: successMsg,
                    icon: "success",
                    button: "OK",
                    timer: 2500,
                    closeOnClickOutside: false,
                    closeOnEsc: false,
                });

                setTimeout(function () {
                    location.reload();
                }, 2600);

            }
            else {
                alert("You recommended already !");
            }

        },
        error: function (result) {
            // Show an error message or handle the error
            console.log("error");

        }
    });

});

//mission rating
$(".starupper i").click(function () {
    var rating = $(this).index() + 1;
    var sIcon = $(this).prevAll().addBack();
    var uIcon = $(this).nextAll();
    var MissionId = $(this).data('mission-id');
    $.ajax({
        type: "POST",
        url: "/Mission/Addrating",
        data: { MissionId: MissionId, rating: rating },
        success: function () {
            sIcon.removeClass('bi-star').addClass('bi-star-fill text-warning');
            uIcon.removeClass('bi-star-fill text-warning').addClass('bi-star');
        },
        error: function (error) {
            console.log(error);

        }
    });
});

//apply status application
$("#applystatus").click(function () {
    var appliedmissionid = $("#applystatusmissionid").text();
    var applieduserid = $("#applystatususerid").text();
    $.ajax({
        type: "POST",
        url: "/Mission/applieduser",
        data: {
            appliedmissionid: appliedmissionid,
            applieduserid: applieduserid
        },
        success: function (data) {
            $("#applystatus").html(data);
            $("#applystatus").html(data).css({
                "cursor": "default"

            }).hover(function () {
                $(this).css("border-color", "#F88634");
            }, function () {
                $(this).css("border-color", "#F88634");
            });
            // Show the success message in a pop-up
            swal({
                title: "Success",
                text: "Mission applied successfully.",
                icon: "success",
                button: "OK",
                timer: 2500,
                closeOnClickOutside: true,
                closeOnEsc: true,
            });

        },
        error: function () {
            console.log("error");
        }

    });
});

// ------------------------- Images upload ----------------------------------------------------------------

$(document).ready(function () {
    $('#submitsharestorydesign').on('click', function (e) {
        e.preventDefault();

        var form = $(this).closest('form');
        var files = $('#file-upload').get(0).files;


        // Check if files are selected
        if (files.length > 0) {
            form.find('input[name="files"]').remove();

            // Create a new input element for file selection
            var newFileInput = $('<input type="file" name="files" multiple>');

            // Append the new input element to the form
            form.append(newFileInput);

            // Create a new FormData object
            var formData = new FormData(form[0]);

            // Append each selected file to the FormData object
            for (var i = 0; i < files.length; i++) {
                formData.append('files', files[i]);
            }

            // Perform an AJAX request to submit the form with the selected images
            $.ajax({
                url: form.attr('action'),
                type: form.attr('method'),
                data: formData,
                processData: false,
                contentType: false,
                success: function (response) {
                    if (response.success) {
                        window.location.href = "/Mission/MissionData";
                    }
                    if (!response.success) {
                        window.location.href = "/Mission/Error";

                    }
                },
                error: function (xhr, status, error) {
                    // Handle the error
                    console.log(error);
                }
            });
        } else {
            alert("please choose alteast one image...");
        }
    });
});

// mission approve status 
let SPApplicationID = 0;
$('.sp_ApproveStatus').on('click', function (e) {
    e.preventDefault();

    var SPApplicationID = $(this).data('missionid');
    $.ajax({
        url: '/Mission/SPMissionStatus',
        type: 'POST',
        data: { SPApplicationID: SPApplicationID },
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

$('.sp_ApproveStatusDecline').on('click', function (e) {
    e.preventDefault();

    var SPApplicationID = $(this).data('missionid');

    $.ajax({
        url: '/Mission/SPMissionStatusDecline',
        type: 'POST',
        data: { SPApplicationID: SPApplicationID },
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

