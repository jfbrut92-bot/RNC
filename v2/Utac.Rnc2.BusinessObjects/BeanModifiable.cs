namespace Utac.Rnc2.BusinessObjects
{
    public partial class BeanModifiable<T> : IModifiable
    {
        private readonly T _realObject = default(T);

        public BeanModifiable(T realObject)
        {
            this._realObject = realObject;
        }

        public override rUtilisateur RUtilisateur
        {
            get
            {

                if (_realObject is pAmendeControleur)
                {
                    pAmendeControleur obj = _realObject as pAmendeControleur;
                    return obj.RUtilisateur;
                }

                if (_realObject is pAuditControleurVL)
                {
                    pAuditControleurVL obj = _realObject as pAuditControleurVL;
                    return obj.RUtilisateur;
                }

                if (_realObject is pAuditControleur)
                {
                    pAuditControleur obj = _realObject as pAuditControleur;
                    return obj.RUtilisateur;
                }

                if (_realObject is pAuditControleurCL)
                {
                    pAuditControleurCL obj = _realObject as pAuditControleurCL;
                    return obj.RUtilisateur;
                }

                if (_realObject is pFormationControleurVL)
                {
                    pFormationControleurVL obj = _realObject as pFormationControleurVL;
                    return obj.RUtilisateur;
                }

                if (_realObject is pFormationControleurPL)
                {
                    pFormationControleurPL obj = _realObject as pFormationControleurPL;
                    return obj.RUtilisateur;
                }

                if (_realObject is pFormationControleurCL)
                {
                    pFormationControleurCL obj = _realObject as pFormationControleurCL;
                    return obj.RUtilisateur;
                }

                if (_realObject is pSanctionControleur)
                {
                    pSanctionControleur obj = _realObject as pSanctionControleur;
                    return obj.RUtilisateur;
                }

                return null; // ou throw Error ?
            }
        }

    }
}
