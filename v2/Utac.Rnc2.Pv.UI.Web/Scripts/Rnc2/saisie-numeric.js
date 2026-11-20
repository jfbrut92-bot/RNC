//N'autoriser que la saisie de num�riques
//
//appel: <script src="/Scripts/rnc2/saisie.numeric.js" type="text/javascript"></script>

function OnlyNumeric(e) {
    //PLE 26/06/2024 Correctif e.KeyCode to e.Key
    //var keyCode = ('which' in e) ? e.which : e.keyCode;
    var keyCode = ('which' in e) ? e.which : e.key;
    isNumeric = (keyCode >= 48 && keyCode <= 57 && e.shiftKey) ||
        (keyCode >= 35 && keyCode <= 40) ||
        (keyCode >= 96 && keyCode <= 105) || keyCode == 8 || keyCode == 46 || keyCode == 9;
    //    alert("isnumeric " + isNumeric + " keycode " + keyCode + " e.shiftKey " + e.shiftKey + " e.which " + e.which + " e.keyCode " + e.keyCode);
    return isNumeric;

    //saisie alpha uniquement
    //        function FilterInput(event) {
    //            var keyCode = ('which' in event) ? event.which : event.keyCode;

    //            isNumeric = (keyCode >= 48 /* KeyboardEvent.DOM_VK_0 */ && keyCode <= 57 /* KeyboardEvent.DOM_VK_9 */) ||
    //                        (keyCode >= 96 /* KeyboardEvent.DOM_VK_NUMPAD0 */ && keyCode <= 105 /* KeyboardEvent.DOM_VK_NUMPAD9 */);
    //            modifiers = (event.altKey || event.ctrlKey || event.shiftKey);
    //            return !isNumeric || modifiers;
    //        }
}
