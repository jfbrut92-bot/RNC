using System.Collections.Generic;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Infrastructure;
using Utac.Rnc2.Infrastructure.Search;

namespace Utac.Rnc2.Contrats.Services
{
    public interface IpControleurEnCourService : IService<pContoleurEncour>
    {
        void modifyEtatCivil(object id, pContoleurEncour model, string codeUtilisateur);
        void retourArriere(object id, bool mode, string codeUtilisateur, bool deleteHisto);
        void demandeAgrement(object id, pContoleurEncour controleur, rCategorieVehicule.CategorieEnum categorie, string codeUtilisateur);
        void notificationAgrement(object id, pContoleurEncour controleur, rCategorieVehicule.CategorieEnum categorie, string codeUtilisateur);
        void rejetAgrement(object id, pContoleurEncour controleur, rCategorieVehicule.CategorieEnum categorie, string codeUtilisateur);
        void annulerAgrement(object id, pContoleurEncour controleur, rCategorieVehicule.CategorieEnum categorie, string codeUtilisateur);

        //EG le 13/07/2011
        pContoleurEncour creerNouveau(pContoleurEncour controleur, rCategorieVehicule.CategorieEnum categorie, string codeUtilisateur);

        void changementCentreRattachement(int id, pContoleurEncour controleur, rCategorieVehicule.CategorieEnum categorie, string codeUtilisateur);
        void declarerHabilitationGaz(int id, pContoleurEncour controleur, string codeUtilisateur);

        void annulerAgrementSuiteSanction(pContoleurEncour controleur, pSanctionControleur sanction, string codeUtilisateur);

        void modifSuivi(pContoleurEncour controleur, rTypeDemandeControleur.TypeModification typeModification, string codeUtilisateur);

        void declarerHabilitationPL(int id, pContoleurEncour model, string codeUtilisateur);

        //YM 29/09/2011
        IList<pContoleurEncour> findAllVL(pCentreEncour centre);
        IList<pContoleurEncour> findAllPL(pCentreEncour centre);
        IList<pContoleurEncour> findAllCL(pCentreEncour centre);

        void modifFiche(int id, pContoleurEncour model, string codeUtilisateur);

        void supprimeFiche(pContoleurEncour controleur);

        //PLE le 07/05/2015
        //void annulerModification(pContoleurEncour controleur);

        //EG le 18/11/2011
        IList<pContoleurEncour> FindByFiltreProfilReseau(FiltreRechercheProfilReseau filtreRechercheProfilReseau, out System.Boolean estDepartementErrone);
        //EG le 16/01/2012
        IList<pContoleurEncour> FindAllForListeDesSuivisQualificationControleursPLSelonRegion(rRegion region);
        IList<pContoleurEncour> FindAllForListeDesSuivisQualificationControleursPLSelonReseaux(IList<rReseau> reseaux);
        //EG le 19/01/2012
        IList<pContoleurEncour> FindAllAgreesCentreFerme(rCategorieVehicule.CategorieEnum categorieVehicule, rPrefecture prefecture);
        IList<pContoleurEncour> FindAllDemandeEnCours(rCategorieVehicule.CategorieEnum categorieVehicule, rPrefecture prefecture);
        IList<pContoleurEncour> FindAllDemandeEnCours(rPrefecture prefecture);
        IList<pContoleurEncour> FindAllNouvellementAgree(rPrefecture prefecture);
        //YM le 25/01/2012
        IList<pContoleurEncour> FindAllAgreesCentreFermeReseau(rCategorieVehicule.CategorieEnum categorieVehicule, IList<rReseau> reseaux);
        //YM le 26/01/2012
        IList<pContoleurEncour> FindAllAgreesCentreFermeRegion(rCategorieVehicule.CategorieEnum categorieVehicule, rRegion region);

        //EG le 23/02/2012
        IList<pContoleurEncour> FindAllAgreesSansCentreRattachement(rCategorieVehicule.CategorieEnum categorieVehicule, rPrefecture prefecture);
        //YM le 23/05/2013
        IList<pContoleurEncour> FindAllForListeDesSuivisQualificationControleursVLSelonRegion(rRegion region);
        //YM le 23/10/2013 liste des préstataires Européens
        IList<pContoleurEncour> FindAllPrestatairesEuropeensVL(rPrefecture prefecture);
        IList<pContoleurEncour> FindAllPrestatairesEuropeensPL(rPrefecture prefecture);

        //PLE le 27/01/2024 Extension agrement catégorie L
        void EnregistrerExtensionAgrementCatL(object id, pContoleurEncour controleur, rCategorieVehicule.CategorieEnum categVehicule, string codeUtilisateur);

    }
}
