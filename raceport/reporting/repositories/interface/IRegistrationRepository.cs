using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using reporting.entities;

namespace reporting.repositories
{

    public interface IRegistrationRepository
    {

        // ekleme
        // liste döndürecek
        Task<IEnumerable<Registration>> GetAllAsync();
        // id ile silme
        Task<Registration> FindAsync(string ulid);
        // isim ile arama
        Task<string> AddAsync(Registration Registration);
        Task<string> UpdateAsync(Registration Registration);
        Registration FindByName(string name);
        IEnumerable<Registration> FindByCondition(string condition);
        // Güncelle
        IEnumerable<Registration> GetAll();
        IEnumerable<Registration> GetItemsByRoundId(string id);
        Registration Find(string ulid);
        /*
         * 
         bu interface'den kalıtım alan bir repository oluşturmalıyız.  hepsini tek tek görün diye gezdim :)
         */
    }
}