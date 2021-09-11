using System;
using System.Collections.Generic;

namespace PROPOSTA
{
    public partial class Portador
    {
        private String Credential;
        private String CurrentUser;
        private SimLib clsLib = new SimLib();
        public Portador(String pCredential)
        {
            this.Credential = pCredential;
            this.CurrentUser = clsLib.Decriptografa(clsLib.GetJsonItem(this.Credential, "Name"));
        }

        public class PortadorModel
        {
            public string Id_Operacao { get; set; }
            public Int32  Id_Portador { get; set; }
            public String Cod_Portador { get; set; }
            public String Descricao_Portador { get; set; }
          
        }



    }
}