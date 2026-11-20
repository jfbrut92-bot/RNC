using System;

namespace Utac.Rnc2.BusinessObjects
{

    public partial class Parametre : BusinessBase<int>
    {

        private string _nom;
        private string _valeur;

        public virtual string Nom
        {
            get { return _nom; }
            set
            {
                OnNomChanging();
                _nom = value;
                OnNomChanged();
            }
        }
        partial void OnNomChanging();
        partial void OnNomChanged();

        public virtual string Valeur
        {
            get { return _valeur; }
            set
            {
                OnValeurChanging();
                _valeur = value;
                OnValeurChanged();
            }
        }
        partial void OnValeurChanging();
        partial void OnValeurChanged();

        public Parametre() { }

        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append(this.GetType().FullName);
            sb.Append(this._nom);
            sb.Append(this._valeur);

            return sb.ToString().GetHashCode();
        }

        protected override void validate()
        {
            throw new NotImplementedException();
        }
    }

}
