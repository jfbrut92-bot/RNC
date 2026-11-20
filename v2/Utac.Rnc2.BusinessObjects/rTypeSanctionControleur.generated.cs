using System.Collections.Generic;

namespace Utac.Rnc2.BusinessObjects
{
    public partial class rTypeSanctionControleur : BusinessBase<int>
    {
        #region Declarations

        private string _libelle = null;

        private IList<pSanctionControleur> _pSanctionControleurs = new List<pSanctionControleur>();

        #endregion

        #region Constructors

        public rTypeSanctionControleur() { }

        #endregion

        #region Methods

        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append(this.GetType().FullName);
            sb.Append(_libelle);

            return sb.ToString().GetHashCode();
        }

        #endregion

        #region Properties

        public virtual string Libelle
        {
            get { return _libelle; }
            set
            {
                OnLibelleChanging();
                _libelle = value;
                OnLibelleChanged();
            }
        }
        partial void OnLibelleChanging();
        partial void OnLibelleChanged();

        public virtual IList<pSanctionControleur> PSanctionControleurs
        {
            get { return _pSanctionControleurs; }
            set
            {
                OnPSanctionControleursChanging();
                _pSanctionControleurs = value;
                OnPSanctionControleursChanged();
            }
        }
        partial void OnPSanctionControleursChanging();
        partial void OnPSanctionControleursChanged();

        #endregion
    }
}
