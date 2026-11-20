using Utac.Rnc2.BusinessObjects;

namespace Utac.Rnc2.UI.Web.DataSets
{

    public partial class DSficheControleur
    {

        public DataSets.DSficheControleur mapControleurDataset(pContoleurEncour controleur)
        {

            DSficheControleur ds = new DSficheControleur();

            //
            //Mapping du controleur
            //
            DSficheControleur.pContoleurEncourDataTable dt = ds.pContoleurEncour;
            DSficheControleur.pContoleurEncourRow dr = dt.NewpContoleurEncourRow();
            dr.CodAgremCl = controleur.CodAgremCl;
            dr.CodAgremPl = controleur.CodAgremPl;
            dr.CodAgremRattachementCl = controleur.CodAgremRattachementCl;
            dr.CodAgremRattachementPl = controleur.CodAgremRattachementPl;
            dr.CodAgremRattachementVl = controleur.CodAgremRattachementVl;
            dr.CodAgremVl = controleur.CodAgremVl;
            dr.CodDepartement = controleur.CodDepartement;
            dr.CommentaireAvisOtcVl = controleur.CommentaireAvisOtcVl;
            dr.CommentaireAvisOtcPl = controleur.CommentaireAvisOtcPl;
            dr.CommentaireAvisOtcCl = controleur.CommentaireAvisOtcCl;
            if (controleur.DateDebutFinHabilitationGazVL != null) { dr.DateDebutFinHabilitationGazVL = controleur.DateDebutFinHabilitationGazVL.Value; }
            if (controleur.DateDebutHabilitationPl != null) { dr.DateDebutHabilitationPl = controleur.DateDebutHabilitationPl.Value; }
            if (controleur.DateDebutRattachementCl != null) { dr.DateDebutRattachementCl = controleur.DateDebutRattachementCl.Value; }
            if (controleur.DateDebutRattachementPl != null) { dr.DateDebutRattachementPl = controleur.DateDebutRattachementPl.Value; }
            if (controleur.DateDebutRattachementVl != null) { dr.DateDebutRattachementVl = controleur.DateDebutRattachementVl.Value; }
            if (controleur.DateEffetNotificationCl != null) { dr.DateEffetNotificationCl = controleur.DateEffetNotificationCl.Value; }
            if (controleur.DateEffetNotificationPl != null) { dr.DateEffetNotificationPl = controleur.DateEffetNotificationPl.Value; }
            if (controleur.DateEffetNotificationVl != null) { dr.DateEffetNotificationVl = controleur.DateEffetNotificationVl.Value; }
            if (controleur.DateFinAgrementCl != null) { dr.DateFinAgrementCl = controleur.DateFinAgrementCl.Value; }
            if (controleur.DateFinAgrementPl != null) { dr.DateFinAgrementPl = controleur.DateFinAgrementPl.Value; }
            if (controleur.DateFinAgrementVl != null) { dr.DateFinAgrementVl = controleur.DateFinAgrementVl.Value; }
            if (controleur.DateFinRattachementCl != null) { dr.DateFinRattachementCl = controleur.DateFinRattachementCl.Value; }
            if (controleur.DateFinRattachementPl != null) { dr.DateFinRattachementPl = controleur.DateFinRattachementPl.Value; }
            if (controleur.DateFinRattachementVl != null) { dr.DateFinRattachementVl = controleur.DateFinRattachementVl.Value; }
            if (controleur.DateMiseaJour != null) { dr.DateMiseaJour = controleur.DateMiseaJour; }
            if (controleur.DateNaissance != null) { dr.DateNaissance = controleur.DateNaissance.Value; }
            dr.EstPrestataire = controleur.EstPrestataire;
            dr.EtatAgrementCl = controleur.EtatAgrementClAsLibelle;
            dr.EtatAgrementPl = controleur.EtatAgrementPlAsLibelle;
            dr.EtatAgrementVl = controleur.EtatAgrementVlAsLibelle;
            dr.HabilitationGazVL = controleur.HabilitationGazVLAsString;
            dr.MotifFinAgrementCl = controleur.MotifFinAgrementCl;
            dr.MotifFinAgrementPl = controleur.MotifFinAgrementPl;
            dr.MotifFinAgrementVl = controleur.MotifFinAgrementVl;
            dr.Nom = controleur.Nom;
            dr.Prenom = controleur.Prenom;
            if (controleur.RHabilitation != null) { dr.RHabilitation = controleur.RHabilitation.LibelleAvecCode; }
            dr.RTypeDemandeControleurLibelle = controleur.RTypeDemandeControleur.Libelle;

            dt.Rows.Add(dr);

            //
            //Mapping de l'historique controleur 
            //

            DSficheControleur.PControleurHistoDataTable dthisto = ds.PControleurHisto;
            this.mapListeControleurHisto(dthisto, controleur.PControleurHistoriques);

            //
            //Mapping de la liste des sanctions 
            //

            DSficheControleur.PSanctionControleurDataTable dtsanction = ds.PSanctionControleur;
            this.mapListeControleurSanction(dtsanction, controleur.PSanctionControleurs);

            return ds;

        }
        private void mapListeControleurHisto(DSficheControleur.PControleurHistoDataTable dt, System.Collections.Generic.IList<pControleurHisto> liste)
        {
            foreach (pControleurHisto controleur in liste)
            {
                DSficheControleur.PControleurHistoRow dr = dt.NewPControleurHistoRow();
                dr.CodAgremCl = controleur.CodAgremCl;
                dr.CodAgremPl = controleur.CodAgremPl;
                dr.CodAgremRattachementCl = controleur.CodAgremRattachementCl;
                dr.CodAgremRattachementPl = controleur.CodAgremRattachementPl;
                dr.CodAgremRattachementVl = controleur.CodAgremRattachementVl;
                dr.CodAgremVl = controleur.CodAgremVl;
                dr.CodDepartement = controleur.CodDepartement;
                if (controleur.DateDebutFinHabilitationGazVL != null) { dr.DateDebutFinHabilitationGazVL = controleur.DateDebutFinHabilitationGazVL.Value; }
                if (controleur.DateDebutHabilitationPl != null) { dr.DateDebutHabilitationPl = controleur.DateDebutHabilitationPl.Value; }
                if (controleur.DateDebutRattachementCl != null) { dr.DateDebutRattachementCl = controleur.DateDebutRattachementCl.Value; }
                if (controleur.DateDebutRattachementPl != null) { dr.DateDebutRattachementPl = controleur.DateDebutRattachementPl.Value; }
                if (controleur.DateDebutRattachementVl != null) { dr.DateDebutRattachementVl = controleur.DateDebutRattachementVl.Value; }
                if (controleur.DateEffetNotificationCl != null) { dr.DateEffetNotificationCl = controleur.DateEffetNotificationCl.Value; }
                if (controleur.DateEffetNotificationPl != null) { dr.DateEffetNotificationPl = controleur.DateEffetNotificationPl.Value; }
                if (controleur.DateEffetNotificationVl != null) { dr.DateEffetNotificationVl = controleur.DateEffetNotificationVl.Value; }
                if (controleur.DateFinAgrementCl != null) { dr.DateFinAgrementCl = controleur.DateFinAgrementCl.Value; }
                if (controleur.DateFinAgrementPl != null) { dr.DateFinAgrementPl = controleur.DateFinAgrementPl.Value; }
                if (controleur.DateFinAgrementVl != null) { dr.DateFinAgrementVl = controleur.DateFinAgrementVl.Value; }
                if (controleur.DateFinRattachementCl != null) { dr.DateFinRattachementCl = controleur.DateFinRattachementCl.Value; }
                if (controleur.DateFinRattachementPl != null) { dr.DateFinRattachementPl = controleur.DateFinRattachementPl.Value; }
                if (controleur.DateFinRattachementVl != null) { dr.DateFinRattachementVl = controleur.DateFinRattachementVl.Value; }
                if (controleur.DateHisto != null) { dr.DateHisto = controleur.DateHisto; }
                if (controleur.DateNaissance != null) { dr.DateNaissance = controleur.DateNaissance.Value; }
                dr.EstPrestataire = controleur.EstPrestataire;
                dr.EtatAgrementCl = controleur.EtatAgrementClAsLibelle;
                dr.EtatAgrementPl = controleur.EtatAgrementPlAsLibelle;
                dr.EtatAgrementVl = controleur.EtatAgrementVlAsLibelle;
                dr.HabilitationGazVL = controleur.HabilitationGazVLAsString;
                dr.MotifFinAgrementCl = controleur.MotifFinAgrementCl;
                dr.MotifFinAgrementPl = controleur.MotifFinAgrementPl;
                dr.MotifFinAgrementVl = controleur.MotifFinAgrementVl;
                dr.Nom = controleur.Nom;
                dr.Prenom = controleur.Prenom;
                if (controleur.RHabilitation != null) { dr.RHabilitation = controleur.RHabilitation.LibelleAvecCode; }
                dr.RTypeDemandeControleurLibelle = controleur.RTypeDemandeControleur.Libelle;

                dt.Rows.Add(dr);
            }

        }

