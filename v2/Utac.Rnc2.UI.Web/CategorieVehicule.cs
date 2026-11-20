using System.Collections.Generic;

namespace Utac.Rnc2.UI.Web.Enums
{
    public sealed class CategorieVehicule
    {
        public static readonly CategorieVehicule PL = new CategorieVehicule("Poids lourd", InnerEnum.PL, "PL");
        public static readonly CategorieVehicule VL = new CategorieVehicule("Véhicule léger", InnerEnum.VL, "VL");
        public static readonly CategorieVehicule CL = new CategorieVehicule("Catégorie L", InnerEnum.CL, "CL");

        private static readonly IList<CategorieVehicule> valueList = new List<CategorieVehicule>();

        static CategorieVehicule()
        {
            valueList.Add(VL);
            valueList.Add(CL);
            valueList.Add(PL);
        }

        public enum InnerEnum
        {
            PL,
            VL,
            CL
        }

        private readonly InnerEnum innerEnumValue;
        private readonly string nameValue;
        private readonly int ordinalValue;
        private static int nextOrdinal = 0;

        private readonly int position = -1;

        private readonly string value;

        public InnerEnum InnerEnumValue
        {
            get { return innerEnumValue; }
        }

        internal CategorieVehicule(string name, InnerEnum innerEnum, string enumValue)
        {
            this.value = enumValue;

            nameValue = name;
            ordinalValue = nextOrdinal++;
            innerEnumValue = innerEnum;
        }

        public void InstanceMethod()
        {
            //...
        }

        public static IList<CategorieVehicule> values()
        {
            return valueList;
        }

        public static IList<CategorieVehicule> Values
        {
            get { return valueList; }
        }

        public int ordinal()
        {
            return ordinalValue;
        }

        public int Ordinal
        {
            get { return ordinalValue; }
        }

        public string Value
        {
            get { return value; }
        }

        public override string ToString()
        {
            return nameValue;
        }

        public static CategorieVehicule valueOf(string name)
        {
            foreach (CategorieVehicule enumInstance in CategorieVehicule.valueList)
            {
                if (enumInstance.nameValue == name)
                {
                    return enumInstance;
                }
            }
            throw new System.ArgumentException(name);
        }

        public static CategorieVehicule enumValueOf(string value)
        {
            foreach (CategorieVehicule enumInstance in CategorieVehicule.valueList)
            {
                if (enumInstance.value == value)
                {
                    return enumInstance;
                }
            }
            throw new System.ArgumentException(value);
        }
    }
}
