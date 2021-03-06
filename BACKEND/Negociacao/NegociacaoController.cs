using System;
using System.Web.Http;
using System.Data;
using System.Collections.Generic;
using System.Globalization;

namespace PROPOSTA
{
    public class NegociacaoController : ApiController
    {
        [Route("api/Negociacao/Select")]
        [HttpGet]
        [ActionName("NegociacaoSelect")]
        [Authorize()]
        public IHttpActionResult NegociacaoSelect([FromUri]Negociacao.NegociacaoFiltroParam Param)
        {
            SimLib clsLib = new SimLib();
            Negociacao Cls = new Negociacao(User.Identity.Name);
            try
            {
                DataTable Retorno = Cls.NegociacaoSelect(Param);
                return Ok(Retorno);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }

        [Route("api/Negociacao/List")]
        [HttpGet]
        [ActionName("NegociacaoList")]
        [Authorize()]
        public IHttpActionResult NegociacaoList([FromUri]Negociacao.NegociacaoFiltroParam Param)
        {
            SimLib clsLib = new SimLib();
            Negociacao Cls = new Negociacao(User.Identity.Name);
            try
            {
                List<Negociacao.NegociacaoModel> Retorno = Cls.NegociacaoList(Param);
                return Ok(Retorno);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }
        [Route("api/Negociacao/Detalhe")]
        [HttpGet]
        [ActionName("NegociacaoDetalhe")]
        [Authorize()]
        public IHttpActionResult NegociacaoDetalhe([FromUri]Negociacao.NegociacaoFiltroParam Param)
        {
            SimLib clsLib = new SimLib();
            Negociacao Cls = new Negociacao(User.Identity.Name);
            try
            {
                DataTable Retorno = Cls.NegociacaoDetalhe(Param.Numero_Negociacao);
                return Ok(Retorno);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }
        [Route("api/Negociacao/Contar")]
        [HttpGet]
        [ActionName("NegociacaoContar")]
        [Authorize()]
        public IHttpActionResult NegociacaoContar()
        {
            SimLib clsLib = new SimLib();
            Negociacao Cls = new Negociacao(User.Identity.Name);
            try
            {
                 Negociacao.NegociacaoCountModel Retorno = Cls.NegociacaoContar();
                return Ok(Retorno);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }
        [Route("api/ImprimirMapa/{Id_Contrato}")]
        [HttpGet]
        [ActionName("ImprimirMapa")]
        [Authorize()]
        public IHttpActionResult ImprimirMapa(Int32 Id_Contrato)
        {
            SimLib clsLib = new SimLib();
            try
            {
                ImpressaoMapa Cls = new ImpressaoMapa(User.Identity.Name);

                return Ok(Cls.ImprimirMapa(Id_Contrato));
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }

        [Route("api/Negociacao/Get")]
        [HttpGet]
        [ActionName("NegociacaoGet")]
        [Authorize()]
        public IHttpActionResult NegociacaoGet([FromUri]Negociacao.NegociacaoModel Param)
        {
            SimLib clsLib = new SimLib();
            Negociacao Cls = new Negociacao(User.Identity.Name);
            try
            {
                Negociacao.NegociacaoModel Retorno = new Negociacao.NegociacaoModel();
                if (Param.Numero_Negociacao==0)
                {
                    Retorno.Empresas_Venda = new List<Negociacao.NegociacaoEmpresaVendaModel>();
                    Retorno.Empresas_Faturamento = new List<Negociacao.NegociacaoEmpresaFaturamentoModel>();
                    Retorno.Agencias= new List<Negociacao.NegociacaoAgenciaModel>();
                    Retorno.Clientes= new List<Negociacao.NegociacaoClienteModel>();
                    Retorno.Contatos = new List<Negociacao.NegociacaoContatoModel>();
                    Retorno.Nucleos= new List<Negociacao.NegociacaoNucleoModel>();
                    Retorno.Intermediarios = new List<Negociacao.NegociacaoIntermediarioModel>();
                    Retorno.Apresentadores= new List<Negociacao.NegociacaoApresentadorModel>();
                    Retorno.Parcelas = new List<Negociacao.NegociacaoParcelaModel>();
                    Retorno.Descontos = new List<Negociacao.NegociacaoDescontoModel>();
                    Retorno.Permite_Editar =true;
                }
                else
                {
                    Retorno = Cls.NegociacaoGet(Param);
                }
                
                return Ok(Retorno);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }

        [Route("api/Negociacao/Salvar")]
        [HttpPost]
        [ActionName("NegociacaoSalvar")]
        [Authorize()]
        public IHttpActionResult NegociacaoSalvar([FromBody]Negociacao.NegociacaoModel Param)
        {
            SimLib clsLib = new SimLib();
            Negociacao Cls = new Negociacao(User.Identity.Name);
            try
            {
                List <Negociacao.NegociacaoCriticaModel> retorno = Cls.SalvarNegociacao(Param);
                return Ok(retorno);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }

        [Route("api/Negociacao/Desativar")]
        [HttpPost]
        [ActionName("NegociacaoDesativar")]
        [Authorize()]
        public IHttpActionResult NegociacaoDesativar([FromBody]Negociacao.NegociacaoDesativarModel Param)
        {
            SimLib clsLib = new SimLib();
            Negociacao Cls = new Negociacao(User.Identity.Name);
            try
            {
                DataTable retorno = Cls.NegociacaoDesativar(Param);
                return Ok(retorno);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }

        [Route("api/Negociacao/TerceiroGet")]
        [HttpGet]
        [ActionName("TerceiroGet")]
        [Authorize()]
        public IHttpActionResult TerceiroGet([FromUri]Negociacao.NegociacaoTerceiroGetModel Param)
        {
            SimLib clsLib = new SimLib();
            Negociacao Cls = new Negociacao(User.Identity.Name);
            try
            {
                List<Negociacao.NegociacaoTerceiroModel> retorno = Cls.TerceiroGet(Param);
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

