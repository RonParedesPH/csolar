
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

    public class RoundController : BaseController
    {
        //public RoundController(IUnitOfWork unitOfWork): base(unitOfWork)
        //{

        //}
        public RoundController() : base(new UnitOfWork(
            ConfigurationManager.ConnectionStrings["racepository"].ConnectionString
            ))
        {

        }

        // GET api/<controller>
        [HttpGet]
        public IHttpActionResult Get()
        {
            return Ok(_unitOfWork.Rounds.GetAll());
        }

        [HttpPost]
        [Route("api/rounds")]
        public IHttpActionResult Rounds()
        {
            IEnumerable<Round> Round = _unitOfWork.Rounds.GetAll();
            return Ok(Round);
        }


    }
}