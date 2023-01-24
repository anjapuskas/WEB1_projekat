using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class KreiranjeTrenera
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Lozinka { get; set; }

        public string Ime { get; set; }

        public string Prezime { get; set; }

        public string Pol { get; set; }

        public string Email { get; set; }

        public int FitnesCentarId { get; set; }

        public string DatumRodjenja { get; set; }


    }
}