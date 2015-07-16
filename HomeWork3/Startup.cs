using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HomeWork3.Startup))]
namespace HomeWork3
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
