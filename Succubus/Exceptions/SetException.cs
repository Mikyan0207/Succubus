using System;

namespace Succubus.Exceptions
{
    public class SetException : SuccubusException
    {
        public SetException(string message, Exception ex) : base(message, ex)
        {}

        public SetException(string message) : base(message, null)
        {}
    }
}