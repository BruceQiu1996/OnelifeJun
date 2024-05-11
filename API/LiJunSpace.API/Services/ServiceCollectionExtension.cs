﻿using Autofac;
using System.Reflection;

namespace LiJunSpace.API.Services
{
    public static class ServiceCollectionExtension
    {
        public static void AddApplicationContainer(this ContainerBuilder container, Assembly assembly)
        {
            container.RegisterAssemblyTypes(assembly)
                .Where(t => typeof(IAppService).IsAssignableFrom(t)).InstancePerLifetimeScope();
        }
    }
}
