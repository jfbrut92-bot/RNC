using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Web;
using Utac.Rnc2.UI.Web.Enums;
using Utac.Rnc2.UI.Web.ServiceRnc2;
using Utac.Rnc2.UI.Web.Utils;

namespace Utac.Rnc2.Pv.UI.Web.Models
{
    public class RecherchePvModel
    {
        private static readonly string _pwd = Pwd;
        private static readonly string _sasPwd = SasPwd;
        private static readonly DateTime _ctrl2018 = new DateTime(2018, 5, 20);

        public string CategorieValue { get; set; }
        public string DetailValue { get; set; }

        private CategorieVehicule categorieVehicule;
        private NiveauDetails niveauDetails;

        private static string Pwd
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["recherche_pv_model_pwd"];
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return "Utac.Otc.Rnc2";
                }
            }
        }

        private static string SasPwd
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["recherche_pv_model_sas_pwd"];
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return "-----BEGIN RSA PRIVATE KEY-----";
                }
            }
        }

        public RecherchePvModel()
        {
            // TODO: Complete member initialization
            this.categorieVehicule = CategorieVehicule.VL;
            this.niveauDetails = NiveauDetails.Summary;
        }

        public RecherchePvModel(CategorieVehicule categorieVehicule, NiveauDetails niveauDetails)
        {
            // TODO: Complete member initialization
            this.categorieVehicule = categorieVehicule;
            this.niveauDetails = niveauDetails;
        }
        /// <summary>
        /// 
        /// </summary>
        public CategorieVehicule Categorie { get { return categorieVehicule; } set { categorieVehicule = value; } }
        /// <summary>
        /// 
        /// </summary>
        public NiveauDetails Details { get { return niveauDetails; } set { niveauDetails = value; } }
        /// <summary>
        /// Collection des résultats des interrogations VL
        /// </summary>
        public IList<VlPControle> ControlesVL { get; set; }
        /// <summary>
        /// Collection des résultats des interrogations PL
        /// </summary>
        public IList<PlPControle> ControlesPL { get; set; }
        /// <summary>
        /// Collection des résultats des interrogations PL
        /// </summary>
        public IList<ClPcontrole> ControlesCL { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public void ResetLists()
        {
            if (ControlesVL != null)
            {
                ControlesVL.Clear();
                ControlesVL = null;
            }
            if (ControlesPL != null)
            {
                ControlesPL.Clear();
                ControlesPL = null;
            }
            if (ControlesCL != null)
            {
                ControlesCL.Clear();
                ControlesCL = null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static string EncodeParams(string p)
        {
            if (!String.IsNullOrEmpty(p)) return StringCipher.Encrypt(p, _pwd);
            return "";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ep"></param>
        /// <returns></returns>
        public static NameValueCollection DecodeParams(string ep)
        {
            string p = StringCipher.Decrypt(ep, _pwd);
            if (!String.IsNullOrEmpty(p)) return HttpUtility.ParseQueryString(p);
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ep"></param>
        /// <returns></returns>
        public static NameValueCollection DecodeSASVayaParams(string ep)
        {
            string p = OpenSSLTools.OpenSSLStringCypher.OpenSSLDecrypt(ep, _sasPwd);
            if (!String.IsNullOrEmpty(p)) return HttpUtility.ParseQueryString(p);
            return null;
        }
        /// <summary>
        /// Test de décodage URL : Pas de cryptage
        /// </summary>
        /// <param name="ep"></param>
        /// <returns></returns>
        public static NameValueCollection NonDecodeSASVayaParams(string ep)
        {
            string p = ep;
            if (!String.IsNullOrEmpty(p)) return HttpUtility.ParseQueryString(p);
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static string GetNatureControlLabel(VlPControle c)
        {
            if (c != null && c.RNatureControle != null)
            {
                if (c.dateCtrl < _ctrl2018 && !String.IsNullOrEmpty(c.RNatureControle.libNatureCtrl))
                {
                    return c.RNatureControle.libNatureCtrl;
                }
                else if (!String.IsNullOrEmpty(c.RNatureControle.libNatureCtrl2018))
                {
                    return c.RNatureControle.libNatureCtrl2018;
                }
            }
            return "";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static string GetNatureControlLabelAbreviation(VlPControle c)
        {
            var split_names = GetNatureControlLabel(c).Trim().Split(new char[] { ' ', '-' });
            var abrev = "";
            foreach (var str in split_names)
            {
                abrev += str.ToCharArray()[0];
            }
            return abrev.ToUpper();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static string GetResultControlLabel(VlPControle c)
        {
            if (c != null && c.RResultCtrl != null)
            {
                if (c.dateCtrl < _ctrl2018 && !String.IsNullOrEmpty(c.RResultCtrl.libResultCtrl))
                {
                    return c.RResultCtrl.libResultCtrl;
                }
                else if (!String.IsNullOrEmpty(c.RResultCtrl.libResultCtrl2018))
                {
                    return c.RResultCtrl.libResultCtrl2018;
                }
            }
            return "";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static string GetNatureControlLabelAbreviation(PlPControle c)
        {
            var split_names = GetNatureControlLabel(c).Trim().Split(new char[] { ' ', '-' });
            var abrev = "";
            foreach (var str in split_names)
            {
                abrev += str.ToCharArray()[0];
            }
            return abrev.ToUpper();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static string GetNatureControlLabel(PlPControle c)
        {
            if (c != null && c.RNatureControle != null)
            {
                if (c.dateCtrl < _ctrl2018 && !String.IsNullOrEmpty(c.RNatureControle.libNatureCtrl))
                {
                    return c.RNatureControle.libNatureCtrl;
                }
                else if (!String.IsNullOrEmpty(c.RNatureControle.libNatureCtrl2018))
                {
                    return c.RNatureControle.libNatureCtrl2018;
                }
            }
            return "";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static string GetResultControlLabel(PlPControle c)
        {
            if (c != null && c.RResultCtrl != null)
            {
                if (c.dateCtrl < _ctrl2018 && !String.IsNullOrEmpty(c.RResultCtrl.libResultCtrl))
                {
                    return c.RResultCtrl.libResultCtrl;
                }
                else if (!String.IsNullOrEmpty(c.RResultCtrl.libResultCtrl2018))
                {
                    return c.RResultCtrl.libResultCtrl2018;
                }
            }
            return "";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static string GetNatureControlLabel(ClPcontrole c)
        {
            if (c != null && c.RNatureControle != null)
            {
                return c.RNatureControle.libNatureCtrl;
            }
            return "";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static string GetNatureControlLabelAbreviation(ClPcontrole c)
        {
            var split_names = GetNatureControlLabel(c).Trim().Split(new char[] { ' ', '-' });
            var abrev = "";
            foreach (var str in split_names)
            {
                abrev += str.ToCharArray()[0];
            }
            return abrev.ToUpper();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static string GetResultControlLabel(ClPcontrole c)
        {
            if (c != null && c.RResultCtrl != null)
            {
                return c.RResultCtrl.libResultCtrl;
            }
            return "";
        }
    }
}
