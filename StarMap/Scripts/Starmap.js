(function ($) {
    $(".confirmDelete").click(function () {
        if (confirm(smMessage.deleteConfirm)) {
            return true;
        }
        return false;
    });
    $("ul#admin-menu li a").each(function () {
        var item = $(this);
        if ((item.attr('href') + ' ').indexOf(getController() + ' ') != -1 || (item.attr('href') + '/').indexOf(getController() + '/') != -1 || (item.attr('href') + '?').indexOf(getController() + '?') != -1) {
            item.parent().addClass('active');
        }
    });
    $("#cboChangeLanguage").change(function () {
        $(this).parents("form").submit(); // post form
    });

})(jQuery);