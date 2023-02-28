var adr_modal = {
    prompt: function(options, callback) {
        var adr_modal_prompt_result = null;
        var defaultOptions = {
            title: '',
            fieldName: 'Lý do',
            isRequired: false,
            minLength: 0,
            maxLength: 0
        };

        if (typeof(options.fieldName) == undefined || options.fieldName == null) {
            options.fieldName = defaultOptions.fieldName;
        }
        if (typeof(options.isRequired) == undefined || options.isRequired == null) {
            options.isRequired = defaultOptions.isRequired;
        }
        if (typeof(options.minLength) == undefined || options.minLength == null) {
            options.minLength = defaultOptions.minLength;
        }
        if (typeof(options.maxLength) == undefined || options.maxLength == null) {
            options.maxLength = defaultOptions.maxLength;
        }

        $('#adr_modal_prompt .modal-title').text(options.title);
        $('#adr_modal_prompt').modal('show');

        if (options.isRequired) {
            $('#frm_adrModalPrompt #frm_adrModalPrompt_reason').rules('add', {
                required: true,
                messages: {
                    required: options.fieldName + ' là thông tin bắt buộc'
                }
            });
        }

        if (options.minLength > 0 && options.maxLength > 0) {
            $('#frm_adrModalPrompt #frm_adrModalPrompt_reason').rules('add', {
                minlength: options.minLength,
                maxlength: options.maxLength,
                messages: {
                    minlength: options.fieldName + ' phải từ ' + options.minLength + ' đến ' + options.maxLength + ' kí tự',
                    maxlength: options.fieldName + ' phải từ ' + options.minLength + ' đến ' + options.maxLength + ' kí tự'
                }
            });
        } else if (options.minLength > 0) {
            $('#frm_adrModalPrompt #frm_adrModalPrompt_reason').rules('add', {
                minlength: options.minLength,
                messages: {
                    minlength: options.fieldName + ' phải có độ dài tối đa ' + options.minLength + ' kí tự'
                }
            });
        } else if (options.maxLength > 0) {
            $('#frm_adrModalPrompt #frm_adrModalPrompt_reason').rules('add', {
                maxlength: options.maxLength,
                messages: {
                    maxlength: options.fieldName + ' phải có độ dài tối thiểu ' + options.maxLength + ' kí tự'
                }
            });
        }

        $('#frm_adrModalPrompt').submit(function(e) {
            e.preventDefault();

            if ($(this).valid()) {
                adr_modal_prompt_result = $('#frm_adrModalPrompt #frm_adrModalPrompt_reason').val();
                adr_modal_prompt_result = $.trim(adr_modal_prompt_result);

                $('#adr_modal_prompt .close').trigger('click');
            }
        });

        $('#adr_modal_prompt .close').click(function(e) {
            $('#frm_adrModalPrompt').unbind('submit');
            $('#adr_modal_prompt .close').unbind('click');

            $('#frm_adrModalPrompt #frm_adrModalPrompt_reason').val('');
            $('#frm_adrModalPrompt .input-validation-error').removeClass('input-validation-error');
            $('#frm_adrModalPrompt [data-valmsg-for=frm_adrModalPrompt_reason]').html('');
            $('#frm_adrModalPrompt #frm_adrModalPrompt_reason').rules("remove");

            callback(adr_modal_prompt_result);
        });
    }
};

$('#adr_modal_prompt #frm_adrModalPrompt button[type=reset]').click(function (e) {
    $('#adr_modal_prompt .close').trigger('click');
});
