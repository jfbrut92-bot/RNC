using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Infrastructure;

namespace Utac.Rnc2.Contrats.Services

{
    public partial interface IrTemplateMailService : IService<rTemplateMail>
    {

        rTemplateMail ajouter(rTemplateMail template);

        void modifier(rTemplateMail template);

        rTemplateMail FindByName(string templateName);
    }
}
