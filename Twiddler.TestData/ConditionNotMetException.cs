using System;
using System.Runtime.Serialization;

namespace Twiddler.TestData
{
    [Serializable]
    public class ConditionNotMetException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public ConditionNotMetException()
        {
        }

        public ConditionNotMetException(string message) : base(message)
        {
        }

        public ConditionNotMetException(string message, Exception inner) : base(message, inner)
        {
        }

        protected ConditionNotMetException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}