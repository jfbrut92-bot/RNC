using NHibernate.Type;

namespace Utac.Rnc2.BusinessObjects
{
    public partial class rCategorieVehicule : BusinessBase<short>
    {
        protected override void validate()
        {
            /* Implementer ICI les r?gles de validation */
        }

        public enum CategorieEnum
        {
            VL, PL, CL
        }

        public enum ExtCatvehEnum : short
        {
            VL = 1,
            PL = 2
        };
    }



    public class CategorieEnumType : EnumStringType
    {
        public CategorieEnumType() : base(typeof(Utac.Rnc2.BusinessObjects.rCategorieVehicule.CategorieEnum), 2)
        {
        }
    }

}
