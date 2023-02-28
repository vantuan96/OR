using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Unity.Mvc5;
using Caching.Core;
using Caching.Microsite;
using Caching.Report;
using Caching.OR;
using Admin.Shared;
using Business.API;
//using Caching.BMSMaster;

namespace Admin
{
    public static class Bootstrapper
    {
        public static IUnityContainer Initialise()
        {
            var container = BuildUnityContainer();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));

            return container;
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();
            //container.LoadConfiguration();
            // register all your components with the container here
            // it is NOT necessary to register your controllers

            container.RegisterType<IAuthenCaching, AuthenCaching>();
            container.RegisterType<ISystemSettingCaching, SystemSettingCaching>();
            container.RegisterType<IUserProfileCaching, UserProfileCaching>();
            container.RegisterType<ILogCaching, LogCaching>();
            container.RegisterType<ISystemCaching, SystemCaching>();
            container.RegisterType<IUserProfileCaching, UserProfileCaching>();
            container.RegisterType<IQueuePatientCaching, QueuePatientCaching>();
            container.RegisterType<IUserMngtCaching, UserMngtCaching>();
            container.RegisterType<IMicrositeMngtCaching, MicrositeMngtCaching>();
            container.RegisterType<IReportCaching, ReportCaching>();
            container.RegisterType<ILocationCaching, LocationCaching>();
            container.RegisterType<ICheckListCaching, CheckListCaching>();
            container.RegisterType<IMasterCaching, MasterCaching>();
            container.RegisterType<IOperationCheckListCaching, OperationCheckListCaching>();
            container.RegisterType<ILogObjectCaching, LogObjectCaching>();
            container.RegisterType<IORCaching, ORCaching>();
            container.RegisterType<ISyncGateWay,SyncGateWay>();

            
            RegisterTypes(container);
            return container;
        }

        public static void RegisterTypes(IUnityContainer container)
        {

        }
    }
}
