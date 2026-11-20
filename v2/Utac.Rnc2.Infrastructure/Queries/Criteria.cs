namespace Utac.Rnc2.Infrastructure.Queries
{
    public class Criteria : ICriteria
    {

        private string _property;
        private object _value;
        private object[] _values;
        private CriteriaOperator _operator;

        public Criteria(string propertyName, object value, CriteriaOperator @operator)
        {
            _property = propertyName;
            _value = value;
            _operator = @operator;
        }

        public Criteria(string propertyName, string[] values)
        {
            _property = propertyName;
            _values = values;
            _operator = CriteriaOperator.IN;
        }

        //EG le 16/11/2011
        public Criteria(string propertyName, string[] values, CriteriaOperator @operator)
        {
            _property = propertyName;
            _values = values;
            _operator = @operator;
        }

        public string PropertyName
        {
            get { return _property; }
            set { _property = value; }
        }

        public object Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public object[] Values
        {
            get { return _values; }
            set { _values = value; }
        }

        public CriteriaOperator CriteriaOperator
        {
            get { return _operator; }
            set { _operator = value; }
        }

    }
}
