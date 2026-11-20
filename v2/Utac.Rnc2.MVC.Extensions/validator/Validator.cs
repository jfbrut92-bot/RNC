namespace Utac.Rnc2.MVC.Extensions.validator
{
    public class Validator
    {
        public static bool IsValidEmail(string Email)
        {
            if (Email == null)
                return (false);

            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            System.Text.RegularExpressions.Regex _Regex = new System.Text.RegularExpressions.Regex(strRegex);
            if (_Regex.IsMatch(Email))
                return (true);
            else
                return (false);
        }

        public static bool IsValidNumeroSiren(string numeroSiren)
        {
            if (numeroSiren == null)
                return (false);

            string strRegex = "[0-9]{9}";
            System.Text.RegularExpressions.Regex _Regex = new System.Text.RegularExpressions.Regex(strRegex);
            if (_Regex.IsMatch(numeroSiren))
                return (true);
            else
                return (false);
        }

        public static bool IsValidIdentifiantUtilisateur(string Id)
        {
            string strRegex = @"^[0-9a-z]{1,20}$";
            System.Text.RegularExpressions.Regex _Regex = new System.Text.RegularExpressions.Regex(strRegex);
            if (_Regex.IsMatch(Id))
                return (true);
            else
                return (false);
        }

        public static bool IsValidMotDePasseUtilisateur(string motDePasse)
        {
            string strRegex = @"^\S*$";
            System.Text.RegularExpressions.Regex _Regex = new System.Text.RegularExpressions.Regex(strRegex);
            if (_Regex.IsMatch(motDePasse))
                return (true);
            else
                return (false);
        }

        public static bool IsValidNumeroAccuseReception(string numero)
        {
            string strRegex = @"^[0-9a-z\#\*\%\+]{4}$";
            System.Text.RegularExpressions.Regex _Regex = new System.Text.RegularExpressions.Regex(strRegex);
            if (_Regex.IsMatch(numero))
                return (true);
            else
                return (false);
        }
    }

}
