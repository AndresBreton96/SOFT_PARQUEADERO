using Negocio.Contratos.Rates;
using Negocio.Contratos.VehiclesRegistration;
using Presentacion.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Transversales.Modelos;
using Transversales.Modelos.Exceptions;
using Transversales.Modelos.RegistrationEntries;

namespace Presentacion.WPF.Commands.VehiclesRegistration
{
    public class CalculatePriceCommand : ICommand
    {
        #region Constructor
        public CalculatePriceCommand(RegisterDepartureViewModel registerDepartureViewModel, ITicketsAdministrator ticketsAdministrator, IRatesAdministrator ratesAdministrator)
        {
            _registerDepartureViewModel = registerDepartureViewModel;
            _ticketsAdministrator = ticketsAdministrator;
            _ratesAdministrator = ratesAdministrator;
        }

        #endregion

        #region Variables
        private readonly RegisterDepartureViewModel _registerDepartureViewModel;
        private readonly ITicketsAdministrator _ticketsAdministrator;
        private readonly IRatesAdministrator _ratesAdministrator;

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
                    var entryTicket = _ticketsAdministrator.GetEntryTicket((string)parameter);
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

                    var departureTime = DateTime.Now;
                    _registerDepartureViewModel.DepartureTime = departureTime;


                    var timeToCharge = Math.Ceiling((departureTime - entryTicket.EntryDate).TotalMinutes);
                    if(timeToCharge < fractionaryRate.Time)
                    {
                        var hours = 0.00;
                        var fractions = 1.00;
                        _registerDepartureViewModel.Hours = hours;
                        _registerDepartureViewModel.Fractions = fractions;
                    }
                    else if (timeToCharge < hourlyRate.Time)
                    {
                        var hours = 0.00;
                        var fractions = Math.Ceiling(timeToCharge / fractionaryRate.Time);
                        if (fractions == Math.Ceiling(hourlyRate.Time / fractionaryRate.Time))
                        {
                            hours = Math.Ceiling(timeToCharge / hourlyRate.Time);
                            fractions = 0;
                        }
                        _registerDepartureViewModel.Hours = hours;
                        _registerDepartureViewModel.Fractions = fractions;
                    }
                    else
                    {
                        var hours = Math.Truncate(timeToCharge / hourlyRate.Time);
                        var fractions = Math.Ceiling((timeToCharge - (hours * hourlyRate.Time)) / fractionaryRate.Time);
                        if (fractions == Math.Ceiling(hourlyRate.Time / fractionaryRate.Time))
                        {
                            hours = Math.Ceiling(timeToCharge / hourlyRate.Time);
                            fractions = 0;
                        }
                        _registerDepartureViewModel.Hours = hours;
                        _registerDepartureViewModel.Fractions = fractions;
                    }
                }
            }
            catch (DepartureTicketAlreadyRegistered ex)
            {
                MessageBox.Show($"La salida del vehículo de placas {(string)parameter} ya ha sido registrada.");
                throw ex;
            }
            catch (TicketNotFoundException ex)
            {
                MessageBox.Show($"No se ha encontrado una entrada registrada para el vehículo de placas {(string)parameter}");
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
