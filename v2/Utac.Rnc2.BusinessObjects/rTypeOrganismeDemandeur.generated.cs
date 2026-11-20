using System.Collections.Generic;

namespace Utac.Rnc2.BusinessObjects
{
    public partial class rTypeOrganismeDemandeur : BusinessBase<int>
    {
        #region Declarations

        private string _libelle = null;

        private IList<pSanctionCentre> _pSanctionCentres = new List<pSanctionCentre>();
        private IList<pSanctionControleur> _pSanctionControleurs = new List<pSanctionControleur>();

        #endregion

        #region Constructors

        public rTypeOrganismeDemandeur() { }

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
