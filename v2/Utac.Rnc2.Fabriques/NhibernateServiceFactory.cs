using Utac.Rnc2.Contrats.Services;
using Utac.Rnc2.Repository;
using Utac.Rnc2.Repository.Nhibernate;
using Utac.Rnc2.Services;

namespace Utac.Rnc2.Fabriques
{
    public class NhibernateServiceFactory : IServiceFactory
    {
        private static NhibernateServiceFactory _factory = null;

        public ICommuneInseeService getCommuneInseeService(bool transaction)
        {
            if (transaction)
            {
                NhibernateUnitOfWork u = new NhibernateUnitOfWork();
                return new CommuneInseeService(new CommuneInseeRepository(), u);
            }
            else
            {
                return new CommuneInseeService(new CommuneInseeRepository(), null);

            }
        }

        public IrUtilisateurService getrUtilisateurService(bool transaction, bool loadDependance)
        {
            IrPrefectureService prefService = this.getrPrefectureService(false);
            IpCentreEncourService centreService = (loadDependance ? this.getpCentreEncourService(false, false) : null);
            IrReseauAppartenanceService reseauAppartenanceService = this.getrReseauAppartenanceService(false);

            if (transaction)
            {
                NhibernateUnitOfWork u = new NhibernateUnitOfWork();
                prefService.setUnitOfWorks(u);
                if (centreService != null)
                    centreService.setUnitOfWorks(u);
                reseauAppartenanceService.setUnitOfWorks(u);
                return new rUtilisateurService(new rUtilisateurRepository(), u, prefService, centreService, reseauAppartenanceService);
            }
            else
            {
                return new rUtilisateurService(new rUtilisateurRepository(), null, prefService, centreService, reseauAppartenanceService);

            }
        }

        public IrNumordreAgrementService getrNumordreAgrementService(bool transaction)
        {
            if (transaction)
            {
                return new rNumordreAgrementService(new rNumordreAgrementRepository(), new NhibernateUnitOfWork());
            }
            else
            {
                return new rNumordreAgrementService(new rNumordreAgrementRepository(), null);
            }
        }

        public IpControleurEnCourService getpControleurEnCourService(bool transaction)
        {
            //EG le 05/04/2011 pour éviter le bug du NullException sur historisation : on instancie un histoService
            IpControleurHistoService histoService = getpControleurHistoService(transaction); //EG le 17/02/2012, l'histo est au même niveau que le contrôleur
            //EG le 06/04/2011
            IrTypeDemandeControleurService typeDemandeControleurService = getIrTypeDemandeControleurService(false);
            //EG le 09/05/2011
            IrUtilisateurService utilisateurService = getrUtilisateurService(false, false);
            //EG le 20/07/2011
            IrNumordreAgrementService numOrdreAgrementService = getrNumordreAgrementService(false);
            IpCentreEncourService centreService = getpCentreEncourService(false, false);
            //EG le 19/10/2011
            IrHabilitationService habilitationService = getrHabilitationService(false);
            //EG le 21/11/2011
            IrPrefectureService prefectureService = getrPrefectureService(false);
            IrReseauService reseauService = getrReseauService(false);

            if (transaction)
            {
                NhibernateUnitOfWork u = new NhibernateUnitOfWork();
                histoService.setUnitOfWorks(u);
                typeDemandeControleurService.setUnitOfWorks(u);
                numOrdreAgrementService.setUnitOfWorks(u);
                centreService.setUnitOfWorks(u);
                habilitationService.setUnitOfWorks(u);
                prefectureService.setUnitOfWorks(u);
                reseauService.setUnitOfWorks(u);
                return new pControleurEnCourService(new pContoleurEncourRepository(), u, histoService, typeDemandeControleurService, utilisateurService, numOrdreAgrementService, centreService, habilitationService, prefectureService, reseauService);
            }
            else
            {

                return new pControleurEnCourService(new pContoleurEncourRepository(), null, histoService, typeDemandeControleurService, utilisateurService, numOrdreAgrementService, centreService, habilitationService, prefectureService, reseauService);
            }
        }

        public IrReseauService getrReseauService(bool transaction)
        {
            IpGestionCertificatService gestionCertificatService = this.getpGestionCertificatService(false);
            if (transaction)
            {
                NhibernateUnitOfWork u = new NhibernateUnitOfWork();
                gestionCertificatService.setUnitOfWorks(u);
                return new rReseauService(new rReseauRepository(), u, gestionCertificatService);
            }
            else
            {
                return new rReseauService(new rReseauRepository(), null, gestionCertificatService);
            }
        }

