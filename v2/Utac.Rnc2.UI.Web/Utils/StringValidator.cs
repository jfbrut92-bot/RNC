using System;
using System.Text.RegularExpressions;

namespace Utac.Rnc2.UI.Web.Utils
{
    public class StringValidator
    {
        public const string SivPattern = @"" +
                            // AA-111-AA
                            @"(([a-hj-np-tvx-zA-HJ-NP-TVX-Z][a-hj-np-rtv-zA-HJ-NP-RTV-Z]|[a-hj-np-rtv-zA-HJ-NP-RTV-Z][a-hj-np-tvx-zA-HJ-NP-TVX-Z])-[0-9]{3}-" +
                            @"([a-hj-np-tv-zA-HJ-NP-TV-Z][a-hj-np-rtv-zA-HJ-NP-RTV-Z]|[a-hj-np-rtv-zA-HJ-NP-RTV-Z][a-hj-np-tv-zA-HJ-NP-TV-Z]))" +
                            // W-001-AA ; WW-001-AA
                            @"|(([wW]?[wW])-[0-9]{3}-([a-hj-np-tv-zA-HJ-NP-TV-Z][a-hj-np-rtv-zA-HJ-NP-RTV-Z]|[a-hj-np-rtv-zA-HJ-NP-RTV-Z][a-hj-np-tv-zA-HJ-NP-TV-Z]))";
        public const string FniPattern = @"" +
                            // 1111 AAA 11 ; 1111 AAA 976 ; 1111 AAA 2A ...
                            @"([0-9]{1,4}(?:\s|-)?[a-hj-np-tv-zA-HJ-NP-TV-Z]{2,3}(?:\s|-)?(?:97[1-6]|0[1-9]|[1-8][0-9]|9[1-5]|2[abAB]))" +
                            // 1111 W 11 ; 1111 WW 11 ; 1111 W 976 ; 1111 WW 976 ; 1111 W 2A ; 1111 WW 2A
                            @"|([0-9]{1,4}(?:\s|-)?[wW]{1,2}(?:\s|-)?(?:97[1-6]|0[1-9]|[1-8][0-9]|9[1-5]|2[abAB]))";

        public const string FniCatgorieLPattern = @"" +
                            //A 99 A ou A 999 A ou AA 99 A ou AA 999 A
                            @"(([a-hj-np-tvx-zA-HJ-NP-TVWX-Z][a-hj-np-rtv-zA-HJ-NP-RTVW-Z]|[a-hj-np-rtv-zA-HJ-NP-RTVW-Z][a-hj-np-tvx-zA-HJ-NP-TVWX-Z]|[a-hj-np-rtv-zA-HJ-NP-RTVW-Z])( )([0-9]{2,3})( )" +
                            @"([a-hj-np-tv-zA-HJ-NP-TVW-Z]|[a-hj-np-rtv-zA-HJ-NP-RTVW-Z]))";

        public const string VinPattern = @"[a-hj-npr-zA-HJ-NPR-Z\d]{5,17}";
        //PLE 
        //@"(?<wmi>[a-hj-npr-zA-HJ-NPR-Z\d]{3})(?<vds>[a-hj-npr-zA-HJ-NPR-Z\d]{5})(?<check>[\dX])(?<vis>(?<year>[a-hj-npr-zA-HJ-NPR-Z\d])" +
        //                @"(?<plant>[a-hj-npr-zA-HJ-NPR-Z\d])(?<seq>[a-hj-npr-zA-HJ-NPR-Z\d]{6}))";

