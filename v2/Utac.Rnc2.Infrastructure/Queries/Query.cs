using System.Collections.Generic;

namespace Utac.Rnc2.Infrastructure.Queries
{
    public class Query
    {

        private List<Criteria> _Criteria;
        private List<JoinCriteria> _JoinCriteria;
        private List<IOrderBy> _OrderBy;

        public Query()
        {
            _Criteria = new List<Criteria>();
            _JoinCriteria = new List<JoinCriteria>();
            _OrderBy = new List<IOrderBy>();
        }

        public List<JoinCriteria> JoinCriteria
        {
            get { return _JoinCriteria; }
            set { _JoinCriteria = value; }
        }

        public List<Criteria> Criteria
        {
            get { return _Criteria; }
            set { _Criteria = value; }
        }

        public List<IOrderBy> OrderBy
        {
            get { return _OrderBy; }
            set { _OrderBy = value; }
        }

        public string OrderByAsString
        {
            get
            {
                string order = null;
                foreach (OrderBy o in this._OrderBy)
                {
                    if (order == null)
                    {
                        order = " order by " + o.PropertyName + " " + (o.Ascending ? "asc" : "desc");
                    }
                    else
                    {
                        order += "," + o.PropertyName + " " + (o.Ascending ? "asc" : "desc");
                    }
                }
                return order;
            }
        }

        public JoinCriteria findJoinCriteriaByAlias(string alias)
        {
            if (string.IsNullOrEmpty(alias)) return null;

            foreach (JoinCriteria j in this._JoinCriteria)
            {
                if (alias.Equals(j.Alias)) return j;
            }
            return null;
        }

    }
}
