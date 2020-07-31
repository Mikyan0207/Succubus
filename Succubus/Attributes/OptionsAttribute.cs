using System;

namespace Succubus.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class OptionsAttribute : Attribute
    {
        public Type Type { get; set; }

        public OptionsAttribute(Type type)
        {
            Type = type;
        }
    }
}