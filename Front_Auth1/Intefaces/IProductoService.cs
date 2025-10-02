using System.Collections.Generic;
using System.Threading.Tasks;

namespace Front_Auth1.Interfaces
{
    public interface IProductoService
    {
        Task<IEnumerable<dynamic>> GetAll();         // GET
        Task<dynamic> GetById(int id);              // GET por ID
        Task<bool> Create(dynamic entity);          // POST
        Task<bool> Update(int id, dynamic entity);  // PUT
        Task<bool> Delete(int id);                  // DELETE
    }
}

