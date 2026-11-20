using System;
using System.Collections.Generic;

namespace Utac.Rnc2.BusinessObjects
{
    public partial class rCategorieVehicule : BusinessBase<short>
    {
        #region Declarations

        private string _codCategorie = String.Empty;
        private string _libelleCategorie = String.Empty;
        private short _numTri = default(Int16);

        private rHabilitation _rHabilitation = null;

        private IList<pAuditControleur> _pAuditControleurs = new List<pAuditControleur>();

        #endregion

        #region Constructors

        public rCategorieVehicule() { }

        #endregion

        #region Methods

        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append(this.GetType().FullName);
            sb.Append(_codCategorie);
            sb.Append(_libelleCategorie);
            sb.Append(_numTri);

            return sb.ToString().GetHashCode();
        }

        #endregion

        #region Properties

        public virtual string CodCategorie
        {
            get { return _codCategorie; }
            set
            {
                OnCodCategorieChanging();
                _codCategorie = value;
                OnCodCategorieChanged();
            }
        }
        partial void OnCodCategorieChanging();
        partial void OnCodCategorieChanged();

        public virtual string LibelleCategorie
        {
            get { return _libelleCategorie; }
            set
            {
                OnLibelleCategorieChanging();
                _libelleCategorie = value;
                OnLibelleCategorieChanged();
            }
        }
        partial void OnLibelleCategorieChanging();
        partial void OnLibelleCategorieChanged();

        public virtual short NumTri
        {
            get { return _numTri; }
            set
            {
                OnNumTriChanging();
                _numTri = value;
                OnNumTriChanged();
            }
        }
        partial void OnNumTriChanging();
        partial void OnNumTriChanged();

        public virtual rHabilitation RHabilitation
        {
            get { return _rHabilitation; }
            set
            {
                OnRHabilitationChanging();
                _rHabilitation = value;
                OnRHabilitationChanged();
            }
        }
        partial void OnRHabilitationChanging();
        partial void OnRHabilitationChanged();

        public virtual IList<pAuditControleur> PAuditControleurs
        {
            get { return _pAuditControleurs; }
            set
            {
                OnPAuditControleursChanging();
                _pAuditControleurs = value;
                OnPAuditControleursChanged();
            }
        }
        partial void OnPAuditControleursChanging();
        partial void OnPAuditControleursChanged();

        #endregion
    }
}
