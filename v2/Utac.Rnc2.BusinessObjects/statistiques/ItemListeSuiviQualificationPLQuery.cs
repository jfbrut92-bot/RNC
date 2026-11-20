using System;

namespace Utac.Rnc2.BusinessObjects.statistiques
{
    /*
    27/08/2018 -PLE- Suivi qualification contrôleur PL
    */
    public class ItemListeSuiviQualificationPLQuery
    {
        public pContoleurEncour Controleur { get; set; }
        public pCumulActivitePlJrnTrt CumulActiviteTraitement { get; set; }
        public int MoisDebut
        {
            get
            {
                if (CumulActiviteTraitement == null) throw new Exception("Impossible de calculer le premier mois de prise en compte");
                //int i_mois_tmp = this.CumulActiviteTraitement.Id % 100;
                int i_anne_tmp = this.CumulActiviteTraitement.Id / 100;
                //int i_mois_deb_tmp;
                //int i_anne_deb_tmp;

                //if ( i_mois_tmp == 12 )
                //{
                //    i_mois_deb_tmp = 1 ;
                //    i_anne_deb_tmp = i_anne_tmp;
                //}
                //else
                //{
                //    i_mois_deb_tmp = i_mois_tmp + 1;
                //    i_anne_deb_tmp = i_anne_tmp - 1;
                //}
                //return i_anne_deb_tmp * 100 + i_mois_deb_tmp;

                //La période commence en janvier de l'année n
                return i_anne_tmp * 100 + 1;

            }
        }
        public int MoisFin
        {
            get
            {
                if (CumulActiviteTraitement == null) throw new Exception("Impossible de calculer le dernier mois de prise en compte");
                return this.CumulActiviteTraitement.Id;
            }
        }
        public DateTime DateMoisDebut
        {
            get
            {
                DateTime dt = new DateTime(this.MoisDebut / 100, this.MoisDebut % 100, 1);
                return dt;
            }
        }
        public DateTime DateMoisFin
        {
            get
            {
                int jour = DateTime.DaysInMonth(this.MoisFin / 100, this.MoisFin % 100);
                DateTime dt = new DateTime(this.MoisFin / 100, this.MoisFin % 100, jour);
                return dt;
            }

        }

        /// <summary>
        /// 27/08/2018 -PLE- Suivi qualification contrôleur PL  : année n-1 début
        /// </summary>
        public int annee_n_moins1Debut
        {
            get
            {
                int i_mois_tmp = this.CumulActiviteTraitement.Id % 100;
                int i_anne_tmp = this.CumulActiviteTraitement.Id / 100;

                return (i_anne_tmp - 1) * 100 + 1;
            }

        }
        /// <summary>
        /// 27/08/2018 -PLE- Suivi qualification contrôleur PL  : année n-1 fin
        /// </summary>
        public int annee_n_moins1Fin
        {
            get
            {
                int i_mois_tmp = this.CumulActiviteTraitement.Id % 100;
                int i_anne_tmp = this.CumulActiviteTraitement.Id / 100;

                return (i_anne_tmp - 1) * 100 + 12;
            }

        }

        /// <summary>
        /// 27/08/2018 -PLE- Suivi qualification contrôleur PL  : année n-1
        /// </summary>
        public int annee_n_moins1
        {
            get
            {
                int i_mois_tmp = this.CumulActiviteTraitement.Id % 100;
                int i_anne_tmp = this.CumulActiviteTraitement.Id / 100;

                return (i_anne_tmp - 1);
            }

        }
    }
}
