using Datos.Contratos.Rates;
using Datos.Dapper.Users;
using Transversales.Modelos;

namespace Datos.Dapper.RatesRepositories
{
    public class RatesRepository : BaseRepository<RatesByTime>, IRatesRepository
    {
        #region Constructors
        public RatesRepository()
        {

        }

        #endregion
    }
}
