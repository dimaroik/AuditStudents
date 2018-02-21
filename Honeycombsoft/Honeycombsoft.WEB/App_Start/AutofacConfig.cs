using Autofac;
using Autofac.Integration.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Reflection;
using Honeycombsoft.DAL.Interfaces;
using Honeycombsoft.DAL.Repositories;
using Honeycombsoft.BLL.Interfaces;
using Honeycombsoft.BLL.Services;
using Honeycombsoft.BLL.Management;

namespace Honeycombsoft.WEB.App_Start
{
    public static class AutofacConfig
    {   
        public static void Run()
        {
            SetAutofacContainer();
        }
 
        private static void SetAutofacContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<IdentityUnitOfWork>().As<IUnitOfWork>().WithParameter("connectionString", "HoneyConnection").InstancePerRequest();
            builder.RegisterType<UserService>().As<IUserService>().InstancePerRequest();
            builder.RegisterType<AdminManage>().As<IAdminManage>().InstancePerRequest();
        
 
            IContainer container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

    }

   
}