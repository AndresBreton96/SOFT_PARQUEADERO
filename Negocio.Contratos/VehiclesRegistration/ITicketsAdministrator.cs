using System;
using System.Collections.Generic;
using Transversales.Modelos.RegistrationEntries;

namespace Negocio.Contratos.VehiclesRegistration
{
    public interface ITicketsAdministrator
    {
        IEnumerable<Tickets> GetAll(DateTime initialDate, DateTime finalDate, EntryType entryType, string licensePlate);

        Tickets GetAll(int ticketId);

        Tickets GetEntryTicket(string licensePlate);

        void AddTicket(Tickets ticket);
    }
}
