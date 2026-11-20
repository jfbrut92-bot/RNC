using System;
using System.Collections.Generic;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Infrastructure;
using Utac.Rnc2.Infrastructure.Search;

namespace Utac.Rnc2.Contrats.Repositories
{
    public interface IpControleurEnCoursRepository : IRepository<pContoleurEncour>
    {
        IList<pContoleurEncour> LoadBy(ControleurTechniqueRecherche search);
        IList<pContoleurEncour> LoadBy(ControleurTechniqueRecherche search, int page, int pageSize, out long count);

        //EG le 21/11/2011
        IList<pContoleurEncour> FindByFiltreProfilReseau(FiltreRechercheProfilReseau form, rReseau reseauFiltre, rPrefecture prefFiltre);
        //EG le 16/01/2012
        IList<pContoleurEncour> FindAllForListeDesSuivisQualificationControleursPLSelonRegion(rRegion region);
        //YM le 24/05/2013
        IList<pContoleurEncour> FindAllForListeDesSuivisQualificationControleursVLSelonRegion(rRegion region);

        IList<pContoleurEncour> FindAllForListeDesSuivisQualificationControleursPLSelonReseaux(IList<rReseau> reseaux);
        //EG le 19/01/2012
        IList<pContoleurEncour> FindAllAgreesCentreFerme(rCategorieVehicule.CategorieEnum categorieVehicule, rPrefecture prefecture);
        //YM le 25/01/2012
        IList<pContoleurEncour> FindAllAgreesCentreFermeReseau(rCategorieVehicule.CategorieEnum categorieVehicule, IList<rReseau> reseaux);
        //YM le 26/01/2012
        IList<pContoleurEncour> FindAllAgreesCentreFermeRegion(rCategorieVehicule.CategorieEnum categorieVehicule, rRegion region);
        //EG le 26/01/2012
        IList<pContoleurEncour> FindAllDemandeEnCours(rCategorieVehicule.CategorieEnum categorieVehicule, rPrefecture prefecture);
        IList<pContoleurEncour> FindAllNouvellementAgree(rPrefecture prefecture, rCategorieVehicule.CategorieEnum categorie, DateTime debut, DateTime fin);

        //EG le 23/02/2012
        IList<pContoleurEncour> FindAllAgreesSansCentreRattachement(rCategorieVehicule.CategorieEnum categorieVehicule, rPrefecture prefecture);
        //YM le 23/10/2013 liste des préstataires Européens
        IList<pContoleurEncour> FindAllPrestatairesEuropeensVL(rPrefecture prefecture);
        IList<pContoleurEncour> FindAllPrestatairesEuropeensPL(rPrefecture prefecture);
    }
}
