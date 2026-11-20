namespace Utac.Rnc2.BusinessObjects
{
    public class RechercheCentreAgreeResultat
    {
        public pCentreEncour.ResultatRechercheCentre Resultat { get; set; }
        public bool EstRechercheValide { get; set; }
        public bool EstCentreAuxiliaire { get; set; }
        public pCentreEncour CentreTrouve { get; set; }
    }
}
