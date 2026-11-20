using System.Collections.Generic;

namespace Utac.Rnc2.BusinessObjects
{
    public partial class rMotifModification : BusinessBase<string>
    {
        #region Declarations

        private string _libelle = null;

        private IList<pCentreEncour> _pCentreEncours = new List<pCentreEncour>();
        private IList<pCentreHisto> _pCentreHistos = new List<pCentreHisto>();

        #endregion

        #region Constructors

        public rMotifModification() { }

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

        public virtual IList<pCentreEncour> PCentreEncours
        {
            get { return _pCentreEncours; }
            set
            {
                OnPCentreEncoursChanging();
                _pCentreEncours = value;
                OnPCentreEncoursChanged();
            }
        }
        partial void OnPCentreEncoursChanging();
        partial void OnPCentreEncoursChanged();

        public virtual IList<pCentreHisto> PCentreHistos
        {
            get { return _pCentreHistos; }
            set
            {
                OnPCentreHistosChanging();
                _pCentreHistos = value;
                OnPCentreHistosChanged();
            }
        }
        partial void OnPCentreHistosChanging();
        partial void OnPCentreHistosChanged();

        #endregion
    }
}
