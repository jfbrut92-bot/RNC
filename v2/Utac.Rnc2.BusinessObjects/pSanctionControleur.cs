namespace Utac.Rnc2.BusinessObjects
{
    public partial class pSanctionControleur : BusinessBase<int>
    {
        protected override void validate()
        {
            /* Implementer ICI les r?gles de validation */
        }

        /* EG le 14/09/2011 : Ajout */
        private string _codAgremCl;
        public virtual string CodAgremCl
        {
            get
            {
                return this._codAgremCl;
            }

            set
            {
                OnCodAgremClChanging();
                this._codAgremCl = value;
                OnCodAgremClChanged();
            }
        }
        partial void OnCodAgremClChanging();
        partial void OnCodAgremClChanged();

        /*<property name="IsAgrementAffecteVL" column="agrement_affecte_vl" />*/
        private bool _isAgrementAffecteVL;
        public virtual bool IsAgrementAffecteVL
        {
            get
            {
                return this._isAgrementAffecteVL;
            }

            set
            {
                OnIsAgrementAffecteVLChanging();
                this._isAgrementAffecteVL = value;
                OnIsAgrementAffecteVLChanged();
            }
        }
        partial void OnIsAgrementAffecteVLChanging();
        partial void OnIsAgrementAffecteVLChanged();

        /*
    <property name="IsAgrementAffectePL" column="agrement_affecte_pl" />
         */
        private bool _isAgrementAffectePL;
        public virtual bool IsAgrementAffectePL
        {
            get
            {
                return this._isAgrementAffectePL;
            }

            set
            {
                OnIsAgrementAffectePLChanging();
                this._isAgrementAffectePL = value;
                OnIsAgrementAffectePLChanged();
            }
        }
        partial void OnIsAgrementAffectePLChanging();
        partial void OnIsAgrementAffectePLChanged();

        /*
       <property name="IsAgrementAffecteCL" column="agrement_affecte_cl" />
           */
        private bool _isAgrementAffecteCL;
        public virtual bool IsAgrementAffecteCL
        {
            get
            {
                return this._isAgrementAffecteCL;
            }

            set
            {
                OnIsAgrementAffecteCLChanging();
                this._isAgrementAffecteCL = value;
                OnIsAgrementAffecteCLChanged();
            }
        }
        partial void OnIsAgrementAffecteCLChanging();
        partial void OnIsAgrementAffecteCLChanged();

        //Fin Ajout du 14/09/2011
    }
}