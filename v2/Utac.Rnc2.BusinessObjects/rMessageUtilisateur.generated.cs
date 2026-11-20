namespace Utac.Rnc2.BusinessObjects
{
    public partial class rMessageUtilisateur : BusinessBase<int>
    {
        #region Declarations

        private string _txtMessageBienvenue = null;

        #endregion

        #region Constructors

        public rMessageUtilisateur() { }

        #endregion

        #region Methods

        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append(this.GetType().FullName);
            sb.Append(_txtMessageBienvenue);

            return sb.ToString().GetHashCode();
        }

        #endregion

        #region Properties

        public virtual string TxtMessageBienvenue
        {
            get { return _txtMessageBienvenue; }
            set
            {
                OnTxtMessageBienvenueChanging();
                _txtMessageBienvenue = value;
                OnTxtMessageBienvenueChanged();
            }
        }
        partial void OnTxtMessageBienvenueChanging();
        partial void OnTxtMessageBienvenueChanged();

        #endregion
    }
}
