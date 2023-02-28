$.validator.unobtrusive.adapters.add('notmarkadr', ['mark'], function (options) {
    options.rules['notmarkadr'] = options.params;
    options.messages['notmarkadr'] = options.message;
});

$.validator.addMethod('notmarkadr', function (value, element, params) {
    var patternNotMark = new RegExp(params.mark);
    return patternNotMark.test(value);
});

