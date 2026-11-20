using System;
using System.Collections.Generic;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Contrats.Repositories;
using Utac.Rnc2.Contrats.Services;
using Utac.Rnc2.Infrastructure;
using Utac.Rnc2.Infrastructure.Queries;

namespace Utac.Rnc2.Services
{
    public class pCourrierEnvoyeService : BaseService<pCourrierEnvoye>, IpCourrierEnvoyeService
    {

        public pCourrierEnvoyeService(IpCourrierEnvoyeRepository repository, IUnitOfWork UnitOfWork) : base(repository, UnitOfWork)
        {

        }

        public pCourrierEnvoye nouveau(rUtilisateur declencheur, rUtilisateur destinataire)
        {
            IUnitOfWork transaction = this.getUnitOfWork();

            pCourrierEnvoye courrier = new pCourrierEnvoye();
            courrier.CodUtilisateur = declencheur.Id;
            courrier.CodUtilisateurDest = destinataire.Id;
            courrier.Date = System.DateTime.Now;
            courrier.EmailDest = destinataire.Email;
            courrier.IdProfil2 = declencheur.RProfil2.Id;
            courrier.Nom = declencheur.Nom;
            courrier.NomDest = destinataire.Nom;
            courrier.Prenom = declencheur.Prenom;
            courrier.PrenomDest = destinataire.Prenom;

            this.Save(courrier);

            if (transaction != null)
            {
                transaction.Commit();
            }

            return courrier;
        }

        public void modifier(pCourrierEnvoye courrier)
        {
            IUnitOfWork transaction = this.getUnitOfWork();

            this.Save(courrier);

            if (transaction != null)
            {
                transaction.Commit();
            }

        }

        public IList<pCourrierEnvoye> FindAllByDate(DateTime debut, DateTime fin)
        {
            Query q = new Query();
            q.Criteria.Add(new Criteria("Date", debut, CriteriaOperator.GE));
            q.Criteria.Add(new Criteria("Date", fin, CriteriaOperator.LE));

            return this.FindBy(q);
        }
    }

}
