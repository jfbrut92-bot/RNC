using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Infrastructure;

namespace Utac.Rnc2.Contrats.Services

{
    public partial interface IUtilisateurWebServiceVLService : IService<UtilisateurWebServiceVL>
    {
        UtilisateurWebServiceVL FindByCodeReseau(string codeReseau);
        UtilisateurWebServiceVL FindByLogin(string login);
        void ajouterUtilisateur(UtilisateurWebServiceVL newUser);
    }
}
