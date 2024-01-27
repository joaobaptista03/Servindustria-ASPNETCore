// Transition for main
document.addEventListener("DOMContentLoaded", function() {
    setTimeout(function() {
        var mainElement = document.querySelector('main');
        if (mainElement) mainElement.style.opacity = 1;
    }, 10);
});