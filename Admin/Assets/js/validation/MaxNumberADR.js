$.validator.unobtrusive.adapters.add('maxnumberadr', ['max'], function (options) {
    options.rules['maxnumberadr'] = options.params;
    options.messages['maxnumberadr'] = options.message;
});

$.validator.addMethod('maxnumberadr', function (value, element, params) {
    var number = parseInt(value);
    var max = parseInt(params.max);

    return (number <= max);
});

