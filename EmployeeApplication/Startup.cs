using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EmployeeApplication.Startup))]
namespace EmployeeApplication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
