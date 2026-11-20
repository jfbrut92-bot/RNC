using Utac.Rnc2.Contrats.Services;
using Utac.Rnc2.Infrastructure;

namespace Utac.Rnc2.Fabriques
{
    public interface IServiceFactory
    {
        IrUtilisateurService getrUtilisateurService(bool transaction, bool loadDependance);
        IrNumordreAgrementService getrNumordreAgrementService(bool transaction);
        IpControleurEnCourService getpControleurEnCourService(bool transaction);
        IpControleurHistoService getpControleurHistoService(bool transaction);
        IrReseauService getrReseauService(bool transaction);
        IpRapportExceptionService getpRapportExceptionService(bool transaction);
        IpRapportExceptionVLService getpRapportExceptionVLService(bool transaction);
        IpCentreEncourService getpCentreEncourService(bool transaction, bool loadDependance);
        IrLoginTempCnrService getrLoginTempCnrService(bool transaction);
        IrMessageUtilisateurService getrMessageUtilisateurService(bool transaction);
        IpCertificatService getpCertificatService(bool transaction);
        IrTypeDemandeControleurService getIrTypeDemandeControleurService(bool transaction);
        IrRegionService getrRegionService(bool transaction);
        IrProfilService getrProfilService(bool transaction);
        IrStatutJuridiqueService getrStatutJuridiqueService(bool transaction);
        IrProfil2Service getrProfil2Service(bool transaction);
        IrPrefectureService getrPrefectureService(bool transaction);
        IrReseauAppartenanceService getrReseauAppartenanceService(bool transaction);
        IrMotifModificationService getrMotifModificationService(bool transaction);
        ICommuneInseeService getCommuneInseeService(bool transaction);
        IUnitOfWork newUnitOfWork();
        IrTypeSanctionCentreService getrTypeSanctionCentreService(bool transaction);
        IrTypeOrganismeDemandeurService getrTypeOrganismeDemandeurService(bool transaction);
        IpSanctionCentreService getpSanctionCentreService(bool transaction);
        IpAmendeCentreService getpAmendeCentreService(bool transaction);
        IpDerogationCentreService getpDerogationCentreService(bool transaction);
        //EG le 19/09/2011
        IrTypeSanctionControleurService getrTypeSanctionControleurService(bool transaction);
        IpSanctionControleurService getpSanctionControleurService(bool transaction);
        IpAmendeControleurService getpAmendeControleurService(bool transaction);
        IrHabilitationService getrHabilitationService(bool transaction);
        IrCategorieVehiculeService getrCategorieVehiculeService(bool transaction);
        IpAuditControleurService getpAuditControleurService(bool transaction);
        //EG le 03/10/2011
        IpAuditControleurVLService getpAuditControleurVLService(bool transaction);
        IpAuditControleurCLService getpAuditControleurCLService(bool transaction);
        IpFormationControleurPLService getpFormationControleurPLService(bool transaction);
        IrTypeFormationService getrTypeFormationService(bool transaction);
        IpFormationControleurVLService getpFormationControleurVLService(bool transaction);
        IpFormationControleurCLService getpFormationControleurCLService(bool transaction);
        IParametreService getParametreService(bool transaction);
        //YM le 12/10/2011
        IpGestionCertificatService getpGestionCertificatService(bool transaction);
        //EG le 14/10/2011
        IrActionCertificatService getrActionCertificatService(bool transaction);
        //YM le 04/11/2011
        IpCentreHistoService getpCentreHistoService(bool transaction);
        //EG le 30/11/2011
        IpEquipementCentreVLService getpEquipementCentreVLService(bool transaction);
        //EG le 01/12/2011
        IpEquipementCentrePLService getpEquipementCentrePLService(bool transaction);
        IpEquipementCentreCLService getpEquipementCentreCLService(bool transaction);

        IpInfoRdvService getpInfoRdvService(bool transaction);

        IpHoraireOuvertureService getpHoraireOuvertureService(bool transaction);

        IpJournalActiviteService getpJournalActiviteService(bool transaction);
        //EG le 20/12/2011 : les statistiques sont en read-only donc pas de transaction
        IStatistiquesControleurService getStatistiquesControleurService();
        IStatistiquesCentreService getStatistiquesCentreService();
        IrEditionService getrEditionService(bool transaction);
        //EG le 10/01/2012
        IrTemplateMailService getrTemplateMailService(bool transaction);
        IpCourrierEnvoyeService getpCourrierEnvoyeService(bool transaction);
        //EG le 13/01/2012
        IpCumulActivitePlJrnTrtService getpCumulActivitePlJrnTrtService(bool transaction);
        //EG le 18/01/2012
        IUtilisateurWebServiceVLService getUtilisateurWebServiceVLService(bool transaction);
        IUtilisateurWebServicePLService getUtilisateurWebServicePLService(bool transaction);
        //PLE le 20/02/2024
        IUtilisateurWebServiceCLService getUtilisateurWebServiceCLService(bool transaction);
        //YMAR le 14/10/2013
        IpActiviteControleurVLService getpActiviteControleurVLService(bool transaction);

        //YMAR le 27/02/2014
        IpTraceRechPvVlService getpTraceRechPvVlService(bool transaction);
        IpTraceRechLieuVlService getpTraceRechLieuVlService(bool transaction);

        IpFactureService getpFactureService(bool transaction);
        //PLE le 05/04/2024
        IpRapportExceptionCLService getpRapportExceptionCLService(bool transaction);
        //PLE le 25/03/2025 Ajout suivi activité CL
        IpActiviteControleurCLService getpActiviteControleurCLService(bool transaction);
    }
}
