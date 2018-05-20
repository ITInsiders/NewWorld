/**
 * bootstrap-multiselect.js
 * https://github.com/davidstutz/bootstrap-multiselect
 *
 * Copyright 2012, 2013 David Stutz
 * 
 * Dual licensed under the BSD-3-Clause and the Apache License, Version 2.0.
 */
function init() {
    !function ($) {

        "use strict";// jshint ;_;

        if (typeof ko != 'undefined' && ko.bindingHandlers && !ko.bindingHandlers.multiselect) {
            ko.bindingHandlers.multiselect = {
                init: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) { },
                update: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
                    var ms = $(element).data('multiselect');
                    if (!ms) {
                        $(element).multiselect(ko.utils.unwrapObservable(valueAccessor()));
                    }
                    else if (allBindingsAccessor().options && allBindingsAccessor().options().length !== ms.originalOptions.length) {
                        ms.updateOriginalOptions();
                        $(element).multiselect('rebuild');
                    }
                }
            };
        }

        function Multiselect(select, options) {

            this.options = this.mergeOptions(options);
            this.$select = $(select);

            // Initialization.
            // We have to clone to create a new reference.
            this.originalOptions = this.$select.clone()[0].options;
            this.query = '';
            this.searchTimeout = null;

            this.options.multiple = this.$select.attr('multiple') == "multiple";
            this.options.onChange = $.proxy(this.options.onChange, this);

            // Build select all if enabled.
            this.buildContainer();
            this.buildButton();
            this.buildSelectAll();
            this.buildDropdown();
            this.buildDropdownOptions();
            this.buildFilter();
            this.updateButtonText();

            this.$select.hide().after(this.$container);
        };

        Multiselect.prototype = {

            // Default options.
            defaults: {
                // Default text function will either print 'None selected' in case no
                // option is selected, or a list of the selected options up to a length of 3 selected options.
                // If more than 3 options are selected, the number of selected options is printed.
                buttonText: function (options, select) {
                    if (options.length == 0) {
                        return 'Не выбрано <b class="caret"></b>';
                    }
                    else {
                        var selected = '';
                        options.each(function () {
                            var label = ($(this).attr('label') !== undefined) ? $(this).attr('label') : $(this).html();
                            switch (label) {
                                case "Понедельник": label = "ПН"; break;
                                case "Вторник": label = "ВТ"; break;
                                case "Среда": label = "СР"; break;
                                case "Четверг": label = "ЧТ"; break;
                                case "Пятница": label = "ПТ"; break;
                                case "Суббота": label = "СБ"; break;
                                case "Воскресенье": label = "ВС"; break;
                            }
                            selected += label + ', ';
                        });
                        selected = selected.substr(0, selected.length - 2);
                        switch (selected) {
                            case "ПН, ВТ, СР, ЧТ, ПТ": selected = "По будням"; break;
                            case "СБ, ВС": selected = "Выходные"; break;
                            case "ПН, ВТ, СР, ЧТ, ПТ, СБ, ВС": selected = "Ежедневно"; break;
                        }
                        if (options.length > 3 && selected != "По будням" && selected != "Ежедневно")
                            return options.length + ' выбрано <b class="caret"></b>';
                        else return selected + ' <b class="caret"></b>';
                    }
                },
                // Like the buttonText option to update the title of the button.
                buttonTitle: function (options, select) {
                    if (options.length == 0) {
                        return this.nonSelectedText;
                    }
                    else {
                        var selected = '';
                        options.each(function () {
                            selected += $(this).text() + ', ';
                        });
                        return selected.substr(0, selected.length - 2);
                    }
                },
                // Is triggered on change of the selected options.
                onChange: function (option, checked) {

                },
                buttonClass: 'btn',
                dropRight: false,
                selectedClass: 'active',
                buttonWidth: 'auto',
                buttonContainer: '<div class="btn-group" />',
                // Maximum height of the dropdown menu.
                // If maximum height is exceeded a scrollbar will be displayed.
                maxHeight: false,
                includeSelectAllOption: false,
                selectAllText: ' Select all',
                selectAllValue: 'multiselect-all',
                enableFiltering: false,
                enableCaseInsensitiveFiltering: false,
                filterPlaceholder: 'Search',
                // possible options: 'text', 'value', 'both'
                filterBehavior: 'text',
                preventInputChangeEvent: false,
                nonSelectedText: 'Non selected',
                nSelectedText: 'selected'
            },

            // Templates.
            templates: {
                button: '<button type="button" class="multiselect dropdown-toggle" data-toggle="dropdown"></button>',
                ul: '<ul class="multiselect-container dropdown-menu"></ul>',
                filter: '<div class="input-group"><span class="input-group-addon"><i class="glyphicon glyphicon-search"></i></span><input class="form-control multiselect-search" type="text"></div>',
                li: '<li><a href="javascript:void(0);"><label></label></a></li>',
                liGroup: '<li><label class="multiselect-group"></label></li>'
            },

            constructor: Multiselect,

            buildContainer: function () {
                this.$container = $(this.options.buttonContainer);
            },

            buildButton: function () {
                // Build button.
                this.$button = $(this.templates.button).addClass(this.options.buttonClass);

                // Adopt active state.
                if (this.$select.prop('disabled')) {
                    this.disable();
                }
                else {
                    this.enable();
                }

                // Manually add button width if set.
                if (this.options.buttonWidth) {
                    this.$button.css({
                        'width': this.options.buttonWidth
                    });
                }

                // Keep the tab index from the select.
                var tabindex = this.$select.attr('tabindex');
                if (tabindex) {
                    this.$button.attr('tabindex', tabindex);
                }

                this.$container.prepend(this.$button)
            },

            // Build dropdown container ul.
            buildDropdown: function () {

                // Build ul.
                this.$ul = $(this.templates.ul);

                if (this.options.dropRight) {
                    this.$ul.addClass('pull-right');
                }

                // Set max height of dropdown menu to activate auto scrollbar.
                if (this.options.maxHeight) {
                    // TODO: Add a class for this option to move the css declarations.
                    this.$ul.css({
                        'max-height': this.options.maxHeight + 'px',
                        'overflow-y': 'auto',
                        'overflow-x': 'hidden'
                    });
                }

                this.$container.append(this.$ul)
            },

            // Build the dropdown and bind event handling.
            buildDropdownOptions: function () {

                this.$select.children().each($.proxy(function (index, element) {
                    // Support optgroups and options without a group simultaneously.
                    var tag = $(element).prop('tagName').toLowerCase();
                    if (tag == 'optgroup') {
                        this.createOptgroup(element);
                    }
                    else if (tag == 'option') {
                        this.createOptionValue(element);
                    }
                    // Other illegal tags will be ignored.
                }, this));

                // Bind the change event on the dropdown elements.
                $('li input', this.$ul).on('change', $.proxy(function (event) {
                    var checked = $(event.target).prop('checked') || false;
                    var isSelectAllOption = $(event.target).val() == this.options.selectAllValue;

                    // Apply or unapply the configured selected class.
                    if (this.options.selectedClass) {
                        if (checked) {
                            $(event.target).parents('li').addClass(this.options.selectedClass);
                        }
                        else {
                            $(event.target).parents('li').removeClass(this.options.selectedClass);
                        }
                    }

                    // Get the corresponding option.
                    var value = $(event.target).val();
                    var $option = this.getOptionByValue(value);

                    var $optionsNotThis = $('option', this.$select).not($option);
                    var $checkboxesNotThis = $('input', this.$container).not($(event.target));

                    // Toggle all options if the select all option was changed.
                    if (isSelectAllOption) {
                        $checkboxesNotThis.filter(function () {
                            return $(this).is(':checked') != checked;
                        }).trigger('click');
                    }

                    if (checked) {
                        $option.prop('selected', true);

                        if (this.options.multiple) {
                            // Simply select additional option.
                            $option.prop('selected', true);
                        }
                        else {
                            // Unselect all other options and corresponding checkboxes.
                            if (this.options.selectedClass) {
                                $($checkboxesNotThis).parents('li').removeClass(this.options.selectedClass);
                            }

                            $($checkboxesNotThis).prop('checked', false);
                            $optionsNotThis.prop('selected', false);

                            // It's a single selection, so close.
                            this.$button.click();
                        }

                        if (this.options.selectedClass == "active") {
                            $optionsNotThis.parents("a").css("outline", "");
                        }
                    }
                    else {
                        // Unselect option.
                        $option.prop('selected', false);
                    }

                    this.updateButtonText();
                    this.$select.change();
                    this.options.onChange($option, checked);

                    if (this.options.preventInputChangeEvent) {
                        return false;
                    }
                }, this));

                $('li a', this.$ul).on('touchstart click', function (event) {
                    event.stopPropagation();
                    $(event.target).blur();
                });

                // Keyboard support.
                this.$container.on('keydown', $.proxy(function (event) {
                    if ($('input[type="text"]', this.$container).is(':focus')) {
                        return;
                    }
                    if ((event.keyCode == 9 || event.keyCode == 27) && this.$container.hasClass('open')) {
                        // Close on tab or escape.
                        this.$button.click();
                    }
                    else {
                        var $items = $(this.$container).find("li:not(.divider):visible a");

                        if (!$items.length) {
                            return;
                        }

                        var index = $items.index($items.filter(':focus'));

                        // Navigation up.
                        if (event.keyCode == 38 && index > 0) {
                            index--;
                        }
                        // Navigate down.
                        else if (event.keyCode == 40 && index < $items.length - 1) {
                            index++;
                        }
                        else if (!~index) {
                            index = 0;
                        }

                        var $current = $items.eq(index);
                        $current.focus();

                        if (event.keyCode == 32 || event.keyCode == 13) {
                            var $checkbox = $current.find('input');

                            $checkbox.prop("checked", !$checkbox.prop("checked"));
                            $checkbox.change();
                        }

                        event.stopPropagation();
                        event.preventDefault();
                    }
                }, this));
            },

            // Will build an dropdown element for the given option.
            createOptionValue: function (element) {
                if ($(element).is(':selected')) {
                    $(element).prop('selected', true);
                }

                // Support the label attribute on options.
                var label = $(element).attr('label') || $(element).html();
                var value = $(element).val();

                //Hack by Victor Valencia R.            
                if ($(element).parent().hasClass('multiselect-icon') || $(element).parent().parent().hasClass('multiselect-icon')) {
                    var icon = $(element).data('icon');
                    label = '<span class="glyphicon ' + icon + '"></span> ' + label;
                }

                var inputType = this.options.multiple ? "checkbox" : "radio";

                var $li = $(this.templates.li);
                $('label', $li).addClass(inputType);
                $('label', $li).append('<input type="' + inputType + '" />');

                //Hack by Victor Valencia R.
                if (($(element).parent().hasClass('multiselect-icon') || $(element).parent().parent().hasClass('multiselect-icon')) && inputType == 'radio') {
                    $('label', $li).css('padding-left', '0px');
                    $('label', $li).find('input').css('display', 'none');
                }

                var selected = $(element).prop('selected') || false;
                var $checkbox = $('input', $li);
                $checkbox.val(value);

                if (value == this.options.selectAllValue) {
                    $checkbox.parent().parent().addClass('multiselect-all');
                }

                $('label', $li).append(" " + label);

                this.$ul.append($li);

                if ($(element).is(':disabled')) {
                    $checkbox.attr('disabled', 'disabled').prop('disabled', true).parents('li').addClass('disabled');
                }

                $checkbox.prop('checked', selected);

                if (selected && this.options.selectedClass) {
                    $checkbox.parents('li').addClass(this.options.selectedClass);
                }
            },

            // Create optgroup.
            createOptgroup: function (group) {
                var groupName = $(group).prop('label');

                // Add a header for the group.
                var $li = $(this.templates.liGroup);
                $('label', $li).text(groupName);

                //Hack by Victor Valencia R.
                $li.addClass('text-primary');

                this.$ul.append($li);

                // Add the options of the group.
                $('option', group).each($.proxy(function (index, element) {
                    this.createOptionValue(element);
                }, this));
            },

            // Add the select all option to the select.
            buildSelectAll: function () {
                var alreadyHasSelectAll = this.$select[0][0] ? this.$select[0][0].value == this.options.selectAllValue : false;
                // If options.includeSelectAllOption === true, add the include all checkbox.
                if (this.options.includeSelectAllOption && this.options.multiple && !alreadyHasSelectAll) {
                    this.$select.prepend('<option value="' + this.options.selectAllValue + '">' + this.options.selectAllText + '</option>');
                }
            },

            // Build and bind filter.
            buildFilter: function () {

                // Build filter if filtering OR case insensitive filtering is enabled and the number of options exceeds (or equals) enableFilterLength.
                if (this.options.enableFiltering || this.options.enableCaseInsensitiveFiltering) {
                    var enableFilterLength = Math.max(this.options.enableFiltering, this.options.enableCaseInsensitiveFiltering);
                    if (this.$select.find('option').length >= enableFilterLength) {

                        this.$filter = $(this.templates.filter);
                        $('input', this.$filter).attr('placeholder', this.options.filterPlaceholder);
                        this.$ul.prepend(this.$filter);

                        this.$filter.val(this.query).on('click', function (event) {
                            event.stopPropagation();
                        }).on('keydown', $.proxy(function (event) {
                            // This is useful to catch "keydown" events after the browser has updated the control.
                            clearTimeout(this.searchTimeout);

                            this.searchTimeout = this.asyncFunction($.proxy(function () {

                                if (this.query != event.target.value) {
                                    this.query = event.target.value;

                                    $.each($('li', this.$ul), $.proxy(function (index, element) {
                                        var value = $('input', element).val();
                                        if (value != this.options.selectAllValue) {
                                            var text = $('label', element).text();
                                            var value = $('input', element).val();
                                            if (value && text && value != this.options.selectAllValue) {
                                                // by default lets assume that element is not
                                                // interesting for this search
                                                var showElement = false;

                                                var filterCandidate = '';
                                                if ((this.options.filterBehavior == 'text' || this.options.filterBehavior == 'both')) {
                                                    filterCandidate = text;
                                                }
                                                if ((this.options.filterBehavior == 'value' || this.options.filterBehavior == 'both')) {
                                                    filterCandidate = value;
                                                }

                                                if (this.options.enableCaseInsensitiveFiltering && filterCandidate.toLowerCase().indexOf(this.query.toLowerCase()) > -1) {
                                                    showElement = true;
                                                }
                                                else if (filterCandidate.indexOf(this.query) > -1) {
                                                    showElement = true;
                                                }

                                                if (showElement) {
                                                    $(element).show();
                                                }
                                                else {
                                                    $(element).hide();
                                                }
                                            }
                                        }
                                    }, this));
                                }
                            }, this), 300, this);
                        }, this));
                    }
                }
            },

            // Destroy - unbind - the plugin.
            destroy: function () {
                this.$container.remove();
                this.$select.show();
            },

            // Refreshs the checked options based on the current state of the select.
            refresh: function () {
                $('option', this.$select).each($.proxy(function (index, element) {
                    var $input = $('li input', this.$ul).filter(function () {
                        return $(this).val() == $(element).val();
                    });

                    if ($(element).is(':selected')) {
                        $input.prop('checked', true);

                        if (this.options.selectedClass) {
                            $input.parents('li').addClass(this.options.selectedClass);
                        }
                    }
                    else {
                        $input.prop('checked', false);

                        if (this.options.selectedClass) {
                            $input.parents('li').removeClass(this.options.selectedClass);
                        }
                    }

                    if ($(element).is(":disabled")) {
                        $input.attr('disabled', 'disabled').prop('disabled', true).parents('li').addClass('disabled');
                    }
                    else {
                        $input.prop('disabled', false).parents('li').removeClass('disabled');
                    }
                }, this));

                this.updateButtonText();
            },

            // Select an option by its value or multiple options using an array of values.
            select: function (selectValues) {
                if (selectValues && !$.isArray(selectValues)) {
                    selectValues = [selectValues];
                }

                for (var i = 0; i < selectValues.length; i++) {

                    var value = selectValues[i];

                    var $option = this.getOptionByValue(value);
                    var $checkbox = this.getInputByValue(value);

                    if (this.options.selectedClass) {
                        $checkbox.parents('li').addClass(this.options.selectedClass);
                    }

                    $checkbox.prop('checked', true);
                    $option.prop('selected', true);
                    this.options.onChange($option, true);
                }

                this.updateButtonText();
            },

            // Deselect an option by its value or using an array of values.
            deselect: function (deselectValues) {
                if (deselectValues && !$.isArray(deselectValues)) {
                    deselectValues = [deselectValues];
                }

                for (var i = 0; i < deselectValues.length; i++) {

                    var value = deselectValues[i];

                    var $option = this.getOptionByValue(value);
                    var $checkbox = this.getInputByValue(value);

                    if (this.options.selectedClass) {
                        $checkbox.parents('li').removeClass(this.options.selectedClass);
                    }

                    $checkbox.prop('checked', false);
                    $option.prop('selected', false);
                    this.options.onChange($option, false);
                }

                this.updateButtonText();
            },

            // Rebuild the whole dropdown menu.
            rebuild: function () {
                this.$ul.html('');

                // Remove select all option in select.
                $('option[value="' + this.options.selectAllValue + '"]', this.$select).remove();

                // Important to distinguish between radios and checkboxes.
                this.options.multiple = this.$select.attr('multiple') == "multiple";

                this.buildSelectAll();
                this.buildDropdownOptions();
                this.updateButtonText();
                this.buildFilter();
            },

            // Build select using the given data as options.
            dataprovider: function (dataprovider) {
                var optionDOM = "";
                dataprovider.forEach(function (option) {
                    optionDOM += '<option value="' + option.value + '">' + option.label + '</option>';
                });

                this.$select.html(optionDOM);
                this.rebuild();
            },

            // Enable button.
            enable: function () {
                this.$select.prop('disabled', false);
                this.$button.prop('disabled', false)
                    .removeClass('disabled');
            },

            // Disable button.
            disable: function () {
                this.$select.prop('disabled', true);
                this.$button.prop('disabled', true)
                    .addClass('disabled');
            },

            // Set options.
            setOptions: function (options) {
                this.options = this.mergeOptions(options);
            },

            // Get options by merging defaults and given options.
            mergeOptions: function (options) {
                return $.extend({}, this.defaults, options);
            },

            // Update button text and button title.
            updateButtonText: function () {
                var options = this.getSelected();

                // First update the displayed button text.
                $('button', this.$container).html(this.options.buttonText(options, this.$select));

                // Now update the title attribute of the button.
                $('button', this.$container).attr('title', this.options.buttonTitle(options, this.$select));

            },

            // Get all selected options.
            getSelected: function () {
                return $('option[value!="' + this.options.selectAllValue + '"]:selected', this.$select).filter(function () {
                    return $(this).prop('selected');
                });
            },

            // Get the corresponding option by ts value.
            getOptionByValue: function (value) {
                return $('option', this.$select).filter(function () {
                    return $(this).val() == value;
                });
            },

            // Get an input in the dropdown by its value.
            getInputByValue: function (value) {
                return $('li input', this.$ul).filter(function () {
                    return $(this).val() == value;
                });
            },

            updateOriginalOptions: function () {
                this.originalOptions = this.$select.clone()[0].options;
            },

            asyncFunction: function (callback, timeout, self) {
                var args = Array.prototype.slice.call(arguments, 3);
                return setTimeout(function () {
                    callback.apply(self || window, args);
                }, timeout);
            }
        };

        $.fn.multiselect = function (option, parameter) {
            return this.each(function () {
                var data = $(this).data('multiselect'), options = typeof option == 'object' && option;

                // Initialize the multiselect.
                if (!data) {
                    $(this).data('multiselect', (data = new Multiselect(this, options)));
                }

                // Call multiselect method.
                if (typeof option == 'string') {
                    data[option](parameter);
                }
            });
        };

        $.fn.multiselect.Constructor = Multiselect;

        // Automatically init selects by their data-role.
        $(function () {
            $("select[role='multiselect']").multiselect();
        });

    }(window.jQuery);
}