        public IpRapportExceptionService getpRapportExceptionService(bool transaction)
        {
            if (transaction)
            {
                return new pRapportExceptionService(new pRapportExceptionRepository(), new NhibernateUnitOfWork());
            }
            else
            {
                return new pRapportExceptionService(new pRapportExceptionRepository(), null);
            }
        }

        public IpRapportExceptionVLService getpRapportExceptionVLService(bool transaction)
        {
            if (transaction)
            {
                return new pRapportExceptionVLService(new pRapportExceptionVLRepository(), new NhibernateUnitOfWork());
            }
            else
            {
                return new pRapportExceptionVLService(new pRapportExceptionVLRepository(), null);
            }
        }

        public IpCentreEncourService getpCentreEncourService(bool transaction, bool loadDependance)
        {
            IpCentreHistoService histoService = (loadDependance ? getpCentreHistoService(false) : null);
            IrMotifModificationService motifModificationService = (loadDependance ? getpCentreMotifModificationService(false) : null);
            IrUtilisateurService utilisateurService = (loadDependance ? getrUtilisateurService(false, false) : null);
            IrNumordreAgrementService numOrdreAgrementService = (loadDependance ? getrNumordreAgrementService(false) : null);
            IrPrefectureService prefectureService = (loadDependance ? getrPrefectureService(false) : null);
            IrReseauService reseauService = (loadDependance ? getrReseauService(false) : null);
            IrStatutJuridiqueService statutJuridiqueService = (loadDependance ? getrStatutJuridiqueService(false) : null);
            IpGestionCertificatService pgestionCertificatService = (loadDependance ? getpGestionCertificatService(false) : null);
            IrProfilService profilService = (loadDependance ? getrProfilService(false) : null);
            IrProfil2Service profil2Service = (loadDependance ? getrProfil2Service(false) : null);
            IUtilisateurWebServiceVLService utilisateurWebServiceVL = (loadDependance ? getUtilisateurWebServiceVLService(false) : null);
            IUtilisateurWebServicePLService utilisateurWebServicePL = (loadDependance ? getUtilisateurWebServicePLService(false) : null);
            IUtilisateurWebServiceCLService utilisateurWebServiceCL = (loadDependance ? getUtilisateurWebServiceCLService(false) : null);
            if (transaction)
            {
                NhibernateUnitOfWork u = new NhibernateUnitOfWork();
                if (histoService != null)
                    histoService.setUnitOfWorks(u);
                if (motifModificationService != null)
                    motifModificationService.setUnitOfWorks(u);
                if (utilisateurService != null)
                    utilisateurService.setUnitOfWorks(u);
                if (numOrdreAgrementService != null)
                    numOrdreAgrementService.setUnitOfWorks(u);
                if (prefectureService != null)
                    prefectureService.setUnitOfWorks(u);
                if (reseauService != null)
                    reseauService.setUnitOfWorks(u);
                if (statutJuridiqueService != null)
                    statutJuridiqueService.setUnitOfWorks(u);
                if (pgestionCertificatService != null)
                    pgestionCertificatService.setUnitOfWorks(u);
                if (profilService != null)
                    profilService.setUnitOfWorks(u);
                if (profil2Service != null)
                    profil2Service.setUnitOfWorks(u);
                if (utilisateurWebServiceVL != null)
                    utilisateurWebServiceVL.setUnitOfWorks(u);
                if (utilisateurWebServicePL != null)
                    utilisateurWebServicePL.setUnitOfWorks(u);
                if (utilisateurWebServiceCL != null)
                    utilisateurWebServiceCL.setUnitOfWorks(u);

                return new pCentreEncourService(new pCentreEncourRepository(), u, histoService, motifModificationService, utilisateurService, numOrdreAgrementService, prefectureService, reseauService, statutJuridiqueService, pgestionCertificatService, profilService, profil2Service, utilisateurWebServiceVL, utilisateurWebServicePL, utilisateurWebServiceCL);
            }
            else
            {
                return new pCentreEncourService(new pCentreEncourRepository(), null, histoService, motifModificationService, utilisateurService, numOrdreAgrementService, prefectureService, reseauService, statutJuridiqueService, pgestionCertificatService, profilService, profil2Service, utilisateurWebServiceVL, utilisateurWebServicePL, utilisateurWebServiceCL);
            }
        }

