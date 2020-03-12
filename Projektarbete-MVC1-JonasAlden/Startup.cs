using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Projektarbete_MVC1_JonasAlden.Startup))]
namespace Projektarbete_MVC1_JonasAlden
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
