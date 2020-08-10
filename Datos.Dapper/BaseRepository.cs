using Dapper;
using Datos.Contratos;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace Datos.Dapper.Users
{
    public class BaseRepository<T> : SPRepository<T>, IBaseRepository<T> where T : class
    {
        public BaseRepository()
        {
        }

        public IEnumerable<T> ExecuteQuery(string query)
        {
            return _cnn.Query<T>(query);
        }

        public IEnumerable<object> ExecuteQueryObject(string query)
        {
            return _cnn.Query<object>(query);
        }

        public IEnumerable<T> GetAll()
        {
            return _cnn.GetList<T>();
        }

        public IEnumerable<T> GetAll(string where)
        {
            return _cnn.GetList<T>(where);
        }

        public IEnumerable<T> GetAll(string columnas, string where)
        {
            return _cnn.Query<T>($"SELECT {columnas} FROM {typeof(T).Name} {where}");
        }

        public IEnumerable<T> GetAll(ref int count, string where, int pagina, int numRegistros, string order)
        {
            count = _cnn.Query<int>($"SELECT ISNULL(Count(1),0) FROM {typeof(T).Name}").First();
            return _cnn.GetListPaged<T>(pagina, numRegistros, where, order);
        }

        public object Add(T modelo)
        {
            return _cnn.Insert<T>(modelo);
        }

        public object Update(T modelo)
        {
            return _cnn.Update<T>(modelo);
        }

        public object Delete(T modelo)
        {
            return _cnn.Delete<T>(modelo);
        }
    }
}
