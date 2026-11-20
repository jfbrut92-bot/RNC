using System;
using System.Collections.Generic;

namespace Utac.Rnc2.BusinessObjects
{
    public partial class rEdition : BusinessBase<int>
    {
        #region Declarations

        private string _libEdition = String.Empty;
        private string _nomActionMvc = String.Empty;

        private IList<rProfil> _rProfils = new List<rProfil>();

        #endregion

        #region Constructors

        public rEdition() { }

        #endregion

        #region Methods

        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append(this.GetType().FullName);
            sb.Append(_libEdition);
            sb.Append(_nomActionMvc);

            return sb.ToString().GetHashCode();
        }

        #endregion

        #region Properties

        public virtual string LibEdition
        {
            get { return _libEdition; }
            set
            {
                OnLibEditionChanging();
                _libEdition = value;
                OnLibEditionChanged();
            }
        }
        partial void OnLibEditionChanging();
        partial void OnLibEditionChanged();

        public virtual string NomActionMvc
        {
            get { return _nomActionMvc; }
            set
            {
                OnNomEtatPbChanging();
                _nomActionMvc = value;
                OnNomEtatPbChanged();
            }
        }
        partial void OnNomEtatPbChanging();
        partial void OnNomEtatPbChanged();

        public virtual IList<rProfil> RProfils
        {
            get { return _rProfils; }
            set
            {
                OnRProfilsChanging();
                _rProfils = value;
                OnRProfilsChanged();
            }
        }
        partial void OnRProfilsChanging();
        partial void OnRProfilsChanged();

        #endregion
    }
}
