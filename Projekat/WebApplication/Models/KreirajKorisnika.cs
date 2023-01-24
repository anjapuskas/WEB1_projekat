using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class KreirajKorisnika
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Lozinka { get; set; }

        public string Ime { get; set; }

        public string Prezime { get; set; }

        public string Pol { get; set; }

        public string Email { get; set; }

        public string DatumRodjenja { get; set; }

    }
}