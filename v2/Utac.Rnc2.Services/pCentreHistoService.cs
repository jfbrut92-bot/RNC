using System;
using System.Collections.Generic;
using System.Linq;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Contrats.Repositories;
using Utac.Rnc2.Contrats.Services;
using Utac.Rnc2.Infrastructure;
using Utac.Rnc2.Infrastructure.Queries;

namespace Utac.Rnc2.Services
{
    public class pCentreHistoService : BaseService<pCentreHisto>, IpCentreHistoService
    {

        public pCentreHistoService(IpCentreHistoRepository repository, IUnitOfWork u)
            : base(repository, u)
        {

        }

        public void historise(pCentreEncour c)
        {
            pCentreHisto centrehisto = new pCentreHisto();
            centrehisto.IdCentreHisto = c.Id;
            centrehisto.DateHisto = c.DateMiseaJour;
            centrehisto.RaisonSociale = c.RaisonSociale;
            centrehisto.NumSiren = c.NumSiren;
            centrehisto.EnseigneCommerciale = c.EnseigneCommerciale;
            centrehisto.L1Adresse = c.L1Adresse;
            centrehisto.L2Adresse = c.L2Adresse;
            centrehisto.CpCentre = c.CpCentre;
            centrehisto.Ville = c.Ville;
            centrehisto.NumTel = c.NumTel;
            centrehisto.NomRepresentantLegal = c.NomRepresentantLegal;
            centrehisto.TypeCentre = c.TypeCentre;
            centrehisto.CodDepartement = c.CodDepartement;
            centrehisto.NumOrdrePl = c.NumOrdrePl;
            centrehisto.NumOrdreVl = c.NumOrdreVl;
            centrehisto.CodAgremPl = c.CodAgremPl;
            centrehisto.CodAgremVl = c.CodAgremVl;
            centrehisto.CodAgremCl = c.CodAgremCl;
            centrehisto.AncienNumeroAgrementVl = c.AncienNumeroAgrementVl;
            centrehisto.AncienNumeroAgrementPl = c.AncienNumeroAgrementPl;
            centrehisto.AncienNumeroAgrementCl = c.AncienNumeroAgrementCl;
            centrehisto.DateEffetNotificationPl = c.DateEffetNotificationPl;
            centrehisto.DateEffetNotificationVl = c.DateEffetNotificationVl;
            centrehisto.DateEffetNotificationCl = c.DateEffetNotificationCl;
            centrehisto.DateFinAgrementPl = c.DateFinAgrementPl;
            centrehisto.DateEcheanceAgrementPl = c.DateEcheanceAgrementPl;
            centrehisto.DateFinAgrementVl = c.DateFinAgrementVl;
            //PLE le 01/09/2025 Ajout de l'hitorisation du la date de fin agrément CL
            centrehisto.DateFinAgrementCl = c.DateFinAgrementCl;
            //PLE le 01/09/2025 Ajout de l'hitorisation du motif Fin Agrement Vl
            centrehisto.MotifFinAgrementVl = c.MotifFinAgrementVl;
            centrehisto.MotifFinAgrementPl = c.MotifFinAgrementPl;
            centrehisto.MotifFinAgrementCl = c.MotifFinAgrementCl;
            centrehisto.AvisOtcVl = c.AvisOtcVl;
            centrehisto.ReferenceAvisOtcVl = c.ReferenceAvisOtcVl;
            centrehisto.DateAvisOtcVl = c.DateAvisOtcVl;
            centrehisto.CommentaireAvisOtcVl = c.CommentaireAvisOtcVl;
            centrehisto.EtatAgrementVl = c.EtatAgrementVl;
            centrehisto.EtatAgrementPl = c.EtatAgrementPl;
            centrehisto.EtatAgrementCl = c.EtatAgrementCl;
            centrehisto.AvisOtcPl = c.AvisOtcPl;
            centrehisto.ReferenceAvisOtcPl = c.ReferenceAvisOtcPl;
            centrehisto.DateAvisOtcPl = c.DateAvisOtcPl;
            centrehisto.CommentaireAvisOtcPl = c.CommentaireAvisOtcPl;
            centrehisto.CommentaireAvisOtcCl = c.CommentaireAvisOtcCl;
            centrehisto.DateEffetModification = c.DateEffetModification;
            centrehisto.NumFax = c.NumFax;
            centrehisto.RPrefecture = c.RPrefecture;
            centrehisto.RMotifModification = c.RMotifModification;
            centrehisto.RRegroupementIndependant = c.RRegroupementIndependant;
            //EG le 11/08/2011 suppression de la colonne
            //centrehisto.RReseau = c.RReseau;
            centrehisto.RStatutJuridique = c.RStatutJuridique;
            centrehisto.RUtilisateur = c.RUtilisateur;
            //EG le 11/08/2011 nouvelles colonnes
            centrehisto.ReseauRattachementVL = c.ReseauRattachementVL;
            centrehisto.ReseauRattachementPL = c.ReseauRattachementPL;
            centrehisto.ReseauRattachementCL = c.ReseauRattachementCL;
            centrehisto.NumAccuseReceptionVL = c.NumAccuseReceptionVL;
            centrehisto.NumAccuseReceptionPL = c.NumAccuseReceptionPL;
            centrehisto.NumAccuseReceptionCL = c.NumAccuseReceptionCL;
            //EG le 06/12/2011
            centrehisto.AccesPublicVl = c.AccesPublicVl;
            centrehisto.AccesPublicPl = c.AccesPublicPl;
            centrehisto.AccesPublicCl = c.AccesPublicCl;
            //PLE le 19/01/2024
            centrehisto.ExtAgremClCatVeh = c.ExtAgremClCatVeh;
            centrehisto.ExtAgremClDateDecla = c.ExtAgremClDateDecla;
            centrehisto.ExtAgremClDateValid = c.ExtAgremClDateValid;
            centrehisto.ExtAgremClStatut = c.ExtAgremClStatut;
            //PLE le 08/04/2025
            centrehisto.SvaVL = c.SvaVL;
            centrehisto.SvaPL = c.SvaPL;
            centrehisto.SvaCL = c.SvaCL;
            this.Save(centrehisto);

        }

        public pCentreHisto findLastBy(object id)
        {
            IpCentreHistoRepository r = this._Repository as IpCentreHistoRepository;
            return r.findLastBy(id);
        }

        public pCentreHisto findByIdAndDate(object id, DateTime dateHisto)
        {
            Query q = new Query();
            q.Criteria.Add(new Criteria("IdCentreHisto", id, CriteriaOperator.EQUALS));
            //q.Criteria.Add(new Criteria("DateHisto", dateHisto, CriteriaOperator.EQUALS));
            //Pour éviter le bug suivant : 06/07/2005 15:31:44 -> NHibernate ne fournit pas les millisecondes et ASE retourne NULL
            DateTime to = dateHisto.AddSeconds(1);
            q.Criteria.Add(new Criteria("DateHisto", dateHisto, CriteriaOperator.GE));
            q.Criteria.Add(new Criteria("DateHisto", to, CriteriaOperator.LT));

            IList<pCentreHisto> liste = this.FindBy(q);
            if (liste.Count > 0)
            {
                return liste.ElementAt(0);
            }
            else
                return null;

        }

        public void removeAll(pCentreEncour c)
        {
            IpCentreHistoRepository r = this._Repository as IpCentreHistoRepository;
            r.removeAll(c);
        }

        public override void Remove(pCentreHisto histo)
        {
            IpCentreHistoRepository r = this._Repository as IpCentreHistoRepository;
            r.Remove(histo);
        }

    }
}
