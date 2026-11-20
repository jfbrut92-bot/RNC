using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Infrastructure;

namespace Utac.Rnc2.Contrats.Services

{
    public partial interface IUtilisateurWebServicePLService : IService<UtilisateurWebServicePL>
    {
        UtilisateurWebServicePL FindByCodeReseau(string codeReseau);
        UtilisateurWebServicePL FindByLogin(string login);
        void ajouterUtilisateur(UtilisateurWebServicePL newUser);
    }
}
