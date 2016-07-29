using System;
using System.Runtime.Serialization;

namespace GpsGateProject
{
    [Serializable]
    internal class EndAlreadyPartOfPathException : Exception
    {
        public EndAlreadyPartOfPathException()
        {
        }

        public EndAlreadyPartOfPathException(string message) : base(message)
        {
        }

        public EndAlreadyPartOfPathException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected EndAlreadyPartOfPathException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}