using System;

namespace Utac.Rnc2.BusinessObjects
{
    public partial class pEquipementCentreCL : BusinessBase<int>
    {
        protected override void validate()
        {
            /* Implementer ICI les r?gles de validation */
        }

        private bool _controleTricyles = default(Boolean);
        public virtual bool ControleTricyles
        {
            get { return _controleTricyles; }
            set
            {
                OnControleTricylesChanging();
                _controleTricyles = value;
                OnControleTricylesChanged();
            }
        }
        partial void OnControleTricylesChanging();
        partial void OnControleTricylesChanged();

        private string _lieuStockagePV = null;
        public virtual string LieuStockagePV
        {
            get { return _lieuStockagePV; }
            set
            {
                OnLieuStockagePVChanging();
                _lieuStockagePV = value;
                OnLieuStockagePVChanged();
            }
        }
        partial void OnLieuStockagePVChanging();
        partial void OnLieuStockagePVChanged();

        private pCentreEncour _pCentreEnCour = null;
        public virtual pCentreEncour PCentreEnCour
        {
            get { return _pCentreEnCour; }
            set
            {
                OnPCentreEnCourChanging();
                _pCentreEnCour = value;
                OnPCentreEnCourChanged();
            }
        }
        partial void OnPCentreEnCourChanging();
        partial void OnPCentreEnCourChanged();

        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append(this.GetType().FullName);
            sb.Append(this._controleTricyles);
            sb.Append(this._lieuStockagePV);

            return sb.ToString().GetHashCode();
        }
    }
}