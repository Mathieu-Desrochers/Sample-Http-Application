
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Dependencies;

using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

namespace SampleHttpApplication.Infrastructure.Code.DependencyInjection
{
    /// <summary>
    /// Represents the dependency resolver factory.
    /// </summary>
    public static class DependencyResolverFactory
    {
        /// <summary>
        /// Builds the dependency resolver.
        /// </summary>
        public static IDependencyResolver NewDependencyResolver()
        {
            // Build the Unity container.
            IUnityContainer unityContainer = new UnityContainer().LoadConfiguration();

            // Build the dependency resolver.
            IDependencyResolver dependencyResolver = new DependencyResolver(unityContainer);
            return dependencyResolver;
        }
    }
}
