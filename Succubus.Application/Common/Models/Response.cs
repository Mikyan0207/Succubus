using Succubus.Core.Common;
using System;

namespace Succubus.Application.Common.Models
{
    public class Response<T>
    {
        public Result Result { get; set; }

        public Optional<T> Content { get; set; } = new None<T>();

        public int ErrorCode { get; set; }

        public Optional<string> ErrorMessage { get; set; } = new None<string>();

        public Optional<Exception> Exception { get; set; } = new None<Exception>();
    }
}
