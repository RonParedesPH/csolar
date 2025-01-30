using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using reporting.repositories;
using reporting.helpers;

namespace reporting.unitofwork
{
    public interface IUnitOfWork
    {
        //IItemCategoryRepository ItemCategories { get; }
        //IItemRepository Items { get; }
        IRacerRepository Racers { get; }
        IRaceClassRepository RaceClass { get; }
        ITeamRepository Teams { get; }
        IRoundRepository Rounds { get; }
        IRegistrationRepository Registrations { get;  }


        ResultOfAction Commit();
    }
}
