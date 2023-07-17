using System.Linq;
using Textile.DAL;

namespace Textile.Web.Roles
{
    public class RolePermissionManager
    {
        public string[] ResolveRoleName(string loginName)
        {
            var db = new TextileEntities();
            //Retrieve user's information from the database
            Users foundUser = (from user in db.Users
                               where user.UserName == loginName
                               select user).SingleOrDefault();

            if (foundUser == null)
            {
                return new string[0];
            }

            var userRoles = db.UserRolesMapping.Where(e => e.UserID.Equals(foundUser.ID)).Select(e => e.RoleID).ToList();
            if (userRoles.Count() == 0) return new string[0];
            var roles = db.Roles.Where(e => userRoles.Any(u => u.Equals(e.ID))).Select(e => e.Role).ToArray();
            return roles;
        }

    }    
}