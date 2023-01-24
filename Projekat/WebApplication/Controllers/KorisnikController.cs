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
    public class KorisnikController : Controller
    {
        // GET: Korisnik
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Registracija(Korisnik korisnik)
        {
            if (Session["id"] != null)
            {
                return View("NeulogovanPristup");
            }

            return View();
        }

        public ActionResult Login()
        {

            if (Session["id"] != null)
            {
                return View("NeulogovanPristup");
            }

            return View();
        }

        [HttpPost]
        public ActionResult Login(Korisnik korisnik)
        {

            if (Session["id"] != null)
            {
                return View("NeulogovanPristup");
            }

            List<Korisnik> korisnici;
            using (StreamReader r = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\Korisnik.json"))
            {
                string json = r.ReadToEnd();
                korisnici = JsonConvert.DeserializeObject<List<Korisnik>>(json);
            }

            if (korisnici != null && korisnici.Count != 0)
            {
                foreach (var k in korisnici)
                {
                    if (string.Equals(k.Username, korisnik.Username, StringComparison.OrdinalIgnoreCase))
                    {
                        if (k.Lozinka == korisnik.Lozinka)
                        {

                            if (k.IsBlocked)
                            {
                                return View("KorisnikBlokiranLogovanje");
                            }
                            Session["Id"] = k.Id.ToString();
                            Session["Uloga"] = k.Uloga.ToString();

                            if(k.Uloga == Uloga.POSETILAC)
                            {
                                return RedirectToAction("FitnesCentarTabela", "FitnesCentar");
                            }
                            if (k.Uloga == Uloga.TRENER)
                            {
                                return RedirectToAction("TrenerGrupniTreninzi", "GrupniTrening");
                            }
                            if (k.Uloga == Uloga.VLASNIK)
                            {
                                return RedirectToAction("FitnesCentarVlasnik", "FitnesCentar");
                            }
                        }
                    }
                }
            }


            return View("KorisnikNePostoji");
        }

        [HttpPost]
        public ActionResult RegistrujSe(KreirajKorisnika kk)
        {

            try
            {
                DateTime.ParseExact(kk.DatumRodjenja, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            } catch (Exception e)
            {
                return View("DatumFormat");
            }

            if (Session["id"] != null)
            {
                return View("NeulogovanPristup");
            }

            List<Korisnik> korisnici;
            using (StreamReader r = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\Korisnik.json"))
            {
                string json = r.ReadToEnd();
                korisnici = JsonConvert.DeserializeObject<List<Korisnik>>(json);
            }

            foreach(var k in korisnici)
            {
                if (k.Username == kk.Username)
                    return View("KorisnikUsernameVecPostoji");
            }


            int maxId = 0;

            if (korisnici != null && korisnici.Count != 0)
            {
                foreach(var k in korisnici)
                {
                    if (k.Id > maxId)
                    {
                        maxId = k.Id;
                    }
                }
            }

            Korisnik korisnik = new Korisnik();

            korisnik.Id = maxId + 1;
            korisnik.Username = kk.Username;
            korisnik.Ime = kk.Ime;
            korisnik.Prezime = kk.Prezime;
            korisnik.Lozinka = kk.Lozinka;
            korisnik.Pol = kk.Pol;
            korisnik.Email = kk.Email;
            korisnik.DatumRodjenja = DateTime.ParseExact(kk.DatumRodjenja, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            korisnik.Uloga = Uloga.POSETILAC;
            korisnik.PrijavljenTrening = new List<GrupniTrening>();

            korisnici.Add(korisnik);

            string json2  = JsonConvert.SerializeObject(korisnici);
            System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\Korisnik.json", json2);

            return View("UspesnaRegistracija");
        }

        // GET: Korisnik/Details/5
        public ActionResult Profil()
        {

            if (Session["id"] == null)
            {
                return View("UlogovanPristup");
            }

            List<Korisnik> korisnici;
            using (StreamReader r = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\Korisnik.json"))
            {
                string json = r.ReadToEnd();
                korisnici = JsonConvert.DeserializeObject<List<Korisnik>>(json);
            }

            int id = Int32.Parse(Session["Id"].ToString());
            foreach (var k in korisnici)
            {
                if (k.Id == id)
                {
                    return View(k);
                }
            }

            return View();
        }

        [HttpPost]
        public ActionResult Profil(Korisnik korisnik)
        {

            if (DateTime.Compare(DateTime.ParseExact("01/01/0001", "dd/MM/yyyy", CultureInfo.InvariantCulture), korisnik.DatumRodjenja) == 0)
            {
                return View("DatumFormat");
            }

            if (Session["id"] == null)
            {
                return View("UlogovanPristup");
            }

            List<Korisnik> korisnici;
            using (StreamReader r = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\Korisnik.json"))
            {
                string json = r.ReadToEnd();
                korisnici = JsonConvert.DeserializeObject<List<Korisnik>>(json);
            }

            Korisnik pomocniKorisnik = null;
            int id = Int32.Parse(Session["Id"].ToString());
            foreach (var k in korisnici)
            {
                if (k.Id == id)
                {
                    pomocniKorisnik = k;
                    k.Username = korisnik.Username;
                    k.Ime = korisnik.Ime;
                    k.Prezime = korisnik.Prezime;
                    k.Pol = korisnik.Pol;
                    k.Email = korisnik.Email;
                    k.DatumRodjenja = korisnik.DatumRodjenja;
                }
            }

            string json2 = JsonConvert.SerializeObject(korisnici);
            System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\Korisnik.json", json2);

            return View(pomocniKorisnik);

        }

        public ActionResult PosetiociGrupnogTreninga(int id)
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

            GrupniTrening grupniTrening = new GrupniTrening();

            foreach (var gt in grupniTreninzi)
            {
                if (gt.Id == id)
                {
                    grupniTrening = gt;
                }
            }

            return View(grupniTrening.Posetioci);
            
        }


        public ActionResult KreirajTrenera()
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
                if (fc.Vlasnik.Id == Int32.Parse((string)Session["id"]) && !fc.IsDeleted) {
                    filtriraniFitnesCentri.Add(fc); 
                }
            }

            ViewBag.FitnesCentri = new SelectList(filtriraniFitnesCentri, "Id", "Naziv");

            return View();
        }

        [HttpPost]
        public ActionResult KreirajTrenera(KreiranjeTrenera trener)
        {

            try
            {
                DateTime.ParseExact(trener.DatumRodjenja, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            } catch (Exception e)
            {
                return View("DatumFormat");
            }
            

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

            List<FitnesCentar> fitnesCentri;
            using (StreamReader r = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\FitnesCentar.json"))
            {
                string json = r.ReadToEnd();
                fitnesCentri = JsonConvert.DeserializeObject<List<FitnesCentar>>(json);
            }

            FitnesCentar pomocniFitnesCentar = new FitnesCentar();

            foreach (var fc in fitnesCentri)
            {
                if (fc.Id == trener.FitnesCentarId)
                {
                    pomocniFitnesCentar = fc;
                }
            }

            foreach (var k in korisnici)
            {
                if (k.Username == trener.Username)
                    return View("KorisnikUsernameVecPostoji");
            }


            int maxId = 0;
            if (korisnici != null && korisnici.Count != 0)
            {
                foreach (var k in korisnici)
                {
                    if (k.Id > maxId)
                    {
                        maxId = k.Id;
                    }
                }
            }

            Korisnik korisnik = new Korisnik();

            korisnik.Id = maxId + 1;
            korisnik.Username = trener.Username;
            korisnik.Lozinka = trener.Lozinka;
            korisnik.Ime = trener.Ime;
            korisnik.Prezime = trener.Prezime;
            korisnik.Pol = trener.Pol;
            korisnik.Email = trener.Email;
            korisnik.DatumRodjenja = DateTime.ParseExact(trener.DatumRodjenja, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            korisnik.Uloga = Uloga.TRENER;
            korisnik.AngazovanTrening = new List<GrupniTrening>();
            korisnik.PrijavljenTrening = new List<GrupniTrening>();
            korisnik.AngazovanFitnesCentar = pomocniFitnesCentar;


            korisnici.Add(korisnik);


            string json2 = JsonConvert.SerializeObject(korisnici);
            System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\Korisnik.json", json2);

            return View("UspesnoKreiranTrener");
        }

        public ActionResult TreneriTabela()
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

            List<Korisnik> filtriraniKorisnici = new List<Korisnik>();

            foreach (var k in korisnici)
            {
                if (k.Uloga == Uloga.TRENER && !k.IsDeleted)
                {
                    filtriraniKorisnici.Add(k);
                }
            }

            return View(filtriraniKorisnici);
        }

        public ActionResult BlokirajTrenera(int Id)
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

            foreach (var k in korisnici)
            {
                if (k.Id == Id)
                {
                    if (k.IsBlocked)
                    {
                        return View("KorisnikVecBlokiran");
                    }

                    k.IsBlocked = true;
                }
            }


            string json2 = JsonConvert.SerializeObject(korisnici);
            System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\Korisnik.json", json2);

            return RedirectToAction("TreneriTabela");
        }

        public ActionResult OdblokirajTrenera(int Id)
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

            foreach (var k in korisnici)
            {
                if (k.Id == Id)
                {
                    if (!k.IsBlocked)
                    {
                        return View("KorisnikNijeBlokiran");
                    }

                    k.IsBlocked = false;
                }
            }


            string json2 = JsonConvert.SerializeObject(korisnici);
            System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\Korisnik.json", json2);

            return RedirectToAction("TreneriTabela");
        }

        public ActionResult Logout(Korisnik korisnik)
        {

            if (Session["id"] == null)
            {
                return View("NeulogovanPristup");
            }

            Session["Id"] = null;
            Session["Uloga"] = null;


            return View("UspesanLogout");
        }

        public ActionResult PocetnaStrana()
        {
            if ((string)Session["Uloga"] == Uloga.POSETILAC.ToString())
            {
                return RedirectToAction("FitnesCentarTabela", "FitnesCentar");
            }
            if ((string)Session["Uloga"] == Uloga.TRENER.ToString())
            {
                return RedirectToAction("TrenerGrupniTreninzi", "GrupniTrening");
            }
            if ((string)Session["Uloga"] == Uloga.VLASNIK.ToString())
            {
                return RedirectToAction("FitnesCentarVlasnik", "FitnesCentar");
            }
            return RedirectToAction("Login", "Korisnik");
        }
    }
}
