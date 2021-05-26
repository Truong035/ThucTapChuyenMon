using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PhamTrongTruong_5951071113.Startup))]
namespace PhamTrongTruong_5951071113
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
