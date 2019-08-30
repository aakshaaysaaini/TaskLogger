using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TaskLoggerApplication.Startup))]
namespace TaskLoggerApplication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
