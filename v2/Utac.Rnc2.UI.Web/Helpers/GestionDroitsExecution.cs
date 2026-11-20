using System;
using System.Security.Principal;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Contrats.Services;
using Utac.Rnc2.Fabriques;
using Utac.Rnc2.UI.Web.Models;

namespace Utac.Rnc2.UI.Web.Helpers
{
    public class GestionDroitsExecution
    {
        private static IpCentreEncourService centreService;
        private static IrUtilisateurService utilisateurService;
        private static IrPrefectureService prefectureService;

        private static void Init()
        {
            if (centreService == null)
            {
                centreService = NhibernateServiceFactory.getInstance().getpCentreEncourService(true, true);
            }

            if (utilisateurService == null)
            {
                utilisateurService = NhibernateServiceFactory.getInstance().getrUtilisateurService(true, true);
            }

            if (prefectureService == null)
            {
                prefectureService = NhibernateServiceFactory.getInstance().getrPrefectureService(true);
            }
        }

        public static bool isRightActionForCentre(IPrincipal utilisateurConnecte, pCentreEncour centre, ActionsFicheCentre action)
        {
            Init();
            //todo : affiner les actions qui doivent vérifier les droits par type d'agrément (un à un)

            //YM 02/02/2012 verifier que l'utilisateur est bien de la prefecture du centre
            rUtilisateur utilisateur = utilisateurService.FindBy(utilisateurConnecte.Identity.Name);

            rPrefecture prefectureUser = new rPrefecture();
            if (utilisateur != null && utilisateur.RPrefecture != null)
            {
                prefectureUser = prefectureService.FindBy(utilisateur.RPrefecture.Id);
            }

            switch (action)
            {
                case ActionsFicheCentre.ADMMODFI:
                    //On doit être OTC
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()))
                    {
                        return true;
                    }
                    return false;

