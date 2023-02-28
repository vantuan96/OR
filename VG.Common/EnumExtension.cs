using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VG.Common
{
    public static class EnumExtension
    {
        /// <summary>
        /// Get description annotation of an enum value
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum element)
        {
            Type type = element.GetType();

            MemberInfo[] memberInfo = type.GetMember(element.ToString());

            if (memberInfo != null && memberInfo.Length > 0)
            {
                object[] attributes = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attributes != null && attributes.Length > 0)
                {
                    return ((DescriptionAttribute)attributes[0]).Description;
                }
            }

            return element.ToString();
        }

        /// <summary>
        /// Get description annotation of an enum value
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static int GetSortValue(this Enum element)
        {
            Type type = element.GetType();

            MemberInfo[] memberInfo = type.GetMember(element.ToString());

            if (memberInfo != null && memberInfo.Length > 0)
            {
                object[] attributes = memberInfo[0].GetCustomAttributes(typeof(SortValueAttribute), false);

                if (attributes != null && attributes.Length > 0)
                {
                    return int.Parse(((SortValueAttribute)attributes[0]).Value.ToString());
                }
            }

            return 0;
        }

        /// <summary>
        /// Check enum value has DesignOnlyAttribute
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static bool IsHiddenEnumItem(this Enum element)
        {
            Type type = element.GetType();

            MemberInfo[] memberInfo = type.GetMember(element.ToString());

            if (memberInfo != null && memberInfo.Length > 0)
            {
                object[] attributes = memberInfo[0].GetCustomAttributes(typeof(DesignOnlyAttribute), false);

                if (attributes != null && attributes.Length > 0)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Convert enum type to list
        /// </summary>
        /// <typeparam name="Tin">Must be Enum type</typeparam>
        /// <returns>List of EnumValue{Value, Description}</returns>
        public static List<EnumValue> ToListOfValueAndDesc<Tin>()
        {
            Type t = typeof(Tin);
            return !t.IsEnum ? null : Enum.GetValues(t).Cast<Enum>().Select(x => new EnumValue { Sort = x.GetSortValue(), Value = (int)Enum.Parse(t, x.ToString()), Description = x.GetDescription(), Name = x.ToString() }).ToList();
        }
        
        /// <summary>
        /// Convert Enum defination to SelectList. Do not showed enum has HiddenInputAttribute
        /// </summary>
        /// <typeparam name="Tin">Must be Enum type</typeparam>
        /// <returns></returns>
        public static List<EnumValue> EnumNoHiddenToListOfValueAndDesc<Tin>(string selectedValue)
        {
            Type t = typeof(Tin);
            return !t.IsEnum ? null : Enum.GetValues(t).Cast<Enum>().Where(e => !e.IsHiddenEnumItem()).Select(x => new EnumValue { Sort = x.GetSortValue(), Value = (int)Enum.Parse(t, x.ToString()), Description = x.GetDescription(), Name = x.ToString() }).ToList();
        }
        public static int GetLastPage(int TotalRow,int PageSize)
        {
            int LastPage = TotalRow / PageSize;
            if (TotalRow % PageSize > 0)
            {
                LastPage++;
            }
            return LastPage;
           
        }

    }

    public sealed class SortValueAttribute : DefaultValueAttribute
    {
        public SortValueAttribute(int value) : base(value) { }
    }

    public class EnumValue
    {
        public int Value { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Sort { get; set; }

        public EnumValue()
        {
            Sort = 0;
        }
    }
}