        private void mapListeControleurSanction(DSficheControleur.PSanctionControleurDataTable dt, System.Collections.Generic.IList<pSanctionControleur> liste)
        {
            foreach (pSanctionControleur sanction in liste)
            {
                DSficheControleur.PSanctionControleurRow dr = dt.NewPSanctionControleurRow();
                dr.CodAgremCl = sanction.CodAgremCl;
                dr.CodAgremPl = sanction.CodAgremPl;
                dr.CodAgremVl = sanction.CodAgremVl;
                if (sanction.DateDebut != null) { dr.DateDebut = sanction.DateDebut.Value; }
                if (sanction.DateFin != null) { dr.DateFin = sanction.DateFin.Value; }
                if (sanction.DateEvenementOrigine != null) { dr.DateEvenementOrigine = sanction.DateEvenementOrigine.Value; }
                dr.IsAgrementAffecteCL = sanction.IsAgrementAffecteCL;
                dr.IsAgrementAffectePL = sanction.IsAgrementAffectePL;
                dr.IsAgrementAffecteVL = sanction.IsAgrementAffecteVL;
                dr.MotifSanction = sanction.MotifSanction;
                dr.RTypeOrganismeDemandeurLibelle = sanction.RTypeOrganismeDemandeur.Libelle;
                dr.RTypeSanctionControleurLibelle = sanction.RTypeSanctionControleur.Libelle;
                //dr.TypeAgrementAffecte = sanction.TypeAgrementAffecte.ToString();
                dr.TypeSanctionAutre = sanction.TypeSanctionAutre;
                dt.Rows.Add(dr);
            }
        }
    }
}
