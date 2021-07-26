using System;
using System.Collections.Generic;

namespace PROPOSTA
{
    public partial class GrupoUsuario
    {
        private String Credential;
        private String CurrentUser;
        private SimLib clsLib = new SimLib();
        public GrupoUsuario(String pCredential)
        {
            this.Credential = pCredential;
            this.CurrentUser = clsLib.Decriptografa(clsLib.GetJsonItem(this.Credential, "Name"));
        }

        public class GrupoUsuarioModel
        {
            public Int32 Id_Grupo{ get; set; }
            public String Cod_Grupo{ get; set; }
            public String Descricao{ get; set; }
            public Boolean Status { get; set; }
            public String Descricao_Status { get; set; }
            public List<PerfilModel> Perfil { get; set; }
            public List<UsuarioModel> Usuarios { get; set; }
        }
        public class PerfilModel
        {
            public Int32 Id_Funcao { get; set; }
            public Int32 Id_Funcao_Root { get; set; }
            public String Descricao_Funcao { get; set; }
            public String Path { get; set; }
            public Boolean Selected { get; set; }
            public Int32 Nivel { get; set; }
        }

        public class UsuarioModel
        {
            public Int32 Id_Usuario { get; set; }
            public String Login { get; set; }
            public String Nome { get; set; }
            public Boolean Selected{ get; set; }
        }
    }
}