var Data = {};

$(document).ready(function () {

    Data.WorkingHours = $("#WorkingHour");
    Data.WorkingHours.WH = Data.WorkingHours.find(".WorkingHour").clone();
    Data.WorkingHours.find(".WorkingHour").remove();

    Data.PictureLoad = $("#PictureLoad");
    Data.PictureLoad.change(PictureLoad);

    $("#AddWorkingHour").click(WorkingHoursAdd);
    WorkingHoursAdd();

    $("#openMapForInput").click(ShowMap);

    WorkingHoursInit();

    Data.SearchAddress = $("#SearchAddress");
})

function ShowMap() {
    var map = $(this).closest(".form-group").find(".map");
    if (map.is(":hidden"))
        map.addClass("active");
    else
        map.removeClass("active");
}

function WorkingHoursInit() {
    init();

    Data.WorkingHours.find(".remove").click(function () { $(this).closest(".WorkingHour").remove(); });
}

function WorkingHoursAdd() {
    var clone = Data.WorkingHours.WH.clone();

    clone.find(".TimeBlockBTN")
        .click(TimeBlockClick)
        .blur(TimeBlockBlur)
        .focus(TimeBlockFocus);

    clone.find(".TimeBlockBTN input")
        .blur(TimeBlockBlurInput);

    Data.WorkingHours.append(clone);
    WorkingHoursInit();
}

