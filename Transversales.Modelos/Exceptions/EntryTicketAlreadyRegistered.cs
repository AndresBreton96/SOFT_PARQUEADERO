using System;

namespace Transversales.Modelos.Exceptions
{
    public class EntryTicketAlreadyRegistered : Exception
    {
        public string LicensePlate { get; set; }

        public EntryTicketAlreadyRegistered(string licensePlate)
        {
            LicensePlate = licensePlate;
        }

        public EntryTicketAlreadyRegistered(string message, string licensePlate) : base(message)
        {
            LicensePlate = licensePlate;
        }

        public EntryTicketAlreadyRegistered(string message, Exception innerException, string licensePlate) : base(message, innerException)
        {
            LicensePlate = licensePlate;
        }
    }
}
