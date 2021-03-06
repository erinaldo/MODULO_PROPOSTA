using System;
using System.Web.Http;
using System.Data;
using System.Collections.Generic;
using System.Globalization;

namespace PROPOSTA
{
    public class MapaReservaController : ApiController
    {
        [Route("api/MapaReserva/List")]
        [HttpPost]
        [ActionName("MapaReservaList")]
        [Authorize()]
        public IHttpActionResult MapaReservaList([FromBody]MapaReserva.MapaReservaFiltroModel Param)
        {
            SimLib clsLib = new SimLib();
            MapaReserva Cls = new MapaReserva(User.Identity.Name);
            try
            {
                DataTable Retorno = Cls.MapaReservaList(Param);
                return Ok(Retorno);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }
        [Route("api/MapaReserva/DetalheContrato/{Id}")]
        [HttpGet]
        [ActionName("DetalheContrato")]
        [Authorize()]
        public IHttpActionResult DetalheContrato(Int32 Id)
        {
            SimLib clsLib = new SimLib();
            MapaReserva Cls = new MapaReserva(User.Identity.Name);
            try
            {
                DataTable Retorno = Cls.MapaReservaDetalheContrato(Id);
                return Ok(Retorno);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }
        [Route("api/MapaReserva/DetalheComercial/{Id}")]
        [HttpGet]
        [ActionName("DetalheComercial")]
        [Authorize()]
        public IHttpActionResult DetalheComercial(Int32 Id)
        {
            SimLib clsLib = new SimLib();
            MapaReserva Cls = new MapaReserva(User.Identity.Name);
            try
            {
                DataTable Retorno = Cls.MapaReservaDetalheComercial(Id);
                return Ok(Retorno);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }
        [Route("api/MapaReserva/DetalheCompetencia/{Id}")]
        [HttpGet]
        [ActionName("DetalheCompetencia")]
        [Authorize()]
        public IHttpActionResult DetalheCompetencia(Int32 Id)
        {
            SimLib clsLib = new SimLib();
            MapaReserva Cls = new MapaReserva(User.Identity.Name);
            try
            {
                DataTable Retorno = Cls.MapaReservaDetalheCompetencia(Id);
                return Ok(Retorno);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }
        [Route("api/MapaReserva/DetalheVeiculo/{Id}")]
        [HttpGet]
        [ActionName("DetalheVeiculo")]
        [Authorize()]
        public IHttpActionResult DetalheVeiculo(Int32 Id)
        {
            SimLib clsLib = new SimLib();
            MapaReserva Cls = new MapaReserva(User.Identity.Name);
            try
            {
                DataTable Retorno = Cls.MapaReservaDetalheVeiculo(Id);
                return Ok(Retorno);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }
        [Route("api/MapaReserva/DetalheMidia")]
        [HttpGet]
        [ActionName("DetalheMidia")]
        [Authorize()]
        public IHttpActionResult DetalheMidia([FromUri] MapaReserva.MapaReservaMidiaFiltroModel Param)
        {
            SimLib clsLib = new SimLib();
            MapaReserva Cls = new MapaReserva(User.Identity.Name);
            try
            {
                List<MapaReserva.MapaReservaMidiaModel> Retorno = Cls.MapaReservaDetalheMidia(Param);
                return Ok(Retorno);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }
        [Route("api/MapaReserva/DetalheVeiculacao")]
        [HttpGet]
        [ActionName("DetalheVeiculacao")]
        [Authorize()]
        public IHttpActionResult DetalheVeiculacao([FromUri] MapaReserva.MapaReservaMidiaFiltroModel Param)
        {
            SimLib clsLib = new SimLib();
            MapaReserva Cls = new MapaReserva(User.Identity.Name);
            try
            {
                DataTable Retorno = Cls.MapaReservaDetalheVeiculacoes(Param);
                return Ok(Retorno);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }
        [Route("api/MapaReserva/DetalheResumo")]
        [HttpGet]
        [ActionName("DetalheResumo")]
        [Authorize()]
        public IHttpActionResult DetalheResumo([FromUri] MapaReserva.MapaReservaMidiaFiltroModel Param)
        {
            SimLib clsLib = new SimLib();
            MapaReserva Cls = new MapaReserva(User.Identity.Name);
            try
            {
                DataTable Retorno = Cls.MapaReservaDetalheResumo(Param);
                return Ok(Retorno);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }
        [Route("api/MapaReserva/Import")]
        [HttpGet]
        [ActionName("MapaReservaImport")]
        [Authorize()]
        public IHttpActionResult MapaReservaImport()
        {
            SimLib clsLib = new SimLib();
            MapaReserva Cls = new MapaReserva(User.Identity.Name);
            try
            {
                DataTable Retorno = Cls.MapaReservaImport();
                return Ok(Retorno);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }
        [Route("api/MapaReserva/CarregarEsquema/{Id_Esquema}")]
        [HttpGet]
        [ActionName("MapaReservaCarregarEsquema")]
        [Authorize()]
        public IHttpActionResult MapaReservaCarregarEsquema(Int32 Id_Esquema)
        {
            SimLib clsLib = new SimLib();
            MapaReserva Cls = new MapaReserva(User.Identity.Name);
            try
            {
                MapaReserva.ContratoModel Retorno = Cls.MapaReservaCarregarEsquema(Id_Esquema);
                return Ok(Retorno);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }

        [Route("api/MapaReserva/ValidarNegociacao")]
        [HttpPost]
        [ActionName("MapaReservaValidarNegociacao")]
        [Authorize()]
        public IHttpActionResult MapaReservaValidadarNegociacao([FromBody] MapaReserva.ContratoModel param)
        {
            SimLib clsLib = new SimLib();
            MapaReserva Cls = new MapaReserva(User.Identity.Name);
            try
            {
                DataTable Retorno = Cls.MapaReservaValidarNegociacao(param);
                return Ok(Retorno);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }
        [Route("api/MapaReserva/LocalizarNegociacao")]
        [HttpPost]
        [ActionName("MapaReservaLocalizarNegociacao")]
        [Authorize()]
        public IHttpActionResult MapaReservaLocalizarNegociacao([FromBody] MapaReserva.ContratoModel param)
        {
            SimLib clsLib = new SimLib();
            MapaReserva Cls = new MapaReserva(User.Identity.Name);
            try
            {
                DataTable Retorno = Cls.MapaReservaLocalizarNegociacao(param);
                return Ok(Retorno);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }

        [Route("api/MapaReserva/ListarProdutoCliente/{Cod_Terceiro}")]
        [HttpGet]
        [ActionName("MapaReservaListarProdutoCliente")]
        [Authorize()]
        public IHttpActionResult MapaReservaListarProdutoCliente(String Cod_Terceiro)
        {
            SimLib clsLib = new SimLib();
            MapaReserva Cls = new MapaReserva(User.Identity.Name);
            try
            {
                DataTable Retorno = Cls.MapaReservaListarProdutoCliente(Cod_Terceiro);
                return Ok(Retorno);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }

        [Route("api/MapaReserva/GetContrato/{Id_Contrato}")]
        [HttpGet]
        [ActionName("MapaReservaGetContrato")]
        [Authorize()]
        public IHttpActionResult MapaReservaGetContrato(Int32 Id_Contrato)
        {
            SimLib clsLib = new SimLib();
            MapaReserva Cls = new MapaReserva(User.Identity.Name);
            try
            {
                MapaReserva.ContratoModel Retorno = new MapaReserva.ContratoModel();
                if (Id_Contrato>0)
                {
                    Retorno = Cls.MapaReservaGetContrato(Id_Contrato);
                    Retorno.Veiculacoes = new List<MapaReserva.VeiculacacaoModel>();
                    Retorno.VeiculacoesOnLine = new List<MapaReserva.VeiculacaoOnLineModel>();
                    Retorno.Editar_Negociacao = false;
                    Retorno.Editar_Empresa_Venda = false;
                    Retorno.Editar_Tipo_Midia = false;
                    Retorno.Editar_Abrangencia = false;
                    if (Retorno.Comerciais.Count>0)
                    {
                        Retorno.Editar_Midia_OnLine = false;
                    }

                    if (Retorno.Tem_Fatura)
                    {
                        Retorno.Editar_Cliente = false;
                        Retorno.Editar_Agencia= false;
                        Retorno.Editar_Empresa_Faturamento = false;
                        Retorno.Editar_Midia_Apoio = false;
                        Retorno.Editar_Conta_Credito= false;
                        Retorno.Editar_Valor_Informado = false;
                    }
                    if (Retorno.Caracteristica_Contrato == "MER")
                    {
                        Retorno.Editar_Caracteristica_Contrato = false;
                    }
                }
                else
                {
                    Retorno.Comerciais = new List<MapaReserva.ComercialModel>();
                    Retorno.Veiculacoes = new List<MapaReserva.VeiculacacaoModel>();
                    Retorno.VeiculacoesOnLine = new List<MapaReserva.VeiculacaoOnLineModel>();
                    Retorno.Veiculos = new List<MapaReserva.VeiculoModel>();
                    Retorno.Editar_Negociacao = true;
                    Retorno.Editar_Cliente = true;
                    Retorno.Editar_Agencia = true;
                    Retorno.Editar_Contato = true;
                    Retorno.Editar_Nucleo = true;
                    Retorno.Editar_Empresa_Venda = true;
                    Retorno.Editar_Empresa_Faturamento = true;
                    Retorno.Editar_Tipo_Midia = true;
                    Retorno.Editar_Mercado = true;
                    Retorno.Editar_Abrangencia = true;
                    Retorno.Editar_Periodo_Campanha = true;
                    Retorno.Editar_Valor_Informado = true;
                    Retorno.Editar_Abrangencia = true;
                    Retorno.Indica_Grade = -1;
                }
                return Ok(Retorno);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }

        [Route("api/MapaReserva/GetTerceirosNegociacao")]
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

        [Route("api/MapaReserva/Salvar")]
        [HttpPost]
        [ActionName("MapaReservaSalvar")]
        [Authorize()]
        public IHttpActionResult MapaReservaSalvar([FromBody] MapaReserva.ContratoModel param)
        {
            SimLib clsLib = new SimLib();
            MapaReserva Cls = new MapaReserva(User.Identity.Name);
            try
            {
                DataTable Retorno = Cls.MapaReservaSalvar(param);
                return Ok(Retorno);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }
        [Route("api/MapaReserva/NewMida")]
        [HttpPost]
        [ActionName("MapaReservaNewMida")]
        [Authorize()]
        public IHttpActionResult MapaReservaNewMidia([FromBody] MapaReserva.ParamNewMidiaModel param)
        {
            SimLib clsLib = new SimLib();
            MapaReserva Cls = new MapaReserva(User.Identity.Name);
            try
            {
                if (param.Indica_Midia_Online)
                {
                    MapaReserva.VeiculacaoOnLineModel Veiculacao = new MapaReserva.VeiculacaoOnLineModel();
                    return Ok(Veiculacao);
                }
                else
                {
                    MapaReserva.VeiculacacaoModel Veiculacao = new MapaReserva.VeiculacacaoModel();
                    Veiculacao.Permite_Editar = true;
                    Veiculacao.Insercoes = Cls.MapaReservaNewMidia(param);
                    return Ok(Veiculacao);
                }
                
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }

        [Route("api/MapaReserva/ValidarPeriodo")]
        [HttpPost]
        [ActionName("MapaReservaValidarPeriodo")]
        [Authorize()]
        public IHttpActionResult MapaReservaValidarPeriodo([FromBody] MapaReserva.ContratoModel param)
        {
            SimLib clsLib = new SimLib();
            MapaReserva Cls = new MapaReserva(User.Identity.Name);
            try
            {
                String Retorno = Cls.MapaReservaValidarPeriodo(param);
                return Ok(Retorno);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }
        [Route("api/MapaReserva/ValidarGradePeriodo")]
        [HttpPost]
        [ActionName("ValidarGradePeriodo")]
        [Authorize()]
        public IHttpActionResult ValidarGradePeriodo([FromBody] MapaReserva.ParamNewMidiaModel param)
        {
            SimLib clsLib = new SimLib();
            MapaReserva Cls = new MapaReserva(User.Identity.Name);
            try
            {
               DataTable Retorno = Cls.ValidarGradePeriodo(param);
                return Ok(Retorno);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }


        [Route("api/MapaReserva/GetNovaSequencia")]
        [HttpPost]
        [ActionName("MapaReservaGetNovaSequencia")]
        [Authorize()]
        public IHttpActionResult MapaReservaGetNovaSequencia(MapaReserva.MapaReservaFiltroModel Param)
        {
            SimLib clsLib = new SimLib();
            MapaReserva Cls = new MapaReserva(User.Identity.Name);
            try
            {
                Int32 Id_Contrato = Cls.GetIdContrato(Param);
                MapaReserva.ContratoModel Retorno = new MapaReserva.ContratoModel();
                Retorno = Cls.MapaReservaGetContrato(Id_Contrato);
                Retorno.Tem_Fatura = false;
                Retorno.Comprovado = false;
                Retorno.Veiculacoes = new List<MapaReserva.VeiculacacaoModel>();
                Retorno.VeiculacoesOnLine = new List<MapaReserva.VeiculacaoOnLineModel>();
                Retorno.Editar_Negociacao = false;
                Retorno.Editar_Empresa_Venda = false;
                Retorno.Editar_Tipo_Midia = false;
                Retorno.Editar_Abrangencia = true;
                Retorno.Editar_Periodo_Campanha = true;
                Retorno.Periodo_Campanha_Inicio = "";
                Retorno.Periodo_Campanha_Termino = "";
                Retorno.Id_Contrato = 0;
                return Ok(Retorno);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }

        [Route("api/MapaReserva/DeParaContato")]
        [HttpPost]
        [ActionName("DeParaContato")]
        [Authorize()]
        public IHttpActionResult DeParaContato([FromBody] MapaReserva.DeParaContatoModel param)
        {
            SimLib clsLib = new SimLib();
            MapaReserva Cls = new MapaReserva(User.Identity.Name);
            try
            {
                DataTable Retorno = Cls.DeParaContato(param);
                return Ok(Retorno);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }

        [Route("api/MapaReserva/GetContratoDeParaNegociacao")]
        [HttpPost]
        [ActionName("DeParaContato")]
        [Authorize()]
        public IHttpActionResult GetContratoDeParaNegociacao([FromBody] MapaReserva.DeParaNegociacaooModel param)
        {
            SimLib clsLib = new SimLib();
            MapaReserva Cls = new MapaReserva(User.Identity.Name);
            try
            {
                MapaReserva.DeParaNegociacaooModel Retorno = Cls.GetContratoDeParaNegociacao(param);
                return Ok(Retorno);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }

        [Route("api/MapaReserva/ProcessaDeParaNegociacao")]
        [HttpPost]
        [ActionName("DeParaContato")]
        [Authorize()]
        public IHttpActionResult ProcessaDeParaNegociacao([FromBody] MapaReserva.DeParaNegociacaooModel param)
        {
            SimLib clsLib = new SimLib();
            MapaReserva Cls = new MapaReserva(User.Identity.Name);
            try
            {
                DataTable Retorno = Cls.ProcessaDeParaNegociacao(param);
                return Ok(Retorno);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }

        [Route("api/MapaReserva/GetImportarDados")]
        [HttpPost]
        [ActionName("MapaReservaGetImportarDados")]
        [Authorize()]
        public IHttpActionResult MapaReservaGetImportarDados(MapaReserva.MapaReservaFiltroModel Param)
        {
            SimLib clsLib = new SimLib();
            MapaReserva Cls = new MapaReserva(User.Identity.Name);
            try
            {
                Int32 Id_Contrato = Cls.GetIdContrato(Param);
                MapaReserva.ContratoModel Retorno = new MapaReserva.ContratoModel();
                Retorno = Cls.MapaReservaGetContrato(Id_Contrato);
                Retorno.Tem_Fatura = false;
                Retorno.Comprovado = false;
                Retorno.Id_Contrato = 0;
                Retorno.Numero_Mr = 0;
                Retorno.Sequencia_Mr = 0;
                Retorno.Veiculacoes = Cls.AddVeiculacoes(Id_Contrato,Param.Operacao);
                Retorno.VeiculacoesOnLine = new List<MapaReserva.VeiculacaoOnLineModel>();
                return Ok(Retorno);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }

        [Route("api/MapaReserva/BaixarVeiculacao")]
        [HttpPost]
        [ActionName("MapaBaixarVeiculacao")]
        [Authorize()]
        public IHttpActionResult MapaBaixarVeiculacao(MapaReserva.BaixaVeiculacaoModel Param)
        {
            SimLib clsLib = new SimLib();
            MapaReserva Cls = new MapaReserva(User.Identity.Name);
            try
            {
                MapaReserva.BaixaVeiculacaoModel retorno = Cls.MapaBaixarVeiculacao(Param);
                return Ok(retorno);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }
        [Route("api/MapaReserva/AlterarCV")]
        [HttpPost]
        [ActionName("AlterarCV")]
        [Authorize()]
        public IHttpActionResult AlterarCV([FromBody] List<MapaReserva.AlterarCVModel> Param)
        {
            SimLib clsLib = new SimLib();
            MapaReserva Cls = new MapaReserva(User.Identity.Name);
            try
            {
                List<MapaReserva.AlterarCVModel> retorno = Cls.AlterarCV(Param);
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

