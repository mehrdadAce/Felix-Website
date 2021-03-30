function InitNewCheckboxes() {
    $(function () {
        var checkboxs = $('input[type=checkbox]');

        function HideModelSection() {
            if ($("#search-models").has("input").length == 0) {
                $("#search-models").css("display", "none");
            } else {
                $("#search-models").css("display", "block");
            }
        }

        checkboxs.each(function () {
            if ($(this).parent(".customCheckbox").length == 0) {
                $(this).wrap('<div class="customCheckbox"></div>');
                $(this).before('<span>&#10004;</span>');
                if ($(this).is(':checked')) {
                    $(this).parent().addClass('customCheckboxChecked');
                }
                if ($(this).prop('disabled')) {
                    $(this).parent().addClass('customCheckboxDisabled');
                }
            }

            HideModelSection();
        });

        checkboxs.change(function () {
            if ($(this).is(':checked')) {
                $(this).parent().addClass('customCheckboxChecked');
            } else {
                $(this).parent().removeClass('customCheckboxChecked');
            }

            if ($(this).prop('disabled')) {
                $(this).parent().addClass('customCheckboxDisabled');
            }
            
            HideModelSection();
        });

        const $checkboxes = $('input[type="checkbox"].car-compare-cbx');

        $checkboxes.change(function () {
            var countCheckedCheckboxes = $checkboxes.filter(':checked').length;
            if (countCheckedCheckboxes < 2) {
                $('#car-compare-btn').prop('disabled', true);
                toastr.info('Gelieve minimum 2 wagens te selecteren.');
            } else if (countCheckedCheckboxes > 5) {
                $('#car-compare-btn').prop('disabled', true);
                toastr.warning('Gelieve maximum 5 wagens te selecteren.');
            } else {
                $('#car-compare-btn').prop('disabled', false);
            }
        });
    });
};