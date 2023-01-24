using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class FitnesCentarKomentar
    {
        public FitnesCentar FitnesCentar { get; set; }

        public Komentar Komentar { get; set; }

        public FitnesCentarKomentar(FitnesCentar fitnesCentar, Komentar komentar)
        {
            FitnesCentar = fitnesCentar;
            Komentar = komentar;
        }

        public FitnesCentarKomentar() {}
    }
}