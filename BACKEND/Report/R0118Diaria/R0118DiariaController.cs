using System;
using System.Web.Http;
using System.Data;
using System.Collections.Generic;
using System.Web;
using Newtonsoft.Json;

namespace PROPOSTA
{
    public class R0118DiariaController : ApiController
    {
        //=================================Lista de Report
        [Route("api/R0118Diaria/GetFilter/{Key}")]
        [HttpGet]
        [ActionName("GetFilter")]
        [Authorize()]
        public IHttpActionResult GetFilter(String Key)
        {
            SimLib clsLib = new SimLib();
            Report Cls = new Report(User.Identity.Name);
            String sPath = HttpContext.Current.Server.MapPath("~/REPORT/JSON");
            if (sPath.Right(1) != @"\")
            {
                sPath += @"\";
            }
            string sFile = sPath + Key + "_FILTER.json";
            try
            {
                string allText = System.IO.File.ReadAllText(sFile);

                object jsonObject = JsonConvert.DeserializeObject(allText);
                return Ok(jsonObject);          
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }

      
        //=================================Gera Report
        [Route("api/R0118Diaria/Print")]
        [HttpPost]
        [ActionName("PrintReport")]
        [Authorize()]
        public IHttpActionResult PrintReport (R0118Diaria.ReportFilterModel Filter)
        {
            SimLib clsLib = new SimLib();
            R0118Diaria Cls = new R0118Diaria(User.Identity.Name);
            try
            {
                //DataTable dtbReport = Cls.ExecuteProc(Filter);
                String pdfFile = Cls.ImprimirRpt(Filter);
                return Ok(pdfFile);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }

        //=================================Gera Report
        [Route("api/R0118Diaria/LoadData")]
        [HttpPost]
        [ActionName("LoadData")]
        [Authorize()]
        public IHttpActionResult LoadData(R0118Diaria.ReportFilterModel Filter)
        {
            SimLib clsLib = new SimLib();
            R0118Diaria Cls = new R0118Diaria(User.Identity.Name);
            try
            {
                DataTable dtbReport = Cls.ReportLoadData(Filter);
                return Ok(dtbReport);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }

    }

}