        //YMAR le 29/01/2011 ajout de la construction du service qui historise le centre
        public IpCentreHistoService getpCentreHistoService(bool transaction)
        {
            if (transaction)
            {
                return new pCentreHistoService(new pCentreHistoRepository(), new NhibernateUnitOfWork());
            }
            else
            {
                return new pCentreHistoService(new pCentreHistoRepository(), null);
            }
        }
        //YMAR le 05/05/2011 ajout de la construction du service qui modifie le centre
        public IrMotifModificationService getpCentreMotifModificationService(bool transaction)
        {
            if (transaction)
            {
                return new rMotifModificationSercice(new rMotifModificationRepository(), new NhibernateUnitOfWork());
            }
            else
            {
                return new rMotifModificationSercice(new rMotifModificationRepository(), null);
            }
        }

        public IrLoginTempCnrService getrLoginTempCnrService(bool transaction)
        {
            if (transaction)
            {
                return new rLoginTempCnrService(new rLoginTempCnrRepository(), new NhibernateUnitOfWork());
            }
            else
            {
                return new rLoginTempCnrService(new rLoginTempCnrRepository(), null);
            }
        }

        public IrMessageUtilisateurService getrMessageUtilisateurService(bool transaction)
        {
            if (transaction)
            {
                return new rMessageUtilisateurService(new rMessageUtilisateurRepository(), new NhibernateUnitOfWork());
            }
            else
            {
                return new rMessageUtilisateurService(new rMessageUtilisateurRepository(), null);
            }
        }

        public IpCertificatService getpCertificatService(bool transaction)
        {
            IpCentreEncourService centreService = getpCentreEncourService(false, false);
            IpGestionCertificatService gestionCertificatService = getpGestionCertificatService(false);
            IrActionCertificatService actionCertificatService = getrActionCertificatService(false);
            IrUtilisateurService utilisateurService = getrUtilisateurService(false, false);

            if (transaction)
            {
                NhibernateUnitOfWork u = new NhibernateUnitOfWork();
                centreService.setUnitOfWorks(u);
                gestionCertificatService.setUnitOfWorks(u);
                actionCertificatService.setUnitOfWorks(u);
                utilisateurService.setUnitOfWorks(u);

                return new pCertificatService(new pCertificatRepository(), u, gestionCertificatService, centreService, actionCertificatService, utilisateurService);
            }
            else
            {
                return new pCertificatService(new pCertificatRepository(), null, gestionCertificatService, centreService, actionCertificatService, utilisateurService);
            }
        }

        public IpControleurHistoService getpControleurHistoService(bool transaction)
        {
            if (transaction)
            {
                return new pControleurHistoService(new pControleurHistoRepository(), new NhibernateUnitOfWork());
            }
            else
            {
                return new pControleurHistoService(new pControleurHistoRepository(), null);
            }
        }

        public IrRegionService getrRegionService(bool transaction)
        {
            if (transaction)
            {
                return new rRegionService(new rRegionRepository(), new NhibernateUnitOfWork());
            }
            else
            {
                return new rRegionService(new rRegionRepository(), null);
            }
        }

        public static IServiceFactory getInstance()
        {
            if (_factory == null)
            {
                _factory = new NhibernateServiceFactory();
            }

            return _factory;
        }

        public Infrastructure.IUnitOfWork newUnitOfWork()
        {
            return new NhibernateUnitOfWork();
        }

        public IrTypeDemandeControleurService getIrTypeDemandeControleurService(bool transaction)
        {
            if (transaction)
            {
                return new rTypeDemandeControleurService(new rTypeDemandeControleurRepository(), new NhibernateUnitOfWork());
            }
            else
            {
                return new rTypeDemandeControleurService(new rTypeDemandeControleurRepository(), null);
            }
        }

        public IrStatutJuridiqueService getrStatutJuridiqueService(bool transaction)
        {
            if (transaction)
            {
                return new rStatutJuridiqueService(new rStatutJuridiqueRepository(), new NhibernateUnitOfWork());
            }
            else
            {
                return new rStatutJuridiqueService(new rStatutJuridiqueRepository(), null);
            }
        }

        public IrProfilService getrProfilService(bool transaction)
        {
            if (transaction)
            {
                return new rProfilService(new rProfilRepository(), new NhibernateUnitOfWork());
            }
            else
            {
                return new rProfilService(new rProfilRepository(), null);
            }
        }

