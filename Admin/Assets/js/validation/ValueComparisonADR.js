$.validator.unobtrusive.adapters.add('valuecomparisonadr', ['otherpropertyname', 'comparison'], function (options) {
    options.rules['valuecomparisonadr'] = options.params;
    options.messages['valuecomparisonadr'] = options.message;
});

$.validator.addMethod('valuecomparisonadr', function (value, element, params) {
    var isValid = false;
    var comparison = params.comparison;
    var otherElement = '#' + params.otherpropertyname;
    var otherValue = $(otherElement).val();
    value = $.trim(value);
    otherValue = $.trim(otherValue);

    if (comparison == 'IsEqual') {
        isValid = value == otherValue;
    }
    else if (comparison == 'IsNotEqual') {
        isValid = value != otherValue;
    }
    else {
        var number = parseInt(value);
        var otherNumber = parseInt(otherValue);

        switch (comparison) {
            case 'IsGreaterThan':
                isValid = number > otherNumber;
                break;
            case 'IsGreaterThanOrEqual':
                isValid = number >= otherNumber;
                break;
            case 'IsLessThan':
                isValid = number < otherNumber;
                break;
            case 'IsLessThanOrEqual':
                isValid = number <= otherNumber;
                break;
        }
    }

    return isValid;
});

