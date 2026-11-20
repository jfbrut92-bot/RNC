using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Utac.Rnc2.BusinessObjects;
//using Utac.Rnc2.Repository.Nhibernate;
using Utac.Rnc2.Contrats.Services;
using Utac.Rnc2.Fabriques;
using Utac.Rnc2.Infrastructure.Queries;

namespace Utac.Rnc2.UI.Web.Controllers
{
    public class NumOrdreController : RestrictedAcessController
    {
        private readonly IrNumordreAgrementService _rNumordreAgrementService;
        private readonly IrPrefectureService _prefectureService;

        //Constructeurs

        public NumOrdreController(IrNumordreAgrementService service, IrPrefectureService service1)
        {
            this._rNumordreAgrementService = service;
            this._prefectureService = service1;
        }

        // public NumOrdreController() :this(new rNumordreAgrementService(new rNumordreAgrementRepository(),new NhibernateUnitOfWork ()))
        // {

        // }

        public NumOrdreController()
            : this(NhibernateServiceFactory.getInstance().getrNumordreAgrementService(true),
                   NhibernateServiceFactory.getInstance().getrPrefectureService(true)
                  )
        {

        }

        //
        // GET: /NumOrdre/Index

        public ActionResult Index(int? page)
        {
            long count = 0;
            int p = (page == null) ? 1 : Convert.ToInt32(page);
            List<rNumordreAgrement> liste = this._rNumordreAgrementService.FindByPage(new Query(), p, 20, out count);
            MvcContrib.Pagination.IPagination listePaginee = new MvcContrib.Pagination.CustomPagination<rNumordreAgrement>(liste, p, 20, Convert.ToInt32(count));
            return View(listePaginee);
        }

        //
        // GET: /NumOrdre/Edit/id

        public ActionResult Edit(int id)
        {
            rNumordreAgrement num = this._rNumordreAgrementService.FindBy(id);
            return View(num);
        }

        // POST: /NumOrdre/Edit/id
        /*
        [HttpPost]
        public ActionResult Edit(int id , FormCollection form)
        {
            try

            {
                //1-Récupération de l'Id
                rNumordreAgrement numOrdre = this._rNumordreAgrementService.FindBy(id);
                //2-Modification des valeurs
                numOrdre.NumOrdreCentre = Convert.ToInt32(form["NumOrdreCentre"]);
                numOrdre.NumOrdreCntlrPl = Convert.ToInt32(form["NumOrdreCntlrPl"]);
                numOrdre.NumOrdreCntlrVl = Convert.ToInt32(form["NumOrdreCntlrVl"]);
                //3-Sauvegarde
                this._rNumordreAgrementService.Save(numOrdre);
                //4-Commit
                this._rNumordreAgrementService.getUnitOfWork().Commit();
                //5-Redirection sur la page index pour éviter le problème du F5 ??????
                return RedirectToAction("Index");
            }
            catch (Exception e) 
            {
                //1-rappeler la vue en mode edit
                //2-récuperer les valeurs du form
                //3-afficher l'erreur
                return View();
            }
        }
        */

        // POST: /NumOrdre/Edit/id
        [HttpPost]
        public ActionResult Edit(rNumordreAgrement objForm)
        {
            try
            {
                //ATTENTION: ne jamais sauvegarder objForm car tous les champs ne sont pas remplis et de plus nhibernate ne connaît pas l'état de l'objet
                //           risque de duplicate key
                //1-Récupération de l'Id
                if (this.ModelState.IsValid == true)
                {
                    rNumordreAgrement numOrdre = this._rNumordreAgrementService.FindBy(objForm.Id);
                    //2-Modification des valeurs
                    numOrdre.NumOrdreCentre = objForm.NumOrdreCentre;
                    numOrdre.NumOrdreCntlrPl = objForm.NumOrdreCntlrPl;
                    numOrdre.NumOrdreCntlrVl = objForm.NumOrdreCntlrVl;
                    //PLE 01/03/2024 Evolution CYCLO
                    numOrdre.NumOrdreCntlrCl = objForm.NumOrdreCntlrCl;
                    numOrdre.RPrefecture.Prefixe = objForm.RPrefecture.Prefixe;
                    numOrdre.RPrefecture.LibTribuAdmin = objForm.RPrefecture.LibTribuAdmin;
                    //3-Sauvegarde
                    this._rNumordreAgrementService.Save(numOrdre);
                    //4-Commit
                    this._rNumordreAgrementService.getUnitOfWork().Commit();

                    //06/08/2014 PLE
                    //Gestion de la mise à jour du préfixe Préfecture et tribunal administratif
                    Boolean savePrefecture = false;
                    if (objForm.RPrefecture.LibTribuAdmin != null)
                    {
                        if ((numOrdre.RPrefecture.LibTribuAdmin == null) || (numOrdre.RPrefecture.LibTribuAdmin != null && !numOrdre.RPrefecture.LibTribuAdmin.Equals(objForm.RPrefecture.LibTribuAdmin)))
                        {
                            numOrdre.RPrefecture.LibTribuAdmin = objForm.RPrefecture.LibTribuAdmin;
                            savePrefecture = true;
                        }
                    }
                    if (objForm.RPrefecture.Prefixe != null)
                    {
                        if ((numOrdre.RPrefecture.Prefixe == null) || (numOrdre.RPrefecture.Prefixe != null && !numOrdre.RPrefecture.Prefixe.Equals(objForm.RPrefecture.Prefixe)))
                        {
                            numOrdre.RPrefecture.Prefixe = objForm.RPrefecture.Prefixe;
                            savePrefecture = true;
                        }
                    }

                    if (savePrefecture)
                    {
                        this._prefectureService.Save(numOrdre.RPrefecture);
                        this._prefectureService.getUnitOfWork().Commit();
                    }

                    //5-Redirection sur la page index pour éviter le problème du F5 ??????
                    return RedirectToAction("Index");
                }
                else throw new Exception("Saisie invalide");
            }
            catch (Exception e)
            {
                //1-rappeler la vue en mode edit
                //2-récuperer les valeurs du form
                //3-afficher l'erreur
                this.ViewData["erreur"] = e.Message;
                return View(objForm);
            }
        }

    }
}
