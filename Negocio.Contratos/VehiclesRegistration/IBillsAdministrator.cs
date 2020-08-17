using System.Collections.Generic;
using Transversales.Modelos.VehiclesRegistration;

namespace Negocio.Contratos.VehiclesRegistration
{
    public interface IBillsAdministrator
    {
        IEnumerable<Bills> GetAll();
        void AddBill(Bills bill);
    }
}
