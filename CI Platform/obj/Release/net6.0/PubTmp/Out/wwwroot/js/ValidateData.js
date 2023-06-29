
/*email-validation*/

function email_validator() {
    var login_email_data = document.getElementById('exampleInputEmail1').value;
    var validRegex = /^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/;

    if (login_email_data.match(validRegex)) {
        document.getElementById('login_email_lable').style.display = "none";
    }
    else {
        document.getElementById('login_email_lable').style.display = "block";

    }
}
/*password-validation with min 8 ch + Upper + Special + lower */

function password_validator() {

    var login_password_data = document.getElementById('exampleInputPassword1').value;
    var validRegexpasssword = /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z0-9])(?!.*\s).{8,15}$/;

    if (login_password_data.match(validRegexpasssword)) {
        document.getElementById('login_password_lable').style.display = "none";
    }
    else {
        document.getElementById('login_password_lable').style.display = "block";

    }
}