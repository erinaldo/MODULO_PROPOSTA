using System;
using System.Web.Http;
using System.Data;
using System.Collections.Generic;
namespace PROPOSTA
{
    public class DeterminacaoController : ApiController
    {
        //===========================Carregar Dados do Contrato
        [Route("api/Determinacao/CarregarDados")]
        [HttpPost]
        [ActionName("CarregarDados")]
        [Authorize()]

        public IHttpActionResult CarregarDados([FromBody] Determinacao.FiltroModel Param)
        {
            SimLib clsLib = new SimLib();
            Determinacao Cls = new Determinacao(User.Identity.Name);
            try
            {
                Determinacao.ContratoModel retorno = Cls.CarregarDados(Param);
                return Ok(retorno);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }
        //===========================Gravar novo Comercial
        [Route("api/Determinacao/SalvarComercial")]
        [HttpPost]
        [ActionName("SalvarComercial")]
        [Authorize()]

        public IHttpActionResult SalvarComercial([FromBody] Determinacao.ComercialModel Param)
        {
            SimLib clsLib = new SimLib();
            Determinacao Cls = new Determinacao(User.Identity.Name);
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

        [Route("api/Determinacao/CarregarVeiculacao")]
        [HttpPost]
        [ActionName("CarregarVeiculacao")]
        [Authorize()]

        public IHttpActionResult CarregarVeiculacao([FromBody] Determinacao.FiltroModel Param)
        {
            SimLib clsLib = new SimLib();
            Determinacao Cls = new Determinacao(User.Identity.Name);
            try
            {
                List<Determinacao.VeiculacaoModel> retorno = Cls.AddVeiculacao(Param);
                return Ok(retorno);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }

        [Route("api/Determinacao/SalvarDeterminacao")]
        [HttpPost]
        [ActionName("SalvarDeterminacao")]
        [Authorize()]

        public IHttpActionResult SalvarDeterminacao([FromBody] Determinacao.ContratoModel Param)
        {
            SimLib clsLib = new SimLib();
            Determinacao Cls = new Determinacao(User.Identity.Name);
            try
            {
                List<Determinacao.VeiculacaoModel> retorno = Cls.SalvarDeterminacao(Param);
                return Ok(retorno);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }

        [Route("api/Determinacao/ExcluirComercialContrato")]
        [HttpPost]
        [ActionName("ExcluirComercialContrato")]
        [Authorize()]

        public IHttpActionResult ExcluirComercialContrato([FromBody] Determinacao.ComercialModel Param)
        {
            SimLib clsLib = new SimLib();
            Determinacao Cls = new Determinacao(User.Identity.Name);
            try
            {
                DataTable retorno = Cls.ExcluirComercialContrato(Param);
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