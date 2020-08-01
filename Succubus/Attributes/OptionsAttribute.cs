using System;

namespace Succubus.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class OptionsAttribute : Attribute
    {
        public OptionsAttribute(Type type)
        {
            Type = type;
        }

        public Type Type { get; set; }
    }
}