using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class ProsliBuduciGrupniTreninzi
    { 
        public List<GrupniTrening> ProsliGrupniTreninzi { get; set; }

        public List<GrupniTrening> BuduciGrupniTreninzi { get; set; }


        public ProsliBuduciGrupniTreninzi(List<GrupniTrening> prosliGrupniTreninzi, List<GrupniTrening> buduciGrupniTreninzi)
        {
            ProsliGrupniTreninzi = prosliGrupniTreninzi;
            BuduciGrupniTreninzi = buduciGrupniTreninzi;
        }

    }
}