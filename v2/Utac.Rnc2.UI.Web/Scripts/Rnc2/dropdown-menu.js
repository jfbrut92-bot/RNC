/*
16/11/2016 PLE Ajout du script d'affichage des sous-menu de la barre de menu principale
*/
/* When the user clicks on the button, 
toggle between hiding and showing the dropdown content */
function rnc2MenuDropDown(object) {
    var element = document.getElementById(object);
    if (rnc2HasClass(element, "show")) {
        element.className = "dropdown-content";
    } else {
        element.className += " show";
    }
}
function rnc2HasClass(element, cls) {
    return (' ' + element.className + ' ').indexOf(' ' + cls + ' ') > -1;
}
