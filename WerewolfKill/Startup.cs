using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WerewolfKill.Startup))]
namespace WerewolfKill
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
