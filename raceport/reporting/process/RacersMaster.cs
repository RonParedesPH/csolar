using logger;
using System;
using System.Web.Hosting;
using reporting.unitofwork;
using reporting.helpers;
using reporting.dtos;
using reporting.entities;
using System.Collections.Generic;
using System.Linq;

namespace reporting.process
{
    public class RacersMaster
    {
        private IUnitOfWork _unitOfWork;

        public RacersMaster(IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
        }

        public string DoGenerate()
        {
            //Process(stub);
            return RawDogIt();
        }

        public string Validate(string filename)
        {
            return DogCheck(filename);
        }

        public MasterRawDto DoReadIt(string filename) {
            return DogDoneIt(filename);
        }



        //public async Task<ProcStubDto> Run() {
        //    ProcStubDto stub = await ProcStubClient.createStub("RacersMaster");

        //    var t = Task.Run(() => Process(stub));
        //    return stub;
        //}

       //private async Task Process( ProcStubDto stub) {
       //     SpreadsheetGear.IWorkbook workbook;
       //     SpreadsheetGear.IWorksheet worksheet;
       //     int line = 0;

       //     IEnumerable<Racer> racers = await _unitOfWork.Racers.GetAllAsync();

       //     workbook = SpreadsheetGear.Factory.GetWorkbook();
       //     worksheet = workbook.Worksheets["Sheet1"];
       //     if (worksheet == null)
       //     {
       //         workbook.Worksheets.Add();
       //         worksheet = workbook.Worksheets["Sheet1"];
       //     }
       //     var cells = worksheet.Cells;
       //     var r = cells["A1"];
       //     line += _prepareWorksheet(worksheet, "Racers Master", "");

       //     List<Racer> racersList = racers.OrderBy(c => c.Number).ToList();

       //     stub.Stage = "Processing records...";
       //     string val = "";
       //     int x = 0;
       //     foreach (Racer racer in racersList)
       //     {              

       //         foreach (columnDefs icol in _reportColumns)
       //         {
       //             val = _columnToValue(icol.id, racer);
       //             r = worksheet.Cells[reportHelper.cell(icol.pos, line)];
       //             _assignCellValue(r, icol, val);
       //         }
       //         line++;

       //         //foreach (SalesOrderDetail detail in order.SalesOrderDetails) {
       //         //    foreach (columnDefs icol in _lineColumns)
       //         //    {
       //         //        val = _columnToValue(icol.id, detail);
       //         //        r = worksheet.Cells[reportHelper.cell(icol.pos, line)];
       //         //        _assignCellValue(r, icol, val);
       //         //    }
       //         //    line++;
       //         //}

       //         x++;

       //         stub.Progress = Convert.ToInt32((decimal)x / racersList.Count() * 100);
       //         stub = await ProcStubClient.updateStub(stub);
       //     }
       //     line += 2;
       //     r = cells[line, 0];
       //     r.Formula = racersList.Count() + " record(s) found.";

       //     worksheet.Name = "Racers";
       //     worksheet.Protect("1hellUvaK3y!");

       //     string filename = "RacersMaster_" + UlidFormatter.ToString(Ulid.NewUlid().ToString()) + ".xls";
       //     try
       //     {
       //         workbook.SaveAs(HostingEnvironment.MapPath("/") + "/reports/" + filename,
       //             SpreadsheetGear.FileFormat.OpenXMLWorkbook);
       //         stub.Stage = "Complete";
       //         stub.Output = "/reports/" + filename;
       //    }
       //     catch (Exception ex)
       //     {
       //         log.LogMessage(log.TracingLevel.ERROR, ExceptionHelper.Verbose(ex));
       //         stub.Stage = "Failed to save";
       //     }

       //     stub = await ProcStubClient.updateStub(stub);
       // }

