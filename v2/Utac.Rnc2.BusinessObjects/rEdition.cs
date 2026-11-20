using System;

namespace Utac.Rnc2.BusinessObjects
{
    public partial class rEdition : BusinessBase<int>
    {
        protected override void validate()
        {
            /* Implementer ICI les r?gles de validation */
        }

        private string _criteres = String.Empty;

        public virtual string Criteres
        {
            get { return _criteres; }
            set
            {
                OnCriteresChanging();
                _criteres = value;
                OnCriteresChanged();
            }
        }

        partial void OnCriteresChanging();
        partial void OnCriteresChanged();

        private bool _estActif = default(bool);

        public virtual bool EstActif
        {
            get { return _estActif; }
            set
            {
                OnEstActifChanging();
                _estActif = value;
                OnEstActifChanged();
            }
        }

        partial void OnEstActifChanging();
        partial void OnEstActifChanged();

    }
}