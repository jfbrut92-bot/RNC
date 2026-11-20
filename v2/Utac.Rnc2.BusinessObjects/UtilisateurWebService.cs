using System;

namespace Utac.Rnc2.BusinessObjects
{

    public partial class UtilisateurWebService : BusinessBase<int>
    {

        private string _login = string.Empty;

        public virtual string Login
        {
            get { return _login; }
            set { _login = value; }
        }

        private DateTime? _dateCreation = null;

        public virtual DateTime? DateCreation
        {
            get { return _dateCreation; }
            set { _dateCreation = value; }
        }

        private DateTime? _dateMaj = null;

        public virtual DateTime? DateMaj
        {
            get { return _dateMaj; }
            set { _dateMaj = value; }
        }

        private string _password = string.Empty;

        public virtual string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        private string _codReseau = string.Empty;

        public virtual string CodReseau
        {
            get { return _codReseau; }
            set { _codReseau = value; }
        }

        public UtilisateurWebService() { }

        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append(this.GetType().FullName);
            sb.Append(this._login);
            sb.Append(this._dateCreation);
            sb.Append(this._dateMaj);
            sb.Append(this._password);
            sb.Append(this._codReseau);

            return sb.ToString().GetHashCode();
        }

        protected override void validate()
        {
            throw new NotImplementedException();
        }
    }

}
