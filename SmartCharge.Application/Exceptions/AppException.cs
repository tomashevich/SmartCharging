using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCharge.Application.Exceptions
{
    public abstract class AppException : Exception
    {
        public virtual string Code { get; }

        protected AppException(string message) : base(message)
        {
        }
    }
}
