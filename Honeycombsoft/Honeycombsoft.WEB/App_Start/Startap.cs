using Hangfire;

using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;


[assembly: OwinStartup(typeof(Honeycombsoft.WEB.App_Start.Startup))]

namespace Honeycombsoft.WEB.App_Start
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
          
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login")
               
            });


            GlobalConfiguration.Configuration.UseSqlServerStorage("HoneyConnection");
            app.UseHangfireDashboard();
            //BackgroundJob.Enqueue(() => Console.WriteLine("Fire-and-forget!"));
            //BackgroundJob.Enqueue(() => emailService.SendEmail("andriyroik@gmail.com"));
            app.UseHangfireServer();
        }

    }
}