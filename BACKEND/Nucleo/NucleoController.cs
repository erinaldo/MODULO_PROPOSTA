using System;
using System.Web.Http;
using System.Data;
namespace PROPOSTA
{
    public class NucleoController : ApiController
    {
        //=================================Lista de Nucleo
        [Route("api/NucleoListar")]
        [HttpGet]
        [ActionName("NucleoListar")]
        [Authorize()]
        public IHttpActionResult NucleoListar()
        {
            SimLib clsLib = new SimLib();
            Nucleo Cls = new Nucleo(User.Identity.Name);
            try
            {
                DataTable dtb = Cls.NucleoListar();
                return Ok(dtb);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }
        ////=================================Obtem dados do Nucleo
        [Route("api/GetNucleoData/{Cod_Nucleo}")]
        [HttpGet]
        [ActionName("GetNucleoData")]
        [Authorize()]
        public IHttpActionResult GetNucleoData(String Cod_Nucleo)
        {
            SimLib clsLib = new SimLib();
            Nucleo Cls = new Nucleo(User.Identity.Name);
            try
            {
                Nucleo.FiltroModel Retorno = new Nucleo.FiltroModel();
                if (Cod_Nucleo != "0")
                {
                    Retorno = Cls.GetNucleoData(Cod_Nucleo);

                }
                return Ok(Retorno);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }

        //===========================Salvar Nucleo

        [Route("api/SalvarNucleo")]
        [HttpPost]
        [ActionName("SalvarNucleo")]
        [Authorize()]

        public IHttpActionResult SalvarNucleo([FromBody] Nucleo.FiltroModel pFiltro)
        {
            SimLib clsLib = new SimLib();
            Nucleo Cls = new Nucleo(User.Identity.Name);
            try
            {
                DataTable retorno = Cls.SalvarNucleo(pFiltro);
                return Ok(retorno);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }

        //===========================Excluir Nucleo

        [Route("api/ExcluirNucleo")]
        [HttpPost]
        [ActionName("ExcluirNucleo")]
        [Authorize()]

        public IHttpActionResult ExcluirNucleo([FromBody] Nucleo.FiltroModel pFiltro)
        {
            SimLib clsLib = new SimLib();
            Nucleo Cls = new Nucleo(User.Identity.Name);
            try
            {
                DataTable retorno = Cls.ExcluirNucleo(pFiltro);
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