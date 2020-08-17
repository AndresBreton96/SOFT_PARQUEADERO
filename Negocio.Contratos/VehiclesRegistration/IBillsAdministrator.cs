using System;
using System.Collections.Generic;
using Transversales.Modelos.VehiclesRegistration;

namespace Negocio.Contratos.VehiclesRegistration
{
    public interface IBillsAdministrator
    {
        IEnumerable<Bills> GetAll();
        IEnumerable<Bills> GetAll(DateTime initialDate, DateTime finalDate, string licensePlate);
        void AddBill(Bills bill);
    }
}
