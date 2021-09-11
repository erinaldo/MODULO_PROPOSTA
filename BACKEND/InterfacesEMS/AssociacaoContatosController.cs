using System;
using System.Web.Http;
using System.Data;
using System.Collections.Generic;
namespace PROPOSTA
{
    public class AssociacaoContatosController : ApiController
    {
        //=================================Lista de contato
        [Route("api/AssociacaoContatosListar")]
        [HttpGet]
        [ActionName("AssociacaoContatosListar")]
        [Authorize()]
        public IHttpActionResult AssociacaoContatosListar()
        {
            SimLib clsLib = new SimLib();
            AssociacaoContatos Cls = new AssociacaoContatos(User.Identity.Name);
            try
            {
                DataTable dtb = Cls.AssociacaoContatosListar(0);
                return Ok(dtb);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }

        [Route("api/GetAssociacaoContatosData/{Id_AssociacaoContatos}")]
        [HttpGet]
        [ActionName("GetAssociacaoContatos")]
        [Authorize()]
        public IHttpActionResult GetAssociacaoContatos(String Id_AssociacaoContatos)
        {
            SimLib clsLib = new SimLib();
            AssociacaoContatos Cls = new AssociacaoContatos(User.Identity.Name);
            try
            {
                AssociacaoContatos.AssociacaoContatosModel Retorno = new AssociacaoContatos.AssociacaoContatosModel();
                if (Id_AssociacaoContatos != "")
                {
                    Retorno = Cls.GetAssociacaoContatos(Id_AssociacaoContatos);
                }

                return Ok(Retorno);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }

        [Route("api/SalvarAssociacaoContatos")]
        [HttpPost]
        [ActionName("SalvarAssociacaoContatos")]
        [Authorize()]
        public IHttpActionResult SalvarAssociacaoContatos([FromBody] AssociacaoContatos.AssociacaoContatosModel param)
        {
            SimLib clsLib = new SimLib();
            AssociacaoContatos Cls = new AssociacaoContatos(User.Identity.Name);
            try
            {
                DataTable Retorno = Cls.SalvarAssociacaoContatos(param);
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

