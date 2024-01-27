// Transition for main
document.addEventListener("DOMContentLoaded", function() {
    setTimeout(function() {
        var mainElement = document.querySelector('main');
        if (mainElement) mainElement.style.opacity = 1;
    }, 10);
});

// Logout
document.getElementById('logout-button').addEventListener('click', function(e) {
    e.preventDefault();
    fetch('/LoginRegister?handler=Logout', {
        method: 'POST',
        headers: {
            'RequestVerificationToken': document.querySelector('#logoutForm input[name="__RequestVerificationToken"]').value,
        }
    }).then(response => {
        if (response.ok) {
            window.location.href = '/';
        }
    });
});