using System;
using System.ComponentModel.DataAnnotations;

using Utac.Rnc2.UI.Web.Enums;
using Utac.Rnc2.UI.Web.Models;
using Utac.Rnc2.UI.Web.Utils;

namespace Utac.Rnc2.Pv.UI.Web.Models
{
    public class RechercheImmatSerieModel : RecherchePvModel
    {
        //private static readonly string _pwd = "Utac.Otc.Rnc2";
        //private static readonly DateTime _ctrl2018 = new DateTime(2018, 5, 20);

        //public string CategorieValue { get; set; }
        //public string DetailValue { get; set; }

        //private CategorieVehicule categorieVehicule;
        //private NiveauDetails niveauDetails;

        public RechercheImmatSerieModel() : base()
        {
            //// TODO: Complete member initialization
            //this.categorieVehicule = CategorieVehicule.VL;
            //this.niveauDetails = NiveauDetails.Summary;
        }

        public RechercheImmatSerieModel(CategorieVehicule categorieVehicule, NiveauDetails niveauDetails, DateTime datelimit) : base(categorieVehicule, niveauDetails)
        {
            //// TODO: Complete member initialization
            //this.categorieVehicule = categorieVehicule;
            //this.niveauDetails = niveauDetails;
            this.DateLimit = datelimit;
        }
        /// <summary>
        /// 
        /// </summary>
        /// PLE le 12/06/2024 : Suppression de RegularExpression
        //[RegularExpression(@"\A" + StringValidator.SivPattern + @"|" + StringValidator.FniPattern +@"|" + StringValidator.FniCatgorieLPattern +@"\Z", ErrorMessage = "Non respect du format AA-111-AA ou 1111 AAA 11")]
        //// AA-111-AA
        //@"(([a-hj-np-tvx-zA-HJ-NP-TVX-Z][a-hj-np-rtv-zA-HJ-NP-RTV-Z]|[a-hj-np-rtv-zA-HJ-NP-RTV-Z][a-hj-np-tvx-zA-HJ-NP-TVX-Z])-[0-9]{3}-" + 
        //@"([a-hj-np-tv-zA-HJ-NP-TV-Z][a-hj-np-rtv-zA-HJ-NP-RTV-Z]|[a-hj-np-rtv-zA-HJ-NP-RTV-Z][a-hj-np-tv-zA-HJ-NP-TV-Z]))" + 
        //// W-001-AA ; WW-001-AA
        //@"|(([wW]?[wW])-[0-9]{3}-([a-hj-np-tv-zA-HJ-NP-TV-Z][a-hj-np-rtv-zA-HJ-NP-RTV-Z]|[a-hj-np-rtv-zA-HJ-NP-RTV-Z][a-hj-np-tv-zA-HJ-NP-TV-Z]))" + 
        //// 1111 AAA 11 ; 1111 AAA 976 ; 1111 AAA 2A ...
        //@"|([0-9]{1,4}(?:\s|-)?[a-hj-np-tv-zA-HJ-NP-TV-Z]{2,3}(?:\s|-)?(?:97[1-6]|0[1-9]|[1-8][0-9]|9[1-5]|2[abAB]))" + 
        //// 1111 W 11 ; 1111 WW 11 ; 1111 W 976 ; 1111 WW 976 ; 1111 W 2A ; 1111 WW 2A
        //@"|([0-9]{1,4}(?:\s|-)?[wW]{1,2}(?:\s|-)?(?:97[1-6]|0[1-9]|[1-8][0-9]|9[1-5]|2[abAB]))\Z", 
        //ErrorMessage = "Non respect du format AA-111-AA ou 1111 AAA 11")]
        public string Immatriculation { get; set; }
        public string NumberPlate
        {
            get
            {
                return !String.IsNullOrEmpty(Immatriculation) ? Immatriculation.ToUpper() : "";
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [RegularExpression(@"\A" + StringValidator.VinPattern + @"\Z", ErrorMessage = "VIN incorrect")]
        public string NumeroSerie { get; set; }
        public string VinNumber
        {
            get
            {
                return !String.IsNullOrEmpty(NumeroSerie) ? NumeroSerie.ToUpper() : "";
            }
        }
        public DateTime DateLimit { get; set; }
        ///// <summary>
        ///// 
        ///// </summary>
        //public CategorieVehicule Categorie { get { return categorieVehicule; } set { categorieVehicule = value; } }
        ///// <summary>
        ///// 
        ///// </summary>
        //public NiveauDetails Details { get { return niveauDetails; } set { niveauDetails = value; } }
        ///// <summary>
        ///// Collection des résultats des interrogations VL
        ///// </summary>
        //public IList<VlPControle> ControlesVL { get; set; }
        ///// <summary>
        ///// Collection des résultats des interrogations PL
        ///// </summary>
        //public IList<PlPControle> ControlesPL { get; set; }
        ///// <summary>
        ///// 
        ///// </summary>
        //public void ResetLists()
        //{
        //    if (ControlesVL != null)
        //    {
        //        ControlesVL.Clear();
        //        ControlesVL = null;
        //    }
        //    if (ControlesPL != null)
        //    {
        //        ControlesPL.Clear();
        //        ControlesPL = null;
        //    }
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="p"></param>
        ///// <returns></returns>
        //public static string EncodeParams(string p)
        //{
        //    if (!String.IsNullOrEmpty(p))return StringCipher.Encrypt(p, _pwd);
        //    return "";
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="ep"></param>
        ///// <returns></returns>
        //public static NameValueCollection DecodeParams(string ep)
        //{
        //    string p = StringCipher.Decrypt(ep, _pwd);
        //    if(!String.IsNullOrEmpty(p))return HttpUtility.ParseQueryString(p);
        //    return null;
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="c"></param>
        ///// <returns></returns>
        //public static string GetNatureControlLabel(VlPControle c)
        //{
        //    if (c != null && c.RNatureControle != null)
        //    {
        //        if (c.dateCtrl < _ctrl2018 && !String.IsNullOrEmpty(c.RNatureControle.libNatureCtrl))
        //        {
        //            return c.RNatureControle.libNatureCtrl;
        //        }
        //        else if (!String.IsNullOrEmpty(c.RNatureControle.libNatureCtrl2018))
        //        {
        //            return c.RNatureControle.libNatureCtrl2018;
        //        }
        //    }
        //    return "";
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="c"></param>
        ///// <returns></returns>
        //public static string GetResultControlLabel(VlPControle c)
        //{
        //    if (c != null && c.RResultCtrl != null)
        //    {
        //        if (c.dateCtrl < _ctrl2018 && !String.IsNullOrEmpty(c.RResultCtrl.libResultCtrl))
        //        {
        //            return c.RResultCtrl.libResultCtrl;
        //        }
        //        else if(!String.IsNullOrEmpty(c.RResultCtrl.libResultCtrl2018))
        //        {
        //            return c.RResultCtrl.libResultCtrl2018;
        //        }
        //    }
        //    return "";
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="c"></param>
        ///// <returns></returns>
        //public static string GetNatureControlLabel(PlPControle c)
        //{
        //    if (c != null && c.RNatureControle != null)
        //    {
        //        if (c.dateCtrl < _ctrl2018 && !String.IsNullOrEmpty(c.RNatureControle.libNatureCtrl))
        //        {
        //            return c.RNatureControle.libNatureCtrl;
        //        }
        //        else if (!String.IsNullOrEmpty(c.RNatureControle.libNatureCtrl2018))
        //        {
        //            return c.RNatureControle.libNatureCtrl2018;
        //        }
        //    }
        //    return "";
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="c"></param>
        ///// <returns></returns>
        //public static string GetResultControlLabel(PlPControle c)
        //{
        //    if (c != null && c.RResultCtrl != null)
        //    {
        //        if (c.dateCtrl < _ctrl2018 && !String.IsNullOrEmpty(c.RResultCtrl.libResultCtrl))
        //        {
        //            return c.RResultCtrl.libResultCtrl;
        //        }
        //        else if(!String.IsNullOrEmpty(c.RResultCtrl.libResultCtrl2018))
        //        {
        //            return c.RResultCtrl.libResultCtrl2018;
        //        }
        //    }
        //    return "";
        //}
    }
}
