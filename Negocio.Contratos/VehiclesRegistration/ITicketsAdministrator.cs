using System;
using System.Collections.Generic;
using Transversales.Modelos.RegistrationEntries;

namespace Negocio.Contratos.VehiclesRegistration
{
    public interface ITicketsAdministrator
    {
        IEnumerable<Tickets> GetAll(DateTime initialDate, DateTime finalDate, EntryType entryType, string licensePlate);

        Tickets GetAll(int ticketId);

        Tickets GetDepartureTicket(int ticketId);

        Tickets GetEntryTicket(string licensePlate, bool ignoreDepartureTicket = false);

        void AddTicket(Tickets ticket);
    }
}
