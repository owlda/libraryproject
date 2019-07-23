using LibraryProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LibraryProject.Controllers
{
    public class HomeController : Controller
    {

        ApplicationDbContext _dbContextLivre;
        public HomeController ()
        {

            _dbContextLivre = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _dbContextLivre.Dispose();
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
           
            ViewBag.Total = _dbContextLivre.Reservations.Count(c => c.IDUser == this.User.Identity.Name);            
            return View();
        }

        public ActionResult About()
        {
           
            ViewBag.Total = _dbContextLivre.Reservations.Count(c => c.IDUser == this.User.Identity.Name);
           
            return View();
        }

        public ActionResult Contact()
        {
            
            ViewBag.Total = _dbContextLivre.Reservations.Count(c => c.IDUser == this.User.Identity.Name);           
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}