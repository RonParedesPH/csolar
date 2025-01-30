
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
    using System.Web;
    using System.IO;
    using reporting.dtos;
    using System.Web.Http.Results;

    public class ProcessController : BaseController
    {
        //public ProcessController(IUnitOfWork unitOfWork): base(unitOfWork)
        //{

        //}
        public ProcessController() : base(new UnitOfWork(
            ConfigurationManager.ConnectionStrings["racepository"].ConnectionString
            ))
        {

        }

        [HttpPost]
        [Route("api/process/extract_master")]
        public HttpResponseMessage Racers_Master()
        {
            //RacersMaster proc = new RacersMaster(_unitOfWork);
            //ProcStubDto stub = await proc.Run();

            //return Ok(stub);
            string result = "No operation.";

            HttpFileCollection hfc = HttpContext.Current.Request.Files;
            if (hfc.Count == 0)
                result = "A file is required to be sent with the request.";
            else
            {
                if (hfc.Count > 1)
                    result = "Multiple files were sent when only one file is required.";
                else
                {

                    HttpPostedFile hpf = hfc[0];    // use only the first
                    if (hpf.ContentLength > 0)
                    {
                        string path = System.Web.Hosting.HostingEnvironment.MapPath("~/uploads/");
                        if (!Directory.Exists(path))
                            result = "Server configuration error, upload directory cannot be located.";
                        else
                        {
                            string filename = path + Path.GetFileName(hpf.FileName);
                            if (File.Exists(filename))
                                File.Delete(filename);

                            hpf.SaveAs(filename);
                            if (File.Exists(filename))
                            {
                                //RacersMasterReader reader = new RacersMasterReader()
                                RacersMaster racer = new RacersMaster(_unitOfWork);
                                result = racer.Validate(filename);
                                if (result == string.Empty)
                                {
                                    MasterRawDto masterDto = racer.DoReadIt(filename);
                                    if (masterDto.errorMessage == string.Empty)
                                    {
                                        return Request.CreateResponse(HttpStatusCode.OK, masterDto);
                                    }
                                    else
                                        result = masterDto.errorMessage;
                                }
                            }
                        }
                    }
                    else
                        result = "Zero-length or empty file attached.";
                }
            }

            return Request.CreateResponse(HttpStatusCode.ExpectationFailed, result);
        }

        [HttpPost]
        [Route("api/process/verify_master")]
        public HttpResponseMessage Verify_Master([FromBody] MasterRawDto rawMaster)
        {
            string result = string.Empty;
            string validTeams = string.Empty;
            string validRaceClass = string.Empty;
            if (rawMaster == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest, "null value submitted to POST:verify_master");

            List<Team> teams = _unitOfWork.Teams.GetAll().ToList();
            teams.ForEach(c =>
            {
                validTeams += c.Name + ';';
            });

            List<RaceClass> raceClasses = _unitOfWork.RaceClass.GetAll().ToList();
            raceClasses.ForEach(c =>
            {
                validRaceClass += c.Code + ';';
            });

            Team team = new Team();
            rawMaster.TeamRaw.ForEach(c =>
            {
                string failReason = string.Empty;
                string warnReason = string.Empty;

                if (c.Id != string.Empty) //validate as existing
                {
                    team = _unitOfWork.Teams.Find(c.Id);
                    DateTime lastmodified = DateTime.Parse(c.dtlastmodified);

                    if (c.Name == string.Empty)
                        failReason = "Cannot set an existing team name as blank";
                    if (c.Description == string.Empty)
                        failReason = "Description cannot be emptry";
                    if (lastmodified < team.dtlastmodified)
                        warnReason = "Record has been modified and might overwrite previous change.";
                }
                else // validate as new
                {
                    if (validTeams.Contains(c.Name))
                        failReason = "Another team is already using this name";
                    if (c.Name == string.Empty)
                        failReason = "Team name cannot be blank";
                }
                if (warnReason != string.Empty)
                    c.lastResult = "Warning: " + warnReason;
                if (failReason != string.Empty)
                    c.lastResult = "Error: " + failReason;
                else
                    validTeams += c.Name + ";";
            });

            RaceClass raceClass = new RaceClass();
            rawMaster.ClassRaw.ForEach(c =>
            {
                string failReason = string.Empty;
                string warnReason = string.Empty;

                if (c.Id != string.Empty) //validate as existing
                {
                    raceClass = _unitOfWork.RaceClass.Find(c.Id);
                    DateTime lastmodified = DateTime.Parse(c.dtlastmodified);

                    if (c.Code == string.Empty)
                        failReason = "Cannot set an existing Race class code as blank";
                    if (c.Description == string.Empty)
                        failReason = "Description cannot be emptry";
                    if (lastmodified < team.dtlastmodified)
                        warnReason = "Record has been modified and might overwrite previous change.";

                }
                else // validate as new
                {
                    if (validRaceClass.Contains(c.Code))
                        failReason = "Another Race class is already using this name";
                    if (c.Code == string.Empty)
                        failReason = "Race class code cannot be blank";
                }
                if (warnReason != string.Empty)
                    c.lastResult = "Warning: " + warnReason;
                if (failReason != string.Empty)
                    c.lastResult = "Error: " + failReason;
                else
                    validRaceClass += c.Code + ";";
            });

            Racer racer = new Racer();            
            rawMaster.RacerRaw.ForEach(c =>
            {

                string failReason = string.Empty;
                string warnReason = string.Empty;

                if (c.Id != string.Empty)
                {
                    racer = _unitOfWork.Racers.Find(c.Id);
                    DateTime lastmodified = DateTime.Parse(c.dtlastmodified);
                    if (c.Name == string.Empty)
                        failReason = "Cannot set racer name to emptry";
                    if (c.CarDetails == string.Empty)
                        warnReason = "Car details is suggested not to be emptry";
                    if (lastmodified < racer.dtlastmodified)
                        warnReason = "Record has been modified and might overwrite previous change.";

                    racer = _unitOfWork.Racers.FindByNumber(Int32.Parse(c.Number));
                    if (racer!=null && racer.Id != c.Id)
                        failReason = "Cannot change number to another which is in use";
                }
                else
                {
                    if (c.Name == string.Empty)
                        failReason = "Racer name should not be emptry";
                    if (c.CarDetails == string.Empty)
                        warnReason = "Car details is suggested not to be emptry";

                    racer = _unitOfWork.Racers.FindByName(c.Name);
                    if (racer != null)
                        failReason = "Racer is already existing in the database";
                    else
                    {
                        racer = _unitOfWork.Racers.FindByNumber(Int32.Parse(c.Number));
                        if (racer != null)
                            failReason = "Number is already assigned";
                    }
                }
                if (c.Team != string.Empty)
                {
                    if (validTeams.IndexOf(c.Team + ";") < 0)
                    {
                        failReason = "Team is not a valid team code.";
                    }
                }
                if (c.Team != string.Empty)
                {
                    if (validTeams.IndexOf(c.Team + ";") < 0)
                    {
                        failReason = "Race Class is not a valid team code.";
                    }
                }

                if (warnReason != string.Empty)
                    c.lastResult = "Warning: " + warnReason;
                if (failReason != string.Empty)
                    c.lastResult = "Error: " + failReason;
            }
            );

            if (result != string.Empty)
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, result);
            else
                return Request.CreateResponse(HttpStatusCode.OK, rawMaster);
        }


        [HttpPost]
        [Route("api/process/post_master")]
        public async Task<HttpResponseMessage> Post_Master([FromBody] MasterRawDto rawMaster)
        {
            if (rawMaster == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest, "null value submitted to POST:verify_master");

            string ret = string.Empty;

            for (int i = 0; i < rawMaster.TeamRaw.Count; i++)
            {
                bool upd = rawMaster.TeamRaw[i].Id.Length > 0;

                Team r = new Team();
                if (upd)
                {
                    r = _unitOfWork.Teams.Find(rawMaster.TeamRaw[i].Id);
                    r.dtlastmodified = DateTime.Now;
                }
                else
                {
                    r.Id = helpers.UlidFormatter.ToString(Ulid.NewUlid().ToString());
                    r.dtcreated = DateTime.UtcNow;
                }
                r.Name = rawMaster.TeamRaw[i].Name;
                r.Description = rawMaster.TeamRaw[i].Description;            
                    
                try
                {
                    ret = upd ?
                        await _unitOfWork.Teams.UpdateAsync(r) :
                        await _unitOfWork.Teams.AddAsync(r);

                    rawMaster.TeamRaw[i].Id = ret;
                    rawMaster.TeamRaw[i].lastResult = upd ? "Updated." : "Saved";
                }
                catch (Exception e)
                {
                    rawMaster.TeamRaw[i].lastResult = "Error: " + helpers.ExceptionHelper.Verbose(e);
                }
            }
            // --------------------------------------------------

            for (int i = 0; i < rawMaster.ClassRaw.Count; i++)
            {
                bool upd = rawMaster.ClassRaw[i].Id.Length > 0;

                RaceClass r = new RaceClass();
                if (upd)
                {
                    r = _unitOfWork.RaceClass.Find(rawMaster.ClassRaw[i].Id);
                    //r.dtlastmodified = DateTime.Now;
                    if (r!=null)
                        r.dtlastmodified = DateTime.Now;
                }
                else
                {
                    r.Id = helpers.UlidFormatter.ToString(Ulid.NewUlid().ToString());
                    r.dtcreated = DateTime.UtcNow;
                }
                r.Code = rawMaster.ClassRaw[i].Code;
                r.Description = rawMaster.ClassRaw[i].Description;

                try
                {
                    ret = upd ?
                        await _unitOfWork.RaceClass.UpdateAsync(r) :
                        await _unitOfWork.RaceClass.AddAsync(r);

                    rawMaster.ClassRaw[i].Id = ret;
                    rawMaster.ClassRaw[i].lastResult = upd ? "Updated." : "Saved";
                }
                catch (Exception e)
                {
                    rawMaster.ClassRaw[i].lastResult = "Error: " + helpers.ExceptionHelper.Verbose(e);
                }
            }

            // --------------------------------------------------
            for (int i = 0; i < rawMaster.RacerRaw.Count; i++)
            {
                bool upd = rawMaster.RacerRaw[i].Id.Length > 0;
                int numbe = int.Parse(rawMaster.RacerRaw[i].Number);

                Racer r = new Racer();
                if (upd) {
                    r = _unitOfWork.Racers.Find(rawMaster.RacerRaw[i].Id);
                    r.dtlastmodified = DateTime.Now;
                }
                else
                {
                    r.Id = helpers.UlidFormatter.ToString(Ulid.NewUlid().ToString());
                    r.dtcreated = DateTime.UtcNow;
                }                
                r.Name = rawMaster.RacerRaw[i].Name;
                r.Number = numbe;
                r.CarDetails = rawMaster.RacerRaw[i].CarDetails;
                r.RaceClass = rawMaster.RacerRaw[i].RaceClass;
                r.TeamIdOptional = rawMaster.RacerRaw[i].Team;


                try
                {
                    ret = upd ?
                        await _unitOfWork.Racers.UpdateAsync(r) :
                        await _unitOfWork.Racers.AddAsync(r);

                    rawMaster.RacerRaw[i].Id = ret;
                    rawMaster.RacerRaw[i].lastResult = upd ? "Updated." : "Saved";
                }
                catch (Exception e)
                {
                    rawMaster.RacerRaw[i].lastResult = "Error: " + helpers.ExceptionHelper.Verbose(e);
                }
            }
            return Request.CreateResponse(HttpStatusCode.OK, rawMaster);

        }
    }    
}