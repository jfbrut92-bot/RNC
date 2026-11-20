using System;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Infrastructure;

namespace Utac.Rnc2.Contrats.Services
{
    public interface IpControleurHistoService : IService<pControleurHisto>
    {

        pControleurHisto historise(pContoleurEncour controleurEnCour, pContoleurEncour controleurForm, rTypeDemandeControleur.TypeModification typeModification);
        pControleurHisto historise(pContoleurEncour c);

        pControleurHisto findLastBy(object id);
        pControleurHisto findByIdAndDate(object id, DateTime dateHisto);

        void removeAll(pContoleurEncour c);
    }
}
