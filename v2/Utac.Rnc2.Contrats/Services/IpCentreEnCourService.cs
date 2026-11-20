using System.Collections.Generic;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.BusinessObjects.certificats;
using Utac.Rnc2.Infrastructure;

namespace Utac.Rnc2.Contrats.Services

{
    public partial interface IpCentreEncourService : IService<pCentreEncour>
    {
        IList<pCentreEncour> findAllPL();
        IList<pCentreEncour> findAllPLAgrees();
        IList<pCentreEncour> findAllVL();
        IList<pCentreEncour> findAllVLAgrees();
        IList<pCentreEncour> findAllAgreeSansCertificatSSL(rCategorieVehicule.CategorieEnum categorie);
        void modifyDenominationSociale(object id, pCentreEncour centre, string codeUtilisateur);
        void modifyRepresentantLegal(object id, string representantlegal, string codeUtilisateur);
        void modifyAutreModification(object id, string NumSiren, string NomRepresentantLegal, string EnseigneCommerciale, string NumTel, rStatutJuridique StatutJuridique, string codeUtilisateur);
        void retourArriere(object id, bool userMode, string codeUtilisateur, bool deleteHisto);
        void demandeAgrement(object id, pCentreEncour centre, rCategorieVehicule.CategorieEnum categVehicule, string codeUtilisateur);

        //EG le 19/05/2011
        //vérifie l'existence d'un centre , et si verifUser = true vérifie existence d'1 utilisateur
        RechercheCentreAgreeResultat estValide(string codAgrement, rCategorieVehicule.CategorieEnum categorieVehicule, bool verifExistenceUser);
        pCentreEncour findByCodeAgrementVL(string codeAgrement);
        pCentreEncour findByCodeAgrementPL(string codeAgrement);
        pCentreEncour findByCodeAgrementCL(string codeAgrement);
        pCentreEncour findByCodeAgrement(string codeAgrement);

        void notificationAgrement(object id, pCentreEncour centre, rCategorieVehicule.CategorieEnum categVehicule, string codeUtilisateur);
        void rejetAgrement(object id, pCentreEncour centre, rCategorieVehicule.CategorieEnum categVehicule, string codeUtilisateur);
        void annulerAgrement(object id, pCentreEncour centre, rCategorieVehicule.CategorieEnum categVehicule, string codeUtilisateur);
        pCentreEncour nouveauCentre(pCentreEncour centre, rCategorieVehicule.CategorieEnum categVehicule, string codeUtilisateur);
        void annulerAgrementSuiteSanction(pCentreEncour centre, pSanctionCentre sanction, string codeUtilisateur);
        void historiserCentre(pCentreEncour centre);
        void modifSuiviCentre(pCentreEncour centre, rMotifModification motif, string codeUtilisateur);
        Dictionary<int, string> ListeMotifAnnulationAgrement(rCategorieVehicule.CategorieEnum categorie, bool avecRetraitSanction);
        void renouvellementAgrementPl(object id, string codeUtilisateur);
        void rejetRenouvellementAgrement(object id, pCentreEncour centre, rCategorieVehicule.CategorieEnum categVehicule, string codeUtilisateur);
        void deleteFiche(pCentreEncour centre);
        bool aUnCertificatValide(int idCentre, rCategorieVehicule.CategorieEnum categorieVehicule);
        bool aUnCertificatValide(pCentreEncour centre, rCategorieVehicule.CategorieEnum categorieVehicule);
        void modifierFiche(object id, pCentreEncour centre, string codeUtilisateur);

        //EG le 06/01/2012
        IList<ItemListeDesInfosCertificatsExpireOuRevoque> findAllAgreeAvecCertificatExpireOuRevoque(rCategorieVehicule.CategorieEnum categorie);
        //YM le 26/01/2012
        IList<pCentreEncour> FindAllCentreNouvellementAgreeRegion(rRegion region, string datedeb);
        //YM le 26/01/2012
        IList<pCentreEncour> FindAllCentreAgreeSansControleurRegion(rRegion region);

        //PLE le 22/01/2024 Extension catégorie L
        void EnregistrerExtensionAgrementCatL(object id, pCentreEncour centre, rCategorieVehicule.CategorieEnum categVehicule, string codeUtilisateur);
    }
}
