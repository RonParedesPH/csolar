
namespace reporting.controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;

    using reporting.entities;
    using reporting.unitofwork;
    using reporting.context;
    using System.Configuration;
    using reporting.process;
    using Dapper;

    public class ClassController : BaseController
    {
        //public ClassController(IUnitOfWork unitOfWork): base(unitOfWork)
        //{

        //}
        public ClassController() : base(new UnitOfWork(
            ConfigurationManager.ConnectionStrings["racepository"].ConnectionString
            ))
        {

        }

        // GET api/<controller>
        [HttpGet]
        public IHttpActionResult Get()
        {
            return Ok(_unitOfWork.RaceClass.GetAll());
        }

        [HttpPost]
        [Route("api/Classes")]
        public IHttpActionResult Classs()
        {
            IEnumerable<RaceClass> Class = _unitOfWork.RaceClass.GetAll();
            return Ok(Class);
        }


    }
}