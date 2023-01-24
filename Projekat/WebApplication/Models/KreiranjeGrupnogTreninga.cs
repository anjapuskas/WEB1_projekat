using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class KreiranjeGrupnogTreninga
    {
        public int Id { get; set; }

        public string Naziv { get; set; }

        public string Tip { get; set; }

        public int FitnesCentarId { get; set; }

        public int TrajanjeTreninga { get; set; }

        public string DatumTreninga { get; set; }

        public int MaximalanBroj { get; set; }


    }
}