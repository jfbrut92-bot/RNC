namespace Utac.Rnc2.BusinessObjects
{
    public partial class rTypeDemandeControleur : BusinessBase<string>
    {
        protected override void validate()
        {
            /* Implementer ICI les r?gles de validation */

        }

        public enum TypeModification
        {
            ADMSUPFI,
            AMEND,
            ANUPL,
            ANUVL,
            ANUCL,
            AUDITPL,
            AUDITVL,
            AUDITCL,
            AVOTCPL,
            AVOTCVL,
            CHCEPL,
            CHCEVL,
            CHCECL,
            CRECO,
            CRECOPL,
            CRECOVL,
            CRECOCL,
            CRECOVPL,
            DEAPL,
            DEAVL,
            DEACL,
            FORMAPL,
            FORMAVL,
            FORMACL,
            HABIL,
            HABILGAZ,
            MODCO,
            NOTPL,
            NOTVL,
            NOTCL,
            REJPL,
            REJVL,
            REJCL,
            RETOUR,
            SANCT,
            ADMMODFI,
            //PLE le 27/01/2024 Extention catégorie L
            EXTDECLA,
            EXTRETRA

        }

        //EG le 06/04/2011 : ajout des énumérations
        /*
            'ADMSUPFI','Suppression de la fiche en cours ...'
            'AMEND','Saisie d''une Amende'
            'ANUPL','Annulation de l''agrément PL'
            'ANUVL','Annulation de l''agrément VL'
            'AUDITPL','Ajout d''un audit PL'
            'AUDITVL','Ajout d''un audit VL'
            'AVOTCPL','Saisie de l''avis OTC PL'
            'AVOTCVL','Saisie de l''avis OTC VL'
            'CHCEPL','Changement de centre de rattachement PL'
            'CHCEVL','Changement de centre de rattachement VL'
            'CRECO','Création du contrôleur'
            'CRECOPL','Création du contrôleur / Demande d''agrément PL'
            'CRECOVL','Création du contrôleur / Demande d''agrément VL'
            'CRECOVPL','Création du contrôleur / Demande d''agrément VL etPL'
            'DEAPL','Demande d''un nouvel agrement PL'
            'DEAVL','Demande d''un nouvel agrement VL'
            'FORMAPL','Ajout d''une formation PL'
            'HABIL','Modification de l''habilitation PL'
            'HABILGAZ','Déclaration/retrait de l''habilitation GAZ VL'
            'MODCO','Modification des information d''état civil'
            'NOTPL','Notification de l''agrément PL'
            'NOTVL','Notification de l''agrément VL'
            'REJPL','Rejet de la demande d''agrement PL'
            'REJVL','Rejet de la demande d''agrement VL'
            'RETOUR','Annulation de la dernière modification effectuée'
            'SANCT','Saisie d''une Sanction'
         */

    }

}
