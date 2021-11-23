using System;
using System.Collections.Generic;

namespace PROPOSTA
{
    public partial class apiCredential
    {
        private String Credential;
        private String CurrentUser;
        private SimLib clsLib = new SimLib();
        public apiCredential(String pUser)
        {
            this.Credential = pUser;
            this.CurrentUser = clsLib.Decriptografa(clsLib.GetJsonItem(this.Credential, "Name"));
        }
        public apiCredential()
        {
        }

        public class ParamCredential
        {

            public Int32 RouteId { get; set; }
            public String UserName { get; set; }
            public String Password { get; set; }
        }
        public class RememberPassword
        {
            public String Email;
            public String Login;
            public Boolean EsqueceuLogin;
            public String Url;
            public String Senha;
            public String Token;
        }
        public class CheckLoginModel
        {
            public String login { get; set; }
            public String password { get; set; }
        }

        public class AppToken
        {
            public String Login { get; set; }
            public String Email { get; set; }
            public String Token { get; set; }
        }

        public class UserDataModel
        {
            public String Login { get; set; }
            public String Nome { get; set; }
            public String Email { get; set; }
            public Int32 Usuario_Id { get; set; }
            public String Cod_Empresa { get; set; }
            public String Nome_Empresa { get; set; }
            public String Register { get; set; }
            public List<UserModulosModel> Modulos { get; set; }
        }
        public class UserModulosModel
        {
            public Int32 Id_Modulo { get; set; }
            public String Descricao { get; set; }
            public String Chave { get; set; }
        }
    }
}