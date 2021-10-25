using System;

namespace Mobile.Helpers
{
    public class BusinessException : Exception
    {
        public BusinessException(string message) : base(message) { }
    }
}
