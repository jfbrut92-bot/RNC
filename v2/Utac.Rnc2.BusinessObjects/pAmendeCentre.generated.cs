namespace Utac.Rnc2.BusinessObjects
{
    public partial class pAmendeCentre : BusinessBase<int>
    {
        #region Declarations

		private string _libContravention = null;
		private System.DateTime? _dateEmisAmende = null;
		
		private pCentreEncour _pCentreEncour = null;

		#endregion

        #region Constructors

        public pAmendeCentre() { }

        #endregion

        #region Methods

        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            
            sb.Append(this.GetType().FullName);
			sb.Append(_libContravention);
			sb.Append(_dateEmisAmende);

            return sb.ToString().GetHashCode();
        }

        #endregion

        #region Properties

		public virtual string LibContravention
        {
            get { return _libContravention; }
			set
			{
				OnLibContraventionChanging();
				_libContravention = value;
				OnLibContraventionChanged();
			}
        }
		partial void OnLibContraventionChanging();
		partial void OnLibContraventionChanged();
		
		public virtual System.DateTime? DateEmisAmende
        {
            get { return _dateEmisAmende; }
			set
			{
				OnDateEmisAmendeChanging();
				_dateEmisAmende = value;
				OnDateEmisAmendeChanged();
			}
        }
		partial void OnDateEmisAmendeChanging();
		partial void OnDateEmisAmendeChanged();
		
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
		partial void OnPCentreEncourChanging();
		partial void OnPCentreEncourChanged();
		
        #endregion
    }
}
