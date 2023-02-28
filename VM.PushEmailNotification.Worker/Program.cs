using Caching.OR;
using Contract.OR;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Policy;
using System.ServiceProcess;
using System.Text.RegularExpressions;
using VG.Common;

namespace OREmailNoti.WindowService
{
    static class Program
    {
       
        static void Main()
        {
            //#if DEBUG
            //MailService mailService = new MailService();
            //mailService.onDebug();
            //#else
                ServiceBase[] services;
                services = new ServiceBase[]
                {
                     new MailService(),
                };
                ServiceBase.Run(services);
            //#endif
        }
    }
    
}
