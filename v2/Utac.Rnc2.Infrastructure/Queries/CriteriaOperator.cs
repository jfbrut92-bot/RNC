namespace Utac.Rnc2.Infrastructure.Queries
{
    public enum CriteriaOperator
    {

        LIKE = 1,
        EQUALS = 2,
        NOT_EQUALS = 3,
        IN = 4,
        IS_NULL = 5,
        IS_NOT_NULL = 6,
        IS_EMPTY = 7,  //Attention uniquement pour les collections Set , IList ,etc...
        IS_NOT_EMPTY = 8, //Attention uniquement pour les collections Set , IList ,etc...
        GT = 9, //strictement supérieur à
        GE = 10, //supérieur ou égale
        LT = 11, //strictement inférieur à
        LE = 12, //inférieur ou égale
        NOT_IN = 13

    }
}
