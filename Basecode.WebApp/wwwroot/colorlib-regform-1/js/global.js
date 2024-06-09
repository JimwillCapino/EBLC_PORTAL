(function ($) {
    'use strict';
    /*==================================================================
        [ Daterangepicker ]*/
    try {
        // Initialize the first datepicker
        $('.js-datepicker').daterangepicker({
            "singleDatePicker": true,
            "showDropdowns": true,
            "autoUpdateInput": false,
            locale: {
                format: 'DD/MM/YYYY'
            },
        });

        // Initialize the second datepicker
        $('.js-datepicker-parent').daterangepicker({
            "singleDatePicker": true,
            "showDropdowns": true,
            "autoUpdateInput": false,
            locale: {
                format: 'DD/MM/YYYY'
            },
        });

        // Handle clicks outside to close datepickers
        var isClick = { birthday: 0, parentBirthday: 0 };

        $(window).on('click', function () {
            isClick.birthday = 0;
            isClick.parentBirthday = 0;
        });

        // Apply date for the first datepicker
        $('.js-datepicker').on('apply.daterangepicker', function (ev, picker) {
            isClick.birthday = 0;
            $(this).val(picker.startDate.format('DD/MM/YYYY'));
        });

        // Apply date for the second datepicker
        $('.js-datepicker-parent').on('apply.daterangepicker', function (ev, picker) {
            isClick.parentBirthday = 0;
            $(this).val(picker.startDate.format('DD/MM/YYYY'));
        });

        // Show the first datepicker on button click
        $('.js-btn-calendar').on('click', function (e) {
            e.stopPropagation();
            if (isClick.birthday === 1) isClick.birthday = 0;
            else if (isClick.birthday === 0) isClick.birthday = 1;

            if (isClick.birthday === 1) {
                $('.js-datepicker').focus();
            }
        });

        // Show the second datepicker on button click
        $('.js-btn-calendar-parent').on('click', function (e) {
            e.stopPropagation();
            if (isClick.parentBirthday === 1) isClick.parentBirthday = 0;
            else if (isClick.parentBirthday === 0) isClick.parentBirthday = 1;

            if (isClick.parentBirthday === 1) {
                $('.js-datepicker-parent').focus();
            }
        });

        // Stop propagation for the first datepicker
        $('.js-datepicker').on('click', function (e) {
            e.stopPropagation();
            isClick.birthday = 1;
        });

        // Stop propagation for the second datepicker
        $('.js-datepicker-parent').on('click', function (e) {
            e.stopPropagation();
            isClick.parentBirthday = 1;
        });

        // Stop propagation for the daterangepicker containers
        $(document).on('click', '.daterangepicker', function (e) {
            e.stopPropagation();
        });

    } catch (er) {
        console.log(er);
    }

    /*[ Select 2 Config ]
        ===========================================================*/
    try {
        var selectSimple = $('.js-select-simple');

        selectSimple.each(function () {
            var that = $(this);
            var selectBox = that.find('select');
            var selectDropdown = that.find('.select-dropdown');
            selectBox.select2({
                dropdownParent: selectDropdown
            });
        });

    } catch (err) {
        console.log(err);
    }

})(jQuery);