using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Contrats.Repositories;
using Utac.Rnc2.Contrats.Services;
using Utac.Rnc2.Infrastructure;

namespace Utac.Rnc2.Services
{
    public partial class ParametreService : BaseService<Parametre>, IParametreService
    {

        public ParametreService(
                                    IParametreRepository repository,
                                    IUnitOfWork UnitOfWork
            )
            : base(repository, UnitOfWork)
        {

        }

        public void modifyParametre(int id, Parametre model)
        {
            Parametre p = this.FindBy(id);

            p.Nom = model.Nom;
            p.Valeur = model.Valeur;

            base.Save(p);
            IUnitOfWork transaction = this.getUnitOfWork();
            if (transaction != null)
            {
                transaction.Commit();
            }

        }

        public void suppression(int id)
        {

            Parametre p = this.FindBy(id);
            this.suppression(p);
        }

        public void suppression(Parametre p)
        {

            base.Remove(p);
            IUnitOfWork transaction = this.getUnitOfWork();
            if (transaction != null)
            {
                transaction.Commit();
            }
        }
    }

}
