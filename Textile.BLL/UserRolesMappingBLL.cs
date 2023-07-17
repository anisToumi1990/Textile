using Textile.DAL;
using Textile.DAL.Models;
using System.Collections.Generic;
using System.Linq;

namespace Textile.BLL
{
    public class UserRolesMappingBLL : GenericBLL<UserRolesMapping>
    {
        public static void CleanUserRoles(int UserID)
        {
            using (TextileEntities DB = new TextileEntities())
            {
                List<UserRolesMapping> res = DB.UserRolesMapping.Where(e => e.UserID == UserID).ToList();
                DB.UserRolesMapping.RemoveRange(res);
                DB.SaveChanges();
            }
        }
        public static void DeleteRoleMapping(int User, int Role)
        {
            using (TextileEntities DB = new TextileEntities())
            {
                UserRolesMapping mapping = DB.UserRolesMapping.Where(e => e.RoleID == Role && e.UserID == User).FirstOrDefault();
                DB.UserRolesMapping.Remove(mapping);
                DB.SaveChanges();
            }
        }
        public static List<RolesModel> GetRolesByUser(int ID)
        {
            List<RolesModel> _RolesModel = new List<RolesModel>();
            using (TextileEntities DB = new TextileEntities())
            {
                List<UserRolesMapping> res = DB.UserRolesMapping.Where(e => e.UserID == ID).ToList();
                if (res != null)
                {
                    foreach (UserRolesMapping item in res)
                    {
                        RolesModel EX = new RolesModel();
                        EX.ID = item.RoleID;
                        EX.Role = item.Roles.Role;
                        _RolesModel.Add(EX);
                    }
                }
                return _RolesModel;


            }
        }
    }
}
