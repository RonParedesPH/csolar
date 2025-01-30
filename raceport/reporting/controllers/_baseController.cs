
namespace reporting.controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Http;
    using reporting.unitofwork;


    public class BaseController : ApiController
    {
        //public IUnitOfWork _unitOfWork { get; set; }
        protected IUnitOfWork _unitOfWork;


        //public BaseController()
        //{
        //    _unitOfWork = new UnitOfWork(new stockroomContext());
        //}
        public BaseController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}