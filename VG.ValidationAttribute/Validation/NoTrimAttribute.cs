using System;

namespace VG.ValidAttribute.Validation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class NoTrimAttribute : Attribute
    {
    }
}
