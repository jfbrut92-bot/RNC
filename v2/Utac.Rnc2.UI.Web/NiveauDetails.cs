using System.Collections.Generic;

namespace Utac.Rnc2.UI.Web.Enums
{
    public sealed class NiveauDetails
    {
        public static readonly NiveauDetails Summary = new NiveauDetails("Résumé", InnerEnum.Summary, "Summary");
        public static readonly NiveauDetails Full = new NiveauDetails("Détaillé", InnerEnum.Full, "Full");

        private static readonly IList<NiveauDetails> valueList = new List<NiveauDetails>();

        static NiveauDetails()
        {
            valueList.Add(Summary);
            valueList.Add(Full);
        }

        public enum InnerEnum
        {
            Summary,
            Full
        }

        public readonly InnerEnum innerEnumValue;
        private readonly string nameValue;
        private readonly int ordinalValue;
        private static int nextOrdinal = 0;

        private readonly string value;

        internal NiveauDetails(string name, InnerEnum innerEnum, string enumValue)
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

        public static IList<NiveauDetails> values()
        {
            return valueList;
        }

        public static IList<NiveauDetails> Values
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

        public static NiveauDetails valueOf(string name)
        {
            foreach (NiveauDetails enumInstance in NiveauDetails.valueList)
            {
                if (enumInstance.nameValue == name)
                {
                    return enumInstance;
                }
            }
            throw new System.ArgumentException(name);
        }

        public static NiveauDetails enumValueOf(string value)
        {
            foreach (NiveauDetails enumInstance in NiveauDetails.valueList)
            {
                if (enumInstance.value == value)
                {
                    return enumInstance;
                }
            }
            throw new System.ArgumentException(value);
        }

        public Utac.Rnc2.BusinessObjects.pTraceRechPvVl.CodeTypeRechercheCT CodeTypeRecherche
        {
            get
            {
                switch (innerEnumValue)
                {
                    case InnerEnum.Full:
                        return BusinessObjects.pTraceRechPvVl.CodeTypeRechercheCT.TOUT;
                    case InnerEnum.Summary:
                    default:
                        return BusinessObjects.pTraceRechPvVl.CodeTypeRechercheCT.REDUIT;
                }
            }
        }
    }
}