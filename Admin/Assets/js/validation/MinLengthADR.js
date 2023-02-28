$.validator.unobtrusive.adapters.add('minlengthadr', ['min'], function (options) {
    options.rules['minlengthadr'] = options.params;
    options.messages['minlengthadr'] = options.message;
});

$.validator.addMethod('minlengthadr', function (value, element, params) {
    var str = value;
    var min = parseInt(params.min);
    return (str.length >= min);
});