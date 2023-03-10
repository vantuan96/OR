using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace OR.AutoUpdateManager
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            //#if DEBUG
            //ORService sv = new ORService();
            //sv.OnDebug();
            //System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
            //#else
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new ORService()
            };
            ServiceBase.Run(ServicesToRun);
            //#endif
        }
    }
}
