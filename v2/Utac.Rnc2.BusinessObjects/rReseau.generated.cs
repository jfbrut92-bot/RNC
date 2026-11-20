using System;
using System.Collections.Generic;

namespace Utac.Rnc2.BusinessObjects
{
    public partial class rReseau : BusinessBase<string>
    {
        #region Declarations

        private string _nom = String.Empty;
        /* private string _logo = null; */
        private bool _typeReseauPl = default(Boolean);
        private bool _typeReseauVl = default(Boolean);
        private string _bddTechLogin = null;
        private bool _facturePapier = default(Boolean);
        private string _emailFacturation = null;

        private IList<pConformiteLogiciel> _pConformiteLogiciels = new List<pConformiteLogiciel>();
        private IList<pCentreEncour> _pCentreEncours = new List<pCentreEncour>();
        private IList<pCentreHisto> _pCentreHistos = new List<pCentreHisto>();
        private IList<pGestionCertificat> _pGestionCertificats = new List<pGestionCertificat>();
        private IList<pInfoRdv> _pInfoRdvs = new List<pInfoRdv>();
        private IList<rUtilisateur> _rUtilisateurs = new List<rUtilisateur>();
        //private IList<rUtilisateur> _rUtilisateurs2 = new List<rUtilisateur>();

        #endregion

        #region Constructors

        public rReseau() { }

        #endregion

        #region Methods

        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append(this.GetType().FullName);
            sb.Append(_nom);
            //sb.Append(_logo);
            sb.Append(_typeReseauPl);
            sb.Append(_typeReseauVl);
            sb.Append(_typeReseauCl);
            sb.Append(_bddTechLogin);
            sb.Append(_facturePapier);
            sb.Append(_emailFacturation);
            sb.Append(_lettreReseau);

            return sb.ToString().GetHashCode();
        }

        #endregion

        #region Properties

        public virtual string Nom
        {
            get { return _nom; }
            set
            {
                OnNomChanging();
                _nom = value;
                OnNomChanged();
            }
        }
        partial void OnNomChanging();
        partial void OnNomChanged();

        /* EG le 12/04/2011
         * public virtual string Logo
        {
            get { return _logo; }
			set
			{
				OnLogoChanging();
				_logo = value;
				OnLogoChanged();
			}
        }
        */
        partial void OnLogoChanging();
        partial void OnLogoChanged();

        public virtual bool TypeReseauPl
        {
            get { return _typeReseauPl; }
            set
            {
                OnTypeReseauPlChanging();
                _typeReseauPl = value;
                OnTypeReseauPlChanged();
            }
        }
        partial void OnTypeReseauPlChanging();
        partial void OnTypeReseauPlChanged();

        public virtual bool TypeReseauVl
        {
            get { return _typeReseauVl; }
            set
            {
                OnTypeReseauVlChanging();
                _typeReseauVl = value;
                OnTypeReseauVlChanged();
            }
        }
        partial void OnTypeReseauVlChanging();
        partial void OnTypeReseauVlChanged();

        public virtual string BddTechLogin
        {
            get { return _bddTechLogin; }
            set
            {
                OnBddTechLoginChanging();
                _bddTechLogin = value;
                OnBddTechLoginChanged();
            }
        }
        partial void OnBddTechLoginChanging();
        partial void OnBddTechLoginChanged();

        public virtual bool FacturePapier
        {
            get { return _facturePapier; }
            set
            {
                OnFacturePapierChanging();
                _facturePapier = value;
                OnFacturePapierChanged();
            }
        }
        partial void OnFacturePapierChanging();
        partial void OnFacturePapierChanged();

        public virtual string EmailFacturation
        {
            get { return _emailFacturation; }
            set
            {
                OnEmailFacturationChanging();
                _emailFacturation = value;
                OnEmailFacturationChanged();
            }
        }
        partial void OnEmailFacturationChanging();
        partial void OnEmailFacturationChanged();

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

        public virtual IList<pCentreEncour> PCentreEncours
        {
            get { return _pCentreEncours; }
            set
            {
                OnPCentreEncoursChanging();
                _pCentreEncours = value;
                OnPCentreEncoursChanged();
            }
        }
        partial void OnPCentreEncoursChanging();
        partial void OnPCentreEncoursChanged();

        public virtual IList<pCentreHisto> PCentreHistos
        {
            get { return _pCentreHistos; }
            set
            {
                OnPCentreHistosChanging();
                _pCentreHistos = value;
                OnPCentreHistosChanged();
            }
        }
        partial void OnPCentreHistosChanging();
        partial void OnPCentreHistosChanged();

        public virtual IList<pGestionCertificat> PGestionCertificats
        {
            get { return _pGestionCertificats; }
            set
            {
                OnPGestionCertificatsChanging();
                _pGestionCertificats = value;
                OnPGestionCertificatsChanged();
            }
        }
        partial void OnPGestionCertificatsChanging();
        partial void OnPGestionCertificatsChanged();

        public virtual IList<pInfoRdv> PInfoRdvs
        {
            get { return _pInfoRdvs; }
            set
            {
                OnPInfoRdvsChanging();
                _pInfoRdvs = value;
                OnPInfoRdvsChanged();
            }
        }
        partial void OnPInfoRdvsChanging();
        partial void OnPInfoRdvsChanged();

        public virtual IList<rUtilisateur> RUtilisateurs
        {
            get { return _rUtilisateurs; }
            set
            {
                OnRUtilisateurs1Changing();
                _rUtilisateurs = value;
                OnRUtilisateurs1Changed();
            }
        }
        partial void OnRUtilisateurs1Changing();
        partial void OnRUtilisateurs1Changed();

        //public virtual IList<rUtilisateur> RUtilisateurs2
        //{
        //    get { return _rUtilisateurs2; }
        //    set
        //    {
        //        OnRUtilisateurs2Changing();
        //        _rUtilisateurs2 = value;
        //        OnRUtilisateurs2Changed();
        //    }
        //}
        //partial void OnRUtilisateurs2Changing();
        //partial void OnRUtilisateurs2Changed();

        #endregion
    }
}
