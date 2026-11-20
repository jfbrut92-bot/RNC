using System;
using System.Data;

namespace Utac.Rnc2.UI.Web.DataSets.Adapters
{
    public class ControleurDataRowAdapter
    {
        private readonly DataRow _realDataRow;

        public DataRow RealDataRow
        {
            get { return _realDataRow; }
        }

        public ControleurDataRowAdapter(DataRow realDataRow)
        {
            if (realDataRow == null)
            {
                throw new Exception("ControleurDataRowAdapter exception : le vrai dataRow ne peut être null!");
            }
            _realDataRow = realDataRow;
        }

        public bool est_type_agrem_vl
        {
            get
            {
                if (_realDataRow is cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow)
                {
                    cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow row = _realDataRow as cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow;
                    return row.est_type_agrem_vl;
                }

                if (_realDataRow is cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow)
                {
                    cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow row = _realDataRow as cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow;
                    return row.est_type_agrem_vl;
                }

                throw new Exception("_realDataRow exception Mauvais type");
            }

            set
            {
                if (_realDataRow is cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow)
                {
                    cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow row = _realDataRow as cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow;
                    row.est_type_agrem_vl = value;
                }
                else
                {
                    if (_realDataRow is cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow)
                    {
                        cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow row = _realDataRow as cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow;
                        row.est_type_agrem_vl = value;
                    }
                    else
                        throw new Exception("_realDataRow exception Mauvais type");
                }

            }
        }

        public bool est_type_agrem_pl
        {
            get
            {
                if (_realDataRow is cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow)
                {
                    cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow row = _realDataRow as cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow;
                    return row.est_type_agrem_pl;
                }

                if (_realDataRow is cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow)
                {
                    cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow row = _realDataRow as cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow;
                    return row.est_type_agrem_pl;
                }

                throw new Exception("_realDataRow exception Mauvais type");
            }

            set
            {
                if (_realDataRow is cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow)
                {
                    cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow row = _realDataRow as cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow;
                    row.est_type_agrem_pl = value;
                }
                else
                {
                    if (_realDataRow is cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow)
                    {
                        cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow row = _realDataRow as cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow;
                        row.est_type_agrem_pl = value;
                    }
                    else
                        throw new Exception("_realDataRow exception Mauvais type");
                }

            }
        }

        public bool est_type_agrem_cl
        {
            get
            {
                if (_realDataRow is cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow)
                {
                    cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow row = _realDataRow as cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow;
                    return row.est_type_agrem_cl;
                }

                if (_realDataRow is cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow)
                {
                    cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow row = _realDataRow as cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow;
                    return row.est_type_agrem_cl;
                }

                throw new Exception("_realDataRow exception Mauvais type");
            }

            set
            {
                if (_realDataRow is cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow)
                {
                    cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow row = _realDataRow as cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow;
                    row.est_type_agrem_cl = value;
                }

                if (_realDataRow is cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow)
                {
                    cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow row = _realDataRow as cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow;
                    row.est_type_agrem_cl = value;
                }

                //throw new Exception("_realDataRow exception Mauvais type");
            }

        }

        public bool est_prestataire
        {
            get
            {
                if (_realDataRow is cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow)
                {
                    cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow row = _realDataRow as cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow;
                    return row.est_prestataire;
                }

                if (_realDataRow is cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow)
                {
                    cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow row = _realDataRow as cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow;
                    return row.est_prestataire;
                }

                throw new Exception("_realDataRow exception Mauvais type");
            }

            set
            {
                if (_realDataRow is cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow)
                {
                    cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow row = _realDataRow as cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow;
                    row.est_prestataire = value;
                }

                if (_realDataRow is cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow)
                {
                    cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow row = _realDataRow as cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow;
                    row.est_prestataire = value;
                }

                //throw new Exception("_realDataRow exception Mauvais type");
            }
        }

        public string titre_departement
        {
            get
            {
                if (_realDataRow is cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow)
                {
                    cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow row = _realDataRow as cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow;
                    return row.titre_departement;
                }

                if (_realDataRow is cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow)
                {
                    cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow row = _realDataRow as cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow;
                    return row.titre_departement;
                }

                throw new Exception("_realDataRow exception Mauvais type");
            }

            set
            {
                if (_realDataRow is cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow)
                {
                    cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow row = _realDataRow as cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow;
                    row.titre_departement = value;
                }

                if (_realDataRow is cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow)
                {
                    cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow row = _realDataRow as cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow;
                    row.titre_departement = value;
                }

                //throw new Exception("_realDataRow exception Mauvais type");
            }
        }

