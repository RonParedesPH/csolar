using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using reporting.entities;

namespace reporting.repositories
{

    public interface ITeamRepository
    {

        // ekleme
        // liste döndürecek
        Task<IEnumerable<Team>> GetAllAsync();
        // id ile silme
        Task<Team> FindAsync(string ulid);
        // isim ile arama
        Task<string> AddAsync(Team team);
        Task<string> UpdateAsync(Team team);
        Team FindByName(string name);
        IEnumerable<Team> FindByCondition(string condition);
        // Güncelle
        IEnumerable<Team> GetAll();
        Team Find(string ulid);
        /*
         * 
         bu interface'den kalıtım alan bir repository oluşturmalıyız.  hepsini tek tek görün diye gezdim :)
         */
    }
}