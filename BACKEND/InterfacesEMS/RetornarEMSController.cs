using System;
using System.Web.Http;
using System.Data;
using System.Collections.Generic;
using System.Net;

namespace PROPOSTA
{
    public class RetornarEMSController : ApiController
    {
        //==================================Processa Integrações de Retorno do EMS
        [Route("api/ProcessarRetorno")]
        [HttpPost]
        [ActionName("ProcessarRetorno")]
        [Authorize()]
        public IHttpActionResult ProcessarRetorno([FromBody] RetornarEMS.RetornarEMSModel Param)
        {
            SimLib clsLib = new SimLib();
            RetornarEMS Cls = new RetornarEMS((User.Identity.Name));
            RetornarEMS.ResponseModel Retorno = new RetornarEMS.ResponseModel();
            Retorno.IsSuccessful = true;
            Retorno.Message = "Integração(ções) de Retorno Concluída(s) com Sucesso";
            try
            {
                Cls.ProcessarRetorno(Param);

            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                Retorno.IsSuccessful = false;
                Retorno.Message = Ex.Message;
                //throw new Exception(Ex.Message);
                
            }
            finally
            {
                
            }
            return Ok(Retorno);
            
            
        }


    }
}

