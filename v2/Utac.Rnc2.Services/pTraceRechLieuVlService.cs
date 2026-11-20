using System;
using System.Collections.Generic;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Contrats.Repositories;
using Utac.Rnc2.Contrats.Services;
using Utac.Rnc2.Infrastructure;
using Utac.Rnc2.Infrastructure.Queries;

namespace Utac.Rnc2.Services
{
    public class pTraceRechLieuVlService : BaseService<pTraceRechLieuVl>, IpTraceRechLieuVlService
    {

        public pTraceRechLieuVlService(
            IpTraceRechLieuVlRepository repository,
            IUnitOfWork UnitOfWork
            )
            : base(repository, UnitOfWork)
        {
        }

        public void TracerAppel(pTraceRechLieuVl trace)
        {
            IUnitOfWork transaction = this.getUnitOfWork();

            // On renseigne la cle primaire de l'audit
            trace.DateRecherche = System.DateTime.Now;

            // On sauvegarde l'audit
            this.Save(trace);

            if (transaction != null)
                transaction.Commit();

        }

        public IList<pTraceRechLieuVl> FindAllByDate(DateTime debut, DateTime fin)
        {
            Query q = new Query();
            q.Criteria.Add(new Criteria("DateRecherche", debut, CriteriaOperator.GE));
            q.Criteria.Add(new Criteria("DateRecherche", fin, CriteriaOperator.LE));
            q.OrderBy.Add(new OrderBy() { PropertyName = "Id", Ascending = false });

            return this.FindBy(q);
        }

        //public pTraceRechLieuVl  nouvelTrace(pTraceRechLieuVl trace)
        //{
        //    IUnitOfWork transaction = this.getUnitOfWork();

        //    // On renseigne la cle primaire de l'audit
        //    trace.DateRecherche = System.DateTime.Now;

        //    // On sauvegarde l'audit
        //    this.Save(trace);

        //    if (transaction != null)
        //        transaction.Commit();

        //    return trace;
        //}

        //public pTraceRechLieuVl modifierTrace(int id , pTraceRechLieuVl trace)
        //{

        //    IUnitOfWork transaction = this.getUnitOfWork();

        //    // On sauvegarde l'audit
        //    pTraceRechLieuVl t = this.FindBy(id);
        //    t.Nom = trace.Nom;
        //    t.Prenom = trace.Prenom;
        //    t.ImmatVeh = trace.ImmatVeh;
        //    t.DateCircu = trace.DateCircu;
        //    t.NumSerie = trace.NumSerie;
        //    t.CodAgremCentre = trace.CodAgremCentre;
        //    t.CodResultat = trace.CodResultat;

        //    this.Save(t);

        //    if (transaction != null)
        //        transaction.Commit();

        //    return t;
        //}

        //public void  deleteTrace(pTraceRechLieuVl trace)
        //{
        //    IUnitOfWork transaction = this.getUnitOfWork();

        //    this.Remove(trace);

        //    if (transaction != null)
        //        transaction.Commit();
        //}

    }
}
