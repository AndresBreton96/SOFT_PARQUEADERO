using System;

namespace Transversales.Modelos.RegistrationEntries
{
    public class Tickets
    {
        public int TicketId { get; set; }
        public string LicensePlate { get; set; }
        public DateTime EntryDate { get; set; }
        public EntryType EntryType { get; set; }
        public int EntryTicketId { get; set; }
    }

    public enum EntryType : byte
    {
        Vacio = 0,
        Entrance = 1,
        Departure = 2
    }
}
