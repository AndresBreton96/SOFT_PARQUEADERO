using System;

namespace Transversales.Modelos.Exceptions
{
    public class EntryTicketNotFoundException : Exception
    {
        public int EntryTicketId { get; set; }

        public EntryTicketNotFoundException(int entryTicketId)
        {
            EntryTicketId = entryTicketId;
        }

        public EntryTicketNotFoundException(string message, int entryTicketId) : base(message)
        {
            EntryTicketId = entryTicketId;
        }

        public EntryTicketNotFoundException(string message, Exception innerException, int entryTicketId) : base(message, innerException)
        {
            EntryTicketId = entryTicketId;
        }
    }
}
