$(document).ready(function(){

    

    $("#selectMemberCount").on("change", function(){
        let memClass = $(".member-class");
        memClass.hide();
        let optionValue = $(this).val();
        for(let i = 0 ; i < optionValue; i++){
            memClass.eq(i).fadeIn(1500);
        }
    }); 
    $('.input-daterange').datepicker({
        format: 'dd-mm-yyyy',
        autoclose: true,
        calendarWeeks : true,
        clearBtn: true,
        disableTouchKeyboard: true
        });   
});

function chatRedirect(){
    window.location.replace("./chat-fields.html");
}
function sLoginRedirect(){
    window.location.replace("./pages/dashboard.html");
}
function sRegRedirect(){
    window.location.replace("./pages/student-registration.html");
}
function supRegRedirect(){
    window.location.replace("./supervisor-registration.html");
}