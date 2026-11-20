namespace Utac.Rnc2.Infrastructure
{
    public class BrokenRule
    {

        private readonly string _RelatesToProperty;
        private readonly string _Rule;

        public BrokenRule(string RelatesToProperty, string Rule)
        {
            _RelatesToProperty = RelatesToProperty;
            _Rule = Rule;
        }

        public string Rule
        {
            get { return _Rule; }
        }

        public string RelatesToProperty
        {
            get { return _RelatesToProperty; }
        }

    }
}
