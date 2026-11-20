using Utac.Rnc2.BusinessObjects;

namespace Utac.Rnc2.UI.Web.DataSets
{

    public partial class DSficheInstallation
    {
        public DataSets.DSficheInstallation mapInstallationDataset(pCentreEncour centre)
        {

            DSficheInstallation ds = new DSficheInstallation();

            //
            //Mapping du centre
            //
            DSficheInstallation.pCentreEncourDataTable dt = ds.pCentreEncour;
            DSficheInstallation.pCentreEncourRow dr = dt.NewpCentreEncourRow();

            dr.AccesPublicCl = centre.AccesPublicCl;
            dr.AccesPublicPl = centre.AccesPublicPl;
            dr.AccesPublicVl = centre.AccesPublicVl;
            dr.AdresseComplete = centre.AdresseComplete;
            dr.AncienNumeroAgrementCl = centre.AncienNumeroAgrementCl;
            dr.AncienNumeroAgrementPl = centre.AncienNumeroAgrementPl;
            dr.AncienNumeroAgrementVl = centre.AncienNumeroAgrementVl;
            dr.CodAgremCl = centre.CodAgremCl;
            dr.CodAgremPl = centre.CodAgremPl;
            dr.CodAgremVl = centre.CodAgremVl;
            dr.CodDepartement = centre.CodDepartement;
            dr.CommentaireAvisOtcCl = centre.CommentaireAvisOtcCl;
            dr.CommentaireAvisOtcPl = centre.CommentaireAvisOtcPl;
            dr.CommentaireAvisOtcVl = centre.CommentaireAvisOtcVl;
            dr.CpCentre = centre.CpCentre;
            if (centre.DateEcheanceAgrementPl != null) { dr.DateEcheanceAgrementPl = centre.DateEcheanceAgrementPl.Value; }

            if (centre.DateEffetModification != null) { dr.DateEffetModification = centre.DateEffetModification.Value; }

            if (centre.DateEffetNotificationCl != null) { dr.DateEffetNotificationCl = centre.DateEffetNotificationCl.Value; }

            if (centre.DateEffetNotificationPl != null) { dr.DateEffetNotificationPl = centre.DateEffetNotificationPl.Value; }

            if (centre.DateEffetNotificationVl != null) { dr.DateEffetNotificationVl = centre.DateEffetNotificationVl.Value; }

            if (centre.DateFinAgrementCl != null) { dr.DateFinAgrementCl = centre.DateFinAgrementCl.Value; }

            if (centre.DateFinAgrementPl != null) { dr.DateFinAgrementPl = centre.DateFinAgrementPl.Value; }

            if (centre.DateFinAgrementVl != null) { dr.DateFinAgrementVl = centre.DateFinAgrementVl.Value; }

            if (centre.DateMiseaJour != null) { dr.DateMiseaJour = centre.DateMiseaJour; }
            dr.EnseigneCommerciale = centre.EnseigneCommerciale;
            dr.EtatAgrementClAsLibelle = centre.EtatAgrementClAsLibelle;
            dr.EtatAgrementPlAsLibelle = centre.EtatAgrementPlAsLibelle;
            dr.EtatAgrementVlAsLibelle = centre.EtatAgrementVlAsLibelle;
            dr.MotifFinAgrementClAsLibelle = centre.MotifFinAgrementClAsLibelle;
            dr.MotifFinAgrementPlAsLibelle = centre.MotifFinAgrementPlAsLibelle;
            dr.MotifFinAgrementVlAsLibelle = centre.MotifFinAgrementVlAsLibelle;
            dr.NomRepresentantLegal = centre.NomRepresentantLegal;
            dr.NumAccuseReceptionCL = centre.NumAccuseReceptionCL;
            dr.NumAccuseReceptionPL = centre.NumAccuseReceptionPL;
            dr.NumAccuseReceptionVL = centre.NumAccuseReceptionVL;
            dr.NumFax = centre.NumFax;
            dr.NumSiren = centre.NumSiren;
            dr.NumTel = centre.NumTel;
            dr.RaisonSociale = centre.RaisonSociale;
            dr.RMotifModificationLibelle = centre.RMotifModification == null ? "" : centre.RMotifModification.Libelle;
            dr.RPrefectureCodDepartement = centre.RPrefecture == null ? "" : centre.RPrefecture.CodDepartement;
            dr.RStatutJuridiqueibelle = centre.RStatutJuridique == null ? "" : centre.RStatutJuridique.Libelle;
            dr.TypeCentre = centre.TypeCentre;
            dr.Ville = centre.Ville;
            dr.ReseauRattachementCLNom = centre.ReseauRattachementCL == null ? "" : centre.ReseauRattachementCL.Nom + " (" + centre.ReseauRattachementCL.Id + ") ";
            dr.ReseauRattachementPLNom = centre.ReseauRattachementPL == null ? "" : centre.ReseauRattachementPL.Nom + " (" + centre.ReseauRattachementPL.Id + ") ";
            dr.ReseauRattachementVLNom = centre.ReseauRattachementVL == null ? "" : centre.ReseauRattachementVL.Nom + " (" + centre.ReseauRattachementVL.Id + ") ";

            //PLE le 10/03/2024
            if (centre.ExtAgremClCatVeh.HasValue && centre.ExtAgremClCatVeh.Equals((short)rCategorieVehicule.ExtCatvehEnum.VL))
            {
                dr.ExtAgremClStatutVLAsString = centre.ExtAgremClStatutVLAsString;
                dr.ExtAgremClDateDeclaVLAsDate = centre.ExtAgremClDateDeclaVLAsDate.Value;
                dr.ExtAgremClDateValidVLAsDate = centre.ExtAgremClDateValidVLAsDate.Value;
            }
            if (centre.ExtAgremClCatVeh.HasValue && centre.ExtAgremClCatVeh.Equals((short)rCategorieVehicule.ExtCatvehEnum.PL))
            {
                dr.ExtAgremClStatutPLAsString = centre.ExtAgremClStatutPLAsString;
                dr.ExtAgremClDateDeclaPLAsDate = centre.ExtAgremClDateDeclaPLAsDate.Value;
                dr.ExtAgremClDateValidPLAsDate = centre.ExtAgremClDateValidPLAsDate.Value;
            }


            dt.Rows.Add(dr);

            this.mapListeCentreHisto(ds.pCentreHisto, centre.PCentreHisto);
            this.mapListeControleur(ds.pControleurEncourCL, centre.ControleurCLs);
            this.mapListeControleur(ds.pControleurEncourPL, centre.ControleurPLs);
            this.mapListeControleur(ds.pControleurEncourVL, centre.ControleurVLs);
            this.mapEquipementVLCentre(ds.pEquipementVL, centre.EquipementCentreVLs.Count.Equals(0) ? null : centre.EquipementCentreVLs[0]);
            this.mapEquipementPLCentre(ds.pEquipementPL, centre.EquipementCentrePLs.Count.Equals(0) ? null : centre.EquipementCentrePLs[0]);
            this.mapEquipementCLCentre(ds.pEquipementCL, centre.EquipementCentreCLs.Count.Equals(0) ? null : centre.EquipementCentreCLs[0]);
            this.mapCentreDerogations(ds.pDerogationCentre, centre.PDerogationCentres);
            this.mapListeCentreSanction(ds.pSanctionCentre, centre.PSanctionCentres);

            return ds;
        }

