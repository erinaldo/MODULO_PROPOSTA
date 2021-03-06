using System;
using System.Web.Http;
using System.Data;
using System.Collections.Generic;
using System.Globalization;

namespace PROPOSTA
{
    public class GradeController : ApiController
    {
        [Route("api/Grade/List")]
        [HttpGet]
        [ActionName("GradeList")]
        [Authorize()]
        public IHttpActionResult GradeList([FromUri]Grade.GradeFiltroModel Filtro)
        {
            SimLib clsLib = new SimLib();
            Grade Cls = new Grade(User.Identity.Name);
            try
            {
                Grade.GradeListModel Retorno = Cls.GradeList(Filtro);
                return Ok(Retorno);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }
        [Route("api/Grade/GetData")]
        [HttpGet]
        [ActionName("GetData")]
        [Authorize()]
        public IHttpActionResult GetData([FromUri]Grade.GradeGetDataModel param)
        {
            SimLib clsLib = new SimLib();
            Grade Cls = new Grade(User.Identity.Name);
            try
            {
                Grade.GradeModel Retorno = new Grade.GradeModel();
                if (param.Action=="Edit")
                {
                    Retorno = Cls.GradeGetData(param);
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
        [Route("api/Grade/GetProgramas")]
        [HttpGet]
        [ActionName("GetProgramas")]
        [Authorize()]
        public IHttpActionResult GetProgramas([FromUri]Grade.GradeFiltroModel param)
        {
            SimLib clsLib = new SimLib();
            Grade Cls = new Grade(User.Identity.Name);
            try
            {
                DataTable Retorno = Cls.GradeGetProgramas(param);
                return Ok(Retorno);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }
        [Route("api/Grade/GetUltimoDiaGrade")]
        [HttpGet]
        [ActionName("GetUltimoDiaGrade")]
        [Authorize()]
        public IHttpActionResult GetUltioDiaGrade([FromUri]Grade.GradeGetDataModel param)
        {
            SimLib clsLib = new SimLib();
            Grade Cls = new Grade(User.Identity.Name);
            try
            {
                String  Retorno = Cls.GetUltimoDiaGrade(param);
                return Ok(Retorno);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }
        [Route("api/Grade/GetVeiculosRede/{RedeId}")]
        [HttpGet]
        [ActionName("GetVeiculosRede")]
        [Authorize()]
        public IHttpActionResult GetVeiculosRede(Int32 RedeId)
        {
            SimLib clsLib = new SimLib();
            Grade Cls = new Grade(User.Identity.Name);
            try
            {
             
                List<Grade.VeiculoModel> Veiculos = Cls.GetVeiculosRede(RedeId);
                return Ok(Veiculos);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }
        [Route("api/Grade/Salvar")]
        [HttpPost]
        [ActionName("Salvar")]
        [Authorize()]
        public IHttpActionResult Salvar([FromBody]Grade.GradeModel param)
        {
            SimLib clsLib = new SimLib();
            Grade Cls = new Grade(User.Identity.Name);
            try
            {
                DataTable dtb = Cls.SalvarGrade(param);
                return Ok(dtb);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }
        [Route("api/Grade/Excluir")]
        [HttpPost]
        [ActionName("Excluir")]
        [Authorize()]
        public IHttpActionResult Excluir([FromBody]Grade.GradeModel param)
        {
            SimLib clsLib = new SimLib();
            Grade Cls = new Grade(User.Identity.Name);
            try
            {
                DataTable dtb = Cls.ExcluirGrade(param);
                return Ok(dtb);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }
        [Route("api/Grade/Desativar")]
        [HttpPost]
        [ActionName("Desativar")]
        [Authorize()]
        public IHttpActionResult Desativar([FromBody]Grade.GradeModel param)
        {
            SimLib clsLib = new SimLib();
            Grade Cls = new Grade(User.Identity.Name);
            try
            {
                DataTable dtb = Cls.DesativarGrade(param,"D");
                return Ok(dtb);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }
        [Route("api/Grade/Reativar")]
        [HttpPost]
        [ActionName("Reativar")]
        [Authorize()]
        public IHttpActionResult Reativar([FromBody]Grade.GradeModel param)
        {
            SimLib clsLib = new SimLib();
            Grade Cls = new Grade(User.Identity.Name);
            try
            {
                DataTable dtb = Cls.DesativarGrade(param, "R");
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
        [Route("api/Grade/CarregaParametrosPropagacao")]
        [HttpGet]
        [ActionName("CarregaParametrosPropagacao")]
        [Authorize()]
        public IHttpActionResult CarregaParametrosPropagacao()
        {
            SimLib clsLib = new SimLib();
            Grade Cls = new Grade(User.Identity.Name);
            try
            {
                Grade.PropagacaoGradeModel GradePropagacao = new Grade.PropagacaoGradeModel();    //instancia o pai de todos
                // aqui agora vou chamar o sql do Veiculo e do programa e ja preencher a lista la
                // entao primeiro declaro uma variavel do tipo lista e vai ser preenchida com o retorno do que volta la do CarregaVeiculo
                // entao o cls.carregarVeiculo tem que voltar uma lista de veiculos
                List<Grade.ListarVeiculoModel> Veiculos = Cls.CarregaVeiculo();
                List<Grade.ListarProgramaModel> Programas = Cls.CarregaPrograma();
                GradePropagacao.Veiculos = Veiculos;
                GradePropagacao.Programas = Programas;
                return Ok(GradePropagacao);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }
        //--Faz a propagação da Grade
        [Route("api/Grade/SalvarPropagacaoGrade")]
        [HttpPost]
        [ActionName("SalvarPropagacaoGrade")]
        [Authorize()]
        public IHttpActionResult SalvarPropagacaoGrade([FromBody] Grade.PropagacaoGradeModel Grade)
        {
            SimLib clsLib = new SimLib();
            Grade Cls = new Grade(User.Identity.Name);
            try
            {
                Boolean retorno = Cls.SalvarPropagacaoGrade(Grade);
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

