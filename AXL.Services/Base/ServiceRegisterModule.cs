using Autofac;
using AXL.Services.Contract.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Features.AttributeFilters;

namespace AXL.Services.Base
{
    public class ServiceRegisterModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(ThisAssembly)
                 .Where(t => t.IsClosedTypeOf(typeof(IService<>)))
                 .AsImplementedInterfaces()
                 .InstancePerLifetimeScope()
                 .WithAttributeFiltering();
                 //.PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);
        }
    }
}
