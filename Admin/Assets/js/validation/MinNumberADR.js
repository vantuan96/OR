$.validator.unobtrusive.adapters.add('minnumberadr', ['min'], function (options) {
    options.rules['minnumberadr'] = options.params;
    options.messages['minnumberadr'] = options.message;
});

$.validator.addMethod('minnumberadr', function (value, element, params) {
    var number = parseInt(value);
    var min = parseInt(params.min);

    return (number >= min);
});

