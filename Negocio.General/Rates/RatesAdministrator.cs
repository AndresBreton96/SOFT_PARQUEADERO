using Datos.Contratos.Rates;
using Negocio.Contratos.Rates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Transversales.Modelos;
using Transversales.Modelos.Exceptions;

namespace Negocio.General.Rates
{
    public class RatesAdministrator : IRatesAdministrator
    {
        #region Constructor
        public RatesAdministrator(IRatesRepository repositorio)
        {
            _repositorio = repositorio;
        }

        #endregion

        #region Variables
        private readonly IRatesRepository _repositorio;

        #endregion

        #region Methods
        public IEnumerable<RatesByTime> GetAll()
        {
            var rates = _repositorio.ExecuteQuery("SELECT * FROM RatesByTime");
            foreach(var rate in rates)
            {
                rate.Name = Enum.GetName(typeof(RateType), rate.RateType);
            }

            return rates;
        }

        public IEnumerable<RatesByTime> GetAll(byte id)
        {
            var rates = _repositorio.ExecuteQuery($"SELECT * FROM RatesByTime WHERE RateId = {id}");
            foreach (var rate in rates)
            {
                rate.Name = Enum.GetName(typeof(RateType), rate.RateType);
            }

            return rates;
        }

        public void AddRate(RatesByTime rate)
        {
            try
            {
                var lastId = _repositorio.GetLastId("RateId");

                var existingRate = _repositorio.ExecuteQuery($"SELECT * FROM RatesByTime WHERE RateType = {(byte)rate.RateType}");

                if (existingRate.Any())
                    throw new ExistingRateException(rate.RateType);

                using(var scope = new TransactionScope())
                {
                    _repositorio.ExecuteQuery($@"INSERT INTO [dbo].[RatesByTime]
                                                 VALUES
                                                    ({lastId + 1}
                                                    ,'{rate.Description}'
                                                    ,{(byte)rate.RateType}
                                                    ,{rate.Value}
                                                    ,{rate.Time}) ");

                    scope.Complete();
                }
            }
            catch (ExistingRateException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateRate(RatesByTime rate)
        {
            try
            {
                var existingRate = _repositorio.ExecuteQuery($"SELECT * FROM RatesByTime WHERE RateId = {rate.RateId}");

                if (!existingRate.Any())
                    throw new ExistingRateException(rate.RateType);

                using (var scope = new TransactionScope())
                {
                    _repositorio.ExecuteQuery($@"UPDATE [dbo].[RatesByTime]
                                                 SET Description = '{rate.Description}',
                                                     RateType = {(byte)rate.RateType},
                                                     Value = {rate.Value},
                                                     Time = {rate.Time}
                                                 WHERE RateId = {rate.RateId}");

                    scope.Complete();
                }
            }
            catch (ExistingRateException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DropRate(RatesByTime rate)
        {
            try
            {
                _repositorio.ExecuteQuery($"DELETE FROM RatesByTime WHERE RateId = {rate.RateId}");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
