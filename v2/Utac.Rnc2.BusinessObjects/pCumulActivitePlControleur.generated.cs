using System;

namespace Utac.Rnc2.BusinessObjects
{
    public partial class pCumulActivitePlControleur : BusinessBase<string>
    {
        #region Declarations

        private string _codAgrem = String.Empty;
        private int _moisActivite = default(Int32);
        private int? _cumulActiviteQ1 = null;
        private int? _cumulActiviteQ2 = null;
        private int? _cumulActiviteQ3 = null;

        private pContoleurEncour _pContoleurEncour = null;

        #endregion

        #region Constructors

        public pCumulActivitePlControleur() { }

        #endregion

        #region Methods

        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append(this.GetType().FullName);
            sb.Append(_codAgrem);
            sb.Append(_moisActivite);
            sb.Append(_cumulActiviteQ1);
            sb.Append(_cumulActiviteQ2);
            sb.Append(_cumulActiviteQ3);

            return sb.ToString().GetHashCode();
        }

        #endregion

        #region Properties

        public override string Id
        {
            get
            {
                System.Text.StringBuilder uniqueId = new System.Text.StringBuilder();
                uniqueId.Append(_codAgrem.ToString());
                uniqueId.Append("^");
                uniqueId.Append(_moisActivite.ToString());
                return uniqueId.ToString();
            }
        }

        public virtual string CodAgrem
        {
            get { return _codAgrem; }
            set
            {
                OnCodAgremChanging();
                _codAgrem = value;
                OnCodAgremChanged();
            }
        }
        partial void OnCodAgremChanging();
        partial void OnCodAgremChanged();

        public virtual int MoisActivite
        {
            get { return _moisActivite; }
            set
            {
                OnMoisActiviteChanging();
                _moisActivite = value;
                OnMoisActiviteChanged();
            }
        }
        partial void OnMoisActiviteChanging();
        partial void OnMoisActiviteChanged();

        public virtual int? CumulActiviteQ1
        {
            get { return _cumulActiviteQ1; }
            set
            {
                OnCumulActiviteQ1Changing();
                _cumulActiviteQ1 = value;
                OnCumulActiviteQ1Changed();
            }
        }
        partial void OnCumulActiviteQ1Changing();
        partial void OnCumulActiviteQ1Changed();

        public virtual int? CumulActiviteQ2
        {
            get { return _cumulActiviteQ2; }
            set
            {
                OnCumulActiviteQ2Changing();
                _cumulActiviteQ2 = value;
                OnCumulActiviteQ2Changed();
            }
        }
        partial void OnCumulActiviteQ2Changing();
        partial void OnCumulActiviteQ2Changed();

        public virtual int? CumulActiviteQ3
        {
            get { return _cumulActiviteQ3; }
            set
            {
                OnCumulActiviteQ3Changing();
                _cumulActiviteQ3 = value;
                OnCumulActiviteQ3Changed();
            }
        }
        partial void OnCumulActiviteQ3Changing();
        partial void OnCumulActiviteQ3Changed();

        public virtual pContoleurEncour PContoleurEncour
        {
            get { return _pContoleurEncour; }
            set
            {
                OnPContoleurEncourChanging();
                _pContoleurEncour = value;
                OnPContoleurEncourChanged();
            }
        }
        partial void OnPContoleurEncourChanging();
        partial void OnPContoleurEncourChanged();

        #endregion
    }
}
