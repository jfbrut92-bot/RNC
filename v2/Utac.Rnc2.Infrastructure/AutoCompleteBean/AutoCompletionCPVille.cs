namespace Utac.Rnc2.Infrastructure.AutoCompleteBean
{
    public class AutoCompletionCPVille
    {
        private string codePostal;
        private string ville;

        public AutoCompletionCPVille(string p_codePostal, string p_ville)
        {
            codePostal = p_codePostal;
            ville = p_ville;
        }

        public string CodePostal
        {
            get { return codePostal; }
            set { codePostal = value; }
        }

        public string Ville
        {
            get { return ville; }
            set { ville = value; }
        }
    }
}
