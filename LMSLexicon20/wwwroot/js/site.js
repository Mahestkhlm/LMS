// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function DisplayAddTeacherSuccess() {
    $("#addteachersuccess").html("En lärare har tilldelats").fadeOut(3000);
}

function DisplayRemoveTeacherSuccess() {
    $("#removeteachersuccess").html("Läraren har tagits bort").fadeOut(3000);
}

function DisplayCreateModuleSuccess() {
    $("#createmodule").children().remove();
    $("#modulesuccess").show().html("Modulen har skapats").fadeOut(3000);
};
