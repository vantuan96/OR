using System;

namespace VG.Framework.Mvc.Conventions
{
    public class MetadataConventionsAttribute : Attribute
    {
        public Type ResourceType { get; set; }
    }
}
