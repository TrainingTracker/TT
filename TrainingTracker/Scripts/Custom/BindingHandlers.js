var windowURL = window.URL || window.webkitURL;

ko.bindingHandlers.datepicker = {
    init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {

        var dateFormat = allBindings().dateFormat;
        var buttonImage = allBindings().buttonImage; // allBindings.get('dateFormat');
        var endDate = allBindings().endDate;
        var startDate = allBindings().startDate;

        if (typeof dateFormat == 'undefined') {
            dateFormat = 'mm/dd/yyyy';
        }

        if (typeof (endDate) === 'undefined') {
            endDate = null;
        }
        if (typeof (startDate) === 'undefined') {
            startDate = null;
        }

        if (typeof buttonImage == 'undefined') {
            buttonImage = "Images/icon_date_picker.png";
        }

        var options = {
            //showOtherMonths: true,
            //selectOtherMonths: true,
            dateFormat: dateFormat,
            autoclose: true,
            //buttonImage: buttonImage,
            //showOn: "both",
            todayHighlight: true,
            endDate: endDate,
            startDate: startDate
        };

        if (typeof valueAccessor() === 'object') {
            $.extend(options, valueAccessor());
        }

        $(element).datepicker(options);
    },

    update: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
        var v = valueAccessor()();
    }

};

ko.bindingHandlers.barClick = {
    init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
        if (!allBindings.has('chartData')) {
            throw Error('chartType must be used in conjunction with chartData and (optionally) chartOptions');
            return;
        }
        var chartType = allBindings.get('chartType');
        if (chartType !== 'Bar') {
            throw Error('barClick can only be used with chartType Bar');
            return;
        }
    },
    update: function (element, valueAccesor, allBindings, viewModel, bindingContext) { }
};
ko.bindingHandlers.lineClick = {
    init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
        if (!allBindings.has('chartData')) {
            throw Error('chartType must be used in conjunction with chartData and (optionally) chartOptions');
            return;
        }
        var chartType = allBindings.get('chartType');
        if (chartType !== 'Line') {
            throw Error('lineClick can only be used with chartType Line');
            return;
        }
    },
    update: function (element, valueAccesor, allBindings, viewModel, bindingContext) { }
};
ko.bindingHandlers.segmentClick = {
    init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
        if (!allBindings.has('chartData')) {
            throw Error('chartType must be used in conjunction with chartData and (optionally) chartOptions');
            return;
        }
        var chartType = allBindings.get('chartType');
        if (chartType !== 'Pie' && chartType !== 'Doughnut') {
            throw Error('segmentClick can only be used with chartType Pie or Donut');
            return;
        }
    },
    update: function (element, valueAccesor, allBindings, viewModel, bindingContext) { }
};
ko.bindingHandlers.chartType = {
    init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
        if (!allBindings.has('chartData')) {
            throw Error('chartType must be used in conjunction with chartData and (optionally) chartOptions');
        }
    },
    update: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
        var ctx = element.getContext('2d'),
            type = ko.unwrap(valueAccessor()),
            data = ko.unwrap(allBindings.get('chartData')),
            options = ko.unwrap(allBindings.get('chartOptions')) || {},
            segmentClick = ko.unwrap(allBindings.get('segmentClick')),
            barClick = ko.unwrap(allBindings.get('barClick')),
            lineClick = ko.unwrap(allBindings.get('lineClick'));

        // NB: Fix for newer knockout (see https://gist.github.com/jmhdez/4987b053e817d65d7c68)
        if (this.chart) {
            this.chart.destroy();
            delete this.chart;
        }
        if (ctx.canvas.chart) {
            ctx.canvas.chart.destroy();
            ctx.clearRect(0, 0, ctx.canvas.width, ctx.canvas.height);
        }

        if ($('#divFeedbackChart').css('display') == 'none') return;

        //this.chart = new Chart(ctx)[type](data, options);
        //*/

        if (data == null) return;

        //ko.utils.domNodeDisposal.addDisposeCallback(element,

        //function ()
        //{
        //    $(element).chart.destroy();
        //    delete $(element).chart;
        //});

        var newChart;

        if (type == 'Line') {
            newChart = new Chart(ctx).Scatter(data, options);
        } else {
            newChart = new Chart(ctx)[type](data, options);
        }
        newChart.clear();
        var $element = $(element)[0];
        $element.chart = newChart;
        //* End of fix

        //* Remove existing click binding
        if ($element.click) {
            $element.removeEventListener('click', $element.click);
            delete ($element.click);
        }
        //* Add segment click binding
        switch (type) {
            case "Pie":
            case "Doughnut":
                if (segmentClick) {
                    $element.click = function (evt) {
                        var activePoints = newChart.getSegmentsAtEvent(evt);
                        segmentClick(activePoints[0], newChart);
                    };
                }
                break;
            case "Bar":
                if (barClick) {
                    $element.click = function (evt) {
                        barClick(newChart.getBarsAtEvent(evt), newChart);
                    };
                }
                break;
            case "Line":
                if (lineClick) {
                    $element.click = function (evt) { lineClick(newChart.getPointsAtEvent(evt), newChart); };
                }
                break;
            default:
                break;
        }
        $element.addEventListener('click', $element.click);
        // if ($('#divFeedbackChart').css('display') == 'inline-block') $('#divFeedbackChart').css('display', 'none');
    }
};

