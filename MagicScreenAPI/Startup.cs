using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MagicScreenAPI.Startup))]
namespace MagicScreenAPI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
