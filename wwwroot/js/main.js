/* MAIN FADE-IN */
document.addEventListener('DOMContentLoaded', function() {
    setTimeout(function() {
        var mainElement = document.querySelector('main');
        if (mainElement) mainElement.style.opacity = 1;
    }, 10);
});

/* MOVABLE ICON */
document.getElementById('movablePhone').onclick = function() {
    document.getElementById('popupBox').style.display = 'block';
};

function closePopup() {
    document.getElementById('popupBox').style.display = 'none';
}

/* CALL REQUEST SUBMIT */
$('#callRequest-form').submit(function(event) {
    event.preventDefault();

    $.ajax({
        type: this.method,
        url: this.action,
        data: $(this).serialize(),
        success: function(response) {
            if (!response.success) {
                $('#callRequest-success').hide();
                $('#callRequest-error').html(response.error).show();
            } else {
                $('#callRequest-error').hide();
                $('#callRequest-success').html('Pedido registado. Iremos contactá-lo logo que possível!').show();
                window.setTimeout(function() {
                    $('#callRequest-success').hide();
                    closePopup();
                }, 1000);
            }
        },
        error: function() {
            $('#callRequest-success').hide();
            $('#callRequest-error').html('An error occurred.').show();
        }
    });
});