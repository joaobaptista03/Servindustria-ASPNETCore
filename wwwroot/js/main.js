/* MAIN FADE-IN */
document.addEventListener('DOMContentLoaded', function() {
    setTimeout(function() {
        var mainElement = document.querySelector('main');
        if (mainElement) mainElement.style.opacity = 1;
    }, 10);
});

/* MOVABLE ICON */
document.getElementById('movablePhone').onclick = function() {
    var popupBox = document.getElementById('popupBox');
    if (popupBox.classList.contains('popup-visible')) {
        popupBox.classList.remove('popup-visible');
    } else {
        popupBox.classList.add('popup-visible');
    }
};

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
                }, 2000);
            }
        },
        error: function() {
            $('#callRequest-success').hide();
            $('#callRequest-error').html('An error occurred.').show();
        }
    });
});