        //EG le 10/05/2011
        public IrProfil2Service getrProfil2Service(bool transaction)
        {
            if (transaction)
            {
                return new rProfil2Service(new rProfil2Repository(), new NhibernateUnitOfWork());
            }
            else
            {
                return new rProfil2Service(new rProfil2Repository(), null);
            }
        }

        //EG le 18/05/2011
        public IrPrefectureService getrPrefectureService(bool transaction)
        {
            if (transaction)
            {
                return new rPrefectureService(new rPrefectureRepository(), new NhibernateUnitOfWork());
            }
            else
            {
                return new rPrefectureService(new rPrefectureRepository(), null);
            }
        }

        public IrReseauAppartenanceService getrReseauAppartenanceService(bool transaction)
        {
            if (transaction)
            {
                return new rReseauAppartenanceService(new rReseauAppartenanceRepository(), new NhibernateUnitOfWork());
            }
            else
            {
                return new rReseauAppartenanceService(new rReseauAppartenanceRepository(), null);
            }
        }

        //YM le 16/06/2011
        public IrMotifModificationService getrMotifModificationService(bool transaction)
        {
            if (transaction)
            {
                return new rMotifModificationSercice(new rMotifModificationRepository(), new NhibernateUnitOfWork());
            }
            else
            {
                return new rMotifModificationSercice(new rMotifModificationRepository(), null);
            }
        }

        public IrTypeSanctionCentreService getrTypeSanctionCentreService(bool transaction)
        {
            if (transaction)
            {
                return new rTypeSanctionCentreService(new rTypeSanctionCentreRepository(), new NhibernateUnitOfWork());
            }
            else
            {
                return new rTypeSanctionCentreService(new rTypeSanctionCentreRepository(), null);
            }
        }

        public IrTypeSanctionControleurService getrTypeSanctionControleurService(bool transaction)
        {
            if (transaction)
            {
                return new rTypeSanctionControleurService(new rTypeSanctionControleurRepository(), new NhibernateUnitOfWork());
            }
            else
            {
                return new rTypeSanctionControleurService(new rTypeSanctionControleurRepository(), null);
            }
        }

        public IrTypeOrganismeDemandeurService getrTypeOrganismeDemandeurService(bool transaction)
        {
            if (transaction)
            {
                return new rTypeOrganismeDemandeurService(new rTypeOrganismeDemandeurRepository(), new NhibernateUnitOfWork());
            }
            else
            {
                return new rTypeOrganismeDemandeurService(new rTypeOrganismeDemandeurRepository(), null);
            }
        }

        public IpSanctionCentreService getpSanctionCentreService(bool transaction)
        {
            IpCentreEncourService centreService = getpCentreEncourService(false, true);
            IrMotifModificationService motifModificationService = getrMotifModificationService(false);
            IrUtilisateurService utilisateurService = getrUtilisateurService(false, false);

            if (transaction)
            {
                NhibernateUnitOfWork u = new NhibernateUnitOfWork();
                centreService.setUnitOfWorks(u);
                return new pSanctionCentreService(new pSanctionCentreRepository(), u, centreService, motifModificationService, utilisateurService);
            }
            else
            {

                return new pSanctionCentreService(new pSanctionCentreRepository(), null, centreService, motifModificationService, utilisateurService);
            }
        }

        public IpSanctionControleurService getpSanctionControleurService(bool transaction)
        {
            IpControleurEnCourService controleurService = getpControleurEnCourService(true);
            IrUtilisateurService utilisateurService = getrUtilisateurService(false, false);

            if (transaction)
            {
                NhibernateUnitOfWork u = new NhibernateUnitOfWork();
                controleurService.setUnitOfWorks(u);
                utilisateurService.setUnitOfWorks(u);
                return new pSanctionControleurService(new pSanctionControleurRepository(), u, controleurService, utilisateurService);
            }
            else
            {

                return new pSanctionControleurService(new pSanctionControleurRepository(), null, controleurService, utilisateurService);
            }
        }

        //YMAR le 14/10/2013 ajout de la construction du service qui recherche l'activité d'un contrôleur
        public IpActiviteControleurVLService getpActiviteControleurVLService(bool transaction)
        {
            return new pActiviteControleurVLService(new pActiviteControleurVLRepository(), null);
        }

        public IpActiviteControleurCLService getpActiviteControleurCLService(bool transaction)
        {
            return new pActiviteControleurCLService(new pActiviteControleurCLRepository(), null);
        }


