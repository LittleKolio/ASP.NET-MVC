using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DemoBlog.Startup))]
namespace DemoBlog
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
