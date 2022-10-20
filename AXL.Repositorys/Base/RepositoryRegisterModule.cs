using Autofac;
using AXL.Repositorys.Contract.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AXL.Repositorys.Base
{
    public class RepositoryRegisterModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(ThisAssembly)
                  .Where(t => t.IsClosedTypeOf(typeof(IRepository<>)))
                 .AsImplementedInterfaces()
                 .InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);
        }
    }
}
