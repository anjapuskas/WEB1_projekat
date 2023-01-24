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
    public class KomentarController : Controller
    {
        // GET: Komentar
        public ActionResult NapraviKomentar(int Id)
        {

            if (!Session["uloga"].Equals(Uloga.POSETILAC.ToString()))
            {
                return View("PosetilacPristup");
            }

            List<FitnesCentar> fitnesCentri;
            using (StreamReader r = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\FitnesCentar.json"))
            {
                string json = r.ReadToEnd();
                fitnesCentri = JsonConvert.DeserializeObject<List<FitnesCentar>>(json);
            }

            FitnesCentar fitnesCentar = new FitnesCentar();

            foreach (var fc in fitnesCentri)
            {
                if (fc.Id == Id)
                {
                    fitnesCentar = fc;
                }
            }

            return View(new FitnesCentarKomentar(fitnesCentar, new Komentar()));
        }
        
        [HttpPost]
        public ActionResult NapraviKomentar(FitnesCentarKomentar fitnesCentarKomentar)
        {

            if (!Session["uloga"].Equals(Uloga.POSETILAC.ToString()))
            {
                return View("PosetilacPristup");
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
                if (fc.Id ==fitnesCentarKomentar.FitnesCentar.Id)
                {
                    pomocniFitnesCentar = fc;
                }
            }

            List<Komentar> komentari;
            using (StreamReader r = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\Komentar.json"))
            {
                string json = r.ReadToEnd();
                komentari = JsonConvert.DeserializeObject<List<Komentar>>(json);
            }

            List<Korisnik> korisnici;
            using (StreamReader r = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\Korisnik.json"))
            {
                string json = r.ReadToEnd();
                korisnici = JsonConvert.DeserializeObject<List<Korisnik>>(json);
            }

            int maxId = 0;

            if (komentari != null && komentari.Count != 0)
            {
                foreach (var k in komentari)
                {
                    if (k.Id > maxId)
                    {
                        maxId = k.Id;
                    }
                }
            }

            Korisnik postavio = new Korisnik();

            foreach (var k in korisnici)
            {
                if (k.Id == Int32.Parse(Session["Id"].ToString()))
                {
                    postavio = k;
                }
            }


            fitnesCentarKomentar.Komentar.Id = maxId + 1;
            fitnesCentarKomentar.Komentar.FitnesCentar = pomocniFitnesCentar;
            fitnesCentarKomentar.Komentar.Komentarisao = postavio;

            komentari.Add(fitnesCentarKomentar.Komentar);

            string json2 = JsonConvert.SerializeObject(komentari);
            System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\Komentar.json", json2);


            return RedirectToAction("StariGrupniTreninzi", "GrupniTrening");
        }

        // GET: Komentar/Details/5
        public ActionResult KomentariTabela()
        {

            if (!Session["uloga"].Equals(Uloga.VLASNIK.ToString()))
            {
                return View("VlasnikPristup");
            }

            List<Komentar> komentari;
            using (StreamReader r = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\Komentar.json"))
            {
                string json = r.ReadToEnd();
                komentari = JsonConvert.DeserializeObject<List<Komentar>>(json);
            }

            List<FitnesCentar> fitnesCentri;
            using (StreamReader r = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\FitnesCentar.json"))
            {
                string json = r.ReadToEnd();
                fitnesCentri = JsonConvert.DeserializeObject<List<FitnesCentar>>(json);
            }

            List<Komentar> filtriraniKomentari = new List<Komentar>();

            foreach (var k in komentari)
            {
                foreach (var fc in fitnesCentri)
                {
                    if (k.FitnesCentar.Id == fc.Id && fc.Vlasnik.Id == Int32.Parse(Session["Id"].ToString()))
                    {
                        filtriraniKomentari.Add(k);
                    }
                }
            }

            return View(komentari);
        }

        public ActionResult OdobriKomentar(int id)
        {

            if (!Session["uloga"].Equals(Uloga.VLASNIK.ToString()))
            {
                return View("VlasnikPristup");
            }

            List<Komentar> komentari;
            using (StreamReader r = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\Komentar.json"))
            {
                string json = r.ReadToEnd();
                komentari = JsonConvert.DeserializeObject<List<Komentar>>(json);
            }

            foreach (var k in komentari)
            {
                if (k.Id == id)
                {
                    k.IsApproved = true;
                }
            }

            string json2 = JsonConvert.SerializeObject(komentari);
            System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\Komentar.json", json2);

            return View("KomentariTabela", komentari);
        }

        public ActionResult BlokirajKomentar(int id)
        {

            if (!Session["uloga"].Equals(Uloga.VLASNIK.ToString()))
            {
                return View("VlasnikPristup");
            }

            List<Komentar> komentari;
            using (StreamReader r = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\Komentar.json"))
            {
                string json = r.ReadToEnd();
                komentari = JsonConvert.DeserializeObject<List<Komentar>>(json);
            }

            foreach (var k in komentari)
            {
                if (k.Id == id)
                {
                    k.IsBlocked = true;
                }
            }

            string json2 = JsonConvert.SerializeObject(komentari);
            System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\Komentar.json", json2);

            return View("KomentariTabela", komentari);
        }
    }
}
