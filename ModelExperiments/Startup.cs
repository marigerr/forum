using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ModelExperiments.Startup))]
namespace ModelExperiments
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