        private void mapListeCentreHisto(DSficheInstallation.pCentreHistoDataTable dt, System.Collections.Generic.IList<pCentreHisto> liste)
        {
            foreach (pCentreHisto c in liste)
            {
                DSficheInstallation.pCentreHistoRow dr = dt.NewpCentreHistoRow();
                dr.DateHisto = c.DateHisto;
                dr.RMotifModificationLibelle = c.RMotifModification == null ? "" : c.RMotifModification.Libelle;
                dr.CodAgremVl = c.CodAgremVl;
                dr.CodAgremPl = c.CodAgremPl;
                dr.CodAgremCl = c.CodAgremCl;
                dt.Rows.Add(dr);
            }
        }

        private void mapListeCentreSanction(DSficheInstallation.pSanctionCentreDataTable dt, System.Collections.Generic.IList<pSanctionCentre> liste)
        {
            foreach (pSanctionCentre c in liste)
            {
                DSficheInstallation.pSanctionCentreRow dr = dt.NewpSanctionCentreRow();
                dr.CodAgremCl = c.CodAgremCl;
                dr.CodAgremPl = c.CodAgremPl;
                dr.CodAgremVl = c.CodAgremVl;
                if (c.DateDebut != null) { dr.DateDebut = c.DateDebut.Value; }
                if (c.DateEvenementOrigine != null) { dr.DateEvenementOrigine = c.DateEvenementOrigine.Value; }
                if (c.DateFin != null) { dr.DateFin = c.DateFin.Value; }
                dr.IsAgrementAffecteCL = c.IsAgrementAffecteCL;
                dr.IsAgrementAffectePL = c.IsAgrementAffectePL;
                dr.IsAgrementAffecteVL = c.IsAgrementAffecteVL;
                dr.MotifSanction = c.MotifSanction;
                dr.RTypeOrganismeDemandeurLibelle = (c.RTypeOrganismeDemandeur == null) ? "" : c.RTypeOrganismeDemandeur.Libelle;
                dr.RTypeSanctionControleurLibelle = (c.RTypeSanctionCentre == null) ? "" : c.RTypeSanctionCentre.Libelle;
                dr.TypeSanctionAutre = c.TypeSanctionAutre;
                dt.Rows.Add(dr);
            }
        }

