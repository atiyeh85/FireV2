using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FireV2.Startup))]
namespace FireV2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
