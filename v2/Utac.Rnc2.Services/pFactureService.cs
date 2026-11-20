using System;
using System.Collections.Generic;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Contrats.Repositories;
using Utac.Rnc2.Contrats.Services;
using Utac.Rnc2.Infrastructure;

namespace Utac.Rnc2.Services
{
    public class pFactureService : BaseService<pFacture>, IpFactureService
    {

        public pFactureService(IpFactureRepository repository, IUnitOfWork UnitOfWork) : base(repository, UnitOfWork)
        {
        }

        public IList<pFacture> FindAllForCNR()
        {
            return this.FindListFacture(" where not (p_facture.cod_agrem_ind = null)");
        }

        public IList<pFacture> FindAllByCentre(pCentreEncour centre)
        {
            if (centre == null) return null;
            return this.FindAllByCentre(centre.Id);
        }

        public IList<pFacture> FindAllByCentre(int idCentre)
        {
            return this.FindListFacture(" where p_facture.id_centre_encours = " + Convert.ToString(idCentre));
        }

        public IList<pFacture> FindAllByReseau(string cod_reseau)
        {
            return this.FindListFacture(" where p_facture.cod_reseau = '" + Convert.ToString(cod_reseau) + "'");
        }

        public void traceTelechargement(int id, string codeUtilisateur)
        {

            pFacture facture = this.FindBy(id);
            this.traceTelechargement(facture, codeUtilisateur);
        }

        public void traceTelechargement(pFacture facture, string codeUtilisateur)
        {
            IUnitOfWork transaction = this.getUnitOfWork();
            if (transaction != null)
            {
                transaction.BeginTran();
            }

            var repository = this._Repository as IpFactureRepository;
            repository.traceTelechargement(facture, codeUtilisateur);

            if (transaction != null)
            {
                transaction.CommitTran();
            }
        }

        public IList<pFacture> FindListFacture(string q)
        {
            IpFactureRepository repository = this._Repository as IpFactureRepository;
            return repository.FindListFacture(q);
        }

    }
}
