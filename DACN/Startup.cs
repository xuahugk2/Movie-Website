using DACN.Hub;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DACN.Startup))]
namespace DACN
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
