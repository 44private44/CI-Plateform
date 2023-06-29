// search storycard
function searchstory() {
    let input = document.getElementById('searchbar').value
    input = input.toLowerCase();
    let x = document.getElementsByClassName('story_title');  //story title
    let y = document.getElementsByClassName('quote1');
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
            console.log("visible");
            document.getElementById("mission_check").style.display = "none";
            document.getElementById("mission_check2").style.display = "block";
        }
        else {
            console.log("notvisible");
            document.getElementById("mission_check").style.display = "block";
            document.getElementById("mission_check2").style.display = "none";
        }
    }
}

// validate blur 
// title validate
$("#sharestorytitle").on("blur", function () {
    var title = $("#sharestorytitle").val();
    if (!title) {
        $("#titlevalidate").addClass("validatefield");
        return false;
    }
    else {
        $("#titlevalidate").removeClass("validatefield");
    }
});
$("#sharestorytype").blur(function () {
    var name = $("#sharestorytype").val();

    if (name != "") {
        $("#storytypevalidate").removeClass("validatefield");
        return false;
    }
    else {
        $("#storytypevalidate").addClass("validatefield");
        return false;
    }
});
$("#sharestoryvideourl").blur(function () {
    var name = $("#sharestoryvideourl").val();

    if (name != "") {
        $("#videourlvalidate").removeClass("validatefield");
        return false;
    }
    else {
        $("#videourlvalidate").addClass("validatefield");
        return false;
    }
});
//save share story
$("#savesharestory").click(function (e) {

    e.preventDefault();
    var missionId = $("#mission-select option:selected").val();

    var title = $("#sharestorytitle").val();

    var date = $("#sharestorydate").val();

    var discription = data.getData();

    var videourl = $("#sharestoryvideourl").val();

    var storytype = $("#sharestorytype").val();


    const formData = new FormData();

    formData.append('MissionId', missionId);
    formData.append('StoryTitle', title);
    formData.append('StoryDate', date);
    formData.append('Description', discription);
    formData.append('VideoURLs', videourl);
    formData.append('Type', storytype);

    //var images = [];
    for (let i = 0; i < imageFiles.length; i++) {
        //images.push(imageFiles[i]);
        formData.append('Images', imageFiles[i]);
    }
    //console.log(images);

    if (missionId == "Select your Mission") {
        $("#missionvalidate").addClass("validatefield");
        alert("first select your mission!")
        return false;
    }
    else {
        $("#missionvalidate").removeClass("validatefield");
    }

    // Check if any of the required fields are empty
    if (!title || !date || !discription || !videourl || !storytype) {

        // Show the required fields in red

        if (!title) $("#titlevalidate").addClass("validatefield");
        if (!date) $("#datevalidate").addClass("validatefield");

        if (!discription) $("#storyvalidate").addClass("validatefield");
        if (!videourl) $("#videourlvalidate").addClass("validatefield");
        if (!storytype) $("#storytypevalidate").addClass("validatefield");

        alert("Please first fill in all required fields!");
        return false;
    }


    $("#titlevalidate").removeClass("validatefield");
    $("#datevalidate").removeClass("validatefield");
    $("#storyvalidate").removeClass("validatefield");
    $("#videourlvalidate").removeClass("validatefield");
    $("#storytypevalidate").removeClass("validatefield");

    var regexforvideo = /^(https?\:\/\/)?(www\.)?(youtube\.com|youtu\.?be)\/.+$/;

    if (!regexforvideo.test(videourl)) {
        $('#videourlvalidate').addClass('text-danger');
        return false;
    } else {
        $('#videourlvalidate').removeClass('text-danger');
    }

    if (imageFiles.length === 0) {
        alert("upload atleast one image")
        $("#uploadvalidate").addClass("validatefield");
        return false;
    }
    else {
        $("#uploadvalidate").removeClass("validatefield");
    }

    $.ajax({
        type: "POST",
        url: "/Story/Storydatasave",
        processData: false,
        contentType: false,
        data: formData,
        success: function () {
            console.log("success");
            alert("story saved successfully!");
            $("#submitsharestory").prop("disabled", false);
            $("#submitsharestorydesign button").removeClass("designpublishedbtn");
            $("#previewstory").removeClass("previewbtnstyle");
            imageFiles = [];
            return false;
        },
        error: function () {
            console.log("error");
            alert("Fill proper data!");
        }
    });

});

