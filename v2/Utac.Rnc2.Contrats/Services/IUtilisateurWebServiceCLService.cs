using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Infrastructure;

namespace Utac.Rnc2.Contrats.Services

{
    public partial interface IUtilisateurWebServiceCLService : IService<UtilisateurWebServiceCL>
    {
        UtilisateurWebServiceCL FindByCodeReseau(string codeReseau);
        UtilisateurWebServiceCL FindByLogin(string login);
        void ajouterUtilisateur(UtilisateurWebServiceCL newUser);
    }
}
