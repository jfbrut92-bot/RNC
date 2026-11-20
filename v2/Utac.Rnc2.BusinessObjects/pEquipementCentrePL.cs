using System;

namespace Utac.Rnc2.BusinessObjects
{
    public partial class pEquipementCentrePL : BusinessBase<int>
    {
        protected override void validate()
        {
            /* Implementer ICI les r?gles de validation */
        }

        private bool _controleTCP = default(Boolean);
        public virtual bool ControleTCP
        {
            get { return _controleTCP; }
            set
            {
                OnControleTCPChanging();
                _controleTCP = value;
                OnControleTCPChanged();
            }
        }
        partial void OnControleTCPChanging();
        partial void OnControleTCPChanged();

        private bool _controleTMD = default(Boolean);
        public virtual bool ControleTMD
        {
            get { return _controleTMD; }
            set
            {
                OnControleTMDChanging();
                _controleTMD = value;
                OnControleTMDChanged();
            }
        }
        partial void OnControleTMDChanging();
        partial void OnControleTMDChanged();

        private bool _decelerometre = default(Boolean);
        public virtual bool Decelerometre
        {
            get { return _decelerometre; }
            set
            {
                OnDecelerometreChanging();
                _decelerometre = value;
                OnDecelerometreChanged();
            }
        }
        partial void OnDecelerometreChanging();
        partial void OnDecelerometreChanged();

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
            sb.Append(this._controleTCP);
            sb.Append(this._controleTMD);
            sb.Append(this._decelerometre);
            sb.Append(this._lieuStockagePV);

            return sb.ToString().GetHashCode();
        }
    }
}