using System;
using System.ComponentModel.DataAnnotations;
using Utac.Rnc2.BusinessObjects;

namespace Utac.Rnc2.UI.Web.Models
{
    public class LieuPassageCTModel
    {
        public int NumChrono { get; set; }
        //[Required(ErrorMessage="Obligatoire")]
        public string Nom { get; set; }
        //[Required(ErrorMessage = "Obligatoire")]
        public string Prenom { get; set; }
        public string NumImmat { get; set; }
        [RegularExpression(@"[a-zA-Z]{2}\-[0-9]{3}\-[a-zA-Z]{2}", ErrorMessage = "Non respect du format AA-111-AA")]
        public string NumImmatSIV { get; set; }
        [RegularExpression(@"[0-9]{2,4}\s[a-zA-Z]{2,3}\s[0-9]{2,3}", ErrorMessage = "Non respect du format 1111 AAA 111")]
        public string NumImmatFNI { get; set; }
        [RegularExpression(@"[a-zA-Z0-9\s\-]*", ErrorMessage = "Caractères non autorisés")]
        public string NumImmatAUTRE { get; set; }
        [Required(ErrorMessage = "Obligatoire")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:DD/MM/YYYY}", ApplyFormatInEditMode = true)]
        //[RegularExpression(@"[0-3][0-9]/(01|02|03|04|05|06|07|08|09|10|11|12)/[1-2][09][0-9][0-9](.*)", ErrorMessage = "La date doit respecter le format JJ/MM/AAAA")]
        [Display(Name = "Date première immat")]
        public System.DateTime? DatePremImmat { get; set; }
        [Required(ErrorMessage = "Obligatoire")]
        [RegularExpression(@"[a-zA-Z0-9]{17}", ErrorMessage = "Le n° de série doit contenir exactement 17 lettres et chiffres")]
        public string NumSerie { get; set; }
        [Required(ErrorMessage = "Sélectionner le type de votre immatriculation")]
        public string FormatImmat { get; set; }
        public DateTime DateRecherche { get; set; }

        public string CodeCentre { get; set; }
        public pCentreEncour Centre { get; set; }
        [Required(ErrorMessage = "Obligatoire")]
        public string Captcha { get; set; }
    }
}
