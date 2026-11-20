using System.Collections.Generic;

namespace Utac.Rnc2.BusinessObjects
{
    public partial class rProfil : BusinessBase<int>
    {
        #region Declarations

        private string _libelleProfil = null;

        private IList<rEdition> _rEditions = new List<rEdition>();
        private IList<rProfil2> _rProfil2s = new List<rProfil2>();
        private IList<rUtilisateur> _rUtilisateurs = new List<rUtilisateur>();

        #endregion

        #region Constructors

        public rProfil() { }

        #endregion

        #region Methods

        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append(this.GetType().FullName);
            sb.Append(_libelleProfil);

            return sb.ToString().GetHashCode();
        }

        #endregion

        #region Properties

        public virtual string LibelleProfil
        {
            get { return _libelleProfil; }
            set
            {
                OnLibelleProfilChanging();
                _libelleProfil = value;
                OnLibelleProfilChanged();
            }
        }
        partial void OnLibelleProfilChanging();
        partial void OnLibelleProfilChanged();

        public virtual IList<rEdition> REditions
        {
            get { return _rEditions; }
            set
            {
                OnREditionsChanging();
                _rEditions = value;
                OnREditionsChanged();
            }
        }
        partial void OnREditionsChanging();
        partial void OnREditionsChanged();

        public virtual IList<rProfil2> RProfil2s
        {
            get { return _rProfil2s; }
            set
            {
                OnRProfil2sChanging();
                _rProfil2s = value;
                OnRProfil2sChanged();
            }
        }
        partial void OnRProfil2sChanging();
        partial void OnRProfil2sChanged();

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