        //YMAR le 27/02/2014 ajout de la construction du service qui gère les traces de demande de PV
        public IpTraceRechPvVlService getpTraceRechPvVlService(bool transaction)
        {
            IrUtilisateurService utilisateurService = getrUtilisateurService(false, false);

            if (transaction)
            {
                NhibernateUnitOfWork u = new NhibernateUnitOfWork();
                utilisateurService.setUnitOfWorks(u);
                return new pTraceRechPvVlService(new pTraceRechPvVlRepository(), u, utilisateurService);
            }
            else
            {
                return new pTraceRechPvVlService(new pTraceRechPvVlRepository(), null, utilisateurService);
            }
        }

        //YMAR le 27/02/2014 ajout de la construction du service qui gère les traces de demande lieu de PV
        public IpTraceRechLieuVlService getpTraceRechLieuVlService(bool transaction)
        {

            if (transaction)
            {
                NhibernateUnitOfWork u = new NhibernateUnitOfWork();
                return new pTraceRechLieuVlService(new pTraceRechLieuVlRepository(), u);
            }
            else
            {
                return new pTraceRechLieuVlService(new pTraceRechLieuVlRepository(), null);
            }
        }

        public IpAmendeCentreService getpAmendeCentreService(bool transaction)
        {
            IpCentreEncourService centreService = getpCentreEncourService(false, true);
            IrMotifModificationService motifModificationService = getrMotifModificationService(false);

            if (transaction)
            {
                NhibernateUnitOfWork u = new NhibernateUnitOfWork();
                centreService.setUnitOfWorks(u);
                return new pAmendeCentreService(new pAmendeCentreRepository(), u, centreService, motifModificationService);
            }
            else
            {

                return new pAmendeCentreService(new pAmendeCentreRepository(), null, centreService, motifModificationService);
            }
        }

        public IpAmendeControleurService getpAmendeControleurService(bool transaction)
        {
            IpControleurEnCourService controleurService = getpControleurEnCourService(false);

            if (transaction)
            {
                NhibernateUnitOfWork u = new NhibernateUnitOfWork();
                controleurService.setUnitOfWorks(u);
                return new pAmendeControleurService(new pAmendeControleurRepository(), u, controleurService);
            }
            else
            {

                return new pAmendeControleurService(new pAmendeControleurRepository(), null, controleurService);
            }
        }

        public IpDerogationCentreService getpDerogationCentreService(bool transaction)
        {
            IpCentreEncourService centreService = getpCentreEncourService(false, true);
            IrMotifModificationService motifModificationService = getrMotifModificationService(false);
            IrUtilisateurService utilisateurService = getrUtilisateurService(false, false);

            if (transaction)
            {
                NhibernateUnitOfWork u = new NhibernateUnitOfWork();
                centreService.setUnitOfWorks(u);
                return new pDerogationCentreService(new pDerogationCentreRepository(), u, centreService, motifModificationService, utilisateurService);
            }
            else
            {

                return new pDerogationCentreService(new pDerogationCentreRepository(), null, centreService, motifModificationService, utilisateurService);
            }
        }

        public IrHabilitationService getrHabilitationService(bool transaction)
        {
            if (transaction)
            {
                NhibernateUnitOfWork u = new NhibernateUnitOfWork();
                return new rHabilitationService(new rHabilitationRepository(), u);
            }
            else
            {

                return new rHabilitationService(new rHabilitationRepository(), null);
            }
        }

        public IrCategorieVehiculeService getrCategorieVehiculeService(bool transaction)
        {
            if (transaction)
            {
                NhibernateUnitOfWork u = new NhibernateUnitOfWork();
                return new rCategorieVehiculeService(new rCategorieVehiculeRepository(), u);
            }
            else
            {

                return new rCategorieVehiculeService(new rCategorieVehiculeRepository(), null);
            }
        }

        public IpAuditControleurService getpAuditControleurService(bool transaction)
        {
            IpControleurEnCourService controleurService = getpControleurEnCourService(false);
            IrUtilisateurService utilisateurService = getrUtilisateurService(false, false);
            if (transaction)
            {
                NhibernateUnitOfWork u = new NhibernateUnitOfWork();
                utilisateurService.setUnitOfWorks(u);
                return new pAuditControleurService(new pAuditControleurRepository(), u, controleurService, utilisateurService);
            }
            else
            {

                return new pAuditControleurService(new pAuditControleurRepository(), null, controleurService, utilisateurService);
            }
        }

