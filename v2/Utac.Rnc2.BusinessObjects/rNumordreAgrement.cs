namespace Utac.Rnc2.BusinessObjects
{
    public partial class rNumordreAgrement : BusinessBase<int>
    {
        #region ajoutée car non existence de clé étrangère
        /* eddy car non détectée par codesmith car absence de cle étrangère */
        private rPrefecture _rPrefecture;

        partial void OnRPrefectureChanging();
        partial void OnRPrefectureChanged();

        public virtual rPrefecture RPrefecture
        {
            get { return _rPrefecture; }
            set
            {
                OnRPrefectureChanging();
                _rPrefecture = value;
                OnRPrefectureChanged();
            }
        }
        #endregion

        //EG le 19/07/2011 pour prise en compte CL
        private int _numOrdreCntlrCl;

        public virtual int NumOrdreCntlrCl
        {
            get { return _numOrdreCntlrCl; }
            set
            {
                OnNumOrdreCntlrClChanging();
                _numOrdreCntlrCl = value;
                OnNumOrdreCntlrClChanged();
            }
        }
        partial void OnNumOrdreCntlrClChanging();
        partial void OnNumOrdreCntlrClChanged();

        protected override void validate()
        {
            /* Implementer ICI les r?gles de validation */
        }

    }
}