function TimeBlockBlurInput() {
    var $this = $(this);
    setTimeout(function () {
        $this.closest(".TimeBlockBTN").blur();
    }, 1);
}

function TimeBlockBlur() {
    var $this = $(this),
        val = parseInt($this.val());

    setTimeout(function () {
        var isFocus = $this.find(".TimeBlock *:focus").length > 0;

        if (!isFocus && val > 2) $this.val(1).find(".TimeBlock").hide();
        else if (!isFocus) $this.val(3).focus();
    }, 1);
}

function TimeBlockFocus() {
    $(this).find(".TimeBlock").show();
}

function TimeBlockClick() {
    var $this = $(this),
        val = parseInt($this.val());
    if ($this.find(".TimeBlock").is(":visible"))
        $this.val(val + 1).blur();
    else
        $this.focus();
}

//-------------------------------------------------------------------------------------------
ymaps.ready(initMap);
var SearchMap = null,
    SuggestView = null,
    Marker = null,
    Search = null,
    SearchAddress = null,
    SearchCoordinates = null,
    HelpMessage = null,
    FlagFocus = false,
    TextWrite = false;

function initMap() {
    SearchAddress = $("#SearchAddress");

    SearchAddress.focus(function () {
        FlagFocus = true;
    }).blur(function () {
        if (FlagFocus) {
            TextWrite = true;
            Search = SearchAddress.val();
            updateAddress();
        }
    });

    SearchCoordinates = $("#SearchCoordinates");
    HelpMessage = SearchAddress.closest(".InputHelp").find(".HelpMessage");

    SuggestView = new ymaps.SuggestView('SearchAddress');

    SuggestView.events.add(["select"], function (e) {
        Search = SearchAddress.val();
        updateAddress();
    });

    Marker = new ymaps
        .Placemark([55.753564, 37.621085], { balloonContent: null },
        {
            iconLayout: 'default#image',
            preset: 'islands#greenDotIconWithCaption',
            iconImageClipRect: [[0, 0], [64, 64]],
            iconImageHref: 'http://гмпотолки.рф/images/point.png',
            iconImageSize: [32, 32],
            iconImageOffset: [-16, -32],
            draggable: true
        });

    Marker.events
        .add("dragend", function (event) {
            Search = self.Marker.geometry.getCoordinates();
            updateAddress();
        });

    SearchMap = new ymaps.Map('AddressMap', {
        center: [55.753564, 37.621085],
        zoom: 16,
        controls: []
    }, { searchControlProvider: 'yandex#search' });

    SearchMap.events.add('click', function (e) {
        Search = e.get('coords');
        updateAddress();
    });

    SearchMap.geoObjects.add(Marker);
}

