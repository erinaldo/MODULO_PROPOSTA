using System;
using System.Web.Http;
using System.Data;
namespace PROPOSTA
{
    public class PortadorController : ApiController
    {
        //=================================Lista de Portadores
        [Route("api/PortadorListar")]
        [HttpGet]
        [ActionName("PortadorListar")]
        [Authorize()]
        public IHttpActionResult PortadorListar()
        {
            SimLib clsLib = new SimLib();
            Portador Cls = new Portador(User.Identity.Name);
            try
            {
                DataTable dtb = Cls.PortadorListar(0);
                return Ok(dtb);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }
        ////=================================Obtem dados do Portador
        [Route("api/GetPortadorData/{Id_Portador}")]
        [HttpGet]
        [ActionName("GetPortadorData")]
        [Authorize()]
        public IHttpActionResult GetPortadorData(Int32 Id_Portador)
        {
            SimLib clsLib = new SimLib();
            Portador Cls = new Portador(User.Identity.Name);
            try
            {
                Portador.PortadorModel Retorno = new Portador.PortadorModel();
                if (Id_Portador != 0)
                {
                    Retorno = Cls.GetPortadorData(Id_Portador);

                }
                return Ok(Retorno);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }

        //===========================Salvar Portador

        [Route("api/SalvarPortador")]
        [HttpPost]
        [ActionName("SalvarPortador")]
        [Authorize()]

        public IHttpActionResult SalvarPortador([FromBody] Portador.PortadorModel pPortador)
        {
            SimLib clsLib = new SimLib();
            Portador Cls = new Portador(User.Identity.Name);
            try
            {
                DataTable retorno = Cls.SalvarPortador(pPortador);
                return Ok(retorno);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }

        //===========================Excluir Portador

        [Route("api/excluirPortador")]
        [HttpPost]
        [ActionName("excluirPortador")]
        [Authorize()]

        public IHttpActionResult excluirPortador([FromBody] Portador.PortadorModel pPortador)
        {
            SimLib clsLib = new SimLib();
            Portador Cls = new Portador(User.Identity.Name);
            try
            {
                DataTable retorno = Cls.excluirPortador(pPortador);
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

