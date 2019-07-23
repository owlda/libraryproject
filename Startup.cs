using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LibraryProject.Startup))]
namespace LibraryProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
