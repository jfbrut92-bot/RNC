using System;

namespace Utac.Rnc2.BusinessObjects
{
    public partial class pEquipementCentreVL : BusinessBase<int>
    {
        protected override void validate()
        {
            /* Implementer ICI les r?gles de validation */
        }

        private bool _controleVehiculeGaz = default(Boolean);
        public virtual bool ControleVehiculeGaz
        {
            get { return _controleVehiculeGaz; }
            set
            {
                OnControleVehiculeGazChanging();
                _controleVehiculeGaz = value;
                OnControleVehiculeGazChanged();
            }
        }
        partial void OnControleVehiculeGazChanging();
        partial void OnControleVehiculeGazChanged();

        private bool _controleVehiculeTransmissionSpecifique = default(Boolean);
        public virtual bool ControleVehiculeTransmissionSpecifique
        {
            get { return _controleVehiculeTransmissionSpecifique; }
            set
            {
                OnControleVehiculeTransmissionSpecifiqueChanging();
                _controleVehiculeTransmissionSpecifique = value;
                OnControleVehiculeTransmissionSpecifiqueChanged();
            }
        }
        partial void OnControleVehiculeTransmissionSpecifiqueChanging();
        partial void OnControleVehiculeTransmissionSpecifiqueChanged();

        private bool _equipementRouleauFou = default(Boolean);
        public virtual bool EquipementRouleauFou
        {
            get { return _equipementRouleauFou; }
            set
            {
                OnEquipementRouleauFouChanging();
                _equipementRouleauFou = value;
                OnEquipementRouleauFouChanged();
            }
        }
        partial void OnEquipementRouleauFouChanging();
        partial void OnEquipementRouleauFouChanged();

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

        private bool _bancRotationInverse = default(Boolean);
        public virtual bool BancRotationInverse
        {
            get { return _bancRotationInverse; }
            set
            {
                OnBancRotationInverseChanging();
                _bancRotationInverse = value;
                OnBancRotationInverseChanged();
            }
        }
        partial void OnBancRotationInverseChanging();
        partial void OnBancRotationInverseChanged();

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
            sb.Append(this._controleVehiculeGaz);
            sb.Append(this._controleVehiculeTransmissionSpecifique);
            sb.Append(this._decelerometre);
            sb.Append(this._equipementRouleauFou);
            sb.Append(this._lieuStockagePV);

            return sb.ToString().GetHashCode();
        }
    }
}