// submit share story 
$("#submitsharestory").click(function () {
    var missionId = $("#mission-select option:selected").val();

    var title = $("#sharestorytitle").val();

    var date = $("#sharestorydate").val();

    var discription = data.getData();

    var videourl = $("#sharestoryvideourl").val();

    var storytype = $("#sharestorytype").val();

    // Retrieve the base64 encoded image data from the hidden input fields
    //var imageData = [];
    //$('input[name="imageData[]"]').each(function () {
    //    imageData.push(JSON.parse($(this).val()));
    //});
    // Extract an array of image names from the imageData array
    //var imageNames = imageData.map(function (data) {
    //    return data.name;
    //});

    //console.log(imageNames);

    if (missionId == "Select your Mission") {
        $("#missionvalidate").addClass("validatefield");
        alert("first select your mission!")
        return false;
    }
    else {
        $("#missionvalidate").removeClass("validatefield");
    }

    // Check if any of the required fields are empty
    if (!title || !date || !discription || !videourl || !storytype) {

        // Show the required fields in red

        if (!title) $("#titlevalidate").addClass("validatefield");
        if (!date) $("#datevalidate").addClass("validatefield");

        if (!discription) $("#storyvalidate").addClass("validatefield");
        if (!videourl) $("#videourlvalidate").addClass("validatefield");
        if (!storytype) $("#storytypevalidate").addClass("validatefield");

        alert("Please first fill in all required fields!");
        return false;
    }


    $("#titlevalidate").removeClass("validatefield");
    $("#datevalidate").removeClass("validatefield");
    $("#storyvalidate").removeClass("validatefield");
    $("#videourlvalidate").removeClass("validatefield");
    $("#storytypevalidate").removeClass("validatefield");

    //if (imageNames.length === 0) {
    //    alert("upload atleast one image")
    //    $("#uploadvalidate").addClass("validatefield");
    //    return false;
    //}
    //else {
    //    $("#uploadvalidate").removeClass("validatefield");
    //}

    //get the first image name
    //var firstName = imageData[0].name;
    //console.log(firstName);

    $.ajax({
        type: "POST",
        url: "/Story/Storydata",
        data: {
            missionId: missionId,
            title: title,
            date: date,
            discription: discription,
            videourl: videourl,
            storytype: storytype,
            //firstName: firstName,
            /*imageNames: imageNames.join()*/
        },
        success: function () {
            console.log("success");
            alert("story upload successfully!");

            $("#submitsharestory").prop("disabled", true);
            $("#submitsharestorydesign button").addClass("designpublishedbtn");

            $("#savesharestory").prop("disabled", true);
            $("#savesharestory").addClass("designpublishedbtn");

            $("#previewstory").addClass("previewbtnstyle");
        },
        error: function () {
            console.log("error");
            alert("Fill proper data!");
        }
    });

});

//according to selected mission change save and submit and preview button
$(document).ready(function () {
    $('#mission-select').on('change', function () {
        var selectedMissionId = $(this).val();
        // Perform AJAX call with selectedMissionId
        $.ajax({
            type: "POST",
            url: "/Story/Buttonstatus",
            data: {
                selectedMissionId: selectedMissionId
            },
            success: function (data) {
                console.log("success");
                console.log(data);
                if (data == "published") {
                    console.log("published available");

                    // Disable the button and change its color
                    $("#submitsharestory").prop("disabled", true);
                    $("#submitsharestorydesign button").addClass("designpublishedbtn");

                    $("#savesharestory").prop("disabled", true);
                    $("#savesharestory").addClass("designpublishedbtn");

                    $("#previewstory").addClass("previewbtnstyle");


                }
                else {
                    console.log("draft available");
                    $("#submitsharestory").prop("disabled", false);
                    $("#submitsharestorydesign button").removeClass("designpublishedbtn");

                    $("#savesharestory").prop("disabled", false);
                    $("#savesharestory").removeClass("designpublishedbtn");

                    $("#previewstory").removeClass("previewbtnstyle");
                }
            },
            error: function () {
                console.log("error");
                $("#submitsharestory").prop("disabled", true);
                $("#submitsharestorydesign button").addClass("designpublishedbtn");

                $("#savesharestory").prop("disabled", false);
                $("#savesharestory").removeClass("designpublishedbtn");

                $("#previewstory").addClass("previewbtnstyle");

            }
        });
    });
});

