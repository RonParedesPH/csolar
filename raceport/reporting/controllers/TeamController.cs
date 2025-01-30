
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

    public class TeamController : BaseController
    {
        //public TeamController(IUnitOfWork unitOfWork): base(unitOfWork)
        //{

        //}
        public TeamController() : base(new UnitOfWork(
            ConfigurationManager.ConnectionStrings["racepository"].ConnectionString
            ))
        {

        }

        // GET api/<controller>
        [HttpGet]
        public IHttpActionResult Get()
        {
            return Ok(_unitOfWork.Teams.GetAll());
        }

        [HttpPost]
        [Route("api/teams")]
        public IHttpActionResult Teams()
        {
            IEnumerable<Team> Team = _unitOfWork.Teams.GetAll();
            return Ok(Team);
        }


    }
}