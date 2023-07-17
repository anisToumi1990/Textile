using Textile.DAL;
using Textile.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Textile.BLL
{
    public class UsersBLL
        : GenericBLL<Users>
    {
        public static TextValueModel GetByUserName(string userName, string password)
        {
            var _password = Convert.ToBase64String(Encoding.UTF8.GetBytes(password));
             using (TextileEntities DB = new TextileEntities())
            {
                Users res = DB.Users.Where(e => e.UserName == userName && (e.Password == _password
                || e.Password == password)).FirstOrDefault();
                if (res == null)
                {
                    return (new TextValueModel("", "", bool1: false, bool2: false) { });
                }
                else
                {
                    return (new TextValueModel("", "", bool1: res != null && res.ID != 0, bool2: res.IsActive) { });
                }

            }
        }
        public static Users GetResultByUserName(string UserName)
        {
            using (TextileEntities DB = new TextileEntities())
            {
                Users res = DB.Users.Where(e => e.UserName == UserName).FirstOrDefault();
                return res;
            }
        }

        public static List<string> GetAdministratorUsers()
        {
            IEnumerable<int> AdministratorsRole = UserRolesMappingBLL.GetAll().Where(elt => elt.Roles.Role == "admin").ToList().Select(elt => elt.UserID);
            List<string> AdministratorUsers = UsersBLL.GetAll().Where(elt => AdministratorsRole.Contains(elt.ID))
                .ToList().Select(elt => elt.UserName).ToList();
            return AdministratorUsers;
        }

    }
}
