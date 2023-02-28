using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc5;
using Caching.Core;

namespace Admin
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            
            // register all your components with the container here
            // it is NOT necessary to register your controllers
            
           //  container.RegisterType<IQueuePatientCaching, QueuePatientCaching>(new ResolvedParameter);
            
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}