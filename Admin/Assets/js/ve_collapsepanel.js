$(function () {
    $('.slide-panel').on('click', '.btn-show', function () {
        var panel = $('.slide-panel', $(this).parents());
        $('.left', $(panel)).css({ width: '30%', display: 'block' });
        $('.right', $(panel)).css({ width: '70%' });
        $('.btn-show', $(panel)).hide();
        $('.btn-hide', $(panel)).show();
    });

    $('.slide-panel').on('click', '.btn-hide', function () {
        var panel = $('.slide-panel', $(this).parents());
        $('.left', $(panel)).css({ width: '0', display: 'none' });
        $('.right', $(panel)).css({ width: '100%' });
        $('.btn-show', $(panel)).show();
        $('.btn-hide', $(panel)).hide();
    });
});