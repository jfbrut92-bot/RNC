using System.Collections.Generic;

namespace Utac.Rnc2.BusinessObjects
{
    public partial class rHabilitation : BusinessBase<string>
    {
        #region Declarations

        private string _libelleHabilitation = null;

        private IList<pContoleurEncour> _pContoleurEncours = new List<pContoleurEncour>();
        private IList<pControleurHisto> _pControleurHistos = new List<pControleurHisto>();
        private IList<rCategorieVehicule> _rCategorieVehicules = new List<rCategorieVehicule>();
        private IList<rTypeFormation> _rTypeFormations = new List<rTypeFormation>();

        #endregion

        #region Constructors

        public rHabilitation() { }

        #endregion

        #region Methods

        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append(this.GetType().FullName);
            sb.Append(_libelleHabilitation);

            return sb.ToString().GetHashCode();
        }

        #endregion

        #region Properties

        public virtual string LibelleHabilitation
        {
            get { return _libelleHabilitation; }
            set
            {
                OnLibelleHabilitationChanging();
                _libelleHabilitation = value;
                OnLibelleHabilitationChanged();
            }
        }
        partial void OnLibelleHabilitationChanging();
        partial void OnLibelleHabilitationChanged();

        public virtual IList<pContoleurEncour> PContoleurEncours
        {
            get { return _pContoleurEncours; }
            set
            {
                OnPContoleurEncoursChanging();
                _pContoleurEncours = value;
                OnPContoleurEncoursChanged();
            }
        }
        partial void OnPContoleurEncoursChanging();
        partial void OnPContoleurEncoursChanged();

        public virtual IList<pControleurHisto> PControleurHistos
        {
            get { return _pControleurHistos; }
            set
            {
                OnPControleurHistosChanging();
                _pControleurHistos = value;
                OnPControleurHistosChanged();
            }
        }
        partial void OnPControleurHistosChanging();
        partial void OnPControleurHistosChanged();

        public virtual IList<rCategorieVehicule> RCategorieVehicules
        {
            get { return _rCategorieVehicules; }
            set
            {
                OnRCategorieVehiculesChanging();
                _rCategorieVehicules = value;
                OnRCategorieVehiculesChanged();
            }
        }
        partial void OnRCategorieVehiculesChanging();
        partial void OnRCategorieVehiculesChanged();

        public virtual IList<rTypeFormation> RTypeFormations
        {
            get { return _rTypeFormations; }
            set
            {
                OnRTypeFormationsChanging();
                _rTypeFormations = value;
                OnRTypeFormationsChanged();
            }
        }
        partial void OnRTypeFormationsChanging();
        partial void OnRTypeFormationsChanged();

        #endregion
    }
}
