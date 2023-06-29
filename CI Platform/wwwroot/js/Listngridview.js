// List View show for desktop
function listview() {

    var buttonElements = document.getElementsByClassName('grid_button');

    for (var i = 0; i < buttonElements.length; i++) {
        buttonElements[i].style.background = "white";
    }


    var buttonElements = document.getElementsByClassName('list_button');

    for (var i = 0; i < buttonElements.length; i++) {
        buttonElements[i].style.background = "gray";
    }

    var elements = document.getElementsByClassName('quote1');
    for (var i = 0; i < elements.length; i++) {
        elements[i].classList.remove("col-xl-4");
        elements[i].classList.add("col-xl-12");
        elements[i].style.paddingBottom = "25px";

    }
    var elements2 = document.getElementsByClassName('demo');
    for (var i = 0; i < elements.length; i++) {
        elements2[i].classList.remove("flex-column");
        elements2[i].classList.add("flex-row");

    }
    var elements3 = document.getElementsByClassName('list_view_location');
    for (var i = 0; i < elements.length; i++) {
        elements3[i].style.left = "22%";
        elements3[i].style.top = "4%";

    }
    var elements4 = document.getElementsByClassName('list_view_heart');
    for (var i = 0; i < elements.length; i++) {
        elements4[i].style.left = "29%";
        elements4[i].style.top = "61%";

    }
    var elements5 = document.getElementsByClassName('list_view_user');
    for (var i = 0; i < elements.length; i++) {
        elements5[i].style.left = "29%";
        elements5[i].style.top = "75%";

    }
    var elements6 = document.getElementsByClassName('card-img-top');
    for (var i = 0; i < elements.length; i++) {
        elements6[i].style.height = "30vh";

    }
    var elements6 = document.getElementsByClassName('for_list_viewdiv');
    for (var i = 0; i < elements.length; i++) {
        elements6[i].style.minWidth = "32%";

    }
}

// Grid View show for desktop
function gridview() {


    var buttonElements = document.getElementsByClassName('list_button');

    for (var i = 0; i < buttonElements.length; i++) {
        buttonElements[i].style.background = "white";
    }

    var buttonElements = document.getElementsByClassName('grid_button');

    for (var i = 0; i < buttonElements.length; i++) {
        buttonElements[i].style.background = "#d5d4d4";
    }

    var elements = document.getElementsByClassName('quote1');
    for (var i = 0; i < elements.length; i++) {
        elements[i].classList.remove("col-xl-12");
        elements[i].classList.add("col-xl-4");
        elements[i].style.paddingBottom = "0px";

    }
    var elements2 = document.getElementsByClassName('demo');
    for (var i = 0; i < elements.length; i++) {
        elements2[i].classList.remove("flex-row");
        elements2[i].classList.add("flex-column");

    }
    var elements3 = document.getElementsByClassName('list_view_location');
    for (var i = 0; i < elements.length; i++) {
        elements3[i].style.left = "70%";
        elements3[i].style.top = "2%";

    }
    var elements4 = document.getElementsByClassName('list_view_heart');
    for (var i = 0; i < elements.length; i++) {
        elements4[i].style.left = "90%";
        elements4[i].style.top = "28%";

    }
    var elements5 = document.getElementsByClassName('list_view_user');
    for (var i = 0; i < elements.length; i++) {
        elements5[i].style.left = "90%";
        elements5[i].style.top = "35%";

    }
}



// Update

$(document).ready(function () {
    const sortableList = $(".sortable-list");
    const items = sortableList.find(".item");

    items.on("dragstart", function () {
        // Adding dragging class to item after a delay
        const item = $(this);
        setTimeout(function () {
            item.addClass("dragging");
        }, 0);
    });

    items.on("dragend", function () {
        // Removing dragging class from item on dragend event
        $(this).removeClass("dragging");
    });

    sortableList.on("dragover", function (e) {
        e.preventDefault();
    });

    sortableList.on("drop", function (e) {
        e.preventDefault();
        const draggingItem = $(".dragging");
        const siblings = $(this).find(".item:not(.dragging)");

        const nextSibling = siblings.toArray().find(function (sibling) {
            return e.clientY <= sibling.offsetTop + sibling.offsetHeight / 2;
        });

        if (nextSibling) {
            $(nextSibling).before(draggingItem);
        } else {
            sortableList.append(draggingItem); // Append to the end if no nextSibling found
        }

        // Get the friend's ID and current order
        const friendId = draggingItem.data("item-id");
        const newOrder = draggingItem.index();

        $.ajax({
            url: "/Mission/UpdateFriendOrder",
            type: "POST",
            data: { friendId: friendId, newOrder: newOrder },
            success: function (data) {
                if (data.success) {
                    window.location.href = "/Mission/DragDrop_Mission_Sort";
                }
            },
            error: function (response) {
                if (response.error) {
                    alert("Something went wrong...");
                }
            }
        });
    });
});


// User by the Sortable.js file 

//document.addEventListener('DOMContentLoaded', function () {
//    var sortableContainer = document.querySelector('.sortable');
//    var scrollContainer = document.querySelector('.scrollable-container');
//    var sortable = Sortable.create(sortableContainer, {
//        scroll: scrollContainer,
//        onEnd: function (event) {
//            // Get the mission ID of the dragged item
//            var missionId = event.item.querySelector('.missionid').textContent;

//            // Get the new index of the dragged item
//            var newIndex = (event.newIndex + 1);

//            // Send the mission ID and new index to your desired function for further processing
//            //handleSortChange(missionId, newIndex);
//        }
//    });
//});


// sortable.js  used 

$(document).ready(function () {
    var sortableContainer = $('.sortable');
    var scrollContainer = $('.scrollable-container');
    var sortable = new Sortable(sortableContainer.get(0), {
        scroll: scrollContainer.get(0),
        onEnd: function (event) {
            // Get the mission ID of the dragged item
            var missionId = $(event.item).find('.missionid').text();

            // Get the new index of the dragged item
            var newIndex = event.newIndex + 1;

            $.ajax({
                url: '/Mission/UpdateSortOrderbylib',
                type: 'POST',
                data: { missionId: missionId, newIndex: newIndex },
                success: function (response) {
                    if (response.success) {
                        window.location.href = "/Mission/SortableJSDragDrop";
                    }
                    else {

                    }
                },
                error: function (xhr, status, error) {
                    console.error('Error:', error);
                }
            });
        }
    });
});


// jquery ui sortable code

//$(document).ready(function () {
//    var sortableContainer = $('.sortable');
//    var scrollContainer = $('.scrollable-container');

//    sortableContainer.sortable({
//        scroll: scrollContainer,
//        update: function (event, ui) {
//            // Get the mission ID of the dragged item
//            var missionId = $(ui.item).find('.missionid').text();

//            // Get the new index of the dragged item
//            var newIndex = ui.item.index() + 1;

//            $.ajax({
//                url: '/Mission/UpdateSortOrderbylib',
//                type: 'POST',
//                data: { missionId: missionId, newIndex: newIndex },
//                success: function (response) {
//                    window.location.href = "/Mission/SortableJSDragDrop";
//                },
//                error: function (xhr, status, error) {
//                    console.error('Error:', error);
//                }
//            });
//        }
//    });
//});
