using System;

namespace Transversales.Modelos.Exceptions
{
    public class TicketNotFoundException : Exception
    {
        public int EntryTicketId { get; set; }

        public TicketNotFoundException()
        {
            EntryTicketId = 0;
        }

        public TicketNotFoundException(int entryTicketId)
        {
            EntryTicketId = entryTicketId;
        }

        public TicketNotFoundException(string message, int entryTicketId) : base(message)
        {
            EntryTicketId = entryTicketId;
        }

        public TicketNotFoundException(string message, Exception innerException, int entryTicketId) : base(message, innerException)
        {
            EntryTicketId = entryTicketId;
        }
    }
}
