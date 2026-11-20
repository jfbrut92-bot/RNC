using System;
using System.Collections.Generic;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Contrats.Repositories;
using Utac.Rnc2.Contrats.Services;
using Utac.Rnc2.Infrastructure;
using Utac.Rnc2.Infrastructure.Queries;

namespace Utac.Rnc2.Services
{
    public class pTraceRechPvVlService : BaseService<pTraceRechPvVl>, IpTraceRechPvVlService
    {

        private readonly IrUtilisateurService _utilisateurService;

        public pTraceRechPvVlService(
            IpTraceRechPvVlRepository repository,
            IUnitOfWork UnitOfWork,
            IrUtilisateurService utilisateurService
            )
            : base(repository, UnitOfWork)
        {
            this._utilisateurService = utilisateurService;
        }

        public void TracerAppel(pTraceRechPvVl trace, string codeUtilisateur)
        {
            IUnitOfWork transaction = this.getUnitOfWork();

            // On renseigne la cle primaire de l'audit
            trace.DateRecherche = System.DateTime.Now;

            //utilisateur qui a fait la modification
            trace.RUtilisateur = this._utilisateurService.FindBy(codeUtilisateur);
            // On sauvegarde l'audit
            this.Save(trace);

            if (transaction != null)
                transaction.Commit();

        }

        //public pTraceRechPvVl  nouvelleTrace(pTraceRechPvVl trace, string codeUtilisateur)
        //{
        //    IUnitOfWork transaction = this.getUnitOfWork();

        //    // On renseigne la cle primaire de l'audit
        //    trace.DateRecherche = System.DateTime.Now;

        //    //utilisateur qui a fait la modification
        //    trace.RUtilisateur = this._utilisateurService.FindBy(codeUtilisateur);
        //    // On sauvegarde l'audit
        //    this.Save(trace);

        //    if (transaction != null)
        //        transaction.Commit();

        //    return trace;
        //}

        //public pTraceRechPvVl modifierTrace(int id, pTraceRechPvVl trace, string codeUtilisateur)
        //{
        //    IUnitOfWork transaction = this.getUnitOfWork();

        //    // On sauvegarde l'audit
        //    pTraceRechPvVl t = this.FindBy(id);
        //    t.NumPvCtrl = trace.NumPvCtrl;
        //    t.CodAgremCentre = trace.CodAgremCentre;
        //    t.DateCtrl = trace.DateCtrl;
        //    t.TypeRecherche = trace.TypeRecherche;
        //    t.CodResultat = trace.CodResultat;
        //    t.RUtilisateur = this._utilisateurService.FindBy(codeUtilisateur);
        //    this.Save(t);

        //    if (transaction != null)
        //        transaction.Commit();

        //    return t;
        //}

        //public void  deleteTrace(pTraceRechPvVl trace)
        //{
        //    IUnitOfWork transaction = this.getUnitOfWork();

        //    this.Remove(trace);

        //    if (transaction != null)
        //        transaction.Commit();
        //}

        public IList<pTraceRechPvVl> FindAllByDate(DateTime debut, DateTime fin)
        {
            Query q = new Query();
            q.Criteria.Add(new Criteria("DateRecherche", debut, CriteriaOperator.GE));
            q.Criteria.Add(new Criteria("DateRecherche", fin, CriteriaOperator.LE));

            return this.FindBy(q);
        }

    }
}
