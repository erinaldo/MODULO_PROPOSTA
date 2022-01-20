
using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Http;
using System.Web.Http;

namespace PROPOSTA
{
    public class CredentialController : ApiController
    {

        [Route("api/Credential/{pRouteId}")]
        [HttpGet]
        [ActionName("Credential")]
        [Authorize()]
        //public IHttpActionResult Permissao([FromBody] apiCredential.ParamCredential Param)
        public IHttpActionResult Permissao(String pRouteId)
        {
            apiCredential Cls = new apiCredential(User.Identity.Name);
            SimLib clsLib = new SimLib();
            Boolean retorno = false;
            try
            {
                retorno = Cls.Permissao(pRouteId);
                return Ok(retorno);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }
        }

        [Route("api/GetUserData")]
        [HttpGet]
        [ActionName("GetUserData")]
        [Authorize()]
        public IHttpActionResult GetUserData()
        {
            apiCredential Cls = new apiCredential(User.Identity.Name);
            SimLib clsLib = new SimLib();
            try
            {
                apiCredential.UserDataModel Retorno = Cls.GetUserData();
                //DataTable dtbRetorno = Cls.GetUserData();
                return Ok(Retorno);
            }
            catch (Exception Ex)
            {
                clsLib.EmailErrorToSuporte(User.Identity.Name, Ex.Message.ToString(), Ex.Source, Ex.StackTrace);
                throw new Exception(Ex.Message);
            }

        }

        [Route("api/NewPassword")]
        [HttpPost]
        [ActionName("NewPassword")]
        //[Authorize()]
        public IHttpActionResult NewPassword([FromBody] apiCredential.RememberPassword Param)
        {

            apiCredential Cls = new apiCredential(User.Identity.Name);
            try
            {
                String Retorno = Cls.EsqueceuSenha(Param);

                return Ok(Retorno);
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
        }

        [Route("api/SetPassword")]
        [HttpPost]
        [ActionName("SetPassword")]
        //[Authorize()]
        public IHttpActionResult SetPassword([FromBody] apiCredential.RememberPassword Param)
        {

            apiCredential Cls = new apiCredential(User.Identity.Name);
            try
            {
                String Retorno = Cls.AlterarSenha(Param);

                return Ok(Retorno);
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
        }

        [Route("api/credential/checklogin")]
        [HttpPost]
        [ActionName("checklogin")]
        //[Authorize()]
        public IHttpActionResult checklogin([FromBody] apiCredential.CheckLoginModel Param)
        {

            apiCredential Cls = new apiCredential(User.Identity.Name);
            try
            {
                DataTable Retorno = Cls.CheckLogin(Param);

                return Ok(Retorno);
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
        }

        [Route("api/credential/GetToken")]
        [HttpPost]
        [ActionName("GetToken")]
        //[Authorize()]
        public IHttpActionResult GetToken([FromBody] apiCredential.CheckLoginModel Param)
        {
            apiCredential Cls = new apiCredential(User.Identity.Name);
            try
            {
                DataTable Retorno = Cls.GetToken(Param);

                return Ok(Retorno);
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
        }

        [Route("api/credential/AppGetToken/{Login}")]
        [HttpGet]
        [ActionName("AppGetToken")]
        [Authorize()]
        public IHttpActionResult AppGetToken(String Login)
        {
            apiCredential Cls = new apiCredential(User.Identity.Name);
            try
            {
                DataTable Retorno = Cls.AppGetToken(Login);
                apiCredential.AppToken Token = new apiCredential.AppToken();
                if (Retorno.Rows.Count>0)
                {
                    Token.Login = Login;
                    Token.Email = Retorno.Rows[0]["Email"].ToString();
                    Token.Token= Retorno.Rows[0]["Token"].ToString();
                }
                return Ok(Token);
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
        }
        
        [Route("api/credential/GetUserModulos")]
        [HttpGet]
        [ActionName("GetUserModulos")]
        [Authorize()]
        public IHttpActionResult GetUserModulos()
        {
            apiCredential Cls = new apiCredential(User.Identity.Name);
            try
            {
                DataTable Retorno = Cls.GetUserModulos();
                return Ok(Retorno);
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
        }

        [Route("api/credential/GetUserMenu")]
        [HttpGet]
        [ActionName("GetUserMenu")]
        [Authorize()]
        public IHttpActionResult GetUserMenu()
        {
            apiCredential Cls = new apiCredential(User.Identity.Name);
            try
            {
                List<apiCredential.MenuModel> Retorno = Cls.GetUserMenu();
                return Ok(Retorno);
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
        }
    }
}
   