using System;
using System.Collections.Generic;
using System.Linq;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Contrats.Repositories;
using Utac.Rnc2.Contrats.Services;
using Utac.Rnc2.Infrastructure;
using Utac.Rnc2.Infrastructure.Queries;
using Utac.Rnc2.Repository;

namespace Utac.Rnc2.Services
{
    public class pControleurHistoService : BaseService<pControleurHisto>, IpControleurHistoService
    {

        public pControleurHistoService(IpControleurHistoRepository controleurHistRepository, IUnitOfWork UnitOfWork)
            : base(controleurHistRepository, UnitOfWork)
        {

        }

        public pControleurHisto historise(pContoleurEncour controleurEnCours, pContoleurEncour controleurForm, rTypeDemandeControleur.TypeModification typeModification)
        {
            pControleurHisto histo = this.historise(controleurEnCours);
            switch (typeModification)
            {
                case rTypeDemandeControleur.TypeModification.CHCEVL:
                    histo.DateFinRattachementVl = controleurForm.DateEffetNotificationVl;
                    break;
                case rTypeDemandeControleur.TypeModification.CHCEPL:
                    histo.DateFinRattachementPl = controleurForm.DateEffetNotificationPl;
                    break;
                case rTypeDemandeControleur.TypeModification.CHCECL:
                    histo.DateFinRattachementCl = controleurForm.DateEffetNotificationCl;
                    break;
            }

            //EG le 25/11/2011 
            this.Save(histo);

            return histo;

        }

        public pControleurHisto historise(pContoleurEncour c)
        {
            pControleurHisto controleurhisto = new pControleurHisto();
            controleurhisto.IdControleurHisto = c.Id;
            controleurhisto.AvisOtcPl = c.AvisOtcPl;
            controleurhisto.AvisOtcVl = c.AvisOtcVl;
            controleurhisto.CodAgremPl = c.CodAgremPl;
            controleurhisto.CodAgremRattachementPl = c.CodAgremRattachementPl;
            controleurhisto.CodAgremRattachementVl = c.CodAgremRattachementVl;
            controleurhisto.CodAgremVl = c.CodAgremVl;
            controleurhisto.CodDepartement = c.CodDepartement;
            controleurhisto.CommentaireAvisOtcPl = c.CommentaireAvisOtcPl;
            controleurhisto.CommentaireAvisOtcVl = c.CommentaireAvisOtcVl;
            controleurhisto.DateAvisOtcPl = c.DateAvisOtcPl;
            controleurhisto.DateAvisOtcVl = c.DateAvisOtcVl;
            controleurhisto.DateDebutHabilitationPl = c.DateDebutHabilitationPl;
            controleurhisto.DateDebutRattachementPl = c.DateDebutRattachementPl;
            controleurhisto.DateDebutRattachementVl = c.DateDebutRattachementVl;
            controleurhisto.DateEffetNotificationPl = c.DateEffetNotificationPl;
            controleurhisto.DateEffetNotificationVl = c.DateEffetNotificationVl;
            controleurhisto.DateFinAgrementPl = c.DateFinAgrementPl;
            controleurhisto.DateFinAgrementVl = c.DateFinAgrementVl;
            controleurhisto.DateFinRattachementPl = c.DateFinRattachementPl;
            controleurhisto.DateFinRattachementVl = c.DateFinRattachementVl;
            controleurhisto.DateHisto = c.DateMiseaJour;
            controleurhisto.DateNaissance = c.DateNaissance;
            controleurhisto.EtatAgrementPl = c.EtatAgrementPl;
            controleurhisto.EtatAgrementVl = c.EtatAgrementVl;
            controleurhisto.MotifFinAgrementPl = c.MotifFinAgrementPl;
            controleurhisto.MotifFinAgrementVl = c.MotifFinAgrementVl;
            controleurhisto.Nom = c.Nom;
            controleurhisto.NumOrdrePl = c.NumOrdrePl;
            controleurhisto.NumOrdreVl = c.NumOrdreVl;
            controleurhisto.Prenom = c.Prenom;
            controleurhisto.ReferenceAvisOtcPl = c.ReferenceAvisOtcPl;
            controleurhisto.ReferenceAvisOtcVl = c.ReferenceAvisOtcVl;
            controleurhisto.RHabilitation = c.RHabilitation;
            controleurhisto.RPrefecture = c.RPrefecture;
            controleurhisto.RTypeDemandeControleur = c.RTypeDemandeControleur;
            controleurhisto.RUtilisateur = c.RUtilisateur;
            //EG le 13/07/2011 : prise en compte du CL
            controleurhisto.PCentreEncourCL = c.PCentreEncourCL;
            controleurhisto.CodAgremRattachementCl = c.CodAgremRattachementCl;
            controleurhisto.DateDebutRattachementCl = c.DateDebutRattachementCl;
            controleurhisto.DateFinRattachementCl = c.DateFinRattachementCl;
            controleurhisto.DateEffetNotificationCl = c.DateEffetNotificationCl;
            controleurhisto.CodAgremCl = c.CodAgremCl;
            controleurhisto.DateFinAgrementCl = c.DateFinAgrementCl;
            controleurhisto.MotifFinAgrementCl = c.MotifFinAgrementCl;
            controleurhisto.EtatAgrementCl = c.EtatAgrementCl;
            controleurhisto.CommentaireAvisOtcCl = c.CommentaireAvisOtcCl;
            controleurhisto.NumOrdreCl = c.NumOrdreCl;
            //EG le 16/09/2011 : habilitation gaz
            controleurhisto.HabilitationGazVL = c.HabilitationGazVL;
            controleurhisto.DateDebutFinHabilitationGazVL = c.DateDebutFinHabilitationGazVL;
            //EG le 05/10/2011
            //controleurhisto.PContoleurEncour = controleurEnCours;

            //PLE le 19/01/2024
            controleurhisto.ExtAgremClCatVeh = c.ExtAgremClCatVeh;
            controleurhisto.ExtAgremClDateDecla = c.ExtAgremClDateDecla;
            controleurhisto.ExtAgremClDateValid = c.ExtAgremClDateValid;
            controleurhisto.ExtAgremClStatut = c.ExtAgremClStatut;
            //PLE le 11/04/2025 Ajout SVA
            controleurhisto.SvaVL = c.SvaVL;
            controleurhisto.SvaPL = c.SvaPL;
            controleurhisto.SvaCL = c.SvaCL;

            //EG le 25/11/2011 on fait plus le Save 
            this.Save(controleurhisto);
            //controleurhisto.PContoleurEncour = c;

            return controleurhisto;
        }

