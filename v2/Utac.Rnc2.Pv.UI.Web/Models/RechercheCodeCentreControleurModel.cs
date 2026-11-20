using System;
using System.ComponentModel.DataAnnotations;
using Utac.Rnc2.UI.Web.Enums;
using Utac.Rnc2.UI.Web.Utils;

namespace Utac.Rnc2.Pv.UI.Web.Models
{
    public class RechercheCodeCentreControleurModel : RecherchePvModel
    {
        //private static readonly string _pwd = "Utac.Otc.Rnc2";
        //private static readonly DateTime _ctrl2018 = new DateTime(2018, 5, 20);

        //public string CategorieValue { get; set; }
        //public string DetailValue { get; set; }

        //private CategorieVehicule categorieVehicule;
        //private NiveauDetails niveauDetails;

        public RechercheCodeCentreControleurModel() : base()
        {
            //// TODO: Complete member initialization
            //this.categorieVehicule = CategorieVehicule.VL;
            //this.niveauDetails = NiveauDetails.Summary;
        }

        public RechercheCodeCentreControleurModel(CategorieVehicule categorieVehicule, NiveauDetails niveauDetails)
            : base(categorieVehicule, niveauDetails)
        {
            //// TODO: Complete member initialization
            //this.categorieVehicule = categorieVehicule;
            //this.niveauDetails = niveauDetails;
            this.DateVisiteFin = DateTime.Today;
            this.DateVisiteDebut = this.DateVisiteFin.AddDays(-1);
        }
        /// <summary>
        /// 
        /// </summary>
        //[RegularExpression(@"\A(" + StringValidator.CodeAgrementCentrePlPattern + @"|" + StringValidator.CodeAgrementCentreVlPattern + @")\Z", ErrorMessage = "Numéro d'agrément centre erronné")]
        public string CodeAgrementCentre { get; set; }
        public string CenterApprovalCode
        {
            get
            {
                return !String.IsNullOrEmpty(CodeAgrementCentre) ? CodeAgrementCentre.ToUpper() : "";
            }
        }
        /// <summary>
        /// 
        /// </summary>
        //[RegularExpression(@"\A(" + StringValidator.CodeAgrementControleurPlPattern + @"|" + StringValidator.CodeAgrementControleurVlPattern + @")\Z", ErrorMessage = "Numéro d'agrément contrôleur erronné")]
        public string CodeAgrementControleur { get; set; }
        public string ControllerApprovalCode
        {
            get
            {
                return !String.IsNullOrEmpty(CodeAgrementControleur) ? CodeAgrementControleur.ToUpper() : "";
            }
        }
        [Required(ErrorMessage = "Obligatoire")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:DD/MM/YYYY}", ApplyFormatInEditMode = true)]
        [RegularExpression(@"[0-3][0-9]/(01|02|03|04|05|06|07|08|09|10|11|12)/[1-2][09][0-9][0-9](.*)", ErrorMessage = "La date doit respecter le format JJ/MM/AAAA")]
        [Display(Name = "Date")]
        public System.DateTime DateVisiteDebut { get; set; }
        [Required(ErrorMessage = "Obligatoire")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:DD/MM/YYYY}", ApplyFormatInEditMode = true)]
        [RegularExpression(@"[0-3][0-9]/(01|02|03|04|05|06|07|08|09|10|11|12)/[1-2][09][0-9][0-9](.*)", ErrorMessage = "La date doit respecter le format JJ/MM/AAAA")]
        [Display(Name = "Date")]
        public System.DateTime DateVisiteFin { get; set; }
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
