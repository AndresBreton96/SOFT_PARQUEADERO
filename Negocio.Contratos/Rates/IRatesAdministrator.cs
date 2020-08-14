using System.Collections.Generic;
using Transversales.Modelos;

namespace Negocio.Contratos.Rates
{
    public interface IRatesAdministrator
    {
        IEnumerable<RatesByTime> GetAll();

        IEnumerable<RatesByTime> GetAll(byte id);

        void AddRate(RatesByTime rate);

        void UpdateRate(RatesByTime rate);

        void DropRate(RatesByTime rate);
    }
}
