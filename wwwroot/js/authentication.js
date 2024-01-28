$('#login-form').submit(function(event) {
    event.preventDefault();

    $.ajax({
        type: this.method,
        url: this.action,
        data: $(this).serialize(),
        success: function(response) {
            if (!response.success) {
                $('#login-success').hide();
                $('#login-error').html(response.error).show();
            } else {
                $('#login-error').hide();
                $('#login-success').html('Login feito com sucesso! A redirecionar...').show();
                setTimeout(function() {
                    window.location.reload();
                }, 1000);
            }
        },
        error: function() {
            $('#login-success').hide();
            $('#login-error').html('An error occurred.').show();
        }
    });
});

$('#register-form').submit(function(event) {
    event.preventDefault();

    $.ajax({
        type: this.method,
        url: this.action,
        data: $(this).serialize(),
        success: function(response) {
            if (!response.success) {
                $('#register-success').hide();
                $('#register-error').html(response.errors.join('<br>')).show();

            } else {
                $('#register-error').hide();
                $('#register-success').html('Registrado com sucesso! A redirecionar').show();
                setTimeout(function() {
                    window.location.reload();
                }, 1000);
            }
        },
        error: function() {
            $('#register-success').hide();
            $('#register-error').html('An error occurred.').show();
        }
    });
});