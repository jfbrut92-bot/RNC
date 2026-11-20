using System.Collections.Generic;

namespace Utac.Rnc2.BusinessObjects
{
    public partial class rStatutJuridique : BusinessBase<string>
    {
        #region Declarations

        private string _libelle = null;
        private short? _ordreAffichage = null;

        private IList<pCentreEncour> _pCentreEncours = new List<pCentreEncour>();
        private IList<pCentreHisto> _pCentreHistos = new List<pCentreHisto>();

        #endregion

        #region Constructors

        public rStatutJuridique() { }

        #endregion

        #region Methods

        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append(this.GetType().FullName);
            sb.Append(_libelle);
            sb.Append(_ordreAffichage);

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

        public virtual short? OrdreAffichage
        {
            get { return _ordreAffichage; }
            set
            {
                OnOrdreAffichageChanging();
                _ordreAffichage = value;
                OnOrdreAffichageChanged();
            }
        }
        partial void OnOrdreAffichageChanging();
        partial void OnOrdreAffichageChanged();

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
