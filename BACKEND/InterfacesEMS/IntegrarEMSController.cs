using System;
using System.Web.Http;
using System.Data;
using System.Collections.Generic;
namespace PROPOSTA
{
    public class IntegrarEMSController : ApiController
    {
        //=================================Carregar Faturas/Boletos
        [Route("api/CarregarFaturasBoletos")]
        [HttpPost]
        [ActionName("CarregarFaturasBoletos")]
        [Authorize()]

        public IHttpActionResult CarregarFaturasBoletos([FromBody]IntegrarEMS.IntegrarEMSModel filtro)
        {
            SimLib clsLib = new SimLib();
            IntegrarEMS Cls = new IntegrarEMS((User.Identity.Name));
            try
            {
                DataTable dtb = Cls.CarregarFaturasBoletos(filtro);
                return Ok(dtb);

            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }

        //=================================Carregar Críticas
        [Route("api/CriticasEMSCarregar")]
        [HttpGet]
        [ActionName("CriticasEMSCarregar")]
        [Authorize()]
        public IHttpActionResult CriticasEMSCarregar([FromUri]IntegrarEMS.CriticaEMSModel Param)
        {
            SimLib clsLib = new SimLib();
            IntegrarEMS Cls = new IntegrarEMS((User.Identity.Name));
            try
            {
                List<IntegrarEMS.CriticaEMSModel> Retorno = Cls.CriticasEMSCarregar(Param);
                return Ok(Retorno);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }

        //=================================Verificar se Em.Fat está parametrizada
        [Route("api/VerificarEmpresaParametrizada/{pCodEmpresa}")]
        [HttpGet]
        [ActionName("VerificarEmpresaParametrizada")]
        [Authorize()]
        public IHttpActionResult VerificarEmpresaParametrizada(String pCodEmpresa)
        {
            SimLib clsLib = new SimLib();
            IntegrarEMS Cls = new IntegrarEMS((User.Identity.Name));
            try
            {
                Int32 Retorno = Cls.VerificarEmpresaParametrizada(pCodEmpresa);
                return Ok(Retorno);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }

        //=================================Processa Faturas
        [Route("api/ProcessarFaturas")]
        [HttpPost]
        [ActionName("ProcessarFaturas")]
        [Authorize()]
        public IHttpActionResult ProcessarFaturas([FromBody] List<IntegrarEMS.IntegrarEMSModel> Param)
        {
            SimLib clsLib = new SimLib();
            IntegrarEMS Cls = new IntegrarEMS((User.Identity.Name));
            IntegrarEMS.ResponseModel Retorno = new IntegrarEMS.ResponseModel();
            Retorno.IsSuccessful = true;
            Retorno.Message = "Integração de Faturas Concluída com Sucesso";

            try
            {
                Retorno.Indica_Critica =  Cls.ProcessarFaturas(Param);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                Retorno.IsSuccessful = false;
                Retorno.Message = Ex.Message;
            }
            finally
            {

            }
            return Ok(Retorno);

        }

        //=================================Processa Boletos
        [Route("api/ProcessarBoletos")]
        [HttpPost]
        [ActionName("ProcessarBoletos")]
        [Authorize()]
        public IHttpActionResult ProcessarBoletos([FromBody] List<IntegrarEMS.IntegrarEMSModel> Param)
        {
            SimLib clsLib = new SimLib();
            IntegrarEMS Cls = new IntegrarEMS((User.Identity.Name));
            IntegrarEMS.ResponseModel Retorno = new IntegrarEMS.ResponseModel();
            Retorno.IsSuccessful = true;
            Retorno.Message = "Integração de Boletos Concluída com Sucesso";

            try
            {
                Retorno.Indica_Critica = Cls.ProcessarBoletos(Param);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                Retorno.IsSuccessful = false;
                Retorno.Message = Ex.Message;
            }
            finally
            {

            }
            return Ok(Retorno);




        }

        //=================================Atualiza o Log de Controle de Integrações
        [Route("api/GravarLogControle")]
        [HttpGet]
        [ActionName("GravarLogControle")]
        [Authorize()]
        public IHttpActionResult GravarLogControle([FromUri]IntegrarEMS.IntegrarEMSModel Param)
        {
            SimLib clsLib = new SimLib();
            IntegrarEMS Cls = new IntegrarEMS((User.Identity.Name));
            try
            {
                Boolean Retorno = Cls.GravarLogControle(Param);
                return Ok(Retorno);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }

        ////=================================testes
        [Route("api/TESTEAPIEMS")]
        [HttpGet]
        [ActionName("TESTEAPIEMS")]
        //[Authorize()]
        public IHttpActionResult TESTEAPIEMS()
        {
            SimLib clsLib = new SimLib();
            IntegrarEMS Cls = new IntegrarEMS((User.Identity.Name));
            try
            {
                Int32 Retorno = Cls.TESTEAPIEMS();
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

