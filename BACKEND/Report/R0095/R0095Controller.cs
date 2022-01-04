using System;
using System.Web.Http;
using System.Data;
using System.Collections.Generic;
using System.Web;
using Newtonsoft.Json;

namespace PROPOSTA
{
    public class R0095Controller : ApiController
    {
        //=================================Lista de Report
        [Route("api/R0095/GetFilter/{Key}")]
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
        [Route("api/R0095/Print")]
        [HttpPost]
        [ActionName("PrintReport")]
        [Authorize()]
        public IHttpActionResult PrintReport (R0095.ReportFilterModel Filter)
        {
            SimLib clsLib = new SimLib();
            R0095 Cls = new R0095(User.Identity.Name);
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
        [Route("api/R0095/LoadData")]
        [HttpPost]
        [ActionName("LoadData")]
        [Authorize()]
        public IHttpActionResult LoadData(R0095.ReportFilterModel Filter)
        {
            SimLib clsLib = new SimLib();
            R0095 Cls = new R0095(User.Identity.Name);
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

