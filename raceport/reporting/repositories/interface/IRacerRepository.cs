using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using reporting.entities;

namespace reporting.repositories
{

    public interface IRacerRepository
    {

        // ekleme
        // liste döndürecek
        Task<IEnumerable<Racer>> GetAllAsync();
        // id ile silme
        Task<Racer> FindAsync(string ulid);
        // isim ile arama
        Racer FindByName(string name);
        Racer FindByNumber(int number);
        IEnumerable<Racer> FindByCondition(string condition);
        // Güncelle
        IEnumerable<Racer> GetAll();
        Racer Find(string ulid);
        Task<string> AddAsync(Racer racer);
        Task<string> UpdateAsync(Racer racer);
        /*
         bu interface'den kalıtım alan bir repository oluşturmalıyız.  hepsini tek tek görün diye gezdim :)
         */
    }
}