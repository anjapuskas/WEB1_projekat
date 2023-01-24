using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class FitnesCentar
    {
        [Required]
        public int Id { get; set; }

        public string Naziv { get; set; }

        public Adresa Adresa { get; set; }

        public int Godina { get; set; }

        public Korisnik Vlasnik { get; set; }

        public double CenaMesecne { get; set; }

        public double CenaGodisnje { get; set; }

        public double CenaJednog { get; set; }

        public double CenaGrupnog { get; set; }

        public double CenaPersonalnog { get; set; }

        public bool IsDeleted { get; set; }

        public string VlasnikImePrezime
        {
            get
            {
                if (Vlasnik != null)
                {
                    return Vlasnik.Ime + " " + Vlasnik.Prezime;
                }
                return "";
            }
        }


    }
}