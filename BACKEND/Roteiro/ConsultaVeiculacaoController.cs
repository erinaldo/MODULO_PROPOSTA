using System;
using System.Web.Http;
using System.Data;
using System.Collections.Generic;

namespace PROPOSTA
{
    public class ConsultaVeiculacaoController : ApiController
    {
        //=================================Consulta de Veiculacoes
        [Route("api/ConsultaVeiculacaoGet")]
        [HttpPost]
        [ActionName("ConsultaVeiculacaoGet")]
        [Authorize()]


        public IHttpActionResult ConsultaVeiculacaoGet([FromBody]ConsultaVeiculacao.FiltroConsultaVeiculacaoModel pFiltro)
        {
            SimLib clsLib = new SimLib();
            ConsultaVeiculacao Cls = new ConsultaVeiculacao(User.Identity.Name);
            try
            {
                DataTable dtb = Cls.ConsultaVeiculacaoGet(pFiltro);
                return Ok(dtb);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }
        //=================================Consulta de Veiculacoes Mercha
        [Route("api/ConsultaVeiculacaoMerchaGet")]
        [HttpPost]
        [ActionName("ConsultaVeiculacaoMerchaGet")]
        [Authorize()]


        public IHttpActionResult ConsultaVeiculacaoMerchaGet([FromBody]ConsultaVeiculacao.FiltroConsultaVeiculacaoModel pFiltro)
        {
            SimLib clsLib = new SimLib();
            ConsultaVeiculacao Cls = new ConsultaVeiculacao(User.Identity.Name);
            try
            {
                DataTable dtb = Cls.ConsultaVeiculacaoMerchaGet(pFiltro);
                return Ok(dtb);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }
    }
}



