using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class Komentar
    {
        public int Id { get; set; }

        public Korisnik Komentarisao { get; set; }

        public FitnesCentar FitnesCentar { get; set; }

        public string Tekst { get; set; }

        public int Ocena { get; set; }

        public bool IsBlocked { get; set; }

        public bool IsApproved { get; set; }
    }
}