using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Fly.Admin.Startup))]
namespace Fly.Admin
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
