﻿var LoginView = function () {

    var that = this;

    this.init = function () {
        $('#btn-login').on('click', that.submitForm);
        $('#password').on('keypress', function (e) {
            if (e.which == 13) {
                that.submitForm();
                return false; 
            }
        });
    };

    this.submitForm = function () {
        //debugger;
        var frm = $('#frm-login');
        $("#msg-error").addClass('hidden');
        $('#btn-login').prop('disabled', true);
        $('#spinner').removeClass("hide");
        var formData = {
            userName: $('#userName').val(),
            password: $('#password').val(),
        };
        var request = $.ajax({
            url: frm.attr('action'),
            method: "POST",
            data: formData
        });

        request.always(function(xhr, textStatus, errorThrown) { 
            $('#btn-login').prop('disabled', false);
            $('#spinner').addClass("hide");
        });
        request.done(function (response) {
            if (response.success === false) {
                Utils.showError("#msg-error", 'Unable to sign in using the supplied email address and password');
            }
            else {
                window.location.assign($('#returnUrl').val());
            }
        });

        request.fail(function (xhr, textStatus) {
            try {
                Utils.showError("#msg-error", xhr.responseJSON.message);
            }
            catch(err) {
                Utils.showError("#msg-error", 'A fatal error occurred');
            }
        });
    };

}


$(document).ready(function()
{
    var lv = new LoginView();
    lv.init();
});