using System;

namespace Utac.Rnc2.BusinessObjects
{

    public class CommuneInsee : BusinessBase<int>
    {

        //private int numeroInsee;
        private String _nomCommune;

        public virtual String NomCommune
        {
            get { return _nomCommune; }
            set { _nomCommune = value; }
        }
        private String _codePostal;

        public virtual String CodePostal
        {
            get { return _codePostal; }
            set { _codePostal = value; }
        }
        private String _departement;

        public virtual String Departement
        {
            get { return _departement; }
            set { _departement = value; }
        }

        public CommuneInsee() { }

        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append(this.GetType().FullName);
            sb.Append(this._nomCommune);
            sb.Append(this._codePostal);
            sb.Append(this._departement);

            return sb.ToString().GetHashCode();
        }

        protected override void validate()
        {
            throw new NotImplementedException();
        }
    }

}
