// List View show for desktop
function listview() {
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
        elements4[i].style.left = "28%";
        elements4[i].style.top = "56%";

    }
    var elements5 = document.getElementsByClassName('list_view_user');
    for (var i = 0; i < elements.length; i++) {
        elements5[i].style.left = "28%";
        elements5[i].style.top = "71%";

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
        elements4[i].style.top = "27%";

    }
    var elements5 = document.getElementsByClassName('list_view_user');
    for (var i = 0; i < elements.length; i++) {
        elements5[i].style.left = "90%";
        elements5[i].style.top = "34%";

    }
}