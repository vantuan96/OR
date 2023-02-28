using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;

namespace ADR.HCM.EncryptLib.Shared
{
    public class MetaInfoUtil
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

        /// <summary>
        /// Get property info of any type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lambda"></param>
        /// <returns></returns>
        protected static PropertyInfo GetPropertyInfo<T>(Expression<Func<T, object>> lambda)
        {
            var member = lambda.Body as MemberExpression;
            return member.Member as PropertyInfo;
        }
    }
}