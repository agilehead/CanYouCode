function validateForm(formId) {
    var validator = new AgileFx.Validator();
    return validator.validate(formId);
}

function clearMessages() {
    var mbox = new AgileFx.MessageBox();
    mbox.clearErrors();
    mbox.clearMessages();
}

function onAjaxComplete(content) {
    var result = content;
    var mbox = new AgileFx.MessageBox();
    mbox.clearErrors();
    mbox.clearMessages();
    if (result.Success) {
        mbox.addMessage(result.Message);
    }
    else {
        mbox.addError(result.Message);
    }
}

function showMessage(message) {
    var mbox = new AgileFx.MessageBox();
    mbox.addMessage(message);
}

function showError(message) {
    var mbox = new AgileFx.MessageBox();
    mbox.addError(message);
}

function validateUploadedFile(container) {
    if ($(container).val().length > 0) {
        var ext = $(container).val().split('.').pop().toLowerCase();
        var allow = new Array('pdf');
        if (jQuery.inArray(ext, allow) == -1) {
            var mbox = new AgileFx.MessageBox();
            mbox.addError('Invalid File, A PDF file is required.');
            return false;
        } else {
            return true;
        }
    } else {
        return true;
    }
}

function flipPanel(from, to) {
    $(from).hide();
    $(to).show();
}

var Canyoucode = Class.create(true);
Canyoucode.namespace({
    Project: Class.create({
        init: function () { },

        getDisplayTitle: function (title) {
            if (title.length > 50) return title.substring(0, 50) + "...";
            else return title;
        }
    })
});

/*--- qtip init ----*/
var TipCorners = {
    BL: 'bottomLeft', BR: 'bottomRight', BM: 'bottomMiddle',
    TR: 'topRight', TL: 'topLeft', TM: 'topMiddle',
    LM: 'leftMiddle', LT: 'leftTop', LB: 'leftBottom',
    RM: 'rightMiddle', RB: 'rightBottom', TR: 'rightTop'
};
