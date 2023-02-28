$.validator.unobtrusive.adapters.add('rangenumberadr', ['min', 'max'], function (options) {
    options.rules['rangenumberadr'] = options.params;
    options.messages['rangenumberadr'] = options.message;
});

$.validator.addMethod('rangenumberadr', function (value, element, params) {
    var number = parseInt(value);
    var min = parseInt(params.min);
    var max = parseInt(params.max);

    return (number >= min && number <= max);
});

