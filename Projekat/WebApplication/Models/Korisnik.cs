using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{

    public class Korisnik
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Lozinka { get; set; }

        public string Ime { get; set; }

        public string Prezime { get; set; }

        public string Pol { get; set; }

        public string Email { get; set; }

        public DateTime DatumRodjenja { get; set; }

        public Uloga Uloga { get; set; }

        public List<GrupniTrening> PrijavljenTrening { get; set; }

        public List<GrupniTrening> AngazovanTrening { get; set; }

        public FitnesCentar AngazovanFitnesCentar { get; set; }

        public List<FitnesCentar> VlasnikFitnesCentar { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsBlocked { get; set; }

    }
}