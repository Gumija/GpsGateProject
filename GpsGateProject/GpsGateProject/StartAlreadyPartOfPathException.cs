using System;
using System.Runtime.Serialization;

namespace GpsGateProject
{
    [Serializable]
    internal class StartAlreadyPartOfPathException : Exception
    {
        public StartAlreadyPartOfPathException()
        {
        }

        public StartAlreadyPartOfPathException(string message) : base(message)
        {
        }

        public StartAlreadyPartOfPathException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected StartAlreadyPartOfPathException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}