        public string libelle_prefet
        {
            get
            {
                if (_realDataRow is cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow)
                {
                    cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow row = _realDataRow as cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow;
                    return row.libelle_prefet;
                }

                if (_realDataRow is cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow)
                {
                    cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow row = _realDataRow as cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow;
                    return row.libelle_prefet;
                }

                throw new Exception("_realDataRow exception Mauvais type");
            }

            set
            {
                if (_realDataRow is cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow)
                {
                    cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow row = _realDataRow as cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow;
                    row.libelle_prefet = value;
                }

                if (_realDataRow is cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow)
                {
                    cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow row = _realDataRow as cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow;
                    row.libelle_prefet = value;
                }

                //throw new Exception("_realDataRow exception Mauvais type");
            }
        }

        public string nom
        {
            get
            {
                if (_realDataRow is cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow)
                {
                    cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow row = _realDataRow as cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow;
                    return row.nom;
                }

                if (_realDataRow is cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow)
                {
                    cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow row = _realDataRow as cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow;
                    return row.nom;
                }

                throw new Exception("_realDataRow exception Mauvais type");
            }

            set
            {
                if (_realDataRow is cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow)
                {
                    cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow row = _realDataRow as cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow;
                    row.nom = value;
                }

                if (_realDataRow is cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow)
                {
                    cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow row = _realDataRow as cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow;
                    row.nom = value;
                }

                //throw new Exception("_realDataRow exception Mauvais type");
            }
        }

        public string prenom
        {
            get
            {
                if (_realDataRow is cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow)
                {
                    cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow row = _realDataRow as cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow;
                    return row.prenom;
                }

                if (_realDataRow is cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow)
                {
                    cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow row = _realDataRow as cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow;
                    return row.prenom;
                }

                throw new Exception("_realDataRow exception Mauvais type");
            }

            set
            {
                if (_realDataRow is cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow)
                {
                    cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow row = _realDataRow as cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow;
                    row.prenom = value;
                }

                if (_realDataRow is cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow)
                {
                    cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow row = _realDataRow as cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow;
                    row.prenom = value;
                }

                //throw new Exception("_realDataRow exception Mauvais type");
            }
        }

        public string cod_agrem_controleur
        {
            get
            {
                if (_realDataRow is cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow)
                {
                    cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow row = _realDataRow as cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow;
                    return row.cod_agrem_controleur;
                }

                if (_realDataRow is cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow)
                {
                    cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow row = _realDataRow as cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow;
                    return row.cod_agrem_controleur;
                }

                throw new Exception("_realDataRow exception Mauvais type");
            }

            set
            {
                if (_realDataRow is cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow)
                {
                    cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow row = _realDataRow as cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow;
                    row.cod_agrem_controleur = value;
                }

                if (_realDataRow is cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow)
                {
                    cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow row = _realDataRow as cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow;
                    row.cod_agrem_controleur = value;
                }

                //throw new Exception("_realDataRow exception Mauvais type");
            }

        }

        public string cod_agrem_centre_rattach
        {
            get
            {
                if (_realDataRow is cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow)
                {
                    cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow row = _realDataRow as cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow;
                    return row.cod_agrem_centre_rattach;
                }

                throw new Exception("_realDataRow exception Mauvais type pour cod_agrem_centre_rattach");
            }

            set
            {
                if (_realDataRow is cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow)
                {
                    cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow row = _realDataRow as cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow;
                    row.cod_agrem_centre_rattach = value;
                }

                //throw new Exception("_realDataRow exception Mauvais type pour cod_agrem_centre_rattach");
            }
        }

        public System.DateTime date_effet_notification
        {
            get
            {
                if (_realDataRow is cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow)
                {
                    cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow row = _realDataRow as cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow;
                    return row.date_effet_notification;
                }

                if (_realDataRow is cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow)
                {
                    cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow row = _realDataRow as cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow;
                    return row.date_effet_notification;
                }

                throw new Exception("_realDataRow exception Mauvais type");
            }

            set
            {
                if (_realDataRow is cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow)
                {
                    cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow row = _realDataRow as cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow;
                    row.date_effet_notification = value;
                }

                if (_realDataRow is cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow)
                {
                    cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow row = _realDataRow as cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow;
                    row.date_effet_notification = value;
                }

                //throw new Exception("_realDataRow exception Mauvais type");
            }
        }

