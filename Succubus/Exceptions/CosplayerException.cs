using System;

namespace Succubus.Exceptions
{
    public class CosplayerException : SuccubusException
    {
        public CosplayerException(string message, Exception ex) : base(message, ex)
        { }

        public CosplayerException(string message) : base(message, null)
        { }
    }
}