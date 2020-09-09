using Negocio.Contratos.Rates;
using Negocio.Contratos.VehiclesRegistration;
using Presentacion.WPF.ViewModels;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Transversales.Modelos;
using Transversales.Modelos.Exceptions;
using Transversales.Modelos.RegistrationEntries;
using Transversales.Modelos.VehiclesRegistration;
using Transversales.Utilitarios.Printing;

namespace Presentacion.WPF.Commands.VehiclesRegistration
{
    public class CreateBillCommand : ICommand
    {
        #region Constructor
        public CreateBillCommand(RegisterDepartureViewModel registerDepartureViewModel, ITicketsAdministrator ticketsAdministrator, IRatesAdministrator ratesAdministrator, IBillsAdministrator billsAdministrator)
        {
            _registerDepartureViewModel = registerDepartureViewModel;
            _ticketsAdministrator = ticketsAdministrator;
            _ratesAdministrator = ratesAdministrator;
            _billsAdministrator = billsAdministrator;
        }

        #endregion

        #region Variables
        private readonly RegisterDepartureViewModel _registerDepartureViewModel;
        private readonly ITicketsAdministrator _ticketsAdministrator;
        private readonly IRatesAdministrator _ratesAdministrator;
        private readonly IBillsAdministrator _billsAdministrator;

        public event EventHandler CanExecuteChanged;

        #endregion

        #region Events
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            try
            {
                if (parameter is string)
                {
                    var entryTicket = _ticketsAdministrator.GetEntryTicket((string)parameter, true);
                    var rates = _ratesAdministrator.GetAll();

                    if (!rates.Any())
                    {
                        MessageBox.Show("No se han configurado tarifas para el cobro, por favor ingrese ambas tarifas e intente nuevamente.");
                        return;
                    }
                    var hourlyRate = rates.FirstOrDefault(x => entryTicket.VehicleType == VehicleType.Car && x.RateType == RateType.HoraCarro || entryTicket.VehicleType == VehicleType.Bike && x.RateType == RateType.HoraMoto);
                    if (hourlyRate == null)
                    {
                        MessageBox.Show("No se ha configurado una tarifa horaria para el cobro, por favor ingrese ambas tarifas e intente nuevamente.");
                        return;
                    }
                    var fractionaryRate = rates.FirstOrDefault(x => entryTicket.VehicleType == VehicleType.Car && x.RateType == RateType.FraccionCarro || entryTicket.VehicleType == VehicleType.Bike && x.RateType == RateType.FraccionMoto);
                    if (fractionaryRate == null)
                    {
                        MessageBox.Show("No se ha configurado una tarifa por fracción para el cobro, por favor ingrese ambas tarifas e intente nuevamente.");
                        return;
                    }

                    var departureTicket = _ticketsAdministrator.GetDepartureTicket(entryTicket.TicketId);

                    var time = _registerDepartureViewModel.Hours * hourlyRate.Time + _registerDepartureViewModel.Fractions * fractionaryRate.Time;

                    var price = (_registerDepartureViewModel.Hours * hourlyRate.Value ) + _registerDepartureViewModel.Fractions * fractionaryRate.Value;

                    var bill = new Bills()
                    {
                        BillId = 0,
                        Consecutive = 0,
                        LicensePlate = (string)parameter,
                        EntryTicketId = entryTicket.TicketId,
                        DepartureTicketId = departureTicket.TicketId,
                        ParkingCharged = time,
                        ParkingTime = (departureTicket.EntryDate - entryTicket.EntryDate).TotalMinutes,
                        Rate = price,
                        EntryDate = entryTicket.EntryDate,
                        DepartureDate = departureTicket.EntryDate
                    };

                    _billsAdministrator.AddBill(bill);

                    _registerDepartureViewModel.Price = price;
                }
            }
            catch (TicketNotFoundException ex)
            {
                MessageBox.Show($"No se ha encontrado el ticket con id {ex.EntryTicketId} para el vehículo de placas {(string)parameter}.");
                throw ex;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw ex;
            }
        }

        #endregion
    }
}
