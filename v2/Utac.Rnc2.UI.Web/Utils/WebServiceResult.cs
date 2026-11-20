namespace Utac.Rnc2.UI.Web.Utils
{
    public class WebServiceResult
    {
        private Utac.Rnc2.UI.Web.Utils.PvCtWebUtil.EnumCodeRetourPVCT _resultCode;
        private System.Collections.IEnumerable _resultValue;

        internal Utac.Rnc2.UI.Web.Utils.PvCtWebUtil.EnumCodeRetourPVCT ResultCode
        {
            get { return _resultCode; }
            set { _resultCode = value; }
        }

        public System.Collections.IEnumerable ResultValue
        {
            get { return _resultValue; }
            set { _resultValue = value; }
        }

        public WebServiceResult()
        {
            this._resultCode = PvCtWebUtil.EnumCodeRetourPVCT.ERREUR_TECHNIQUE;
            this._resultValue = null;
        }

        internal WebServiceResult(PvCtWebUtil.EnumCodeRetourPVCT enumCodeRetourPVCT)
        {
            this._resultCode = enumCodeRetourPVCT;
            this._resultValue = null;
        }

        internal WebServiceResult(System.Collections.IEnumerable col, bool bPL)
        {
            if (col != null && col.GetEnumerator().MoveNext())
            {
                this._resultCode = bPL ? PvCtWebUtil.EnumCodeRetourPVCT.TROUVE_CT_PL : PvCtWebUtil.EnumCodeRetourPVCT.TROUVE_CT;
                this._resultValue = col;
            }
            else
            {
                this._resultCode = bPL ? PvCtWebUtil.EnumCodeRetourPVCT.AUCUN_CT_PL : PvCtWebUtil.EnumCodeRetourPVCT.AUCUN_CT;
                this._resultValue = null;
                //if(bPL)
                //    this._resultValue = new List<Utac.Rnc2.UI.Web.ServiceRnc2.PlPControle>();
                //else
                //    this._resultValue = new List<Utac.Rnc2.UI.Web.ServiceRnc2.VlPControle>();
            }
        }

    }
}