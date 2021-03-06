using System;
using System.Web.Http;
using System.Data;
using System.Collections.Generic;

namespace PROPOSTA
{
    public class UsuarioController : ApiController
    {
        [Route("api/UsuarioListar")]
        [HttpGet]
        [ActionName("UsuarioListar")]
        [Authorize()]
        public IHttpActionResult UsuarioListar()
        {
            SimLib clsLib = new SimLib();
            Usuario Cls = new Usuario(User.Identity.Name);
            try
            {
                DataTable dtb = Cls.UsuarioListar(0);
                return Ok(dtb);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }

        [Route("api/GetUsuario/{pIdUsuario}")]
        [HttpGet]
        [ActionName("GetUsuario")]
        [Authorize()]
        public IHttpActionResult GetUsuario(Int32 pIdUsuario)
        {
            SimLib clsLib = new SimLib();
            Usuario Cls = new Usuario(User.Identity.Name);
            try
            {
                Usuario.UsuarioModel Usuario = new Usuario.UsuarioModel();
                if (pIdUsuario == 0)
                {
                    Usuario.Nivel_Superior = new List<Usuario.HierarquiaModel>();
                    Usuario.Nivel_Inferior = new List<Usuario.HierarquiaModel>();
                    Usuario.Perfil = Cls.addPerfil(0);
                    Usuario.Empresas = Cls.addEmpresas(0);
                    Usuario.Grupos = Cls.addGrupos(0);
                    Usuario.Modulos = Cls.addModulos(0);

                }
                else
                {
                    Usuario = Cls.GetUsuario(pIdUsuario);
                }

                return Ok(Usuario);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }

        [Route("api/SalvarUsuario")]
        [HttpPost]
        [ActionName("SalvarUsuario")]
        [Authorize()]

        public IHttpActionResult SalvarUsuario([FromBody] Usuario.UsuarioModel Usuario)
        {
            SimLib clsLib = new SimLib();
            Usuario Cls = new Usuario(User.Identity.Name);
            try
            {
                DataTable retorno = Cls.SalvarUsuario(Usuario);
                return Ok(retorno);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }

        [Route("api/DesativarReativar")]
        [HttpPost]
        [ActionName("DesativarReativar")]
        [Authorize()]
        public IHttpActionResult DesativarReativar([FromBody] Usuario.UsuarioModel Usuario)
        {
            SimLib clsLib = new SimLib();
            Usuario Cls = new Usuario(User.Identity.Name);
            try
            {
                Cls.DesativarReativar(Usuario);
                return Ok(true);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }

        [Route("api/ExcluirUsuario")]
        [HttpPost]
        [ActionName("ExcluirUsuario")]
        [Authorize()]
        public IHttpActionResult ExcluirUsuario([FromBody] Usuario.UsuarioModel Usuario)
        {
            SimLib clsLib = new SimLib();
            Usuario Cls = new Usuario(User.Identity.Name);
            try
            {
                Cls.ExcluirUsuario(Usuario);
                return Ok(true);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }


        [Route("api/Usuario/ImportarPerfil/{Login}")]
        [HttpGet]
        [ActionName("ImportarPerfil")]
        [Authorize()]
        public IHttpActionResult GetIdUsuario(String Login)
        {
            SimLib clsLib = new SimLib();
            Usuario Cls = new Usuario(User.Identity.Name);
            try
            {
                Int32 Id_Usuario = Cls.GetIdUsuario(Login);
                Usuario.UsuarioModel Usuario = new Usuario.UsuarioModel();
                Usuario.Perfil = Cls.addPerfil(Id_Usuario);
                Usuario.Modulos = Cls.addModulos(Id_Usuario);
                return Ok(Usuario);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }
    }

}