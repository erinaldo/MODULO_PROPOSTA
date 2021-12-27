using System;
using System.Web.Http;
using System.Data;
using System.Collections.Generic;
namespace PROPOSTA
{
    public class RotateController : ApiController
    {
        //===========================Carregar Dados do Contrato
        [Route("api/Rotate/CarregarDados")]
        [HttpPost]
        [ActionName("CarregarDados")]
        [Authorize()]

        public IHttpActionResult CarregarDados([FromBody] Rotate.FiltroModel Param)
        {
            SimLib clsLib = new SimLib();
            Rotate Cls = new Rotate(User.Identity.Name);
            try
            {
                Rotate.DeterminacaoModel retorno = Cls.CarregarDados(Param);
                return Ok(retorno);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }
        //===========================Gravar novo Comercial
        [Route("api/Rotate/SalvarComercial")]
        [HttpPost]
        [ActionName("SalvarComercial")]
        [Authorize()]

        public IHttpActionResult SalvarComercial([FromBody] Rotate.DeterminacaoComercialModel Param)
        {
            SimLib clsLib = new SimLib();
            Rotate Cls = new Rotate(User.Identity.Name);
            try
            {
                DataTable retorno = Cls.SalvarComercial(Param);
                return Ok(retorno);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }

        //===========================Analisar Rotate
        [Route("api/Rotate/AnalisarRotate")]
        [HttpPost]
        [ActionName("AnalisarRotate")]
        [Authorize()]
        public IHttpActionResult AnalisarRotate([FromBody] Rotate.DeterminacaoModel Param)
        {
            SimLib clsLib = new SimLib();
            Rotate Cls = new Rotate(User.Identity.Name);
            try
            {
                List<Rotate.AnaliseRotateModel> retorno = Cls.AnalisarRotate(Param);
                return Ok(retorno);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }
        //===========================Salvar Determinacao
        [Route("api/Rotate/SalvarDeterminacao")]
        [HttpPost]
        [ActionName("SalvarDeterminacao")]
        [Authorize()]
        public IHttpActionResult SalvarDeterminacao([FromBody] Rotate.DeterminacaoModel Param)
        {
            SimLib clsLib = new SimLib();
            Rotate Cls = new Rotate(User.Identity.Name);
            try
            {
                DataTable retorno = Cls.SalvarDeterminacao(Param);
                return Ok(retorno);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }
    }
}