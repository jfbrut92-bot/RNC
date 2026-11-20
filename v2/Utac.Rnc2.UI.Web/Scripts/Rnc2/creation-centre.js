$(document).ready(function ($) {
    //$('#centre_CpCentre').autocomplete({ source: '/Communes/Liste', minLength: 2 });
    var cache = {};
    $(function () {
        $("#Centre_CpCentre, #Centre_Ville").autocomplete({
            source: function (request, response) {
                //Si la réponse est dans le cache
                if (('FR' + '-' + request.term) in cache) {
                    response($.map(cache['FR' + '-' + request.term], function (item) {

                        return {
                            label: item.CodePostal + ", " + item.Ville,
                            value: function () {
                                if ($(this).attr('id') == 'Centre_CpCentre') {
                                    $('#Centre_Ville').val(item.Ville);
                                    return item.CodePostal;
                                }
                                else {
                                    $('#Centre_CpCentre').val(item.CodePostal);
                                    return item.Ville;
                                }
                            }
                        }
                    }));
                }
                //Sinon -> Requete Ajax
                else {
                    var objData = {};
                    if ($(this.element).attr('id') == 'Centre_CpCentre') {
                        objData = { codePostal: request.term, pays: 'FR', maxRows: 10 };
                    }
                    else {
                        objData = { ville: request.term, pays: 'FR', maxRows: 10 };
                    }

                    $.ajax({
                        url: "/Communes/Liste",
                        dataType: "json",
                        data: objData,
                        type: 'POST',
                        success: function (data) {
                            //Ajout de reponse dans le cache

                            cache[('FR' + '-' + request.term)] = data;
                            response($.map(data, function (item) {

                                return {
                                    label: item.CodePostal + ", " + item.Ville,
                                    value: function () {
                                        if ($(this).attr('id') == 'Centre_CpCentre') {
                                            $('#Centre_Ville').val(item.Ville);
                                            return item.CodePostal;
                                        }
                                        else {
                                            $('#Centre_CpCentre').val(item.CodePostal);
                                            return item.Ville;
                                        }
                                    }
                                }
                            }));
                        }
                    });
                    /*
                    $.getJSON('/Communes/Liste', { 'codePostal': request.term }, function (data) {
                        var items = [];
                        cache[('FR' + '-' + request.term)] = data;
                        $.each(data, function (item) {
                            alert(item.CodePostal);
                        });
                    });*/
                }
            },
            minLength: 3,
            delay: 600
        });
    });
});
