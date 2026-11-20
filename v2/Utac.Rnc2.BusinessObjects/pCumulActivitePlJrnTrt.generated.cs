using System;

namespace Utac.Rnc2.BusinessObjects
{
    public partial class pCumulActivitePlJrnTrt : BusinessBase<int>
    {
        #region Declarations

        private System.DateTime _dateDebutTraitement = new DateTime();
        private System.DateTime? _dateFinTraitement = null;

        #endregion

        #region Constructors

        public pCumulActivitePlJrnTrt() { }

        #endregion

        #region Methods

        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append(this.GetType().FullName);
            sb.Append(_dateDebutTraitement);
            sb.Append(_dateFinTraitement);

            return sb.ToString().GetHashCode();
        }

        #endregion

        #region Properties

        public virtual System.DateTime DateDebutTraitement
        {
            get { return _dateDebutTraitement; }
            set
            {
                OnDateDebutTraitementChanging();
                _dateDebutTraitement = value;
                OnDateDebutTraitementChanged();
            }
        }
        partial void OnDateDebutTraitementChanging();
        partial void OnDateDebutTraitementChanged();

        public virtual System.DateTime? DateFinTraitement
        {
            get { return _dateFinTraitement; }
            set
            {
                OnDateFinTraitementChanging();
                _dateFinTraitement = value;
                OnDateFinTraitementChanged();
            }
        }
        partial void OnDateFinTraitementChanging();
        partial void OnDateFinTraitementChanged();

        #endregion
    }
}