//recommended co-worker in story details 
$(".storyrecobtn").click(function () {

    var recoemail = $(this).val();
    var recomstoryid = $('.selectedstoryid2').text();
    $.ajax({
        url: '/Story/recommendedcoworkers',
        type: 'POST',
        data: { recoemail: recoemail, recomstoryid: recomstoryid },
        // Show a success message or update the UI
        success: function (result) {

            var successMsg2 = "Email successfully sent!";
            if (result.success) {

                // Show the success message in a pop-up
                swal({
                    title: "Success",
                    text: successMsg2,
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
        error: function (error) {
            // Show an error message or handle the error
            console.log("error");

        }
    });


});

// Draft data show
$('#mission-select').change(function () {

    var choosemissionId = $("#mission-select option:selected").val();

    $.ajax({
        url: '/Story/Draftdatashow',
        type: 'POST',
        data: { choosemissionId: choosemissionId },
        // Show a success message or update the UI
        success: function (result) {
            if (result.success) {
                var data = result.data;
                $("#sharestorytitle").val(data.title);

                // create the date object with the timezone offset adjustment
                var date = new Date(data.date);
                var timezoneOffset = date.getTimezoneOffset();
                date.setMinutes(date.getMinutes() - timezoneOffset);

                // format the date and set the value of the date input field
                var day = date.getDate();
                var month = (date.getMonth() + 1).toString().padStart(2, '0');
                var year = date.getFullYear();
                var formattedDate = year + "-" + month + "-" + day.toString().padStart(2, '0');
                $("#sharestorydate").val(formattedDate);

                $("#sharestoryvideourl").val(data.url);
                $("#sharestorytype").val(data.status);

                var scriptElement = $('.story_ckeditor').remove();
                var description = data.description.replace(/<\/?[^>]+(>|$)/g, "");

                // create a div element with the class "story_ckeditor"
                var storyDiv = $('<div>', { class: 'story_ckeditor' });

                // create a label element with the text "My Story" and append it to the storyDiv
                var label = $('<label>', { for: 'editor', id: 'storyvalidate' }).text('My Story');
                storyDiv.append(label);

                // create a textarea element with the id "editor" and append it to the storyDiv
                var textarea = $('<textarea>', { id: 'editor', rows: 5, cols: 50, placeholder: 'Enter your text here...' });
                textarea.val(description);
                storyDiv.append(textarea);

                // create a script element and append it to the storyDiv
                var script = $('<script>').text('var data; ClassicEditor.create(document.querySelector("#editor")).then(editor => {data = editor;}).catch(error => {console.error(error);});');
                storyDiv.append(script);

                // append the storyDiv to a container, for example, the body element
                $('#specialckeditor').append(storyDiv);

                //var images = data.storydraftimages;

            }
            else {
                $("#sharestorytitle").val('');
                $("#sharestorydate").val('');
                $("#sharestoryvideourl").val('');
                $("#sharestorytype").val('');
            }
        },
        error: function (error) {
            // Show an error message or handle the error
            console.log("error");
        }
    });

});

//preview buttton
$("#previewbtnclick").click(function () {

    var datamid = $("#mission-select option:selected").val();

    window.location.href = "/Story/previewdata?id=" + datamid;

});



//-------------------------------------------------------------------------------------

//image upload
const uploadContainer = document.querySelector('.upload-container');
const previewContainer = document.querySelector('#preview-container');

// Prevent default drag behaviors
['dragenter', 'dragover', 'dragleave', 'drop'].forEach(eventName => {
    uploadContainer.addEventListener(eventName, e => {
        e.preventDefault();
        e.stopPropagation();
    });
});

// Highlight drop area when item is dragged over
['dragenter', 'dragover'].forEach(eventName => {
    uploadContainer.addEventListener(eventName, e => {
        uploadContainer.classList.add('highlight');
    });
});

['dragleave', 'drop'].forEach(eventName => {
    uploadContainer.addEventListener(eventName, e => {
        uploadContainer.classList.remove('highlight');
    });
});

// Handle dropped files
uploadContainer.addEventListener('drop', e => {
    const files = e.dataTransfer.files;
    handleFiles(files);
});

// Handle selected files
document.querySelector('#file-upload').addEventListener('change', e => {
    const files = e.target.files;
    handleFiles(files);
});

const imageFiles = [];
function handleFiles(files) {

    for (let i = 0; i < files.length; i++) {
        const file = files[i];
        if (file.type.startsWith('image/')) {
            const reader = new FileReader();
            reader.onload = e => {
                const image = new Image();
                image.src = e.target.result;
                image.addEventListener('load', () => {
                    const previewImage = document.createElement('div');
                    previewImage.classList.add('preview-image');
                    previewImage.innerHTML = `
                <img src="${image.src}">
                <div class="image-tag">${file.name}</div>
                <button class="cancel-btn">x</button>
              `;
                    previewContainer.appendChild(previewImage);
                    const cancelButton = previewImage.querySelector('.cancel-btn');
                    cancelButton.addEventListener('click', () => {
                        previewImage.remove();
                    });

                    // Append the image file to the array
                    imageFiles.push(file);
                    console.log(imageFiles);
                });
            };
            reader.readAsDataURL(file);
        }
    }
}

