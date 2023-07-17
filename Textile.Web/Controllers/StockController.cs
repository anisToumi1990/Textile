using Textile.DAL;
using Textile.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Textile.BLL;
using Textile.Infrastructure;

namespace Textile.Web.Controllers
{
    [Authorize]
    public class StockController : Controller
    {
        // GET: Stock
        public ActionResult Index()
        {
            return View();
        }

        #region OpenStock
        public JsonResult GetAllOpenStock()
        {
            using (TextileEntities db = new TextileEntities())
            {
                List<StockModel> soList = (from so in db.StockOuvert
                                           join article in db.Articles on so.reference equals article.id
                                           join r in db.References on article.reference equals r.id
                                           select new StockModel
                                           {
                                               id = so.id,
                                               reference = r.Libellé + "/" + article.CodeArticle,
                                               qte = Math.Round((double)so.Qte,1),
                                              
                                           }).ToList();
                return Json(soList, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Mouvement(int code)
        {
            using (TextileEntities db = new TextileEntities())
            {
                StockOuvert openedStock = db.StockOuvert.Find(code);
                if (openedStock != null)
                {
                    Articles article = db.Articles.Find(openedStock.reference);
                    if (article != null)
                    {
                        References reference = db.References.Find(article.reference);
                        if (reference != null) ViewBag.Reference = reference.Libellé + "/" + article.CodeArticle;
                    }
                }

                ViewBag.id = code;
            }
            return View();
        }
        public JsonResult GetAllMouvement(int code)
        {
            if (code <= 0) throw new ArgumentOutOfRangeException(nameof(code));
            using (TextileEntities db = new TextileEntities())
            {
                StockOuvert so = db.StockOuvert.Find(code);
                List<StockMouvement> list = db.StockMouvement.Where(e => e.Reference == so.reference).ToList();
               
                return Json(StockMouvementBLL.GetStockMouvement(list), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Dechet(int id, string Qte)
        {
            using (TextileEntities db = new TextileEntities())
            {
                StockOuvert so = db.StockOuvert.Find(id);
                if (so != null)
                {
                    var qte = so.Qte;
                    StockMouvement sm = new StockMouvement
                    {
                        Qte = Convert.ToDouble(Qte.Replace('.',',')),
                        Reference = so.reference,
                        Mouvement = "Dechet",
                        DateCreation = DateTime.Now
                    };
                    db.StockMouvement.Add(sm);
                    so.Qte = qte - Convert.ToDouble(Qte.Replace('.', ','));
                }

                db.SaveChanges();
            }
            return Json(new TextValueModel("OK", ""), JsonRequestBehavior.AllowGet);
        }
        public ActionResult AnnulerDechet(int id)
        {
            using (TextileEntities db = new TextileEntities())
            {
                StockMouvement mv = db.StockMouvement.Find(id);
                StockOuvert so = db.StockOuvert.FirstOrDefault(e => e.reference == mv.Reference);
                so.Qte = so.Qte + mv.Qte;
                db.SaveChanges();
            }
            StockMouvementBLL.Delete(id);
            return Json(new TextValueModel("OK", ""), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetQteToDechet(int id)
        {
            using (TextileEntities db = new TextileEntities())
            {
                StockOuvert so = db.StockOuvert.Find(id);
                if (so != null)
                    if (so.Qte != null)
                        return Json(Math.Round((double)so.Qte, 1), JsonRequestBehavior.AllowGet);
            }

            return null;
        }
        #endregion
        #region Reference
        public ActionResult References()
        {
            return View();
        }
        public static IList<SelectListItem> GetReferencesList()
        {
            List<SelectListItem> res = new List<SelectListItem>
            {
                new SelectListItem()
                {
                    Text = "-",
                    Value = "0"
                }
            };
            var references = ReferencesBLL.GetAll();
            foreach (References reference in references)
            {
                res.Add(new SelectListItem()
                {
                    Text = reference.Libellé,
                    Value = reference.id.ToString()
                });
            }
            return res.OrderBy(e => e.Text).ToList();
        }

        public JsonResult GetAllReferences()
        {
            List<References> refModels = ReferencesBLL.GetAll();
            List<ReferencesModel> cl = GenericModelMapper.GetModelList<ReferencesModel, References>(refModels);

            return Json(cl, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddOrUpdateReference(ReferencesModel referenceModelToAdd)
        {
            try
            {
                References c = GenericModelMapper.GetModel<References, ReferencesModel>(referenceModelToAdd);
                if (c.id == 0)
                {
                    c.DateCreation = DateTime.Now;
                    c.DateModification = DateTime.Now;
                    ReferencesBLL.Insert(c);
                }

                else
                    c.DateModification = DateTime.Now;
                ReferencesBLL.Update(c);
                return Json(new TextValueModel("OK", "Référence ajoutée avec succès"), JsonRequestBehavior.AllowGet);
            }
            catch (DbException e)
            {
                return Json(new TextValueModel("KO", e.Message), JsonRequestBehavior.AllowGet); ;
            }
        }
        public ActionResult DeleteReference(int id)
        {
            var articles = ArticlesBLL.GetAll().Where(e => e.reference == id);

            if (articles.Count() == 0) ReferencesBLL.Delete(id);
            else return Content("Cette référence ne peut pas être supprimée");
            return Content("");
        }
        public ActionResult OpenReferenceEditor(int id)
        {
            ReferencesModel referenceModel = new ReferencesModel();
            if (id != 0)
                referenceModel = GenericModelMapper.GetModel<ReferencesModel, References>(ReferencesBLL.GetById(id));
            //else
            //{
            //    int LastOrder = ClientBLL.GetLastOerder();
            //    ClientModel.ClientOrder = LastOrder + 1;
            //}
            return PartialView("~/Views/Stock/EditorTemplates/_Reference.cshtml", referenceModel);
        }
        #endregion
        #region Articles
        public ActionResult Article(int code)
        {
            using (TextileEntities db = new TextileEntities())
            {
                References reference = db.References.Find(code);
                if (reference != null) ViewBag.Reference = reference.Libellé;
                ViewBag.id = code;
            }
            return View();
        }
        public static IList<SelectListItem> GetArticleList(int reference)
        {
            List<SelectListItem> res = new List<SelectListItem>
            {
                new SelectListItem()
                {
                    Text = "-",
                    Value = "0"
                }
            };
            var references = ArticlesBLL.GetArticles(reference);
            foreach (Articles detail in references)
            {
                res.Add(new SelectListItem()
                {
                    Text = detail.CodeArticle,
                    Value = detail.id.ToString()
                });
            }
            return res.OrderBy(e => e.Text).ToList();
        }
        public static IList<SelectListItem> GetAvailableArticleList()
        {
            List<SelectListItem> res = new List<SelectListItem>
            {
                new SelectListItem()
                {
                    Text = "-",
                    Value = "0"
                }
            };
            var stock = StockOuvertBLL.GetAll().Where(e=>e.Qte > 0);
            using (TextileEntities db = new TextileEntities())
            {
                List<StockOuvertModel> list = (from s in stock
                                               join a in db.Articles on s.reference equals a.id
                                               join r in db.References on a.reference equals r.id
                                               select new StockOuvertModel
                                               {
                                                 CodeReference = r.Libellé  + "/" + a.CodeArticle,
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
            return res.OrderBy(e => e.Text).ToList();
        }
        public JsonResult GetAllArticle(int code)
        {
            List<Articles> refModels = ArticlesBLL.GetArticles(code);
            List<ArticlesModel> cl = GenericModelMapper.GetModelList<ArticlesModel, Articles>(refModels);
            foreach (var item in cl)
            {
                using (TextileEntities db = new TextileEntities())
                {
                    item.Quantite = db.ArticleDetails.Where(e => e.article == item.id && e.Disponible == true).Sum(e => e.Qte);
                }
            }
            return Json(cl, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddOrUpdateArticle(ArticlesModel articleModelToAdd)
        {
            try
            {
                
                Articles c = GenericModelMapper.GetModel<Articles, ArticlesModel>(articleModelToAdd);
                if (c.id == 0)
                {
                    c.DateCreation = DateTime.Now;
                    c.DateModification = DateTime.Now;
                    ArticlesBLL.Insert(c);
                }

                else
                    c.DateModification = DateTime.Now;
                ArticlesBLL.Update(c);
                return Json(new TextValueModel("OK", "Article ajouté avec succès"), JsonRequestBehavior.AllowGet);
            }
            catch (DbException e)
            {
                return Json(new TextValueModel("KO", e.Message), JsonRequestBehavior.AllowGet); ;
            }
        }
        public ActionResult DeleteArticle(int id)
        {
            var details = ArticleDetailsBLL.GetAll().Where(e => e.article == id);
            if (details.Count() == 0) ArticlesBLL.Delete(id);
            else return Content("Cet article ne peut pas être supprimé");
            return Content("");
        }
        public ActionResult OpenArticleEditor(int id, int reference)
        {
            ArticlesModel referenceModel = new ArticlesModel();
            if (id != 0)
                referenceModel = GenericModelMapper.GetModel<ArticlesModel, Articles>(ArticlesBLL.GetById(id));
           
            referenceModel.reference = reference;
            return PartialView("~/Views/Stock/EditorTemplates/_Article.cshtml", referenceModel);
        }
        #endregion
        #region ArticleDetails
        public ActionResult ArticleDetails(int code)
        {
            using (TextileEntities db = new TextileEntities())
            {
                Articles article = db.Articles.Find(code);
                if (article != null)
                {
                    References references = db.References.Find(article.reference);
                    if (references != null)
                    {
                        ViewBag.article = references.id;
                        ViewBag.CodeArticle = references.Libellé + "/" + article.CodeArticle;
                    }
                }

                ViewBag.id = code;
            }
            return View();
        }
        public ActionResult OpenArticleDetailEditor(int ID, int article)
        {
            ArticleDetailsModel articleDetailModel = new ArticleDetailsModel();
            if (ID != 0)
                articleDetailModel = GenericModelMapper.GetModel<ArticleDetailsModel, ArticleDetails>(ArticleDetailsBLL.GetById(ID));
            articleDetailModel.article = article;
            return PartialView("~/Views/Stock/EditorTemplates/_ArticleDetails.cshtml", articleDetailModel);
        }

        public JsonResult GetAllArticleDetails(int code)
        {
            List<ArticleDetails> refModels = ArticleDetailsBLL.GetArticleDetail(code);
            List<ArticleDetailsModel> cl = GenericModelMapper.GetModelList<ArticleDetailsModel, ArticleDetails>(refModels);
            var res = cl.OrderBy(e => e.Disponible).ThenByDescending(e=>e.DateModification).ToList();
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddOrUpdateArticleDetail(ArticleDetailsModel articleModelToAdd)
        {
            try
            {

                ArticleDetails c = GenericModelMapper.GetModel<ArticleDetails, ArticleDetailsModel>(articleModelToAdd);
                if (c.id == 0)
                {
                    c.DateCreation = DateTime.Now;
                    c.DateModification = DateTime.Now;
                    c.Disponible = true;
                    ArticleDetailsBLL.Insert(c);
                }
                else
                    c.DateModification = DateTime.Now;
                ArticleDetailsBLL.Update(c);
                return Json(new TextValueModel("OK", "Détail ajouté avec succés"), JsonRequestBehavior.AllowGet);
            }
            catch (DbException e)
            {
                return Json(new TextValueModel("KO", e.Message), JsonRequestBehavior.AllowGet); ;
            }
        }

        public ActionResult OpenImageUploader(int id)
        {
            return PartialView("~/Views/Stock/EditorTemplates/_ImgUploader.cshtml", id);
        }
        [HttpPost]
        public JsonResult UploadHomeReport(string id)
        {
            int identifier = int.Parse(id);
            try
            {
                foreach (string file in Request.Files)
                {
                    var fileContent = Request.Files[file];
                    if (fileContent != null && fileContent.ContentLength > 0)
                    {
                        // get a stream
                        byte[] buffer = new byte[16 * 1024];
                        byte[] byteFile;
                        var stream = fileContent.InputStream;
                        using (MemoryStream ms = new MemoryStream())
                        {
                            int read;
                            while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                ms.Write(buffer, 0, read);
                            }
                            byteFile = ms.ToArray();
                        }
                        // and optionally write the file to disk
                        var fileName = Path.GetFileName(fileContent.FileName);
                        var path = Path.Combine(Server.MapPath("~/Images"), fileName);
                        System.IO.File.WriteAllBytes(path, byteFile);
                        using (TextileEntities db = new TextileEntities())
                        {
                            ArticleDetails detail = db.ArticleDetails.FirstOrDefault(elt => elt.id == identifier);
                            if (detail != null) detail.imgURL = fileName;
                            db.SaveChanges();
                        }
                    }

                }
            }
            catch (Exception)
            {
                // ignored
            }

            return Json("File uploaded successfully");
        }
        public ActionResult DeleteArticleDetail(int ID)
        {
            var so = StockOuvertBLL.GetAll().Where(e => e.reference == ID);
            if (so.Count() == 0) ArticleDetailsBLL.Delete(ID);
            else return Content("Cet article ne peut pas être supprimé");
           
            return Content("");
        }
        public ActionResult DeplacerVersStockOuvert(int id)
        {
            try
            {
                using (TextileEntities db = new TextileEntities())
                {
                    
                    ArticleDetails aricleDetails = db.ArticleDetails.Find(id);

                    StockOuvert so = db.StockOuvert.FirstOrDefault(e => e.reference == aricleDetails.article);
                    if(so != null)
                    {
                        if (aricleDetails != null) so.Qte += aricleDetails.Qte;
                    }
                    else
                    {
                        if (aricleDetails != null)
                            so = new StockOuvert
                            {
                                Qte = aricleDetails.Qte,
                                reference = aricleDetails.article
                            };
                        if (so != null) db.StockOuvert.Add(so);
                    }

                    if (aricleDetails != null)
                    {
                        StockMouvement sm = new StockMouvement
                        {
                            DateCreation = DateTime.Now,
                            Qte = aricleDetails.Qte,
                            Reference = aricleDetails.article,
                            Mouvement = "Stock Fermé => Stock Ouvert"
                        };
                        db.StockMouvement.Add(sm);
                    }

                    if (aricleDetails != null) aricleDetails.Disponible = false;
                    db.SaveChanges();
                }
                return Json(new TextValueModel("OK", ""), JsonRequestBehavior.AllowGet);
            }
            catch (DbException e)
            {
                return Json(new TextValueModel("KO", e.Message), JsonRequestBehavior.AllowGet); ;
            }
        }
        public JsonResult GetQte(int article)
        {
            using (TextileEntities db = new TextileEntities())
            {
                QuantiteModel ex = new QuantiteModel();
                var qte = db.ArticleDetails.Where(e => e.article == article && e.Disponible == true).ToList();
                ex.Qte = qte.Sum(e => e.Qte);
                ex.Rouloux = qte.Count();
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}