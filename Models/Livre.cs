using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LibraryProject.Models
{
    public class Livre
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Nom")]
        public String Name { get; set; }

        [Required]
        [Display(Name = "Description")]
        public String Description { get; set; }

        [Required]
        [Display(Name = "Auteur")]
        public String  Author { get; set; }

        [Display(Name = "Statut")]
        public String NumberAvalible { get; set; }

        [Required]
        [Display(Name = "Categorie")]
        public String Category{ get; set; }

     


    }
}