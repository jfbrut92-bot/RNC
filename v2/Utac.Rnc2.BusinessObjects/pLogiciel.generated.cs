using System;
using System.Collections.Generic;

namespace Utac.Rnc2.BusinessObjects
{
    public partial class pLogiciel : BusinessBase<int>
    {
        #region Declarations

        private string _nomLogiciel = String.Empty;
        private string _nomEditeur = null;

        private IList<pConformiteLogiciel> _pConformiteLogiciels = new List<pConformiteLogiciel>();

        #endregion

        #region Constructors

        public pLogiciel() { }

        #endregion

        #region Methods

        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append(this.GetType().FullName);
            sb.Append(_nomLogiciel);
            sb.Append(_nomEditeur);

            return sb.ToString().GetHashCode();
        }

        #endregion

        #region Properties

        public virtual string NomLogiciel
        {
            get { return _nomLogiciel; }
            set
            {
                OnNomLogicielChanging();
                _nomLogiciel = value;
                OnNomLogicielChanged();
            }
        }
        partial void OnNomLogicielChanging();
        partial void OnNomLogicielChanged();

        public virtual string NomEditeur
        {
            get { return _nomEditeur; }
            set
            {
                OnNomEditeurChanging();
                _nomEditeur = value;
                OnNomEditeurChanged();
            }
        }
        partial void OnNomEditeurChanging();
        partial void OnNomEditeurChanged();

        public virtual IList<pConformiteLogiciel> PConformiteLogiciels
        {
            get { return _pConformiteLogiciels; }
            set
            {
                OnPConformiteLogicielsChanging();
                _pConformiteLogiciels = value;
                OnPConformiteLogicielsChanged();
            }
        }
        partial void OnPConformiteLogicielsChanging();
        partial void OnPConformiteLogicielsChanged();

        #endregion
    }
}