        private string RawDogIt()
        {
            SpreadsheetGear.IWorkbook workbook;
            SpreadsheetGear.IWorksheet worksheet;

            workbook = SpreadsheetGear.Factory.GetWorkbook();
            worksheet = workbook.Worksheets["Sheet1"];
            if (worksheet == null)
            {
                workbook.Worksheets.Add();
                worksheet = workbook.Worksheets["Sheet1"];
            }

            IWorksheetWorker render = new RacersMasterRender(worksheet, _unitOfWork);
            render.Render("RaceMaster", "Master List", "List of all active race drivers", "", "");
            worksheet.Name = "MasterList";
            worksheet.Protect("1hellUvaK3y!");

            List<Round> roundsList = _unitOfWork.Rounds.GetAll().ToList();
            int n = 50; // unicode of '2'
            foreach (Round round in roundsList)
            {
                workbook.Worksheets.Add();
                worksheet = workbook.Worksheets["Sheet" + Convert.ToChar(n)];

                render = new RegistrationRender(worksheet, _unitOfWork);
                render.Render("RaceMaster", 
                    "Drivers List -" + round.Name, 
                    "List of registered participants",
                    round.Id, round.Name);
                worksheet.Name = round.Name;
                worksheet.Protect("1hellUvaK3y!");
                n++;
            };

            workbook.Worksheets[0].Select();


            string filename = "Racemaster_" + 
                DateTime.UtcNow.ToString("yyyyMMdd_hhmm_ss") +
                UlidFormatter.ToString(Ulid.NewUlid().ToString()) + ".xls";
            try
            {
                workbook.SaveAs(HostingEnvironment.MapPath("/") + "/reports/" + filename,
                    SpreadsheetGear.FileFormat.OpenXMLWorkbook);
                //stub.Stage = "Complete";
                //stub.Output = "/reports/" + filename;
            }
            catch (Exception ex)
            {
                log.LogMessage(log.TracingLevel.ERROR, ExceptionHelper.Verbose(ex));
                return "Failure:" + ExceptionHelper.Verbose(ex);
                //stub.Stage = "Failed to save";
            }

            //stub = await ProcStubClient.updateStub(stub);
            return "reports/" + filename;
        }


        private MasterRawDto DogDoneIt(string filename)
        {
            SpreadsheetGear.IWorksheet worksheet;

            MasterRawDto result = new MasterRawDto() { errorMessage = "No content"};

            using (SpreadsheetGear.IWorkbookSet workbookset = SpreadsheetGear.Factory.GetWorkbookSet())
            {
                SpreadsheetGear.IWorkbook workbook = null;

                try
                {
                    workbook = workbookset.Workbooks.Open(filename);
                    worksheet = workbook.Worksheets[0];

                }
                catch
                {
                    result.errorMessage = "Invalid file format detected.";
                    return result;
                }

                RacersMasterReader reader = new RacersMasterReader(worksheet, _unitOfWork);
                result.errorMessage = "";
                result.RacerRaw = reader.ReadRacer();
                result.ClassRaw = reader.ReadClass();
                result.TeamRaw = reader.ReadTeam();
                workbook.Close();

            }

            return result;
            }
        





        private string DogCheck(string filename)
        {
            string result = "";

            using (SpreadsheetGear.IWorkbookSet workbookset = SpreadsheetGear.Factory.GetWorkbookSet())
            {
                SpreadsheetGear.IWorkbook workbook = null;
                SpreadsheetGear.IWorksheet worksheet;

                try
                {
                    workbook = workbookset.Workbooks.Open(filename);
                    worksheet = workbook.Worksheets[0];

                    result = CheckWorksheetHeaders(worksheet);
                    workbook.Close();                }
                catch
                {
                    result = "Invalid file format detected.";
                }


            }

            return result;
        }

        private string CheckWorksheetHeaders(SpreadsheetGear.IWorksheet worksheet)
        {
            columnDefSet defSet = DefinedSetsOfColumnDefs.RaceSet();
            bool valid = true;

            int i = defSet.ColumnOffset -1;
            foreach (columnDefs def in defSet.columnList)
            {
                if (!def.title.Equals(worksheet.Cells[defSet.HeaderRow -1, i].Formula))
                    valid = false;
                i++;
            }

            return (!valid ? "Columns not matching to expected worksheet contents." : "");
        }

    }
}
