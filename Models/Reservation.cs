using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LibraryProject.Models
{
    public class Reservation
    {

        [Key]
        public int ID { get; set; }

       
        [Display(Name = "Nom")]
        public String Name { get; set; }

       
        [Display(Name = "Description")]
        public String Description { get; set; }

        
        [Display(Name = "Auteur")]
        public String Author { get; set; }

        [Display(Name = "Statut")]
        public String NumberAvalible { get; set; }

       
        [Display(Name = "Categorie")]
        public String Category { get; set; }

       
        [Display(Name = "Date de reservation")]
        public DateTime DateTimeReservation { get; set; }
        
        [Display(Name = "Date de annulation")]
        public DateTime DateTimeRetour { get; set; }

        public int NumberInCatalog { get; set; }

        public string IDUser { get; set; }

    }
}