using System;
using System.Web.Http;
using System.Data;
using System.Collections.Generic;

namespace PROPOSTA
{
    public class GrupoUsuarioController : ApiController
    {
        [Route("api/GrupoUsuarioListar")]
        [HttpGet]
        [ActionName("GrupoUsuarioListar")]
        [Authorize()]
        public IHttpActionResult GrupoUsuarioListar()
        {
            SimLib clsLib = new SimLib();
            GrupoUsuario Cls = new GrupoUsuario(User.Identity.Name);
            try
            {
                DataTable dtb = Cls.GrupoUsuarioListar(0);
                return Ok(dtb);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }

        [Route("api/GetGrupoUsuario/{pIdGrupoUsuario}")]
        [HttpGet]
        [ActionName("GetGrupoUsuario")]
        [Authorize()]
        public IHttpActionResult GetGrupoUsuario(Int32 pIdGrupoUsuario)
        {
            SimLib clsLib = new SimLib();
            GrupoUsuario Cls = new GrupoUsuario(User.Identity.Name);
            try
            {
                GrupoUsuario.GrupoUsuarioModel GrupoUsuario = new GrupoUsuario.GrupoUsuarioModel();
                if (pIdGrupoUsuario==0)
                {
                    GrupoUsuario.Perfil = Cls.addPerfil(0);
                    GrupoUsuario.Usuarios = Cls.addUsuarios(0);

                }
                else
                {
                    GrupoUsuario = Cls.GetGrupoUsuario(pIdGrupoUsuario);
                }
                
                return Ok(GrupoUsuario);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }

        [Route("api/SalvarGrupoUsuario")]
        [HttpPost]
        [ActionName("SalvarGrupoUsuario")]
        [Authorize()]
        
        public IHttpActionResult SalvarGrupoUsuario([FromBody] GrupoUsuario.GrupoUsuarioModel GrupoUsuario )
        {
            SimLib clsLib = new SimLib();
            GrupoUsuario Cls = new GrupoUsuario(User.Identity.Name);
            try
            {
                DataTable retorno = Cls.SalvarGrupoUsuario(GrupoUsuario);
                return Ok(retorno);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }

        [Route("api/DesativarReativarGrupoUsurio")]
        [HttpPost]
        [ActionName("DesativarReativarGrupoUsurio")]
        [Authorize()]
        public IHttpActionResult DesativarReativarGrupoUsurio([FromBody] GrupoUsuario.GrupoUsuarioModel GrupoUsuario)
        {
            SimLib clsLib = new SimLib();
            GrupoUsuario Cls = new GrupoUsuario(User.Identity.Name);
            try
            {
                Cls.DesativarReativar(GrupoUsuario);
                return Ok(true);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }

        [Route("api/ExcluirGrupoUsuario")]
        [HttpPost]
        [ActionName("ExcluirGrupoUsuario")]
        [Authorize()]
        public IHttpActionResult ExcluirGrupoUsuario([FromBody] GrupoUsuario.GrupoUsuarioModel GrupoUsuario)
        {
            SimLib clsLib = new SimLib();
            GrupoUsuario Cls = new GrupoUsuario(User.Identity.Name);
            try
            {
                Cls.ExcluirGrupoUsuario(GrupoUsuario);
                return Ok(true);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }

    }

}

