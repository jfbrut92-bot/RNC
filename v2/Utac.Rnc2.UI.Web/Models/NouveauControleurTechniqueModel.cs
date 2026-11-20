// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Utac.Rnc2.BusinessObjects;
using System.Collections.Generic;

namespace Utac.Rnc2.UI.Web.Models
{
    public class NouveauControleurTechniqueModel
    {
        public pContoleurEncour controleur { get; set; }
        public rCategorieVehicule.CategorieEnum categorie { get; set; }
        public int NombreChargement { get; set; }
        //Pour la gestion du booléen pas renseigné
        public string EstPrestataire { get; set; }
        public String Token { get; set; }
        public string ConfirmPrestataire { get; set; }
        public string ConfirmControleursEnDoublons { get; set; }
        public IList<pContoleurEncour> controleursEnDoublons { get; set; }
    }
}
