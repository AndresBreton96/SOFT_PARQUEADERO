using Datos.Contratos.VehiclesRegistration;
using Negocio.Contratos.VehiclesRegistration;
using System;
using System.Collections.Generic;
using System.Transactions;
using Transversales.Modelos.VehiclesRegistration;

namespace Negocio.General.VehiclesRegistration
{
    public class BillsAdministrator : IBillsAdministrator
    {
        #region Constructor
        public BillsAdministrator(IBillsRepository repositorio)
        {
            _repositorio = repositorio;
        }

        #endregion

        #region Variables
        private readonly IBillsRepository _repositorio;

        #endregion

        #region Methods
        public IEnumerable<Bills> GetAll()
        {
            return _repositorio.GetAll();
        }

        public void AddBill(Bills bill)
        {
            try
            {
                var lastId = _repositorio.GetLastId("BillId");

                using (var scope = new TransactionScope())
                {
                    _repositorio.ExecuteQuery($@"INSERT INTO [dbo].[Bills]
                                                 VALUES
                                                    ({lastId + 1}
                                                    ,'{bill.LicensePlate}'
                                                    ,{bill.ParkingCharged}
                                                    ,{bill.ParkingTime}
                                                    ,{bill.EntryTicketId}
                                                    ,{bill.DepartureTicketId}
                                                    ,{bill.Rate}
                                                    ,'{bill.EntryDate:yyyy-MM-dd HH:mm:ss}'
                                                    ,'{bill.DepartureDate:yyyy-MM-dd HH:mm:ss}')");

                    scope.Complete();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
