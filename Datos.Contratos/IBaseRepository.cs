using System.Collections.Generic;

namespace Datos.Contratos
{
    public interface IBaseRepository<T> where T : class
    {
        IEnumerable<T> ExecuteQuery(string query);

        IEnumerable<object> ExecuteQueryObject(string query);

        IEnumerable<T> GetAll();

        IEnumerable<T> GetAll(string where);

        IEnumerable<T> GetAll(string columnas, string where);

        IEnumerable<T> GetAll(ref int count, string where, int pagina, int numRegistros, string order);

        object Add(T modelo);

        object Update(T modelo);

        object Delete(T modelo);
    }
}
