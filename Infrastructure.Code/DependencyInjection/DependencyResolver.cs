
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
    /// Represents the dependency resolver.
    /// </summary>
    public class DependencyResolver : DependencyScope, IDependencyResolver
    {
        /// <summary>
        /// The underlying Unity container.
        /// </summary>
        private IUnityContainer unityContainer;

        /// <summary>
        /// Initialization constructor.
        /// </summary>
        public DependencyResolver(IUnityContainer unityContainer)
            : base(unityContainer)
        {
            this.unityContainer = unityContainer;
        }

        /// <summary>
        /// Begins a dependency scope.
        /// </summary>
        public IDependencyScope BeginScope()
        {
            // Unity represents dependency scopes as child containers.
            IUnityContainer unityChildContainer = this.unityContainer.CreateChildContainer();
            
            // Build the dependency scope.
            IDependencyScope dependencyScope = new DependencyScope(unityChildContainer);
            return dependencyScope;
        }
    }
}
