document.addEventListener('DOMContentLoaded', function() {
    /* MAIN FADE-IN */
    setTimeout(function() {
        var mainElement = document.querySelector('main');
        if (mainElement) mainElement.style.opacity = 1;
    }, 10);

    /* MAIN FADE-OUT */
    var exitElements = document.querySelectorAll('.exit-trigger');
    exitElements.forEach(function(element) {
        element.addEventListener('click', function(event) {
            event.preventDefault();

            var mainElement = document.querySelector('main');
            if (mainElement) {
                mainElement.style.transition = '0.4s ease;';
                mainElement.style.opacity = 0;
            }

            setTimeout(function() {
                window.location.href = event.target.href;
            }, 400);
        });
    });
});

/* CALL REQUEST SUBMIT */
$('#callRequest-form').off('submit').on('submit', function(event) {
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
                    document.getElementById('popupBox').classList.remove('popup-visible');
                }, 2000);
            }
        },
        error: function() {
            $('#callRequest-success').hide();
            $('#callRequest-error').html('An error occurred.').show();
        }
    });
});