        //EG le 03/10/2011
        public IpAuditControleurVLService getpAuditControleurVLService(bool transaction)
        {
            IpControleurEnCourService controleurService = getpControleurEnCourService(false);
            IrUtilisateurService utilisateurService = getrUtilisateurService(false, false);
            if (transaction)
            {
                NhibernateUnitOfWork u = new NhibernateUnitOfWork();
                utilisateurService.setUnitOfWorks(u);
                return new pAuditControleurVLService(new pAuditControleurVLRepository(), u, controleurService, utilisateurService);
            }
            else
            {

                return new pAuditControleurVLService(new pAuditControleurVLRepository(), null, controleurService, utilisateurService);
            }
        }

        public IpAuditControleurCLService getpAuditControleurCLService(bool transaction)
        {
            IpControleurEnCourService controleurService = getpControleurEnCourService(false);
            IrUtilisateurService utilisateurService = getrUtilisateurService(false, false);
            if (transaction)
            {
                NhibernateUnitOfWork u = new NhibernateUnitOfWork();
                utilisateurService.setUnitOfWorks(u);
                return new pAuditControleurCLService(new pAuditControleurCLRepository(), u, controleurService, utilisateurService);
            }
            else
            {

                return new pAuditControleurCLService(new pAuditControleurCLRepository(), null, controleurService, utilisateurService);
            }
        }

        public IpFormationControleurPLService getpFormationControleurPLService(bool transaction)
        {
            IpControleurEnCourService controleurService = getpControleurEnCourService(false);
            IrUtilisateurService utilisateurService = getrUtilisateurService(false, false);
            if (transaction)
            {
                NhibernateUnitOfWork u = new NhibernateUnitOfWork();
                utilisateurService.setUnitOfWorks(u);
                return new pFormationControleurPLService(new pFormationControleurPLRepository(), u, controleurService, utilisateurService);
            }
            else
            {

                return new pFormationControleurPLService(new pFormationControleurPLRepository(), null, controleurService, utilisateurService);
            }
        }

        public IpFormationControleurVLService getpFormationControleurVLService(bool transaction)
        {
            IpControleurEnCourService controleurService = getpControleurEnCourService(false);
            IrUtilisateurService utilisateurService = getrUtilisateurService(false, false);
            if (transaction)
            {
                NhibernateUnitOfWork u = new NhibernateUnitOfWork();
                utilisateurService.setUnitOfWorks(u);
                return new pFormationControleurVLService(new pFormationControleurVLRepository(), u, controleurService, utilisateurService);
            }
            else
            {

                return new pFormationControleurVLService(new pFormationControleurVLRepository(), null, controleurService, utilisateurService);
            }
        }

        public IpFormationControleurCLService getpFormationControleurCLService(bool transaction)
        {
            IpControleurEnCourService controleurService = getpControleurEnCourService(false);
            IrUtilisateurService utilisateurService = getrUtilisateurService(false, false);
            if (transaction)
            {
                NhibernateUnitOfWork u = new NhibernateUnitOfWork();
                utilisateurService.setUnitOfWorks(u);
                return new pFormationControleurCLService(new pFormationControleurCLRepository(), u, controleurService, utilisateurService);
            }
            else
            {

                return new pFormationControleurCLService(new pFormationControleurCLRepository(), null, controleurService, utilisateurService);
            }
        }

        public IrTypeFormationService getrTypeFormationService(bool transaction)
        {
            if (transaction)
            {
                NhibernateUnitOfWork u = new NhibernateUnitOfWork();
                return new rTypeFormationService(new rTypeFormationRepository(), u);
            }
            else
            {

                return new rTypeFormationService(new rTypeFormationRepository(), null);
            }
        }

        public IParametreService getParametreService(bool transaction)
        {
            if (transaction)
            {
                NhibernateUnitOfWork u = new NhibernateUnitOfWork();
                return new ParametreService(new ParametreRepository(), u);
            }
            else
            {

                return new ParametreService(new ParametreRepository(), null);
            }
        }

        public IpGestionCertificatService getpGestionCertificatService(bool transaction)
        {
            if (transaction)
            {
                NhibernateUnitOfWork u = new NhibernateUnitOfWork();
                return new pGestionCertificatService(new pGestionCertificatRepository(), u);
            }
            else
            {

                return new pGestionCertificatService(new pGestionCertificatRepository(), null);
            }
        }