ko.bindingHandlers.fadeVisible = {
    init: function (element, valueAccessor) {
        var shouldDisplay = valueAccessor();
        $(element).toggle(shouldDisplay);
    },
    update: function (element, valueAccessor) {
        // On update, fade in/out
        var shouldDisplay = valueAccessor();

        if (shouldDisplay) {
            $(element).fadeIn(600, "swing");
        }
        else
            $(element).fadeOut(300, "swing");

    }
};

ko.bindingHandlers.chartData = {
    init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
        if (!allBindings.has('chartType')) {
            throw Error('chartData must be used in conjunction with chartType and (optionally) chartOptions');
        }
    }
};

ko.bindingHandlers.chartOptions = {
    init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
        if (!allBindings.has('chartData') || !allBindings.has('chartType')) {
            throw Error('chartOptions must be used in conjunction with chartType and chartData');
        }
    }
};

ko.bindingHandlers.fullWindowHeight =
{
    update: function (element, valueAccessor) {
        // On update, fade in/out
        var shouldDisplay = valueAccessor();

        if (shouldDisplay) {
            $(document).on("custom-resize", function () {
                $('html,body').css("height", $(document).height());
                $(element).css({ 'min-height': $(document).height(), 'height': $(document).height() });
            });
            $("html, body").animate({ scrollTop: 0 }, "slow");
            return;
        }
        $(element).css({ 'min-height': 0, 'height': 0 });
        $('html,body').css("height", "auto");
        $(document).off('custom-resize');
        return;

    }
};

/***** End of ko-chart.js *****/


/*Added for file upload*/

