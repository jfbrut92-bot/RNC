using System;
using System.Collections.Generic;

namespace Utac.Rnc2.BusinessObjects
{
    public partial class rActionCertificat : BusinessBase<short>
    {
        #region Declarations

        private string _libAction = null;
        private bool _adminAutorise = default(Boolean);
        private bool _operateurAutorise = default(Boolean);

        private IList<pGestionCertificat> _pGestionCertificats = new List<pGestionCertificat>();

        #endregion

        #region Constructors

        public rActionCertificat() { }

        #endregion

        #region Methods

        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append(this.GetType().FullName);
            sb.Append(_libAction);
            sb.Append(_adminAutorise);
            sb.Append(_operateurAutorise);

            return sb.ToString().GetHashCode();
        }

        #endregion

        #region Properties

        public virtual string LibAction
        {
            get { return _libAction; }
            set
            {
                OnLibActionChanging();
                _libAction = value;
                OnLibActionChanged();
            }
        }
        partial void OnLibActionChanging();
        partial void OnLibActionChanged();

        public virtual bool AdminAutorise
        {
            get { return _adminAutorise; }
            set
            {
                OnAdminAutoriseChanging();
                _adminAutorise = value;
                OnAdminAutoriseChanged();
            }
        }
        partial void OnAdminAutoriseChanging();
        partial void OnAdminAutoriseChanged();

        public virtual bool OperateurAutorise
        {
            get { return _operateurAutorise; }
            set
            {
                OnOperateurAutoriseChanging();
                _operateurAutorise = value;
                OnOperateurAutoriseChanged();
            }
        }
        partial void OnOperateurAutoriseChanging();
        partial void OnOperateurAutoriseChanged();

        public virtual IList<pGestionCertificat> PGestionCertificats
        {
            get { return _pGestionCertificats; }
            set
            {
                OnPGestionCertificatsChanging();
                _pGestionCertificats = value;
                OnPGestionCertificatsChanged();
            }
        }
        partial void OnPGestionCertificatsChanging();
        partial void OnPGestionCertificatsChanged();

        #endregion
    }
}
