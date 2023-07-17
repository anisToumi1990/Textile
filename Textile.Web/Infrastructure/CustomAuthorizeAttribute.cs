
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Textile.Web;

namespace Textile.Infrastructure
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    { // private string v;

        //public CustomAuthorizeAttribute(string v)
        //{
        //    this.v = v;
        //}

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.User.Identity.IsAuthenticated)
            {

                var authCookie = httpContext.Request.Cookies[FormsAuthentication.FormsCookieName];

                string[] roles = null;

                if (authCookie != null)
                {
                    var ticket = FormsAuthentication.Decrypt(authCookie.Value);
                    roles = ticket.UserData.Split('|');
                    var identity = new GenericIdentity(ticket.Name);
                    httpContext.User = new GenericPrincipal(identity, roles);
                }

                if (Roles == string.Empty)
                    return true;

                //Assuming Roles given in the MyAuthorize attribute will only have 1 UserAccountType - if more than one, no errors thrown but will always return false
                else if (SessionParameters.UserHasOneOfRoles(Roles))
                    return true;
                else
                    return false;
            }
            else
                return false;

            //return base.AuthorizeCore(httpContext);
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //if (!Authenticate.IsAuthenticated())
            //    HandleUnauthorizedRequest(filterContext);

            base.OnAuthorization(filterContext);
        }
        //private readonly string[] allowedroles;
        //public CustomAuthorizeAttribute(params string[] roles)
        //{
        //    this.allowedroles = roles;
        //}
        //protected override bool AuthorizeCore(HttpContextBase httpContext)
        //{
        //    bool authorize = false;
        //    string userId = HttpContext.Current.Session["CurrentUserID"] as string;
        //    if (!String.IsNullOrEmpty(userId))

        //        using (BakeryEntities entity = new BakeryEntities())
        //        {

        //            var userRole = (from u in entity.UserRolesMapping
        //                            join r in entity.Roles on u.RoleID equals r.ID
        //                            where u.UserID == int.Parse(userId)
        //                            select new
        //                            {
        //                                r.Role

        //                            }).ToList();
        //            foreach (var role in allowedroles)
        //            {
        //                if (userRole.Select(e=>e.Role).Contains(role)) return true;
        //            }
        //        }
        //    return authorize;
        //}

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
               new RouteValueDictionary
               {
                    { "controller", "Home" },
                    { "action", "UnAuthorized" }
               });
        }
    }
}