using System;
using NHibernate.Criterion;
using Utac.Rnc2.Infrastructure.Queries;

namespace Utac.Rnc2.Repository.Nhibernate
{
    public class QueryTranslator
    {

        public static NHibernate.ICriteria GetCriteriaFrom(NHibernate.ICriteria Criteria, Query q)
        {

            foreach (Criteria c in q.Criteria)
            {

                Criteria.Add(GetCriteriaFrom(c));

            }

            foreach (JoinCriteria j in q.JoinCriteria)
            {
                Criteria.CreateAlias(j.PropertyName, j.Alias);
                foreach (Criteria c in j.Criterias)
                {
                    Criteria.Add(GetCriteriaFrom(c, j.Alias));
                }
            }

            /*
             * foreach (JoinCriteria j in q.JoinCriteria)
            {
                Criteria.CreateAlias(j.PropertyName, j.Alias);
                foreach (Utac.Rnc2.Infrastructure.Queries.ICriteria c in j.Criterias)
                {
                    if (c is Criteria)
                    {
                        Criteria.Add(GetCriteriaFrom((Criteria)c, j.Alias));
                    }
                    else
                    {
                        //c'est JoinCriteria
                        var j2 = c as JoinCriteria;
                        Criteria.CreateAlias(j2.PropertyName, j.Alias+"."+j2.Alias);
                        foreach (Utac.Rnc2.Infrastructure.Queries.ICriteria c2 in j2.Criterias)
                        {
                            Criteria.Add(GetCriteriaFrom((Criteria)c2, j2.Alias));
                        }
                    }
                }
            }
             * */

            foreach (OrderBy o in q.OrderBy)
            {
                Criteria.AddOrder(new Order(o.PropertyName, o.Ascending));
            }

            return Criteria;
        }

        public static NHibernate.Criterion.ICriterion GetCriteriaFrom(Criteria c, string alias = "")
        {

            string propertyName = c.PropertyName;
            if (!string.IsNullOrEmpty(alias))
            {
                propertyName = alias + "." + propertyName;
            }

            switch (c.CriteriaOperator)
            {

                case CriteriaOperator.EQUALS:
                    return Expression.Eq(propertyName, c.Value);

                case CriteriaOperator.LIKE:
                    return Expression.Like(propertyName, string.Format("{0}%", c.Value));

                case CriteriaOperator.NOT_EQUALS:
                    return Expression.Not(Expression.Eq(propertyName, c.Value));

                case CriteriaOperator.IN:
                    return Expression.In(propertyName, c.Values);

                case CriteriaOperator.IS_NULL:
                    return Expression.IsNull(propertyName);

                case CriteriaOperator.IS_NOT_NULL:
                    return Expression.IsNotNull(propertyName);

                case CriteriaOperator.IS_EMPTY:
                    return Restrictions.IsEmpty(propertyName);

                case CriteriaOperator.IS_NOT_EMPTY:
                    return Restrictions.IsNotEmpty(propertyName);

                case CriteriaOperator.GE:
                    return Restrictions.Ge(propertyName, c.Value);

                case CriteriaOperator.GT:
                    return Restrictions.Gt(propertyName, c.Value);

                case CriteriaOperator.LT:
                    return Restrictions.Lt(propertyName, c.Value);

                case CriteriaOperator.LE:
                    return Restrictions.Le(propertyName, c.Value);
                //EG le 16/11/2011
                case CriteriaOperator.NOT_IN:
                    return Restrictions.Not(Restrictions.In(propertyName, c.Values));

                default: throw new Exception("Opérateur inconnu :" + c.CriteriaOperator.ToString());

            }

        }

    }

}
