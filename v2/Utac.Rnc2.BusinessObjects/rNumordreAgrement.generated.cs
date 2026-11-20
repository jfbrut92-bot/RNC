using System;

namespace Utac.Rnc2.BusinessObjects
{
    public partial class rNumordreAgrement : BusinessBase<int>
    {
        #region Declarations

        private int _numOrdreCentre = default(Int32);
        private int _numOrdreCntlrVl = default(Int32);
        private int _numOrdreCntlrPl = default(Int32);

        #endregion

        #region Constructors

        public rNumordreAgrement() { }

        #endregion

        #region Methods

        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append(this.GetType().FullName);
            sb.Append(_numOrdreCentre);
            sb.Append(_numOrdreCntlrVl);
            sb.Append(_numOrdreCntlrPl);

            return sb.ToString().GetHashCode();
        }

        #endregion

        #region Properties

        public virtual int NumOrdreCentre
        {
            get { return _numOrdreCentre; }
            set
            {
                OnNumOrdreCentreChanging();
                _numOrdreCentre = value;
                OnNumOrdreCentreChanged();
            }
        }
        partial void OnNumOrdreCentreChanging();
        partial void OnNumOrdreCentreChanged();

        public virtual int NumOrdreCntlrVl
        {
            get { return _numOrdreCntlrVl; }
            set
            {
                OnNumOrdreCntlrVlChanging();
                _numOrdreCntlrVl = value;
                OnNumOrdreCntlrVlChanged();
            }
        }
        partial void OnNumOrdreCntlrVlChanging();
        partial void OnNumOrdreCntlrVlChanged();

        public virtual int NumOrdreCntlrPl
        {
            get { return _numOrdreCntlrPl; }
            set
            {
                OnNumOrdreCntlrPlChanging();
                _numOrdreCntlrPl = value;
                OnNumOrdreCntlrPlChanged();
            }
        }
        partial void OnNumOrdreCntlrPlChanging();
        partial void OnNumOrdreCntlrPlChanged();

        #endregion
    }
}
