using System.Collections.Generic;
using System.Threading.Tasks;

namespace Front_Auth1.Interfaces
{
    public interface IMarcaService
    {
        Task<IEnumerable<object>> GetAll();
        Task<object> GetById(int id);
        Task<bool> Create(object entity);
        Task<bool> Update(int id, object entity);
        Task<bool> Delete(int id);
    }
}

