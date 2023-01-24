using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class GrupniTrening
    {
        public int Id { get; set; }

        public string Naziv { get; set; }

        public string Tip { get; set; }

        public FitnesCentar FitnesCentar { get; set; }

        public int TrajanjeTreninga { get; set; }

        public DateTime DatumTreninga { get; set; }

        public int MaximalanBroj { get; set; }

        public List<Korisnik> Posetioci { get; set; }

        public bool IsDeleted { get; set; }


    }
}