        public IrActionCertificatService getrActionCertificatService(bool transaction)
        {
            if (transaction)
            {
                NhibernateUnitOfWork u = new NhibernateUnitOfWork();
                return new rActionCertificatSercice(new rActionCertificatRepository(), u);
            }
            else
            {

                return new rActionCertificatSercice(new rActionCertificatRepository(), null);
            }
        }

        public IpEquipementCentreVLService getpEquipementCentreVLService(bool transaction)
        {
            IpCentreEncourService centreService = getpCentreEncourService(false, true);
            IrMotifModificationService motifModificationService = getrMotifModificationService(false);

            if (transaction)
            {
                NhibernateUnitOfWork u = new NhibernateUnitOfWork();
                centreService.setUnitOfWorks(u);
                motifModificationService.setUnitOfWorks(u);
                return new pEquipementCentreVLService(new pEquipementCentreVLRepository(), u, centreService, motifModificationService);
            }
            else
            {
                return new pEquipementCentreVLService(new pEquipementCentreVLRepository(), null, centreService, motifModificationService);
            }
        }

        public IpEquipementCentrePLService getpEquipementCentrePLService(bool transaction)
        {
            IpCentreEncourService centreService = getpCentreEncourService(false, true);
            IrMotifModificationService motifModificationService = getrMotifModificationService(false);

            if (transaction)
            {
                NhibernateUnitOfWork u = new NhibernateUnitOfWork();
                centreService.setUnitOfWorks(u);
                motifModificationService.setUnitOfWorks(u);
                return new pEquipementCentrePLService(new pEquipementCentrePLRepository(), u, centreService, motifModificationService);
            }
            else
            {
                return new pEquipementCentrePLService(new pEquipementCentrePLRepository(), null, centreService, motifModificationService);
            }
        }

        public IpEquipementCentreCLService getpEquipementCentreCLService(bool transaction)
        {
            IpCentreEncourService centreService = getpCentreEncourService(false, true);
            IrMotifModificationService motifModificationService = getrMotifModificationService(false);

            if (transaction)
            {
                NhibernateUnitOfWork u = new NhibernateUnitOfWork();
                centreService.setUnitOfWorks(u);
                motifModificationService.setUnitOfWorks(u);
                return new pEquipementCentreCLService(new pEquipementCentreCLRepository(), u, centreService, motifModificationService);
            }
            else
            {
                return new pEquipementCentreCLService(new pEquipementCentreCLRepository(), null, centreService, motifModificationService);
            }
        }

        public IpInfoRdvService getpInfoRdvService(bool transaction)
        {
            IpCentreEncourService centreService = getpCentreEncourService(false, true);
            IrMotifModificationService motifModificationService = getrMotifModificationService(false);

            if (transaction)
            {
                NhibernateUnitOfWork u = new NhibernateUnitOfWork();
                centreService.setUnitOfWorks(u);
                motifModificationService.setUnitOfWorks(u);
                return new pInfoRdvService(new pInfoRdvRepository(), u, centreService, motifModificationService);
            }
            else
            {
                return new pInfoRdvService(new pInfoRdvRepository(), null, centreService, motifModificationService);
            }
        }

        public IpHoraireOuvertureService getpHoraireOuvertureService(bool transaction)
        {
            IpCentreEncourService centreService = getpCentreEncourService(false, true);
            IrMotifModificationService motifModificationService = getrMotifModificationService(false);

            if (transaction)
            {
                NhibernateUnitOfWork u = new NhibernateUnitOfWork();
                centreService.setUnitOfWorks(u);
                motifModificationService.setUnitOfWorks(u);
                return new pHoraireOuvertureService(new pHoraireOuvertureRepository(), u, centreService, motifModificationService);
            }
            else
            {
                return new pHoraireOuvertureService(new pHoraireOuvertureRepository(), null, centreService, motifModificationService);
            }
        }

        public IpJournalActiviteService getpJournalActiviteService(bool transaction)
        {
            IpCentreEncourService centreService = getpCentreEncourService(false, true);
            IpControleurEnCourService controleurService = getpControleurEnCourService(false);
            IrUtilisateurService utilisateurService = getrUtilisateurService(false, false);

            if (transaction)
            {
                NhibernateUnitOfWork u = new NhibernateUnitOfWork();
                centreService.setUnitOfWorks(u);
                controleurService.setUnitOfWorks(u);
                utilisateurService.setUnitOfWorks(u);
                return new pJournalActiviteService(new pJournalActiviteRepository(), u, centreService, controleurService, utilisateurService);
            }
            else
            {
                return new pJournalActiviteService(new pJournalActiviteRepository(), null, centreService, controleurService, utilisateurService);
            }
        }