function updateAddress() {
    ymaps.geocode(Search).then(function (res) {
        var obj = res.geoObjects.get(0),
            hint;

        if (obj) {
            switch (obj.properties.get('metaDataProperty.GeocoderMetaData.precision')) {
                case 'exact':
                    break;
                case 'number':
                case 'near':
                case 'range':
                    hint = 'Уточните номер дома';
                    break;
                case 'street':
                    hint = 'Уточните номер дома';
                    break;
                case 'other':
                default:
                    hint = 'Уточните адрес';
            }
        } else {
            hint = 'Уточните адрес';
        }
        
        if (hint) {
            HelpMessage.text(hint);
            HelpMessage.addClass("Show");
        } else {
            HelpMessage.removeClass("Show");
        }

        if (Array.isArray(Search) || TextWrite)
            SearchAddress.val(obj.getAddressLine());

        var Position = obj.geometry.getCoordinates();
        SearchCoordinates.val(JSON.stringify(Position));
        GoCenter(Position);
        TextWrite = FlagFocus = false;
        SearchAddress.blur();
        }, function (err) {
            console.log(err);
        });
}

function GoCenter(Position) {
    Marker.geometry.setCoordinates(Position);
    SearchMap.geoObjects.add(Marker);
    SearchMap.setCenter(Position, SearchMap.getZoom(), { duration: 300 });
}

