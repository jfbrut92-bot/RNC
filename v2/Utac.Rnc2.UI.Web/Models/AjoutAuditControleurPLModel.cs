using System.Collections.Generic;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.UI.Web.Utils;

namespace Utac.Rnc2.UI.Web.Models
{
    public class AjoutAuditControleurPLModel
    {

        //EG le 11/07/2012 pour essayer d'éviter l'erreur de conversion de jeu de caractères Sybase ASE
        //On décompose la propriété Audit

        private pAuditControleur _audit;
        public pAuditControleur Audit
        {
            get
            {
                return this._audit;
            }
            set
            {
                //EG le 11/07/2012...on essaye d'encoder en ISO-8859-1 pour éviter l'erreur ASE...tant pis pour les copier-coller de caractères bizarre
                //Seulement pour les propriétés de type chaîne de caractères
                value.CodAgremCntr = StringEncoder.Utf8ToIso(value.CodAgremCntr);
                value.ImmatVehicule = StringEncoder.Utf8ToIso(value.ImmatVehicule);
                value.NumPv = StringEncoder.Utf8ToIso(value.NumPv);

                this._audit = value;

            }
        }
        public pContoleurEncour Controleur { get; set; }
        public IList<rCategorieVehicule> ListeCategorieVehicule { get; set; }
    }
}