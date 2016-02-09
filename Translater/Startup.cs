using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Translater.Startup))]
namespace Translater
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
