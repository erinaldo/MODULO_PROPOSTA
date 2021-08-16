using System;
using System.Web.Http;
using System.Data;
using System.Collections.Generic;
using System.Globalization;

namespace PROPOSTA
{
    public class PermutaController : ApiController
    {
        
        [Route("api/CarregarPermutas")]
        [HttpPost]
        [ActionName("CarregarPermutas")]
        [Authorize()]


        public IHttpActionResult CarregarPermutas([FromBody]Permutas.PermutasFiltroModel filtro)
        {
            SimLib clsLib = new SimLib();
            Permutas Cls = new Permutas((User.Identity.Name));
            try
            {
                //DataTable Retorno = Cls.CarregarPermutas(filtro);


                List<Permutas.PermutasModel> Retorno = Cls.CarregarPermutas(filtro);
                return Ok(Retorno);

            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }

 
        [Route("api/Permutas/GetPermuta/{Id_Permuta}")]
        [HttpGet]
        [ActionName("PermutasGetPermuta")]
        [Authorize()]
        public IHttpActionResult PermutasGetPermuta(Int32 Id_Permuta)
        {
            SimLib clsLib = new SimLib();
            Permutas Cls = new Permutas(User.Identity.Name);
            try
            {
                Permutas.PermutasModel Retorno = new Permutas.PermutasModel();
                if (Id_Permuta > 0)
                {
                    Retorno = Cls.PermutasGetPermuta(Id_Permuta);
                }
                else
                {
                    Retorno.ItensPermuta = new List<Permutas.ItensPermutaModel>();
                }
                return Ok(Retorno);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }

        [Route("api/Permutas/GetEntregaPermuta/{Id_Permuta}")]
        [HttpGet]
        [ActionName("PermutasGetEntregaPermuta")]
        [Authorize()]
        public IHttpActionResult PermutasGetEntregaPermuta(Int32 Id_Permuta)
        {
            SimLib clsLib = new SimLib();
            Permutas Cls = new Permutas(User.Identity.Name);
            try
            {
                Permutas.PermutasModel Retorno = new Permutas.PermutasModel();
                if (Id_Permuta > 0)
                {
                    Retorno = Cls.PermutasGetEntregaPermuta(Id_Permuta);
                }
                else
                {
                    Retorno.ItensEntregaPermuta = new List<Permutas.ItensEntregaPermutaModel>();
                }
                return Ok(Retorno);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }


        [Route("api/Permutas/ValidarNegociacao")]
        [HttpPost]
        [ActionName("PermutasValidarNegociacao")]
        [Authorize()]
        public IHttpActionResult PermutasValidarNegociacao([FromBody] Permutas.PermutasModel param)
        {
            SimLib clsLib = new SimLib();
            Permutas Cls = new Permutas(User.Identity.Name);
            try
            {
                DataTable Retorno = Cls.PermutasValidarNegociacao(param);
                return Ok(Retorno);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }

        [Route("api/Permutas/GetTerceirosNegociacao")]
        [HttpGet]
        [ActionName("GetTerceirosNegociacao")]
        [Authorize()]
        public IHttpActionResult GetTerceirosNegociacao([FromUri] MapaReserva.GetTerceirosNegociacaoModel param)
        {
            SimLib clsLib = new SimLib();
            MapaReserva Cls = new MapaReserva(User.Identity.Name);
            try
            {
                DataTable Retorno = Cls.GetTerceirosNegociacao(param);
                return Ok(Retorno);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }





        [Route("api/Permutas/Salvar")]
        [HttpPost]
        [ActionName("PermutasSalvar")]
        [Authorize()]
        public IHttpActionResult PermutasSalvar([FromBody] Permutas.PermutasModel param)
        {
            SimLib clsLib = new SimLib();
            Permutas Cls = new Permutas(User.Identity.Name);
            try
            {
                DataTable Retorno = Cls.PermutasSalvar(param);
                return Ok(Retorno);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }


        [Route("api/Permutas/SalvarEntrega")]
        [HttpPost]
        [ActionName("PermutasEntregaSalvar")]
        [Authorize()]
        public IHttpActionResult PermutasEntregaSalvar([FromBody] Permutas.PermutasModel param)
        {
            SimLib clsLib = new SimLib();
            Permutas Cls = new Permutas(User.Identity.Name);
            try
            {
                DataTable Retorno = Cls.PermutasEntregaSalvar(param);
                return Ok(Retorno);
                //return Ok(param);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }



        //===========================Excluir Itens

        [Route("api/ExcluirItensPermuta")]
        [HttpPost]
        [ActionName("ExcluirItensPermuta")]
        [Authorize()]

        public IHttpActionResult ExcluirItensPermuta([FromBody] Permutas.ItensPermutaModel param)
        {
            SimLib clsLib = new SimLib();
            Permutas Cls = new Permutas(User.Identity.Name);
            try
            {
                DataTable retorno = Cls.ExcluirItensPermuta(param);
                return Ok(retorno);


            }
            catch (Exception Ex)
            {
                //clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }

        //===========================Excluir Itens

        [Route("api/ExcluirItensEntregaPermuta")]
        [HttpPost]
        [ActionName("ExcluirItensEntregaPermuta")]
        [Authorize()]

        public IHttpActionResult ExcluirItensEntregaPermuta([FromBody] Permutas.ItensEntregaPermutaModel param)
        {
            SimLib clsLib = new SimLib();
            Permutas Cls = new Permutas(User.Identity.Name);
            try
            {
                DataTable retorno = Cls.ExcluirItensEntregaPermuta(param);
                return Ok(retorno);


            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }

        [Route("api/Permutas/PesquisarItemPermuta")]
        [HttpPost]
        [ActionName("PermutasPesquisarItemPermuta")]
        [Authorize()]
        public IHttpActionResult PermutasPesquisarItemPermuta([FromBody] Permutas.PermutasModel param)
        {
            SimLib clsLib = new SimLib();
            Permutas Cls = new Permutas(User.Identity.Name);
            try
            {
                DataTable Retorno = Cls.PermutasPesquisarItemPermuta(param);
                return Ok(Retorno);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }

        [Route("api/Permutas/ValidarItemPermuta")]
        [HttpPost]
        [ActionName("PermutasValidarItemPermuta")]
        [Authorize()]
        public IHttpActionResult PermutasValidarItemPermuta([FromBody] Permutas.PermutasModel param)
        {
            SimLib clsLib = new SimLib();
            Permutas Cls = new Permutas(User.Identity.Name);
            try
            {
                DataTable Retorno = Cls.PermutasValidarItemPermuta(param);
                return Ok(Retorno);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }



        [Route("api/Permutas/ProdutosVeiculos")]
        [HttpPost]
        [ActionName("ProdutosVeiculos")]
        [Authorize()]
        public IHttpActionResult ProdutosVeiculos([FromBody]Permutas.PermutasModel Param)
        {
            SimLib clsLib = new SimLib();
            Permutas Cls = new Permutas(User.Identity.Name);
            try
            {
                DataTable Retorno = Cls.ProdutosVeiculos(Param);
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

