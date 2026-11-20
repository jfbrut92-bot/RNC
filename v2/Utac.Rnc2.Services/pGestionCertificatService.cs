using System.Collections.Generic;
using System.Linq;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Contrats.Repositories;
using Utac.Rnc2.Contrats.Services;
using Utac.Rnc2.Infrastructure;
using Utac.Rnc2.Infrastructure.Queries;

namespace Utac.Rnc2.Services
{
    public class pGestionCertificatService : BaseService<pGestionCertificat>, IpGestionCertificatService
    {

        public pGestionCertificatService(IpGestionCertificatRepository repository, IUnitOfWork UnitOfWork) : base(repository, UnitOfWork)
        {

        }

        public pCertificat findLastValide(pCentreEncour centre, rCategorieVehicule.CategorieEnum categorieVehicule)
        {
            return this.findLastValide(centre.Id, categorieVehicule);
        }

        public pCertificat findLastValide(int idCentre, rCategorieVehicule.CategorieEnum categorieVehicule)
        {
            pCertificat certif = findLastValideOuNonValide(idCentre, categorieVehicule);
            if (certif == null) return null;
            if (certif.DateExpiration >= System.DateTime.Now)
            {
                return certif;
            }
            else
            {
                return null;
            }
        }

        public pCertificat findLastValideOuNonValide(int id, rCategorieVehicule.CategorieEnum categorieVehicule)
        {
            Query q = new Query();
            switch (categorieVehicule)
            {
                case rCategorieVehicule.CategorieEnum.VL:
                    q.Criteria.Add(new Criteria("PCentreEncourVL.Id", id, CriteriaOperator.EQUALS));
                    break;
                case rCategorieVehicule.CategorieEnum.PL:
                    q.Criteria.Add(new Criteria("PCentreEncourPL.Id", id, CriteriaOperator.EQUALS));
                    break;
                case rCategorieVehicule.CategorieEnum.CL:
                    q.Criteria.Add(new Criteria("PCentreEncourCL.Id", id, CriteriaOperator.EQUALS));
                    break;
            }

            q.OrderBy.Add(new OrderBy() { PropertyName = "Id", Ascending = false });
            IList<pGestionCertificat> gestionCertificats = this.FindBy(q);
            if (gestionCertificats.Any())
            {
                return gestionCertificats.ElementAt(0).PCertificat;
            }

            return null;
        }

        public pCertificat findLastValideOuNonValide(pCentreEncour centre, rCategorieVehicule.CategorieEnum categorieVehicule)
        {
            return this.findLastValideOuNonValide(centre.Id, categorieVehicule);
        }

        public pCertificat findLastValide(rReseau reseau)
        {
            pCertificat certif = findLastValideOuNonValide(reseau);
            if (certif == null) return null;
            if (certif.DateExpiration >= System.DateTime.Now)
            {
                return certif;
            }
            else
            {
                return null;
            }
        }

        public pCertificat findLastValideOuNonValide(rReseau reseau)
        {
            IList<pGestionCertificat> gestionCertificats;
            Query q = new Query();
            q.Criteria.Add(new Criteria("RReseau.Id", reseau.Id, CriteriaOperator.EQUALS));

            q.OrderBy.Add(new OrderBy() { PropertyName = "Id", Ascending = false });
            gestionCertificats = this.FindBy(q);
            if (gestionCertificats.Count() > 0)
            {
                return gestionCertificats.ElementAt(0).PCertificat;
            }
            else
            {
                return null;
            }
        }

    }
}
