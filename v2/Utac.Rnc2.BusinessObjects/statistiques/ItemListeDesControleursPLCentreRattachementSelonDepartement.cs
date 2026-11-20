namespace Utac.Rnc2.BusinessObjects.statistiques
{
    public partial class ItemListeDesControleursPLCentreRattachementSelonDepartement
    {
        public string IdPrefecture { get; set; }
        public string CodDepartement { get; set; }
        public string Region { get; set; }
        public string LibellePrefecture { get; set; }
        public string TypeCentre { get; set; }
        public string RaisonSociale { get; set; }
        public string CodeReseau { get; set; }
        public string CodAgrementPl { get; set; }
        public string DateEffetNotificationPl { get; set; }
        public string EtatAgrementPl { get; set; }
        public string CpCentre { get; set; }
        public string Ville { get; set; }
        public string ControleurNom { get; set; }
        public string ControleurCodeAgrementPl { get; set; }
        public string ControleurDateEffetNotificationPl { get; set; }
        public string ControleurDateNaissance { get; set; }
        public string ControleurCodeHabilitation { get; set; }
        public string ControleurDateHabilitation { get; set; }
        public string LibelleRegion { get; set; }

        public string EtatAgrementPlAsLibelle
        {
            /*  ETAT agrement
            *  0 : AGREMENT_KO : le contrôleur n'a pas d'agrément valide. -->annulation de l'agrément précédent ou retrait définitif.
               1 : AGREMENT_OK : La préfecture a notifié l'agrément du contrôleur .
               2 : AGREMENT_DEMANDE_EN COURS : L'agrément est en cours de validation par la prefecture.
               3 : AGREMENT_REJETE : La demande d'agrément a été rejeté par la préfecture. 
           */
            get
            {
                switch (this.EtatAgrementPl)
                {
                    case "0": return this.CodAgrementPl != null ? "annulé" : "sans agrément";
                    case "1": //todo sanction 

                        return "valide";

                    case "2": return "demande en cours";
                    case "3": return "rejeté";
                    default: return "";

                }
            }
        }

    }
}

