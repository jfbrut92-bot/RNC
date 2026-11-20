namespace Utac.Rnc2.BusinessObjects
{
    public partial class rPrefecture : BusinessBase<int>
    {
        protected override void validate()
        {
            /* Implementer ICI les r?gles de validation */
        }

        //EG le 19/10/2011
        private rNumordreAgrement _rNumordreAgrement = null;
        public virtual rNumordreAgrement RNumordreAgrement
        {
            get
            {
                return _rNumordreAgrement;
            }

            set
            {
                OnRNumordreAgrementChanging();
                _rNumordreAgrement = value;
                OnRNumordreAgrementChanged();
            }
        }

        partial void OnRNumordreAgrementChanging();
        partial void OnRNumordreAgrementChanged();

        public virtual string PrefixeSansSlashe
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Prefixe)) return null;
                return this.Prefixe.Replace("/", "");
            }

        }

    }
}