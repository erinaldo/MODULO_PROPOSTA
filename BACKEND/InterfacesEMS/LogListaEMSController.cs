using System;
using System.Web.Http;
using System.Data;
using System.Collections.Generic;
namespace PROPOSTA
{
    public class LogListaController : ApiController
    {
        //=================================Lista de Log de Lista
        [Route("api/CarregarLogListaEMS")]
        [HttpPost]
        [ActionName("CarregarLogListaEMS")]
        [Authorize()]


        public IHttpActionResult CarregarLogListaEMS([FromBody]LogListaEMS.LogListaEMSModel filtro)
        {
            SimLib clsLib = new SimLib();
            LogListaEMS Cls = new LogListaEMS((User.Identity.Name));
            try
            {

                List<LogListaEMS.LogListaEMSModel> Retorno = Cls.CarregarLogListaEMS(filtro);
                return Ok(Retorno);

            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }
               
    }
}

