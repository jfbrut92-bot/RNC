using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Utac.Rnc2.Infrastructure;

namespace Utac.Rnc2.BusinessObjects
{
    interface IBusinessBase<T> : IAggregateRoot
    {
        // Methods
        int GetHashCode();
        bool Equals(object obj);

        // Properties
        T Id { get; }
    }

    /// <summary>
    /// Base for all business objects.
    /// 
    /// For an explanation of why Equals and GetHashCode are overriden, read the following...
    /// http://devlicio.us/blogs/billy_mccafferty/archive/2007/04/25/using-equals-gethashcode-effectively.aspx
    /// </summary>
    /// <typeparam name="T">DataType of the primary key.</typeparam>
    public abstract class BusinessBase<T> : IBusinessBase<T>
    {
        #region Declarations

        private T _id = default(T);

        #endregion

        #region Methods

        public override bool Equals(object obj)
        {
            BusinessBase<T> compareTo = obj as BusinessBase<T>;

            return compareTo != null
                && HasSameNonDefaultIdAs(compareTo)
                && HasSameBusinessSignatureAs(compareTo);
        }

        /// <summary>  
        /// Transient objects are not associated with an   
        /// item already in storage.  For instance, a   
        /// Customer is transient if its ID is 0.  
        /// </summary>  
        public virtual bool IsTransient()
        {
            return Id == null || Id.Equals(default(T));
        }

        /// <summary>  
        /// Must be implemented to compare two objects  
        /// </summary>  
        public abstract override int GetHashCode();

        private bool HasSameBusinessSignatureAs(BusinessBase<T> compareTo)
        {
            return GetHashCode().Equals(compareTo.GetHashCode());
        }

        /// <summary>  
        /// Returns true if self and the provided domain   
        /// object have the same ID values and the IDs   
        /// are not of the default ID value  
        /// </summary>  
        private bool HasSameNonDefaultIdAs(BusinessBase<T> compareTo)
        {
            return IsTransient() || compareTo.IsTransient() || Id.Equals(compareTo.Id);
        }

        #endregion

        #region Properties

        public virtual T Id
        {
            get { return _id; }
            set { _id = value; }
        }

        #endregion

        #region Ajout EDDY

        private readonly List<BrokenRule> _brokenRules = new List<BrokenRule>();

        protected abstract void validate();

        protected List<BrokenRule> BrokenRules
        {
            get { return this._brokenRules; }
        }

        public virtual ReadOnlyCollection<BrokenRule> GetBrokenRules()
        {
            this.BrokenRules.Clear();
            this.validate();
            return this._brokenRules.AsReadOnly();
        }

        //EG le 18/02/2012 pour pouvoir tracer les ID retourné par FindBy

        public override string ToString()
        {
            StringBuilder b = new StringBuilder("Type :" + this.GetType());
            b.Append("|ID = " + this.Id);
            //Todo : liste des propriétés de type int , string, date, etc...
            return b.ToString();
        }

        public virtual object getId()
        {
            return this.Id;
        }

        #endregion

    }

}
