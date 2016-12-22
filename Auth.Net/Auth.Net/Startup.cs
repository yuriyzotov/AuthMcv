using Auth.Net.App_Start;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Auth.Net.Startup))]
namespace Auth.Net
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            // Any connection or hub wire up and configuration should go here
            app.MapSignalR();
            //add asp .net session
            app.RequireAspNetSession();
            
            
        }
    }
}
