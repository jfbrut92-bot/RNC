namespace Utac.Rnc2.BusinessObjects.statistiques
{
    public class NombreControleursParRegionEtDepartement
    {
        public string LibelleRegion { get; set; }
        public string CodeDepartement { get; set; }
        public int NombreControleurPL { get; set; }
        public int NombreControleurVL { get; set; }
        public int NombreControleurPlEtVl { get; set; }

        public NombreControleursParRegionEtDepartement()
        {
        }

    }
}