        //Quelques trous dans la raquette pour le test sur les numéros de département mais c'est pas grave
        public const string CodeAgrementCentreVlPattern = @"(a|s|A|S)(0|9)(\d\d|2[aAbB])(c|d|f|s|t|v|z|C|D|F|S|T|V|Z)[0-9]{3}";
        //10/05/2024 PLE Catégorie L
        public const string CodeAgrementCentreClPattern = @"(l|s|L|S)(0|9)(\d\d|2[aAbB])(c|d|f|s|t|v|z|C|D|F|S|T|V|Z)[0-9]{3}";
        public const string CodeAgrementCentrePlPattern = @"(a|s|A|S)(0|9)(\d\d|2[aAbB])(j|k|z|J|K|Z)[0-9]{3}";
        public const string CodeAgrementControleurVlPattern = @"(0|9)(\d\d|2[aAbB])(c|d|f|s|t|v|z|C|D|F|S|T|V|Z)[0-9]{4}";
        public const string CodeAgrementControleurPlPattern = @"(0|9)(\d\d|2[aAbB])(j|k|z|J|K|Z)[0-9]{4}";
        public const string CodeAgrementControleurClPattern = @"(0|9)(\d\d|2[aAbB])(c|d|f|s|t|v|z|C|D|F|S|T|V|Z)[0-9]{4}";

        public static bool IsFniNumberPlateMatch(string s)
        {
            try
            {
                return Regex.IsMatch(s, @"\A" + FniPattern + @"\Z");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(String.Format("StringValidator.IsFniNumberPlateMatch({0})\n{1}", s, ex.ToString()));
            }
            return false;
        }

        public static bool IsSivNumberPlateMatch(string s)
        {
            try
            {
                return Regex.IsMatch(s, @"\A" + SivPattern + @"\Z");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(String.Format("StringValidator.IsSivNumberPlateMatch({0})\n{1}", s, ex.ToString()));
            }
            return false;
        }
        public static bool IsFniCatgorieLNumberPlateMatch(string s)
        {
            try
            {
                return Regex.IsMatch(s, @"\A" + FniCatgorieLPattern + @"\Z");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(String.Format("StringValidator.IsFniCatgorieLNumberPlateMatch({0})\n{1}", s, ex.ToString()));
            }
            return false;
        }


        public static bool IsNumberPlateMatch(string s)
        {
            return IsSivNumberPlateMatch(s) || IsFniNumberPlateMatch(s);
        }

        public static bool IsVinNumberMatch(string s)
        {
            try
            {
                return Regex.IsMatch(s, @"\A" + VinPattern + @"\Z");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(String.Format("StringValidator.IsVinNumberMatch({0})\n{1}", s, ex.ToString()));
            }
            return false;
        }

        public static bool IsCenterApprovalCodeMatch(string s, string c)
        {
            try
            {
                if (String.Compare(c, "VL", true) == 0)
                    return Regex.IsMatch(s, @"\A" + CodeAgrementCentreVlPattern + @"\Z");
                else if (String.Compare(c, "PL", true) == 0)
                    return Regex.IsMatch(s, @"\A" + CodeAgrementCentrePlPattern + @"\Z");
                else if (String.Compare(c, "CL", true) == 0)
                    return Regex.IsMatch(s, @"\A" + CodeAgrementCentreClPattern + @"\Z");
                else
                    return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(String.Format("StringValidator.IsCenterApprovalCodeMatch({0}, {1})\n{2}", s, c, ex.ToString()));
            }
            return false;
        }

        public static bool IsControllerApprovalCodeMatch(string s, string c)
        {
            try
            {
                if (String.Compare(c, "VL", true) == 0)
                    return Regex.IsMatch(s, @"\A" + CodeAgrementControleurVlPattern + @"\Z");
                else if (String.Compare(c, "PL", true) == 0)
                    return Regex.IsMatch(s, @"\A" + CodeAgrementControleurPlPattern + @"\Z");
                else if (String.Compare(c, "CL", true) == 0)
                    return Regex.IsMatch(s, @"\A" + CodeAgrementControleurClPattern + @"\Z");
                else
                    return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(String.Format("StringValidator.IsControllerApprovalCodeMatch({0}, {1})\n{2}", s, c, ex.ToString()));
            }
            return false;
        }
    }
}
