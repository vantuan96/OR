using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace VG.Framework.Mvc.Extentions
{
    public static class ReflectionExtensions
    {
        public static bool PropertyExists(this Type type, string propertyName)
        {
            if (type == null || propertyName == null)
            {
                return false;
            }

            var property = type.GetProperty(propertyName,
                BindingFlags.NonPublic
                | BindingFlags.Public
                | BindingFlags.Static
                | BindingFlags.Instance);

            if (property == null)
            {
                return false;
            }

            var getter = property.GetGetMethod(true);

            return getter.IsPublic || getter.IsAssembly || getter.IsFamilyOrAssembly;

        }

        /// <summary>
        /// Sao chép giá trị của các property từ dữ liệu nguồn sang dữ liệu đích
        /// </summary>
        /// <typeparam name="Tin">Kiểu dữ liệu nguồn</typeparam>
        /// <typeparam name="Tout">Kiểu dữ liệu đích</typeparam>
        /// <param name="source">Dữ liệu nguồn</param>
        /// <param name="desc">Dữ liệu đích</param>
        /// <param name="keyPair">Định nghĩa tên property nguồn và property đích map với nhau</param>
        /// <returns>Trả về true nếu 2 kiểu có thể map với nhau ngược lại trả về false</returns>
        /// <exception cref="System.ArgumentException"></exception>
        public static bool CloneObj<Tin, Tout>(this object source, ref Tout desc, Dictionary<string, string> keyPair = null)
            where Tin : new()
            where Tout : new()
        {
            bool ret = true;
            PropertyInfo[] sourcePropInfos = typeof(Tin).GetProperties();
            List<string> sourcePropName = new List<string>();
            PropertyInfo[] descPropInfos = typeof(Tout).GetProperties();
            string errorNotMatchTypeProp = "Kiểu dữ liệu của trường {0}[{1}] và trường {2}[{3}] không giống nhau";
            sourcePropInfos.ToList().ForEach(x =>
            {
                sourcePropName.Add(x.Name);
            });

            foreach (var propertyInfo in descPropInfos)
            {
                string propName = propertyInfo.Name;

                if (keyPair != null && keyPair.Keys.Count > 0)
                {
                    KeyValuePair<string, string> mapKey = keyPair.Where(x => x.Value == propName).FirstOrDefault();

                    if (mapKey.Key != null && !string.IsNullOrEmpty(mapKey.Key.ToString()))
                    {
                        string mapKeyVal = mapKey.Key.ToString();
                        string descPropTypeName = propertyInfo.PropertyType.Name;
                        string sourcePropTypeName = source.GetType().GetProperty(mapKeyVal).PropertyType.Name;

                        if (descPropTypeName == sourcePropTypeName)
                        {
                            propertyInfo.SetValue(desc, GetPropValue(source, mapKeyVal));
                        }
                        else
                        {
                            throw new ArgumentException(string.Format(errorNotMatchTypeProp, propName, descPropTypeName, mapKeyVal, sourcePropTypeName));
                        }
                    }
                    else if (sourcePropName.Contains(propName))
                    {
                        propertyInfo.SetValue(desc, GetPropValue(source, propName));
                    }
                    else if (ret)
                    {
                        //property của 2 object không khớp
                        ret = false;
                    }
                }
                else
                {
                    if (sourcePropName.Contains(propName))
                    {
                        propertyInfo.SetValue(desc, GetPropValue(source, propName));
                    }
                    else if (ret)
                    {
                        //property của 2 object không khớp
                        ret = false;
                    }
                }
            }

            return ret;
        }

        private static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }
    }
}
