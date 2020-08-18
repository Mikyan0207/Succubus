using System;

namespace Succubus.Exceptions
{
    public class SuccubusException : Exception
    {
        public SuccubusException(string message, Exception ex) : base(message, ex)
        {}
    }
}