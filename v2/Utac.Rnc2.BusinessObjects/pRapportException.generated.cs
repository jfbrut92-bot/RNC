using System;

namespace Utac.Rnc2.BusinessObjects
{
    public partial class pRapportException : BusinessBase<int>
    {
        #region Declarations

		private string _codAgremPl = null;
		private string _codReseau = null;
		private string _nomRapport = String.Empty;
		private System.DateTime _dateDebutTransmissionPv = new DateTime();
		private System.DateTime _dateFinTransmissionPv = new DateTime();
		private System.DateTime _dateGenerationRapport = new DateTime();
		private byte[] _imgFichier = null;
        //private Image _imgFichier;

		#endregion

        #region Constructors

        public pRapportException() { }

        #endregion

        #region Methods

        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            
            sb.Append(this.GetType().FullName);
			sb.Append(_codAgremPl);
			sb.Append(_codReseau);
			sb.Append(_nomRapport);
			sb.Append(_dateDebutTransmissionPv);
			sb.Append(_dateFinTransmissionPv);
			sb.Append(_dateGenerationRapport);
			sb.Append(_imgFichier);

            return sb.ToString().GetHashCode();
        }

        #endregion

        #region Properties

		public virtual string CodAgremPl
        {
            get { return _codAgremPl; }
			set
			{
				OnCodAgremIndChanging();
				_codAgremPl = value;
				OnCodAgremIndChanged();
			}
        }
		partial void OnCodAgremIndChanging();
		partial void OnCodAgremIndChanged();
		
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
		
		public virtual string NomRapport
        {
            get { return _nomRapport; }
			set
			{
				OnNomRapportChanging();
				_nomRapport = value;
				OnNomRapportChanged();
			}
        }
		partial void OnNomRapportChanging();
		partial void OnNomRapportChanged();
		
		public virtual System.DateTime DateDebutTransmissionPv
        {
            get { return _dateDebutTransmissionPv; }
			set
			{
				OnDateDebutTransmissionPvChanging();
				_dateDebutTransmissionPv = value;
				OnDateDebutTransmissionPvChanged();
			}
        }
		partial void OnDateDebutTransmissionPvChanging();
		partial void OnDateDebutTransmissionPvChanged();
		
		public virtual System.DateTime DateFinTransmissionPv
        {
            get { return _dateFinTransmissionPv; }
			set
			{
				OnDateFinTransmissionPvChanging();
				_dateFinTransmissionPv = value;
				OnDateFinTransmissionPvChanged();
			}
        }
		partial void OnDateFinTransmissionPvChanging();
		partial void OnDateFinTransmissionPvChanged();
		
		public virtual System.DateTime DateGenerationRapport
        {
            get { return _dateGenerationRapport; }
			set
			{
				OnDateGenerationRapportChanging();
				_dateGenerationRapport = value;
				OnDateGenerationRapportChanged();
			}
        }
		partial void OnDateGenerationRapportChanging();
		partial void OnDateGenerationRapportChanged();
		
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

        /*public virtual Image ImgFichier
        {
            get { return _imgFichier; }
			set
			{
				OnImgFichierChanging();
				_imgFichier = value;
				OnImgFichierChanged();
			}
        }
        */
		partial void OnImgFichierChanging();
		partial void OnImgFichierChanged();
		
        #endregion
    }
}
