using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CoachSeek.WebUI.Startup))]
namespace CoachSeek.WebUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
