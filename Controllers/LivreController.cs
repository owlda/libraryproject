using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LibraryProject.Models;
using LibraryProject.Models.LivresViewModel;

namespace LibraryProject.Views.Home
{
    public class LivreController : Controller
    {

        ApplicationDbContext _dbContextLivre;
        public LivreController() {

            _dbContextLivre = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _dbContextLivre.Dispose();
        }

        // GET: Livre
        [HttpGet]
        [Authorize]
        [Route("livre/show")]
        public ActionResult Show()
        {
            if (User.IsInRole("AdminAccount"))
            {
                var userID = _dbContextLivre.Reservations.Count(c => c.IDUser == this.User.Identity.Name);
                ViewBag.Total = _dbContextLivre.Reservations.Count(c => c.IDUser == this.User.Identity.Name);
                var _listFromDBContext = _dbContextLivre.Livres.ToList();
                return View(_listFromDBContext);
            }
            else
            {
                var userID = _dbContextLivre.Reservations.Count(c => c.IDUser == this.User.Identity.Name);
                ViewBag.Total = _dbContextLivre.Reservations.Count(c => c.IDUser == this.User.Identity.Name);
                var _listFromDBContext = _dbContextLivre.Livres.ToList();
                return View("ShowForUser", _listFromDBContext);
            }          
        }
        // GET: Livre/Create
        [HttpGet]
        [Authorize]
        [Route("livre/create")]
        public ActionResult Create() {
            ViewBag.Total = _dbContextLivre.Reservations.Count(c => c.IDUser == this.User.Identity.Name);
            var _modelCreateLivre = new Livre(){};
            return View(_modelCreateLivre);
        }
        // POST: Livre/Create
        [HttpPost]
        [Authorize]
        [Route("livre/create")]
        public ActionResult Create(Livre livre)
        {
            if (ModelState.IsValid)
            {

                ViewBag.Total = _dbContextLivre.Reservations.Count(c => c.IDUser == this.User.Identity.Name);
                _dbContextLivre.Livres.Add(livre);
                _dbContextLivre.SaveChanges();
                return RedirectToAction("index", "home");
            }
            else {

                return View("ErrorCreate");
            }
              

        }
        // GET: Livre/Edit/5        
        [Authorize]
        public ActionResult Edit(int id)
        {
            ViewBag.Total = _dbContextLivre.Reservations.Count(c => c.IDUser == this.User.Identity.Name);
            var _livre = _dbContextLivre.Livres.SingleOrDefault(c => c.Id == id);
            if (_livre == null)
            {
                return HttpNotFound();
            }
            else
            {
                var modelEdit = _livre;                
                return View(modelEdit);

            }
        }
        // POST: Livre/Edit/5
        [HttpPost]
        [Authorize]
        public ActionResult Edit(Livre livre)
        {

            var livreInDB = _dbContextLivre.Livres.Single(c => c.Id == livre.Id);
            ViewBag.Total = _dbContextLivre.Reservations.Count(c => c.IDUser == this.User.Identity.Name);
            livreInDB.Name = livre.Name;
            livreInDB.Description = livre.Description;
            livreInDB.Author = livre.Author;
            livreInDB.Category = livre.Category;
            livreInDB.NumberAvalible = livre.NumberAvalible;

            _dbContextLivre.SaveChanges();
            return RedirectToAction("show", "livre");

        }
        // GET: Livre/DELETE/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            ViewBag.Total = _dbContextLivre.Reservations.Count(c => c.IDUser == this.User.Identity.Name);
            var _livre = _dbContextLivre.Livres.SingleOrDefault(c => c.Id == id);
            if (_livre == null)
            {
                return HttpNotFound();
            }
            else
            {
                var modelDelete = new Livre(){};
                return View(modelDelete);
            }
        }
        // POST: Livre/DELETE/5
        [HttpPost]
        [Authorize]
        public ActionResult Delete(Livre livre)
        {
            ViewBag.Total = _dbContextLivre.Reservations.Count(c => c.IDUser == this.User.Identity.Name);
            var livreInDB = _dbContextLivre.Livres.Single(c => c.Id == livre.Id);
            _dbContextLivre.Livres.Remove(livreInDB);
            _dbContextLivre.SaveChanges();
            return RedirectToAction("Show", "livre");

        }

        [HttpGet]
        [Authorize]
        [Route("livre/details/{id:regex(\\d+)}")]
        public ActionResult ShowByID(int id)
        {
            var details = _dbContextLivre.Livres.Single(c => c.Id == id);
            if (details == null)
            {
                return HttpNotFound();
            }
            else
            {
                ViewBag.Total = _dbContextLivre.Reservations.Count(c => c.IDUser == this.User.Identity.Name);
                var viewModel = details;
                return View(viewModel);
            }

        }

        // GET: Reserver
        [HttpGet]
        [Authorize]
        [Route("livre/reserver/{id:regex(\\d+)}")]
        public ActionResult Reserver(int id)
        {           
                var _rent = _dbContextLivre.Livres.Single(c => c.Id == id);
                if (_rent == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    ViewBag.Total = _dbContextLivre.Reservations.Count(c => c.IDUser == this.User.Identity.Name);
                    var viewModel = _rent;
                    return View(viewModel);
                }        

        }       

        // POST: Livre/RESERVERPOST/5
        [HttpPost]
        [Authorize]
        public ActionResult ReserverPost(int id)
        {
            ViewBag.Total = _dbContextLivre.Reservations.Count(c => c.IDUser == this.User.Identity.Name);
            
            if (_dbContextLivre.Reservations.Count(c => c.IDUser == this.User.Identity.Name) >= 3)
            {
                ViewBag.Total = _dbContextLivre.Reservations.Count(c => c.IDUser == this.User.Identity.Name);
                return View("Error");
            }
            else
            {
                var _reservationDB = _dbContextLivre.Reservations;
                var _livresDB = _dbContextLivre.Livres;

                var livreInDB = _dbContextLivre.Livres.Single(c => c.Id == id);

                if (livreInDB == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    livreInDB.NumberAvalible = "Non disponible";

                    var reservation = new Reservation()
                    {
                        DateTimeReservation = DateTime.Now,
                        DateTimeRetour = DateTime.Now.AddDays(3),
                        Author = livreInDB.Author,
                        Category = livreInDB.Category,
                        Description = livreInDB.Description,
                        IDUser = this.User.Identity.Name,
                        Name = livreInDB.Name,
                        NumberAvalible = "Non disponible",
                        NumberInCatalog = livreInDB.Id
                    };
                    _reservationDB.Add(reservation);
                    _dbContextLivre.SaveChanges();
                    return RedirectToAction("Show", "livre");
                }

            }          

        }

    }    
}