using CrystalDecisions.CrystalReports.Engine;
using Textile.DAL;
using Textile.DAL.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Textile.BLL;
using Textile.Infrastructure;

namespace Textile.Web.Controllers
{
    [ CustomAuthorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        [CustomAuthorize(Roles = "Admin")]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult UnAuthorized()
        {
            return View();
        }
        public ActionResult GetImages()
        {
            using (TextileEntities db = new TextileEntities())
            {
                List<ArticleDetails> list = db.ArticleDetails.Where(e=>e.imgURL != null).ToList();
                List<ArticleDetailsModel> ad = GenericModelMapper.GetModelList<ArticleDetailsModel,ArticleDetails>(list);
                return Json(ad, JsonRequestBehavior.AllowGet);
            }            
        }
        public ActionResult Statistics()
        {
            using (TextileEntities db = new TextileEntities())
            {
                var sales = (from vd in db.VenteDetails
                        join v in db.Ventes on vd.vente equals v.id
                        where v.Status == "Valide" && v.DateCreation.Value.Year == DateTime.Now.Year
                             select new VenteDetailsModel
                        {
                            Qte = vd.Qte,
                            PrixUnitaire = vd.PrixUnitaire,
                            DateCreation = vd.DateCreation
                        }
                    ).ToList();


                var salesPerMonth = sales.Where(v=>v.DateCreation != null && v.DateCreation.Value.Month == DateTime.Now.Month).ToList().Sum(e => Convert.ToDouble(e.PrixUnitaire) * e.Qte);
                var salesPerYear = sales.Where(e => e.DateCreation != null && e.DateCreation.Value.Year == DateTime.Now.Year).ToList().Sum(e => Convert.ToDouble(e.PrixUnitaire) * e.Qte);
               
                ViewBag.VenteMois = salesPerMonth != null ? $"{(salesPerMonth):0.000}" : "0,000";
                ViewBag.VenteAnnee = salesPerYear != null ? $"{(double)(salesPerYear):0.000}" : "0,000";
                
                ViewBag.Mois = DateTime.Now.Month;
                ViewBag.MoisPourcent = salesPerMonth != null && salesPerYear != null ? salesPerMonth * 100 / salesPerYear : 0;
            }
            return View();
        }
        public FileResult PrintSalesDetail(int month, int year)
        {
            using (TextileEntities db = new TextileEntities())
            {
                List<VentesParMoisModel> salesPerMonthModel = new List<VentesParMoisModel>();
                List<Ventes> sales = db.Ventes.Where(e => e.DateCreation != null 
                && e.DateCreation.Value.Month == month
                && e.DateCreation.Value.Year == year
                && e.Status.Equals("Valide")).ToList();
                foreach (var item in sales)
                {
                    VentesParMoisModel v = new VentesParMoisModel();
                   var detail = db.VenteDetails.Where(e => e.vente == item.id).ToList();

                    v.Code = item.Code;
                    if (item.DateCreation != null)
                    {
                        v.Mois = item.DateCreation.Value.Month.ToString();
                        v.Montant = detail != null ? Convert.ToDecimal(detail.Sum(e => e.Qte * Convert.ToDouble(e.PrixUnitaire))) : 0;
                        v.Client = db.Client.Find(item.client)?.Nom;
                        v.Annee = item.DateCreation.Value.Year.ToString();
                        v.Date = item.DateCreation != null
                            ? Convert.ToDateTime(item.DateCreation).ToShortDateString()
                            : string.Empty;
                        v.Qte = detail.Sum(e => e.Qte).ToString();
                    }

                    v.Paiement = item.TypePaiement ?? string.Empty;
                    salesPerMonthModel.Add(v);
                }

               
                dynamic dt = (from d in salesPerMonthModel                              
                              select new
                              {
                                  Code = d.Code,
                                  Montant = d.Montant,
                                  Annee = d.Annee,
                                  Mois = d.Mois,
                                  Client = d.Client,
                                  Date = d.Date,
                                  Qte = d.Qte
                              });

                ReportDocument rptH = new ReportDocument();
                string fileName = Server.MapPath("/Reports/VentesParMois.rpt");
                rptH.Load(fileName);
                rptH.SummaryInfo.ReportTitle = "Ventes ";
                rptH.SetDataSource(dt);
                Stream stream = rptH.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                FileStreamResult file = File(stream, "application/pdf");
                return file;
            }
        }
        public ActionResult GetSalePerMonthStats(int year)
        {
            List<string> result = new List<string>();
            try
            {
                using (TextileEntities db = new TextileEntities())
                {
                    List<Ventes> v = db.Ventes.Where(p => p.DateCreation.Value.Year == year  && p.Status == "Valide").ToList();
                    for (int i = 1; i < 13; i++)
                    {
                        var venteMonth = v.Where(e => e.DateCreation != null && e.DateCreation.Value.Month == i).ToList();
                        var detail = from m in venteMonth
                                join d in db.VenteDetails on m.id equals d.vente
                                select new TextValueModel
                                {
                                    Val1 = d.Qte * Convert.ToDouble(d.PrixUnitaire),
                                    Text = ""
                                };

                        var sum = detail.Sum(e => e.Val1).Value;// (int)Math.Truncate(detail.Sum(e => e.Val1).Value);
                        result.Add(sum.ToString().Replace(",","."));
                    }

                }

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetCustomerStatsPerMonth(int year)
        {
            Dictionary<string, double> result = new Dictionary<string, double>();
            try
            {
                using (TextileEntities db = new TextileEntities())
                {
                    List<Ventes> v = db.Ventes
                        .Where(p => p.DateCreation.Value.Year == year  && p.Status == "Valide")
                        .ToList();
                    foreach (var item in v)
                    {
                        var client = db.Client.Find(item.client);
                        var vd = db.VenteDetails.Where(e => e.vente == item.id).ToList();
                        if (client != null && result.ContainsKey(client.Nom))
                        {
                            result[client.Nom] += (double)vd.Sum(e => e.Qte);
                        }
                        else
                        {
                            if (client != null) result.Add(client.Nom, (double)vd.Sum(e => e.Qte));
                        }
                    }
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetSale(int year)
        {
            using (TextileEntities db = new TextileEntities())
            {
                var sale = (from vd in db.VenteDetails
                        join v in db.Ventes on vd.vente equals v.id
                        where v.Status == "Valide"  && v.DateCreation.Value.Year == year
                        select new VenteDetailsModel
                        {
                            Qte = vd.Qte,
                            PrixUnitaire = vd.PrixUnitaire,
                            DateCreation = vd.DateCreation
                        }
                    ).ToList();


                var salePerMonth = sale
                    .Where(v => v.DateCreation != null && v.DateCreation.Value.Month == DateTime.Now.Month &&
                                v.DateCreation.Value.Year == year).ToList()
                    .Sum(e => Convert.ToDouble(e.PrixUnitaire) * e.Qte);
                var salePerYear = sale.Where(e => e.DateCreation != null && e.DateCreation.Value.Year == year).ToList()
                    .Sum(e => Convert.ToDouble(e.PrixUnitaire) * e.Qte);

                var result = new VenteDetailsPerYearModel
                {
                    VenteMois = salePerMonth != null ? $"{(salePerMonth):0.000}".Replace(",",".") : "0.000",
                    VenteAnnee = salePerYear != null ? $"{(double)(salePerYear):0.000}".Replace(",", ".") : "0.000",
                    Mois = DateTime.Now.Month,
                    MoisPourcent = salePerMonth != null && salePerYear != null && salePerYear != 0
                        ? $"{salePerMonth * 100 / salePerYear:0.000}"
                        : "0"

                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        public static IList<SelectListItem> GetYearsList()
        {
            List<SelectListItem> res = new List<SelectListItem>();
            int currentYear = DateTime.Now.Year;
            int fromYear = currentYear - 10;
            int toYear = currentYear;
            for (int i = fromYear; i <= toYear; i++)
            {
                res.Add(new SelectListItem()
                {
                    Text = i.ToString(),
                    Value = i.ToString()
                });               
            }

            SelectListItem first = null;
            foreach (var e in res)
            {
                if (e.Value == toYear.ToString())
                {
                    first = e;
                    break;
                }
            }

            if (first != null) first.Selected = true;
            return res;
        }
        public static IList<SelectListItem> GetMonthList()
        {
            List<SelectListItem> res = new List<SelectListItem>();
            Dictionary<int, string> months = new Dictionary<int, string>() {
                {1,"Janvier" },
                {2,"Février" },
                {3, "Mars" },
                {4,"Avril" },
                {5,"Mai" },
                {6,"Juin" },
                {7,"Juillet" },
                {8,"Août" },
                {9,"Septembre" },
                {10,"Octobre" },
                {11,"Novembre" },
                {12,"Décembre" },
            };
            
            foreach (var item in months)
            {
                res.Add(new SelectListItem()
                {
                    Text = item.Value,
                    Value = item.Key.ToString() 
                });
            }
            var currentMonth = DateTime.Now.Month.ToString();
            res.First(e => e.Value == currentMonth).Selected = true;
            return res;
        }
    }
}