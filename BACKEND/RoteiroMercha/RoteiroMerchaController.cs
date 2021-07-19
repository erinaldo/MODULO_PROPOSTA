using System;
using System.Web.Http;
using System.Data;
using System.Collections.Generic;

namespace PROPOSTA
{
    public class RoteiroMerchaController : ApiController
    {
        //=================================Lista de RoteiroMerchas
        [Route("api/RoteiroMercha/GuiaProgramacao")]
        [HttpPost]
        [ActionName("RoteiroMerchaGuiaProgramacao")]
        [Authorize()]
        public IHttpActionResult RoteiroMerchaGuiaProgramacao([FromBody]RoteiroMercha.RoteiroMerchaFiltroModel Filtro)
        {
            SimLib clsLib = new SimLib();
            RoteiroMercha Cls = new RoteiroMercha(User.Identity.Name);
            try
            {
                DataTable dtb = Cls.CarregarGuiaProgramacao(Filtro);
                return Ok(dtb);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }
        //=================================Lista de RoteiroMerchas
        [Route("api/RoteiroMercha/CarregarRoteiro")]
        [HttpPost]
        [ActionName("CarregarRoteiroMercha")]
        [Authorize()]
        public IHttpActionResult CarregarRoteiroMercha([FromBody]RoteiroMercha.RoteiroMerchaFiltroModel Filtro)
        {
            SimLib clsLib = new SimLib();
            RoteiroMercha Cls = new RoteiroMercha(User.Identity.Name);
            try
            {
                List<RoteiroMercha.RoteiroMerchaModel> RoteiroMercha = Cls.RoteiroMerchaCarregar(Filtro);
                return Ok(RoteiroMercha);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }
        //=================================Lista de Comerciais
        [Route("api/RoteiroMercha/CarregarComerciais")]
        [HttpPost]
        [ActionName("RoteiroMerchaCarregarComerciais")]
        [Authorize()]
        public IHttpActionResult RoteiroMerchaCarregarComerciais([FromBody]RoteiroMercha.RoteiroMerchaFiltroModel Filtro)
        {
            SimLib clsLib = new SimLib();
            RoteiroMercha Cls = new RoteiroMercha(User.Identity.Name);
            try
            {
                List<RoteiroMercha.RoteiroMerchaComercialModel>   Comerciais = Cls.RoteiroMerchaCarregarComerciais(Filtro);
                return Ok(Comerciais);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }
        //=================================Baixa de comerciais no RoteiroMercha
        [Route("api/RoteiroMercha/BaixarVeiculacao")]
        [HttpPost]
        [ActionName("RoteiroMerchaBaixarVeiculacao")]
        [Authorize()]
        public IHttpActionResult RoteiroMerchaBaixarVeiculacao([FromBody]RoteiroMercha.RoteiroMerchaModel pRoteiroMercha)
        {
            SimLib clsLib = new SimLib();
            RoteiroMercha Cls = new RoteiroMercha(User.Identity.Name);
            try
            {
                DataTable Retorno = Cls.RoteiroMerchaBaixarVeiculacao(pRoteiroMercha);
                return Ok(Retorno);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }
        //=================================Exclusao do RoteiroMercha
        [Route("api/RoteiroMercha/Excluir")]
        [HttpPost]
        [ActionName("RoteiroMerchaExcluir")]
        [Authorize()]
        public IHttpActionResult RoteiroMerchaExcluir([FromBody]RoteiroMercha.RoteiroMerchaFiltroModel pRoteiroMercha)
        {
            SimLib clsLib = new SimLib();
            RoteiroMercha Cls = new RoteiroMercha(User.Identity.Name);
            try
            {
                Cls.RoteiroMerchaExcluir(pRoteiroMercha);
                return Ok(true);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }

        //=================================Salvar Ordenacao
        [Route("api/RoteiroMercha/Salvar")]
        [HttpPost]
        [ActionName("RoteiroMerchaSalvar")]
        [Authorize()]
        public IHttpActionResult RoteiroMerchaSalvar ([FromBody]List<RoteiroMercha.RoteiroMerchaModel> pRoteiroMercha)
        {
            SimLib clsLib = new SimLib();
            RoteiroMercha Cls = new RoteiroMercha(User.Identity.Name);
            try
            {
                DataTable Retorno =  Cls.RoteiroMerchaSalvar(pRoteiroMercha);
                return Ok(Retorno);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }
        //=================================Salvar Ordenacao
        [Route("api/RoteiroMercha/ListarBreak")]
        [HttpGet]
        [ActionName("RoteiroMerchaListarBreak")]
        [Authorize()]
        public IHttpActionResult RoteiroMerchaListarBreak([FromUri] RoteiroMercha.RoteiroMerchaFiltroModel Param)
        {
            SimLib clsLib = new SimLib();
            RoteiroMercha Cls = new RoteiroMercha(User.Identity.Name);
            try
            {
                RoteiroMercha.BreakModel Retorno = Cls.RoteiroMerchaListarBreak(Param);
                return Ok(Retorno);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }


        [Route("api/RoteiroMercha/GravarBreak")]
        [HttpPost]
        [ActionName("RoteiroMerchaGravarBreak")]
        [Authorize()]
        public IHttpActionResult RoteiroMerchaGravarBreak([FromBody] RoteiroMercha.BreakModel Param)
        {
            SimLib clsLib = new SimLib();
            RoteiroMercha Cls = new RoteiroMercha(User.Identity.Name);
            try
            {
                DataTable Retorno = Cls.RoteiroMerchaGravarBreak(Param);
                return Ok(Retorno);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }

        [Route("api/RoteiroMercha/ProgramasBreak")]
        [HttpGet]
        [ActionName("RoteiroMerchaProgramasBreak")]
        [Authorize()]
        public IHttpActionResult RoteiroMerchaProgramasBreak([FromUri] RoteiroMercha.RoteiroMerchaFiltroModel Param)
        {
            SimLib clsLib = new SimLib();
            RoteiroMercha Cls = new RoteiroMercha(User.Identity.Name);
            try
            {
                DataTable Retorno = Cls.RoteiroMerchaProgramasBreak(Param);
                return Ok(Retorno);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }

        [Route("api/RoteiroMercha/PreOrdenar")]
        [HttpPost]
        [ActionName("PreOrdenar")]
        [Authorize()]
        public IHttpActionResult PreOrdenar([FromBody] RoteiroMercha.FiltroPreOrdModel RoteiroMercha)
        {
            SimLib clsLib = new SimLib();
            RoteiroMercha Cls = new RoteiroMercha(User.Identity.Name);
            try
            {
                DataTable retorno = Cls.PreOrdenar(RoteiroMercha);
                return Ok(retorno);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }

        [Route("api/RoteiroMercha/ExisteRoteiroMerchaOrdenado")]
        [HttpGet]
        [ActionName("ExisteRoteiroMerchaOrdenado")]
        [Authorize()]
        public IHttpActionResult ExisteRoteiroMerchaOrdenado([FromUri] RoteiroMercha.FiltroPreOrdModel RoteiroMercha)
        {
            SimLib clsLib = new SimLib();
            RoteiroMercha Cls = new RoteiroMercha(User.Identity.Name);
            try
            {
                Boolean Retorno = Cls.ExisteRoteiroMerchaOrdenado(RoteiroMercha);
                return Ok(Retorno);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }

        [Route("api/RoteiroMercha/ImprimirRoteiro")]
        [HttpPost]
        [ActionName("ImprimirRoteiroMercha")]
        [Authorize()]
        public IHttpActionResult ImprimirRoteiroMercha([FromBody] RoteiroMercha.RoteiroMerchaFiltroModel Param)
        {
            SimLib clsLib = new SimLib();
            try
            {
                ImpressaoRoteiroMercha Cls = new ImpressaoRoteiroMercha(User.Identity.Name);
                return Ok(Cls.ImprimirRoteiroMercha(Param));
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }
        [Route("api/RoteiroMercha/EncerrarRoteiroMercha")]
        [HttpPost]
        [ActionName("EncerrarRoteiroMercha")]
        [Authorize()]
        public IHttpActionResult EncerrarRoteiroMercha([FromBody] RoteiroMercha.EncerramentoRoteiroMerchaModel Param)
        {
            SimLib clsLib = new SimLib();
            RoteiroMercha Cls = new RoteiroMercha(User.Identity.Name);
            try
            {
                DataTable retorno = Cls.EncerrarRoteiroMercha(Param);
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

