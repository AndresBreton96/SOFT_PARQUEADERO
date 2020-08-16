using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Datos.Dapper
{
    public class SPRepository<T> where T : class
    {
        #region Constructor
        public SPRepository()
        {
            _cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["DataBaseConn"].ConnectionString);
        }

        #endregion

        #region Variables
        internal readonly SqlConnection _cnn;

        #endregion

        #region Methods
        public async Task<IEnumerable<T>> ExecuteSP(string nombre, object parametros)
        {
            try
            {
                _cnn.Open();
                var rta = await _cnn.QueryAsync<T>(nombre, parametros, commandType: System.Data.CommandType.StoredProcedure);
                _cnn.Close();
                return rta;
            }
            catch (Exception ex)
            {
                _cnn.Close();
                return default;
            }
        }

        public IEnumerable<IEnumerable<object>> EjecutarSP(string nombre, object parametros, int respuestas)
        {
            try
            {
                var retorno = new List<IEnumerable<object>>();

                _cnn.Open();
                var rta = _cnn.QueryMultiple(nombre, parametros, commandType: System.Data.CommandType.StoredProcedure);
                for (int i = 0; i < respuestas; i++)
                {
                    retorno.Add(rta.Read<object>());
                }
                _cnn.Close();

                return retorno;
            }
            catch (Exception ex)
            {
                _cnn.Close();
                return default;
            }
        }

        #endregion
    }
}
