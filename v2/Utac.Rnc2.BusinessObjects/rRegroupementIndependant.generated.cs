using System;
using System.Collections.Generic;

namespace Utac.Rnc2.BusinessObjects
{
    public partial class rRegroupementIndependant : BusinessBase<int>
    {
        #region Declarations

		private string _libelleRegroupementInd = String.Empty;

		private IList<pCentreEncour> _pCentreEncours = new List<pCentreEncour>();
		private IList<pCentreHisto> _pCentreHistos = new List<pCentreHisto>();
		
		#endregion

        #region Constructors

        public rRegroupementIndependant() { }

        #endregion

        #region Methods

        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            
            sb.Append(this.GetType().FullName);
			sb.Append(_libelleRegroupementInd);

            return sb.ToString().GetHashCode();
        }

        #endregion

        #region Properties

		public virtual string LibelleRegroupementInd
        {
            get { return _libelleRegroupementInd; }
			set
			{
				OnLibelleRegroupementIndChanging();
				_libelleRegroupementInd = value;
				OnLibelleRegroupementIndChanged();
			}
        }
		partial void OnLibelleRegroupementIndChanging();
		partial void OnLibelleRegroupementIndChanged();
		
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
