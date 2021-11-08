using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Commu_balance.Startup))]
namespace Commu_balance
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
