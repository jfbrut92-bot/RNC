using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Infrastructure;

namespace Utac.Rnc2.Contrats.Services

{
    public partial interface IpAmendeControleurService : IService<pAmendeControleur>
    {
        pAmendeControleur nouvelleAmende(pContoleurEncour controleur, pAmendeControleur amende, string codeUtilisateur);
        pAmendeControleur modifierAmende(pAmendeControleur amende, string codeUtilisateur);
        void deleteAmende(pAmendeControleur amende);
    }
}
