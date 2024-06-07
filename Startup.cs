using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FantasyForge_N01543896.Startup))]
namespace FantasyForge_N01543896
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
