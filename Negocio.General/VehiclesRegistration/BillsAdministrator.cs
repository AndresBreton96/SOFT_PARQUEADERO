using Datos.Contratos.VehiclesRegistration;
using Negocio.Contratos.VehiclesRegistration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Transactions;
using Transversales.Modelos.VehiclesRegistration;
using Transversales.Utilitarios.Enums;
using Transversales.Utilitarios.Tools;

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

        public IEnumerable<Bills> GetAll(DateTime initialDate, DateTime finalDate, string licensePlate)
        {
            var query = $@"SELECT * 
                           FROM Bills 
                           WHERE CAST(BillDate AS date) >= '{initialDate:yyyy/MM/dd}' AND CAST(BillDate AS date) < '{finalDate:yyyy/MM/dd}'
                                 AND LicensePlate LIKE '%{licensePlate.Replace(' ', '%')}%'";

            var bills = _repositorio.ExecuteQuery(query);
            if (bills.Any())
                return bills;
            return new List<Bills>();
        }

        public void AddBill(Bills bill)
        {
            try
            {
                var lastId = _repositorio.GetLastId("BillId");
                var consecutive = _repositorio.GetConsecutive("BillId");
                var maxConsecutive = _repositorio.ExecuteQueryInt($"SELECT Value FROM LocalParameters WHERE Id = {Convert.ToInt32(ResourcesReader.GetProperty("LocalParameters",LocalParameterEnum.MaximumConsecutive.ToString()))}");

                if (consecutive >= maxConsecutive)
                    consecutive = 0;

                using (var scope = new TransactionScope())
                {
                    _repositorio.ExecuteQuery($@"INSERT INTO [dbo].[Bills]
                                                 VALUES
                                                    ({lastId + 1}
                                                    ,{consecutive + 1}
                                                    ,'{bill.LicensePlate}'
                                                    ,{bill.ParkingCharged}
                                                    ,{bill.ParkingTime}
                                                    ,{bill.EntryTicketId}
                                                    ,{bill.DepartureTicketId}
                                                    ,{bill.Rate}
                                                    ,'{bill.EntryDate:yyyy-MM-dd HH:mm:ss}'
                                                    ,'{bill.DepartureDate:yyyy-MM-dd HH:mm:ss}'
                                                    ,GETDATE())");

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
