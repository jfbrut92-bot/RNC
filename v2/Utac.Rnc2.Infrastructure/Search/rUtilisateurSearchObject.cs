namespace Utac.Rnc2.Infrastructure.Search
{
    public class rUtilisateurSearchObject : ISearchParameter
    {
        private int? _idDepartement;
        private int? _idRegion;
        private string _codeReseau;
        private string _categorieVehicule;
        private string _nom;
        private int? _idProfil;
        private int? _idProfil2;

        public string Nom
        {
            get { return _nom; }
            set { _nom = value; }
        }

        public int? IdProfil
        {
            get { return _idProfil; }
            set { _idProfil = value; }
        }

        public string CategorieVehicule
        {
            get { return _categorieVehicule; }
            set { _categorieVehicule = value; }
        }

        public string CodeReseau
        {
            get { return _codeReseau; }
            set { _codeReseau = value; }
        }

        public int? IdRegion
        {
            get { return _idRegion; }
            set { _idRegion = value; }
        }

        public int? IdDepartement
        {
            get { return _idDepartement; }
            set { _idDepartement = value; }
        }

        public int? IdProfil2
        {
            get { return _idProfil2; }
            set { _idProfil2 = value; }
        }

        public rUtilisateurSearchObject()
        {
        }
    }
}
