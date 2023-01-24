using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class FitnesCentarGrupniTreninziKomentari
    {
        public FitnesCentar FitnesCentar { get; set; }

        public List<GrupniTrening> GrupniTreninzi { get; set; }

        public List<Komentar> Komentari { get; set; }

        public FitnesCentarGrupniTreninziKomentari(FitnesCentar fitnesCentar, List<GrupniTrening> grupniTreninzi, List<Komentar> komentari)
        {
            FitnesCentar = fitnesCentar;
            GrupniTreninzi = grupniTreninzi;
            Komentari = komentari;
        }
    }
}