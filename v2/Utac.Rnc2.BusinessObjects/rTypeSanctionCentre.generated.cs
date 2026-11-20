using System.Collections.Generic;

namespace Utac.Rnc2.BusinessObjects
{
    public partial class rTypeSanctionCentre : BusinessBase<int>
    {
        #region Declarations

        private string _libelle = null;

        private IList<pSanctionCentre> _pSanctionCentres = new List<pSanctionCentre>();

        #endregion

        #region Constructors

        public rTypeSanctionCentre() { }

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

        public virtual IList<pSanctionCentre> PSanctionCentres
        {
            get { return _pSanctionCentres; }
            set
            {
                OnPSanctionCentresChanging();
                _pSanctionCentres = value;
                OnPSanctionCentresChanged();
            }
        }
        partial void OnPSanctionCentresChanging();
        partial void OnPSanctionCentresChanged();

        #endregion
    }
}
