using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class FitnesCentarController : Controller
    {
        

        public ActionResult FitnesCentarTabela(string sort)
        {

            ViewBag.SortByNaziv = sort == "naziv asc" ? "naziv desc" : "naziv asc";
            ViewBag.SortByAdresa = sort == "adresa asc" ? "adresa desc" : "adresa asc";
            ViewBag.SortByGodina = sort == "godina asc" ? "godina desc" : "godina asc";
            sort = sort == null ? "naziv asc" : sort;

            List<FitnesCentar> fitnesCentri;
            using (StreamReader r = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\FitnesCentar.json"))
            {
                string json = r.ReadToEnd();
                fitnesCentri = JsonConvert.DeserializeObject<List<FitnesCentar>>(json);
            }

            List<FitnesCentar> filtriraniFitnesCentri = new List<FitnesCentar>();

            foreach (var fc in fitnesCentri)
            {
                if (!fc.IsDeleted)
                {
                    filtriraniFitnesCentri.Add(fc);
                }
            }

            return View(filtriraniFitnesCentri);
        }

        public ActionResult UcitajFitnesCentre(string pretragaNaziv, string pretragaAdresa, string pretragaGodinaMin, string pretragaGodinaMax, string sort)
        {
            
            ViewBag.SortByNaziv = sort == "naziv asc" ? "naziv desc" : "naziv asc";
            ViewBag.SortByAdresa = sort == "adresa asc" ? "adresa desc" : "adresa asc";
            ViewBag.SortByGodina = sort == "godina asc" ? "godina desc" : "godina asc";
            sort = sort == null ? "naziv asc" : sort;

            List<FitnesCentar> fitnesCentri;
            using (StreamReader r = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\FitnesCentar.json"))
            {
                string json = r.ReadToEnd();
                fitnesCentri = JsonConvert.DeserializeObject<List<FitnesCentar>>(json);
            }

            List<FitnesCentar> filtriraniFitnesCentri = new List<FitnesCentar>();
            List<FitnesCentar> pomocniFitnesCentri = new List<FitnesCentar>();

            foreach (var fc in fitnesCentri)
            {
                if(!fc.IsDeleted)
                {
                    filtriraniFitnesCentri.Add(fc);
                }
            }



        if (pretragaNaziv != null && !pretragaNaziv.Equals(""))
            {
                pomocniFitnesCentri = filtriraniFitnesCentri;
                filtriraniFitnesCentri = new List<FitnesCentar>();
                foreach (var fc in pomocniFitnesCentri)
                {
                    if (fc.Naziv.IndexOf(pretragaNaziv, 0, StringComparison.CurrentCultureIgnoreCase) >= 0)
                    {
                        filtriraniFitnesCentri.Add(fc);
                    }
                }
            }
            if(pretragaAdresa != null && !pretragaAdresa.Equals(""))
            {
                pomocniFitnesCentri = filtriraniFitnesCentri;
                filtriraniFitnesCentri = new List<FitnesCentar>();
                foreach (var fc in pomocniFitnesCentri)
                {
                    if (fc.Adresa.ToString().IndexOf(pretragaAdresa, 0, StringComparison.CurrentCultureIgnoreCase) >= 0)
                    {
                        filtriraniFitnesCentri.Add(fc);
                    }
                }
            }
            if (pretragaGodinaMin != null && !pretragaGodinaMin.Equals(""))
            {
                pomocniFitnesCentri = filtriraniFitnesCentri;
                filtriraniFitnesCentri = new List<FitnesCentar>();
                foreach (var fc in pomocniFitnesCentri)
                {
                    if (fc.Godina >= Int32.Parse(pretragaGodinaMin))
                    {
                        filtriraniFitnesCentri.Add(fc);
                    }
                }
            }

            if (pretragaGodinaMax != null && !pretragaGodinaMax.Equals(""))
            {
                pomocniFitnesCentri = filtriraniFitnesCentri;
                filtriraniFitnesCentri = new List<FitnesCentar>();
                foreach (var fc in pomocniFitnesCentri)
                {
                    if (fc.Godina <= Int32.Parse(pretragaGodinaMax))
                    {
                        filtriraniFitnesCentri.Add(fc);
                    }
                }
            }

            if (sort=="naziv asc")
            {
                filtriraniFitnesCentri = filtriraniFitnesCentri.OrderBy(ffc => ffc.Naziv).ToList();
            }
            if (sort == "naziv desc")
            {
                filtriraniFitnesCentri = filtriraniFitnesCentri.OrderByDescending(ffc => ffc.Naziv).ToList();
            }
            if (sort == "adresa asc")
            {
                filtriraniFitnesCentri = filtriraniFitnesCentri.OrderBy(ffc => ffc.Adresa.ToString()).ToList();
            }
            if (sort == "adresa desc")
            {
                filtriraniFitnesCentri = filtriraniFitnesCentri.OrderByDescending(ffc => ffc.Adresa.ToString()).ToList();
            }
            if (sort == "godina asc")
            {
                filtriraniFitnesCentri = filtriraniFitnesCentri.OrderBy(ffc => ffc.Godina).ToList();
            }
            if (sort == "godina desc")
            {
                filtriraniFitnesCentri = filtriraniFitnesCentri.OrderByDescending(ffc => ffc.Godina).ToList();
            }

            return View("FitnesCentarTabela", filtriraniFitnesCentri);
        }

        // GET: FitnesCentar/Details/5
        public ActionResult FitnesCentarViseInformacija(int id)
        {
            List<FitnesCentar> fitnesCentri;
            using (StreamReader r = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\FitnesCentar.json"))
            {
                string json = r.ReadToEnd();
                fitnesCentri = JsonConvert.DeserializeObject<List<FitnesCentar>>(json);
            }

            FitnesCentar fitnesCentar = new FitnesCentar();

            foreach (var fc in fitnesCentri)
            {
                if (fc.Id == id)
                {
                    fitnesCentar = fc;
                }
            }

            List<GrupniTrening> grupniTreninzi;
            List<GrupniTrening> filtriraniGrupniTreninzi = new List<GrupniTrening>();
            using (StreamReader r = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\GrupniTrening.json"))
            {
                string json = r.ReadToEnd();
                grupniTreninzi = JsonConvert.DeserializeObject<List<GrupniTrening>>(json);
            }

            foreach (var gt in grupniTreninzi)
            {
                if (DateTime.Compare(gt.DatumTreninga, DateTime.Now) > 0 && !gt.IsDeleted && gt.FitnesCentar.Id == fitnesCentar.Id)
                {
                    filtriraniGrupniTreninzi.Add(gt);
                }
            }

            List<Komentar> komentari;
            using (StreamReader r = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\Komentar.json"))
            {
                string json = r.ReadToEnd();
                komentari = JsonConvert.DeserializeObject<List<Komentar>>(json);
            }

            List<Komentar> filtriraniKomentari = new List<Komentar>();
            foreach (var k in komentari)
            {
                if (k.IsApproved && k.FitnesCentar.Id == fitnesCentar.Id)
                {
                    filtriraniKomentari.Add(k);
                }
            }


            return View(new FitnesCentarGrupniTreninziKomentari(fitnesCentar, filtriraniGrupniTreninzi, filtriraniKomentari));
        }

        public ActionResult FitnesCentarVlasnik()
        {

            if (!Session["uloga"].Equals(Uloga.VLASNIK.ToString()))
            {
                return View("VlasnikPristup");
            }

            List<FitnesCentar> fitnesCentri;
            using (StreamReader r = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\FitnesCentar.json"))
            {
                string json = r.ReadToEnd();
                fitnesCentri = JsonConvert.DeserializeObject<List<FitnesCentar>>(json);
            }

            List<FitnesCentar> filtriraniFitnesCentri = new List<FitnesCentar>();

            foreach (var fc in fitnesCentri)
            {
                if (fc.Vlasnik.Id == Int32.Parse((string)Session["id"]) && !fc.IsDeleted)
                {
                    filtriraniFitnesCentri.Add(fc);
                }
            }

            return View(filtriraniFitnesCentri);
        }

        public ActionResult IzmeniFitnesCentar(int id)
        {
            if (!Session["uloga"].Equals(Uloga.VLASNIK.ToString()))
            {
                return View("VlasnikPristup");
            }

            List<FitnesCentar> fitnesCentri;
            using (StreamReader r = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\FitnesCentar.json"))
            {
                string json = r.ReadToEnd();
                fitnesCentri = JsonConvert.DeserializeObject<List<FitnesCentar>>(json);
            }

            foreach (var fc in fitnesCentri)
            {
                if (fc.Id == id && !fc.IsDeleted)
                {
                    return View(fc);
                }
            }

            return View();
        }

        [HttpPost]
        public ActionResult IzmeniFitnesCentar(FitnesCentar fitnesCentar)
        {

            if (!Session["uloga"].Equals(Uloga.VLASNIK.ToString()))
            {
                return View("VlasnikPristup");
            }

            List<Korisnik> korisnici;
            using (StreamReader r = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\Korisnik.json"))
            {
                string json = r.ReadToEnd();
                korisnici = JsonConvert.DeserializeObject<List<Korisnik>>(json);
            }

            Korisnik korisnik = new Korisnik();

            foreach (var k in korisnici)
            {
                if (k.Id == Int32.Parse((string)Session["id"]))
                {
                    korisnik = k;
                }
            }

            List<FitnesCentar> fitnesCentri;
            using (StreamReader r = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\FitnesCentar.json"))
            {
                string json = r.ReadToEnd();
                fitnesCentri = JsonConvert.DeserializeObject<List<FitnesCentar>>(json);
            }

            foreach (var fc in fitnesCentri)
            {
                if (fc.Id == fitnesCentar.Id && !fc.IsDeleted)
                {

                    fc.Naziv = fitnesCentar.Naziv;
                    fc.Vlasnik = korisnik;
                    fc.Adresa = new Adresa();
                    fc.Adresa.Ulica = fitnesCentar.Adresa.Ulica;
                    fc.Adresa.Broj = fitnesCentar.Adresa.Broj;
                    fc.Adresa.Mesto = fitnesCentar.Adresa.Mesto;
                    fc.Adresa.PostanskiBroj = fitnesCentar.Adresa.PostanskiBroj;
                    fc.Godina = fitnesCentar.Godina;
                    fc.CenaMesecne = fitnesCentar.CenaMesecne;
                    fc.CenaJednog = fitnesCentar.CenaJednog;
                    fc.CenaGodisnje = fitnesCentar.CenaGodisnje;
                    fc.CenaGrupnog = fitnesCentar.CenaGrupnog;
                    fc.CenaPersonalnog = fitnesCentar.CenaPersonalnog;
                }
            }



            string json2 = JsonConvert.SerializeObject(fitnesCentri);
            System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\FitnesCentar.json", json2);

            return RedirectToAction("FitnesCentarVlasnik", "FitnesCentar");

        }

        public ActionResult ObrisiFitnesCentar(int id)  //SOFT DELETE
        {

            if (!Session["uloga"].Equals(Uloga.VLASNIK.ToString()))
            {
                return View("VlasnikPristup");
            }

            List<Korisnik> korisnici;
            using (StreamReader r = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\Korisnik.json"))
            {
                string json = r.ReadToEnd();
                korisnici = JsonConvert.DeserializeObject<List<Korisnik>>(json);
            }

            Korisnik aktuelniKorisnik = new Korisnik();

            foreach (var k in korisnici)
            {
                if (k.Id == Int32.Parse((string)Session["id"]))
                {
                    aktuelniKorisnik = k;
                }
            }



            List<FitnesCentar> fitnesCentri;
            using (StreamReader r = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\FitnesCentar.json"))
            {
                string json = r.ReadToEnd();
                fitnesCentri = JsonConvert.DeserializeObject<List<FitnesCentar>>(json);
            }

            foreach (var fc in fitnesCentri)
            {
                if (fc.Id == id)
                {

                    List<GrupniTrening> grupniTreninzi;
                    using (StreamReader r = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\GrupniTrening.json"))
                    {
                        string json = r.ReadToEnd();
                        grupniTreninzi = JsonConvert.DeserializeObject<List<GrupniTrening>>(json);
                    }

                    foreach (var gt in grupniTreninzi)
                    {
                        if (fc.Id == gt.FitnesCentar.Id && DateTime.Compare(gt.DatumTreninga, DateTime.Now) > 0)
                        {
                            return View("ZabranjenoBrisanjeFitnesCentraGrupniTrening");
                        }

                    }

                    fc.IsDeleted = true;

                    foreach (var k in korisnici)
                    {
                        if (k.AngazovanFitnesCentar != null && k.AngazovanFitnesCentar.Id == fc.Id)
                        {
                            k.IsBlocked = true;
                        }
                    }
                }
            }

            string json2 = JsonConvert.SerializeObject(fitnesCentri);
            System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\FitnesCentar.json", json2);

            json2 = JsonConvert.SerializeObject(korisnici);
            System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\Korisnik.json", json2);

            return RedirectToAction("FitnesCentarVlasnik", "FitnesCentar");

        }

        public ActionResult KreirajFitnesCentar()
        {

            if (!Session["uloga"].Equals(Uloga.VLASNIK.ToString()))
            {
                return View("VlasnikPristup");
            }

            return View();
        }

        [HttpPost]
        public ActionResult KreirajFitnesCentar(FitnesCentar fitnesCentar)
        {

            if (!Session["uloga"].Equals(Uloga.VLASNIK.ToString()))
            {
                return View("VlasnikPristup");
            }

            List<Korisnik> korisnici;
            using (StreamReader r = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\Korisnik.json"))
            {
                string json = r.ReadToEnd();
                korisnici = JsonConvert.DeserializeObject<List<Korisnik>>(json);
            }

            Korisnik korisnik = new Korisnik();

            foreach (var k in korisnici)
            {
                if (k.Id == Int32.Parse((string)Session["id"]))
                {
                    korisnik = k;
                }
            }

            List<FitnesCentar> fitnesCentri;
            using (StreamReader r = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\FitnesCentar.json"))
            {
                string json = r.ReadToEnd();
                fitnesCentri = JsonConvert.DeserializeObject<List<FitnesCentar>>(json);
            }

            int maxId = 0;
            foreach (var fc2 in fitnesCentri)
            {
                if (fc2.Id > maxId)
                {
                    maxId = fc2.Id;
                }
            }

            FitnesCentar fc = new FitnesCentar();

            fc.Id = maxId + 1;
            fc.Naziv = fitnesCentar.Naziv;
            fc.Vlasnik = korisnik;
            fc.Adresa = new Adresa();
            fc.Adresa.Ulica = fitnesCentar.Adresa.Ulica;
            fc.Adresa.Broj = fitnesCentar.Adresa.Broj;
            fc.Adresa.Mesto = fitnesCentar.Adresa.Mesto;
            fc.Adresa.PostanskiBroj = fitnesCentar.Adresa.PostanskiBroj;
            fc.Godina = fitnesCentar.Godina;
            fc.CenaMesecne = fitnesCentar.CenaMesecne;
            fc.CenaGodisnje = fitnesCentar.CenaGodisnje;
            fc.CenaJednog = fitnesCentar.CenaJednog;
            fc.CenaGrupnog = fitnesCentar.CenaGrupnog;
            fc.CenaPersonalnog = fitnesCentar.CenaPersonalnog;

            fitnesCentri.Add(fc);


            string json2 = JsonConvert.SerializeObject(fitnesCentri);
            System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\FitnesCentar.json", json2);

            return RedirectToAction("FitnesCentarVlasnik", "FitnesCentar");

        }

    }
}
