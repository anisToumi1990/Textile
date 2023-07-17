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
    [CustomAuthorize(Roles = "admin")]
    public class AdministrationController : Controller
    {

        // GET: Administration
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Users()
        {
            return View();
        }
        #region Customer

        public ActionResult Customer()
        {
            return View();
        }

        public static IList<SelectListItem> GetCustomersList(int Id)
        {
            List<SelectListItem> res = new List<SelectListItem>();
            if (Id == 0)
            {
                res.Add(
            new SelectListItem()
            {
                Text = "-",
                Value = "0"
            });
            }
            else
            {
                var vente = VentesBLL.GetById(Id);
                var customer = ClientBLL.GetById(vente.client);
                if (!customer.Active)
                {
                    res.Add(
                                        new SelectListItem()
                                        {
                                            Text = customer.Nom,
                                            Value = customer.idClient.ToString()
                                        });
                }
            }
            var clients = ClientBLL.GetAll().Where(e => e.Active).ToList();
            foreach (Client customer in clients)
            {
                res.Add(new SelectListItem()
                {
                    Text = customer.Nom,
                    Value = customer.idClient.ToString()
                });
            }
            return res.Distinct().OrderBy(e => e.Text).ToList();
        }

        public static IList<SelectListItem> GetPaymentTypeList()
        {
            List<SelectListItem> res = new List<SelectListItem>
            {
                new SelectListItem()
                {
                    Text = "-",
                    Value = "0"
                }
            };

            foreach (string s in TextValueModel.paiement)
            {
                res.Add(new SelectListItem()
                {
                    Text = s,
                    Value = s.ToString(),
                });
            }
            return res.OrderBy(e => e.Text).ToList();
        }
        public static IList<SelectListItem> GetSaleTypeList()
        {
            List<SelectListItem> res = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = "-",
                    Value = "0"
                }
            };

            foreach (string s in TextValueModel.typeVente)
            {
                res.Add(new SelectListItem
                {
                    Text = s,
                    Value = s.ToString(),
                });
            }
            return res.OrderBy(e => e.Text).ToList();
        }
        public JsonResult GetAllClients()
        {
            List<Client> customersModel = ClientBLL.GetAll();
            List<ClientModel> cl = GenericModelMapper.GetModelList<ClientModel, Client>(customersModel);

            return Json(cl, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddOrUpdateCustomer(ClientModel customerModelToAdd)
        {
            try
            {
                Client c = GenericModelMapper.GetModel<Client, ClientModel>(customerModelToAdd);
                if (c.idClient == 0)
                    ClientBLL.Insert(c);
                else
                    ClientBLL.Update(c);
                return Json(new TextValueModel("OK", "Opération réussite"), JsonRequestBehavior.AllowGet);
            }
            catch (DbException e)
            {
                return Json(new TextValueModel("KO", e.Message), JsonRequestBehavior.AllowGet); ;
            }
        }
        public ActionResult DeleteCustomer(int id)
        {
            ClientBLL.Delete(id);
            return Content("");
        }
        public ActionResult OpenClientsEditor(int id)
        {
            ClientModel customerModel = new ClientModel();
            if (id != 0)
                customerModel = GenericModelMapper.GetModel<ClientModel, Client>(ClientBLL.GetById(id));
            return PartialView("~/Views/Administration/EditorTemplates/_Clients.cshtml", customerModel);
        }

        public ActionResult AddOrUpdateCompany(SocieteModel sm)
        {
            try
            {
                using (TextileEntities db = new TextileEntities())
                {
                    var company = db.Societe.FirstOrDefault();
                    if (company != null)
                    {
                        company.NomSociete = sm.NomSociete;
                        company.Adresse = sm.Adresse;
                        company.Email = sm.Email;
                        company.NumeroTelephone = sm.NumeroTelephone;
                        company.Fax = sm.Fax;
                        db.SaveChanges();
                    }
                    else
                    {
                        Societe s = new Societe
                        {
                            NomSociete = sm.NomSociete,
                            Adresse = sm.Adresse,
                            Email = sm.Email,
                            NumeroTelephone = sm.NumeroTelephone,
                            Fax = sm.Fax
                        };
                        db.Societe.Add(s);
                        db.SaveChanges();
                    }
                }
                return Json(new TextValueModel("OK", "Société ajouté avec succés"), JsonRequestBehavior.AllowGet);
            }
            catch (DbException e)
            {
                return Json(new TextValueModel("KO", e.Message), JsonRequestBehavior.AllowGet); ;
            }
        }
        [HttpPost]
        public JsonResult UploadHomeReport()
        {
            var fileName = string.Empty;
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
                        fileName = Path.GetFileName(fileContent.FileName);
                        var path = Path.Combine(Server.MapPath("~/Images"), fileName);
                        System.IO.File.WriteAllBytes(path, byteFile);
                        using (TextileEntities DB = new TextileEntities())
                        {
                            Societe s = DB.Societe.FirstOrDefault();
                            if (s != null)
                            {
                                s.Logo = fileName;
                            }
                            DB.SaveChanges();
                        }
                    }

                }
            }
            catch (Exception)
            {
                // ignored
            }

            return Json(fileName);
        }
        public ActionResult OpenImageUploader(int id)
        {
            return PartialView("~/Views/Administration/EditorTemplates/_SocieteImage.cshtml", id);
        }

        //OpenImageUploader
        #endregion
        public ActionResult Company()
        {
            using (TextileEntities db = new TextileEntities())
            {
                var company = db.Societe.FirstOrDefault();
                SocieteModel cl = GenericModelMapper.GetModel<SocieteModel, Societe>(company);
                return View(cl);
            }

        }

        public JsonResult GetAllUsers()
        {
            List<Users> users = UsersBLL.GetAll();
            List<UsersModel> userModels = GenericModelMapper.GetModelList<UsersModel, Users>(users);
            return Json(userModels, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAllUserRoles(int userId)
        {
            List<RolesModel> roles = UserRolesMappingBLL.GetRolesByUser(userId);
            //List<UsersModel> userModels = GenericModelMapper.GetModelList<UsersModel, Users>(users);
            return Json(roles, JsonRequestBehavior.AllowGet);
        }

        public ActionResult OpenUserEditor(int id)
        {
            UsersModel usersModel = new UsersModel();
            if (id != 0)
                usersModel = GenericModelMapper.GetModel<UsersModel, Users>(UsersBLL.GetById(id));

            usersModel.AvailableRoles = GenericModelMapper.GetModelList<RolesModel, Textile.DAL.Roles>(RolesBLL.GetAll());
            usersModel.SelectedRoles = UserRolesMappingBLL.GetRolesByUser(id);
            usersModel.AvailableRoles.ForEach(e => e.Checked = usersModel.SelectedRoles.Select(j => j.Role).Contains(e.Role));

            return PartialView("~/Views/Administration/EditorTemplates/_UsersEditor.cshtml", usersModel);
        }
        public ActionResult OpenUserRolesEditor(int ID)
        {
            List<RolesModel> roles = GenericModelMapper.GetModelList<RolesModel, Textile.DAL.Roles>(RolesBLL.GetAll());
            List<RolesModel> userRoles = UserRolesMappingBLL.GetRolesByUser(ID);
            roles.ForEach(e => e.Checked = userRoles.Select(j => j.Role).Contains(e.Role));
            ViewBag.ID = ID;

            return PartialView("~/Views/Administration/EditorTemplates/_UserRolesEditor.cshtml", roles);
        }
        public ActionResult ChangeUserRole(int userId, int roleId, bool isChecked)
        {
            //UserRolesMappingBLL.CleanUserRoles(u.ID);
            if (isChecked)
            {
                UserRolesMapping ex = new UserRolesMapping
                {
                    RoleID = roleId,
                    UserID = userId
                };
                UserRolesMappingBLL.Insert(ex);
            }
            else
            {
                UserRolesMappingBLL.DeleteRoleMapping(userId, roleId);
            }
            return Content("");
        }
        public JsonResult AddOrUpdateUser(UsersModel usersModelToAdd)
        {
            try
            {
                Users u = GenericModelMapper.GetModel<Users, UsersModel>(usersModelToAdd);
                if (u.ID == 0)
                {
                    UsersBLL.Insert(u);
                }
                else
                    UsersBLL.Update(u);
                return Json(new TextValueModel("OK", "Opération réussite"), JsonRequestBehavior.AllowGet);
            }
            catch (DbException e)
            {
                return Json(new TextValueModel("KO", e.Message), JsonRequestBehavior.AllowGet); ;
            }
        }
        public ActionResult DeleteUser(int id)
        {
            UserRolesMappingBLL.CleanUserRoles(id);
            UsersBLL.Delete(id);
            return Content("");
        }
        public static List<RolesModel> GetAllRoles()
        {
            List<RolesModel> res = new List<RolesModel>
            {
                new RolesModel()
                {
                    ID = 0,
                    Role = "0"
                }
            };
            var motifsRoles = RolesBLL.GetAll();
            foreach (Textile.DAL.Roles role in motifsRoles)
            {
                res.Add(new RolesModel()
                {
                    ID = role.ID,
                    Role = role.ID.ToString()
                });
            }
            return res;
        }

        public JsonResult EnableOrDisableCustomer(int id)
        {
            using (TextileEntities db = new TextileEntities())
            {
                var client = db.Client.Find(id);
                if (client != null && client.Active == true)
                {
                    client.Active = false;
                }
                else
                {
                    if (client != null) client.Active = true;
                }
                db.SaveChanges();
            }
            return Json(new TextValueModel("OK", "Opération réussite"), JsonRequestBehavior.AllowGet);

        }

        public JsonResult EnableOrDisableUser(int id)
        {
            using (TextileEntities db = new TextileEntities())
            {
                var user = db.Users.Find(id);
                if (user != null && user.IsActive == true)
                {
                    user.IsActive = false;
                }
                else
                {
                    if (user != null) user.IsActive = true;
                }
                db.SaveChanges();
            }
            return Json(new TextValueModel("OK", "Opération réussite"), JsonRequestBehavior.AllowGet);
        }
        public ActionResult Payment(int id)
        {
            using (TextileEntities db = new TextileEntities())
            {
                var customer = db.Client.Find(id);
                ViewBag.ID = id;
                if (customer != null) ViewBag.Client = customer.Nom;
                return View();
            }

        }
        public JsonResult GetAllPayment(int id)
        {
            List<Paiement> p = PaymentBLL.GetAll().Where(e => e.client == id).ToList();
            List<PaiementModel> paymentModels = GenericModelMapper.GetModelList<PaiementModel, Paiement>(p);
            return Json(paymentModels, JsonRequestBehavior.AllowGet);
        }
        public ActionResult OpenPaymentEditor(int id, int client)
        {
            PaiementModel paymentModel = new PaiementModel();
            if (id != 0)
                paymentModel = GenericModelMapper.GetModel<PaiementModel, Paiement>(PaymentBLL.GetById(id));
            paymentModel.client = client;
            return PartialView("~/Views/Administration/EditorTemplates/_Paiement.cshtml", paymentModel);
        }
        public ActionResult AddOrUpdatePayment(PaiementModel pm)
        {
            try
            {
                Paiement p = GenericModelMapper.GetModel<Paiement, PaiementModel>(pm);
                if (p.DatePaiement == null) p.DatePaiement = DateTime.Now;
                if (p.DateEcheance == null || p.DateEcheance.Equals(new DateTime(0001, 1, 1))) p.DateEcheance = DateTime.Now;
                p.client = pm.client;
                if (p.id == 0)
                    PaymentBLL.Insert(p);
                else
                    PaymentBLL.Update(p);
                return Json(new TextValueModel("OK", "Paiement ajouté avec succés"), JsonRequestBehavior.AllowGet);
            }
            catch (DbException e)
            {
                return Json(new TextValueModel("KO", e.Message), JsonRequestBehavior.AllowGet); ;
            }
        }

        public bool CheckUserName(int id, string userName)
        {
            using (TextileEntities db = new TextileEntities())
            {
                var user = db.Users.Where(e => e.UserName.Equals(userName) && e.ID != id).Count();
                if(user == 0) return true;
                return false;
            }
        }
    }
}