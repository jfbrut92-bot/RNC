using System.Collections.Generic;

namespace Utac.Rnc2.Infrastructure.Queries
{
    public class JoinCriteria : ICriteria
    {

        private string _alias;
        private string _propertyName;

        private IList<ICriteria> _criterias;

        public JoinCriteria(string propertyName, string alias)
        {
            _alias = alias;
            _propertyName = propertyName;
            _criterias = new List<ICriteria>();
        }

        public IList<ICriteria> Criterias
        {
            get { return _criterias; }
            set { _criterias = value; }
        }

        public string Alias
        {
            get { return _alias; }
            set { _alias = value; }
        }

        public string PropertyName
        {
            get { return _propertyName; }
            set { _propertyName = value; }
        }

    }
}
