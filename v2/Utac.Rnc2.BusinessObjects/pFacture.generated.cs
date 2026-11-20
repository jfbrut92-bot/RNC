using System;
using System.Collections;
using System.Collections.Generic;
using Utac.Rnc2.Infrastructure.types;

namespace Utac.Rnc2.BusinessObjects
{
    public partial class pFacture : BusinessBase<int>
    {
        #region Declarations

		private string _codAgremInd = null;
        private string _codCategorieVeh = null;
        private string _codReseau = null;
		private string _nomFichier = String.Empty;
		private string _numFacture = String.Empty;
		private System.DateTime _dateFacture = new DateTime();
		private System.DateTime _dateGenerationFacture = new DateTime();
		private System.DateTime? _dateTelechargementFacture = null;
        private string _codUtilisateurTelechargement = null;
		private byte[] _imgFichier = null;
        private rReseau _rReseau;
        private pCentreEncour _pCentreEncour;

		#endregion

        #region Constructors

        public pFacture() { }

        #endregion

        #region Methods

        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            
            sb.Append(this.GetType().FullName);
            sb.Append(_codAgremInd);
            sb.Append(_codCategorieVeh);
            sb.Append(_codReseau);
            sb.Append(_nomFichier);
            sb.Append(_numFacture);
            sb.Append(_dateFacture);
            sb.Append(_dateGenerationFacture);
            sb.Append(_dateTelechargementFacture);
            sb.Append(_codUtilisateurTelechargement);
			sb.Append(_imgFichier);

            return sb.ToString().GetHashCode();
        }

        #endregion

        #region Properties

		public virtual string CodAgremInd
        {
            get { return _codAgremInd; }
			set
			{
                OnCodAgremIndChanging();
                _codAgremInd = value;
				OnCodAgremIndChanged();
			}
        }
		partial void OnCodAgremIndChanging();
		partial void OnCodAgremIndChanged();

        public virtual string CodCategorieVeh
        {
            get { return _codCategorieVeh; }
            set
            {
                OnCodCategorieVehChanging();
                _codCategorieVeh = value;
                OnCodCategorieVehChanged();
            }
        }
        partial void OnCodCategorieVehChanging();
        partial void OnCodCategorieVehChanged();
		
		public virtual string NomFichier
        {
            get { return _nomFichier; }
			set
			{
                OnNomFichierChanging();
                _nomFichier = value;
                OnNomFichierChanged();
			}
        }
        partial void OnNomFichierChanging();
        partial void OnNomFichierChanged();
		
		public virtual string NumFacture
        {
            get { return _numFacture; }
			set
			{
                OnNumFactureChanging();
                _numFacture = value;
                OnNumFactureChanged();
			}
        }
        partial void OnNumFactureChanging();
        partial void OnNumFactureChanged();

		public virtual System.DateTime DateFacture
        {
            get { return _dateFacture; }
			set
			{
                OnDateFactureChanging();
                _dateFacture = value;
                OnDateFactureChanged();
			}
        }
        partial void OnDateFactureChanging();
		partial void OnDateFactureChanged();
		
		public virtual System.DateTime DateGenerationFacture
        {
            get { return _dateGenerationFacture; }
			set
			{
                OnDateGenerationFactureChanging();
                _dateGenerationFacture = value;
                OnDateGenerationFactureChanged();
			}
        }
        partial void OnDateGenerationFactureChanging();
        partial void OnDateGenerationFactureChanged();

        public virtual System.DateTime? DateTelechargementFacture
        {
            get { return _dateTelechargementFacture; }
			set
			{
                OnDateTelechargementFactureChanging();
                _dateTelechargementFacture = value;
                OnDateTelechargementFactureChanged();
			}
        }
        partial void OnDateTelechargementFactureChanging();
        partial void OnDateTelechargementFactureChanged();

        public virtual string CodUtilisateurTelechargement
        {
            get { return _codUtilisateurTelechargement; }
            set
            {
                OnCodUtilisateurTelechargementChanging();
                _codUtilisateurTelechargement = value;
                OnCodUtilisateurTelechargementChanged();
            }
        }
        partial void OnCodUtilisateurTelechargementChanging();
        partial void OnCodUtilisateurTelechargementChanged();

        public virtual string CodReseau
        {
            get { return _codReseau; }
            set
            {
                OnCodReseauChanging();
                _codReseau = value;
                OnCodReseauChanged();
            }
        }
        partial void OnCodReseauChanging();
        partial void OnCodReseauChanged();

		public virtual byte[] ImgFichier
        {
            get { return _imgFichier; }
			set
			{
				OnImgFichierChanging();
				_imgFichier = value;
				OnImgFichierChanged();
			}
        }

		partial void OnImgFichierChanging();
		partial void OnImgFichierChanged();

        partial void OnRReseauChanging();
        partial void OnRReseauChanged();

        public virtual rReseau RReseau
        {
            get { return _rReseau; }
            set
            {
                OnRReseauChanging();
                _rReseau = value;
                OnRReseauChanged();
            }
        }

        partial void OnPCentreEncourChanging();
        partial void OnPCentreEncourChanged();

        public virtual pCentreEncour PCentreEncour
        {
            get { return _pCentreEncour; }
            set
            {
                OnPCentreEncourChanging();
                _pCentreEncour = value;
                OnPCentreEncourChanged();
            }
        }

        #endregion
    }
}
