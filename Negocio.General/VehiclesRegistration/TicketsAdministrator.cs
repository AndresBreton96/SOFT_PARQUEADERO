using Datos.Contratos.VehiclesRegistration;
using Negocio.Contratos.VehiclesRegistration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Transversales.Modelos.Exceptions;
using Transversales.Modelos.RegistrationEntries;
using Transversales.Utilitarios.Printing;

namespace Negocio.General.VehiclesRegistration
{
    public class TicketsAdministrator : ITicketsAdministrator
    {
        #region Constructor
        public TicketsAdministrator(ITicketsRepository repositorio)
        {
            _repositorio = repositorio;
        }

        #endregion

        #region Variables
        private readonly ITicketsRepository _repositorio;

        #endregion

        #region Methods
        public IEnumerable<Tickets> GetAll(DateTime initialDate, DateTime finalDate, EntryType entryType, string licensePlate)
        {
            var tickets = _repositorio.ExecuteQuery($@"SELECT * 
                                                       FROM Tickets 
                                                       WHERE CAST(EntryDate AS date) >= '{initialDate:yyyy/MM/dd}' AND CAST(EntryDate AS date) < '{finalDate:yyyy/MM/dd}' AND
                                                             (0 = {(byte)entryType} OR EntryType = {(byte)entryType}) AND LicensePlate LIKE '%{licensePlate.Replace(' ', '%')}%'");
            if (tickets.Any())
            {
                foreach(var ticket in tickets)
                {
                    ticket.EntryTypeName = Enum.GetName(typeof(EntryType), ticket.EntryType);
                }
                return tickets;
            }

            throw new TicketNotFoundException();
        }

        public Tickets GetAll(int ticketId)
        {
            var ticket = _repositorio.ExecuteQuery($"SELECT * FROM Tickets WHERE TicketId = {ticketId}");
            if (ticket.Any())
                return ticket.FirstOrDefault();

            throw new TicketNotFoundException(ticketId);
        }

        public Tickets GetDepartureTicket(int ticketId)
        {
            var ticket = _repositorio.ExecuteQuery($"SELECT * FROM Tickets WHERE EntryTicketId = {ticketId}");
            if (ticket.Any())
                return ticket.FirstOrDefault();

            throw new TicketNotFoundException(ticketId);
        }

        public Tickets GetEntryTicket(string licensePlate, bool ignoreDepartureTicket = false)
        {
            var ticket = _repositorio.ExecuteQuery($@"SELECT * 
                                                      FROM Tickets 
                                                      WHERE LicensePlate = '{licensePlate}' AND EntryType = {(byte)EntryType.Entrada}
                                                      ORDER BY TicketId Desc");
            if (ticket.Any())
            {
                if (!ignoreDepartureTicket)
                {
                    var departureTicket = _repositorio.ExecuteQuery($"SELECT * FROM Tickets WHERE EntryTicketId = {ticket.FirstOrDefault().TicketId}");
                    if (departureTicket.Any())
                        throw new DepartureTicketAlreadyRegistered(departureTicket.FirstOrDefault().TicketId);
                }
                return ticket.FirstOrDefault();
            }

            throw new TicketNotFoundException();
        }

        public void AddTicket(Tickets ticket)
        {
            try
            {
                var lastId = _repositorio.GetLastId("TicketId");

                if (ticket.EntryType == EntryType.Entrada)
                {
                    var entryTicket = _repositorio.ExecuteQuery($"SELECT * FROM Tickets WHERE LicensePlate = '{ticket.LicensePlate}' AND EntryType = {(byte)EntryType.Entrada} ORDER BY TicketId Desc");
                    if (entryTicket.Any())
                    {
                        var departureTicket = _repositorio.ExecuteQuery($"SELECT * FROM Tickets WHERE EntryTicketId = {entryTicket.FirstOrDefault().TicketId}");

                        if (!departureTicket.Any())
                            throw new EntryTicketAlreadyRegistered(ticket.LicensePlate);
                    }

                }
                if (ticket.EntryType == EntryType.Salida)
                {
                    var entryTicket = _repositorio.ExecuteQuery($"SELECT * FROM Tickets WHERE TicketId = {ticket.EntryTicketId}");
                    if (!entryTicket.Any())
                        throw new EntryTicketNotFoundException(ticket.EntryTicketId);
                    var departureTicket = _repositorio.ExecuteQuery($"SELECT * FROM Tickets WHERE EntryTicketId = {ticket.EntryTicketId}");
                    if (departureTicket.Any())
                        throw new DepartureTicketAlreadyRegistered(departureTicket.FirstOrDefault().TicketId);
                }

                using (var scope = new TransactionScope())
                {
                    _repositorio.ExecuteQuery($@"INSERT INTO [dbo].[Tickets]
                                                 VALUES
                                                    ({lastId + 1}
                                                    ,'{ticket.LicensePlate}'
                                                    ,'{ticket.EntryDate:yyyy-MM-dd HH:mm:ss}'
                                                    ,{(byte)ticket.EntryType}
                                                    ,{(byte)ticket.VehicleType}
                                                    ,{ticket.EntryTicketId})");

                    if (ticket.EntryType == EntryType.Entrada)
                        PrintService.GenerateEntryTicket(ticket);

                    scope.Complete();
                }

            }
            catch (EntryTicketAlreadyRegistered ex)
            {
                throw ex;
            }
            catch (EntryTicketNotFoundException ex)
            {
                throw ex;
            }
            catch (DepartureTicketAlreadyRegistered ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
