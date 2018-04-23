using System;
using System.Runtime.Serialization;

namespace TrainingPlan.ApplicationCore.Exceptions
{
    public class TrainingPlanException : Exception
    {
        protected TrainingPlanException()
        {
        }

        protected TrainingPlanException(string message) : base(message)
        {
        }

        protected TrainingPlanException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TrainingPlanException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}