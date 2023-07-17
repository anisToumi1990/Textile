using CrystalDecisions.CrystalReports.Engine;
using Textile.DAL;
using Textile.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Textile.BLL;
using Microsoft.Ajax.Utilities;

namespace Textile.Web.Controllers
{
    [Authorize]
    public class VenteController : Controller
    {
        // GET: Vente
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult History()
        {
            return View();
        }
        public JsonResult GetAllVentes()
        {
            using (TextileEntities db = new TextileEntities())
            {
                List<VentesModel> list = (from v in db.Ventes
                                          join c in db.Client on v.client equals c.idClient
                                          select new VentesModel
                                          {
                                              id = v.id,
                                              NomClient = c.Nom,
                                              Code = v.Code,
                                              DateCreation = v.DateCreation,
                                              Type = v.Type != null ? v.Type.ToString() : string.Empty,
                                              TypePaiement = v.TypePaiement != null ? v.TypePaiement : string.Empty,
                                        Status = v.Status,
                                          }).ToList();

                return Json(list, JsonRequestBehavior.AllowGet);
            }   
        }
        public JsonResult GetAllVenteDetails(int Code)
        {
            using (TextileEntities db = new TextileEntities())
            {
                List<VenteDetailsModel> list = (from v in db.VenteDetails
                                                join c in db.Ventes on v.vente equals c.id
                                                join s in db.StockOuvert on v.reference equals s.id
                                                join d in db.Articles on s.reference equals d.id
                                                join r in db.References on d.reference equals r.id
                                                where c.id == Code
                                                select new VenteDetailsModel
                                                {
                                                    //@(string.Format("{0:0.000}", (double)item.PrixVente / 1000)) & nbsp; DT
                                                    id = v.id,
                                                    CodeReference = r.Libellé + "/" + d.CodeArticle,
                                                    DateCreation = v.DateCreation,
                                                    DateModification = v.DateModification,
                                                    Description = v.Description,
                                                    PrixUnitaire = v.PrixUnitaire,
                                                    Remise = v.Remise != null ? v.Remise : 0,
                                                   Qte = v.Qte,
                                                    
                                                }).ToList();

                foreach (var item in list)
                {
                    item._PrixTotal = Convert.ToDecimal(item.Qte * Convert.ToDouble(item.PrixUnitaire));
                }
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetAvailableQte(int ID)   
        {
            using (TextileEntities db = new TextileEntities())
            {
                var stockOuvert = db.StockOuvert.Find(ID);
                var qte = stockOuvert != null ? stockOuvert.Qte : 0;
                return Json(Math.Round((double)qte, 1), JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult VenteDetails(int Code)
        {
            using (TextileEntities db = new TextileEntities())
            {
                Ventes v = db.Ventes.Find(Code);
                Client cl = db.Client.Find(v.client);
                ViewBag.Client = cl.Nom;
                ViewBag.id = Code;
                ViewBag.Facture = v.Code;
                ViewBag.Status = v.Status;
            }
            return View();
        }
        public ActionResult AddOrUpdateVente(VentesModel ventesModel)
        {
            try
            {
                Ventes v = GenericModelMapper.GetModel<Ventes, VentesModel>(ventesModel);
                if (v.id == 0)
                {
                    int max = 0;
                    using (TextileEntities db = new TextileEntities())
                    {
                        var venteC = db.Ventes.Where(e => e.Type == v.Type).Where(e=>e.Code != null).ToList();
                        foreach (var item in venteC)
                        {
                            string[] number = item.Code.Split('-');
                            string num = number[1];
                            int maximum = int.Parse(num);
                            if (max <= maximum)
                            {
                                max = maximum;
                            }
                        }
                        string str = (max + 1).ToString();
                        if(v.Type.Equals("Facture"))
                        {
                            v.Code = "FAC-" + str.PadLeft(4, '0');

                        }
                        else
                        {
                            v.Code = "BLL-" + str.PadLeft(4, '0');

                        }
                        v.Status = "Valide";
                    }

                    v.DateCreation = DateTime.Now; 
                    VentesBLL.Insert(v);
                }
                else
                    VentesBLL.Update(v);
                return Json(new TextValueModel("OK", "ajouté avec succés"), JsonRequestBehavior.AllowGet);
            }
            catch (DbException e)
            {
                return Json(new TextValueModel("KO", e.Message), JsonRequestBehavior.AllowGet); ;
            }
        }

        public ActionResult AddOrUpdateVenteDetail(VenteDetailsModel ventedetailsModel)
        {
            try
            {
                VenteDetails v = GenericModelMapper.GetModel<VenteDetails, VenteDetailsModel>(ventedetailsModel);
                if (v.id == 0)
                {
                    v.DateCreation = DateTime.Now;
                    v.DateModification = DateTime.Now;
                    v.vente = ventedetailsModel.vente;
                    v.reference = ventedetailsModel.reference;
                    v.Qte = ventedetailsModel.Qte;
                    v.Remise = ventedetailsModel.Remise;
                    VenteDetailsBLL.Insert(v);
                    using (TextileEntities db = new TextileEntities())
                    {
                        StockOuvert so = db.StockOuvert.Find(v.reference);
                        so.Qte = so.Qte - v.Qte;
                        StockMouvement sm = new StockMouvement();
                        sm.Mouvement = "Vente-Add";
                        sm.DateCreation = DateTime.Now;
                        sm.Qte = v.Qte;
                        sm.Reference = so.reference;
                        sm.Facture = v.vente;
                        db.StockMouvement.Add(sm);
                        db.SaveChanges();
                    }
                }
                else

                    using (TextileEntities db = new TextileEntities())
                    {
                        VenteDetails vd = db.VenteDetails.Find(v.id);
                  
                            StockOuvert so = db.StockOuvert.Find(v.reference);
                            so.Qte = so.Qte + vd.Qte - v.Qte;
                            db.SaveChanges();
                            StockMouvement sm = new StockMouvement
                            {
                                Mouvement = "Vente-Update",
                                DateCreation = DateTime.Now,
                                Qte = v.Qte,
                                Reference = so.reference,
                                Facture = v.vente
                            };
                            db.StockMouvement.Add(sm);
                            db.SaveChanges();
                        //}
                    }
                VenteDetailsBLL.Update(v);
                return Json(new TextValueModel("OK", "ajouté avec succés"), JsonRequestBehavior.AllowGet);
            }
            catch (DbException e)
            {
                return Json(new TextValueModel("KO", e.Message), JsonRequestBehavior.AllowGet); ;
            }
        }
        public ActionResult OpenVenteEditor(int ID)
        {
            VentesModel venteModel = new VentesModel();
            if (ID != 0)
                venteModel = GenericModelMapper.GetModel<VentesModel, Ventes>(VentesBLL.GetById(ID));

            return PartialView("~/Views/Vente/EditorTemplates/_Vente.cshtml", venteModel);
        }
        public ActionResult OpenVenteDetailsEditor(int ID, int vente)
        {
            VenteDetailsModel venteModel = new VenteDetailsModel();
            if (ID != 0)
            {
                venteModel = GenericModelMapper.GetModel<VenteDetailsModel, VenteDetails>(VenteDetailsBLL.GetById(ID));
                venteModel.Mode = "Edit";
            }
                
            venteModel.vente = vente;
            return PartialView("~/Views/Vente/EditorTemplates/_VenteDetails.cshtml", venteModel);
        }
        public static IList<SelectListItem> GetReferencesAvailableListFromSO(string vente, int id)
        {
            int _v = int.Parse(vente);
            List<SelectListItem> res = new List<SelectListItem>();
            using (TextileEntities db = new TextileEntities())
            {
                if(id == 0)
                {
                    List<string> ExistingRef = new List<string>();

                    ExistingRef = db.VenteDetails.Where(e => e.vente == _v)
                        .Select(elt => elt.reference.ToString()).ToList();

                    //if (id != 0)
                    //{
                    //    VenteDetails vd = db.VenteDetails.Find(id);
                    //    ExistingRef.RemoveAll(e => e == vd.reference.ToString());

                    //}
                    res.Add(new SelectListItem()
                    {
                        Text = "-",
                        Value = "0"
                    });
                    var stock = StockOuvertBLL.GetAll().Where(e => e.Qte > 0);

                    List<StockOuvertModel> list = (from s in stock
                                                   join a in db.Articles on s.reference equals a.id
                                                   join r in db.References on a.reference equals r.id
                                                   select new StockOuvertModel
                                                   {
                                                       CodeReference = r.Libellé + "/" + a.CodeArticle,
                                                       id = s.id
                                                   }).ToList();
                    foreach (StockOuvertModel so in list)
                    {
                        res.Add(new SelectListItem()
                        {
                            Text = so.CodeReference,
                            Value = so.id.ToString()
                        });
                    }
                    res = res.Where(e => !ExistingRef.Contains(e.Value)).ToList();

                }
                else
                {
                    
                    List<StockOuvertModel> list = (from vd in db.VenteDetails
                                                   join s in db.StockOuvert on vd.reference equals s.id
                                                   join a in db.Articles on s.reference equals a.id
                                                   join r in db.References on a.reference equals r.id
                                                   where vd.id == id
                                                   select new StockOuvertModel
                                                   {
                                                       CodeReference = r.Libellé + "/" + a.CodeArticle,
                                                       id = s.id
                                                   }).ToList();
                    foreach (StockOuvertModel so in list)
                    {
                        res.Add(new SelectListItem()
                        {
                            Text = so.CodeReference,
                            Value = so.id.ToString()
                        });
                    }
                }

                return res;
            }
        }

        //DeleteVenteDetail
        public ActionResult DeleteVenteDetail(int ID)
        {
            using (TextileEntities db = new TextileEntities())
            {
                var details = db.VenteDetails.Find(ID);
                var qte = details.Qte;
                var reference = details.reference;
                StockOuvert so = db.StockOuvert.Find(reference);
                so.Qte = so.Qte + qte;
                
                StockMouvement sm = new StockMouvement();
                sm.Qte = qte;
                sm.Mouvement = "Vente-Delete";
                sm.Reference = so.reference;
                sm.DateCreation = DateTime.Now;
                sm.Facture = so.id;
                db.StockMouvement.Add(sm);
                db.SaveChanges();
                VenteDetailsBLL.Delete(ID);

            }
            return Content("");
        }
         public ActionResult AnnulerOuActiverFacture(int ID)
        {
            string message = string.Empty;
            using (TextileEntities db = new TextileEntities())
            {
                var ventes = db.Ventes.Find(ID);
                List<VenteDetails> details = db.VenteDetails.Where(e => e.vente == ID).ToList();
                if(ventes.Status == "Valide")
                {
                    foreach (var item in details)
                    {
                        StockOuvert so = db.StockOuvert.Where(e => e.id == item.reference).FirstOrDefault();
                        so.Qte = so.Qte + item.Qte;
                    }
                    ventes.Status = "Non Valide";
                    db.SaveChanges();
                }
                else
                {
                    bool siffusant = true;
                    foreach (var item in details)
                    {
                        StockOuvert so = db.StockOuvert.Where(e => e.id == item.reference).FirstOrDefault();
                        if(so.Qte>=item.Qte)
                        {
                            so.Qte = so.Qte - item.Qte;
                        }
                        else
                        {
                            siffusant = false;
                            message = "Stock insuffisant, merci de verifier le stock des références de cette facture";

                            break;
                        }
                        
                    }
                    if(siffusant)
                    {
                        db.SaveChanges();
                        ventes.Status = "Valide";
                        db.SaveChanges();
                    }
                }
            }
            return Content(message);
        }
        public JsonResult GetAllVentesByClient(int id)
        {
            using (TextileEntities db = new TextileEntities())
            {
                var list = (from v in db.Ventes
                                          join c in db.Client on v.client equals c.idClient
                                          where v.client == id
                                          select new VentesModel
                                          {
                                              id = v.id,
                                              NomClient = c.Nom,
                                              Code = v.Code,
                                              DateCreation = v.DateCreation,
                                              Type = v.Type != null ? v.Type.ToString() : string.Empty,
                                              TypePaiement = v.TypePaiement != null ? v.TypePaiement : string.Empty,
                                              TotalAmount = (from details in db.VenteDetails
                                                             where details.vente == v.id 
                                                             select new
                                                             {
                                                                 Q = details.Qte,
                                                                 V = details.PrixUnitaire
                                                             }).AsEnumerable().Sum(d => d.Q* (double)d.V).ToString(),
                                              Status = v.Status,
                                          }).ToList();

                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetAmountByCustomer(int id)
        {

            using (TextileEntities db = new TextileEntities())
            {
                Amount a = new Amount();
                List<Ventes> ventes = db.Ventes.Where(e => e.client == id).ToList();
                List<VentesModel> venteModel = GenericModelMapper.GetModelList<VentesModel, Ventes>(ventes);
                List<VenteDetails> ventesDetails = db.VenteDetails.ToList();
                List<VenteDetailsModel> details = GenericModelMapper.GetModelList<VenteDetailsModel, VenteDetails>(ventesDetails);
                List<VenteDetailsModel> vd = (from d in details
                                         join v in venteModel on d.vente equals v.id
                                         select new VenteDetailsModel
                                         {
                                             PrixUnitaire = d.PrixUnitaire,
                                             Qte = d.Qte
                                         }).ToList();
                List<Paiement> p = db.Paiement.Where(e => e.client == id).ToList();
                a.receivedAmount = p.Sum(e => e.Montant)?.ToString("0.000").Replace(",", ".") ?? "0";
                a.payrollAmount = Convert.ToDecimal(vd.Sum(e => (double)(Convert.ToDouble(e.PrixUnitaire) * e.Qte))).ToString("0.000").Replace(",",".") ?? "0"; ;
                return Json(a, JsonRequestBehavior.AllowGet);
            }
        }
      
        public ActionResult DeletePayment(int ID)
        {
            PaymentBLL.Delete(ID);
            return Content("");
        }
        #region Print Facture
        public FileResult PrintFacture(int ID)
        {
            var culture = new System.Globalization.CultureInfo("fr-FR");

            using (TextileEntities db = new TextileEntities())
            {
                //Ventes v = db.Ventes.Find(ID);
                //Client c = db.Client.Find(v.client);
                List<VenteDetails> details = db.VenteDetails.Where(e => e.vente == ID).ToList();
                var pt = details.Sum(e => e.Qte * Convert.ToDouble(e.PrixUnitaire));
                dynamic dt = (from d in details
                              join v in db.Ventes on d.vente equals v.id
                              join c in db.Client on v.client equals c.idClient
                              join so in db.StockOuvert on d.reference equals so.id
                              join a in db.Articles on so.reference equals a.id
                              join r in db.References on a.reference equals r.id
                              where d.vente == ID
                              select new
                              {
                                  id = ID,
                                  Code = v.Code != null ? v.Code : string.Empty,
                                  Client = c.Nom,
                                  TelephoneClient = c.NumeroTelephoe,
                                  Reference = r.Libellé + "/" + a.CodeArticle,
                                  Quantite = d.Qte.ToString(),
                                  Remise = "",
                                  PrixReferenceUnitaire = d.PrixUnitaire.ToString(),
                                  PrixReferenceTotal = string.Format("{0:0.000}", (double)((Convert.ToDouble(d.PrixUnitaire) * d.Qte))) + " DT", //string.Format("{0:0.000}", (double)(d.PrixUnitaire * d.Qte) / 1000) + " DT",
                                  PrixTotal = string.Format("{0:0.000}", (double)(pt)) + " DT",
                                  Total = string.Empty, //string.Format("{0:0.000}", (double)(pt) / 1000) + " DT",
                                  EmailClient = string.Empty,
                                  Logo = string.Empty,

                                  Adresse = c.Adresse,
                              }); 
                
                ReportDocument rptH = new ReportDocument();
                string FileName = Server.MapPath("/Reports/Facture.rpt");
                rptH.Load(FileName);
                rptH.SummaryInfo.ReportTitle = "Facture ";
                rptH.SetDataSource(dt);
              Stream stream = rptH.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
               FileStreamResult _file = File(stream, "application/pdf");
                return _file;
            }
            // return _file;

        }
        #endregion

        public JsonResult GetAllCustomerSale()
        {
            using (TextileEntities db = new TextileEntities())
            {
                List<HistoryCustomerModel> list = (from v in db.VenteDetails
                                                   join a in db.Ventes on v.vente equals a.id
                                                   join s in db.StockOuvert on v.reference equals s.id
                                                   join d in db.Articles on s.reference equals d.id
                                                   join r in db.References on d.reference equals r.id
                                                   join c in db.Client on a.client equals c.idClient
                                                   select new HistoryCustomerModel
                                                   {
                                                       client = c.Nom,
                                                       reference = r.Libellé + "/" + d.CodeArticle,
                                                       quantite = v.Qte.ToString()
                                                   }).ToList();

                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }


    }
}