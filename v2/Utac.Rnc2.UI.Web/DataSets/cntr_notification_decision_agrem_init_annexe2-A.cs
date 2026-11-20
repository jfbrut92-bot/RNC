using Utac.Rnc2.BusinessObjects;

namespace Utac.Rnc2.UI.Web.DataSets
{

    public partial class cntr_notification_decision_agrem_init_annexe2_A
    {
        public void mapCentreDerogationsDataset(DataSets.cntr_notification_decision_agrem_init_annexe2_A ds, pCentreEncour centre)
        {
            cntr_notification_decision_agrem_init_annexe2_A.pDerogationCentreDataTable dtDerog = ds.pDerogationCentre;

            if (ds.notification_centre.Count == 0)
            {
                return;
            }

            ds.notification_centre[0].est_soumis_derogation = false;

            foreach (pDerogationCentre derog in centre.PDerogationCentres)
            {

                if ((ds.notification_centre[0].est_type_agrem_cl & derog.CategorieVehicule.Equals(rCategorieVehicule.CategorieEnum.CL)) ||
                    (ds.notification_centre[0].est_type_agrem_vl & derog.CategorieVehicule.Equals(rCategorieVehicule.CategorieEnum.VL)) ||
                    (ds.notification_centre[0].est_type_agrem_pl & derog.CategorieVehicule.Equals(rCategorieVehicule.CategorieEnum.PL)))
                {
                    cntr_notification_decision_agrem_init_annexe2_A.pDerogationCentreRow drDerog = dtDerog.NewpDerogationCentreRow();

                    if (derog.DateDebut != null) { drDerog.dateDebut = derog.DateDebut.Value; }
                    if (derog.DateFin != null) { drDerog.dateFin = derog.DateFin.Value; }
                    drDerog.commentaire = derog.Commentaire;
                    drDerog.categorieVehicule = derog.CategorieVehicule.ToString();

                    dtDerog.Rows.Add(drDerog);

                    ds.notification_centre[0].est_soumis_derogation = true;
                }

            }
        }

    }
}

