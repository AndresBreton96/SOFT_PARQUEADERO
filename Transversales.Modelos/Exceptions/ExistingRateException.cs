using System;

namespace Transversales.Modelos.Exceptions
{
    public class ExistingRateException : Exception
    {
        public RateType RateType { get; set; }

        public ExistingRateException(RateType rateType)
        {
            RateType = rateType;
        }

        public ExistingRateException(string message, RateType rateType) : base(message)
        {
            RateType = rateType;
        }

        public ExistingRateException(string message, Exception innerException, RateType rateType) : base(message, innerException)
        {
            RateType = rateType;
        }
    }
}
