document.getElementById('logout-button').addEventListener('click', function(e) {
    e.preventDefault();
    var form = this.closest('form');
    $.ajax({
        type: form.method,
        url: form.action,
        data: $(form).serialize(),
        success: function(response) {
            if (response.success) {
                window.location.href = '/';
            }
        },
    });
});