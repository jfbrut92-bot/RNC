using System;

namespace Utac.Rnc2.BusinessObjects
{
    public partial class pVersionMineure : BusinessBase<string>
    {
        #region Declarations

        private int _idConformiteLogiciel = default(Int32);
        private string _version = String.Empty;
        private System.DateTime _date = new DateTime();
        private string _commentaire = null;

        #endregion

        #region Constructors

        public pVersionMineure() { }

        #endregion

        #region Methods

        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append(this.GetType().FullName);
            sb.Append(_idConformiteLogiciel);
            sb.Append(_version);
            sb.Append(_date);
            sb.Append(_commentaire);

            return sb.ToString().GetHashCode();
        }

        #endregion

        #region Properties

        public override string Id
        {
            get
            {
                System.Text.StringBuilder uniqueId = new System.Text.StringBuilder();
                uniqueId.Append(_idConformiteLogiciel.ToString());
                uniqueId.Append("^");
                uniqueId.Append(_version.ToString());
                return uniqueId.ToString();
            }
        }

        public virtual int IdConformiteLogiciel
        {
            get { return _idConformiteLogiciel; }
            set
            {
                OnIdConformiteLogicielChanging();
                _idConformiteLogiciel = value;
                OnIdConformiteLogicielChanged();
            }
        }
        partial void OnIdConformiteLogicielChanging();
        partial void OnIdConformiteLogicielChanged();

        public virtual string Version
        {
            get { return _version; }
            set
            {
                OnVersionChanging();
                _version = value;
                OnVersionChanged();
            }
        }
        partial void OnVersionChanging();
        partial void OnVersionChanged();

        public virtual System.DateTime Date
        {
            get { return _date; }
            set
            {
                OnDateChanging();
                _date = value;
                OnDateChanged();
            }
        }
        partial void OnDateChanging();
        partial void OnDateChanged();

        public virtual string Commentaire
        {
            get { return _commentaire; }
            set
            {
                OnCommentaireChanging();
                _commentaire = value;
                OnCommentaireChanged();
            }
        }
        partial void OnCommentaireChanging();
        partial void OnCommentaireChanged();

        #endregion
    }
}
