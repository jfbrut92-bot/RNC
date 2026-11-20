using System;
using System.Collections.Generic;

namespace Utac.Rnc2.BusinessObjects
{
    public partial class rPrefecture : BusinessBase<int>
    {
        #region Declarations

        private string _codDepartement = String.Empty;
        private string _libellePrefecture = null;
        private string _prefixe = null;
        private string _libTribuAdmin = null;

        private rRegion _rRegion = null;

        private IList<pCentreEncour> _pCentreEncours = new List<pCentreEncour>();
        private IList<pCentreHisto> _pCentreHistos = new List<pCentreHisto>();
        private IList<pContoleurEncour> _pContoleurEncours = new List<pContoleurEncour>();
        private IList<pControleurHisto> _pControleurHistos = new List<pControleurHisto>();
        private IList<rUtilisateur> _rUtilisateurs = new List<rUtilisateur>();

        #endregion

        #region Constructors

        public rPrefecture() { }

        #endregion

        #region Methods

        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append(this.GetType().FullName);
            sb.Append(_codDepartement);
            sb.Append(_libellePrefecture);
            sb.Append(_prefixe);
            sb.Append(_libTribuAdmin);

            return sb.ToString().GetHashCode();
        }

        #endregion

        #region Properties

        public virtual string CodDepartement
        {
            get { return _codDepartement; }
            set
            {
                OnCodDepartementChanging();
                _codDepartement = value;
                OnCodDepartementChanged();
            }
        }
        partial void OnCodDepartementChanging();
        partial void OnCodDepartementChanged();

        public virtual string LibellePrefecture
        {
            get { return _libellePrefecture; }
            set
            {
                OnLibellePrefectureChanging();
                _libellePrefecture = value;
                OnLibellePrefectureChanged();
            }
        }
        partial void OnLibellePrefectureChanging();
        partial void OnLibellePrefectureChanged();

        public virtual string Prefixe
        {
            get { return _prefixe; }
            set
            {
                OnPrefixeChanging();
                _prefixe = value;
                OnPrefixeChanged();
            }
        }
        partial void OnPrefixeChanging();
        partial void OnPrefixeChanged();

        public virtual string LibTribuAdmin
        {
            get { return _libTribuAdmin; }
            set
            {
                OnLibTribuAdminChanging();
                _libTribuAdmin = value;
                OnLibTribuAdminChanged();
            }
        }
        partial void OnLibTribuAdminChanging();
        partial void OnLibTribuAdminChanged();

        public virtual rRegion RRegion
        {
            get { return _rRegion; }
            set
            {
                OnRRegionChanging();
                _rRegion = value;
                OnRRegionChanged();
            }
        }
        partial void OnRRegionChanging();
        partial void OnRRegionChanged();

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
