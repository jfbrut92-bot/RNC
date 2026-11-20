// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Utac.Rnc2.BusinessObjects;

namespace Utac.Rnc2.UI.Web.Models
{
    public class NotificationAgrementModel
    {
        public rCategorieVehicule.CategorieEnum CategVehicule { get; set; }
        public Boolean DateValide { get; set; }
        public pCentreEncour Centre { get; set; }
    }
}
