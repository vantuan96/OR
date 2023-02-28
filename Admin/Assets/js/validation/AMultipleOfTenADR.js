$.validator.unobtrusive.adapters.add('amultipleoftenadr', [], function (options) {
    options.rules['amultipleoftenadr'] = options.params;
    options.messages['amultipleoftenadr'] = options.message;
});

$.validator.addMethod('amultipleoftenadr', function (value, element, params) {
    var number = parseInt(value);
    return (number % 10 == 0);
});