        public System.DateTime date_naissance
        {
            get
            {
                if (_realDataRow is cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow)
                {
                    cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow row = _realDataRow as cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow;
                    return row.date_naissance;
                }

                if (_realDataRow is cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow)
                {
                    cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow row = _realDataRow as cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow;
                    return row.date_naissance;
                }

                throw new Exception("_realDataRow exception Mauvais type");
            }

            set
            {
                if (_realDataRow is cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow)
                {
                    cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow row = _realDataRow as cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow;
                    row.date_naissance = value;
                }

                if (_realDataRow is cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow)
                {
                    cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow row = _realDataRow as cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow;
                    row.date_naissance = value;
                }

                //throw new Exception("_realDataRow exception Mauvais type");
            }
        }

        public string cod_agrem_centre_rattach_ancien
        {
            get
            {
                if (_realDataRow is cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow)
                {
                    cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow row = _realDataRow as cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow;
                    return row.cod_agrem_centre_rattach_ancien;
                }

                throw new Exception("_realDataRow exception Mauvais type pour cod_agrem_centre_rattach_ancien");
            }

            set
            {
                if (_realDataRow is cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow)
                {
                    cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow row = _realDataRow as cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow;
                    row.cod_agrem_centre_rattach_ancien = value;
                }

                //throw new Exception("_realDataRow exception Mauvais type pour cod_agrem_centre_rattach_ancien");

            }
        }

        public string cod_agrem_centre_rattach_nouveau
        {
            get
            {
                if (_realDataRow is cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow)
                {
                    cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow row = _realDataRow as cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow;
                    return row.cod_agrem_centre_rattach_nouveau;
                }

                throw new Exception("_realDataRow exception Mauvais type pour cod_agrem_centre_rattach_nouveau");
            }

            set
            {
                if (_realDataRow is cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow)
                {
                    cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow row = _realDataRow as cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow;
                    row.cod_agrem_centre_rattach_nouveau = value;
                }

                //throw new Exception("_realDataRow exception Mauvais type pour cod_agrem_centre_rattach_nouveau");

            }
        }

        public bool est_motif_cessation_activite
        {
            get
            {
                if (_realDataRow is cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow)
                {
                    cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow row = _realDataRow as cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow;
                    return row.est_motif_cessation_activite;
                }

                throw new Exception("_realDataRow exception Mauvais type pour est_motif_cessation_activite");
            }

            set
            {
                if (_realDataRow is cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow)
                {
                    cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow row = _realDataRow as cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow;
                    row.est_motif_cessation_activite = value;
                }

                //throw new Exception("_realDataRow exception Mauvais type pour est_motif_cessation_activite");

            }
        }

        public bool est_motif_chang_centre_rattach
        {
            get
            {
                if (_realDataRow is cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow)
                {
                    cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow row = _realDataRow as cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow;
                    return row.est_motif_chang_centre_rattach;
                }

                throw new Exception("_realDataRow exception Mauvais type pour est_motif_chang_centre_rattach");
            }

            set
            {
                if (_realDataRow is cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow)
                {
                    cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow row = _realDataRow as cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow;
                    row.est_motif_chang_centre_rattach = value;
                }

                //throw new Exception("_realDataRow exception Mauvais type pour est_motif_chang_centre_rattach");

            }
        }

        public System.DateTime date_edition
        {
            get
            {
                if (_realDataRow is cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow)
                {
                    cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow row = _realDataRow as cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow;
                    return row.date_edition;
                }

                if (_realDataRow is cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow)
                {
                    cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow row = _realDataRow as cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow;
                    return row.date_edition;
                }

                throw new Exception("_realDataRow exception Mauvais type");
            }

            set
            {
                if (_realDataRow is cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow)
                {
                    cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow row = _realDataRow as cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow;
                    row.date_edition = value;
                }

                if (_realDataRow is cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow)
                {
                    cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow row = _realDataRow as cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow;
                    row.date_edition = value;
                }

                //throw new Exception("_realDataRow exception Mauvais type");
            }
        }
        public System.Boolean est_sva
        {
            get
            {
                if (_realDataRow is cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow)
                {
                    cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow row = _realDataRow as cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow;
                    return row.est_sva;
                }

                if (_realDataRow is cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow)
                {
                    cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow row = _realDataRow as cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow;
                    return row.est_sva;
                }

                throw new Exception("_realDataRow exception Mauvais type");
            }

            set
            {
                if (_realDataRow is cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow)
                {
                    cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow row = _realDataRow as cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow;
                    row.est_sva = value;
                }

                if (_realDataRow is cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow)
                {
                    cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow row = _realDataRow as cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow;
                    row.est_sva = value;
                }

                //throw new Exception("_realDataRow exception Mauvais type");
            }
        }


    }

}
