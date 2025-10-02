using System.Collections.Generic;
using System.Threading.Tasks;

namespace Front_Auth1.Interfaces
{
    public interface IClienteService
    {
        Task<IEnumerable<dynamic>> GetAll();         
        Task<dynamic> GetById(int id);              
        Task<bool> Create(dynamic entity);          
        Task<bool> Update(int id, dynamic entity);  
        Task<bool> Delete(int id);                  
    }
}
  
