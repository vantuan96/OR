$.validator.unobtrusive.adapters.add('intnumberadr', [], function (options) {
    options.rules['intnumberadr'] = options.params;
    options.messages['intnumberadr'] = options.message;
});

$.validator.addMethod('intnumberadr', function (value, element, params) {
    return /^\-?[\d]*$/.test(value);
});

