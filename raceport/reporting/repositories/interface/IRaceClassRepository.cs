using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using reporting.entities;

namespace reporting.repositories
{

    public interface IRaceClassRepository
    {

        // ekleme
        // liste döndürecek
        Task<IEnumerable<RaceClass>> GetAllAsync();
        // id ile silme
        Task<RaceClass> FindAsync(string ulid);
        Task<string> AddAsync(RaceClass raceClass);
        Task<string> UpdateAsync(RaceClass raceClass);
        // isim ile arama
        RaceClass FindByCode(string code);
        IEnumerable<RaceClass> FindByCondition(string condition);
        // Güncelle
        IEnumerable<RaceClass> GetAll();
        RaceClass Find(string ulid);
        /*
         bu interface'den kalıtım alan bir repository oluşturmalıyız.  hepsini tek tek görün diye gezdim :)
         */
    }
}