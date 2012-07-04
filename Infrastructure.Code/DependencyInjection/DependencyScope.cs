
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Dependencies;

using Microsoft.Practices.Unity;

namespace SampleHttpApplication.Infrastructure.Code.DependencyInjection
{
    /// <summary>
    /// Represents the dependency scope.
    /// </summary>
    public class DependencyScope : IDependencyScope
    {
        /// <summary>
        /// The underlying Unity container.
        /// </summary>
        private IUnityContainer unityContainer;

        /// <summary>
        /// Initialization constructor.
        /// </summary>
        public DependencyScope(IUnityContainer unityContainer)
        {
            this.unityContainer = unityContainer;
        }

        /// <summary>
        /// Resolves singly registered services that support arbitrary object creation.
        /// </summary>
        public object GetService(Type serviceType)
        {
            // Make sure the service type is registered.
            if (this.unityContainer.IsRegistered(serviceType) == false)
            {
                return null;
            }

            // Resolve the service type.
            object service = this.unityContainer.Resolve(serviceType);
            return service;
        }

        /// <summary>
        /// Resolves multiply registered services.
        /// </summary>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            // Make sure the service type is registered.
            if (this.unityContainer.IsRegistered(serviceType) == false)
            {
                return new object[0];
            }

            // Resolve the service type.
            object[] services = this.unityContainer.ResolveAll(serviceType).ToArray();
            return services;
        }

        /// <summary>
        /// Disposes of the dependency scope.
        /// </summary>
        public void Dispose()
        {
            this.unityContainer.Dispose();
        }
    }
}