$(document).ready(function () {
    ko.bindingHandlers.fileSrc = {

        update: function (element, valueAccessor) {
            ko.utils.registerEventHandler(element, "change", function () {
                var reader = new FileReader();

                reader.onloadstart = function () {
                    my.toggleLoader(true);
                };

                reader.onload = function (e) {
                    var value = valueAccessor();
                    value(e.target.result);
                };

                reader.onloadend = function () {
                    my.toggleLoader(false);
                };

                reader.readAsText(element.files[0]);
            });
        }
    };

    //Added for enable and disable all the child elements
    ko.bindingHandlers.enableChildren = {
        init: function (elem, valueAccessor) {
            var enabled = ko.utils.unwrapObservable(valueAccessor());
            ko.utils.arrayForEach(elem.getElementsByTagName('input'), function (i) {
                i.disabled = !enabled;
            });
            ko.utils.arrayForEach(elem.getElementsByTagName('select'), function (i) {
                i.disabled = !enabled;
            });
            ko.utils.arrayForEach(elem.getElementsByTagName('button'), function (i) {
                i.disabled = !enabled;
            });
            ko.utils.arrayForEach(elem.getElementsByTagName('a'), function (i) {
                i.disabled = !enabled;
            });

        },
        update: function (elem, valueAccessor) {
            var enabled = ko.utils.unwrapObservable(valueAccessor());
            ko.utils.arrayForEach(elem.getElementsByTagName('input'), function (i) {
                i.disabled = !enabled;
            });
            ko.utils.arrayForEach(elem.getElementsByTagName('select'), function (i) {
                i.disabled = !enabled;
            });
            ko.utils.arrayForEach(elem.getElementsByTagName('button'), function (i) {
                i.disabled = !enabled;
            });
            ko.utils.arrayForEach(elem.getElementsByTagName('a'), function (i) {
                i.disabled = !enabled;
            });
        }
    };
    ko.bindingHandlers.wzTooltip = {
        update: function (element, valueAccessor) {
            var options = valueAccessor();
            config.FontSize = '12px';
            if (!my.isNullorEmpty(options.ClickSticky)) {
                config.ClickSticky = JSON.parse(options.ClickSticky);
            };
            if (!my.isNullorEmpty(options.ClickClose)) {
                config.ClickClose = JSON.parse(options.ClickClose);
            };
            if (!my.isNullorEmpty(options.Width)) {
                config.Width = JSON.parse(options.Width);
            };
            if (!my.isNullorEmpty(options.Above)) {
                config.Above = JSON.parse(options.Above);
            };
            if (!my.isNullorEmpty(options.Padding)) {
                config.Padding = JSON.parse(options.Padding);
            };
            ko.utils.registerEventHandler(element, "mouseover", function () {
                if (options.HtmlTag === "true") {
                    TagToTip(options.HtmlId);
                }
                else { Tip(options.tittle, CLICKSTICKY, config.ClickSticky, CLICKCLOSE, config.ClickClose, WIDTH, config.Width, PADDING, config.Padding); }
            });
            ko.utils.registerEventHandler(element, "mouseout", function () {
                UnTip();
            });
        }
    };
    
    ko.bindingHandlers.fileInput = {
        init: function (element, valueAccessor)
        {
            element.onchange = function ()
            {
                var fileData = ko.utils.unwrapObservable(valueAccessor()) || {};
                if (fileData.dataUrl)
                {
                    fileData.dataURL = fileData.dataUrl;
                }
                if (fileData.objectUrl)
                {
                    fileData.objectURL = fileData.objectUrl;
                }
                fileData.file = fileData.file || ko.observable();
                fileData.fileArray = fileData.fileArray || ko.observableArray([]);

                var file = this.files[0];
                fileData.fileArray([]);
                if (file)
                {
                    var fileArray = [];
                    for (var i = 0; i < this.files.length; i++)
                    { // FileList is not an array
                        fileArray.push(this.files[i]);
                    }
                    fileData.fileArray(fileArray); // set it once for subscriptions to work properly
                    fileData.file(file);
                }

                if (!fileData.clear) {
                    fileData.clear = function() {
                        ['objectURL', 'base64String', 'binaryString', 'text', 'dataURL', 'arrayBuffer'].forEach(function(property, i) {
                            if (fileData[property + 'Array'] && ko.isObservable(fileData[property + 'Array'])) {
                                var values = fileData[property + 'Array'];
                                while (values().length) {
                                    var val = values.splice(0, 1);
                                    if (property == 'objectURL') {
                                        windowURL.revokeObjectURL(val);
                                    }
                                }
                            }
                            if (fileData[property] && ko.isObservable(fileData[property])) {
                                fileData[property](null);
                            }
                        });
                        element.value = '';
                        fileData.fileArray([]);
                        fileData.file(null);
                    };
                }
                if (ko.isObservable(valueAccessor()))
                {
                    valueAccessor()(fileData);
                }
            };
            element.onchange();

            ko.utils.domNodeDisposal.addDisposeCallback(element, function ()
            {
                var fileData = ko.utils.unwrapObservable(valueAccessor()) || {};
                fileData.clear = undefined;
            });
        },
        update: function (element, valueAccessor, allBindingsAccessor)
        {

            var fileData = ko.utils.unwrapObservable(valueAccessor());

            function fillData(file, index)
            {

                if (fileData.objectURL && ko.isObservable(fileData.objectURL))
                {
                    var newUrl = file && windowURL.createObjectURL(file);
                    if (newUrl)
                    {
                        var oldUrl = fileData.objectURL();
                        if (oldUrl)
                        {
                            windowURL.revokeObjectURL(oldUrl);
                        }
                        fileData.objectURL(newUrl);
                    }
                }


                if (fileData.base64String && ko.isObservable(fileData.base64String))
                {
                    if (!(fileData.dataURL && ko.isObservable(fileData.dataURL)))
                    {
                        fileData.dataURL = ko.observable(); // adding on demand
                    }
                }
                if (fileData.base64StringArray && ko.isObservable(fileData.base64StringArray))
                {
                    if (!(fileData.dataURLArray && ko.isObservable(fileData.dataURLArray)))
                    {
                        fileData.dataURLArray = ko.observableArray();
                    }
                }

                ['binaryString', 'text', 'dataURL', 'arrayBuffer'].forEach(function (property)
                {
                    var method = 'readAs' + (property.substr(0, 1).toUpperCase() + property.substr(1));
                    if (property != 'dataURL' && !(fileData[property] && ko.isObservable(fileData[property])))
                    {
                        return true;
                    }
                    if (!file)
                    {
                        return true;
                    }
                    var reader = new FileReader();
                    

                    reader.onloadstart = function ()
                    {
                        my.toggleLoader(true);
                    };

                    reader.onloadend = function ()
                    {
                        my.toggleLoader(false);
                    };

                    reader.onload = function (e)
                    {
                        function fillDataToProperty(result, prop)
                        {
                            if (index == 0 && fileData[prop] && ko.isObservable(fileData[prop]))
                            {
                                fileData[prop](result);
                            }
                            if (fileData[prop + 'Array'] && ko.isObservable(fileData[prop + 'Array']))
                            {
                                if (index == 0)
                                {
                                    fileData[prop + 'Array']([]);
                                }
                                fileData[prop + 'Array'].push(result);
                            }
                        }
                        fillDataToProperty(e.target.result, property);
                        if (method == 'readAsDataURL' && (fileData.base64String || fileData.base64StringArray))
                        {
                            var resultParts = e.target.result.split(",");
                            if (resultParts.length === 2)
                            {
                                fillDataToProperty(resultParts[1], 'base64String');
                            }
                        }
                    };
                    reader[method](file);
                });
            }

            fileData.fileArray().forEach(function(file, index) {
                fillData(file, index);
            });
        }
    };
    
    ko.bindingHandlers.autoResizeTextArea = {
        init: function (element)
        {
            if (typeof(autosize) != 'undefined') {
                autosize($(element));
            }
           
        },
    };
    
   
    
    ko.bindingHandlers.wizardControl = {
        init: function (element, valueAccessor, allBindingsAccessor)
        {
            var options = allBindingsAccessor();
            $(element).steps(
            {
                transitionEffect: typeof(options.animation) != 'undefined' ? options.animation : 0,
                stepsOrientation: typeof(options.orientation) != 'undefined' ? options.orientation : 0,
                transitionEffectSpeed: 300,
                titleTemplate: ' <span class="number">#index#</span>',
                enableFinishButton: true,
                headerTag: 'div',
                autoFocus: true,
                labels:
                {
                    finish:'Submit Survey'
                },
                onStepChanging: function (event, currentIndex, newIndex)
                {
                    
                    if ((currentIndex + 1) <= valueAccessor().length) {
                        var currentStep = $($(element).find('.content .body')[currentIndex])[0].innerHTML;
                        var answerObject =
                        {
                            QuestionId: 0,
                            AnswerId: [],
                            AdditionalNotes: ''
                        };

                        var stepAnswer = Object.create(answerObject);

                        stepAnswer.QuestionId = $(currentStep).attr('id');
                        stepAnswer.AnswerId = [];
                        var answerinput = $(currentStep).find('.wizard-answer input');

                        $.each(answerinput, function(key, value) {
                            if ($('#' + $(answerinput)[key].id).is(':checked')) stepAnswer.AnswerId.push(answerinput[key].id.replace("input_", ""));
                        });

                        var additionalNotes = '#' + $(currentStep).find('.wizard-additional-note textarea')[0].id;

                        stepAnswer.AdditionalNotes = $(additionalNotes)[0].value;

                        var errorMsg = options.wizardStepCallback(stepAnswer, currentIndex);
                        $('#' + $(currentStep).find('#divWizardErrorMessage_' + stepAnswer.QuestionId)[0].id).find('label').text(errorMsg);
                        return errorMsg.length == 0;
                    }
                    else
                    {
                        return true;
                    }

                },
                onStepChanged: function (event, currentIndex, priorIndex)
                {
                    options.wizardOnStepChanged(currentIndex);
                    
                    if ((currentIndex + 1) > valueAccessor().length)
                    {
                        options.loadPreviewFunction(function(previewHtml) {
                            var currentStep = $($(element).find('.content .body')[currentIndex])[0].id;
                            $('#' + currentStep).find('#divWizardFeedbackPreview').html(previewHtml);
                            my.toggleLoader(false);
                        });
                                              
                    }
                },
                
                onFinishing: function (event, currentIndex)
                {
                    var currentStep = $($(element).find('.content .body')[currentIndex])[0];
                   
                    var errorMsg =  options.wizardOnSubmit();
                    $('#' + $(currentStep).find('#divWizardErrorMessage_Preview')[0].id).find('label').text(errorMsg);
                    return errorMsg.length == 0;
                }
        });

        },
        
        update: function (element, valueAccessor, allBindingsAccessor) {
            var data = valueAccessor();

            $.each(data, function (key)
            {
                    var title = '<span class="wizard-header">' + data[key].CategoryHeader + '</span><div class="arrow"></div>';
                    var content = '';

                    content += '<div id=' + data[key].QuestionId + '><div class="wizard-question">' + '<span id=""> ' + data[key].QuestionText + '</span><label class="danger" style="font-weight:    font-weight: bold;position: relative;top: -8px;bold;display:' + (data[key].IsMandatory == true ? 'inline-block' : 'none') + ';">' +
                        '*</label>' +
                       (!my.isNullorEmpty(data[key].HelpText) ? '<span class="glyphicon glyphicon-question-sign"  onmouseover="Tip(&quot;' + data[key].HelpText + '&quot;)" onmouseout="UnTip()"></span>' : '') +
                        '</div>';
                    if (data[key].ResponseType == 4) {
                        content += '<div class="wizard-answer">';
                        $.each(data[key].Answer, function(answerKey, value) {
                            content += '<div><span class="custom-checkbox">' +
                                '<input type="radio" id="input_' + data[key].Answer[answerKey].AnswerId + '" name="Question_' + data[key].QuestionId + '"  />' +
                                '<span class="box"><span class="tick"></span></span> ' +
                                '</span>' +
                                '<label for="input_' + data[key].Answer[answerKey].AnswerId + '" class=" lblForCheckbox make-checkbox-label-in-align">' + data[key].Answer[answerKey].AnswerText + '</label></div>';
                        });
                        content += '</div>';
                    }

                    content += '<div class="wizard-additional-note" >' +
                        '<label >Additional Notes (2-3 lines max.)</label><span class="danger" style="display:' + (data[key].AdditionalNoteRequired ? 'inline-block' : 'none') + '">*</span>' +
                        '<textarea id="textareaQuestion_' + data[key].QuestionId + '" class="form-control comment-input-control" rows="5" type="text" placeholder="Enter notes"></textarea></div>';
                    content += '<div id="divWizardErrorMessage_' + data[key].QuestionId + '"><label class="danger"></label></div>';
                    content += '</div>';
                    $(element).steps('add', {
                        title: title,
                        content: content
                    });
            });
            
            if (data.length) {
                $(element).steps('add',
                    {
                        title: '<span class="wizard-header">Preview</span><div class="arrow"></div>',
                        content: '<div id="divWizardFeedbackPreview"></div><div id="divWizardErrorMessage_Preview"><label class="danger"></label></div>'
                   });
            }
            

                autosize.destroy($(element).find('textarea'));
                autosize($(element).find('textarea'));

        }
    };
});


