namespace Utac.Rnc2.BusinessObjects
{
    public partial class rTemplateMail : BusinessBase<int>
    {
        #region propriétés privées
        private string _nomTemplate = string.Empty;
        private string _body = string.Empty;
        private string _sujet = string.Empty;
        #endregion

        #region Constructors

        public rTemplateMail() { }

        #endregion

        #region Methods

        protected override void validate()
        {
            /* Implementer ICI les r?gles de validation */
        }

        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append(this.GetType().FullName);
            sb.Append(_nomTemplate);
            sb.Append(_body);
            sb.Append(_sujet);
            return sb.ToString().GetHashCode();
        }
        #endregion

        #region propriétés publiques
        public virtual string NomTemplate
        {
            get { return this._nomTemplate; }
            set
            {
                OnNomTemplateChanging();
                _nomTemplate = value;
                OnNomTemplateChanged();
            }
        }
        partial void OnNomTemplateChanging();
        partial void OnNomTemplateChanged();

        public virtual string BodyTemplate
        {
            get { return this._body; }
            set
            {
                OnBodyTemplateChanging();
                _body = value;
                OnBodyTemplateChanged();
            }
        }
        partial void OnBodyTemplateChanging();
        partial void OnBodyTemplateChanged();

        public virtual string SujetTemplate
        {
            get { return this._sujet; }
            set
            {
                OnSujetTemplateChanging();
                _sujet = value;
                OnSujetTemplateChanged();
            }
        }
        partial void OnSujetTemplateChanging();
        partial void OnSujetTemplateChanged();

        #endregion

        #region enum
        public enum TemplateEnum
        {
            NOUVEL_UTILISATEUR,
            NOUVEAU_PASSWORD,
            NOUVEAU_EMAIL,
            INSCRIPTION_CENTRE_INDEPENDANT,
            PREMIER_CERTIFICAT,
            COPIE_CERTIFICAT,
            RENOUVELLEMENT_CERTIFICAT
        }
        #endregion
    }
}
