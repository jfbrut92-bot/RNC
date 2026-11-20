// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Utac.Rnc2.UI.Web.Enums;
namespace Utac.Rnc2.Pv.UI.Web.Utils
{
    public class WebServiceResult
    {
        private PvCtWebUtil.EnumCodeRetourPVCT _resultCode;
        private System.Collections.IEnumerable _resultValue;

        internal PvCtWebUtil.EnumCodeRetourPVCT ResultCode
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

        internal WebServiceResult(System.Collections.IEnumerable col, CategorieVehicule cat)
        {
            if (col != null && col.GetEnumerator().MoveNext())
            {
                //this._resultCode = bPL ? PvCtWebUtil.EnumCodeRetourPVCT.TROUVE_CT_PL : PvCtWebUtil.EnumCodeRetourPVCT.TROUVE_CT;
                switch (cat.InnerEnumValue)
                {
                    case CategorieVehicule.InnerEnum.VL:
                        this._resultCode = PvCtWebUtil.EnumCodeRetourPVCT.TROUVE_CT;
                        break;
                    case CategorieVehicule.InnerEnum.PL:
                        this._resultCode = PvCtWebUtil.EnumCodeRetourPVCT.TROUVE_CT_PL;
                        break;
                    case CategorieVehicule.InnerEnum.CL:
                        this._resultCode = PvCtWebUtil.EnumCodeRetourPVCT.TROUVE_CT_CL;
                        break;
                }
                this._resultValue = col;
            }
            else
            {
                //this._resultCode = bPL ? PvCtWebUtil.EnumCodeRetourPVCT.AUCUN_CT_PL : PvCtWebUtil.EnumCodeRetourPVCT.AUCUN_CT;
                switch (cat.InnerEnumValue)
                {
                    case CategorieVehicule.InnerEnum.VL:
                        this._resultCode = PvCtWebUtil.EnumCodeRetourPVCT.AUCUN_CT;
                        break;
                    case CategorieVehicule.InnerEnum.PL:
                        this._resultCode = PvCtWebUtil.EnumCodeRetourPVCT.AUCUN_CT_PL;
                        break;
                    case CategorieVehicule.InnerEnum.CL:
                        this._resultCode = PvCtWebUtil.EnumCodeRetourPVCT.AUCUN_CT_CL;
                        break;
                }
                this._resultValue = null;

            }
        }

    }
}
