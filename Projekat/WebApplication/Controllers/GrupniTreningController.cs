using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class GrupniTreningController : Controller
    {
        // GET: GrupniTrening
        public ActionResult StariGrupniTreninzi(string sort)
        {

            if (!Session["uloga"].Equals(Uloga.POSETILAC.ToString()))
            {
                return View("PosetilacPristup");
            }

            ViewBag.SortByNaziv = sort == "naziv asc" ? "naziv desc" : "naziv asc";
            ViewBag.SortByTip = sort == "tip asc" ? "tip desc" : "tip asc";
            ViewBag.SortByDatum = sort == "datum asc" ? "datum desc" : "datum asc";
            sort = sort == null ? "naziv asc" : sort;

            List<GrupniTrening> grupniTreninzi;
            using (StreamReader r = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\GrupniTrening.json"))
            {
                string json = r.ReadToEnd();
                grupniTreninzi = JsonConvert.DeserializeObject<List<GrupniTrening>>(json);
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

            List<GrupniTrening> filtriraniGrupniTreninzi = new List<GrupniTrening>();
            foreach (var gt in grupniTreninzi)
            {
                if (DateTime.Compare(DateTime.Now, gt.DatumTreninga) > 0 && !gt.IsDeleted)
                {

                    foreach (var gt2 in korisnik.PrijavljenTrening)
                    {
                        if (gt2.Id == gt.Id)
                        {
                            filtriraniGrupniTreninzi.Add(gt);
                        }
                    }

                    
                }


            }

            return View(filtriraniGrupniTreninzi);
        }

        public ActionResult UcitajStareGrupneTreninge(string pretragaNaziv, string pretragaTip, string pretragaFitnesCentar, string sort)
        {

            if (!Session["uloga"].Equals(Uloga.POSETILAC.ToString()))
            {
                return View("PosetilacPristup");
            }

            ViewBag.SortByNaziv = sort == "naziv asc" ? "naziv desc" : "naziv asc";
            ViewBag.SortByTip = sort == "tip asc" ? "tip desc" : "tip asc";
            ViewBag.SortByDatum = sort == "datum asc" ? "datum desc" : "datum asc";
            sort = sort == null ? "naziv asc" : sort;

            List<GrupniTrening> grupniTreninzi;
            using (StreamReader r = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\GrupniTrening.json"))
            {
                string json = r.ReadToEnd();
                grupniTreninzi = JsonConvert.DeserializeObject<List<GrupniTrening>>(json);
            }

            List<GrupniTrening> filtriraniGrupniTreninzi = new List<GrupniTrening>();
            List<GrupniTrening> pomocniGrupniTreninzi = new List<GrupniTrening>();

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

            foreach (var gt in grupniTreninzi)
            {
                if (DateTime.Compare(DateTime.Now, gt.DatumTreninga) > 0 && !gt.IsDeleted)
                {

                    foreach (var gt2 in korisnik.PrijavljenTrening)
                    {
                        if (gt2.Id == gt.Id)
                        {
                            filtriraniGrupniTreninzi.Add(gt);
                        }
                    }
                }
            }


            if (pretragaNaziv != null && !pretragaNaziv.Equals(""))
            {
                pomocniGrupniTreninzi = filtriraniGrupniTreninzi;
                filtriraniGrupniTreninzi = new List<GrupniTrening>();
                foreach (var gt in pomocniGrupniTreninzi)
                {
                    if (gt.Naziv.IndexOf(pretragaNaziv, 0, StringComparison.CurrentCultureIgnoreCase) >= 0)
                    {
                        filtriraniGrupniTreninzi.Add(gt);
                    }
                }
            }
            if (pretragaTip != null && !pretragaTip.Equals(""))
            {
                pomocniGrupniTreninzi = filtriraniGrupniTreninzi;
                filtriraniGrupniTreninzi = new List<GrupniTrening>();
                foreach (var gt in pomocniGrupniTreninzi)
                {
                    if (gt.Tip.ToString().IndexOf(pretragaTip, 0, StringComparison.CurrentCultureIgnoreCase) >= 0)
                    {
                        filtriraniGrupniTreninzi.Add(gt);
                    }
                }
            }
            if (pretragaFitnesCentar != null && !pretragaFitnesCentar.Equals(""))
            {
                pomocniGrupniTreninzi = filtriraniGrupniTreninzi;
                filtriraniGrupniTreninzi = new List<GrupniTrening>();
                foreach (var gt in pomocniGrupniTreninzi)
                {
                    if (gt.FitnesCentar.Naziv.ToString().IndexOf(pretragaFitnesCentar, 0, StringComparison.CurrentCultureIgnoreCase) >= 0)
                    {
                        filtriraniGrupniTreninzi.Add(gt);
                    }
                }
            }

            if (sort == "naziv asc")
            {
                filtriraniGrupniTreninzi = filtriraniGrupniTreninzi.OrderBy(ffc => ffc.Naziv).ToList();
            }
            if (sort == "naziv desc")
            {
                filtriraniGrupniTreninzi = filtriraniGrupniTreninzi.OrderByDescending(ffc => ffc.Naziv).ToList();
            }
            if (sort == "tip asc")
            {
                filtriraniGrupniTreninzi = filtriraniGrupniTreninzi.OrderBy(ffc => ffc.Tip.ToString()).ToList();
            }
            if (sort == "tip desc")
            {
                filtriraniGrupniTreninzi = filtriraniGrupniTreninzi.OrderByDescending(ffc => ffc.Tip.ToString()).ToList();
            }
            if (sort == "datum asc")
            {
                filtriraniGrupniTreninzi = filtriraniGrupniTreninzi.OrderBy(ffc => ffc.DatumTreninga).ToList();
            }
            if (sort == "datum desc")
            {
                filtriraniGrupniTreninzi = filtriraniGrupniTreninzi.OrderByDescending(ffc => ffc.DatumTreninga).ToList();
            }

            return View("StariGrupniTreninzi", filtriraniGrupniTreninzi);
        }

        public ActionResult GrupniTreninziZakazivanje(int id)
        {

            if (!Session["uloga"].Equals(Uloga.POSETILAC.ToString()))
            {
                return View("PosetilacPristup");
            }

            List<GrupniTrening> grupniTreninzi;
            using (StreamReader r = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\GrupniTrening.json"))
            {
                string json = r.ReadToEnd();
                grupniTreninzi = JsonConvert.DeserializeObject<List<GrupniTrening>>(json);
            }

            List<GrupniTrening> filtriraniGrupniTreninzi = new List<GrupniTrening>();

            foreach (var gt in grupniTreninzi)
            {
                if (DateTime.Compare(gt.DatumTreninga, DateTime.Now) > 0 && !gt.IsDeleted && gt.FitnesCentar.Id == id)
                {
                    filtriraniGrupniTreninzi.Add(gt);
                }
            }


            return View(filtriraniGrupniTreninzi);
        }

        public ActionResult ZakaziTrening(int id)
        {

            if (!Session["uloga"].Equals(Uloga.POSETILAC.ToString()))
            {
                return View("PosetilacPristup");
            }

            List<GrupniTrening> grupniTreninzi;
            using (StreamReader r = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\GrupniTrening.json"))
            {
                string json = r.ReadToEnd();
                grupniTreninzi = JsonConvert.DeserializeObject<List<GrupniTrening>>(json);
            }

            GrupniTrening grupniTrening = new GrupniTrening();

            foreach (var gt in grupniTreninzi)
            {
                if (gt.Id == id && !gt.IsDeleted)
                {
                    grupniTrening = gt;
                }
            }

            if (grupniTrening.Posetioci.Count + 1 > grupniTrening.MaximalanBroj)
            {
                return View("MaxLjudiGrupniTrening");
            }

            foreach (var p in grupniTrening.Posetioci)
            {
                if (p.Id == Int32.Parse((string)Session["id"]))
                {
                    return View("KorisnikVecZakazaoTrening");
                }
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

            GrupniTrening pomocniGrupniTrening = new GrupniTrening(); 

            foreach (var gt in grupniTreninzi)
            {
                if (gt.Id == id)
                {
                    gt.Posetioci.Add(korisnik);
                    pomocniGrupniTrening = gt;

                    string json2 = JsonConvert.SerializeObject(grupniTreninzi);
                    System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\GrupniTrening.json", json2);
                }
            }

            


            pomocniGrupniTrening.Posetioci = new List<Korisnik>();
            korisnik.PrijavljenTrening.Add(pomocniGrupniTrening);

            string json3 = JsonConvert.SerializeObject(korisnici);
            System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\Korisnik.json", json3);

            return RedirectToAction("FitnesCentarTabela", "FitnesCentar");
        }

        public ActionResult TrenerGrupniTreninzi(string sort)
        {

            if (!Session["uloga"].Equals(Uloga.TRENER.ToString()))
            {
                return View("TrenerPristup");
            }

            ViewBag.SortByNaziv = sort == "naziv asc" ? "naziv desc" : "naziv asc";
            ViewBag.SortByTip = sort == "tip asc" ? "tip desc" : "tip asc";
            ViewBag.SortByDatum = sort == "datum asc" ? "datum desc" : "datum asc";

            List<Korisnik> korisnici;
            using (StreamReader r = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\Korisnik.json"))
            {
                string json = r.ReadToEnd();
                korisnici = JsonConvert.DeserializeObject<List<Korisnik>>(json);
            }

            List<GrupniTrening> grupniTreninzi;
            using (StreamReader r = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\GrupniTrening.json"))
            {
                string json = r.ReadToEnd();
                grupniTreninzi = JsonConvert.DeserializeObject<List<GrupniTrening>>(json);
            }

            Korisnik korisnik = new Korisnik();

            foreach (var k in korisnici)
            {
                if (k.Id == Int32.Parse((string)Session["id"]))
                {
                    korisnik = k;
                }
            }

            List<GrupniTrening> prosliGrupniTreninzi = new List<GrupniTrening>();
            List<GrupniTrening> buduciGrupniTreninzi = new List<GrupniTrening>();

            foreach (var gt in korisnik.AngazovanTrening)
            {
                foreach (var gt2 in grupniTreninzi)
                {
                    if (gt2.Id == gt.Id && !gt2.IsDeleted)
                    {
                        if (DateTime.Compare(DateTime.Now, gt.DatumTreninga) > 0)
                        {
                            prosliGrupniTreninzi.Add(gt2);
                        }
                        else
                        {
                            buduciGrupniTreninzi.Add(gt2);
                        }
                    }

                }

            }

            return View(new ProsliBuduciGrupniTreninzi(prosliGrupniTreninzi, buduciGrupniTreninzi));
        }

        public ActionResult UcitajProsleGrupneTreninge(string pretragaNaziv, string pretragaTip, string pretragaDatumOd, string pretragaDatumDo, string sort)
        {

            if (!Session["uloga"].Equals(Uloga.TRENER.ToString()))
            {
                return View("TrenerPristup");
            }

            ViewBag.SortByNaziv = sort == "naziv asc" ? "naziv desc" : "naziv asc";
            ViewBag.SortByTip = sort == "tip asc" ? "tip desc" : "tip asc";
            ViewBag.SortByDatum = sort == "datum asc" ? "datum desc" : "datum asc";

            List<Korisnik> korisnici;
            using (StreamReader r = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\Korisnik.json"))
            {
                string json = r.ReadToEnd();
                korisnici = JsonConvert.DeserializeObject<List<Korisnik>>(json);
            }

            List<GrupniTrening> grupniTreninzi;
            using (StreamReader r = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\GrupniTrening.json"))
            {
                string json = r.ReadToEnd();
                grupniTreninzi = JsonConvert.DeserializeObject<List<GrupniTrening>>(json);
            }

            Korisnik korisnik = new Korisnik();

            foreach (var k in korisnici)
            {
                if (k.Id == Int32.Parse((string)Session["id"]))
                {
                    korisnik = k;
                }
            }

            List<GrupniTrening> prosliGrupniTreninzi = new List<GrupniTrening>();
            List<GrupniTrening> buduciGrupniTreninzi = new List<GrupniTrening>();

            foreach (var gt in korisnik.AngazovanTrening)
            {
                foreach (var gt2 in grupniTreninzi)
                {
                    if (gt2.Id == gt.Id && !gt2.IsDeleted)
                    {
                        if (DateTime.Compare(DateTime.Now, gt.DatumTreninga) > 0)
                        {
                            prosliGrupniTreninzi.Add(gt2);
                        }
                        else
                        {
                            buduciGrupniTreninzi.Add(gt2);
                        }
                    }
                    
                }
                
            }

            List<GrupniTrening> pomocniGrupniTreninzi = new List<GrupniTrening>();

            if (pretragaNaziv != null && !pretragaNaziv.Equals(""))
            {
                pomocniGrupniTreninzi = prosliGrupniTreninzi;
                prosliGrupniTreninzi = new List<GrupniTrening>();
                foreach (var gt in pomocniGrupniTreninzi)
                {
                    if (gt.Naziv.IndexOf(pretragaNaziv, 0, StringComparison.CurrentCultureIgnoreCase) >= 0)
                    {
                        prosliGrupniTreninzi.Add(gt);
                    }
                }
            }
            if (pretragaTip != null && !pretragaTip.Equals(""))
            {
                pomocniGrupniTreninzi = prosliGrupniTreninzi;
                prosliGrupniTreninzi = new List<GrupniTrening>();
                foreach (var gt in pomocniGrupniTreninzi)
                {
                    if (gt.Tip.ToString().IndexOf(pretragaTip, 0, StringComparison.CurrentCultureIgnoreCase) >= 0)
                    {
                        prosliGrupniTreninzi.Add(gt);
                    }
                }
            }
            if (pretragaDatumOd != null && !pretragaDatumOd.Equals(""))
            {
                pomocniGrupniTreninzi = prosliGrupniTreninzi;
                prosliGrupniTreninzi = new List<GrupniTrening>();
                foreach (var gt in pomocniGrupniTreninzi)
                {
                    if (DateTime.Compare(gt.DatumTreninga, DateTime.Parse(pretragaDatumOd)) > 0)
                    {
                        prosliGrupniTreninzi.Add(gt);
                    }
                }
            }

            if (pretragaDatumDo != null && !pretragaDatumDo.Equals(""))
            {
                pomocniGrupniTreninzi = prosliGrupniTreninzi;
                prosliGrupniTreninzi = new List<GrupniTrening>();
                foreach (var gt in pomocniGrupniTreninzi)
                {
                    if (DateTime.Compare(gt.DatumTreninga, DateTime.Parse(pretragaDatumDo)) < 0)
                    {
                        prosliGrupniTreninzi.Add(gt);
                    }
                }
            }

            if (sort == "naziv asc")
            {
                prosliGrupniTreninzi = prosliGrupniTreninzi.OrderBy(ffc => ffc.Naziv).ToList();
            }
            if (sort == "naziv desc")
            {
                prosliGrupniTreninzi = prosliGrupniTreninzi.OrderByDescending(ffc => ffc.Naziv).ToList();
            }
            if (sort == "tip asc")
            {
                prosliGrupniTreninzi = prosliGrupniTreninzi.OrderBy(ffc => ffc.Tip.ToString()).ToList();
            }
            if (sort == "tip desc")
            {
                prosliGrupniTreninzi = prosliGrupniTreninzi.OrderByDescending(ffc => ffc.Tip.ToString()).ToList();
            }
            if (sort == "datum asc")
            {
                prosliGrupniTreninzi = prosliGrupniTreninzi.OrderBy(ffc => ffc.DatumTreninga).ToList();
            }
            if (sort == "datum desc")
            {
                prosliGrupniTreninzi = prosliGrupniTreninzi.OrderByDescending(ffc => ffc.DatumTreninga).ToList();
            }

            return View("TrenerGrupniTreninzi", new ProsliBuduciGrupniTreninzi(prosliGrupniTreninzi, buduciGrupniTreninzi));
        }

        public ActionResult IzmeniGrupniTrening(int id)
        {

            if (!Session["uloga"].Equals(Uloga.TRENER.ToString()))
            {
                return View("TrenerPristup");
            }
            List<GrupniTrening> grupniTreninzi;
            using (StreamReader r = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\GrupniTrening.json"))
            {
                string json = r.ReadToEnd();
                grupniTreninzi = JsonConvert.DeserializeObject<List<GrupniTrening>>(json);
            }

            foreach (var gt in grupniTreninzi)
            {
                if (gt.Id == id && !gt.IsDeleted)
                {
                    return View(gt);
                }
            }

            return View();
        }

        [HttpPost]
        public ActionResult IzmeniGrupniTrening(GrupniTrening grupniTrening)
        {

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

            List<GrupniTrening> grupniTreninzi;
            using (StreamReader r = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\GrupniTrening.json"))
            {
                string json = r.ReadToEnd();
                grupniTreninzi = JsonConvert.DeserializeObject<List<GrupniTrening>>(json);
            }

            GrupniTrening pomocniGrupniTrening = new GrupniTrening();

            foreach (var gt in grupniTreninzi)
            {
                if (gt.Id == grupniTrening.Id && !gt.IsDeleted)
                {

                    pomocniGrupniTrening = gt;
                    gt.Naziv = grupniTrening.Naziv;
                    gt.Tip = grupniTrening.Tip;
                    gt.TrajanjeTreninga = grupniTrening.TrajanjeTreninga;
                    gt.DatumTreninga = grupniTrening.DatumTreninga;
                    gt.MaximalanBroj = grupniTrening.MaximalanBroj;
                }
            }

            if (korisnik.AngazovanFitnesCentar.Id != pomocniGrupniTrening.FitnesCentar.Id)
            {
                return View("TrenerNePripadaFitnesCentru");
            }

            if (DateTime.Compare(pomocniGrupniTrening.DatumTreninga, DateTime.Now) < 0)
            {
                return View("GrupniTreningPreVremena");
            }


            bool pripada = false;

            foreach (var gt in korisnik.AngazovanTrening)
            {
                if (gt.Id == pomocniGrupniTrening.Id)
                {
                    pripada = true;
                    gt.Naziv = grupniTrening.Naziv;
                    gt.Tip = grupniTrening.Tip;
                    gt.TrajanjeTreninga = grupniTrening.TrajanjeTreninga;
                    gt.DatumTreninga = grupniTrening.DatumTreninga;
                    gt.MaximalanBroj = grupniTrening.MaximalanBroj;
                }
            }

            if (!pripada)
            {
                return View("TrenerNeOdrzavaGrupniTrening");
            }


            string json2 = JsonConvert.SerializeObject(grupniTreninzi);
            System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\GrupniTrening.json", json2);

            json2 = JsonConvert.SerializeObject(korisnici);
            System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\Korisnik.json", json2);

            return RedirectToAction("TrenerGrupniTreninzi", "GrupniTrening", new { sort = "" });

        }

        public ActionResult KreirajGrupniTrening()
        {

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

            ViewBag.FitnesCentri = new SelectList(filtriraniFitnesCentri, "Id", "Naziv");

            return View();
        }

        [HttpPost]
        public ActionResult KreirajGrupniTrening(KreiranjeGrupnogTreninga kgt)
        {

            try
            {
                DateTime.ParseExact(kgt.DatumTreninga, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            }
            catch (Exception e)
            {
                return View("DatumVremeFormat");
            }

            if (!Session["uloga"].Equals(Uloga.TRENER.ToString()))
            {
                return View("TrenerPristup");
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

            FitnesCentar pomocniFitnesCentar = new FitnesCentar();

            foreach (var fc in fitnesCentri)
            {
                if (fc.Id == kgt.FitnesCentarId)
                {
                    pomocniFitnesCentar = fc;
                }
            }

            List<GrupniTrening> grupniTreninzi;
            using (StreamReader r = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\GrupniTrening.json"))
            {
                string json = r.ReadToEnd();
                grupniTreninzi = JsonConvert.DeserializeObject<List<GrupniTrening>>(json);
            }



            if (korisnik.AngazovanFitnesCentar.Id != pomocniFitnesCentar.Id)
            {
                return View("TrenerNePripadaFitnesCentru");
            }

            DateTime currentDateTime = DateTime.Now.AddDays(3.0);

            if (DateTime.Compare(DateTime.ParseExact(kgt.DatumTreninga, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture), DateTime.Now) < 0)
            {
                return View("GrupniTreningPreVremena");
            }

            if (DateTime.Compare(DateTime.ParseExact(kgt.DatumTreninga, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture),  currentDateTime) < 0)
            {
                return View("GrupniTreningPreVremenaTriDana");
            }

            int maxId = 0;
            if (grupniTreninzi != null && grupniTreninzi.Count != 0)
            {
                foreach (var gt in grupniTreninzi)
                {
                    if (gt.Id > maxId)
                    {
                        maxId = gt.Id;
                    }
                }
            }

                GrupniTrening grupniTrening = new GrupniTrening();

            grupniTrening.Id = maxId + 1;
            grupniTrening.Naziv = kgt.Naziv;
            grupniTrening.Tip = kgt.Tip;
            grupniTrening.FitnesCentar = pomocniFitnesCentar;
            grupniTrening.TrajanjeTreninga = kgt.TrajanjeTreninga;
            grupniTrening.DatumTreninga = DateTime.ParseExact(kgt.DatumTreninga, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            grupniTrening.MaximalanBroj = kgt.MaximalanBroj;
            grupniTrening.Posetioci = new List<Korisnik>();


            grupniTreninzi.Add(grupniTrening);


            string json2 = JsonConvert.SerializeObject(grupniTreninzi);
            System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\GrupniTrening.json", json2);


            foreach (var k in korisnici)
            {
                if (k.Id == Int32.Parse((string)Session["id"]))
                {
                    k.AngazovanTrening.Add(grupniTrening);
                }
            }

            json2 = JsonConvert.SerializeObject(korisnici);
            System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\Korisnik.json", json2);

            return RedirectToAction("TrenerGrupniTreninzi", "GrupniTrening", new { sort = "" });

        }

        public ActionResult ObrisiGrupniTrening(int id)  //SOFT DELETE
        {

            if (!Session["uloga"].Equals(Uloga.TRENER.ToString()))
            {
                return View("TrenerPristup");
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



            List<GrupniTrening> grupniTreninzi;
            using (StreamReader r = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\GrupniTrening.json"))
            {
                string json = r.ReadToEnd();
                grupniTreninzi = JsonConvert.DeserializeObject<List<GrupniTrening>>(json);
            }

            foreach (var gt in grupniTreninzi)
            {
                if (gt.Id == id)
                {
                    if(gt.Posetioci.Count > 0)
                    {
                        return View("ZabranjenoBrisanjePosecenogGrupnogTreninga");
                    }

                    if (gt.FitnesCentar.Id != aktuelniKorisnik.AngazovanFitnesCentar.Id)
                    {
                        return View("TrenerNePripadaFitnesCentru");
                    }

                    bool pripada = false;

                    foreach (var gt2 in aktuelniKorisnik.AngazovanTrening)
                    {
                        if (gt2.Id == gt.Id)
                        {
                            pripada = true;
                        }
                    }

                    if (!pripada)
                    {
                        return View("TrenerNeOdrzavaGrupniTrening");
                    }
        
                    gt.IsDeleted = true;
                }
            }

            string json2 = JsonConvert.SerializeObject(grupniTreninzi);
            System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\GrupniTrening.json", json2);

            Korisnik korisnik = new Korisnik();

            foreach (var k in korisnici)
            {
                if (k.AngazovanTrening != null)
                {
                    foreach (var at in k.AngazovanTrening)
                    {
                        if (at.Id == id)
                        {
                            at.IsDeleted = true;
                        }
                    }
                }
            }

            json2 = JsonConvert.SerializeObject(korisnici);
            System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\Korisnik.json", json2);

            return RedirectToAction("TrenerGrupniTreninzi", "GrupniTrening", new { sort = "" });
        }
    }
}