        public IStatistiquesControleurService getStatistiquesControleurService()
        {
            IpCumulActivitePlJrnTrtService cumulActiviteService = this.getpCumulActivitePlJrnTrtService(false);
            IpControleurEnCourService controleurService = this.getpControleurEnCourService(false);

            return new StatistiquesService(new StatistiquesControleurRepository(), cumulActiviteService, controleurService);
        }

        public IStatistiquesCentreService getStatistiquesCentreService()
        {
            IpCumulActivitePlJrnTrtService cumulActiviteService = this.getpCumulActivitePlJrnTrtService(false);
            IpControleurEnCourService controleurService = this.getpControleurEnCourService(false);

            return new StatistiquesCentreService(new StatistiquesCentreRepository());
        }

        public IrEditionService getrEditionService(bool transaction)
        {
            if (transaction)
            {
                NhibernateUnitOfWork u = new NhibernateUnitOfWork();
                return new rEditionService(new rEditionRepository(), u);
            }
            else
            {
                return new rEditionService(new rEditionRepository(), null);
            }
        }

        public IrTemplateMailService getrTemplateMailService(bool transaction)
        {
            if (transaction)
            {
                NhibernateUnitOfWork u = new NhibernateUnitOfWork();
                return new rTemplateMailService(new rTemplateMailRepository(), u);
            }
            else
            {
                return new rTemplateMailService(new rTemplateMailRepository(), null);
            }
        }

        public IpCourrierEnvoyeService getpCourrierEnvoyeService(bool transaction)
        {
            if (transaction)
            {
                NhibernateUnitOfWork u = new NhibernateUnitOfWork();
                return new pCourrierEnvoyeService(new pCourrierEnvoyeRepository(), u);
            }
            else
            {
                return new pCourrierEnvoyeService(new pCourrierEnvoyeRepository(), null);
            }
        }

        public IpCumulActivitePlJrnTrtService getpCumulActivitePlJrnTrtService(bool transaction)
        {
            if (transaction)
            {
                NhibernateUnitOfWork u = new NhibernateUnitOfWork();
                return new pCumulActivitePlJrnTrtService(new pCumulActivitePlJrnTrtRepository(), u);
            }
            else
            {
                return new pCumulActivitePlJrnTrtService(new pCumulActivitePlJrnTrtRepository(), null);
            }
        }

        public IUtilisateurWebServiceVLService getUtilisateurWebServiceVLService(bool transaction)
        {
            if (transaction)
            {
                NhibernateUnitOfWork u = new NhibernateUnitOfWork();
                return new UtilisateurWebServiceVLService(new UtilisateurWebServiceVLRepository(), u);
            }
            else
            {
                return new UtilisateurWebServiceVLService(new UtilisateurWebServiceVLRepository(), null);
            }
        }
        //PLE le 20/02/2024 Catégorie L
        public IUtilisateurWebServiceCLService getUtilisateurWebServiceCLService(bool transaction)
        {
            if (transaction)
            {
                NhibernateUnitOfWork u = new NhibernateUnitOfWork();
                return new UtilisateurWebServiceCLService(new UtilisateurWebServiceCLRepository(), u);
            }
            else
            {
                return new UtilisateurWebServiceCLService(new UtilisateurWebServiceCLRepository(), null);
            }
        }

        public IUtilisateurWebServicePLService getUtilisateurWebServicePLService(bool transaction)
        {
            if (transaction)
            {
                NhibernateUnitOfWork u = new NhibernateUnitOfWork();
                return new UtilisateurWebServicePLService(new UtilisateurWebServicePLRepository(), u);
            }
            else
            {
                return new UtilisateurWebServicePLService(new UtilisateurWebServicePLRepository(), null);
            }
        }

        public IpFactureService getpFactureService(bool transaction)
        {
            if (transaction)
            {
                return new pFactureService(new pFactureRepository(), new NhibernateUnitOfWork());
            }
            else
            {
                return new pFactureService(new pFactureRepository(), null);
            }
        }

        public IpRapportExceptionCLService getpRapportExceptionCLService(bool transaction)
        {
            if (transaction)
            {
                return new pRapportExceptionCLService(new pRapportExceptionCLRepository(), new NhibernateUnitOfWork());
            }
            else
            {
                return new pRapportExceptionCLService(new pRapportExceptionCLRepository(), null);
            }
        }
    }
}
