document.addEventListener('DOMContentLoaded', function() {
    var token = document.querySelector('input[name="__RequestVerificationToken"]').value;

    fetch('/admin/pedidosContacto?handler=NrContactForms', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': token
        },
        body: JSON.stringify({})
    })
    .then(response => response.json())
    .then(data => {
        if (data.result > 0) {
            document.getElementById('notificationContactForms').style.visibility = 'visible';
        }
    })
    .catch(error => {
        console.error('Error:', error);
    });

    fetch('/admin/pedidosChamada?handler=NrCallRequests', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': token
        },
        body: JSON.stringify({})
    })
    .then(response => response.json())
    .then(data => {
        if (data.result > 0) {
            document.getElementById('notificationCallRequests').style.visibility = 'visible';
        }
    })
    .catch(error => {
        console.error('Error:', error);
    });
});