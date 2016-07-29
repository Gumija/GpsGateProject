using System;
using System.Runtime.Serialization;

namespace GpsGateProject
{
    [Serializable]
    internal class NoSuchPathException : Exception
    {
        public NoSuchPathException()
        {
        }

        public NoSuchPathException(string message) : base(message)
        {
        }

        public NoSuchPathException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NoSuchPathException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}