/*
var iMap = null;
var isInitMap = false;

function GoAddress(NewAddress) {
    if (iMap === null) setTimeout(function () { GoAddress(NewAddress); }, 500);
    else iMap.GoAddress(NewAddress);
}

function GoAddressP(latitude, longitude) {
    if (iMap === null) setTimeout(function () { GoAddressP(latitude, longitude); }, 500);
    else iMap.GoAddressPosition(latitude, longitude);
}

function GetAddress() {
    if (iMap === null) setTimeout(function () { GetAddress(); }, 500);
    else iMap.getAddress(null);
}

var isReload = true
ymaps.ready(initM);
function initM() {
    iMap = new initMap();
    isInitMap = true;
    console.log(iMap);
}

function initMap() {
    this.position = null;
    this.map = null;

    var self = this;

    this.Marker = new ymaps
        .Placemark([0, 0], { balloonContent: null },
        {
            iconLayout: 'default#image',
            preset: 'islands#greenDotIconWithCaption',
            iconImageClipRect: [[0, 0], [64, 64]],
            iconImageHref: 'http://calc.gm-vrn.ru/images/point.png',
            iconImageSize: [32, 32],
            iconImageOffset: [-16, -32],
            draggable: true
        });

    self.Marker.events
        .add("dragend", function (event) {
            self.getAddress(self.Marker.geometry.getCoordinates());
        });

    self.map = new ymaps.Map('AddressMap', {
        center: [55.753564, 37.621085],
        zoom: 16,
        controls: []
    }, { searchControlProvider: 'yandex#search' });

    self.map.events.add('click', function (e) {
        self.position = e.get('coords');
        self.getAddress();
        self.GoCenter();
    });

    this.GoAddress = function (NewAddress) {
        ymaps.geocode(NewAddress, { results: 1 })
            .then(function (res) {
                var GeoObject = res.geoObjects.get(0);
                self.position = GeoObject.geometry.getCoordinates();
                self.GoCenter();
            });
    };

    this.GoAddressPosition = function (latitude, longitude) {
        self.position = [latitude, longitude];
        self.getAddress();
        self.GoCenter();
    };

    this.GoCenter = function () {
        if (self.position === null) setTimeout(function () { self.GoCenter(); }, 500);
        else {
            self.Marker.geometry.setCoordinates(self.position);
            self.map.geoObjects.add(self.Marker);
            self.map.setCenter(self.position, self.map.getZoom(), { duration: 300 });
            isReload = false
        }
    };

    this.getAddress = function (CoordinatesPosition = null) {
        if (CoordinatesPosition === null)
            CoordinatesPosition = self.position;

        ymaps.geocode(CoordinatesPosition).then(function (res) {
            var firstMarker = res.geoObjects.get(0);
            Data.SearchAddress.val(firstMarker.getAddressLine());
        });
    };
}
*/
//------------------------------------------------------------------------------------------------

function PictureLoad() {
    var picture = $('<img class="Picture"/>'),
        block = $(this).siblings(".Pictures");

    for (var i = 0; i < this.files.length; ++i) {
        var READER = new FileReader();

        READER.addEventListener("load", function (e) {
            var pic = picture.clone();
            pic.attr("src", e.target.result);
            block.append(pic);
        }, false);

        READER.readAsDataURL(this.files[i]);
    }
}

document.querySelector("input").addEventListener("change", function () {
    if (this.files[0]) {
        var fr = new FileReader();

        fr.addEventListener("load", function () {
            document.querySelector("label").style.backgroundImage = "url(" + fr.result + ")";
        }, false);

        fr.readAsDataURL(this.files[0]);
    }
});