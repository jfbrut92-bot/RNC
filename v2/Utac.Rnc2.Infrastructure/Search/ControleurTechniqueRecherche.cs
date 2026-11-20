namespace Utac.Rnc2.Infrastructure.Search
{
    public class ControleurTechniqueRecherche : ISearchParameter
    {
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string NumeroAgrementControleurVL { get; set; }
        public string NumeroAgrementCentreVL { get; set; }
        public string NumeroAgrementControleurPL { get; set; }
        public string NumeroAgrementCentrePL { get; set; }

    }
}
