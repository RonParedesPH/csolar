using reporting.unitofwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reporting.process
{
    
    public abstract class IWorksheetWorker
    {
        private readonly SpreadsheetGear.IWorksheet _worksheet;
        private readonly IUnitOfWork _unitOfWork;
        protected IWorksheetWorker(SpreadsheetGear.IWorksheet worksheet, IUnitOfWork unitofwork)
        {
            _worksheet = worksheet;
            _unitOfWork = unitofwork;
        }

        public abstract bool Render(string appName, string title, string subTitle, string key, string tabName);
    }
}
