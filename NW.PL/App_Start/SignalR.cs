using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(NW.SignalR))]
namespace NW
{
    public class SignalR
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}