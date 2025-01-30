
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

    public class ReportsController : BaseController
    {
        //public ReportsController(IUnitOfWork unitOfWork): base(unitOfWork)
        //{

        //}
        public ReportsController() : base(new UnitOfWork(
            ConfigurationManager.ConnectionStrings["racepository"].ConnectionString
            ))
        {

        }

        // GET api/<controller>
        [HttpGet]
        [Route("api/reports/racers_master")]
        public HttpResponseMessage Get()
        {
            RacersMaster proc = new RacersMaster(_unitOfWork);
            string result = proc.DoGenerate();
            if (result.Contains("Failure"))
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, result);
            else
                return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        //[HttpPost]
        //[Route("api/reports/racers_master")]
        //public async Task<IHttpActionResult> Racers_Master()
        //{
        //    RacersMaster proc = new RacersMaster(_unitOfWork);
        //    ProcStubDto stub = await proc.Run();

        //    return Ok(stub);

        //}
        [HttpPost]
        [Route("api/reports/round_result")]
        public HttpResponseMessage Round_Result([FromBody] string roundId)
        {
            RoundResult proc = new RoundResult(_unitOfWork);
            string result = proc.DoGenerate(roundId);
            if (result.Contains("Failure"))
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, result);
            else
                return Request.CreateResponse(HttpStatusCode.OK, result);
        }



    }
}