using System.Collections.Generic;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Infrastructure;

namespace Utac.Rnc2.Contrats.Services
{
    public interface IrReseauService : IService<rReseau>
    {
        List<rReseau> FindAllReseauPLSansZ();
        List<rReseau> FindAllReseauVLSansZ();
        List<rReseau> FindAllReseauCLSansW();
        List<rReseau> FindAllReseauSansA();

        List<rReseau> FindAllReseauPLAvecZ();
        List<rReseau> FindAllReseauVLAvecZ();
        List<rReseau> FindAllReseauCLAvecW();

        List<rReseau> FindAllReseauSansAWZ();

        //EG le 22/11/2011
        bool aUnCertificatValide(rReseau r);

        rReseau ModifierParametreFacturationReseau(rReseau reseau, bool facturePapier, string adresseMail);

    }
}
