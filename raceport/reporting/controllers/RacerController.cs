
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

    public class RacerController : BaseController
    {
        //public RacerController(IUnitOfWork unitOfWork): base(unitOfWork)
        //{

        //}
        public RacerController() : base(new UnitOfWork(
            ConfigurationManager.ConnectionStrings["racepository"].ConnectionString
            ))
        {

        }

        // GET api/<controller>
        [HttpGet]
        public IHttpActionResult Get()
        {
            return Ok(_unitOfWork.Racers.GetAll());
        }

        [HttpPost]
        [Route("api/racers")]
        public IHttpActionResult Racers()
        {
            IEnumerable<Racer> racers = _unitOfWork.Racers.GetAll();
            return Ok(racers);
        }


    }
}