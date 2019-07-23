using LibraryProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LibraryProject.Controllers
{
    public class PretController : Controller
    {
        ApplicationDbContext _dbContext;
        public PretController()
        {

            _dbContext = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _dbContext.Dispose();
        }
        // GET: Pret
        [Authorize]
        public ActionResult Show()
        {
            if (User.IsInRole("AdminAccount"))
            {
                var _listLivres = _dbContext.Prets.ToList();
                return View("ShowPretsForAdmin", _listLivres);
            }
            else
            {
                var _listLivres = _dbContext.Prets.ToList();                
                var _listOfPretsForUser = new List<Pret>();

                foreach (var item in _listLivres)
                {

                    if (this.User.Identity.Name.Equals(item.IDUser))
                    {
                        _listOfPretsForUser.Add(item);
                    }
                }

                return View(_listOfPretsForUser);

            }            
        }

        // GET: Pret/Retour/id
        [HttpGet]
        [Authorize]
        [Route("pret/retour/{id:regex(\\d+)}")]
        public ActionResult Retour(int id)
        {
            var userIDCount = _dbContext.Prets.Count(c => c.IDUser == this.User.Identity.Name);
            if (User.IsInRole("AdminAccount"))
            {
                try
                {
                    var uniquePret = _dbContext.Prets.Single(r => r.ID == id);
                    return View("RetourAdmin", uniquePret);
                }
                catch
                {
                    return HttpNotFound();
                }
            }
            else {
                try
                {
                    var uniquePret = _dbContext.Prets.Single(r => r.ID == id);
                    return View(uniquePret);
                }
                catch
                {
                    return HttpNotFound();
                }
            }
            

        }
        // POST: Pret/retourbyid/id
        [Authorize]
        [HttpPost]
        [Route("pret/retournerbyid/{id:regex(\\d+)}")]
        public ActionResult RetournerByID(int id)
        {
            var userIDCount = _dbContext.Reservations.Count(c => c.IDUser == this.User.Identity.Name);
            try
            {
                ViewBag.Total = userIDCount;
                var _resForBack = _dbContext.Prets.Single(r => r.ID == id);
                var _numberInCatalog = _resForBack.NumberInCatalog;
                var _catalogLivres = _dbContext.Livres;
                var _catalogPrets = _dbContext.Prets;
                var livreInCatalog = _catalogLivres.Single(c => c.Id == _numberInCatalog);
                livreInCatalog.NumberAvalible = "Disponible";                
                _catalogPrets.Remove(_resForBack);                
                _dbContext.SaveChanges();
                return RedirectToAction("index", "home");
            }
            catch
            {
                return HttpNotFound();
            }
        }
    }
}