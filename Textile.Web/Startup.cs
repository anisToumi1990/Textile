using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Textile.Web.Startup))]
namespace Textile.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
