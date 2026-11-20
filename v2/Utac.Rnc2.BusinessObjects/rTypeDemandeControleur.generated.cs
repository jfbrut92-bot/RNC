using System;
using System.Collections.Generic;

namespace Utac.Rnc2.BusinessObjects
{
    public partial class rTypeDemandeControleur : BusinessBase<string>
    {
        #region Declarations

        private string _libelle = String.Empty;

        private IList<pContoleurEncour> _pContoleurEncours = new List<pContoleurEncour>();
        private IList<pControleurHisto> _pControleurHistos = new List<pControleurHisto>();

        #endregion

        #region Constructors

        public rTypeDemandeControleur() { }

        #endregion

        #region Methods

        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append(this.GetType().FullName);
            sb.Append(_libelle);

            return sb.ToString().GetHashCode();
        }

        #endregion

        #region Properties

        public virtual string Libelle
        {
            get { return _libelle; }
            set
            {
                OnLibelleChanging();
                _libelle = value;
                OnLibelleChanged();
            }
        }
        partial void OnLibelleChanging();
        partial void OnLibelleChanged();

        public virtual IList<pContoleurEncour> PContoleurEncours
        {
            get { return _pContoleurEncours; }
            set
            {
                OnPContoleurEncoursChanging();
                _pContoleurEncours = value;
                OnPContoleurEncoursChanged();
            }
        }
        partial void OnPContoleurEncoursChanging();
        partial void OnPContoleurEncoursChanged();

        public virtual IList<pControleurHisto> PControleurHistos
        {
            get { return _pControleurHistos; }
            set
            {
                OnPControleurHistosChanging();
                _pControleurHistos = value;
                OnPControleurHistosChanged();
            }
        }
        partial void OnPControleurHistosChanging();
        partial void OnPControleurHistosChanged();

        #endregion
    }
}
