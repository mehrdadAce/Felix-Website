// Source: http://bootstrap-notify.remabledesigns.com/
function notifyDanger(text) {
    $.notify({
        // options
        message: text
    }, {
        // settings
        type: 'danger'
    });
}

function notifyWarning(text) {
    $.notify({
        // options
        message: text
    }, {
        // settings
        type: 'warning'
    });
}


function notifySuccess(text) {
    $.notify({
        // options
        message: text
    }, {
        // settings
        type: 'success'
    });
}
