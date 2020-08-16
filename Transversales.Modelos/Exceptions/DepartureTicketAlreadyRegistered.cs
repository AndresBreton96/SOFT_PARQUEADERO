using System;

namespace Transversales.Modelos.Exceptions
{
    public class DepartureTicketAlreadyRegistered : Exception
    {
        public int DepartureTicketId { get; set; }

        public DepartureTicketAlreadyRegistered(int entryTicketId)
        {
            DepartureTicketId = entryTicketId;
        }

        public DepartureTicketAlreadyRegistered(string message, int entryTicketId) : base(message)
        {
            DepartureTicketId = entryTicketId;
        }

        public DepartureTicketAlreadyRegistered(string message, Exception innerException, int entryTicketId) : base(message, innerException)
        {
            DepartureTicketId = entryTicketId;
        }
    }
}
