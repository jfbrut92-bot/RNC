namespace Utac.Rnc2.BusinessObjects
{
    public partial class rMotifModification : BusinessBase<string>
    {
        protected override void validate()
        {
            /* Implementer ICI les r?gles de validation */
        }

        public enum TypeModification
        {
            ADMMODFI,
            ADMSUPFI,
            AMEND,
            ANUPL,
            ANUVL,
            ANUCL,
            AVOTCPL,
            AVOTCVL,
            CHDECOM,
            CHEXPLPL,
            CHEXPLVL,
            CHEXPLCL,
            CHHORAI,
            CHLOCAPL,
            CHLOCAVL,
            CHLOCACL,
            CHRATTPL,
            CHRATTVL,
            CHRATTCL,
            CHREPLEG,
            CRECE,
            CRECEPL,
            CRECEVL,
            CRECECL,
            CRECEVPL,
            DEAPL,
            DEAVL,
            DEACL,
            DERPL,
            MODAUTRE,
            NOTPL,
            NOTRPL,
            NOTVL,
            NOTCL,
            RDV,
            REJPL,
            REJRPL,
            REJVL,
            REJCL,
            RETOUR,
            SANCT,
            //EG le 30/11/2011
            EQUIPVL,
            EQUIPPL,
            EQUIPCL,
            //YM le 10/02/2012
            DEROG,
            MODDEROG,
            SUPDEROG,
            //PLE le 22/01/2024
            EXTDECLA,
            EXTRETRA
        }
        /*
        YM 04/05/2011  : ajout des énumérations

        ADMMODFI	Reprise des données existantes par OTC
        ADMSUPFI	Suppression de la fiche en cours ...
        AMEND	Ajout d'une amende
        ANUPL	Annulation de l'agrément PL
        ANUVL	Annulation de l'agrément VL
        AVOTCPL	Saisie de l'avis OTC PL
        AVOTCVL	Saisie de l'avis OTC VL
        CHDECOM	Changement de dénomination sociale
        CHEXPLPL	Changement d'exploitant PL
        CHEXPLVL	Changement d'exploitant VL
        CHHORAI	Changement des horaires d'ouverture
        CHLOCAPL	Changement de localisation PL
        CHLOCAVL	Changement de localisation VL
        CHRATTPL	Changement de réseau de rattachement PL
        CHRATTVL	Changement de réseau de rattachement VL
        CHREPLEG	Changement de représentant légal
        CRECE	Création du Centre/Inst. Aux.
        CRECEPL	Création du Centre/Inst. Aux. et Demande d'agrément PL
        CRECEVL	Création du Centre/Inst. Aux. et Demande d'agrément VL
        CRECEVPL	Création du Centre/Inst.Aux. & Demande d'agrément VL/PL
        DEAPL	Demande d'un nouvel agrément PL
        DEAVL	Demande d'un nouvel agrément VL
        DERPL	Demande renouvellement de l'agrément PL
        MODAUTRE	Autres modifications/Correction/Complément
        NOTPL	Notification de l'agrément PL
        NOTRPL	Notification du renouvellement de l'agrément PL
        NOTVL	Notification de l'agrément VL
        RDV	Saisie des informations de prise de Rendez-vous
        REJPL	Rejet de la demande d'agrément PL
        REJRPL	Rejet de la demande de renouvellement d'agrément PL
        REJVL	Rejet de la demande d'agrément VL
        RETOUR	Annulation de la dernière modification effectuée
        SANCT	Ajout d'une sanction
        DEROG   Ajout d'une dérogation
        */
    }
}