        public pControleurHisto findLastBy(object id)
        {
            Query q = new Query();
            //filtrer sur l'id
            q.Criteria.Add(new Criteria("IdControleurHisto", id, CriteriaOperator.EQUALS));

            //trier par DateHisto
            q.OrderBy.Add(new OrderBy() { PropertyName = "DateHisto", Ascending = false });

            IList<pControleurHisto> liste = this.FindBy(q);

            if (liste.Count > 0)
            {
                return liste.ElementAt(0);
            }
            else
                return null;
        }

        public pControleurHisto findByIdAndDate(object id, DateTime dateHisto)
        {
            Query q = new Query();
            q.Criteria.Add(new Criteria("IdControleurHisto", id, CriteriaOperator.EQUALS));
            //q.Criteria.Add(new Criteria("DateHisto", dateHisto, CriteriaOperator.EQUALS));
            //Pour éviter le bug suivant : 06/07/2005 15:31:44 -> NHibernate ne fournit pas les millisecondes et ASE retourne NULL cf, Controleur ID = 1700
            DateTime to = dateHisto.AddSeconds(1);
            q.Criteria.Add(new Criteria("DateHisto", dateHisto, CriteriaOperator.GE));
            q.Criteria.Add(new Criteria("DateHisto", to, CriteriaOperator.LT));

            IList<pControleurHisto> liste = this.FindBy(q);
            if (liste.Count > 0)
            {
                return liste.ElementAt(0);
            }
            else
                return null;

        }

        public void removeAll(pContoleurEncour c)
        {
            var r = this._Repository as pControleurHistoRepository;
            r.removeAll(c);
        }

        //Override pour éviter le bug NHibernate.StaleStateException: Unexpected row count: 0; expected: 1 à cause millisecondes dans date_histo
        public override void Remove(pControleurHisto histo)
        {
            var r = this._Repository as pControleurHistoRepository;
            r.Remove(histo);
        }
    }
}
