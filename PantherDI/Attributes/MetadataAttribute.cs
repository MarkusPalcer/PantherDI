using System;

namespace PantherDI.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
    public class MetadataAttribute : Attribute
    {
        public MetadataAttribute(string key, object value)
        {
            Key = key;
            Value = value;
            HasValue = true;
        }

        public MetadataAttribute(string key = null)
        {
            Key = key;
            HasValue = false;
        }

        public bool HasValue { get; }

        public string Key { get; }

        public object Value { get; }
    }
}