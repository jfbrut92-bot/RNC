using System;

namespace Utac.Rnc2.BusinessObjects
{
    public partial class pGestionCertificat : BusinessBase<int>
    {
        #region Declarations

		private System.DateTime _dateMaj = new DateTime();
		private string _libCommentaire = null;
		//private int _idReq = default(Int32);
		//private string _numSerie = String.Empty;
		//private short _codAction = default(Int16);
		//private string _codReseau = null;
		//private int? _idCentreEncoursVl = null;
		//private int? _idCentreEncoursPl = null;
		//private string _codUtilisateur = String.Empty;
		
		private pCentreEncour _pCentreEncourVL = null;
		private pCentreEncour _pCentreEncourPL = null;
        //EG le 14/11/2011
        private pCentreEncour _pCentreEncourCL = null;
		private rReseau _rReseau = null;
		private rUtilisateur _rUtilisateur = null;
		private rActionCertificat _rActionCertificat = null;

		#endregion

        #region Constructors

        public pGestionCertificat() { }

        #endregion

        #region Methods

        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            
            sb.Append(this.GetType().FullName);
			sb.Append(_dateMaj);
			sb.Append(_libCommentaire);
			//sb.Append(_idReq);
			//sb.Append(_numSerie);
			//sb.Append(_codAction);
			//sb.Append(_codReseau);
			//sb.Append(_idCentreEncoursVl);
			//sb.Append(_idCentreEncoursPl);
			//sb.Append(_codUtilisateur);

            return sb.ToString().GetHashCode();
        }

        #endregion

        #region Properties

		public virtual System.DateTime DateMaj
        {
            get { return _dateMaj; }
			set
			{
				OnDateMajChanging();
				_dateMaj = value;
				OnDateMajChanged();
			}
        }
		partial void OnDateMajChanging();
		partial void OnDateMajChanged();
		
		public virtual string LibCommentaire
        {
            get { return _libCommentaire; }
			set
			{
				OnLibCommentaireChanging();
				_libCommentaire = value;
				OnLibCommentaireChanged();
			}
        }
		partial void OnLibCommentaireChanging();
		partial void OnLibCommentaireChanged();
		
        /* EG le 14/11/2011
		public virtual int IdReq
        {
            get { return _idReq; }
			set
			{
				OnIdReqChanging();
				_idReq = value;
				OnIdReqChanged();
			}
        }
		partial void OnIdReqChanging();
		partial void OnIdReqChanged();
		
		public virtual string NumSerie
        {
            get { return _numSerie; }
			set
			{
				OnNumSerieChanging();
				_numSerie = value;
				OnNumSerieChanged();
			}
        }
		partial void OnNumSerieChanging();
		partial void OnNumSerieChanged();
		
		public virtual short CodAction
        {
            get { return _codAction; }
			set
			{
				OnCodActionChanging();
				_codAction = value;
				OnCodActionChanged();
			}
        }
		partial void OnCodActionChanging();
		partial void OnCodActionChanged();
		
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
         */

        /*
		public virtual int? IdCentreEncoursVl
        {
            get { return _idCentreEncoursVl; }
			set
			{
				OnIdCentreEncoursVlChanging();
				_idCentreEncoursVl = value;
				OnIdCentreEncoursVlChanged();
			}
        }
		partial void OnIdCentreEncoursVlChanging();
		partial void OnIdCentreEncoursVlChanged();
		
		public virtual int? IdCentreEncoursPl
        {
            get { return _idCentreEncoursPl; }
			set
			{
				OnIdCentreEncoursPlChanging();
				_idCentreEncoursPl = value;
				OnIdCentreEncoursPlChanged();
			}
        }
		partial void OnIdCentreEncoursPlChanging();
		partial void OnIdCentreEncoursPlChanged();
		*/

        /*
		public virtual string CodUtilisateur
        {
            get { return _codUtilisateur; }
			set
			{
				OnCodUtilisateurChanging();
				_codUtilisateur = value;
				OnCodUtilisateurChanged();
			}
        }
		partial void OnCodUtilisateurChanging();
		partial void OnCodUtilisateurChanged();
		*/

		public virtual pCentreEncour PCentreEncourVL
        {
            get { return _pCentreEncourVL; }
			set
			{
				OnPCentreEncourVLChanging();
				_pCentreEncourVL = value;
				OnPCentreEncourVLChanged();
			}
        }
		partial void OnPCentreEncourVLChanging();
		partial void OnPCentreEncourVLChanged();
		
		public virtual pCentreEncour PCentreEncourPL
        {
            get { return _pCentreEncourPL; }
			set
			{
				OnPCentreEncourPLChanging();
				_pCentreEncourPL = value;
				OnPCentreEncourPLChanged();
			}
        }
		partial void OnPCentreEncourPLChanging();
		partial void OnPCentreEncourPLChanged();

        public virtual pCentreEncour PCentreEncourCL
        {
            get { return _pCentreEncourCL; }
            set
            {
                OnPCentreEncourCLChanging();
                _pCentreEncourCL = value;
                OnPCentreEncourCLChanged();
            }
        }
        partial void OnPCentreEncourCLChanging();
        partial void OnPCentreEncourCLChanged();

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
		partial void OnRReseauChanging();
		partial void OnRReseauChanged();
		
		public virtual rUtilisateur RUtilisateur
        {
            get { return _rUtilisateur; }
			set
			{
				OnRUtilisateurChanging();
				_rUtilisateur = value;
				OnRUtilisateurChanged();
			}
        }
		partial void OnRUtilisateurChanging();
		partial void OnRUtilisateurChanged();
		
		public virtual rActionCertificat RActionCertificat
        {
            get { return _rActionCertificat; }
			set
			{
				OnRActionCertificatChanging();
				_rActionCertificat = value;
				OnRActionCertificatChanged();
			}
        }
		partial void OnRActionCertificatChanging();
		partial void OnRActionCertificatChanged();
		
        #endregion
    }
}
