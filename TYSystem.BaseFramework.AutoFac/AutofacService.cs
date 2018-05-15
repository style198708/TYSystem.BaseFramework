using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Autofac;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Autofac.Extensions.DependencyInjection;
using System.Linq;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace TYSystem.BaseFramework.AutoFac
{
    public  class AutofacService
    {
        /// <summary>
        /// 未完成
        /// </summary>
        public  IContainer ApplicationContainer { get; private set; }

        public  IServiceProvider RegisterAutofac(IServiceCollection services, Assembly iface, Assembly service)
        {
            services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());
            var assembly = this.GetType().GetTypeInfo().Assembly;


            var manager = new ApplicationPartManager();
            manager.ApplicationParts.Add(new AssemblyPart(assembly));
            manager.FeatureProviders.Add(new ControllerFeatureProvider());
            var feature = new ControllerFeature();
            manager.PopulateFeature(feature);

            var bulider = new ContainerBuilder();
            bulider.RegisterType<ApplicationPartManager>().AsSelf().SingleInstance();
            bulider.RegisterTypes(feature.Controllers.Select(ti => ti.AsType()).ToArray()).PropertiesAutowired();
            bulider.Populate(services);

            //bulider.RegisterType<AopInterceptor>()

            //Assembly IServices = Assembly.Load("CSTJR.Message.Contracts");
            //Assembly Services = Assembly.Load("CSTJR.Message.Service");
            bulider.RegisterAssemblyTypes(iface, service)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();


            this.ApplicationContainer = bulider.Build();

            return new AutofacServiceProvider(this.ApplicationContainer);
        }
    }
}
