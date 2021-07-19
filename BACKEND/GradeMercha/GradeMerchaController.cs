using System;
using System.Web.Http;
using System.Data;
using System.Collections.Generic;
using System.Globalization;

namespace PROPOSTA
{
    public class GradeMerchaController : ApiController
    {
        [Route("api/GradeMercha/List")]
        [HttpGet]
        [ActionName("GradeMerchaList")]
        [Authorize()]
        public IHttpActionResult GradeMerchaList([FromUri]GradeMercha.GradeMerchaFiltroModel Filtro)
        {
            SimLib clsLib = new SimLib();
            GradeMercha Cls = new GradeMercha(User.Identity.Name);
            try
            {
                GradeMercha.GradeMerchaListModel Retorno = Cls.GradeMerchaList(Filtro);
                return Ok(Retorno);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }
        [Route("api/GradeMercha/GetData")]
        [HttpGet]
        [ActionName("GetData")]
        [Authorize()]
        public IHttpActionResult GetData([FromUri]GradeMercha.GradeMerchaGetDataModel param)
        {
            SimLib clsLib = new SimLib();
            GradeMercha Cls = new GradeMercha(User.Identity.Name);
            try
            {
                GradeMercha.GradeMerchaModel Retorno = new GradeMercha.GradeMerchaModel();
                if (param.Action=="Edit")
                {
                    Retorno = Cls.GradeMerchaGetData(param);
                }
                Retorno.Action = param.Action;
                return Ok(Retorno);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }
        [Route("api/GradeMercha/GetProgramas")]
        [HttpGet]
        [ActionName("GetProgramas")]
        [Authorize()]
        public IHttpActionResult GetProgramas([FromUri]GradeMercha.GradeMerchaFiltroModel param)
        {
            SimLib clsLib = new SimLib();
            GradeMercha Cls = new GradeMercha(User.Identity.Name);
            try
            {
                DataTable Retorno = Cls.GradeMerchaGetProgramas(param);
                return Ok(Retorno);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }
        [Route("api/GradeMercha/GetUltimoDiaGrade")]
        [HttpGet]
        [ActionName("GetUltimoDiaGrade")]
        [Authorize()]
        public IHttpActionResult GetUltioDiaGrade([FromUri]GradeMercha.GradeMerchaGetDataModel param)
        {
            SimLib clsLib = new SimLib();
            GradeMercha Cls = new GradeMercha(User.Identity.Name);
            try
            {
                String  Retorno = Cls.GetUltimoDiaGradeMercha(param);
                return Ok(Retorno);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }
        [Route("api/GradeMercha/GetVeiculosRede/{RedeId}")]
        [HttpGet]
        [ActionName("GetVeiculosRede")]
        [Authorize()]
        public IHttpActionResult GetVeiculosRede(Int32 RedeId)
        {
            SimLib clsLib = new SimLib();
            GradeMercha Cls = new GradeMercha(User.Identity.Name);
            try
            {
             
                List<GradeMercha.VeiculoModel> Veiculos = Cls.GetVeiculosRede(RedeId);
                return Ok(Veiculos);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }
        [Route("api/GradeMercha/Salvar")]
        [HttpPost]
        [ActionName("Salvar")]
        [Authorize()]
        public IHttpActionResult Salvar([FromBody]GradeMercha.GradeMerchaModel param)
        {
            SimLib clsLib = new SimLib();
            GradeMercha Cls = new GradeMercha(User.Identity.Name);
            try
            {
                DataTable dtb = Cls.SalvarGradeMercha(param);
                return Ok(dtb);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }
        [Route("api/GradeMercha/Excluir")]
        [HttpPost]
        [ActionName("Excluir")]
        [Authorize()]
        public IHttpActionResult Excluir([FromBody]GradeMercha.GradeMerchaModel param)
        {
            SimLib clsLib = new SimLib();
            GradeMercha Cls = new GradeMercha(User.Identity.Name);
            try
            {
                DataTable dtb = Cls.ExcluirGradeMercha(param);
                return Ok(dtb);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }
        [Route("api/GradeMercha/Desativar")]
        [HttpPost]
        [ActionName("Desativar")]
        [Authorize()]
        public IHttpActionResult Desativar([FromBody]GradeMercha.GradeMerchaModel param)
        {
            SimLib clsLib = new SimLib();
            GradeMercha Cls = new GradeMercha(User.Identity.Name);
            try
            {
                DataTable dtb = Cls.DesativarGradeMercha(param,"D");
                return Ok(dtb);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }
        [Route("api/GradeMercha/Reativar")]
        [HttpPost]
        [ActionName("Reativar")]
        [Authorize()]
        public IHttpActionResult Reativar([FromBody]GradeMercha.GradeMerchaModel param)
        {
            SimLib clsLib = new SimLib();
            GradeMercha Cls = new GradeMercha(User.Identity.Name);
            try
            {
                DataTable dtb = Cls.DesativarGradeMercha(param, "R");
                return Ok(dtb);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }


        //--mmm INICIO

        //--Carrega as Listas nos parametros da propagação
        [Route("api/GradeMercha/CarregaParametrosPropagacao")]
        [HttpGet]
        [ActionName("CarregaParametrosPropagacao")]
        [Authorize()]
        public IHttpActionResult CarregaParametrosPropagacao()
        {
            SimLib clsLib = new SimLib();
            GradeMercha Cls = new GradeMercha(User.Identity.Name);
            try
            {
                GradeMercha.PropagacaoGradeMerchaModel GradeMerchaPropagacao = new GradeMercha.PropagacaoGradeMerchaModel();    //instancia o pai de todos
                // aqui agora vou chamar o sql do Veiculo e do programa e ja preencher a lista la
                // entao primeiro declaro uma variavel do tipo lista e vai ser preenchida com o retorno do que volta la do CarregaVeiculo
                // entao o cls.carregarVeiculo tem que voltar uma lista de veiculos
                List<GradeMercha.ListarVeiculoModel> Veiculos = Cls.CarregaVeiculo();
                List<GradeMercha.ListarProgramaModel> Programas = Cls.CarregaPrograma();
                GradeMerchaPropagacao.Veiculos = Veiculos;
                GradeMerchaPropagacao.Programas = Programas;
                return Ok(GradeMerchaPropagacao);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }
        //--Faz a propagação da GradeMercha
        [Route("api/GradeMercha/SalvarPropagacaoGrade")]
        [HttpPost]
        [ActionName("SalvarPropagacaoGrade")]
        [Authorize()]
        public IHttpActionResult SalvarPropagacaoGradeMercha([FromBody] GradeMercha.PropagacaoGradeMerchaModel GradeMercha)
        {
            SimLib clsLib = new SimLib();
            GradeMercha Cls = new GradeMercha(User.Identity.Name);
            try
            {
                Boolean retorno = Cls.SalvarPropagacaoGradeMercha(GradeMercha);
                return Ok(retorno);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }

        //--mmm FIM 
    }
}

