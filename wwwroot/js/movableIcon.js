/* MOVABLE ICON */
document.getElementById('movablePhone').onclick = function() {
    var popupBox = document.getElementById('popupBox');
    if (popupBox.classList.contains('popup-visible')) {
        popupBox.classList.remove('popup-visible');
    } else {
        popupBox.classList.add('popup-visible');
    }
};