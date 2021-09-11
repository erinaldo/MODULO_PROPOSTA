using System;
using System.Web.Http;
using System.Data;
using System.Collections.Generic;
namespace PROPOSTA
{
    public class AssociacaoProgramasController : ApiController
    {
        //=================================Lista de contato
        [Route("api/AssociacaoProgramasListar")]
        [HttpPost]
        [ActionName("AssociacaoProgramasListar")]
        [Authorize()]
        public IHttpActionResult AssociacaoProgramasListar([FromBody]AssociacaoProgramas.AssociacaoProgramasModel pCodEmpresaFat)
        {
            SimLib clsLib = new SimLib();
            AssociacaoProgramas Cls = new AssociacaoProgramas(User.Identity.Name);
            try
            {
                DataTable dtb = Cls.AssociacaoProgramasListar(pCodEmpresaFat);
                return Ok(dtb);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }

        //[Route("api/GetAssociacaoProgramasData")]
        //[HttpPost]
        //[ActionName("GetAssociacaoProgramas")]
        //[Authorize()]
        //public IHttpActionResult GetAssociacaoProgramas([FromBody]AssociacaoProgramas.AssociacaoProgramasModel pAssociacaoProgramas)
        //{
        //    SimLib clsLib = new SimLib();
        //    AssociacaoProgramas Cls = new AssociacaoProgramas(User.Identity.Name);
        //    try
        //    {
        //        //AssociacaoProgramas.AssociacaoProgramasModel Retorno = new AssociacaoProgramas.AssociacaoProgramasModel();
        //        DataTable Retorno = Cls.GetAssociacaoProgramas(pAssociacaoProgramas);
        //        //DataTable dtb = Cls.GetAssociacaoProgramas(pAssociacaoProgramas);


        //        return Ok(Retorno);
        //    }
        //    catch (Exception Ex)
        //    {
        //        clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
        //        throw new Exception(Ex.Message);
        //    }
        //}



        [Route("api/GetAssociacaoProgramas")]
        [HttpGet]
        [ActionName("GetAssociacaoProgramas")]
        [Authorize()]
        public IHttpActionResult GetAssociacaoProgramas(String Cod_Empresa_Faturamento, String Cod_Programa)
        {
            SimLib clsLib = new SimLib();
            AssociacaoProgramas Cls = new AssociacaoProgramas(User.Identity.Name);
            try
            {
                AssociacaoProgramas.AssociacaoProgramasModel Retorno = new AssociacaoProgramas.AssociacaoProgramasModel();

                Retorno = Cls.GetAssociacaoProgramas(Cod_Empresa_Faturamento, Cod_Programa);
                return Ok(Retorno);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }

        [Route("api/SalvarAssociacaoProgramas")]
        [HttpPost]
        [ActionName("SalvarAssociacaoProgramas")]
        [Authorize()]
        public IHttpActionResult SalvarAssociacaoProgramas([FromBody] AssociacaoProgramas.AssociacaoProgramasModel param)
        {
            SimLib clsLib = new SimLib();
            AssociacaoProgramas Cls = new AssociacaoProgramas(User.Identity.Name);
            try
            {
                DataTable Retorno = Cls.SalvarAssociacaoProgramas(param);
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