        private void mapListeControleur(System.Data.DataTable dt, System.Collections.Generic.IList<pContoleurEncour> liste)
        {
            System.Data.DataRow dr = null;
            foreach (pContoleurEncour c in liste)
            {
                if (dt.GetType().Equals(typeof(DSficheInstallation.pControleurEncourVLDataTable)))
                {
                    dr = ((DSficheInstallation.pControleurEncourVLDataTable)dt).NewpControleurEncourVLRow();
                }

                if (dt.GetType().Equals(typeof(DSficheInstallation.pControleurEncourPLDataTable)))
                {
                    dr = ((DSficheInstallation.pControleurEncourPLDataTable)dt).NewpControleurEncourPLRow();
                }

                if (dt.GetType().Equals(typeof(DSficheInstallation.pControleurEncourCLDataTable)))
                {
                    dr = ((DSficheInstallation.pControleurEncourCLDataTable)dt).NewpControleurEncourCLRow();
                }

                dr["Nom"] = c.Nom;
                dr["Prenom"] = c.Prenom;
                dr["CodAgremVl"] = c.CodAgremVl;
                dr["CodAgremPl"] = c.CodAgremPl;
                dr["CodAgremCl"] = c.CodAgremCl;
                if (c.DateEffetNotificationCl != null) { dr["DateEffetNotificationCl"] = c.DateEffetNotificationCl; }
                if (c.DateEffetNotificationPl != null) { dr["DateEffetNotificationPl"] = c.DateEffetNotificationPl; }
                if (c.DateEffetNotificationVl != null) { dr["DateEffetNotificationVl"] = c.DateEffetNotificationVl; }
                if (c.DateFinAgrementCl != null) { dr["DateFinAgrementCl"] = c.DateFinAgrementCl; }
                if (c.DateFinAgrementPl != null) { dr["DateFinAgrementPl"] = c.DateFinAgrementPl; }
                if (c.DateFinAgrementVl != null) { dr["DateFinAgrementVl"] = c.DateFinAgrementVl; }

                dt.Rows.Add(dr);
            }
        }

        private void mapEquipementVLCentre(DSficheInstallation.pEquipementVLDataTable dt, pEquipementCentreVL equipement)
        {
            if (equipement != null)
            {
                DSficheInstallation.pEquipementVLRow dr = dt.NewpEquipementVLRow();
                dr.ControleVehiculeGaz = equipement.ControleVehiculeGaz;
                dr.BancRotationInverse = equipement.BancRotationInverse;
                dr.ControleVehiculeTransmissionSpecifique = equipement.ControleVehiculeTransmissionSpecifique;
                dr.Decelerometre = equipement.Decelerometre;
                dr.EquipementRouleauFou = equipement.EquipementRouleauFou;
                dr.LieuStockagePV = equipement.LieuStockagePV;
                dt.Rows.Add(dr);
            }
        }

        private void mapEquipementPLCentre(DSficheInstallation.pEquipementPLDataTable dt, pEquipementCentrePL equipement)
        {
            if (equipement != null)
            {
                DSficheInstallation.pEquipementPLRow dr = dt.NewpEquipementPLRow();
                dr.ControleTCP = equipement.ControleTCP;
                dr.ControleTMD = equipement.ControleTMD;
                dr.Decelerometre = equipement.Decelerometre;
                dr.LieuStockagePV = equipement.LieuStockagePV;
                dt.Rows.Add(dr);
            }
        }

        private void mapEquipementCLCentre(DSficheInstallation.pEquipementCLDataTable dt, pEquipementCentreCL equipement)
        {
            if (equipement != null)
            {
                DSficheInstallation.pEquipementCLRow dr = dt.NewpEquipementCLRow();
                dr.ControleTricyles = equipement.ControleTricyles;
                dr.LieuStockagePV = equipement.LieuStockagePV;
                dt.Rows.Add(dr);
            }
        }

        private void mapCentreDerogations(DSficheInstallation.pDerogationCentreDataTable dt, System.Collections.Generic.IList<pDerogationCentre> liste)
        {
            foreach (pDerogationCentre derog in liste)
            {
                DSficheInstallation.pDerogationCentreRow dr = dt.NewpDerogationCentreRow();
                dr.categorieVehicule = derog.CategorieVehicule.ToString();
                dr.commentaire = derog.Commentaire;
                if (derog.DateDebut != null) { dr.dateDebut = derog.DateDebut.Value; }
                if (derog.DateFin != null) { dr.dateFin = derog.DateFin.Value; }
                dt.Rows.Add(dr);
            }
        }

    }

}