                case ActionsFicheCentre.ADMMODFINT:
                    //On doit être OTC
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()))
                    {
                        return true;
                    }
                    return false;

                case ActionsFicheCentre.ADMSUPFI:
                    //On doit être OTC
                    //(1)	SAUF si motif_fin_agrement_vl=2;3;4 et f_est_ancien_installation(id_centre_encours)=true
                    //CHANGEMENT_EXPLOITANT = 2,
                    //CHANGEMENT_LOCALISATION = 3,
                    //CHANGEMENT_RESEAU = 4,

                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()))
                    {
                        if (
                            centre.EtatAgrementVl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementVl.AGREMENT_KO))
                            &&
                            (
                                Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementVL.CHANGEMENT_EXPLOITANT).Equals(centre.MotifFinAgrementVl) ||
                                Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementVL.CHANGEMENT_LOCALISATION).Equals(centre.MotifFinAgrementVl) ||
                                Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementVL.CHANGEMENT_RESEAU).Equals(centre.MotifFinAgrementVl)
                            )
                            && centreService.isAncienCentre(centre)
                        )
                        {
                            return false;
                        }

                        if (
                            centre.EtatAgrementPl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_KO))
                            &&
                            (
                                Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementPL.CHANGEMENT_EXPLOITANT).Equals(centre.MotifFinAgrementPl) ||
                                Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementPL.CHANGEMENT_LOCALISATION).Equals(centre.MotifFinAgrementPl) ||
                                Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementPL.CHANGEMENT_RESEAU).Equals(centre.MotifFinAgrementPl)
                            )
                            && centreService.isAncienCentre(centre)
                        )
                        {
                            return false;
                        }

                        if (
                            centre.EtatAgrementCl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementCl.AGREMENT_KO))
                            &&
                            (
                                Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementCL.CHANGEMENT_EXPLOITANT).Equals(centre.MotifFinAgrementCl) ||
                                Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementCL.CHANGEMENT_LOCALISATION).Equals(centre.MotifFinAgrementCl) ||
                                Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementCL.CHANGEMENT_RESEAU).Equals(centre.MotifFinAgrementCl)
                            )
                            && centreService.isAncienCentre(centre)
                        )
                        {
                            return false;
                        }

                        return true;

                    }
                    return false;

                case ActionsFicheCentre.AMEND:
                    //TODO DRIRE => ORGANISME SURVEILLANCE dans r_profil2
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || utilisateurConnecte.IsInRole(rUtilisateur.Profil.DRIRE.ToString()))
                    {
                        if (centre.EtatAgrementVl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementVl.AGREMENT_OK)))
                        {
                            return true;
                        }

                        if (centre.EtatAgrementPl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_OK)) ||
                            centre.EtatAgrementPl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_RENOUVELE_OK))
                        )
                        {
                            return true;
                        }

                        if (centre.EtatAgrementCl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementCl.AGREMENT_OK)))
                        {
                            return true;
                        }

                    }
                    return false;

                case ActionsFicheCentre.ANUVL:
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || (utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) && prefectureUser == centre.RPrefecture))
                    {
                        if (centre.EtatAgrementVl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementVl.AGREMENT_OK)))
                        {
                            return true;
                        }
                    }
                    return false;

                case ActionsFicheCentre.ANUPL:
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || (utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) && prefectureUser == centre.RPrefecture))
                    {
                        if (centre.EtatAgrementPl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_OK)) ||
                            centre.EtatAgrementPl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_RENOUVELE_OK))
                        )
                        {
                            return true;
                        }
                    }
                    return false;

                case ActionsFicheCentre.ANUCL:
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || (utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) && prefectureUser == centre.RPrefecture))
                    {
                        if (centre.EtatAgrementCl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementCl.AGREMENT_OK)))
                        {
                            return true;
                        }
                    }
                    return false;

                case ActionsFicheCentre.CHDECOM:
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || (utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) && prefectureUser == centre.RPrefecture))
                    {
                        if (centre.EtatAgrementVl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementVl.AGREMENT_OK)) ||
                            centre.EtatAgrementVl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementVl.AGREMENT_DEMANDE_EN_COURS))
                        )
                        {
                            return true;
                        }

                        if (centre.EtatAgrementPl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_OK)) ||
                           centre.EtatAgrementPl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_DEMANDE_EN_COURS)) ||
                           centre.EtatAgrementPl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_RENOUVELEMENT_EN_COURS)) ||
                            centre.EtatAgrementPl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_RENOUVELE_OK))
                        )
                        {
                            return true;
                        }

                        if (centre.EtatAgrementCl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementCl.AGREMENT_OK)) ||
                          centre.EtatAgrementCl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementCl.AGREMENT_DEMANDE_EN_COURS))
                      )
                        {
                            return true;
                        }
                    }
                    return false;

                case ActionsFicheCentre.CHEXPLVL:
                    //ACTION "NOUVEL_AGREM" et motif de la fin d'agrément VL="2"// Changement d’exploitant
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || (utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) && prefectureUser == centre.RPrefecture))
                    {
                        if (centre.EtatAgrementVl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementVl.AGREMENT_KO)))
                        {
                            if (Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementVL.CHANGEMENT_EXPLOITANT).Equals(centre.MotifFinAgrementVl))
                                return true;
                        }
                    }
                    return false;

                case ActionsFicheCentre.CHLOCAVL:
                    //ACTION "NOUVEL_AGREM" et motif de la fin d'agrément VL="3"// Changement de localisation
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || (utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) && prefectureUser == centre.RPrefecture))
                    {
                        if (centre.EtatAgrementVl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementVl.AGREMENT_KO)))
                        {
                            if (Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementVL.CHANGEMENT_LOCALISATION).Equals(centre.MotifFinAgrementVl))
                                return true;
                        }
                    }
                    return false;

                case ActionsFicheCentre.CHRATTVL:
                    //ACTION "NOUVEL_AGREM" et motif de la fin d'agrément VL="4"// Changement de réseau de rattachement
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || (utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) && prefectureUser == centre.RPrefecture))
                    {
                        if (centre.EtatAgrementVl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementVl.AGREMENT_KO)))
                        {
                            if (Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementVL.CHANGEMENT_RESEAU).Equals(centre.MotifFinAgrementVl))
                                return true;
                        }
                    }
                    return false;

                case ActionsFicheCentre.NOUVEL_AGREM_VL:
                    //EG le 09/12/2011 : juste pour faire un combo pour éviter de tester les trois cas de figures dans la fiche du centre
                    //la fiche du centre ne doit pas avoir un duplicata
                    if (centreService.isAncienCentre(centre)) return false;

                    return isRightActionForCentre(utilisateurConnecte, centre, ActionsFicheCentre.CHEXPLVL) ||
                           isRightActionForCentre(utilisateurConnecte, centre, ActionsFicheCentre.CHLOCAVL) ||
                           isRightActionForCentre(utilisateurConnecte, centre, ActionsFicheCentre.CHRATTVL);

                case ActionsFicheCentre.CHEXPLPL:
                    //ACTION "NOUVEL_AGREM" et motif de la fin d'agrément VL="2"// Changement d’exploitant
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || (utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) && prefectureUser == centre.RPrefecture))
                    {
                        if (centre.EtatAgrementPl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_KO)))
                        {
                            if (Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementPL.CHANGEMENT_EXPLOITANT).Equals(centre.MotifFinAgrementPl))
                                return true;
                        }
                    }
                    return false;

                case ActionsFicheCentre.CHLOCAPL:
                    //ACTION "NOUVEL_AGREM" et motif de la fin d'agrément VL="3"// Changement d’exploitant
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || (utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) && prefectureUser == centre.RPrefecture))
                    {
                        if (centre.EtatAgrementPl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_KO)))
                        {
                            if (Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementPL.CHANGEMENT_LOCALISATION).Equals(centre.MotifFinAgrementPl))
                                return true;
                        }
                    }
                    return false;

                case ActionsFicheCentre.CHRATTPL:
                    //ACTION "NOUVEL_AGREM" et motif de la fin d'agrément VL="4"// Changement de réseau de rattachement
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || (utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) && prefectureUser == centre.RPrefecture))
                    {
                        if (centre.EtatAgrementPl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_KO)))
                        {
                            if (Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementPL.CHANGEMENT_RESEAU).Equals(centre.MotifFinAgrementPl))
                                return true;
                        }
                    }
                    return false;

                case ActionsFicheCentre.NOUVEL_AGREM_PL:
                    //EG le 09/12/2011 : juste pour faire un combo pour éviter de tester les trois cas de figures dans la fiche du centre
                    if (centreService.isAncienCentre(centre)) return false;

                    return isRightActionForCentre(utilisateurConnecte, centre, ActionsFicheCentre.CHEXPLPL) ||
                           isRightActionForCentre(utilisateurConnecte, centre, ActionsFicheCentre.CHLOCAPL) ||
                           isRightActionForCentre(utilisateurConnecte, centre, ActionsFicheCentre.CHRATTPL);

                case ActionsFicheCentre.CHEXPLCL:
                    //ACTION "NOUVEL_AGREM" et motif de la fin d'agrément VL="2"// Changement d’exploitant
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || (utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) && prefectureUser == centre.RPrefecture))
                    {
                        if (centre.EtatAgrementCl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementCl.AGREMENT_KO)))
                        {
                            if (Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementCL.CHANGEMENT_EXPLOITANT).Equals(centre.MotifFinAgrementCl))
                                return true;
                        }
                    }
                    return false;

                case ActionsFicheCentre.CHLOCACL:
                    //ACTION "NOUVEL_AGREM" et motif de la fin d'agrément VL="3"// Changement de localisation
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || (utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) && prefectureUser == centre.RPrefecture))
                    {
                        if (centre.EtatAgrementCl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementCl.AGREMENT_KO)))
                        {
                            if (Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementCL.CHANGEMENT_LOCALISATION).Equals(centre.MotifFinAgrementCl))
                                return true;
                        }
                    }
                    return false;

                case ActionsFicheCentre.CHRATTCL:
                    //ACTION "NOUVEL_AGREM" et motif de la fin d'agrément VL="4"// Changement de réseau de rattachement
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || (utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) && prefectureUser == centre.RPrefecture))
                    {
                        if (centre.EtatAgrementCl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementCl.AGREMENT_KO)))
                        {
                            if (Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementCL.CHANGEMENT_RESEAU).Equals(centre.MotifFinAgrementCl))
                                return true;
                        }
                    }
                    return false;

                case ActionsFicheCentre.NOUVEL_AGREM_CL:
                    //EG le 09/12/2011 : juste pour faire un combo pour éviter de tester les trois cas de figures dans la fiche du centre
                    if (centreService.isAncienCentre(centre)) return false;

                    return isRightActionForCentre(utilisateurConnecte, centre, ActionsFicheCentre.CHEXPLCL) ||
                           isRightActionForCentre(utilisateurConnecte, centre, ActionsFicheCentre.CHLOCACL) ||
                           isRightActionForCentre(utilisateurConnecte, centre, ActionsFicheCentre.CHRATTCL);

                case ActionsFicheCentre.CHREPLEG:
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || (utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) && prefectureUser == centre.RPrefecture))
                    {
                        if (
                           centre.EtatAgrementVl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementVl.AGREMENT_KO))
                           &&
                           (
                               Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementVL.CHANGEMENT_EXPLOITANT).Equals(centre.MotifFinAgrementVl) ||
                               Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementVL.CHANGEMENT_LOCALISATION).Equals(centre.MotifFinAgrementVl) ||
                               Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementVL.CHANGEMENT_RESEAU).Equals(centre.MotifFinAgrementVl)
                           )
                           && centreService.isAncienCentre(centre)
                       )
                        {
                            return false;
                        }

                        if (
                            centre.EtatAgrementPl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_KO))
                            &&
                            (
                                Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementPL.CHANGEMENT_EXPLOITANT).Equals(centre.MotifFinAgrementPl) ||
                                Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementPL.CHANGEMENT_LOCALISATION).Equals(centre.MotifFinAgrementPl) ||
                                Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementPL.CHANGEMENT_RESEAU).Equals(centre.MotifFinAgrementPl)
                            )
                            && centreService.isAncienCentre(centre)
                        )
                        {
                            return false;
                        }

                        if (
                            centre.EtatAgrementCl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementCl.AGREMENT_KO))
                            &&
                            (
                                Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementCL.CHANGEMENT_EXPLOITANT).Equals(centre.MotifFinAgrementCl) ||
                                Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementCL.CHANGEMENT_LOCALISATION).Equals(centre.MotifFinAgrementCl) ||
                                Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementCL.CHANGEMENT_RESEAU).Equals(centre.MotifFinAgrementCl)
                            )
                            && centreService.isAncienCentre(centre)
                        )
                        {
                            return false;
                        }

                        return true;

                    }
                    return false;

                case ActionsFicheCentre.CRECEVL:
                case ActionsFicheCentre.CRECEPL:
                case ActionsFicheCentre.CRECECL:
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()))
                    {
                        return true;
                    }
                    return false;

                case ActionsFicheCentre.DEAVL:
                    //EG le 12/12/2011 : prise en compte de la duplication d'une fiche centre
                    if (centreService.isAncienCentre(centre)) return false;

                    //EG le 29/11/2011 : prise en compte des profils autorisés
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || (utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) && prefectureUser == centre.RPrefecture))
                    {

                        if (
                                centre.EtatAgrementVl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementVl.AGREMENT_KO)) ||
                                centre.EtatAgrementVl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementVl.AGREMENT_REJETE))
                            )
                        {
                            if (centre.EtatAgrementVl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementVl.AGREMENT_KO)))
                            {
                                if (
                                    Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementVL.CHANGEMENT_EXPLOITANT).Equals(centre.MotifFinAgrementVl) ||
                                    Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementVL.CHANGEMENT_LOCALISATION).Equals(centre.MotifFinAgrementVl) ||
                                    Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementVL.CHANGEMENT_RESEAU).Equals(centre.MotifFinAgrementVl) ||
                                     //05/08/2014 - PLE - autres conditions pour empécher le nouvel agrément : si pas d'agrément PL
                                     (
                                     (Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementVL.FERMETURE_DEFINITIVE).Equals(centre.MotifFinAgrementVl) ||
                                     Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementVL.RETRAIT_SANCTION).Equals(centre.MotifFinAgrementVl))
                                     &&
                                     !centre.EtatAgrementPl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_OK))
                                     )
                                    )
                                {
                                    return false;
                                }
                            }
                            return true;
                        }
                    }

                    return false;

                case ActionsFicheCentre.DEAPL:
                    //EG le 12/12/2011 : prise en compte de la duplication d'une fiche centre
                    if (centreService.isAncienCentre(centre)) return false;

                    //EG le 29/11/2011 : prise en compte des profils autorisés
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || (utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) && prefectureUser == centre.RPrefecture))
                    {
                        if (
                                centre.EtatAgrementPl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_KO)) ||
                                centre.EtatAgrementPl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_REJETE)) ||
                                centre.EtatAgrementPl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_RENOUVELE_REJETE))
                            )
                        {
                            if (centre.EtatAgrementPl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_KO)))
                            {
                                if (
                                    Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementPL.CHANGEMENT_EXPLOITANT).Equals(centre.MotifFinAgrementPl) ||
                                    Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementPL.CHANGEMENT_LOCALISATION).Equals(centre.MotifFinAgrementPl) ||
                                    Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementPL.CHANGEMENT_RESEAU).Equals(centre.MotifFinAgrementPl) ||
                                    //05/08/2014 - PLE - autres conditions pour empécher le nouvel agrément : si pas d'agrément VL
                                    (
                                     (Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementPL.FERMETURE_DEFINITIVE).Equals(centre.MotifFinAgrementPl) ||
                                     Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementPL.RETRAIT_SANCTION).Equals(centre.MotifFinAgrementPl))
                                     &&
                                     !centre.EtatAgrementVl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementVl.AGREMENT_OK))
                                     )
                                    )
                                {
                                    return false;
                                }
                            }
                            return true;
                        }
                    }
                    return false;

                case ActionsFicheCentre.DEACL:
                    //EG le 12/12/2011 : prise en compte de la duplication d'une fiche centre
                    if (centreService.isAncienCentre(centre)) return false;

                    //EG le 29/11/2011 : prise en compte des profils autorisés
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || (utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) && prefectureUser == centre.RPrefecture))
                    {

                        if (
                                centre.EtatAgrementCl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementCl.AGREMENT_KO)) ||
                                centre.EtatAgrementCl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementCl.AGREMENT_REJETE))
                            )
                        {
                            if (centre.EtatAgrementCl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementCl.AGREMENT_KO)))
                            {
                                if (Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementCL.CHANGEMENT_EXPLOITANT).Equals(centre.MotifFinAgrementCl) ||
                                    Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementCL.CHANGEMENT_LOCALISATION).Equals(centre.MotifFinAgrementCl) ||
                                    Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementCL.CHANGEMENT_RESEAU).Equals(centre.MotifFinAgrementCl) ||
                                    //05/08/2014 - PLE - autres conditions pour empécher le nouvel agrément : si pas d'agrément VL et PL
                                    (
                                     (Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementCL.FERMETURE_DEFINITIVE).Equals(centre.MotifFinAgrementCl) ||
                                     Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementCL.RETRAIT_SANCTION).Equals(centre.MotifFinAgrementCl))
                                     &&
                                     (!centre.EtatAgrementVl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementVl.AGREMENT_OK)) || !centre.EtatAgrementPl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_OK)))
                                     )
                                    )
                                {
                                    return false;
                                }
                            }
                            return true;
                        }
                    }
                    return false;

                case ActionsFicheCentre.DERPL:
                    if (centre.TypeCentre == pCentreEncour.TypeInstallation.A.ToString())
                    {
                        if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || (utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) && prefectureUser == centre.RPrefecture))
                        {
                            if (centre.EtatAgrementPl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_OK)) ||
                               centre.EtatAgrementPl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_RENOUVELE_OK))
                            )
                            {
                                return true;
                            }

                        }
                    }

                    return false;

                case ActionsFicheCentre.MODAUTRE:
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || (utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) && prefectureUser == centre.RPrefecture))
                    {
                        return true;
                    }
                    return false;

                case ActionsFicheCentre.NOTVL:
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || (utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) && prefectureUser == centre.RPrefecture))
                    {
                        if (centre.EtatAgrementVl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementVl.AGREMENT_DEMANDE_EN_COURS)))
                        {
                            return true;
                        }
                    }
                    return false;

                case ActionsFicheCentre.NOTPL:
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || (utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) && prefectureUser == centre.RPrefecture))
                    {
                        if (centre.EtatAgrementPl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_DEMANDE_EN_COURS)))
                        {
                            return true;
                        }
                    }
                    return false;

                case ActionsFicheCentre.NOTCL:
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || (utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) && prefectureUser == centre.RPrefecture))
                    {
                        if (centre.EtatAgrementCl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementCl.AGREMENT_DEMANDE_EN_COURS)))
                        {
                            return true;
                        }
                    }
                    return false;

                case ActionsFicheCentre.NOTRPL:
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || (utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) && prefectureUser == centre.RPrefecture))
                    {
                        if (centre.EtatAgrementPl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_RENOUVELEMENT_EN_COURS)))
                        {
                            return true;
                        }
                    }
                    return false;

                case ActionsFicheCentre.REJVL:
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || (utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) && prefectureUser == centre.RPrefecture))
                    {
                        if (centre.EtatAgrementVl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementVl.AGREMENT_DEMANDE_EN_COURS)))
                        {
                            return true;
                        }
                    }
                    return false;

                case ActionsFicheCentre.REJPL:
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || (utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) && prefectureUser == centre.RPrefecture))
                    {
                        if (centre.EtatAgrementPl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_DEMANDE_EN_COURS)))
                        {
                            return true;
                        }
                    }
                    return false;

                case ActionsFicheCentre.REJRPL:
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || (utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) && prefectureUser == centre.RPrefecture))
                    {
                        if (centre.EtatAgrementPl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_RENOUVELEMENT_EN_COURS)))
                        {
                            return true;
                        }
                    }
                    return false;

                case ActionsFicheCentre.REJCL:
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || (utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) && prefectureUser == centre.RPrefecture))
                    {
                        if (centre.EtatAgrementCl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementCl.AGREMENT_DEMANDE_EN_COURS)))
                        {
                            return true;
                        }
                    }
                    return false;

                case ActionsFicheCentre.RETOUR:

                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()))
                    {
                        if (
                           centre.RMotifModification.Id.Equals(rMotifModification.TypeModification.CRECEVL.ToString()) ||
                           centre.RMotifModification.Id.Equals(rMotifModification.TypeModification.CRECEPL.ToString()) ||
                           centre.RMotifModification.Id.Equals(rMotifModification.TypeModification.CRECECL.ToString())
                           )
                        {
                            return false;
                        }

                        return true;
                    }
                    if ((utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) && prefectureUser == centre.RPrefecture))
                    {
                        //SI La dernière action appartient à la liste suivante : CRECEVL, CRECEPL, CRECECL,*ADMMODFI*, RETOUR, SANCT, AMEND, CHHORAI, RDV		
                        if (
                            centre.RMotifModification.Id.Equals(rMotifModification.TypeModification.ADMMODFI.ToString()) ||
                            centre.RMotifModification.Id.Equals(rMotifModification.TypeModification.CRECEVL.ToString()) ||
                            centre.RMotifModification.Id.Equals(rMotifModification.TypeModification.CRECEPL.ToString()) ||
                            centre.RMotifModification.Id.Equals(rMotifModification.TypeModification.CRECECL.ToString()) ||
                            centre.RMotifModification.Id.Equals(rMotifModification.TypeModification.RETOUR.ToString()) ||
                            centre.RMotifModification.Id.Equals(rMotifModification.TypeModification.SANCT.ToString()) ||
                            centre.RMotifModification.Id.Equals(rMotifModification.TypeModification.AMEND.ToString()) ||
                            centre.RMotifModification.Id.Equals(rMotifModification.TypeModification.CHHORAI.ToString()) ||
                            centre.RMotifModification.Id.Equals(rMotifModification.TypeModification.RDV.ToString()) ||
                            centre.RMotifModification.Id.Equals(rMotifModification.TypeModification.EQUIPCL.ToString()) ||
                            centre.RMotifModification.Id.Equals(rMotifModification.TypeModification.EQUIPPL.ToString()) ||
                            centre.RMotifModification.Id.Equals(rMotifModification.TypeModification.EQUIPVL.ToString())
                            )
                        {
                            return false;
                        }

                        return true;
                    }

                    return false;

                case ActionsFicheCentre.SANCT:
                    if (centre.AunAgrementValide)
                    {
                        if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()))
                        {
                            return true;
                        }
                        rUtilisateur u = utilisateurService.FindBy(utilisateurConnecte.Identity.Name);
                        if (centre.RPrefecture.Equals(u.RPrefecture))
                        {
                            return true;
                        }
                    }
                    return false;

                //if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()))
                //{
                //    rUtilisateur u = utilisateurService.FindBy(utilisateurConnecte.Identity.Name);

                //    if (centre.EtatAgrementVl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementVl.AGREMENT_OK)))
                //    {
                //        if (centre.RPrefecture.Equals(u.RPrefecture))
                //        {
                //            return true;
                //        }
                //        else
                //        {
                //            return false;
                //        }
                //    }

                //    if (
                //        centre.EtatAgrementPl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_OK)) ||
                //        centre.EtatAgrementPl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_RENOUVELE_OK))
                //       )
                //    {
                //        if (centre.RPrefecture.Equals(u.RPrefecture))
                //        {
                //            return true;
                //        }
                //        else
                //        {
                //            return false;
                //        }
                //    }

                //    if (centre.EtatAgrementCl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementCl.AGREMENT_OK)))
                //    {
                //        if (centre.RPrefecture.Equals(u.RPrefecture))
                //        {
                //            return true;
                //        }
                //        else
                //        {
                //            return false;
                //        }
                //    }
                //}
                //return false;

                case ActionsFicheCentre.SHOW_SANCT:
                    if (
                        utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) ||
                        utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) ||
                        utilisateurConnecte.IsInRole(rUtilisateur.Profil.DRIRE.ToString()) ||
                        utilisateurConnecte.IsInRole(rUtilisateur.Profil.DSCR.ToString()) ||
                        utilisateurConnecte.IsInRole(rUtilisateur.Profil.PIV.ToString())
                        )
                    {
                        //if (centre.EtatAgrementVl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementVl.AGREMENT_OK)))
                        //{
                        //    return true;
                        //}

                        //if (
                        //    centre.EtatAgrementPl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_OK)) ||
                        //    centre.EtatAgrementPl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_RENOUVELE_OK))
                        //   )
                        //{
                        //    return true;
                        //}

                        //if (centre.EtatAgrementCl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementCl.AGREMENT_OK)))
                        //{
                        return true;
                        //}
                    }
                    return false;

                case ActionsFicheCentre.DEROG_VL:
                    if (centre.EstAgrementValideVL)
                    {
                        if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()))
                        {
                            return true;
                        }
                        rUtilisateur u = utilisateurService.FindBy(utilisateurConnecte.Identity.Name);
                        //Profil prefecture
                        if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()))
                        {
                            if (centre.RPrefecture.Equals(u.RPrefecture))
                            {
                                return true;
                            }
                        }
                        //Profil DREAL
                        if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.DRIRE.ToString()))
                        {
                            if (centre.RPrefecture.RRegion.Equals(u.RRegion))
                            {
                                return true;
                            }
                        }
                    }
                    return false;

                case ActionsFicheCentre.DEROG_PL:
                    if (centre.EstAgrementValidePL)
                    {
                        if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()))
                        {
                            return true;
                        }
                        rUtilisateur u = utilisateurService.FindBy(utilisateurConnecte.Identity.Name);
                        //Profil prefecture
                        if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()))
                        {
                            if (centre.RPrefecture.Equals(u.RPrefecture))
                            {
                                return true;
                            }
                        }
                        //Profil DREAL
                        if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.DRIRE.ToString()))
                        {
                            if (centre.RPrefecture.RRegion.Equals(u.RRegion))
                            {
                                return true;
                            }
                        }
                    }
                    return false;

                //MODIF DEROG
                case ActionsFicheCentre.MOD_DEROG_VL:
                    //Pour admin tout le temps
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()))
                    {
                        return true;
                    }

                    if (centre.EstAgrementValideVL)
                    {

                        rUtilisateur u = utilisateurService.FindBy(utilisateurConnecte.Identity.Name);
                        //Profil prefecture
                        if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()))
                        {
                            if (centre.RPrefecture.Equals(u.RPrefecture))
                            {
                                return true;
                            }
                        }
                        //Profil DREAL
                        if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.DRIRE.ToString()))
                        {
                            if (centre.RPrefecture.RRegion.Equals(u.RRegion))
                            {
                                return true;
                            }
                        }
                    }
                    return false;

                case ActionsFicheCentre.MOD_DEROG_PL:
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()))
                    {
                        return true;
                    }

                    if (centre.EstAgrementValidePL)
                    {
                        rUtilisateur u = utilisateurService.FindBy(utilisateurConnecte.Identity.Name);
                        //Profil prefecture
                        if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()))
                        {
                            if (centre.RPrefecture.Equals(u.RPrefecture))
                            {
                                return true;
                            }
                        }
                        //Profil DREAL
                        if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.DRIRE.ToString()))
                        {
                            if (centre.RPrefecture.RRegion.Equals(u.RRegion))
                            {
                                return true;
                            }
                        }
                    }
                    return false;

                case ActionsFicheCentre.SHOW_DEROG:
                    if (
                        utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) ||
                        utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) ||
                        utilisateurConnecte.IsInRole(rUtilisateur.Profil.DRIRE.ToString())
                        )
                    {
                        return true;
                    }
                    return false;

                case ActionsFicheCentre.EQUIPVL:
                    return isRightForCentreAppartenanceAndEtatAgrementCommonVL(utilisateurConnecte, centre);

                case ActionsFicheCentre.EQUIPPL:
                    return isRightForCentreAppartenanceAndEtatAgrementCommonPL(utilisateurConnecte, centre);

                case ActionsFicheCentre.EQUIPCL:
                    return isRightForCentreAppartenanceAndEtatAgrementCommonCL(utilisateurConnecte, centre);

                case ActionsFicheCentre.SHOW_EQUIPVL:
                    //PLE 27/03/2012 
                    if (isRightForCentreAppartenanceAndEtatAgrementCommonForConsultVL(utilisateurConnecte, centre))
                    {
                        return true;
                    }
                    else
                    {
                        if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.DRIRE.ToString()) || utilisateurConnecte.IsInRole(rUtilisateur.Profil.DSCR.ToString()))
                        {
                            return true;
                        }
                    }
                    return false;

                case ActionsFicheCentre.SHOW_EQUIPPL:
                    //PLE 27/03/2012 
                    if (isRightForCentreAppartenanceAndEtatAgrementCommonForConsultPL(utilisateurConnecte, centre))
                    {
                        return true;
                    }
                    else
                    {
                        if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.DRIRE.ToString()) || utilisateurConnecte.IsInRole(rUtilisateur.Profil.DSCR.ToString()))
                        {
                            return true;
                        }
                    }
                    return false;

                case ActionsFicheCentre.SHOW_EQUIPCL:
                    //PLE 27/03/2012 
                    if (isRightForCentreAppartenanceAndEtatAgrementCommonForConsultCL(utilisateurConnecte, centre))
                    {
                        return true;
                    }
                    else
                    {
                        if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.DRIRE.ToString()) || utilisateurConnecte.IsInRole(rUtilisateur.Profil.DSCR.ToString()))
                        {
                            return true;
                        }
                    }
                    return false;

                //EG le 01/12/2011
                case ActionsFicheCentre.TELECHARGE_CERTIF_SSL_VL:
                    //que pour un centre indépendant
                    if (centre.ReseauRattachementVL != null && rReseau.IdCodeReseau.Z.ToString().Equals(centre.ReseauRattachementVL.Id))
                    {
                        if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Informatique.ToString()))
                        {
                            return true;
                        }

                        //CENTRE INDEPENDANT
                        //06/06/2013 PLE Interdiction de télécharger un certificat pour une centre non valide
                        if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.CENTRE_INDEPENDANT.ToString()) && centre.EstAgrementValideVL)
                        {
                            rUtilisateur u = utilisateurService.FindBy(utilisateurConnecte.Identity.Name);
                            if (centre.Equals(u.PCentreEncourVL))
                            {
                                return true;
                            }
                        }
                    }

                    return false;

                //EG le 01/12/2011
                case ActionsFicheCentre.TELECHARGE_CERTIF_SSL_PL:
                    //que pour un centre indépendant
                    if (centre.ReseauRattachementPL != null && rReseau.IdCodeReseau.Z.ToString().Equals(centre.ReseauRattachementPL.Id))
                    {
                        if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Informatique.ToString()))
                        {
                            return true;
                        }

                        //CENTRE INDEPENDANT
                        //06/06/2013 PLE Interdiction de télécharger un certificat pour une centre non valide
                        if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.CENTRE_INDEPENDANT.ToString()) && centre.EstAgrementValidePL)
                        {
                            rUtilisateur u = utilisateurService.FindBy(utilisateurConnecte.Identity.Name);
                            if (centre.Equals(u.PCentreEncourPL))
                            {
                                return true;
                            }
                        }
                    }

                    return false;

                //EG le 01/12/2011
                case ActionsFicheCentre.TELECHARGE_CERTIF_SSL_CL:
                    //que pour un centre indépendant
                    if (centre.EstAgrementValideCL && rReseau.IdCodeReseau.Z.ToString().Equals(centre.ReseauRattachementCL.Id))
                    {
                        if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()))
                        {
                            return true;
                        }

                        //CENTRE INDEPENDANT
                        //06/11/2026 PLE Interdiction de télécharger un certificat pour une centre non valide
                        if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.CENTRE_INDEPENDANT.ToString()) && centre.EstAgrementValideCL)
                        {
                            rUtilisateur u = utilisateurService.FindBy(utilisateurConnecte.Identity.Name);
                            if (centre.Equals(u.PCentreEncourCL))
                            {
                                return true;
                            }
                        }
                    }

                    return false;

                case ActionsFicheCentre.REVOCATION_CERTIF:

                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()))
                    {
                        return true;
                    }

                    return false;

                //EG le 05/12/2011
                case ActionsFicheCentre.TELECHARGE_CE_PL:
                    //Seulement les centres indépendants
                    if (
                         //PLE 04/12/2013 PLE Ajout de OTC_consultation
                         (centre.CodAgremPl != null) &&
                         (
                            utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) ||
                            utilisateurConnecte.IsInRole(rUtilisateur.Profil.DRIRE.ToString()) ||
                            utilisateurConnecte.IsInRole(rUtilisateur.Profil.DSCR.ToString()) ||
                            utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Consultation.ToString())
                         )
                        )
                    {
                        return true;
                    }
                    else
                    {
                        //if (centre.EstAgrementValidePL && "Z".Equals(centre.ReseauRattachementPL.Id))
                        //YM 23/05/2013 proposer le téléchargement même si le centre est fermé (fiche 125)
                        if (centre.CodAgremPl != null && "Z".Equals(centre.ReseauRattachementPL.Id))
                        {
                            if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.CENTRE_INDEPENDANT.ToString()))
                            {
                                //rUtilisateur u = utilisateurService.FindBy(utilisateurConnecte.Identity.Name);
                                if (centre.Equals(utilisateur.PCentreEncourPL))
                                {
                                    return true;
                                }
                            }
                        }
                        else
                        //if (centre.EstAgrementValidePL && !"Z".Equals(centre.ReseauRattachementPL.Id))
                        //YM 23/05/2013 proposer le téléchargement même si le centre est fermé (fiche 125)
                        if (centre.CodAgremPl != null && !"Z".Equals(centre.ReseauRattachementPL.Id))
                        {
                            if (
                                utilisateurConnecte.IsInRole(rUtilisateur.Profil.RESEAU_Gestionnaire.ToString()) ||
                                utilisateurConnecte.IsInRole(rUtilisateur.Profil.RESEAU_Administrateur.ToString()) ||
                                utilisateurConnecte.IsInRole(rUtilisateur.Profil.RESEAU_Consultation.ToString())
                                )
                            {

                                bool autorise = false;
                                foreach (rReseau r in utilisateur.RReseaux)
                                {
                                    if (r.Id.Equals(centre.ReseauRattachementPL.Id))
                                    {
                                        autorise = true;
                                        break;
                                    }
                                }

                                return autorise;
                            }
                        }
                    }

                    return false;

                case ActionsFicheCentre.TELECHARGE_CE_VL:
                    //if (centre.EstAgrementValideVL)
                    if (centre.CodAgremVl != null)
                    {
                        if (
                             //PLE 04/12/2013 PLE Ajout de OTC_consultation
                             utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) ||
                             utilisateurConnecte.IsInRole(rUtilisateur.Profil.DRIRE.ToString()) ||
                             utilisateurConnecte.IsInRole(rUtilisateur.Profil.DSCR.ToString()) ||
                             utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Consultation.ToString())
                            )
                        {
                            return true;
                        }
                        else
                        {
                            if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.CENTRE_INDEPENDANT.ToString()))
                            {
                                //rUtilisateur u = utilisateurService.FindBy(utilisateurConnecte.Identity.Name);
                                if (centre.Equals(utilisateur.PCentreEncourVL))
                                {
                                    return true;
                                }
                            }

                            if (
                                utilisateurConnecte.IsInRole(rUtilisateur.Profil.RESEAU_Gestionnaire.ToString()) ||
                                utilisateurConnecte.IsInRole(rUtilisateur.Profil.RESEAU_Administrateur.ToString()) ||
                                utilisateurConnecte.IsInRole(rUtilisateur.Profil.RESEAU_Consultation.ToString())
                                )
                            {
                                //rUtilisateur u = utilisateurService.FindBy(utilisateurConnecte.Identity.Name);

                                bool autorise = false;
                                foreach (rReseau r in utilisateur.RReseaux)
                                {
                                    if (r.Id.Equals(centre.ReseauRattachementVL.Id))
                                    {
                                        autorise = true;
                                        break;
                                    }
                                }

                                return autorise;
                            }
                        }

                    }
                    return false;

                case ActionsFicheCentre.TELECHARGE_CE_CL:
                    //if (centre.EstAgrementValideVL)
                    if (centre.CodAgremCl != null)
                    {
                        if (
                             //PLE 04/12/2013 PLE Ajout de OTC_consultation
                             utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) ||
                             utilisateurConnecte.IsInRole(rUtilisateur.Profil.DRIRE.ToString()) ||
                             utilisateurConnecte.IsInRole(rUtilisateur.Profil.DSCR.ToString()) ||
                             utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Consultation.ToString())
                            )
                        {
                            return true;
                        }
                        else
                        {
                            if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.CENTRE_INDEPENDANT.ToString()))
                            {
                                //rUtilisateur u = utilisateurService.FindBy(utilisateurConnecte.Identity.Name);
                                if (centre.Equals(utilisateur.PCentreEncourCL))
                                {
                                    return true;
                                }
                            }

                            if (
                                utilisateurConnecte.IsInRole(rUtilisateur.Profil.RESEAU_Gestionnaire.ToString()) ||
                                utilisateurConnecte.IsInRole(rUtilisateur.Profil.RESEAU_Administrateur.ToString()) ||
                                utilisateurConnecte.IsInRole(rUtilisateur.Profil.RESEAU_Consultation.ToString())
                                )
                            {
                                //rUtilisateur u = utilisateurService.FindBy(utilisateurConnecte.Identity.Name);

                                bool autorise = false;
                                foreach (rReseau r in utilisateur.RReseaux)
                                {
                                    if (r.Id.Equals(centre.ReseauRattachementCL.Id))
                                    {
                                        autorise = true;
                                        break;
                                    }
                                }

                                return autorise;
                            }
                        }

                    }
                    return false;

                case ActionsFicheCentre.SHOW_HISTO:
                    //EG le 22/02/2012 : accès à l'historique dans tous les cas
                    if (
                        utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) ||
                        utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) ||
                        utilisateurConnecte.IsInRole(rUtilisateur.Profil.DRIRE.ToString()) ||
                        utilisateurConnecte.IsInRole(rUtilisateur.Profil.DSCR.ToString()) ||
                        utilisateurConnecte.IsInRole(rUtilisateur.Profil.PIV.ToString())
                        )
                    {
                        //if (centre.AunAgrementValide)
                        //{
                        return true;
                        //}
                    }

                    return false;

                case ActionsFicheCentre.SHOW_AMEND:
                    if (
                        utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) ||
                        utilisateurConnecte.IsInRole(rUtilisateur.Profil.DRIRE.ToString()) ||
                        utilisateurConnecte.IsInRole(rUtilisateur.Profil.DSCR.ToString()) ||
                        utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) ||
                        utilisateurConnecte.IsInRole(rUtilisateur.Profil.PIV.ToString())
                        )
                    {
                        if (centre.AunAgrementValide)
                        {
                            return true;
                        }
                    }

                    return false;

                case ActionsFicheCentre.SHOW_CONTROLEUR:
                    //Tout le monde
                    return true;

                case ActionsFicheCentre.RDV:
                    //EG le 14/12/2011 : seulement pour les centres indépendants , pour les réseaux cette action ne sera pas vérifiée
                    //return isRightForCentreAppartenanceAndEtatAgrementCommonPL(utilisateurConnecte, centre) && rReseau.IdCodeReseau.Z.ToString().Equals(centre.ReseauRattachementPL.Id);
                    return isRightForCentreAppartenanceAndEtatAgrementCommonPL(utilisateurConnecte, centre);

                case ActionsFicheCentre.SHOW_RDV:
                    if (isRightForCentreAppartenanceAndEtatAgrementCommonForConsultPL(utilisateurConnecte, centre))
                    {
                        return true;
                    }
                    if (centre.EstAgrementValidePL)
                    {
                        if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.DRIRE.ToString()) ||
                            utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) ||
                            utilisateurConnecte.IsInRole(rUtilisateur.Profil.DSCR.ToString()) ||
                            utilisateurConnecte.IsInRole(rUtilisateur.Profil.PIV.ToString())
                            )
                        {
                            return true;
                        }
                    }

                    return false;
                //EG le 14/12/2011
                case ActionsFicheCentre.CHHORAI:
                    return isRightForCentreAppartenanceAndEtatAgrementCommonPL(utilisateurConnecte, centre) && pCentreEncour.TypeInstallation.A.ToString().Equals(centre.TypeCentre);

                case ActionsFicheCentre.SHOW_HORAIRE_OUVERTURE:
                    if (isRightForCentreAppartenanceAndEtatAgrementCommonPL(utilisateurConnecte, centre) && pCentreEncour.TypeInstallation.A.ToString().Equals(centre.TypeCentre))
                    {
                        return true;
                    }
                    if (centre.EstAgrementValidePL && pCentreEncour.TypeInstallation.A.ToString().Equals(centre.TypeCentre))
                    {
                        if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.DRIRE.ToString()) ||
                            utilisateurConnecte.IsInRole(rUtilisateur.Profil.DSCR.ToString()) ||
                            utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) ||
                            utilisateurConnecte.IsInRole(rUtilisateur.Profil.RESEAU_Consultation.ToString()) ||
                            utilisateurConnecte.IsInRole(rUtilisateur.Profil.PIV.ToString())
                           )
                        {
                            return true;
                        }
                    }

                    return false;

                //EG le 19/12/2011
                case ActionsFicheCentre.JOURNAL:
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()))
                    {
                        return true;
                    }
                    return false;

                //PLE le 19/03/2012
                case ActionsFicheCentre.TELECHARGE_FICHE_CENTRE:
                    if (
                        utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) ||
                        utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) ||
                        utilisateurConnecte.IsInRole(rUtilisateur.Profil.DRIRE.ToString()) ||
                        utilisateurConnecte.IsInRole(rUtilisateur.Profil.DSCR.ToString()) ||
                        utilisateurConnecte.IsInRole(rUtilisateur.Profil.PIV.ToString())
                        )
                    {
                        return true;
                    }
                    return false;

                case ActionsFicheCentre.TELECHARGE_FACTURE_VL:
                    if (centre.CodAgremVl != null)
                    {
                        if (
                             utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) ||
                             utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Consultation.ToString())
                            )
                        {
                            return true;
                        }
                        else
                        {
                            if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.CENTRE_INDEPENDANT.ToString()))
                            {
                                if (centre.Equals(utilisateur.PCentreEncourVL))
                                {
                                    return true;
                                }
                            }
                        }

                    }
                    return false;

                case ActionsFicheCentre.TELECHARGE_FACTURE_PL:
                    if (centre.CodAgremPl != null)
                    {
                        if (
                             utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) ||
                             utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Consultation.ToString())
                            )
                        {
                            return true;
                        }
                        else
                        {
                            if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.CENTRE_INDEPENDANT.ToString()))
                            {
                                if (centre.Equals(utilisateur.PCentreEncourPL))
                                {
                                    return true;
                                }
                            }
                        }

                    }
                    return false;

                case ActionsFicheCentre.TELECHARGE_FACTURE_CL:
                    if (centre.CodAgremCl != null)
                    {
                        if (
                             utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) ||
                             utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Consultation.ToString())
                            )
                        {
                            return true;
                        }
                        else
                        {
                            if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.CENTRE_INDEPENDANT.ToString()))
                            {
                                if (centre.Equals(utilisateur.PCentreEncourCL))
                                {
                                    return true;
                                }
                            }
                        }

                    }
                    return false;

                case ActionsFicheCentre.EXTDECLA_VL:
                case ActionsFicheCentre.EXTRETRA_VL:
                    //if (centre.EstAgrementValideVL)
                    if (centre.CodAgremVl != null && centre.EstAgrementValideVL)
                    {
                        if (
                            (action.Equals(ActionsFicheCentre.EXTDECLA_VL) &&
                            (((short?)rCategorieVehicule.ExtCatvehEnum.VL).Equals(centre.ExtAgremClCatVeh) || centre.ExtAgremClCatVeh == null) &&
                            (((short?)0).Equals(centre.ExtAgremClStatut) || centre.ExtAgremClStatut == null))
                            ||
                            (action.Equals(ActionsFicheCentre.EXTRETRA_VL) &&
                            (((short?)rCategorieVehicule.ExtCatvehEnum.VL).Equals(centre.ExtAgremClCatVeh) || centre.ExtAgremClCatVeh == null) &&
                            (((short?)1).Equals(centre.ExtAgremClStatut)))
                           )

                            if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()))
                            {
                                return true;
                            }
                            else
                            {
                                //PLE le 14/04/2025 : Supression de la possiblité de déclarer une extension d'agrément à partir du 15 avril 2025
                                if (action.Equals(ActionsFicheCentre.EXTDECLA_VL))
                                {
                                    return false;
                                }
                                if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.CENTRE_INDEPENDANT.ToString()))
                                {
                                    //rUtilisateur u = utilisateurService.FindBy(utilisateurConnecte.Identity.Name);
                                    if (centre.Equals(utilisateur.PCentreEncourVL))
                                    {
                                        return true;
                                    }
                                }

                                if (
                                utilisateurConnecte.IsInRole(rUtilisateur.Profil.RESEAU_Gestionnaire.ToString()) ||
                                utilisateurConnecte.IsInRole(rUtilisateur.Profil.RESEAU_Administrateur.ToString())
                                )
                                {
                                    //rUtilisateur u = utilisateurService.FindBy(utilisateurConnecte.Identity.Name);

                                    bool autorise = false;
                                    foreach (rReseau r in utilisateur.RReseaux)
                                    {
                                        if (r.Id.Equals(centre.ReseauRattachementVL.Id))
                                        {
                                            autorise = true;
                                            break;
                                        }
                                    }

                                    return autorise;
                                }
                            }
                    }
                    return false;

                case ActionsFicheCentre.EXTDECLA_PL:
                case ActionsFicheCentre.EXTRETRA_PL:
                    //if (centre.EstAgrementValideVL)
                    if (centre.CodAgremPl != null && centre.EstAgrementValidePL)
                    {
                        if (
                            (action.Equals(ActionsFicheCentre.EXTDECLA_PL) &&
                            (((short?)rCategorieVehicule.ExtCatvehEnum.PL).Equals(centre.ExtAgremClCatVeh) || centre.ExtAgremClCatVeh == null) &&
                            (((short?)0).Equals(centre.ExtAgremClStatut) || centre.ExtAgremClStatut == null))
                            ||
                            (action.Equals(ActionsFicheCentre.EXTRETRA_PL) &&
                            (((short?)rCategorieVehicule.ExtCatvehEnum.PL).Equals(centre.ExtAgremClCatVeh) || centre.ExtAgremClCatVeh == null) &&
                            (((short?)1).Equals(centre.ExtAgremClStatut)))
                           )

                            if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()))
                            {
                                return true;
                            }
                            else
                            {
                                //PLE le 14/04/2025 : Supression de la possiblité de déclarer une extension d'agrément à partir du 15 avril 2025
                                if (action.Equals(ActionsFicheCentre.EXTDECLA_PL))
                                {
                                    return false;
                                }
                                if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.CENTRE_INDEPENDANT.ToString()))
                                {
                                    //rUtilisateur u = utilisateurService.FindBy(utilisateurConnecte.Identity.Name);
                                    if (centre.Equals(utilisateur.PCentreEncourPL))
                                    {
                                        return true;
                                    }
                                }

                                if (
                                    utilisateurConnecte.IsInRole(rUtilisateur.Profil.RESEAU_Gestionnaire.ToString()) ||
                                    utilisateurConnecte.IsInRole(rUtilisateur.Profil.RESEAU_Administrateur.ToString())
                                    )
                                {
                                    //rUtilisateur u = utilisateurService.FindBy(utilisateurConnecte.Identity.Name);

                                    bool autorise = false;
                                    foreach (rReseau r in utilisateur.RReseaux)
                                    {
                                        if (r.Id.Equals(centre.ReseauRattachementPL.Id))
                                        {
                                            autorise = true;
                                            break;
                                        }
                                    }

                                    return autorise;
                                }
                            }

                    }
                    return false;

                default:
                    return false;
            }

        }

        //EG le 29/11/2011 : droits commun à plusieurs actions
        private static bool isRightForCentreAppartenanceAndEtatAgrementCommonVL(IPrincipal utilisateurConnecte, pCentreEncour centre)
        {
            if (centre.EstAgrementValideVL)
            {
                if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()))
                {
                    return true;
                }
                else
                {
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.RESEAU_Gestionnaire.ToString()) ||
                        utilisateurConnecte.IsInRole(rUtilisateur.Profil.RESEAU_Administrateur.ToString()) ||
                        utilisateurConnecte.IsInRole(rUtilisateur.Profil.CENTRE_INDEPENDANT.ToString()))
                    {

                        //PLE : 27/03/2012 factorisation de la recherche
                        //rUtilisateur u = utilisateurService.FindBy(utilisateurConnecte.Identity.Name);
                        //bool autorise = false;
                        //foreach (rReseau r in u.RReseaux)
                        //{
                        //    if (r.Id.Equals(centre.ReseauRattachementVL.Id))
                        //    {
                        //        autorise = true;
                        //        break;
                        //    }
                        //}
                        //return autorise;

                        return isUtilisateurProprietaireCentre(utilisateurConnecte, centre, rCategorieVehicule.CategorieEnum.VL);
                    }

                    //else
                    //{
                    //    //CENTRE INDEPENDANT
                    //    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.CENTRE_INDEPENDANT.ToString()))
                    //    {
                    //        rUtilisateur u = utilisateurService.FindBy(utilisateurConnecte.Identity.Name);
                    //        if (centre.Equals(u.PCentreEncourVL))
                    //        {
                    //            return true;
                    //        }
                    //    }
                    //}

                }
            }

            return false;
        }

        //PLE le 27/03/2012 : même droits que isRightForCentreAppartenanceAndEtatAgrementCommonVL + les droits de visu
        private static bool isRightForCentreAppartenanceAndEtatAgrementCommonForConsultVL(IPrincipal utilisateurConnecte, pCentreEncour centre)
        {
            if (centre.EstAgrementValideVL)
            {
                if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()))
                {
                    return true;
                }
                else
                {
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.RESEAU_Gestionnaire.ToString()) ||
                        utilisateurConnecte.IsInRole(rUtilisateur.Profil.RESEAU_Administrateur.ToString()) ||
                        utilisateurConnecte.IsInRole(rUtilisateur.Profil.RESEAU_Consultation.ToString()) ||
                        utilisateurConnecte.IsInRole(rUtilisateur.Profil.CENTRE_INDEPENDANT.ToString()))
                    {
                        return isUtilisateurProprietaireCentre(utilisateurConnecte, centre, rCategorieVehicule.CategorieEnum.VL);
                    }
                }
            }
            return false;
        }

        private static bool isRightForCentreAppartenanceAndEtatAgrementCommonPL(IPrincipal utilisateurConnecte, pCentreEncour centre)
        {
            if (centre.EstAgrementValidePL)
            {
                if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()))
                {
                    return true;
                }
                else
                {
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.RESEAU_Gestionnaire.ToString()) ||
                        utilisateurConnecte.IsInRole(rUtilisateur.Profil.RESEAU_Administrateur.ToString()) ||
                        utilisateurConnecte.IsInRole(rUtilisateur.Profil.CENTRE_INDEPENDANT.ToString()))
                    {
                        return isUtilisateurProprietaireCentre(utilisateurConnecte, centre, rCategorieVehicule.CategorieEnum.PL);
                    }
                }
            }

            return false;
        }

        //PLE le 27/03/2012 : même droits que isRightForCentreAppartenanceAndEtatAgrementCommonPL + les droits de visu
        private static bool isRightForCentreAppartenanceAndEtatAgrementCommonForConsultPL(IPrincipal utilisateurConnecte, pCentreEncour centre)
        {
            if (centre.EstAgrementValidePL)
            {
                if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()))
                {
                    return true;
                }
                else
                {
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.RESEAU_Gestionnaire.ToString()) ||
                        utilisateurConnecte.IsInRole(rUtilisateur.Profil.RESEAU_Administrateur.ToString()) ||
                        utilisateurConnecte.IsInRole(rUtilisateur.Profil.RESEAU_Consultation.ToString()) ||
                        utilisateurConnecte.IsInRole(rUtilisateur.Profil.CENTRE_INDEPENDANT.ToString()))
                    {
                        return isUtilisateurProprietaireCentre(utilisateurConnecte, centre, rCategorieVehicule.CategorieEnum.PL);
                    }
                }
            }

            return false;
        }

        private static bool isRightForCentreAppartenanceAndEtatAgrementCommonCL(IPrincipal utilisateurConnecte, pCentreEncour centre)
        {
            if (centre.EstAgrementValideCL)
            {
                if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()))
                {
                    return true;
                }
                else
                {
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.RESEAU_Gestionnaire.ToString()) ||
                          utilisateurConnecte.IsInRole(rUtilisateur.Profil.RESEAU_Administrateur.ToString()) ||
                          utilisateurConnecte.IsInRole(rUtilisateur.Profil.CENTRE_INDEPENDANT.ToString()))
                    {
                        return isUtilisateurProprietaireCentre(utilisateurConnecte, centre, rCategorieVehicule.CategorieEnum.CL);
                    }
                }
            }

            return false;
        }

        //PLE le 27/03/2012 : même droits que isRightForCentreAppartenanceAndEtatAgrementCommonVL + les droits de visu
        private static bool isRightForCentreAppartenanceAndEtatAgrementCommonForConsultCL(IPrincipal utilisateurConnecte, pCentreEncour centre)
        {
            if (centre.EstAgrementValideCL)
            {
                if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()))
                {
                    return true;
                }
                else
                {
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.RESEAU_Gestionnaire.ToString()) ||
                          utilisateurConnecte.IsInRole(rUtilisateur.Profil.RESEAU_Administrateur.ToString()) ||
                          utilisateurConnecte.IsInRole(rUtilisateur.Profil.RESEAU_Consultation.ToString()) ||
                          utilisateurConnecte.IsInRole(rUtilisateur.Profil.CENTRE_INDEPENDANT.ToString()))
                    {
                        return isUtilisateurProprietaireCentre(utilisateurConnecte, centre, rCategorieVehicule.CategorieEnum.CL);
                    }
                }
            }

            return false;
        }

        public static bool isRightImpNotification(IPrincipal utilisateurConnecte, pCentreEncour centre, rCategorieVehicule.CategorieEnum categorieVehicule)
        {
            Init();

            string m1 = centre.RMotifModification.Id;

            //YM 08/02/2012 verifier que l'utilisateur est bien de la prefecture du centre
            rUtilisateur utilisateur = utilisateurService.FindBy(utilisateurConnecte.Identity.Name);

            rPrefecture prefectureUser = new rPrefecture();
            if (utilisateur != null && utilisateur.RPrefecture != null)
            {
                prefectureUser = prefectureService.FindBy(utilisateur.RPrefecture.Id);
            }

            //EG le 29/11/2011 : seulement OTC_Administration et PREFECTURE
            //YM le 08/02/2012 : seulement pour les centres de la prefecture de l'utilisateur
            if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || (utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) && prefectureUser == centre.RPrefecture))
            {
                switch (categorieVehicule)
                {
                    case rCategorieVehicule.CategorieEnum.VL:
                        if ((int)pCentreEncour.CodeEtatAgrementVl.AGREMENT_KO == Convert.ToInt16(centre.EtatAgrementVl)) return false;

                        if (
                            (m1 == Convert.ToString(rMotifModification.TypeModification.NOTVL) ||
                             m1 == Convert.ToString(rMotifModification.TypeModification.CHEXPLVL) ||
                             m1 == Convert.ToString(rMotifModification.TypeModification.CHLOCAVL) ||
                             m1 == Convert.ToString(rMotifModification.TypeModification.DEROG) ||
                             m1 == Convert.ToString(rMotifModification.TypeModification.SUPDEROG) || //EG le 05/03/2012
                             m1 == Convert.ToString(rMotifModification.TypeModification.CHRATTVL)) &&
                            (categorieVehicule == rCategorieVehicule.CategorieEnum.VL)
                           )
                        {
                            return true;
                        }

                        break;

                    case rCategorieVehicule.CategorieEnum.PL:
                        if ((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_KO == Convert.ToInt16(centre.EtatAgrementPl)) return false;

                        //YM 23/05/2013 supprimé, doublon avec lignes du dessous
                        //if (
                        //    (m1 == Convert.ToString(rMotifModification.TypeModification.NOTPL) ||
                        //     m1 == Convert.ToString(rMotifModification.TypeModification.CHEXPLPL) ||
                        //     m1 == Convert.ToString(rMotifModification.TypeModification.CHLOCAPL) ||
                        //     m1 == Convert.ToString(rMotifModification.TypeModification.DEROG) ||
                        //     m1 == Convert.ToString(rMotifModification.TypeModification.SUPDEROG) || //EG le 05/03/2012
                        //     m1 == Convert.ToString(rMotifModification.TypeModification.CHRATTPL)) &&
                        //    (categorieVehicule == rCategorieVehicule.CategorieEnum.PL)
                        //   )
                        //    {
                        //        return true;
                        //    }

                        if (
                           (m1 == Convert.ToString(rMotifModification.TypeModification.NOTPL) ||
                            m1 == Convert.ToString(rMotifModification.TypeModification.CHEXPLPL) ||
                            m1 == Convert.ToString(rMotifModification.TypeModification.CHLOCAPL) ||
                            m1 == Convert.ToString(rMotifModification.TypeModification.CHRATTPL) ||
                            m1 == Convert.ToString(rMotifModification.TypeModification.DEROG) ||
                            m1 == Convert.ToString(rMotifModification.TypeModification.SUPDEROG) || //EG le 05/03/2012
                            m1 == Convert.ToString(rMotifModification.TypeModification.NOTRPL) || //YM le 23/05/2013 (fiche 123)
                            m1 == Convert.ToString(rMotifModification.TypeModification.REJRPL)) &&
                           (categorieVehicule == rCategorieVehicule.CategorieEnum.PL)
                          )
                        {
                            return true;
                        }

                        break;

                    case rCategorieVehicule.CategorieEnum.CL:
                        if ((int)pCentreEncour.CodeEtatAgrementCl.AGREMENT_KO == Convert.ToInt16(centre.EtatAgrementCl)) return false;

                        if (
                          (m1 == Convert.ToString(rMotifModification.TypeModification.NOTCL) ||
                           m1 == Convert.ToString(rMotifModification.TypeModification.CHEXPLCL) ||
                           m1 == Convert.ToString(rMotifModification.TypeModification.CHLOCACL) ||
                           m1 == Convert.ToString(rMotifModification.TypeModification.DEROG) ||
                           m1 == Convert.ToString(rMotifModification.TypeModification.SUPDEROG) || //EG le 05/03/2012
                           m1 == Convert.ToString(rMotifModification.TypeModification.CHRATTCL)) &&
                          (categorieVehicule == rCategorieVehicule.CategorieEnum.CL)
                         )
                        {
                            return true;
                        }
                        break;

                }

            }

            return false;
        }

        public static bool isRightImpModifNotification(IPrincipal utilisateurConnecte, pCentreEncour centre, rCategorieVehicule.CategorieEnum categorieVehicule)
        {
            Init();

            string m1 = centre.RMotifModification.Id;

            //YM 08/02/2012 verifier que l'utilisateur est bien de la prefecture du centre
            rUtilisateur utilisateur = utilisateurService.FindBy(utilisateurConnecte.Identity.Name);

            rPrefecture prefectureUser = new rPrefecture();
            if (utilisateur != null && utilisateur.RPrefecture != null)
            {
                prefectureUser = prefectureService.FindBy(utilisateur.RPrefecture.Id);
            }

            //YM le 08/02/2012 : seulement OTC_Administration et PREFECTURE

            if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || (utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) && prefectureUser == centre.RPrefecture))
            {

                if (
                    (m1 == Convert.ToString(rMotifModification.TypeModification.ANUVL)) &&
                    (categorieVehicule == rCategorieVehicule.CategorieEnum.VL)
                   )
                {
                    return true;
                }

                if (
                    (m1 == Convert.ToString(rMotifModification.TypeModification.ANUPL)) &&
                    (categorieVehicule == rCategorieVehicule.CategorieEnum.PL)
                   )
                {
                    return true;
                }

                if (
                    (m1 == Convert.ToString(rMotifModification.TypeModification.ANUCL)) &&
                    (categorieVehicule == rCategorieVehicule.CategorieEnum.CL)
                   )
                {
                    return true;
                }
                //03/05/2012 PLE autorisation de la notification de chagement de dénomination commerciale
                if ((m1 == Convert.ToString(rMotifModification.TypeModification.CHDECOM)) && (categorieVehicule == rCategorieVehicule.CategorieEnum.VL) && centre.EstAgrementValideVL)
                {
                    return true;
                }

                if ((m1 == Convert.ToString(rMotifModification.TypeModification.CHDECOM)) && (categorieVehicule == rCategorieVehicule.CategorieEnum.PL) && centre.EstAgrementValidePL)
                {
                    return true;
                }

                if ((m1 == Convert.ToString(rMotifModification.TypeModification.CHDECOM)) && (categorieVehicule == rCategorieVehicule.CategorieEnum.CL) && centre.EstAgrementValideCL)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool isRightAfficheTelechergePV(IPrincipal utilisateurConnecte)
        {
            Init();

            if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()))
            {
                return true;
            }
            return false;
        }

        public static bool isRightAfficheLieuPassageCT(IPrincipal utilisateurConnecte)
        {
            Init();

            if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()))
            {
                return true;
            }
            return false;
        }

        public static bool isRightModificationCentre(IPrincipal utilisateurConnecte, pCentreEncour centre)
        {
            Init();

            rUtilisateur u = utilisateurService.FindBy(utilisateurConnecte.Identity.Name);
            if (
                centre.RPrefecture == u.RPrefecture ||
                utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) ||
                utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Informatique.ToString()))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public static bool isRightAfficheAcceder(IPrincipal utilisateurConnecte, pCentreEncour centre)
        {
            Init();

            if (
                utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) ||
                utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Informatique.ToString()) ||
                utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Consultation.ToString()) ||
                utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) ||
                utilisateurConnecte.IsInRole(rUtilisateur.Profil.DRIRE.ToString()) ||
                utilisateurConnecte.IsInRole(rUtilisateur.Profil.DSCR.ToString()) ||
                utilisateurConnecte.IsInRole(rUtilisateur.Profil.PIV.ToString())
                )
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// isUtilisateurProprietaireCentre
        /// </summary>
        /// <param name="utilisateurConnecte">utilisateur connecté</param>
        /// <param name="centre">centre visé</param>
        /// <param name="categorieCentre">catégorie agrément visée VL, PL CL</param>
        /// <returns></returns>
        public static bool isUtilisateurProprietaireCentre(IPrincipal utilisateurConnecte, pCentreEncour centre, rCategorieVehicule.CategorieEnum categorieCentre)
        {
            Init();
            if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.RESEAU.ToString()))
            {
                rUtilisateur u = utilisateurService.FindBy(utilisateurConnecte.Identity.Name);

                bool autorise = false;
                foreach (rReseau r in u.RReseaux)
                {
                    if ((centre.ReseauRattachementVL != null && r.Id.Equals(centre.ReseauRattachementVL.Id) && categorieCentre.Equals(rCategorieVehicule.CategorieEnum.VL))
                        || (centre.ReseauRattachementPL != null && r.Id.Equals(centre.ReseauRattachementPL.Id) && categorieCentre.Equals(rCategorieVehicule.CategorieEnum.PL))
                        || (centre.ReseauRattachementCL != null && r.Id.Equals(centre.ReseauRattachementCL.Id) && categorieCentre.Equals(rCategorieVehicule.CategorieEnum.CL)))
                    {
                        autorise = true;
                        break;
                    }
                }

                return autorise;
            }
            else
            {
                //CENTRE INDEPENDANT
                if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.CENTRE_INDEPENDANT.ToString()))
                {
                    rUtilisateur u = utilisateurService.FindBy(utilisateurConnecte.Identity.Name);
                    if ((centre.Equals(u.PCentreEncourVL) && categorieCentre.Equals(rCategorieVehicule.CategorieEnum.VL))
                        || (centre.Equals(u.PCentreEncourPL) && categorieCentre.Equals(rCategorieVehicule.CategorieEnum.PL))
                        || (centre.Equals(u.PCentreEncourCL) && categorieCentre.Equals(rCategorieVehicule.CategorieEnum.CL)))

                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Test si l'utilisateur appartient au réseau passé en paramètre
        /// </summary>
        /// <param name="utilisateurConnecte"></param>
        /// <param name="reseau"></param>
        /// <returns></returns>
        public static bool isUtilisateurAppartientReseau(IPrincipal utilisateurConnecte, rReseau reseau)
        {
            Init();

            bool autorise = false;

            if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.RESEAU.ToString()))
            {
                rUtilisateur u = utilisateurService.FindBy(utilisateurConnecte.Identity.Name);
                foreach (rReseau r in u.RReseaux)
                {
                    if (r.Equals(reseau))
                    {
                        autorise = true;
                        break;
                    }
                }
            }

            return autorise;
        }

        public enum ActionsFicheCentre
        {
            ADMMODFI,
            ADMSUPFI,
            AMEND,
            ANUCL,
            ANUPL,
            ANUVL,
            CHDECOM,
            CHHORAI,
            CHEXPLVL,
            CHLOCAVL,
            CHRATTVL,
            CHEXPLPL,
            CHRATTPL,
            CHLOCAPL,
            CHEXPLCL,
            CHLOCACL,
            CHRATTCL,
            CHREPLEG,
            CRECECL,
            CRECEPL,
            CRECEVL,
            DEACL,
            DEAPL,
            DEAVL,
            DERPL,
            DEROG_VL,
            DEROG_PL,
            DEROG_CL,
            MOD_DEROG_VL,
            MOD_DEROG_PL,
            MOD_DEROG_CL,
            MODAUTRE,
            NOTCL,
            NOTPL,
            NOTRPL,
            NOTVL,
            RDV,
            REJCL,
            REJPL,
            REJRPL,
            REJVL,
            RETOUR,
            SANCT,
            ADMMODFINT,
            //EG le 29/11/2011
            EQUIPVL,
            EQUIPPL,
            EQUIPCL,
            SHOW_EQUIPVL,
            SHOW_EQUIPPL,
            SHOW_EQUIPCL,
            TELECHARGE_CERTIF_SSL_VL,
            TELECHARGE_CERTIF_SSL_PL,
            TELECHARGE_CERTIF_SSL_CL,
            REVOCATION_CERTIF,
            TELECHARGE_CE_PL,
            TELECHARGE_CE_VL,
            TELECHARGE_CE_CL,
            SHOW_SANCT,
            SHOW_AMEND,
            SHOW_DEROG,
            SHOW_HISTO,
            SHOW_CONTROLEUR,
            NOUVEL_AGREM_VL,
            NOUVEL_AGREM_PL,
            NOUVEL_AGREM_CL,
            SHOW_HORAIRE_OUVERTURE,
            SHOW_RDV,
            JOURNAL,
            //PLE le 19/03/2012
            TELECHARGE_FICHE_CENTRE,
            TELECHARGE_FACTURE_VL,
            TELECHARGE_FACTURE_PL,
            TELECHARGE_FACTURE_CL,
            //PLE le 19/01/2024
            EXTDECLA_VL,
            EXTDECLA_PL,
            EXTRETRA_VL,
            EXTRETRA_PL
        }

        public static bool isRightActionForControleur(IPrincipal utilisateurConnecte, pContoleurEncour controleur, ActionsFicheControleur action)
        {

            Init();

            //YM 02/02/2012 verifier que l'utilisateur est bien dans la prefecture du centre
            rUtilisateur utilisateur = new rUtilisateur();
            utilisateur = utilisateurService.FindBy(utilisateurConnecte.Identity.Name);

            //PLE 07/05/2012 Suppression de la gestion de l'appartenance PREFECTURE / CONTROLEUR
            //
            ////Prefecture du user connecté
            //rPrefecture prefectureUser = new rPrefecture();
            ////Préfecture de l'installation de rattachement du contrôleur
            //rPrefecture prefectureControleur = new rPrefecture();
            //if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()))
            //{
            //    prefectureUser = prefectureService.FindBy(utilisateur.RPrefecture.Id);
            //    //si le controleur n'est pas rattaché à une préfecture, on lui attribue la préfecture de l'utilisateur pour éviter que prefectureControleur soit null
            //    if (controleur == null || controleur.RPrefecture == null)
            //    {
            //        prefectureControleur = prefectureUser;
            //    }
            //    else
            //    {
            //        prefectureControleur = prefectureService.FindBy(controleur.RPrefecture.Id);
            //    }
            //}

            switch (action)
            {

                //EG le 19/10/2011
                case ActionsFicheControleur.RETOUR:
                    //Retour arrière PREFECTURE
                    //PLE 07/05/2012 : Le retour arrière ne pas du lien PREFECTURE/ CONTROLEUR
                    //if ( utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || (utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) && prefectureUser.Id == prefectureControleur.Id))
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || (utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString())))
                    {
                        if (
                             !rTypeDemandeControleur.TypeModification.RETOUR.ToString().Equals(controleur.RTypeDemandeControleur.Id) &&
                             !rTypeDemandeControleur.TypeModification.ADMMODFI.ToString().Equals(controleur.RTypeDemandeControleur.Id) &&
                             !rTypeDemandeControleur.TypeModification.CRECO.ToString().Equals(controleur.RTypeDemandeControleur.Id) &&
                             !rTypeDemandeControleur.TypeModification.CRECOVL.ToString().Equals(controleur.RTypeDemandeControleur.Id) &&
                             !rTypeDemandeControleur.TypeModification.CRECOPL.ToString().Equals(controleur.RTypeDemandeControleur.Id) &&
                             !rTypeDemandeControleur.TypeModification.CRECOCL.ToString().Equals(controleur.RTypeDemandeControleur.Id) &&
                             !rTypeDemandeControleur.TypeModification.SANCT.ToString().Equals(controleur.RTypeDemandeControleur.Id) &&
                             !rTypeDemandeControleur.TypeModification.AMEND.ToString().Equals(controleur.RTypeDemandeControleur.Id) &&
                             !rTypeDemandeControleur.TypeModification.AUDITVL.ToString().Equals(controleur.RTypeDemandeControleur.Id) &&
                             !rTypeDemandeControleur.TypeModification.FORMAPL.ToString().Equals(controleur.RTypeDemandeControleur.Id) &&
                             !rTypeDemandeControleur.TypeModification.AUDITPL.ToString().Equals(controleur.RTypeDemandeControleur.Id) &&
                             !rTypeDemandeControleur.TypeModification.FORMAVL.ToString().Equals(controleur.RTypeDemandeControleur.Id) &&
                             !rTypeDemandeControleur.TypeModification.FORMACL.ToString().Equals(controleur.RTypeDemandeControleur.Id) &&
                             !rTypeDemandeControleur.TypeModification.FORMACL.ToString().Equals(controleur.RTypeDemandeControleur.Id) &&
                             !rTypeDemandeControleur.TypeModification.HABIL.ToString().Equals(controleur.RTypeDemandeControleur.Id)
                           )
                        {
                            return true;
                        }

                    }

                    return false;

                //EG le 23/11/2011
                case ActionsFicheControleur.HABILGAZ:
                    return isRightForControleurAppartenanceAndEtatAgrementCommonVL(utilisateurConnecte, controleur, false);

                //EG le 23/11/2011
                case ActionsFicheControleur.ADMSUPFI:
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()))
                    {
                        return true;
                    }

                    return false;

                //EG le 23/11/2011
                case ActionsFicheControleur.ADMANUFI:
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()))
                    {
                        return true;
                    }

                    return false;

                //EG le 23/11/2011
                case ActionsFicheControleur.AMEND:
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || utilisateurConnecte.IsInRole(rUtilisateur.Profil.DRIRE.ToString()))
                    {
                        if (controleur.AunAgrementValide)
                        {
                            return true;
                        }
                    }

                    return false;

                //EG le 23/11/2011
                //PLE 07/05/2011 Seule la prefecture du centre de rattachement du contrôleur peut annuler l'agrément du contrôleur
                case ActionsFicheControleur.ANUVL:
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || isUtilisateurPrefectureEnChargeControleur(utilisateurConnecte, controleur, rCategorieVehicule.CategorieEnum.VL))
                    {
                        if (controleur.EstAgrementValideVL)
                        {
                            return true;
                        }
                    }

                    return false;

                //EG le 23/11/2011
                case ActionsFicheControleur.AUDITVL:
                    return isRightForControleurAppartenanceAndEtatAgrementCommonVL(utilisateurConnecte, controleur, false);

                //YM le 26/03/2012
                //PLE 07/05/2012 Modification des droits de visu des audits
                case ActionsFicheControleur.VOIR_AUDITVL:
                    //return isRightForControleurAppartenanceAndEtatAgrementCommonVL(utilisateurConnecte, controleur, true);
                    if (controleur.EstAgrementValideVL)
                    {
                        return true;
                    }
                    return false;

                //EG le 23/11/2011
                case ActionsFicheControleur.CHCEVL:
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()))
                    {
                        if (controleur.EstAgrementValideVL)
                        {
                            return true;
                        }
                    }

                    return false;

                //EG le 23/11/2011
                case ActionsFicheControleur.DEAVL:
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || (utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString())))
                    {
                        if (controleur.EstAgrementVLNonOK || controleur.EstAgrementVLRejete)
                        {
                            return true;
                        }
                    }

                    return false;

                //EG le 23/11/2011
                case ActionsFicheControleur.CRECOVL:
                //PLE 25/11/2011 Ajout CRECOPL et CRECOCL
                case ActionsFicheControleur.CRECOPL:
                case ActionsFicheControleur.CRECOCL:
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()))
                    {
                        return true;
                    }

                    return false;

                //EG le 23/11/2011
                case ActionsFicheControleur.FORMAVL:
                    return isRightForControleurAppartenanceAndEtatAgrementCommonVL(utilisateurConnecte, controleur, false);

                //YM le 26/03/2012
                //PLE le 07/05/2012 : Modification des droits de visu des formations
                case ActionsFicheControleur.VOIR_FORMAVL:
                    //return isRightForControleurAppartenanceAndEtatAgrementCommonVL(utilisateurConnecte, controleur, true);
                    if (controleur.EstAgrementValideVL)
                    {
                        return true;
                    }
                    return false;

                //EG le 23/11/2011
                //PLE 07/05/2012 : La modification de l'état civil d'un contrôleur est possible quelque soit l'état de l'agrément.
                case ActionsFicheControleur.MODCO:
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || (utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString())))
                    {
                        return true;
                    }

                    return false;

                //EG le 23/11/2011
                case ActionsFicheControleur.NOTVL:
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || (isUtilisateurPrefectureEnChargeControleur(utilisateurConnecte, controleur, rCategorieVehicule.CategorieEnum.VL)))
                    {
                        if (controleur.EstAgrementVLEnCours)
                        {
                            return true;
                        }
                    }
                    return false;

                //EG le 23/11/2011
                case ActionsFicheControleur.REJVL:
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || (isUtilisateurPrefectureEnChargeControleur(utilisateurConnecte, controleur, rCategorieVehicule.CategorieEnum.VL)))
                    {
                        if (controleur.EstAgrementVLEnCours)
                        {
                            return true;
                        }
                    }
                    return false;

                //EG le 23/11/2011
                case ActionsFicheControleur.SANCT:
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()))
                    {
                        if (controleur.AunAgrementValide)
                        {
                            return true;
                        }
                    }

                    return false;

                //EG le 25/11/2011 
                case ActionsFicheControleur.VOIR_SANCT:
                    if (
                        utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) ||
                        utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) ||
                        utilisateurConnecte.IsInRole(rUtilisateur.Profil.DRIRE.ToString()) ||
                        utilisateurConnecte.IsInRole(rUtilisateur.Profil.DSCR.ToString()) ||
                        utilisateurConnecte.IsInRole(rUtilisateur.Profil.PIV.ToString())

                        )
                    {
                        //16/05/2022 -PLE- Quelque soit l'état de l'agrément contrôleur la liste des sanctions est accessible
                        /*if (controleur.AunAgrementValide)
                        {
                            return true;
                        }*/
                        return true;

                    }

                    return false;

                //EG Modifié le 24/11/2011
                //PLE 07/05/2012 : Uniquement la PREFECTURE en charge du contrôleur
                case ActionsFicheControleur.TELECHARGE_NOTIFVL_PDF:

                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || (isUtilisateurPrefectureEnChargeControleur(utilisateurConnecte, controleur, rCategorieVehicule.CategorieEnum.VL)))
                    {
                        if (controleur.EstAgrementVLNonOK && controleur.RTypeDemandeControleur.Id.Equals(rTypeDemandeControleur.TypeModification.ANUVL.ToString()))
                        {
                            return true;
                        }

                        if (controleur.EstAgrementValideVL)
                        {
                            if (
                                controleur.RTypeDemandeControleur.Id.Equals(rTypeDemandeControleur.TypeModification.NOTVL.ToString()) ||
                                controleur.RTypeDemandeControleur.Id.Equals(rTypeDemandeControleur.TypeModification.CHCEVL.ToString())
                                )
                            {
                                return true;
                            }
                        }
                    }
                    return false;

                //EG le 24/11/2011
                case ActionsFicheControleur.TELECHARGE_FICHE_CONTROLEUR:
                    if (
                        utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) ||
                        utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) ||
                        utilisateurConnecte.IsInRole(rUtilisateur.Profil.DRIRE.ToString()) ||
                        utilisateurConnecte.IsInRole(rUtilisateur.Profil.DSCR.ToString()) ||
                        utilisateurConnecte.IsInRole(rUtilisateur.Profil.PIV.ToString())
                        )
                    {
                        return true;
                    }
                    return false;

                //EG le 24/11/2011
                case ActionsFicheControleur.TELECHARGE_FICHE_QUALIFICATION_CONTROLEUR:
                    //PL 12/03/2012 Ajout accès DRIRE
                    //PLE 07/05/2012 Ajout de l'accès DRIRE et RESEAU CONSULTATION en visu
                    return isRightForControleurAppartenanceAndEtatAgrementCommonPL(utilisateurConnecte, controleur, true);

                //YM le 24/05/2013
                case ActionsFicheControleur.TELECHARGE_FICHE_QUALIFICATION_CONTROLEUR_VL:
                    //PLE le 24/10/2013 Ajout des accès OTC, Prefecture et drire pour les controleurs agrées et annulés
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) ||
                        utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Consultation.ToString()) ||
                        utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) ||
                        utilisateurConnecte.IsInRole(rUtilisateur.Profil.DRIRE.ToString()) ||
                        utilisateurConnecte.IsInRole(rUtilisateur.Profil.DSCR.ToString()) ||
                        utilisateurConnecte.IsInRole(rUtilisateur.Profil.PIV.ToString()))
                    {
                        if (controleur.EstAgrementValideVL || controleur.EstAgrementAnnuleVL)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                        return isRightForControleurAppartenanceAndEtatAgrementCommonVL(utilisateurConnecte, controleur, true);

                //EG le 24/11/2011
                case ActionsFicheControleur.ANUPL:
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || (isUtilisateurPrefectureEnChargeControleur(utilisateurConnecte, controleur, rCategorieVehicule.CategorieEnum.PL)))
                    {
                        if (controleur.EstAgrementValidePL)
                        {
                            return true;
                        }
                    }

                    return false;

                //EG le 24/11/2011
                case ActionsFicheControleur.AUDITPL:
                    return isRightForControleurAppartenanceAndEtatAgrementCommonPL(utilisateurConnecte, controleur, false);

                //YM le 26/03/2012
                case ActionsFicheControleur.VOIR_AUDITPL:
                    //return isRightForControleurAppartenanceAndEtatAgrementCommonPL(utilisateurConnecte, controleur, true);
                    if (controleur.EstAgrementValidePL)
                    {
                        return true;
                    }
                    return false;

                //EG le 24/11/2011
                case ActionsFicheControleur.CHCEPL:
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()))
                    {
                        if (controleur.EstAgrementValidePL)
                        {
                            return true;
                        }
                    }

                    return false;

                //EG le 24/11/2011
                case ActionsFicheControleur.DEAPL:
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || (utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString())))
                    {
                        if (controleur.EstAgrementPLNonOK || controleur.EstAgrementPLRejete)
                        {
                            return true;
                        }
                    }

                    return false;

                //EG le 24/11/2011
                case ActionsFicheControleur.FORMAPL:
                    return isRightForControleurAppartenanceAndEtatAgrementCommonPL(utilisateurConnecte, controleur, false);

                //YM le 26/03/2012
                case ActionsFicheControleur.VOIR_FORMAPL:
                    //return isRightForControleurAppartenanceAndEtatAgrementCommonPL(utilisateurConnecte, controleur, true);
                    if (controleur.EstAgrementValidePL)
                    {
                        return true;
                    }
                    return false;

                //EG le 24/11/2011
                case ActionsFicheControleur.HABIL:
                    return isRightForControleurAppartenanceAndEtatAgrementCommonPL(utilisateurConnecte, controleur, false);

                //EG le 24/11/2011
                case ActionsFicheControleur.NOTPL:
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || (isUtilisateurPrefectureEnChargeControleur(utilisateurConnecte, controleur, rCategorieVehicule.CategorieEnum.PL)))
                    {
                        if (controleur.EstAgrementPLEnCours)
                        {
                            return true;
                        }
                    }
                    return false;

                //EG le 24/11/2011
                case ActionsFicheControleur.REJPL:
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || (isUtilisateurPrefectureEnChargeControleur(utilisateurConnecte, controleur, rCategorieVehicule.CategorieEnum.PL)))
                    {
                        if (controleur.EstAgrementPLEnCours)
                        {
                            return true;
                        }
                    }
                    return false;

                //EG Modifié le 24/11/2011
                case ActionsFicheControleur.TELECHARGE_NOTIFPL_PDF:

                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || (isUtilisateurPrefectureEnChargeControleur(utilisateurConnecte, controleur, rCategorieVehicule.CategorieEnum.PL)))
                    {
                        if (controleur.EstAgrementPLNonOK && controleur.RTypeDemandeControleur.Id.Equals(rTypeDemandeControleur.TypeModification.ANUPL.ToString()))
                        {
                            return true;
                        }

                        if (controleur.EstAgrementValidePL)
                        {
                            if (
                                controleur.RTypeDemandeControleur.Id.Equals(rTypeDemandeControleur.TypeModification.NOTPL.ToString()) ||
                                controleur.RTypeDemandeControleur.Id.Equals(rTypeDemandeControleur.TypeModification.CHCEPL.ToString())
                                )
                            {
                                return true;
                            }
                        }
                    }
                    return false;

                //EG le 24/11/2011
                case ActionsFicheControleur.ANUCL:
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || isUtilisateurPrefectureEnChargeControleur(utilisateurConnecte, controleur, rCategorieVehicule.CategorieEnum.CL))
                    {
                        if (controleur.EstAgrementValideCL)
                        {
                            return true;
                        }
                    }

                    return false;

                //EG le 24/11/2011
                case ActionsFicheControleur.AUDITCL:
                    return isRightForControleurAppartenanceAndEtatAgrementCommonCL(utilisateurConnecte, controleur, false);

                //YM le 26/03/2012
                //PLE 07/05/2012 Modification des droits de visu des audits
                case ActionsFicheControleur.VOIR_AUDITCL:
                    //return isRightForControleurAppartenanceAndEtatAgrementCommonCL(utilisateurConnecte, controleur, true);
                    if (controleur.EstAgrementValideCL)
                    {
                        return true;
                    }
                    return false;

                //EG le 24/11/2011
                case ActionsFicheControleur.CHCECL:
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()))
                    {
                        if (controleur.EstAgrementValideCL)
                        {
                            return true;
                        }
                    }

                    return false;

                //EG le 24/11/2011
                case ActionsFicheControleur.DEACL:
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || (utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString())))
                    {
                        if (controleur.EstAgrementCLNonOK || controleur.EstAgrementCLRejete)
                        {
                            return true;
                        }
                    }

                    return false;

                //EG le 24/11/2011
                case ActionsFicheControleur.FORMACL:
                    return isRightForControleurAppartenanceAndEtatAgrementCommonCL(utilisateurConnecte, controleur, false);

                //YM le 26/03/2012
                //PLE le 07/05/2012 : Modification des droits de visu des formations
                case ActionsFicheControleur.VOIR_FORMACL:
                    //return isRightForControleurAppartenanceAndEtatAgrementCommonCL(utilisateurConnecte, controleur, true);
                    if (controleur.EstAgrementValideCL)
                    {
                        return true;
                    }
                    return false;

                //EG le 23/11/2011
                case ActionsFicheControleur.NOTCL:
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || (isUtilisateurPrefectureEnChargeControleur(utilisateurConnecte, controleur, rCategorieVehicule.CategorieEnum.CL)))
                    {
                        if (controleur.EstAgrementCLEnCours)
                        {
                            return true;
                        }
                    }
                    return false;

                //EG le 24/11/2011
                case ActionsFicheControleur.REJCL:
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || (isUtilisateurPrefectureEnChargeControleur(utilisateurConnecte, controleur, rCategorieVehicule.CategorieEnum.CL)))
                    {
                        if (controleur.EstAgrementCLEnCours)
                        {
                            return true;
                        }
                    }
                    return false;

                //EG Modifié le 24/11/2011
                case ActionsFicheControleur.TELECHARGE_NOTIFCL_PDF:
                    //PLE 20/08/2024 Correction autorisation TELECHARGE_NOTIFCL_PDF
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || (isUtilisateurPrefectureEnChargeControleur(utilisateurConnecte, controleur, rCategorieVehicule.CategorieEnum.CL)))
                    {
                        if (controleur.EstAgrementCLNonOK && controleur.RTypeDemandeControleur.Id.Equals(rTypeDemandeControleur.TypeModification.ANUCL.ToString()))
                        {
                            return true;
                        }

                        if (controleur.EstAgrementValideCL)
                        {
                            if (
                                controleur.RTypeDemandeControleur.Id.Equals(rTypeDemandeControleur.TypeModification.NOTCL.ToString()) ||
                                controleur.RTypeDemandeControleur.Id.Equals(rTypeDemandeControleur.TypeModification.CHCECL.ToString())
                                )
                            {
                                return true;
                            }
                        }
                    }
                    return false;

                //EG le 25/11/2011 
                case ActionsFicheControleur.VOIR_HISTO:
                    //EG le 22/02/2012 : accès à l'historique dans tous les cas
                    if (
                        utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) ||
                        utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) ||
                        utilisateurConnecte.IsInRole(rUtilisateur.Profil.DRIRE.ToString()) ||
                        utilisateurConnecte.IsInRole(rUtilisateur.Profil.DSCR.ToString()) ||
                        utilisateurConnecte.IsInRole(rUtilisateur.Profil.PIV.ToString())
                        )
                    {
                        /*if (controleur.AunAgrementValide || controleur.AunAgrementDemandeEnCours )
                        {
                            return true;
                        }*/
                        return true;
                    }

                    return false;

                case ActionsFicheControleur.VOIR_AMEND:
                    if (
                        utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) ||
                        utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) ||
                        utilisateurConnecte.IsInRole(rUtilisateur.Profil.DRIRE.ToString()) ||
                        utilisateurConnecte.IsInRole(rUtilisateur.Profil.DSCR.ToString()) ||
                        utilisateurConnecte.IsInRole(rUtilisateur.Profil.PIV.ToString())
                        )
                    {
                        if (controleur.AunAgrementValide)
                        {
                            return true;
                        }
                    }

                    return false;

                case ActionsFicheControleur.EXTDECLA_VL:
                case ActionsFicheControleur.EXTRETRA_VL:

                    if (controleur.CodAgremVl != null && controleur.EstAgrementValideVL)
                    {
                        if (
                            (action.Equals(ActionsFicheControleur.EXTDECLA_VL) &&
                            (((short?)rCategorieVehicule.ExtCatvehEnum.VL).Equals(controleur.ExtAgremClCatVeh) || controleur.ExtAgremClCatVeh == null) &&
                            (((short?)0).Equals(controleur.ExtAgremClStatut) || controleur.ExtAgremClStatut == null))
                            ||
                            (action.Equals(ActionsFicheControleur.EXTRETRA_VL) &&
                            (((short?)rCategorieVehicule.ExtCatvehEnum.VL).Equals(controleur.ExtAgremClCatVeh) || controleur.ExtAgremClCatVeh == null) &&
                            (((short?)1).Equals(controleur.ExtAgremClStatut)))
                           )

                            if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()))
                            {
                                return true;
                            }
                            else
                            {
                                //PLE le 14/04/2025 Suppression de la déclaration d'extension CL à partir du 15 avril 2025
                                if (action.Equals(ActionsFicheControleur.EXTDECLA_VL))
                                {
                                    return false;
                                }
                                if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.CENTRE_INDEPENDANT.ToString()) ||
                                    utilisateurConnecte.IsInRole(rUtilisateur.Profil.RESEAU_Gestionnaire.ToString()) ||
                                    utilisateurConnecte.IsInRole(rUtilisateur.Profil.RESEAU_Administrateur.ToString()))
                                {
                                    return isRightForControleurAppartenanceAndEtatAgrementCommonVL(utilisateurConnecte, controleur, false);
                                }
                            }

                    }
                    return false;

                case ActionsFicheControleur.EXTDECLA_PL:
                case ActionsFicheControleur.EXTRETRA_PL:

                    if (controleur.CodAgremPl != null && controleur.EstAgrementValidePL)
                    {
                        if (
                            (action.Equals(ActionsFicheControleur.EXTDECLA_PL) &&
                            (((short?)rCategorieVehicule.ExtCatvehEnum.PL).Equals(controleur.ExtAgremClCatVeh) || controleur.ExtAgremClCatVeh == null) &&
                            (((short?)0).Equals(controleur.ExtAgremClStatut) || controleur.ExtAgremClStatut == null))
                            ||
                            (action.Equals(ActionsFicheControleur.EXTRETRA_PL) &&
                            (((short?)rCategorieVehicule.ExtCatvehEnum.PL).Equals(controleur.ExtAgremClCatVeh) || controleur.ExtAgremClCatVeh == null) &&
                            (((short?)1).Equals(controleur.ExtAgremClStatut)))
                           )

                            if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()))
                            {
                                return true;
                            }
                            else
                            {
                                //PLE le 14/04/2025 Suppression de la déclaration d'extension CL à partir du 15 avril 2025
                                if (action.Equals(ActionsFicheControleur.EXTDECLA_PL))
                                {
                                    return false;
                                }
                                if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.CENTRE_INDEPENDANT.ToString()) ||
                                    utilisateurConnecte.IsInRole(rUtilisateur.Profil.RESEAU_Gestionnaire.ToString()) ||
                                    utilisateurConnecte.IsInRole(rUtilisateur.Profil.RESEAU_Administrateur.ToString()))
                                {
                                    return isRightForControleurAppartenanceAndEtatAgrementCommonPL(utilisateurConnecte, controleur, false);
                                }
                            }

                    }
                    return false;

                default:
                    return false;

            }
        }

        public static bool canModify(IPrincipal utilisateurConnecte, IModifiable bean)
        {
            if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()))
            {
                return true;
            }
            else
            {
                //L'utilisateur connecté est-il l'auteur de la modification ?
                InformationPersonelle info = InformationHelper.getInformationPersonnelle(false);
                if (info.Utilisateur.Equals(bean.RUtilisateur))
                {
                    return true;
                }

            }

            return false;

        }
        /// <summary>
        /// Autorisation Action sur fiche contrôleur :
        /// L'agrément du contrôleur est valide et utilisateur = réseau gestionnaire ou admin ou Centre indépendant
        /// PLE 07/05/2012 : Ajout de l'option bVisuOnly pour les droits en consultation
        /// </summary>
        /// <param name="utilisateurConnecte"></param>
        /// <param name="controleur"></param>
        /// <param name="bVisuOnly"></param>
        /// <returns></returns>
        /// 
        private static bool isRightForControleurAppartenanceAndEtatAgrementCommonVL(IPrincipal utilisateurConnecte, pContoleurEncour controleur, Boolean bVisuOnly)
        {
            if (controleur.EstAgrementValideVL)
            {
                //03/05/2012 PLE Action non autorisé pour profil Préfecture et DRIRE
                //if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) || utilisateurConnecte.IsInRole(rUtilisateur.Profil.DRIRE.ToString()))
                if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || (bVisuOnly && (utilisateurConnecte.IsInRole(rUtilisateur.Profil.DRIRE.ToString()))))
                {
                    return true;
                }
                else
                {
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.RESEAU_Gestionnaire.ToString()) || utilisateurConnecte.IsInRole(rUtilisateur.Profil.RESEAU_Administrateur.ToString()) || (bVisuOnly && (utilisateurConnecte.IsInRole(rUtilisateur.Profil.RESEAU_Consultation.ToString()))))
                    {
                        rUtilisateur u = utilisateurService.FindBy(utilisateurConnecte.Identity.Name);

                        bool autorise = false;
                        pCentreEncour centreVL = controleur.PCentreEncourVL;
                        if (centreVL == null)
                        {
                            centreVL = centreService.findByCodeAgrement(controleur.CodAgremRattachementVl);
                        }
                        foreach (rReseau r in u.RReseaux)
                        {
                            if (r.Id.Equals(centreVL.ReseauRattachementVL.Id))
                            {
                                autorise = true;
                                break;
                            }
                        }

                        return autorise;
                    }
                    else
                    {
                        //CENTRE INDEPENDANT
                        if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.CENTRE_INDEPENDANT.ToString()))
                        {
                            rUtilisateur u = utilisateurService.FindBy(utilisateurConnecte.Identity.Name);
                            if (controleur.PCentreEncourVL.Equals(u.PCentreEncourVL))
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }
        /// <summary>
        /// 07/05/2012 PLE Supression profil PREFECTURE ET DRIRE
        /// </summary>
        /// <param name="utilisateurConnecte"></param>
        /// <param name="controleur"></param>
        /// <returns></returns>
        private static bool isRightForControleurAppartenanceAndEtatAgrementCommonPL(IPrincipal utilisateurConnecte, pContoleurEncour controleur, Boolean bVisuOnly)
        {
            if (controleur.EstAgrementValidePL)
            {
                //PLE 04/12/2013 Ajout OTC_consultation en visu
                //if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) || utilisateurConnecte.IsInRole(rUtilisateur.Profil.DRIRE.ToString()))
                //if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || (bVisuOnly && (utilisateurConnecte.IsInRole(rUtilisateur.Profil.DSCR.ToString()))) || (bVisuOnly && (utilisateurConnecte.IsInRole(rUtilisateur.Profil.DRIRE.ToString()))))
                if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || (bVisuOnly && (utilisateurConnecte.IsInRole(rUtilisateur.Profil.DSCR.ToString()))) || (bVisuOnly && (utilisateurConnecte.IsInRole(rUtilisateur.Profil.PIV.ToString()))) || (bVisuOnly && (utilisateurConnecte.IsInRole(rUtilisateur.Profil.DRIRE.ToString()))) || (bVisuOnly && (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Consultation.ToString()))))
                {
                    return true;
                }
                else
                {
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.RESEAU_Gestionnaire.ToString()) || utilisateurConnecte.IsInRole(rUtilisateur.Profil.RESEAU_Administrateur.ToString()) || (bVisuOnly && (utilisateurConnecte.IsInRole(rUtilisateur.Profil.RESEAU_Consultation.ToString()))))
                    {
                        rUtilisateur u = utilisateurService.FindBy(utilisateurConnecte.Identity.Name);

                        bool autorise = false;
                        pCentreEncour centrePL = controleur.PCentreEncourPL;
                        if (centrePL == null)
                        {
                            centrePL = centreService.findByCodeAgrement(controleur.CodAgremRattachementPl);
                        }
                        foreach (rReseau r in u.RReseaux)
                        {
                            if (r.Id.Equals(centrePL.ReseauRattachementPL.Id))
                            {
                                autorise = true;
                                break;
                            }
                        }

                        return autorise;
                    }
                    else
                    {
                        //CENTRE INDEPENDANT
                        if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.CENTRE_INDEPENDANT.ToString()))
                        {
                            rUtilisateur u = utilisateurService.FindBy(utilisateurConnecte.Identity.Name);
                            if (controleur.PCentreEncourPL.Equals(u.PCentreEncourPL))
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 07/05/2012 PLE Supression profil PREFECTURE ET DRIRE
        /// </summary>
        /// <param name="utilisateurConnecte"></param>
        /// <param name="controleur"></param>
        /// <param name="bVisuOnly"></param>
        /// <returns></returns>
        private static bool isRightForControleurAppartenanceAndEtatAgrementCommonCL(IPrincipal utilisateurConnecte, pContoleurEncour controleur, Boolean bVisuOnly)
        {
            if (controleur.EstAgrementValideCL)
            {
                //07/05/2012 PLE Action non autorisé pour profil Prefecture et DRIRE
                //if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) || utilisateurConnecte.IsInRole(rUtilisateur.Profil.DRIRE.ToString()))
                if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || (bVisuOnly && (utilisateurConnecte.IsInRole(rUtilisateur.Profil.DRIRE.ToString()))))
                {
                    return true;
                }
                else
                {
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.RESEAU_Gestionnaire.ToString()) || utilisateurConnecte.IsInRole(rUtilisateur.Profil.RESEAU_Administrateur.ToString()) || (bVisuOnly && (utilisateurConnecte.IsInRole(rUtilisateur.Profil.RESEAU_Consultation.ToString()))))
                    {
                        rUtilisateur u = utilisateurService.FindBy(utilisateurConnecte.Identity.Name);

                        bool autorise = false;
                        pCentreEncour centreCL = controleur.PCentreEncourCL;
                        if (centreCL == null)
                        {
                            centreCL = centreService.findByCodeAgrement(controleur.CodAgremRattachementCl);
                        }
                        foreach (rReseau r in u.RReseaux)
                        {
                            if (r.Id.Equals(centreCL.ReseauRattachementCL.Id))
                            {
                                autorise = true;
                                break;
                            }
                        }

                        return autorise;
                    }
                    else
                    {
                        //CENTRE INDEPENDANT
                        if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.CENTRE_INDEPENDANT.ToString()))
                        {
                            rUtilisateur u = utilisateurService.FindBy(utilisateurConnecte.Identity.Name);
                            if (controleur.PCentreEncourCL.Equals(u.PCentreEncourCL))
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }
        /// <summary>
        /// Test si l'utilisateur connecté est de profil PREFECTURE et en charge de la gestion du contrôleur
        /// c'est à dire rattaché à un centre de la préfecture pour la catégorie de véhicule passé en paramètre.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="controleur"></param>
        /// <param name="categorie"></param>
        /// <returns></returns>
        private static bool isUtilisateurPrefectureEnChargeControleur(IPrincipal user, pContoleurEncour controleur, rCategorieVehicule.CategorieEnum categorie)
        {
            if (user.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()))
            {
                //todo tester selon les actions
                rUtilisateur u = utilisateurService.FindBy(user.Identity.Name);

                switch (categorie)
                {
                    case rCategorieVehicule.CategorieEnum.VL:
                        if (controleur.PCentreEncourVL != null)
                        {
                            if (controleur.PCentreEncourVL.RPrefecture.Id == u.RPrefecture.Id)
                                return true;
                            else
                                return false;
                        }
                        return true;
                    case rCategorieVehicule.CategorieEnum.PL:
                        if (controleur.PCentreEncourPL != null)
                        {
                            if (controleur.PCentreEncourPL.RPrefecture.Id == u.RPrefecture.Id)
                                return true;
                            else
                                return false;
                        }
                        return true;
                    case rCategorieVehicule.CategorieEnum.CL:
                        if (controleur.PCentreEncourCL != null)
                        {
                            if (controleur.PCentreEncourCL.RPrefecture.Id == u.RPrefecture.Id)
                                return true;
                            else
                                return false;
                        }
                        return true;

                    default: return false;
                }
            }

            return false;
        }
        private static bool isAuthorizedPrefecture(IPrincipal user, pContoleurEncour controleur, ActionsFicheControleur action)
        {
            if (user.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()))
            {
                //todo tester selon les actions
                rUtilisateur u = utilisateurService.FindBy(user.Identity.Name);
                if (controleur.EstAgrementValideVL)
                {
                    if (controleur.PCentreEncourVL.RPrefecture.Id == u.RPrefecture.Id)
                    {
                        return true;
                    }
                }

                if (controleur.EstAgrementValidePL)
                {
                    if (controleur.PCentreEncourPL.RPrefecture.Id == u.RPrefecture.Id)
                    {
                        return true;
                    }
                }

                if (controleur.EstAgrementValideCL)
                {
                    if (controleur.PCentreEncourCL.RPrefecture.Id == u.RPrefecture.Id)
                    {
                        return true;
                    }
                }

            }

            return false;
        }

        private static bool canEditPdf(pContoleurEncour c)
        {
            if (
                 c.RTypeDemandeControleur.Id.Equals(rTypeDemandeControleur.TypeModification.NOTVL.ToString()) ||
                 c.RTypeDemandeControleur.Id.Equals(rTypeDemandeControleur.TypeModification.NOTPL.ToString()) ||
                 c.RTypeDemandeControleur.Id.Equals(rTypeDemandeControleur.TypeModification.NOTCL.ToString()) ||
                 c.RTypeDemandeControleur.Id.Equals(rTypeDemandeControleur.TypeModification.ANUVL.ToString()) ||
                 c.RTypeDemandeControleur.Id.Equals(rTypeDemandeControleur.TypeModification.ANUPL.ToString()) ||
                 c.RTypeDemandeControleur.Id.Equals(rTypeDemandeControleur.TypeModification.ANUCL.ToString()) ||
                 c.RTypeDemandeControleur.Id.Equals(rTypeDemandeControleur.TypeModification.CHCEVL.ToString()) ||
                 c.RTypeDemandeControleur.Id.Equals(rTypeDemandeControleur.TypeModification.CHCEPL.ToString()) ||
                 c.RTypeDemandeControleur.Id.Equals(rTypeDemandeControleur.TypeModification.CHCECL.ToString())
                 )
            {

                return true;

            }
            else
                return false;
        }

        /* On le met ici ? 
        public static bool verifModifForPDF(rMotifModification motif, rCategorieVehicule.CategorieEnum categVehicule)
        {
            switch (categVehicule)
            {
                case rCategorieVehicule.CategorieEnum.VL:
                    if (motif.Id.Equals(rMotifModification.TypeModification.NOTVL.ToString())) return true;
                    if (motif.Id.Equals(rMotifModification.TypeModification.CHEXPLVL.ToString())) return true;
                    if (motif.Id.Equals(rMotifModification.TypeModification.CHLOCAVL.ToString())) return true;
                    if (motif.Id.Equals(rMotifModification.TypeModification.CHRATTVL.ToString())) return true;
                    break;

                case rCategorieVehicule.CategorieEnum.PL:
                    if (motif.Id.Equals(rMotifModification.TypeModification.NOTPL.ToString())) return true;
                    if (motif.Id.Equals(rMotifModification.TypeModification.CHEXPLPL.ToString())) return true;
                    if (motif.Id.Equals(rMotifModification.TypeModification.CHLOCAPL.ToString())) return true;
                    if (motif.Id.Equals(rMotifModification.TypeModification.CHRATTPL.ToString())) return true;
                    break;

                case rCategorieVehicule.CategorieEnum.CL:
                    if (motif.Id.Equals(rMotifModification.TypeModification.NOTCL.ToString())) return true;
                    if (motif.Id.Equals(rMotifModification.TypeModification.CHEXPLCL.ToString())) return true;
                    if (motif.Id.Equals(rMotifModification.TypeModification.CHLOCACL.ToString())) return true;
                    if (motif.Id.Equals(rMotifModification.TypeModification.CHRATTCL.ToString())) return true;
                    break;

            }

            return false;
        }
        */

        public enum ActionsFicheControleur
        {
            ADMMODFI,
            ADMSUPFI,
            AMEND,
            ANUCL,
            ANUPL,
            ANUVL,
            AUDITCL,
            AUDITPL,
            AUDITVL,
            AVOTCPL,
            AVOTCVL,
            CHCECL,
            CHCEPL,
            CHCEVL,
            CRECO,
            CRECOCL,
            CRECOPL,
            CRECOVL,
            CRECOVPL,
            DEACL,
            DEAPL,
            DEAVL,
            DEROG,
            FORMACL,
            FORMAPL,
            FORMAVL,
            HABIL,
            HABILGAZ,
            MODCO,
            NOTCL,
            NOTPL,
            NOTVL,
            REJCL,
            REJPL,
            REJVL,
            RETOUR,
            SANCT,
            TELECHARGE_NOTIFVL_PDF,
            TELECHARGE_NOTIFPL_PDF,
            TELECHARGE_NOTIFCL_PDF,
            //EG le 23/11/2011
            ADMANUFI,
            TELECHARGE_FICHE_CONTROLEUR,
            TELECHARGE_FICHE_QUALIFICATION_CONTROLEUR,
            TELECHARGE_FICHE_QUALIFICATION_CONTROLEUR_VL,
            //EG le 25/11/2011
            VOIR_SANCT,
            VOIR_HISTO,
            VOIR_AMEND,
            VOIR_DEROG,
            //YM le 26/03/2012
            VOIR_FORMAVL,
            VOIR_FORMAPL,
            VOIR_FORMACL,
            VOIR_AUDITCL,
            VOIR_AUDITPL,
            VOIR_AUDITVL,
            //PLE le 27/01/2024 Extention catégorie L
            EXTDECLA_VL,
            EXTDECLA_PL,
            EXTRETRA_VL,
            EXTRETRA_PL,
            //PLE le 25/03/2026 Suivi qualification CL
            TELECHARGE_FICHE_QUALIFICATION_CONTROLEUR_CL

        }

        public static bool isRightEditionEtatStatistique(IPrincipal utilisateurConnecte, int etat)
        {
            Init();

            rUtilisateur u = utilisateurService.FindBy(utilisateurConnecte.Identity.Name);
            if (u.RProfil.LibelleProfil != rUtilisateur.Profil.RESEAU.ToString())
            {
                return true;
            }

            //profil réseau
            if (
                (etat == 1 || etat == 2 || etat == 6 || etat == 7 || etat == 8 || etat == 12 || etat == 15 || etat == 20 || etat == 25 || etat == 30 || etat == 40 || etat == 50) &&
                (u.EstRattacheAUnCentreVL)
               )
            {
                return true;
            }
            if (
                (etat == 1 || etat == 3 || etat == 6 || etat == 7 || etat == 8 || etat == 9 || etat == 10 || etat == 105 || etat == 110 || etat == 115 || etat == 120 || etat == 130 || etat == 140) &&
                (u.EstRattacheAUnCentrePL)
               )
            {
                return true;
            }

            return false;

        }

        public static bool isRightActionForCentreV2(IPrincipal utilisateurConnecte, pCentreEncour centre, ActionsFicheCentre action, rUtilisateur utilisateur)
        {
            Init();
            //todo : affiner les actions qui doivent vérifier les droits par type d'agrément (un à un)

            rPrefecture prefectureUser = new rPrefecture();
            if (utilisateur != null && utilisateur.RPrefecture != null)
            {
                prefectureUser = prefectureService.FindBy(utilisateur.RPrefecture.Id);
            }

            switch (action)
            {
                case ActionsFicheCentre.ADMMODFI:
                    //On doit être OTC
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()))
                    {
                        return true;
                    }
                    return false;

                case ActionsFicheCentre.ADMMODFINT:
                    //On doit être OTC
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()))
                    {
                        return true;
                    }
                    return false;

                case ActionsFicheCentre.ADMSUPFI:
                    //On doit être OTC
                    //(1)	SAUF si motif_fin_agrement_vl=2;3;4 et f_est_ancien_installation(id_centre_encours)=true
                    //CHANGEMENT_EXPLOITANT = 2,
                    //CHANGEMENT_LOCALISATION = 3,
                    //CHANGEMENT_RESEAU = 4,

                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()))
                    {
                        if (
                            centre.EtatAgrementVl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementVl.AGREMENT_KO))
                            &&
                            (
                                Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementVL.CHANGEMENT_EXPLOITANT).Equals(centre.MotifFinAgrementVl) ||
                                Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementVL.CHANGEMENT_LOCALISATION).Equals(centre.MotifFinAgrementVl) ||
                                Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementVL.CHANGEMENT_RESEAU).Equals(centre.MotifFinAgrementVl)
                            )
                            && centreService.isAncienCentre(centre)
                        )
                        {
                            return false;
                        }

                        if (
                            centre.EtatAgrementPl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_KO))
                            &&
                            (
                                Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementPL.CHANGEMENT_EXPLOITANT).Equals(centre.MotifFinAgrementPl) ||
                                Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementPL.CHANGEMENT_LOCALISATION).Equals(centre.MotifFinAgrementPl) ||
                                Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementPL.CHANGEMENT_RESEAU).Equals(centre.MotifFinAgrementPl)
                            )
                            && centreService.isAncienCentre(centre)
                        )
                        {
                            return false;
                        }

                        if (
                            centre.EtatAgrementCl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementCl.AGREMENT_KO))
                            &&
                            (
                                Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementCL.CHANGEMENT_EXPLOITANT).Equals(centre.MotifFinAgrementCl) ||
                                Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementCL.CHANGEMENT_LOCALISATION).Equals(centre.MotifFinAgrementCl) ||
                                Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementCL.CHANGEMENT_RESEAU).Equals(centre.MotifFinAgrementCl)
                            )
                            && centreService.isAncienCentre(centre)
                        )
                        {
                            return false;
                        }

                        return true;

                    }
                    return false;

                case ActionsFicheCentre.AMEND:
                    //TODO DRIRE => ORGANISME SURVEILLANCE dans r_profil2
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || utilisateurConnecte.IsInRole(rUtilisateur.Profil.DRIRE.ToString()))
                    {
                        if (centre.EtatAgrementVl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementVl.AGREMENT_OK)))
                        {
                            return true;
                        }

                        if (centre.EtatAgrementPl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_OK)) ||
                            centre.EtatAgrementPl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_RENOUVELE_OK))
                        )
                        {
                            return true;
                        }

                        if (centre.EtatAgrementCl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementCl.AGREMENT_OK)))
                        {
                            return true;
                        }

                    }
                    return false;

                case ActionsFicheCentre.ANUVL:
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || (utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) && prefectureUser == centre.RPrefecture))
                    {
                        if (centre.EtatAgrementVl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementVl.AGREMENT_OK)))
                        {
                            return true;
                        }
                    }
                    return false;

                case ActionsFicheCentre.ANUPL:
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || (utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) && prefectureUser == centre.RPrefecture))
                    {
                        if (centre.EtatAgrementPl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_OK)) ||
                            centre.EtatAgrementPl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_RENOUVELE_OK))
                        )
                        {
                            return true;
                        }
                    }
                    return false;

                case ActionsFicheCentre.ANUCL:
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || (utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) && prefectureUser == centre.RPrefecture))
                    {
                        if (centre.EtatAgrementCl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementCl.AGREMENT_OK)))
                        {
                            return true;
                        }
                    }
                    return false;

                case ActionsFicheCentre.CHDECOM:
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || (utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) && prefectureUser == centre.RPrefecture))
                    {
                        if (centre.EtatAgrementVl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementVl.AGREMENT_OK)) ||
                            centre.EtatAgrementVl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementVl.AGREMENT_DEMANDE_EN_COURS))
                        )
                        {
                            return true;
                        }

                        if (centre.EtatAgrementPl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_OK)) ||
                           centre.EtatAgrementPl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_DEMANDE_EN_COURS)) ||
                           centre.EtatAgrementPl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_RENOUVELEMENT_EN_COURS)) ||
                            centre.EtatAgrementPl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_RENOUVELE_OK))
                        )
                        {
                            return true;
                        }

                        if (centre.EtatAgrementCl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementCl.AGREMENT_OK)) ||
                          centre.EtatAgrementCl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementCl.AGREMENT_DEMANDE_EN_COURS))
                      )
                        {
                            return true;
                        }
                    }
                    return false;

                case ActionsFicheCentre.CHEXPLVL:
                    //ACTION "NOUVEL_AGREM" et motif de la fin d'agrément VL="2"// Changement d’exploitant
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || (utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) && prefectureUser == centre.RPrefecture))
                    {
                        if (centre.EtatAgrementVl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementVl.AGREMENT_KO)))
                        {
                            if (Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementVL.CHANGEMENT_EXPLOITANT).Equals(centre.MotifFinAgrementVl))
                                return true;
                        }
                    }
                    return false;

                case ActionsFicheCentre.CHLOCAVL:
                    //ACTION "NOUVEL_AGREM" et motif de la fin d'agrément VL="3"// Changement de localisation
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || (utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) && prefectureUser == centre.RPrefecture))
                    {
                        if (centre.EtatAgrementVl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementVl.AGREMENT_KO)))
                        {
                            if (Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementVL.CHANGEMENT_LOCALISATION).Equals(centre.MotifFinAgrementVl))
                                return true;
                        }
                    }
                    return false;

                case ActionsFicheCentre.CHRATTVL:
                    //ACTION "NOUVEL_AGREM" et motif de la fin d'agrément VL="4"// Changement de réseau de rattachement
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || (utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) && prefectureUser == centre.RPrefecture))
                    {
                        if (centre.EtatAgrementVl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementVl.AGREMENT_KO)))
                        {
                            if (Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementVL.CHANGEMENT_RESEAU).Equals(centre.MotifFinAgrementVl))
                                return true;
                        }
                    }
                    return false;

                case ActionsFicheCentre.NOUVEL_AGREM_VL:
                    //EG le 09/12/2011 : juste pour faire un combo pour éviter de tester les trois cas de figures dans la fiche du centre
                    //la fiche du centre ne doit pas avoir un duplicata
                    if (centreService.isAncienCentre(centre)) return false;

                    return isRightActionForCentre(utilisateurConnecte, centre, ActionsFicheCentre.CHEXPLVL) ||
                           isRightActionForCentre(utilisateurConnecte, centre, ActionsFicheCentre.CHLOCAVL) ||
                           isRightActionForCentre(utilisateurConnecte, centre, ActionsFicheCentre.CHRATTVL);

                case ActionsFicheCentre.CHEXPLPL:
                    //ACTION "NOUVEL_AGREM" et motif de la fin d'agrément VL="2"// Changement d’exploitant
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || (utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) && prefectureUser == centre.RPrefecture))
                    {
                        if (centre.EtatAgrementPl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_KO)))
                        {
                            if (Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementPL.CHANGEMENT_EXPLOITANT).Equals(centre.MotifFinAgrementPl))
                                return true;
                        }
                    }
                    return false;

                case ActionsFicheCentre.CHLOCAPL:
                    //ACTION "NOUVEL_AGREM" et motif de la fin d'agrément VL="3"// Changement d’exploitant
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || (utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) && prefectureUser == centre.RPrefecture))
                    {
                        if (centre.EtatAgrementPl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_KO)))
                        {
                            if (Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementPL.CHANGEMENT_LOCALISATION).Equals(centre.MotifFinAgrementPl))
                                return true;
                        }
                    }
                    return false;

                case ActionsFicheCentre.CHRATTPL:
                    //ACTION "NOUVEL_AGREM" et motif de la fin d'agrément VL="4"// Changement de réseau de rattachement
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || (utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) && prefectureUser == centre.RPrefecture))
                    {
                        if (centre.EtatAgrementPl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_KO)))
                        {
                            if (Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementPL.CHANGEMENT_RESEAU).Equals(centre.MotifFinAgrementPl))
                                return true;
                        }
                    }
                    return false;

                case ActionsFicheCentre.NOUVEL_AGREM_PL:
                    //EG le 09/12/2011 : juste pour faire un combo pour éviter de tester les trois cas de figures dans la fiche du centre
                    if (centreService.isAncienCentre(centre)) return false;

                    return isRightActionForCentre(utilisateurConnecte, centre, ActionsFicheCentre.CHEXPLPL) ||
                           isRightActionForCentre(utilisateurConnecte, centre, ActionsFicheCentre.CHLOCAPL) ||
                           isRightActionForCentre(utilisateurConnecte, centre, ActionsFicheCentre.CHRATTPL);

                case ActionsFicheCentre.CHEXPLCL:
                    //ACTION "NOUVEL_AGREM" et motif de la fin d'agrément VL="2"// Changement d’exploitant
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || (utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) && prefectureUser == centre.RPrefecture))
                    {
                        if (centre.EtatAgrementCl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementCl.AGREMENT_KO)))
                        {
                            if (Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementCL.CHANGEMENT_EXPLOITANT).Equals(centre.MotifFinAgrementCl))
                                return true;
                        }
                    }
                    return false;

                case ActionsFicheCentre.CHLOCACL:
                    //ACTION "NOUVEL_AGREM" et motif de la fin d'agrément VL="3"// Changement de localisation
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || (utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) && prefectureUser == centre.RPrefecture))
                    {
                        if (centre.EtatAgrementCl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementCl.AGREMENT_KO)))
                        {
                            if (Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementCL.CHANGEMENT_LOCALISATION).Equals(centre.MotifFinAgrementCl))
                                return true;
                        }
                    }
                    return false;

                case ActionsFicheCentre.CHRATTCL:
                    //ACTION "NOUVEL_AGREM" et motif de la fin d'agrément VL="4"// Changement de réseau de rattachement
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || (utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) && prefectureUser == centre.RPrefecture))
                    {
                        if (centre.EtatAgrementCl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementCl.AGREMENT_KO)))
                        {
                            if (Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementCL.CHANGEMENT_RESEAU).Equals(centre.MotifFinAgrementCl))
                                return true;
                        }
                    }
                    return false;

                case ActionsFicheCentre.NOUVEL_AGREM_CL:
                    //EG le 09/12/2011 : juste pour faire un combo pour éviter de tester les trois cas de figures dans la fiche du centre
                    if (centreService.isAncienCentre(centre)) return false;

                    return isRightActionForCentre(utilisateurConnecte, centre, ActionsFicheCentre.CHEXPLCL) ||
                           isRightActionForCentre(utilisateurConnecte, centre, ActionsFicheCentre.CHLOCACL) ||
                           isRightActionForCentre(utilisateurConnecte, centre, ActionsFicheCentre.CHRATTCL);

                case ActionsFicheCentre.CHREPLEG:
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || (utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) && prefectureUser == centre.RPrefecture))
                    {
                        if (
                           centre.EtatAgrementVl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementVl.AGREMENT_KO))
                           &&
                           (
                               Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementVL.CHANGEMENT_EXPLOITANT).Equals(centre.MotifFinAgrementVl) ||
                               Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementVL.CHANGEMENT_LOCALISATION).Equals(centre.MotifFinAgrementVl) ||
                               Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementVL.CHANGEMENT_RESEAU).Equals(centre.MotifFinAgrementVl)
                           )
                           && centreService.isAncienCentre(centre)
                       )
                        {
                            return false;
                        }

                        if (
                            centre.EtatAgrementPl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_KO))
                            &&
                            (
                                Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementPL.CHANGEMENT_EXPLOITANT).Equals(centre.MotifFinAgrementPl) ||
                                Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementPL.CHANGEMENT_LOCALISATION).Equals(centre.MotifFinAgrementPl) ||
                                Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementPL.CHANGEMENT_RESEAU).Equals(centre.MotifFinAgrementPl)
                            )
                            && centreService.isAncienCentre(centre)
                        )
                        {
                            return false;
                        }

                        if (
                            centre.EtatAgrementCl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementCl.AGREMENT_KO))
                            &&
                            (
                                Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementCL.CHANGEMENT_EXPLOITANT).Equals(centre.MotifFinAgrementCl) ||
                                Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementCL.CHANGEMENT_LOCALISATION).Equals(centre.MotifFinAgrementCl) ||
                                Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementCL.CHANGEMENT_RESEAU).Equals(centre.MotifFinAgrementCl)
                            )
                            && centreService.isAncienCentre(centre)
                        )
                        {
                            return false;
                        }

                        return true;

                    }
                    return false;

                case ActionsFicheCentre.CRECEVL:
                case ActionsFicheCentre.CRECEPL:
                case ActionsFicheCentre.CRECECL:
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()))
                    {
                        return true;
                    }
                    return false;

                case ActionsFicheCentre.DEAVL:
                    //EG le 12/12/2011 : prise en compte de la duplication d'une fiche centre
                    if (centreService.isAncienCentre(centre)) return false;

                    //EG le 29/11/2011 : prise en compte des profils autorisés
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || (utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) && prefectureUser == centre.RPrefecture))
                    {

                        if (
                                centre.EtatAgrementVl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementVl.AGREMENT_KO)) ||
                                centre.EtatAgrementVl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementVl.AGREMENT_REJETE))
                            )
                        {
                            if (centre.EtatAgrementVl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementVl.AGREMENT_KO)))
                            {
                                if (
                                    Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementVL.CHANGEMENT_EXPLOITANT).Equals(centre.MotifFinAgrementVl) ||
                                    Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementVL.CHANGEMENT_LOCALISATION).Equals(centre.MotifFinAgrementVl) ||
                                    Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementVL.CHANGEMENT_RESEAU).Equals(centre.MotifFinAgrementVl)
                                    )
                                {
                                    return false;
                                }
                            }
                            return true;
                        }
                    }

                    return false;

                case ActionsFicheCentre.DEAPL:
                    //EG le 12/12/2011 : prise en compte de la duplication d'une fiche centre
                    if (centreService.isAncienCentre(centre)) return false;

                    //EG le 29/11/2011 : prise en compte des profils autorisés
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || (utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) && prefectureUser == centre.RPrefecture))
                    {
                        if (
                                centre.EtatAgrementPl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_KO)) ||
                                centre.EtatAgrementPl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_REJETE)) ||
                                centre.EtatAgrementPl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_RENOUVELE_REJETE))
                            )
                        {
                            if (centre.EtatAgrementPl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_KO)))
                            {
                                if (
                                    Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementPL.CHANGEMENT_EXPLOITANT).Equals(centre.MotifFinAgrementPl) ||
                                    Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementPL.CHANGEMENT_LOCALISATION).Equals(centre.MotifFinAgrementPl) ||
                                    Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementPL.CHANGEMENT_RESEAU).Equals(centre.MotifFinAgrementPl)
                                    )
                                {
                                    return false;
                                }
                            }
                            return true;
                        }
                    }
                    return false;

                case ActionsFicheCentre.DEACL:
                    //EG le 12/12/2011 : prise en compte de la duplication d'une fiche centre
                    if (centreService.isAncienCentre(centre)) return false;

                    //EG le 29/11/2011 : prise en compte des profils autorisés
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || (utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) && prefectureUser == centre.RPrefecture))
                    {

                        if (
                                centre.EtatAgrementCl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementCl.AGREMENT_KO)) ||
                                centre.EtatAgrementCl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementCl.AGREMENT_REJETE))
                            )
                        {
                            if (centre.EtatAgrementCl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementCl.AGREMENT_KO)))
                            {
                                if (Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementCL.CHANGEMENT_EXPLOITANT).Equals(centre.MotifFinAgrementCl) ||
                                    Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementCL.CHANGEMENT_LOCALISATION).Equals(centre.MotifFinAgrementCl) ||
                                    Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementCL.CHANGEMENT_RESEAU).Equals(centre.MotifFinAgrementCl)
                                    )
                                {
                                    return false;
                                }
                            }
                            return true;
                        }
                    }
                    return false;

                case ActionsFicheCentre.DERPL:
                    if (centre.TypeCentre == pCentreEncour.TypeInstallation.A.ToString())
                    {
                        if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || (utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) && prefectureUser == centre.RPrefecture))
                        {
                            if (centre.EtatAgrementPl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_OK)) ||
                               centre.EtatAgrementPl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_RENOUVELE_OK))
                            )
                            {
                                return true;
                            }

                        }
                    }

                    return false;

                case ActionsFicheCentre.MODAUTRE:
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || (utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) && prefectureUser == centre.RPrefecture))
                    {
                        return true;
                    }
                    return false;

                case ActionsFicheCentre.NOTVL:
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || (utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) && prefectureUser == centre.RPrefecture))
                    {
                        if (centre.EtatAgrementVl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementVl.AGREMENT_DEMANDE_EN_COURS)))
                        {
                            return true;
                        }
                    }
                    return false;

                case ActionsFicheCentre.NOTPL:
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || (utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) && prefectureUser == centre.RPrefecture))
                    {
                        if (centre.EtatAgrementPl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_DEMANDE_EN_COURS)))
                        {
                            return true;
                        }
                    }
                    return false;

                case ActionsFicheCentre.NOTCL:
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || (utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) && prefectureUser == centre.RPrefecture))
                    {
                        if (centre.EtatAgrementCl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementCl.AGREMENT_DEMANDE_EN_COURS)))
                        {
                            return true;
                        }
                    }
                    return false;

                case ActionsFicheCentre.NOTRPL:
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || (utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) && prefectureUser == centre.RPrefecture))
                    {
                        if (centre.EtatAgrementPl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_RENOUVELEMENT_EN_COURS)))
                        {
                            return true;
                        }
                    }
                    return false;

                case ActionsFicheCentre.REJVL:
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || (utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) && prefectureUser == centre.RPrefecture))
                    {
                        if (centre.EtatAgrementVl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementVl.AGREMENT_DEMANDE_EN_COURS)))
                        {
                            return true;
                        }
                    }
                    return false;

                case ActionsFicheCentre.REJPL:
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || (utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) && prefectureUser == centre.RPrefecture))
                    {
                        if (centre.EtatAgrementPl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_DEMANDE_EN_COURS)))
                        {
                            return true;
                        }
                    }
                    return false;

                case ActionsFicheCentre.REJRPL:
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || (utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) && prefectureUser == centre.RPrefecture))
                    {
                        if (centre.EtatAgrementPl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_RENOUVELEMENT_EN_COURS)))
                        {
                            return true;
                        }
                    }
                    return false;

                case ActionsFicheCentre.REJCL:
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || (utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) && prefectureUser == centre.RPrefecture))
                    {
                        if (centre.EtatAgrementCl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementCl.AGREMENT_DEMANDE_EN_COURS)))
                        {
                            return true;
                        }
                    }
                    return false;

                case ActionsFicheCentre.RETOUR:
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || (utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) && prefectureUser == centre.RPrefecture))
                    {
                        //SI La dernière action appartient à la liste suivante : CRECEVL, CRECEPL, CRECECL,*ADMMODFI*, RETOUR, SANCT, AMEND, CHHORAI, RDV		
                        if (
                            centre.RMotifModification.Equals(rMotifModification.TypeModification.ADMMODFI) ||
                            centre.RMotifModification.Equals(rMotifModification.TypeModification.CRECEVL) ||
                            centre.RMotifModification.Equals(rMotifModification.TypeModification.CRECEPL) ||
                            centre.RMotifModification.Equals(rMotifModification.TypeModification.CRECECL) ||
                            centre.RMotifModification.Equals(rMotifModification.TypeModification.RETOUR) ||
                            centre.RMotifModification.Equals(rMotifModification.TypeModification.SANCT) ||
                            centre.RMotifModification.Equals(rMotifModification.TypeModification.AMEND) ||
                            centre.RMotifModification.Equals(rMotifModification.TypeModification.CHHORAI) ||
                            centre.RMotifModification.Equals(rMotifModification.TypeModification.RDV)
                            )
                        {
                            return false;
                        }

                        return true;

                    }
                    return false;

                case ActionsFicheCentre.SANCT:
                    if (centre.AunAgrementValide)
                    {
                        if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()))
                        {
                            return true;
                        }
                        rUtilisateur u = utilisateurService.FindBy(utilisateurConnecte.Identity.Name);
                        if (centre.RPrefecture.Equals(u.RPrefecture))
                        {
                            return true;
                        }
                    }
                    return false;

                //if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()))
                //{
                //    rUtilisateur u = utilisateurService.FindBy(utilisateurConnecte.Identity.Name);

                //    if (centre.EtatAgrementVl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementVl.AGREMENT_OK)))
                //    {
                //        if (centre.RPrefecture.Equals(u.RPrefecture))
                //        {
                //            return true;
                //        }
                //        else
                //        {
                //            return false;
                //        }
                //    }

                //    if (
                //        centre.EtatAgrementPl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_OK)) ||
                //        centre.EtatAgrementPl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_RENOUVELE_OK))
                //       )
                //    {
                //        if (centre.RPrefecture.Equals(u.RPrefecture))
                //        {
                //            return true;
                //        }
                //        else
                //        {
                //            return false;
                //        }
                //    }

                //    if (centre.EtatAgrementCl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementCl.AGREMENT_OK)))
                //    {
                //        if (centre.RPrefecture.Equals(u.RPrefecture))
                //        {
                //            return true;
                //        }
                //        else
                //        {
                //            return false;
                //        }
                //    }
                //}
                //return false;

                case ActionsFicheCentre.SHOW_SANCT:
                    if (
                        utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) ||
                        utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) ||
                        utilisateurConnecte.IsInRole(rUtilisateur.Profil.DRIRE.ToString())
                        )
                    {
                        //if (centre.EtatAgrementVl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementVl.AGREMENT_OK)))
                        //{
                        //    return true;
                        //}

                        //if (
                        //    centre.EtatAgrementPl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_OK)) ||
                        //    centre.EtatAgrementPl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_RENOUVELE_OK))
                        //   )
                        //{
                        //    return true;
                        //}

                        //if (centre.EtatAgrementCl.Equals(Convert.ToString((int)pCentreEncour.CodeEtatAgrementCl.AGREMENT_OK)))
                        //{
                        return true;
                        //}
                    }
                    return false;

                case ActionsFicheCentre.DEROG_VL:
                    if (centre.EstAgrementValideVL)
                    {
                        if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()))
                        {
                            return true;
                        }
                        rUtilisateur u = utilisateurService.FindBy(utilisateurConnecte.Identity.Name);
                        //Profil prefecture
                        if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()))
                        {
                            if (centre.RPrefecture.Equals(u.RPrefecture))
                            {
                                return true;
                            }
                        }
                        //Profil DREAL
                        if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.DRIRE.ToString()))
                        {
                            if (centre.RPrefecture.RRegion.Equals(u.RRegion))
                            {
                                return true;
                            }
                        }
                    }
                    return false;

                case ActionsFicheCentre.DEROG_PL:
                    if (centre.EstAgrementValidePL)
                    {
                        if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()))
                        {
                            return true;
                        }
                        rUtilisateur u = utilisateurService.FindBy(utilisateurConnecte.Identity.Name);
                        //Profil prefecture
                        if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()))
                        {
                            if (centre.RPrefecture.Equals(u.RPrefecture))
                            {
                                return true;
                            }
                        }
                        //Profil DREAL
                        if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.DRIRE.ToString()))
                        {
                            if (centre.RPrefecture.RRegion.Equals(u.RRegion))
                            {
                                return true;
                            }
                        }
                    }
                    return false;

                //MODIF DEROG
                case ActionsFicheCentre.MOD_DEROG_VL:
                    //Pour admin tout le temps
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()))
                    {
                        return true;
                    }

                    if (centre.EstAgrementValideVL)
                    {

                        rUtilisateur u = utilisateurService.FindBy(utilisateurConnecte.Identity.Name);
                        //Profil prefecture
                        if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()))
                        {
                            if (centre.RPrefecture.Equals(u.RPrefecture))
                            {
                                return true;
                            }
                        }
                        //Profil DREAL
                        if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.DRIRE.ToString()))
                        {
                            if (centre.RPrefecture.RRegion.Equals(u.RRegion))
                            {
                                return true;
                            }
                        }
                    }
                    return false;

                case ActionsFicheCentre.MOD_DEROG_PL:
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()))
                    {
                        return true;
                    }

                    if (centre.EstAgrementValidePL)
                    {
                        rUtilisateur u = utilisateurService.FindBy(utilisateurConnecte.Identity.Name);
                        //Profil prefecture
                        if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()))
                        {
                            if (centre.RPrefecture.Equals(u.RPrefecture))
                            {
                                return true;
                            }
                        }
                        //Profil DREAL
                        if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.DRIRE.ToString()))
                        {
                            if (centre.RPrefecture.RRegion.Equals(u.RRegion))
                            {
                                return true;
                            }
                        }
                    }
                    return false;

                case ActionsFicheCentre.SHOW_DEROG:
                    if (
                        utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) ||
                        utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) ||
                        utilisateurConnecte.IsInRole(rUtilisateur.Profil.DRIRE.ToString())
                        )
                    {
                        return true;
                    }
                    return false;

                case ActionsFicheCentre.EQUIPVL:
                    return isRightForCentreAppartenanceAndEtatAgrementCommonVL(utilisateurConnecte, centre);

                case ActionsFicheCentre.EQUIPPL:
                    return isRightForCentreAppartenanceAndEtatAgrementCommonPL(utilisateurConnecte, centre);

                case ActionsFicheCentre.EQUIPCL:
                    return isRightForCentreAppartenanceAndEtatAgrementCommonCL(utilisateurConnecte, centre);

                case ActionsFicheCentre.SHOW_EQUIPVL:
                    if (isRightForCentreAppartenanceAndEtatAgrementCommonVL(utilisateurConnecte, centre))
                    {
                        return true;
                    }
                    else
                    {
                        if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.DRIRE.ToString()) || utilisateurConnecte.IsInRole(rUtilisateur.Profil.DSCR.ToString()))
                        {
                            return true;
                        }
                    }
                    return false;

                case ActionsFicheCentre.SHOW_EQUIPPL:
                    if (isRightForCentreAppartenanceAndEtatAgrementCommonPL(utilisateurConnecte, centre))
                    {
                        return true;
                    }
                    else
                    {
                        if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.DRIRE.ToString()) || utilisateurConnecte.IsInRole(rUtilisateur.Profil.DSCR.ToString()))
                        {
                            return true;
                        }
                    }
                    return false;

                case ActionsFicheCentre.SHOW_EQUIPCL:
                    if (isRightForCentreAppartenanceAndEtatAgrementCommonCL(utilisateurConnecte, centre))
                    {
                        return true;
                    }
                    else
                    {
                        if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.DRIRE.ToString()) || utilisateurConnecte.IsInRole(rUtilisateur.Profil.DSCR.ToString()))
                        {
                            return true;
                        }
                    }
                    return false;

                //EG le 01/12/2011
                case ActionsFicheCentre.TELECHARGE_CERTIF_SSL_VL:
                    //que pour un centre indépendant
                    if (rReseau.IdCodeReseau.Z.ToString().Equals(centre.ReseauRattachementVL.Id))
                    {
                        if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Informatique.ToString()))
                        {
                            return true;
                        }

                        //CENTRE INDEPENDANT
                        //06/06/2013 PLE Interdiction de télécharger un certificat pour une centre non valide
                        if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.CENTRE_INDEPENDANT.ToString()) && centre.EstAgrementValideVL)
                        {
                            rUtilisateur u = utilisateurService.FindBy(utilisateurConnecte.Identity.Name);
                            if (centre.Equals(u.PCentreEncourVL))
                            {
                                return true;
                            }
                        }
                    }

                    return false;

                //EG le 01/12/2011
                case ActionsFicheCentre.TELECHARGE_CERTIF_SSL_PL:
                    //que pour un centre indépendant
                    if (rReseau.IdCodeReseau.Z.ToString().Equals(centre.ReseauRattachementPL.Id))
                    {
                        if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Informatique.ToString()))
                        {
                            return true;
                        }

                        //CENTRE INDEPENDANT
                        //06/06/2013 PLE Interdiction de télécharger un certificat pour une centre non valide
                        if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.CENTRE_INDEPENDANT.ToString()) && centre.EstAgrementValidePL)
                        {
                            rUtilisateur u = utilisateurService.FindBy(utilisateurConnecte.Identity.Name);
                            if (centre.Equals(u.PCentreEncourPL))
                            {
                                return true;
                            }
                        }
                    }

                    return false;

                //EG le 01/12/2011
                case ActionsFicheCentre.TELECHARGE_CERTIF_SSL_CL:
                    //que pour un centre indépendant
                    if (centre.EstAgrementValideCL && rReseau.IdCodeReseau.Z.ToString().Equals(centre.ReseauRattachementCL.Id))
                    {
                        if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()))
                        {
                            return true;
                        }

                        //CENTRE INDEPENDANT
                        if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.CENTRE_INDEPENDANT.ToString()))
                        {
                            rUtilisateur u = utilisateurService.FindBy(utilisateurConnecte.Identity.Name);
                            if (centre.Equals(u.PCentreEncourCL))
                            {
                                return true;
                            }
                        }
                    }

                    return false;

                case ActionsFicheCentre.REVOCATION_CERTIF:

                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()))
                    {
                        return true;
                    }

                    return false;

                //EG le 05/12/2011
                case ActionsFicheCentre.TELECHARGE_CE_PL:
                    //Seulement les centres indépendants
                    if (centre.EstAgrementValidePL && "Z".Equals(centre.ReseauRattachementPL.Id))
                    {
                        if (
                             utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) ||
                             utilisateurConnecte.IsInRole(rUtilisateur.Profil.DRIRE.ToString()) ||
                             utilisateurConnecte.IsInRole(rUtilisateur.Profil.DSCR.ToString())
                            )
                        {
                            return true;
                        }
                        else
                        {
                            if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.CENTRE_INDEPENDANT.ToString()))
                            {
                                //rUtilisateur u = utilisateurService.FindBy(utilisateurConnecte.Identity.Name);
                                if (centre.Equals(utilisateur.PCentreEncourPL))
                                {
                                    return true;
                                }
                            }
                        }

                    }

                    return false;

                case ActionsFicheCentre.TELECHARGE_CE_VL:
                    if (centre.EstAgrementValideVL)
                    {
                        if (
                             utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) ||
                             utilisateurConnecte.IsInRole(rUtilisateur.Profil.DRIRE.ToString()) ||
                             utilisateurConnecte.IsInRole(rUtilisateur.Profil.DSCR.ToString())
                            )
                        {
                            return true;
                        }
                        else
                        {
                            if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.CENTRE_INDEPENDANT.ToString()))
                            {
                                //rUtilisateur u = utilisateurService.FindBy(utilisateurConnecte.Identity.Name);
                                if (centre.Equals(utilisateur.PCentreEncourVL))
                                {
                                    return true;
                                }
                            }

                            if (
                                utilisateurConnecte.IsInRole(rUtilisateur.Profil.RESEAU_Gestionnaire.ToString()) ||
                                utilisateurConnecte.IsInRole(rUtilisateur.Profil.RESEAU_Administrateur.ToString()) ||
                                utilisateurConnecte.IsInRole(rUtilisateur.Profil.RESEAU_Consultation.ToString())
                                )
                            {
                                //rUtilisateur u = utilisateurService.FindBy(utilisateurConnecte.Identity.Name);

                                bool autorise = false;
                                foreach (rReseau r in utilisateur.RReseaux)
                                {
                                    if (r.Id.Equals(centre.ReseauRattachementVL.Id))
                                    {
                                        autorise = true;
                                        break;
                                    }
                                }

                                return autorise;
                            }
                        }

                    }
                    return false;

                case ActionsFicheCentre.SHOW_HISTO:
                    //EG le 22/02/2012 : accès à l'historique dans tous les cas
                    if (
                        utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) ||
                        utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) ||
                        utilisateurConnecte.IsInRole(rUtilisateur.Profil.DRIRE.ToString()) ||
                        utilisateurConnecte.IsInRole(rUtilisateur.Profil.DSCR.ToString()) ||
                        utilisateurConnecte.IsInRole(rUtilisateur.Profil.PIV.ToString())

                        )
                    {
                        //if (centre.AunAgrementValide)
                        //{
                        return true;
                        //}
                    }

                    return false;

                case ActionsFicheCentre.SHOW_AMEND:
                    if (
                        utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) ||
                        utilisateurConnecte.IsInRole(rUtilisateur.Profil.DRIRE.ToString()) ||
                        utilisateurConnecte.IsInRole(rUtilisateur.Profil.DSCR.ToString()) ||
                        utilisateurConnecte.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) ||
                        utilisateurConnecte.IsInRole(rUtilisateur.Profil.PIV.ToString())
                        )
                    {
                        if (centre.AunAgrementValide)
                        {
                            return true;
                        }
                    }

                    return false;

                case ActionsFicheCentre.SHOW_CONTROLEUR:
                    //Tout le monde
                    return true;

                case ActionsFicheCentre.RDV:
                    //EG le 14/12/2011 : seulement pour les centres indépendants , pour les réseaux cette action ne sera pas vérifiée
                    return isRightForCentreAppartenanceAndEtatAgrementCommonPL(utilisateurConnecte, centre) && rReseau.IdCodeReseau.Z.ToString().Equals(centre.ReseauRattachementPL.Id);

                //EG le 14/12/2011
                case ActionsFicheCentre.CHHORAI:
                    return isRightForCentreAppartenanceAndEtatAgrementCommonPL(utilisateurConnecte, centre) && pCentreEncour.TypeInstallation.A.ToString().Equals(centre.TypeCentre);

                case ActionsFicheCentre.SHOW_HORAIRE_OUVERTURE:
                    if (isRightForCentreAppartenanceAndEtatAgrementCommonPL(utilisateurConnecte, centre) && pCentreEncour.TypeInstallation.A.ToString().Equals(centre.TypeCentre))
                    {
                        return true;
                    }
                    if (centre.EstAgrementValidePL && pCentreEncour.TypeInstallation.A.ToString().Equals(centre.TypeCentre))
                    {
                        if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.DRIRE.ToString()))
                        {
                            return true;
                        }
                    }

                    return false;

                //EG le 19/12/2011
                case ActionsFicheCentre.JOURNAL:
                    if (utilisateurConnecte.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()))
                    {
                        return true;
                    }
                    return false;

                default:
                    return false;
            }

        }

    }
}
