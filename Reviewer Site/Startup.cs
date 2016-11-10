using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Reviewer_Site.Startup))]
namespace Reviewer_Site
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
