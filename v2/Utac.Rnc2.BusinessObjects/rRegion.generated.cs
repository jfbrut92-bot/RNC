using System.Collections.Generic;

namespace Utac.Rnc2.BusinessObjects
{
    public partial class rRegion : BusinessBase<int>
    {
        #region Declarations

		private string _libelleRegion = null;

		private IList<rPrefecture> _rPrefectures = new List<rPrefecture>();
		private IList<rUtilisateur> _rUtilisateurs = new List<rUtilisateur>();
		
		#endregion

        #region Constructors

        public rRegion() { }

        #endregion

        #region Methods

        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            
            sb.Append(this.GetType().FullName);
			sb.Append(_libelleRegion);

            return sb.ToString().GetHashCode();
        }

        #endregion

        #region Properties

		public virtual string LibelleRegion
        {
            get { return _libelleRegion; }
			set
			{
				OnLibelleRegionChanging();
				_libelleRegion = value;
				OnLibelleRegionChanged();
			}
        }
		partial void OnLibelleRegionChanging();
		partial void OnLibelleRegionChanged();
		
		public virtual IList<rPrefecture> RPrefectures
        {
            get { return _rPrefectures; }
            set
			{
				OnRPrefecturesChanging();
				_rPrefectures = value;
				OnRPrefecturesChanged();
			}
        }
		partial void OnRPrefecturesChanging();
		partial void OnRPrefecturesChanged();
		
		public virtual IList<rUtilisateur> RUtilisateurs
        {
            get { return _rUtilisateurs; }
            set
			{
				OnRUtilisateursChanging();
				_rUtilisateurs = value;
				OnRUtilisateursChanged();
			}
        }
		partial void OnRUtilisateursChanging();
		partial void OnRUtilisateursChanged();
		
        #endregion
    }
}
