using System.Collections.Generic;
using System;

namespace Utac.Rnc2.BusinessObjects
{
    public partial class rTypeFormation : BusinessBase<short>
    {
        #region Declarations

        private string _libelleTypeFormation = null;

        private rHabilitation _rHabilitation = null;

        private IList<pFormationControleurPL> _pFormationControleurs = new List<pFormationControleurPL>();

        //PLE le 26/1/2024 Extension Catégorie L
        private string _codCategorieVeh = String.Empty;

        #endregion

        #region Constructors

        public rTypeFormation() { }

        #endregion

        #region Methods

        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append(this.GetType().FullName);
            sb.Append(_libelleTypeFormation);

            return sb.ToString().GetHashCode();
        }

        #endregion

        #region Properties

        public virtual string LibelleTypeFormation
        {
            get { return _libelleTypeFormation; }
            set
            {
                OnLibelleTypeFormationChanging();
                _libelleTypeFormation = value;
                OnLibelleTypeFormationChanged();
            }
        }
        partial void OnLibelleTypeFormationChanging();
        partial void OnLibelleTypeFormationChanged();

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

        public virtual IList<pFormationControleurPL> PFormationControleurs
        {
            get { return _pFormationControleurs; }
            set
            {
                OnPFormationControleursChanging();
                _pFormationControleurs = value;
                OnPFormationControleursChanged();
            }
        }
        partial void OnPFormationControleursChanging();
        partial void OnPFormationControleursChanged();


        //PLE le 26/1/2024 Extension Catégorie L
        public virtual string CodCategorieVeh
        {
            get { return _codCategorieVeh; }
            set
            {
                OnCodCategorieVehChanging();
                _codCategorieVeh = value;
                OnCodCategorieVehChanged();
            }
        }

        partial void OnCodCategorieVehChanging();
        partial void OnCodCategorieVehChanged();
        #endregion
    }
}
