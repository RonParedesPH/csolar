using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using reporting.entities;

namespace reporting.repositories
{

    public interface IRoundRepository
    {

        // ekleme
        // liste döndürecek
        Task<IEnumerable<Round>> GetAllAsync();
        // id ile silme
        Task<Round> FindAsync(string ulid);
        // isim ile arama
        Task<string> AddAsync(Round Round);
        Task<string> UpdateAsync(Round Round);
        Round FindByName(string name);
        IEnumerable<Round> FindByCondition(string condition);
        // Güncelle
        IEnumerable<Round> GetAll();
        Round Find(string ulid);
        /*
         * 
         bu interface'den kalıtım alan bir repository oluşturmalıyız.  hepsini tek tek görün diye gezdim :)
         */
    }
}