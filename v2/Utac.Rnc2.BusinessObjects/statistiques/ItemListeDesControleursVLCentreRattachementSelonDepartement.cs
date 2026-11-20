namespace Utac.Rnc2.BusinessObjects.statistiques
{
    public partial class ItemListeDesControleursVLCentreRattachementSelonDepartement
    {
        public pContoleurEncour Controleur { get; set; }
        public string IdPrefecture { get; set; }
        public string CodDepartement { get; set; }
        public string LibellePrefecture { get; set; }
        public string TypeCentre { get; set; }
        public string RaisonSociale { get; set; }
        public string CodeReseau { get; set; }
        public string DateEffetNotificationVl { get; set; }
        public string CodAgrementVl { get; set; }
        public string L1Adresse { get; set; }
        public string L2Adresse { get; set; }
        public string CpCentre { get; set; }
        public string Ville { get; set; }
        public string ControleurNom { get; set; }
        public string ControleurCodeAgrementVl { get; set; }
        public string ControleurDateEffetNotificationVl { get; set; }
        public string ControleurDateNaissance { get; set; }
        public string ControleurEtatAgrementVl { get; set; }
        public string ControleurHabilitationGaz { get; set; }
        public string ControleurDateHabilitationGaz { get; set; }
        public string LibelleRegion { get; set; }

        //public string ControleurEtatAgrementVlAsLibelle
        //{
        //    /*  ETAT agrement
        //    *  0 : AGREMENT_KO : le contrôleur n'a pas d'agrément valide. -->annulation de l'agrément précédent ou retrait définitif.
        //       1 : AGREMENT_OK : La préfecture a notifié l'agrément du contrôleur .
        //       2 : AGREMENT_DEMANDE_EN COURS : L'agrément est en cours de validation par la prefecture.
        //       3 : AGREMENT_REJETE : La demande d'agrément a été rejeté par la préfecture. 
        //   */
        //    get
        //    {
        //        switch (this.ControleurEtatAgrementVl)
        //        {
        //            case "0": return this.ControleurCodeAgrementVl != null ? "annulé" : "sans agrément";
        //            case "1": //todo sanction 
        //                if (this.ExisteSanctionEncours(rCategorieVehicule.CategorieEnum.VL))
        //                {
        //                    return "suspension";
        //                }
        //                else
        //                {
        //                    return "valide";
        //                }

        //            case "2": return "demande en cours";
        //            case "3": return "rejeté";
        //            default: return "";

        //        }
        //    }
        //}

        //public string ControleurHabilitationGazAsLibelle
        //{
        //    get
        //    {
        //        switch (this.ControleurHabilitationGaz)
        //        {
        //            case "0": return "Oui";
        //            case "1": return "Non";
        //            default: return "";

        //        }
        //    }

        //}

    }
}
