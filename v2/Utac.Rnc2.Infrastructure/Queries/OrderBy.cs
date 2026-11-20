namespace Utac.Rnc2.Infrastructure.Queries
{
    public class OrderBy : IOrderBy
    {
        private string _propertyName;

        public string PropertyName
        {
            get { return _propertyName; }
            set { _propertyName = value; }
        }

        private bool _ascending;

        public bool Ascending
        {
            get { return _ascending; }
            set { _ascending = value; }
        }

    }
}
