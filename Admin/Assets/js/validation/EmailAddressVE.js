$.validator.unobtrusive.adapters.add('emailaddressadr', ['email'], function (options) {
    options.rules['emailaddressadr'] = options.params;
    options.messages['emailaddressadr'] = options.message;
});

$.validator.addMethod('emailaddressadr', function (value, element, params) {
    var patternEmail = new RegExp(params.email);
    return patternEmail.test(value);
});

