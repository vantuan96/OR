using System.Diagnostics;
using System.Reflection;

namespace VG.EncryptLib.Shared
{
    public class MethodInfo
    {
        /// <summary>
        /// Get method info
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentMethod()
        {
            StackFrame stackFrame = new StackFrame();
            MethodBase methodBase = stackFrame.GetMethod();

            string fullName = string.Empty;
            if (methodBase.ReflectedType != null)
            {
                fullName = string.Format("{0}.{1}", methodBase.ReflectedType.FullName, methodBase.Name);
            }
            return fullName;
        }
    }
}
