$(document).ready(function () {

    //On pose un listener sur le changement de profil
    $('#filtre_ProfilId').change(function () {
        changeState();
    });

    //On adapte l'écran selon le profilId
    changeState();
});

function changeState() {

    var choix = $('#filtre_ProfilId option:selected').html();

    //$('.editor-field').hide();
    //$('.editor-label').hide();
    $("#idDivDepartement").hide();
    $("#idDivRegion").hide();
    $("#idDivReseau").hide()
    $("#idDivCatVeh").hide();

    //On réaffiche le profil
    //$('#filtre_ProfilId').parent().show();
    //$('label[for="filtre_ProfilId"]').parent().show();

    if (choix == "OTC") {
        //On affiche uniquement l'input du nom de l'utilisateur et son label
        //$('#filtre_Nom').parent().show();
        //$('label[for="filtre_Nom"]').parent().show();


    } else {

        if (choix == "PREFECTURE") {
            //On affiche le nom et le code département
            //$('#filtre_Nom').parent().show();
            //$('label[for="filtre_Nom"]').parent().show();

            //$('#filtre_CodeDepartement').parent().show();
            //$('label[for="filtre_CodeDepartement"]').parent().show();
            $("#idDivDepartement").show();
        } else {

            if (choix == "DRIRE") {
                //On affiche le nom et le code région
                //$('#filtre_Nom').parent().show();
                //$('label[for="filtre_Nom"]').parent().show();

                $("#idDivRegion").show();
            } else {
                //CENTRE INDEPENDANT
                if (choix == "CENTRE_INDEPENDANT") {
                    //On affiche le nom et le code département et login provisoire et catégorie de véhicule
                    //$('#filtre_Nom').parent().show();
                    //$('label[for="filtre_Nom"]').parent().show();

                    //$('#filtre_CodeDepartement').parent().show();
                    //$('label[for="filtre_CodeDepartement"]').parent().show();

                    //$('#filtre_LoginProvisoire').parent().show();
                    ///$('label[for="filtre_LoginProvisoire"]').parent().show();

                    //$('#filtre_CategorieVehicule').parent().show();
                    //$('label[for="filtre_CategorieVehicule"]').parent().show();
                    $("#idDivDepartement").show();
                    $("#idDivCatVeh").show();


                } else {
                    //RESEAU
                    if (choix == "RESEAU") {
                        //On affiche le nom et le code réseau(VL,PL,CL) et la catégorie de véhicule
                        //$('#filtre_Nom').parent().show();
                        //$('label[for="filtre_Nom"]').parent().show();

                        //$('#filtre_CodeReseau').parent().show();
                        //$('label[for="filtre_CodeReseau"]').parent().show();

                        //$('#filtre_CategorieVehicule').parent().show();
                        //$('label[for="filtre_CategorieVehicule"]').parent().show();

                        $("#idDivReseau").show()


                    } else {
                        //DSCR
                        if (choix == "DSCR") {
                            //On affiche le nom 
                            //$('#filtre_Nom').parent().show();
                            //$('label[for="filtre_Nom"]').parent().show();

                        }
                    }
                }
            }
        }
    }

}
