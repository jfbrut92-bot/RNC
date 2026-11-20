using System;
using System.Collections.Generic;

namespace Utac.Rnc2.BusinessObjects
{
    public partial class rProfil2 : BusinessBase<int>
    {
        #region Declarations

        private string _libelleProfil = String.Empty;

        private rProfil _rProfil = null;

        private IList<rUtilisateur> _rUtilisateurs = new List<rUtilisateur>();

        #endregion

        #region Constructors

        public rProfil2() { }

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

        public virtual rProfil RProfil
        {
            get { return _rProfil; }
            set
            {
                OnRProfilChanging();
                _rProfil = value;
                OnRProfilChanged();
            }
        }
        partial void OnRProfilChanging();
        partial void OnRProfilChanged();

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
