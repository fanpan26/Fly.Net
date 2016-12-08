using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Fly.UI.Startup))]
namespace Fly.UI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
