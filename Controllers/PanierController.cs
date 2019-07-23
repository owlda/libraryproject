using LibraryProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LibraryProject.Controllers
{
    public class PanierController : Controller
    {
        ApplicationDbContext _dbContext;
        public PanierController()
        {

            _dbContext = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _dbContext.Dispose();
        }

        // GET: Panier
        [HttpGet]
        [Authorize]
        [Route("panier/show")]
        public ActionResult Show()        {
           
            var userIDCount = _dbContext.Reservations.Count(c => c.IDUser == this.User.Identity.Name);            
            ViewBag.Total = userIDCount;

            if (User.IsInRole("AdminAccount"))
            {
                var _listLivres = _dbContext.Reservations.ToList();
                return View("ShowReservationsForAdmin", _listLivres);
            }
            else {
                var _listLivres = _dbContext.Reservations.ToList();
                var _listNamesOnly = _dbContext.Livres.ToList();
                var _listOfReservationsForUser = new List<Reservation>();

                foreach (var item in _listLivres)
                {

                    if (this.User.Identity.Name.Equals(item.IDUser))
                    {
                        _listOfReservationsForUser.Add(item);
                    }
                }

                return View(_listOfReservationsForUser);

            }              
            
            
        }

        // GET: Panier/Retour/id
        [HttpGet]
        [Authorize]
        [Route("panier/retour/{id:regex(\\d+)}")]
        public ActionResult Retour(int id)
        {
            var userIDCount = _dbContext.Reservations.Count(c => c.IDUser == this.User.Identity.Name);
                try
                {
                    ViewBag.Total = userIDCount;
                    var uniqueReservation = _dbContext.Reservations.Single(r => r.ID == id);
                    return View(uniqueReservation);
                }
                catch
                {
                    return HttpNotFound();
                }
                       
        }

        // POST: Panier/retourbyid/id
        [Authorize]
        [HttpPost]        
        [Route("panier/retourbyid/{id:regex(\\d+)}")] ///panier/retourbyid/2
        public ActionResult RetourByID(int id)
        {
            var userIDCount = _dbContext.Reservations.Count(c => c.IDUser == this.User.Identity.Name);
            var userIDReservation = _dbContext.Reservations.Single(c => c.ID == id).IDUser;
            var livreIDCount = _dbContext.Prets.Count(c => c.IDUser == userIDReservation);
            try
            {
                if (livreIDCount >= 3)
                {
                    return View("ErrorPret");

                }
                else {
                    ViewBag.Total = userIDCount;
                    var _resForBack = _dbContext.Reservations.Single(r => r.ID == id);
                    var _numberInCatalog = _resForBack.NumberInCatalog;
                    var _catalogLivres = _dbContext.Livres;
                    var _catalogPrets = _dbContext.Prets;
                    var livreInCatalog = _catalogLivres.Single(c => c.Id == _numberInCatalog);
                    var _catalogReservation = _dbContext.Reservations;
                    var _livrePret = new Pret()
                    {
                        Author = _resForBack.Author,
                        Category = _resForBack.Category,
                        DateTimeReservation = DateTime.Now,
                        DateTimeRetour = DateTime.Now.AddDays(7),
                        Description = _resForBack.Description,
                        IDUser = _resForBack.IDUser,
                        Name = _resForBack.Name,
                        NumberAvalible = _resForBack.NumberAvalible,
                        NumberInCatalog = _resForBack.NumberInCatalog
                    };
                    _catalogPrets.Add(_livrePret);
                    _catalogReservation.Remove(_resForBack);
                    _dbContext.SaveChanges();
                    return RedirectToAction("show", "panier");
                }

            }
            catch
            {
                return HttpNotFound();
            }
        }

        // GET: Panier/Retour/id
        [HttpGet]
        [Authorize]
        [Route("panier/annuler/{id:regex(\\d+)}")]
        public ActionResult Annuler(int id)
        {
            
            try
            {
                
                var uniqueAnnulation = _dbContext.Reservations.Single(r => r.ID == id);
                return View(uniqueAnnulation);
            }
            catch
            {
                return HttpNotFound();
            }

        }

        // POST: Panier/annulerbyid/id
        [Authorize]
        [HttpPost]
        [Route("panier/annulerbyid/{id:regex(\\d+)}")]
        public ActionResult AnnulerByID(int id)
        {
            
            try
            {
                
                var _resForBack = _dbContext.Reservations.Single(r => r.ID == id);
                var _numberInCatalog = _resForBack.NumberInCatalog;
                var _catalogLivres = _dbContext.Livres;                
                var livreInCatalog = _catalogLivres.Single(c => c.Id == _numberInCatalog);
                var _catalogReservation = _dbContext.Reservations;

                livreInCatalog.NumberAvalible = "Disponible";
                _catalogReservation.Remove(_resForBack);
                _dbContext.SaveChanges();
                return RedirectToAction("show", "panier");

            }
            catch
            {
                return HttpNotFound();
            }
        }



    }
}
